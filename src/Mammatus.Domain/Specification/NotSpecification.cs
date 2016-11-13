using System;
using System.Linq;
using System.Linq.Expressions;

namespace Mammatus.Domain.Specification
{
    /// <summary>
    /// NotEspecification convert a original
    /// specification with NOT logic operator
    /// </summary>
    /// <typeparam name="TEntity">Type of element for this specificaiton</typeparam>
    public class NotSpecification<TEntity> : Specification<TEntity>
        where TEntity : class
    {
        private readonly Expression<Func<TEntity, bool>> originalCriteria;

        /// <summary>
        /// Constructor for NotSpecificaiton
        /// </summary>
        /// <param name="originalSpecification">Original specification</param>
        public NotSpecification(ISpecification<TEntity> originalSpecification)
        {
            if (originalSpecification == null)
            {
                throw new ArgumentNullException(nameof(originalSpecification));
            }

            this.originalCriteria = originalSpecification.SatisfiedBy();
        }

        /// <summary>
        /// Constructor for NotSpecification
        /// </summary>
        /// <param name="originalSpecification">Original specificaiton</param>
        public NotSpecification(Expression<Func<TEntity, bool>> originalSpecification)
        {
            if (originalSpecification == null)
            {
                throw new ArgumentNullException(nameof(originalSpecification));
            }

            this.originalCriteria = originalSpecification;
        }

        /// <summary>
        /// <see cref="ISpecification{TEntity}"/>
        /// </summary>
        /// <returns><see cref="ISpecification{TEntity}"/></returns>
        public override Expression<Func<TEntity, bool>> SatisfiedBy()
        {
            return Expression.Lambda<Func<TEntity, bool>>(
                Expression.Not(this.originalCriteria.Body),
                this.originalCriteria.Parameters.Single());
        }
    }
}