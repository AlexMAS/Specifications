using System;
using System.Linq.Expressions;

namespace DevCon2011.Specifications
{
    /// <summary>
    /// Базовый класс спецификации
    /// </summary>
    /// <typeparam name="T">Тип объекта, для которого применяется спецификация</typeparam>
    public abstract class Specification<T>
    {
        /// <summary>
        /// Удовлетворяет ли объект спецификации
        /// </summary>
        /// <param name="item">Проверяемый объект</param>
        public bool IsSatisfiedBy(T item)
        {
            return Predicate.Compile()(item);
        }

        /// <summary>
        /// Предикат для проверки спецификации
        /// </summary>
        protected abstract Expression<Func<T, bool>> Predicate { get; }


        public static Specification<T> operator !(Specification<T> specification)
        {
            return new NotSpecification<T>(specification);
        }

        public static Specification<T> operator |(Specification<T> left, Specification<T> right)
        {
            return new OrSpecification<T>(left, right);
        }

        public static Specification<T> operator &(Specification<T> left, Specification<T> right)
        {
            return new AndSpecification<T>(left, right);
        }


        public static implicit operator Predicate<T>(Specification<T> specification)
        {
            return specification.IsSatisfiedBy;
        }

        public static implicit operator Func<T, bool>(Specification<T> specification)
        {
            return specification.IsSatisfiedBy;
        }

        public static implicit operator Expression<Func<T, bool>>(Specification<T> specification)
        {
            return specification.Predicate;
        }
    }
}