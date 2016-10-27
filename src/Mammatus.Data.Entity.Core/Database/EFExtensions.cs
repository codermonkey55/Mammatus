﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Mammatus.Data.Entity.Core.Extensions
{
    public static class EFExtensions
    {
        public static TEntity Find<TEntity>(this DbContext context, params object[] keyValues)
            where TEntity : class
        {
            var set = context.Set<TEntity>();
            //var context = ((IAccessor<IServiceProvider>)set).Service.GetService<DbContext>();

            var entityType = context.Model.GetEntityTypes().Where(et => et.Name == typeof(TEntity).Name).FirstOrDefault();
            var key = entityType.FindPrimaryKey();

            var entries = context.ChangeTracker.Entries<TEntity>();

            var i = 0;
            foreach (var property in key.Properties)
            {
                var keyValue = keyValues[i];
                entries = entries.Where(e => e.Property(property.Name).CurrentValue == keyValue);
                i++;
            }

            var entry = entries.FirstOrDefault();
            if (entry != null)
            {
                // Return the local object if it exists.
                return entry.Entity;
            }

            // TODO: Build the real LINQ Expression
            // set.Where(x => x.Id == keyValues[0]);
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var query = set.Where((Expression<Func<TEntity, bool>>)
                Expression.Lambda(
                    Expression.Equal(
                        Expression.Property(parameter, "Id"),
                        Expression.Constant(keyValues[0])),
                    parameter));

            // Look in the database
            return query.FirstOrDefault();
        }
    }
}
