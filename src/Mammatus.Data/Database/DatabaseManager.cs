using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Mammatus.Data.Database
{
    using System;

    public class DatabaseManager
    {
        //private static readonly ILogger log = LoggerManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly DbProviderFactory connectionProvider;
        private readonly string connectionString;
        private readonly string providerName;

        public DatabaseManager(string provider, string connectionString)
        {
            this.providerName = provider;
            this.connectionString = connectionString;
            this.connectionProvider = DbProviderFactories.GetFactory(providerName);
        }

        public DbProviderFactory DbProviderFactory
        {
            get
            {
                return this.connectionProvider;
            }
        }

        public void CreateDatabase()
        {
            InternalCreateDatabase(this.connectionProvider, this.connectionString, this.providerName);
        }

        public bool DatabaseExists()
        {
            return InternalDatabaseExists(this.connectionProvider, this.connectionString, this.providerName);
        }

        public void DropDatabase()
        {
            this.InternalDropDatabase(this.connectionProvider, this.connectionString, this.providerName);
        }

        private static void InternalClearAllPools(string providerName)
        {
            if (providerName == DbProvider.MsSqlProvider)
            {
                SqlConnection.ClearAllPools();
            }

            if (providerName == DbProvider.PostgreSQLProvider)
            {
                Type type = Type.GetType("Npgsql.NpgsqlConnection, Npgsql", true);
                MethodInfo method = type.GetMethod("ClearAllPools", BindingFlags.Static | BindingFlags.Public);
                method.Invoke(null, null);
            }

            if (providerName == DbProvider.Firebird)
            {
                Type type = Type.GetType(
                                "FirebirdSql.Data.FirebirdClient.FbConnection, FirebirdSql.Data.FirebirdClient", true);
                MethodInfo method = type.GetMethod("ClearAllPools", BindingFlags.Static | BindingFlags.Public);
                method.Invoke(null, null);
            }
        }

        private static void InternalCreateDatabase(DbProviderFactory provider, string connectionString, string providerName)
        {
            string dbName, dbFile;
            string connStr = InternalStripDbName(connectionString, providerName, out dbName, out dbFile);
            var command = new StringBuilder(); // Build SQL Command..

            if (providerName == DbProvider.SQLiteProvider)
            {
                // Do nothing..
                return;
            }

            if (providerName == DbProvider.SqlCe)
            {
                if (File.Exists(dbName))
                {
                    File.Delete(dbName);
                }

                Type type = Type.GetType("System.Data.SqlServerCe.SqlCeEngine, System.Data.SqlServerCe");
                PropertyInfo localConnectionString = type.GetProperty("LocalConnectionString");
                MethodInfo createDatabase = type.GetMethod("CreateDatabase");

                object engine = Activator.CreateInstance(type);
                localConnectionString.SetValue(engine, string.Format("Data Source='{0}';", dbName), null);

                createDatabase.Invoke(engine, new object[0]);

                return;
            }

            if (providerName == DbProvider.Firebird)
            {
                if (File.Exists(dbName))
                {
                    File.Delete(dbName);
                }

                Type type = Type.GetType("FirebirdSql.Data.FirebirdClient.FbConnection, FirebirdSql.Data.FirebirdClient");
                MethodInfo createDatabase = type.GetMethod(
                                                "CreateDatabase",
                                                new[]
                {
                    typeof(string), typeof(int), typeof(bool),
                    typeof(bool)
                });

                object engine = Activator.CreateInstance(type);
                createDatabase.Invoke(engine, new object[] { connectionString, 8192, true, false });

                return;
            }
            else if (providerName == DbProvider.OracleDataProvider)
            {
                throw new NotImplementedException();
            }
            else if (providerName == DbProvider.PostgreSQLProvider)
            {
                command.AppendFormat(CultureInfo.InvariantCulture, "CREATE DATABASE \"{0}\" WITH ENCODING = 'UTF8'", dbName);
            }
            else if (providerName == DbProvider.MsSqlProvider)
            {
                command.AppendFormat(CultureInfo.InvariantCulture, "CREATE DATABASE [{0}] ", dbName);

                // Handle MSSQL AttachDBFile..
                if (providerName == DbProvider.MsSqlProvider && !string.IsNullOrEmpty(dbFile))
                {
                    string fname = Path.GetFileNameWithoutExtension(dbFile);
                    string pathname = Path.Combine(Path.GetDirectoryName(dbFile), fname);

                    command.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "ON PRIMARY (NAME = {0}, FILENAME = '{1}.mdf', SIZE = 10MB) " + "LOG ON (NAME = {0}_log, FILENAME = '{1}.ldf', SIZE = 2MB)",
                        fname,
                        pathname);
                }
            }

            //log.DebugFormat(CultureInfo.InvariantCulture, "Creating Database '{0}..", dbName);
            provider.ExecuteNonQuery(connStr, command.ToString());
            //log.InfoFormat(CultureInfo.InvariantCulture, "Database instance '{0}' created!", dbName);
        }

        private static bool InternalDatabaseExists(DbProviderFactory provider, string connectionString, string providerName)
        {
            string dbName, cmdText = null;
            string connStr = InternalStripDbName(connectionString, providerName, out dbName);

            try
            {
                //log.DebugFormat(
                //    CultureInfo.InvariantCulture,
                //    "Checking if database '{0}' exists, with provider: {1}, and connectionString: {2}",
                //    dbName,
                //    providerName,
                //    connStr);

                if (providerName == DbProvider.SQLiteProvider)
                {
                    if (dbName.ToUpperInvariant() == ":MEMORY:")
                    {
                        return false;
                    }
                    else
                    {
                        return File.Exists(dbName);
                    }
                }

                if (providerName == DbProvider.SqlCe || providerName == DbProvider.Firebird)
                {
                    return File.Exists(dbName);
                }

                switch (providerName)
                {
                    case DbProvider.MsSqlProvider:
                    cmdText = string.Format(CultureInfo.InvariantCulture, "select COUNT(*) from sys.sysdatabases where name=\'{0}\'", dbName);
                    break;
                    case DbProvider.MySqlProvider:
                    cmdText = string.Format(
                                  CultureInfo.InvariantCulture,
                                  @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{0}'",
                                  dbName);
                    break;
                    case DbProvider.OracleDataProvider:
                    cmdText = "SELECT 1 FROM DUAL";
                    break;
                    case DbProvider.PostgreSQLProvider:
                    cmdText = string.Format(
                                  CultureInfo.InvariantCulture,
                                  "select count(*) from pg_catalog.pg_database where datname = '{0}'",
                                  dbName);
                    break;
                default:
                    throw new NotSupportedException(string.Format(
                                                        CultureInfo.InvariantCulture,
                                                        "Provider {0} is not supported",
                                                        providerName));
                }

                object ret = provider.ExecuteScalar(connStr, cmdText);
                int count = ret == null ? 0 : Convert.ToInt32(ret, CultureInfo.InvariantCulture);

                if (count > 0)
                {
                    return true;
                }
            }
            catch (NotSupportedException)
            {
                throw;
            }
            catch (Exception)
            {
                //log.Error("Database connection failed", ex);
            }

            //log.DebugFormat(CultureInfo.InvariantCulture, "Database '{0}' does not exists", dbName);
            return false;
        }

        private static string InternalStripDbName(string connectionString, string providerName, out string dbName, out string dbFile)
        {
            var builder = new DbConnectionStringBuilder
            {
                ConnectionString = connectionString
            };
            string dbname = null, dbfile = null;
            object tmp;

            // SQLServer.. minimal option..
            if (builder.TryGetValue("Initial Catalog", out tmp))
            {
                dbname = tmp.ToString();
                builder.Remove("Initial Catalog");
            }

            // SQLServer default option..
            if (builder.TryGetValue("Database", out tmp))
            {
                dbname = tmp.ToString();
                builder.Remove("Database");
            }

            // SQLite! (XXX: MsSql has 'Data Source' as a means to specify Server address)
            if ((providerName == DbProvider.SQLiteProvider || providerName == DbProvider.SqlCe) && builder.TryGetValue("Data Source", out tmp))
            {
                dbname = tmp.ToString();
                builder.Remove("Data Source");
            }

            // SQLServer (auto attach alternate)
            if (builder.TryGetValue("AttachDBFileName", out tmp))
            {
                dbfile = tmp.ToString();

                // Replace |DataDirectory| in connection string.
                dbfile = dbfile.Replace("|DataDirectory|", AppDomain.CurrentDomain.GetData("DataDirectory") as string);
                builder.Remove("AttachDBFileName");
            }

            // Oracle SID
            if (providerName == DbProvider.OracleDataProvider && builder.TryGetValue("Data Source", out tmp))
            {
                string connStr = tmp.ToString().Replace(" ", "").Replace("\r", "").Replace("\n", "");
                Match match = Regex.Match(connStr, @"SERVICE_NAME=([^\)]+)");

                if (match.Success)
                {
                    dbname = match.Groups[1].Value;
                }

                // Try EZ-Connect method..
                if (string.IsNullOrEmpty(dbname))
                {
                    match = Regex.Match(connStr, ".*/([^$]*)$");

                    if (match.Success)
                    {
                        dbname = match.Groups[1].Value;
                    }
                }
            }

            // If no database is specified at connStr, throw error..
            if (string.IsNullOrEmpty(dbname) && string.IsNullOrEmpty(dbfile))
            {
                throw new ArgumentException("ConnectionString should specify a database name or file");
            }

            // If not catalog nor database name passed, try to obtain it from db file path.
            if (string.IsNullOrEmpty(dbname))
            {
                dbname = dbfile;
            }

            // Save return values..
            dbName = dbname;
            dbFile = dbfile;

            return builder.ToString();
        }

        private static string InternalStripDbName(string connectionString, string providerName, out string dbName)
        {
            string name, file;
            string ret = InternalStripDbName(connectionString, providerName, out name, out file);

            dbName = !string.IsNullOrEmpty(name) ? name : file;

            return ret;
        }

        private void InternalDropDatabase(DbProviderFactory provider, string connectionString, string providerName)
        {
            string dbName, dbFile;
            string connStr = InternalStripDbName(connectionString, providerName, out dbName, out dbFile);

            // XXX: Maybe calling this.DatabaseExists prior to deletion would allow for a cleaner error.

            if (providerName == DbProvider.OracleDataProvider)
            {
                throw new NotImplementedException();
            }
            else if (providerName == DbProvider.SQLiteProvider)
            {
                if (dbName.ToUpperInvariant() != ":MEMORY:")
                {
                    File.Delete(dbName);
                }
            }
            else if (providerName == DbProvider.SqlCe)
            {
                File.Delete(dbName);
            }
            else if (providerName == DbProvider.Firebird)
            {
                InternalClearAllPools(providerName);
                File.Delete(dbName);
            }
            else if (providerName == DbProvider.MsSqlProvider)
            {
                InternalClearAllPools(providerName);

                string cmd = string.Format(
                                 CultureInfo.InvariantCulture,
                                 "USE master; ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;",
                                 dbName);

                this.connectionProvider.ExecuteNonQuery(connStr, cmd);
                this.connectionProvider.ExecuteNonQuery(connStr, string.Format(CultureInfo.InvariantCulture, "DROP DATABASE [{0}]", dbName));
            }
            else if (providerName == DbProvider.PostgreSQLProvider)
            {
                this.connectionProvider.ExecuteNonQuery(connStr, string.Format(CultureInfo.InvariantCulture, "DROP DATABASE \"{0}\"", dbName));
            }
            else
            {
                this.connectionProvider.ExecuteNonQuery(connStr, string.Format(CultureInfo.InvariantCulture, "DROP DATABASE '{0}'", dbName));
            }
        }
    }
}