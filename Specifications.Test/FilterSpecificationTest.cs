using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;

using DevCon2011.Specifications.Filters;

using Moq;

using NUnit.Framework;

namespace DevCon2011.Specifications.Test
{
    [TestFixture]
    public class FilterSpecificationTest
    {
        class FilterTestEntity
        {
            public int IntField { get; set; }
            public string StringField { get; set; }
            public int? NullableIntField { get; set; }
            public IEnumerable<int> CollectionField { get; set; }
        }


        [Test]
        public void SholdSupportEqualOperator()
        {
            // Given
            const int value = 10;
            var entity = new FilterTestEntity { IntField = value };
            var specification = new FilterSpecification<FilterTestEntity>(f => f.And(c => c.Equal(i => i.IntField, value)));

            // When
            var result = specification.IsSatisfiedBy(entity);

            // Then
            Assert.IsTrue(result);
        }

        [Test]
        public void SholdSupportNotEqualOperator()
        {
            // Given
            const int value = 10;
            var entity = new FilterTestEntity { IntField = value + 1 };
            var specification = new FilterSpecification<FilterTestEntity>(f => f.And(c => c.NotEqual(i => i.IntField, value)));

            // When
            var result = specification.IsSatisfiedBy(entity);

            // Then
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldSupportNullOperator()
        {
            // Given
            var entity = new FilterTestEntity { StringField = null };
            var specification = new FilterSpecification<FilterTestEntity>(f => f.And(c => c.Null(i => i.StringField)));

            // When
            var result = specification.IsSatisfiedBy(entity);

            // Then
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldSupportNotNullOperator()
        {
            // Given
            var entity = new FilterTestEntity { StringField = "NotNull" };
            var specification = new FilterSpecification<FilterTestEntity>(f => f.And(c => c.NotNull(i => i.StringField)));

            // When
            var result = specification.IsSatisfiedBy(entity);

            // Then
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldSupportGreaterOperator([Range(0, 5)] int value)
        {
            // Given
            var entity = new FilterTestEntity { IntField = 3 };
            var specification = new FilterSpecification<FilterTestEntity>(f => f.And(c => c.Greater(i => i.IntField, value)));

            // When
            var result = specification.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(entity.IntField > value, result);
        }

        [Test]
        public void ShouldSupportGreaterOrEqualOperator([Range(0, 5)] int value)
        {
            // Given
            var entity = new FilterTestEntity { IntField = 3 };
            var specification = new FilterSpecification<FilterTestEntity>(f => f.And(c => c.GreaterOrEqual(i => i.IntField, value)));

            // When
            var result = specification.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(entity.IntField >= value, result);
        }

        [Test]
        public void ShouldSupportLessOperator([Range(0, 5)] int value)
        {
            // Given
            var entity = new FilterTestEntity { IntField = 3 };
            var specification = new FilterSpecification<FilterTestEntity>(f => f.And(c => c.Less(i => i.IntField, value)));

            // When
            var result = specification.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(entity.IntField < value, result);
        }

        [Test]
        public void ShouldSupportLessOrEqualOperator([Range(0, 5)] int value)
        {
            // Given
            var entity = new FilterTestEntity { IntField = 3 };
            var specification = new FilterSpecification<FilterTestEntity>(f => f.And(c => c.LessOrEqual(i => i.IntField, value)));

            // When
            var result = specification.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(entity.IntField <= value, result);
        }

        [Test]
        public void ShouldSupportContainsOperatorForString()
        {
            // Given
            var entity = new FilterTestEntity { StringField = "12345" };
            var specification = new FilterSpecification<FilterTestEntity>(f => f.And(c => c.Contains(i => i.StringField, "234")));

            // When
            var result = specification.IsSatisfiedBy(entity);

            // Then
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldSupportContainsOperatorForCollection([Range(0, 5)] int value)
        {
            // Given
            var entity = new FilterTestEntity { CollectionField = new[] { 1, 2, 3 } };
            var specification = new FilterSpecification<FilterTestEntity>(f => f.And(c => c.Contains(i => i.CollectionField, value)));

            // When
            var result = specification.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(entity.CollectionField.Contains(value), result);
        }

        [Test]
        public void ShouldSupportNotContainsOperatorForString()
        {
            // Given
            var entity = new FilterTestEntity { StringField = "54321" };
            var specification = new FilterSpecification<FilterTestEntity>(f => f.And(c => c.NotContains(i => i.StringField, "234")));

            // When
            var result = specification.IsSatisfiedBy(entity);

            // Then
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldSupportNotContainsOperatorForCollection([Range(0, 5)] int value)
        {
            // Given
            var entity = new FilterTestEntity { CollectionField = new[] { 1, 2, 3 } };
            var specification = new FilterSpecification<FilterTestEntity>(f => f.And(c => c.NotContains(i => i.CollectionField, value)));

            // When
            var result = specification.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(!entity.CollectionField.Contains(value), result);
        }

        [Test]
        public void ShouldSupportStartsWithOperator()
        {
            // Given
            var entity = new FilterTestEntity { StringField = "12345" };
            var specification = new FilterSpecification<FilterTestEntity>(f => f.And(c => c.StartsWith(i => i.StringField, "12")));

            // When
            var result = specification.IsSatisfiedBy(entity);

            // Then
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldSupportEndsWithOperator()
        {
            // Given
            var entity = new FilterTestEntity { StringField = "12345" };
            var specification = new FilterSpecification<FilterTestEntity>(f => f.And(c => c.EndsWith(i => i.StringField, "45")));

            // When
            var result = specification.IsSatisfiedBy(entity);

            // Then
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldSupportInOperator([Range(0, 5)] int value)
        {
            // Given
            var set = new[] { 1, 2, 3 };
            var entity = new FilterTestEntity { IntField = value };
            var specification = new FilterSpecification<FilterTestEntity>(f => f.And(c => c.In(i => i.IntField, set)));

            // When
            var result = specification.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(set.Contains(value), result);
        }

        [Test]
        public void ShouldSupportNotInOperator([Range(0, 5)] int value)
        {
            // Given
            var set = new[] { 1, 2, 3 };
            var entity = new FilterTestEntity { IntField = value };
            var specification = new FilterSpecification<FilterTestEntity>(f => f.And(c => c.NotIn(i => i.IntField, set)));

            // When
            var result = specification.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(!set.Contains(value), result);
        }

        [Test]
        public void ShouldSupportBetweenOperator([Range(0, 5)] int value)
        {
            // Given
            var entity = new FilterTestEntity { IntField = value };
            var specification = new FilterSpecification<FilterTestEntity>(f => f.And(c => c.Between(i => i.IntField, 1, 4)));

            // When
            var result = specification.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(entity.IntField >= 1 && entity.IntField <= 4, result);
        }

        [Test]
        public void ShouldSupportNotBetweenOperator([Range(0, 5)] int value)
        {
            // Given
            var entity = new FilterTestEntity { IntField = value };
            var specification = new FilterSpecification<FilterTestEntity>(f => f.And(c => c.NotBetween(i => i.IntField, 1, 4)));

            // When
            var result = specification.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(!(entity.IntField >= 1 && entity.IntField <= 4), result);
        }

        [Test]
        public void ShouldSupportComplexConditions([Range(0, 3)] int intField, [Range(0, 3)] int stringField)
        {
            // Given
            var entity = new FilterTestEntity { IntField = intField, StringField = stringField.ToString() };

            var specification = new FilterSpecification<FilterTestEntity>(
                f => f.And(c => c
                                    .NotNull(i => i.StringField)
                                    .In(i => i.IntField, new[] { 1, 2 })
                                    .Or(or1 => or1.Equal(i => i.IntField, 1).StartsWith(i => i.StringField, "1"))
                                    .Or(or2 => or2.Equal(i => i.IntField, 2).StartsWith(i => i.StringField, "2"))));

            // When
            var result = specification.IsSatisfiedBy(entity);

            // Then
            Assert.AreEqual(entity.StringField != null
                            && new[] { 1, 2 }.Contains(entity.IntField)
                            && (entity.IntField == 1 || entity.StringField == "1")
                            && (entity.IntField == 2 || entity.StringField == "2")
                            , result);
        }

        [Test]
        public void ShouldSupportNullable()
        {
            // Given
            var entity = new FilterTestEntity { NullableIntField = 1 };

            var specification = new FilterSpecification<FilterTestEntity>(
                builder => builder.Or(condition => condition
                                                       .Equal(i => i.NullableIntField, 10)
                                                       .NotEqual(i => i.NullableIntField, 1)
                                                       .Greater(i => i.NullableIntField, 10)
                                                       .GreaterOrEqual(i => i.NullableIntField, 10)
                                                       .Less(i => i.NullableIntField, 0)
                                                       .LessOrEqual(i => i.NullableIntField, 0)
                                                       .In(i => i.NullableIntField, new int?[] { 0, 2, 4 })
                                                       .NotIn(i => i.NullableIntField, new int?[] { 1, 3, 5 })
                                                       .Between(i => i.NullableIntField, 5, 10)
                                                       .NotBetween(i => i.NullableIntField, 0, 5)));

            // When
            var result = specification.IsSatisfiedBy(entity);

            // Then
            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldBuildOperatorDuringTheCall()
        {
            // Given
            var entity = new FilterTestEntity();

            var operatorMock = new Mock<IFilterOperator>();
            operatorMock.Setup(o => o.CreateFilter()).Returns((Expression<Func<FilterTestEntity, bool>>)(item => true));

            var specification = new FilterSpecification<FilterTestEntity>(b => operatorMock.Object);

            // When
            specification.IsSatisfiedBy(entity);
            specification.IsSatisfiedBy(entity);
            specification.IsSatisfiedBy(entity);

            // Then
            operatorMock.Verify(o => o.CreateFilter(), Times.Exactly(3));
        }

        [Test]
        public void SholdSerializeOperator()
        {
            // Given
            var entity = new FilterTestEntity { StringField = "Abc", IntField = 5 };

            // Какое-то сложное логическое выражение
            var specification = new FilterSpecification<FilterTestEntity>(
                b => b.Or(
                    c => c
                             .And(a => a.Or(o => o
                                                     .Null(i => i.StringField)
                                                     .Equal(i => i.StringField, string.Empty))
                                           .Equal(i => i.IntField, 0))
                             .And(a => a
                                           .NotNull(i => i.StringField)
                                           .NotEqual(i => i.StringField, string.Empty)
                                           .Or(o => o
                                                        .In(i => i.StringField.ToLower(), new[] { "a", "b", "c" })
                                                        .Contains(i => i.StringField.ToLower(), "abc")
                                                        .StartsWith(i => i.StringField, "1")
                                                        .EndsWith(i => i.StringField, "5"))
                                           .Or(o => o
                                                        .And(a2 => a2
                                                                       .GreaterOrEqual(i => i.IntField, 0)
                                                                       .LessOrEqual(i => i.IntField, 10))
                                                        .And(a2 => a2
                                                                       .Greater(i => i.IntField, 10)
                                                                       .Less(i => i.IntField, 20))
                                                        .Between(i => i.IntField, 20, 30)))));

            IFilterOperator operatorFromStream;
            IFilterOperator operatorToStream = specification.Operator;

            // When
            using (var stream = new MemoryStream())
            {
                // Сериализация
                var serializer = new NetDataContractSerializer();
                serializer.Serialize(stream, operatorToStream);
                stream.Position = 0;

                // Десериализация
                operatorFromStream = (IFilterOperator)serializer.ReadObject(stream);
            }

            // Then
            Assert.IsNotNull(operatorFromStream);
            Assert.AreEqual(operatorToStream.IsSatisfiedBy(entity), operatorFromStream.IsSatisfiedBy(entity));
        }
    }
}