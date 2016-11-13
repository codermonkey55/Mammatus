using System;
using System.Linq.Expressions;

namespace Mammatus.Domain.Specification
{
    /// <summary>
    /// True specification
    /// </summary>
    /// <typeparam name="TEntity">Type of entity in this specification</typeparam>
    public class TrueSpecification<TEntity> : Specification<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// <see cref=" Specification{TEntity}"/>
        /// </summary>
        /// <returns><see cref=" Specification{TEntity}"/></returns>
        public override Expression<Func<TEntity, bool>> SatisfiedBy()
        {
            Expression<Func<TEntity, bool>> trueExpression = t => true;
            return trueExpression;
        }
    }
}