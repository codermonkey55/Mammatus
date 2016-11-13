using System;
using System.Linq.Expressions;

namespace Mammatus.Domain.Specification
{
    /// <summary>
    /// A Direct Specification is a simple implementation
    /// of specification that acquire this from a lambda expression
    /// in  constructor
    /// </summary>
    /// <typeparam name="TEntity">Type of entity that check this specification</typeparam>
    public class DirectSpecification<TEntity> : Specification<TEntity>
        where TEntity : class
    {
        private readonly Expression<Func<TEntity, bool>> matchingCriteria;

        /// <summary>
        /// Default constructor for Direct Specification
        /// </summary>
        /// <param name="matchingCriteria">A Matching Criteria</param>
        public DirectSpecification(Expression<Func<TEntity, bool>> matchingCriteria)
        {
            if (matchingCriteria == null)
            {
                throw new ArgumentNullException(nameof(matchingCriteria));
            }

            this.matchingCriteria = matchingCriteria;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override Expression<Func<TEntity, bool>> SatisfiedBy()
        {
            return this.matchingCriteria;
        }
    }
}