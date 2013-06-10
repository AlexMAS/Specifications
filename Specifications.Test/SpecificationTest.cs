using System;
using System.Linq;
using System.Linq.Expressions;

using NUnit.Framework;

namespace DevCon2011.Specifications.Test
{
    [TestFixture]
    public class SpecificationTest
    {
        class OddNumber : Specification<int>
        {
            protected override Expression<Func<int, bool>> Predicate
            {
                get { return number => (number % 2) != 0; }
            }
        }

        class EvenNumber : Specification<int>
        {
            protected override Expression<Func<int, bool>> Predicate
            {
                get { return number => (number % 2) == 0; }
            }
        }

        class NegativeNumber : Specification<int>
        {
            protected override Expression<Func<int, bool>> Predicate
            {
                get { return numeber => numeber < 0; }
            }
        }

        public readonly int[] Numbers = new[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5 };

        [Test]
        public void ShouldSelectOddNumbers()
        {
            // When
            var result = Numbers.Where(new OddNumber().IsSatisfiedBy);

            // Then
            CollectionAssert.AreEquivalent(new[] { -5, -3, -1, 1, 3, 5 }, result);
        }

        [Test]
        public void ShouldSelectEvenNumbers()
        {
            // When
            var result = Numbers.Where(new EvenNumber().IsSatisfiedBy);

            // Then
            CollectionAssert.AreEquivalent(new[] { -4, -2, 0, 2, 4 }, result);
        }

        [Test]
        public void ShouldSelectNegativeNumbers()
        {
            // When
            var result = Numbers.Where(new NegativeNumber().IsSatisfiedBy);

            // Then
            CollectionAssert.AreEquivalent(new[] { -5, -4, -3, -2, -1 }, result);
        }

        [Test]
        public void ShouldSelectPositiveNumbers()
        {
            // When
            var result = Numbers.Where((!new NegativeNumber()).IsSatisfiedBy);

            // Then
            CollectionAssert.AreEquivalent(new[] { 0, 1, 2, 3, 4, 5 }, result);
        }

        [Test]
        public void ShouldSelectOddOrEvenNumbers()
        {
            // When
            var result = Numbers.Where((new OddNumber() | new EvenNumber()).IsSatisfiedBy);

            // Then
            CollectionAssert.AreEquivalent(Numbers, result);
        }

        [Test]
        public void ShouldSelectNegativeOddNumbers()
        {
            // When
            var result = Numbers.Where((new NegativeNumber() & new OddNumber()).IsSatisfiedBy);

            // Then
            CollectionAssert.AreEquivalent(new[] { -5, -3, -1 }, result);
        }

        [Test]
        public void ShouldSelectPositiveOddNumbers()
        {
            // When
            var result = Numbers.Where((!new NegativeNumber() & new OddNumber()).IsSatisfiedBy);

            // Then
            CollectionAssert.AreEquivalent(new[] { 1, 3, 5 }, result);
        }

        [Test]
        public void ShouldSelectNegativeEvenNumbers()
        {
            // When
            var result = Numbers.Where((new NegativeNumber() & new EvenNumber()).IsSatisfiedBy);

            // Then
            CollectionAssert.AreEquivalent(new[] { -4, -2 }, result);
        }

        [Test]
        public void ShouldSelectPositiveEvenNumbers()
        {
            // When
            var result = Numbers.Where((!new NegativeNumber() & new EvenNumber()).IsSatisfiedBy);

            // Then
            CollectionAssert.AreEquivalent(new[] { 0, 2, 4 }, result);
        }

        [Test]
        public void ShouldSelectEmpty()
        {
            // When
            var result = Numbers.Where((!new NegativeNumber() & new NegativeNumber()).IsSatisfiedBy);

            // Then
            CollectionAssert.AreEquivalent(new int[] { }, result);
        }

        [Test]
        public void ShouldSelectAllNumbers()
        {
            // When
            var result = Numbers.Where((!new NegativeNumber() | new NegativeNumber()).IsSatisfiedBy);

            // Then
            CollectionAssert.AreEquivalent(Numbers, result);
        }
    }
}