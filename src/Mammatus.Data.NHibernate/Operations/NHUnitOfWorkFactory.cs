﻿using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Mammatus.Data.Database;
using Mammatus.Data.Enums;
using Mammatus.Data.NHibernate.SqlExceptionHandlers;
using Mammatus.Domain.UnitOfWork;
using Mammatus.Extensions;
using Mammatus.Validation;
using NHibernate;
using NHibernate.Bytecode;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Mammatus.Data.NHibernate.DbOperations
{
    using Environment = global::NHibernate.Cfg.Environment;

    public sealed class NHUnitOfWorkFactory : BaseUnitOfWorkFactory, IDatabaseManager
    {
        private static Configuration builtConfiguration;
        private static string connectionString;
        private static DbProvider _dbProvider;
        private static bool inMemoryDatabase;
        private static bool validationSupported = true;

        private ISessionFactory sessionFactory;

        public NHUnitOfWorkFactory(
            DbProvider provider,
            string connectionString,
            string cacheProvider,
            Assembly[] mappingAssemblies)
        {
            NHUnitOfWorkFactory._dbProvider = provider;
            NHUnitOfWorkFactory.connectionString = connectionString;

            FluentConfiguration cfg = null;

            switch (_dbProvider)
            {
                case DbProvider.MsSqlProvider:
                    {
                        cfg = Fluently.Configure().Database(MsSqlConfiguration.MsSql2008
                                                            .Raw("format_sql", "true")
                                                            .ConnectionString(connectionString))
                              .ExposeConfiguration(
                                  c =>
                                  c.Properties.Add(
                                      Environment.SqlExceptionConverter,
                                      typeof(SqlExceptionHandler).AssemblyQualifiedName))
                              .ExposeConfiguration(c => c.Properties.Add(Environment.DefaultSchema, "dbo"));

                        break;
                    }

                case DbProvider.SQLiteProvider:
                    {
                        cfg = Fluently.Configure().Database(SQLiteConfiguration.Standard
                                                            .Raw("format_sql", "true")
                                                            .ConnectionString(connectionString));

                        inMemoryDatabase = connectionString.ToUpperInvariant().Contains(":MEMORY:");

                        break;
                    }

                case DbProvider.SqlCe:
                    {
                        cfg = Fluently.Configure().Database(MsSqlCeConfiguration.Standard
                                                            .Raw("format_sql", "true")
                                                            .ConnectionString(connectionString))
                              .ExposeConfiguration(
                                  c =>
                                  c.Properties.Add(
                                      Environment.SqlExceptionConverter,
                                      typeof(SqlExceptionHandler).AssemblyQualifiedName));

                        validationSupported = false;

                        break;
                    }

                case DbProvider.Firebird:
                    {
                        cfg = Fluently.Configure().Database(new FirebirdConfiguration()
                                                            .Raw("format_sql", "true")
                                                            .ConnectionString(connectionString));

                        break;
                    }

                case DbProvider.PostgreSQLProvider:
                    {
                        cfg = Fluently.Configure().Database(PostgreSQLConfiguration.PostgreSQL82
                                                            .Raw("format_sql", "true")
                                                            .ConnectionString(connectionString));

                        validationSupported = false;

                        break;
                    }
            }

            Guard.IsNotNull(
                cfg, $"Db provider {_dbProvider.GetDescription()} is currently not supported.");

            PropertyInfo pinfo = typeof(FluentConfiguration)
                                 .GetProperty(
                                     "Configuration",
                                     BindingFlags.Instance | BindingFlags.NonPublic);

            Configuration nhConfiguration = pinfo.GetValue(cfg, null) as Configuration;

            //IoC.RegisterInstance<Configuration>(nhConfiguration);

            cfg.Mappings(m =>
            {
                m.FluentMappings.Conventions.AddAssembly(typeof(NHUnitOfWorkFactory).Assembly);
                foreach (Assembly mappingAssembly in mappingAssemblies)
                {
                    m.FluentMappings.Conventions.AddAssembly(mappingAssembly);
                }
            })
            .Mappings(m =>
            {
                m.FluentMappings.AddFromAssembly(typeof(NHUnitOfWorkFactory).Assembly);
                foreach (Assembly mappingAssembly in mappingAssemblies)
                {
                    m.FluentMappings.AddFromAssembly(mappingAssembly);
                }
            })
            .Mappings(m =>
            {
                m.HbmMappings.AddFromAssembly(typeof(NHUnitOfWorkFactory).Assembly);
                foreach (Assembly mappingAssembly in mappingAssemblies)
                {
                    m.HbmMappings.AddFromAssembly(mappingAssembly);
                }
            })
            .ExposeConfiguration(c => c.Properties.Add(Environment.BatchSize, "100"))
            .ExposeConfiguration(c => c.Properties.Add(Environment.UseProxyValidator, "true"));

            if (!string.IsNullOrEmpty(cacheProvider))
            {
                cfg.ExposeConfiguration(c => c.Properties.Add(Environment.CacheProvider, cacheProvider))
                .ExposeConfiguration(c => c.Properties.Add(Environment.UseSecondLevelCache, "true"))
                .ExposeConfiguration(c => c.Properties.Add(Environment.UseQueryCache, "true"));
            }

            NHUnitOfWorkFactory.builtConfiguration = cfg.BuildConfiguration();
            NHUnitOfWorkFactory.builtConfiguration.SetProperty(
                Environment.ProxyFactoryFactoryClass,
                typeof(DefaultProxyFactoryFactory).AssemblyQualifiedName);
        }

        internal NHUnitOfWorkFactory()
        {
        }

        public ISession CurrentSession
        {
            get
            {
                INHSessionUnitOfWork unitOfWork = this.Current as INHSessionUnitOfWork;
                Guard.IsNotNull(unitOfWork, "No UnitOfWork in scope!!!");

                return unitOfWork.Session;
            }
        }

        public ISessionFactory CreateSessionFactory()
        {
            if (this.sessionFactory == null)
            {
                this.sessionFactory = NHUnitOfWorkFactory.builtConfiguration.BuildSessionFactory();
            }

            return this.sessionFactory;
        }

        public void CreateDatabase()
        {
            var dbManager = new DatabaseManager(NHUnitOfWorkFactory._dbProvider, NHUnitOfWorkFactory.connectionString);

            if (!dbManager.DatabaseExists())
            {
                dbManager.CreateDatabase();
                new SchemaExport(NHUnitOfWorkFactory.builtConfiguration).Create(false, true);

                if (_dbProvider == DbProvider.MsSqlProvider)
                {
                    using (var conn = new SqlConnection(NHUnitOfWorkFactory.connectionString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "RENAME_UNIQUE_KEYS";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public bool DatabaseExists()
        {
            var dbManager = new DatabaseManager(NHUnitOfWorkFactory._dbProvider, NHUnitOfWorkFactory.connectionString);
            return dbManager.DatabaseExists();
        }

        public void DeleteDatabase()
        {
            var dbManager = new DatabaseManager(NHUnitOfWorkFactory._dbProvider, NHUnitOfWorkFactory.connectionString);

            if (dbManager.DatabaseExists())
            {
                dbManager.DropDatabase();
            }
        }

        public void ValidateDatabaseSchema()
        {
            if (!NHUnitOfWorkFactory.inMemoryDatabase && NHUnitOfWorkFactory.validationSupported)
            {
                new SchemaValidator(NHUnitOfWorkFactory.builtConfiguration).Validate();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        protected override INestableUnitOfWork InternalCreate(IUnitOfWork previousUnitOfWork)
        {
            ISession session = this.sessionFactory.OpenSession();
            session.Transaction.Begin();

            INHSessionUnitOfWork newUnitOfWork = new NHUnitOfWork(session, previousUnitOfWork, this);
            return newUnitOfWork;
        }
    }
}