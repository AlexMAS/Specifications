using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using DevCon2011.Specifications.Filters.ConditionalOperators;

using NUnit.Framework;

namespace DevCon2011.Specifications.Test
{
    [TestFixture]
    public class FilterOperatorTest
    {
        public class FilterTestEntity
        {
            public int Field;
            public int IntField { get; set; }
            public string StringField { get; set; }
        }


        public readonly string Field = "Field";
        public readonly string IntField = "IntField";
        public readonly string StringField = "StringField";
        public readonly string EntityType = typeof(FilterTestEntity).AssemblyQualifiedName;


        [Test]
        public void ShouldBeEqual()
        {
            // Given
            var filter = new EqualOperator(EntityType, StringField, "Equal");
            var satisfyEntity = new FilterTestEntity { StringField = "Equal" };
            var noSatisfyEntity = new FilterTestEntity { StringField = "NotEqual" };

            // When
            var isSatisfy = filter.IsSatisfiedBy(satisfyEntity);
            var isNoSatisfy = filter.IsSatisfiedBy(noSatisfyEntity);

            // Then
            Assert.IsTrue(isSatisfy);
            Assert.IsFalse(isNoSatisfy);
        }

        [Test]
        public void ShouldBeNotEqual()
        {
            // Given
            var filter = !new EqualOperator(EntityType, StringField, "Equal");
            var satisfyEntity = new FilterTestEntity { StringField = "NotEqual" };
            var noSatisfyEntity = new FilterTestEntity { StringField = "Equal" };

            // When
            var isSatisfy = filter.IsSatisfiedBy(satisfyEntity);
            var isNoSatisfy = filter.IsSatisfiedBy(noSatisfyEntity);

            // Then
            Assert.IsTrue(isSatisfy);
            Assert.IsFalse(isNoSatisfy);
        }

        [Test]
        public void ShouldBeNull()
        {
            // Given
            var filter = new EqualOperator(EntityType, StringField, null);
            var satisfyEntity = new FilterTestEntity { StringField = null };
            var noSatisfyEntity = new FilterTestEntity { StringField = "NotNull" };

            // When
            var isSatisfy = filter.IsSatisfiedBy(satisfyEntity);
            var isNoSatisfy = filter.IsSatisfiedBy(noSatisfyEntity);

            // Then
            Assert.IsTrue(isSatisfy);
            Assert.IsFalse(isNoSatisfy);
        }

        [Test]
        public void ShouldBeNotNull()
        {
            // Given
            var filter = !new EqualOperator(EntityType, StringField, null);
            var satisfyEntity = new FilterTestEntity { StringField = "NotNull" };
            var noSatisfyEntity = new FilterTestEntity { StringField = null };

            // When
            var isSatisfy = filter.IsSatisfiedBy(satisfyEntity);
            var isNoSatisfy = filter.IsSatisfiedBy(noSatisfyEntity);

            // Then
            Assert.IsTrue(isSatisfy);
            Assert.IsFalse(isNoSatisfy);
        }

        [Test]
        public void ShouldBeGreaterThanValue([Range(0, 5)] int value)
        {
            // Given
            var filter = new GreaterOperator(EntityType, IntField, value);
            var entity = new FilterTestEntity { IntField = 3 };

            // When
            var isSatisfy = filter.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(entity.IntField > value, isSatisfy);
        }

        [Test]
        public void ShouldBeNotGreaterThanValue([Range(0, 5)] int value)
        {
            // Given
            var filter = !new GreaterOperator(EntityType, IntField, value);
            var entity = new FilterTestEntity { IntField = 3 };

            // When
            var isSatisfy = filter.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(!(entity.IntField > value), isSatisfy);
        }

        [Test]
        public void ShouldBeGreaterThanOrEqualValue([Range(0, 5)] int value)
        {
            // Given
            var filter = new GreaterOrEqualOperator(EntityType, IntField, value);
            var entity = new FilterTestEntity { IntField = 3 };

            // When
            var isSatisfy = filter.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(entity.IntField >= value, isSatisfy);
        }

        [Test]
        public void ShouldBeNotGreaterThanOrEqualValue([Range(0, 5)] int value)
        {
            // Given
            var filter = !new GreaterOrEqualOperator(EntityType, IntField, value);
            var entity = new FilterTestEntity { IntField = 3 };

            // When
            var isSatisfy = filter.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(!(entity.IntField >= value), isSatisfy);
        }

        [Test]
        public void ShouldBeLessThanValue([Range(0, 5)] int value)
        {
            // Given
            var filter = new LessOperator(EntityType, IntField, value);
            var entity = new FilterTestEntity { IntField = 3 };

            // When
            var isSatisfy = filter.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(entity.IntField < value, isSatisfy);
        }

        [Test]
        public void ShouldBeNotLessThanValue([Range(0, 5)] int value)
        {
            // Given
            var filter = !new LessOperator(EntityType, IntField, value);
            var entity = new FilterTestEntity { IntField = 3 };

            // When
            var isSatisfy = filter.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(!(entity.IntField < value), isSatisfy);
        }

        [Test]
        public void ShouldBeLessThanOrEqualValue([Range(0, 5)] int value)
        {
            // Given
            var filter = new LessOrEqualOperator(EntityType, IntField, value);
            var entity = new FilterTestEntity { IntField = 3 };

            // When
            var isSatisfy = filter.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(entity.IntField <= value, isSatisfy);
        }

        [Test]
        public void ShouldBeNotLessThanOrEqual([Range(0, 5)] int value)
        {
            // Given
            var filter = !new LessOrEqualOperator(EntityType, IntField, value);
            var entity = new FilterTestEntity { IntField = 3 };

            // When
            var isSatisfy = filter.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(!(entity.IntField <= value), isSatisfy);
        }

        [Test]
        public void ShouldContains()
        {
            // Given
            var filter = new ContainsOperator(EntityType, StringField, "23");
            var satisfyEntity = new FilterTestEntity { StringField = "1234" };
            var noSatisfyEntity = new FilterTestEntity { StringField = "5678" };

            // When
            var isSatisfy = filter.IsSatisfiedBy(satisfyEntity);
            var isNoSatisfy = filter.IsSatisfiedBy(noSatisfyEntity);

            // Then
            Assert.IsTrue(isSatisfy);
            Assert.IsFalse(isNoSatisfy);
        }

        [Test]
        public void ShouldNotContains()
        {
            // Given
            var filter = !new ContainsOperator(EntityType, StringField, "23");
            var satisfyEntity = new FilterTestEntity { StringField = "5678" };
            var noSatisfyEntity = new FilterTestEntity { StringField = "1234" };

            // When
            var isSatisfy = filter.IsSatisfiedBy(satisfyEntity);
            var isNoSatisfy = filter.IsSatisfiedBy(noSatisfyEntity);

            // Then
            Assert.IsTrue(isSatisfy);
            Assert.IsFalse(isNoSatisfy);
        }

        [Test]
        public void ShouldStartsWith()
        {
            // Given
            var filter = new StartsWithOperator(EntityType, StringField, "12");
            var satisfyEntity = new FilterTestEntity { StringField = "1234" };
            var noSatisfyEntity = new FilterTestEntity { StringField = "5678" };

            // When
            var isSatisfy = filter.IsSatisfiedBy(satisfyEntity);
            var isNoSatisfy = filter.IsSatisfiedBy(noSatisfyEntity);

            // Then
            Assert.IsTrue(isSatisfy);
            Assert.IsFalse(isNoSatisfy);
        }

        [Test]
        public void ShouldNotStartsWith()
        {
            // Given
            var filter = !new StartsWithOperator(EntityType, StringField, "12");
            var satisfyEntity = new FilterTestEntity { StringField = "5678" };
            var noSatisfyEntity = new FilterTestEntity { StringField = "1234" };

            // When
            var isSatisfy = filter.IsSatisfiedBy(satisfyEntity);
            var isNoSatisfy = filter.IsSatisfiedBy(noSatisfyEntity);

            // Then
            Assert.IsTrue(isSatisfy);
            Assert.IsFalse(isNoSatisfy);
        }

        [Test]
        public void ShouldEndsWith()
        {
            // Given
            var filter = new EndsWithOperator(EntityType, StringField, "34");
            var satisfyEntity = new FilterTestEntity { StringField = "1234" };
            var noSatisfyEntity = new FilterTestEntity { StringField = "5678" };

            // When
            var isSatisfy = filter.IsSatisfiedBy(satisfyEntity);
            var isNoSatisfy = filter.IsSatisfiedBy(noSatisfyEntity);

            // Then
            Assert.IsTrue(isSatisfy);
            Assert.IsFalse(isNoSatisfy);
        }

        [Test]
        public void ShouldNotEndsWith()
        {
            // Given
            var filter = !new EndsWithOperator(EntityType, StringField, "34");
            var satisfyEntity = new FilterTestEntity { StringField = "5678" };
            var noSatisfyEntity = new FilterTestEntity { StringField = "1234" };

            // When
            var isSatisfy = filter.IsSatisfiedBy(satisfyEntity);
            var isNoSatisfy = filter.IsSatisfiedBy(noSatisfyEntity);

            // Then
            Assert.IsTrue(isSatisfy);
            Assert.IsFalse(isNoSatisfy);
        }

        [Test]
        public void ShouldBeValueInSet([Range(0, 5)] int value)
        {
            // Given
            var list = new[] { 0, 1, 2 };
            var filter = new InOperator(EntityType, IntField, list);
            var entity = new FilterTestEntity { IntField = value };

            // When
            var isSatisfy = filter.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(list.Contains(value), isSatisfy);
        }

        [Test]
        public void ShouldBeValueNotInSet([Range(0, 5)] int value)
        {
            // Given
            var list = new List<int> { 0, 1, 2 };
            var filter = !new InOperator(EntityType, IntField, list);
            var entity = new FilterTestEntity { IntField = value };

            // When
            var isSatisfy = filter.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(!list.Contains(value), isSatisfy);
        }

        [Test]
        public void ShouldBeValueBetweenTwoValues([Range(0, 5)] int value)
        {
            // Given
            var filter = new BetweenOperator(EntityType, IntField, 1, 4);
            var entity = new FilterTestEntity { IntField = value };

            // When
            var result = filter.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(value >= 1 && value <= 4, result);
        }

        [Test]
        public void ShouldBeValueNotBetweenTwoValues([Range(0, 5)] int value)
        {
            // Given
            var filter = !new BetweenOperator(EntityType, IntField, 1, 4);
            var entity = new FilterTestEntity { IntField = value };

            // When
            var result = filter.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(!(value >= 1 && value <= 4), result);
        }

        [Test]
        public void ShouldBeValueBetweenBeginValueAndInfinity([Range(0, 5)] int value)
        {
            // Given
            var filter = new BetweenOperator(EntityType, IntField, 3, null);
            var entity = new FilterTestEntity { IntField = value };

            // When
            var result = filter.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(value >= 3, result);
        }

        [Test]
        public void ShouldBeValueNotBetweenBeginValueAndInfinity([Range(0, 5)] int value)
        {
            // Given
            var filter = !new BetweenOperator(EntityType, IntField, 3, null);
            var entity = new FilterTestEntity { IntField = value };

            // When
            var result = filter.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(!(value >= 3), result);
        }

        [Test]
        public void ShouldBeValueBetweenInfinityAndEndValue([Range(0, 5)] int value)
        {
            // Given
            var filter = new BetweenOperator(EntityType, IntField, null, 3);
            var entity = new FilterTestEntity { IntField = value };

            // When
            var result = filter.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(value <= 3, result);
        }

        [Test]
        public void ShouldBeValueNotBetweenInfinityAndEndValue([Range(0, 5)] int value)
        {
            // Given
            var filter = !new BetweenOperator(EntityType, IntField, null, 3);
            var entity = new FilterTestEntity { IntField = value };

            // When
            var result = filter.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(!(value <= 3), result);
        }

        [Test]
        public void ShouldReturnTrueWhenRangeIsNotSpecify([Range(0, 5)] int value)
        {
            // Given
            var filter = new BetweenOperator(EntityType, IntField, null, null);
            var entity = new FilterTestEntity { IntField = value };

            // When
            var result = filter.IsSatisfiedBy(entity);

            // Then
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldBeSupportAndOperator()
        {
            // Given
            var entity = new FilterTestEntity { StringField = "12345" };

            var filter = new EndsWithOperator(EntityType, StringField, "45")
                         & new StartsWithOperator(EntityType, StringField, "12")
                         & new ContainsOperator(EntityType, StringField, "234");

            // When
            var isSatisfy = filter.IsSatisfiedBy(entity);

            // Then
            Assert.IsTrue(isSatisfy);
        }

        [Test]
        public void ShouldBeSupportOrOperator()
        {
            // Given
            var entity = new FilterTestEntity { StringField = "?234?" };

            var filter = new EndsWithOperator(EntityType, StringField, "45")
                         | new StartsWithOperator(EntityType, StringField, "12")
                         | new ContainsOperator(EntityType, StringField, "234");

            // When
            var isSatisfy = filter.IsSatisfiedBy(entity);

            // Then
            Assert.IsTrue(isSatisfy);
        }

        [Test]
        public void ShouldBeSupportCompositeFilters()
        {
            // Given
            var entity = new FilterTestEntity { StringField = "12345", IntField = 10 };

            var filter = !new EqualOperator(EntityType, StringField, "12345")
                         & new ContainsOperator(EntityType, StringField, "234")
                         | new GreaterOrEqualOperator(EntityType, IntField, 5);

            // When
            var isSatisfy = filter.IsSatisfiedBy(entity);

            // Then
            Assert.IsTrue(isSatisfy);
        }

        [Test]
        public void ShouldBeSupportLambdaExpressionInProperty()
        {
            // Given
            Expression<Func<FilterTestEntity, string>> propertyExpression = item => item.StringField.ToLower();

            var entity = new FilterTestEntity { StringField = "AbCd" };
            var filter = new EqualOperator(EntityType, propertyExpression, "abcd");

            // When
            var isSatisfy = filter.IsSatisfiedBy(entity);

            // Then
            Assert.IsTrue(isSatisfy);
        }

        [Test]
        public void ShouldBeSupportField()
        {
            // Given
            var entity = new FilterTestEntity { Field = 10 };
            var filter = new EqualOperator(EntityType, Field, 10);

            // When
            var isSatisfy = filter.IsSatisfiedBy(entity);

            // Then
            Assert.IsTrue(isSatisfy);
        }
    }
}