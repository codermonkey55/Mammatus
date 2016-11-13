using System;
using System.Linq.Expressions;

namespace Mammatus.Domain.Specification
{
    /// <summary>
    /// A logic AND Specification
    /// </summary>
    /// <typeparam name="T">Type of entity that check this specification</typeparam>
    public class AndAlsoSpecification<T> : CompositeSpecification<T>
        where T : class
    {
        private readonly ISpecification<T> leftSideSpecification;
        private readonly ISpecification<T> rightSideSpecification;

        /// <summary>
        /// Default constructor for AndSpecification
        /// </summary>
        /// <param name="leftSide">Left side specification</param>
        /// <param name="rightSide">Right side specification</param>
        public AndAlsoSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
        {
            if (leftSide == null)
            {
                throw new ArgumentNullException(nameof(leftSide));
            }

            if (rightSide == null)
            {
                throw new ArgumentNullException(nameof(rightSide));
            }

            this.leftSideSpecification = leftSide;
            this.rightSideSpecification = rightSide;
        }

        /// <summary>
        /// Left side specification
        /// </summary>
        public override ISpecification<T> LeftSideSpecification
        {
            get
            {
                return this.leftSideSpecification;
            }
        }

        /// <summary>
        /// Right side specification
        /// </summary>
        public override ISpecification<T> RightSideSpecification
        {
            get
            {
                return this.rightSideSpecification;
            }
        }

        /// <summary>
        /// <see cref="ISpecification{TEntity}"/>
        /// </summary>
        /// <returns><see cref="ISpecification{TEntity}"/></returns>
        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            Expression<Func<T, bool>> left = this.leftSideSpecification.SatisfiedBy();
            Expression<Func<T, bool>> right = this.rightSideSpecification.SatisfiedBy();

            return left.AndAlso(right);
        }
    }
}