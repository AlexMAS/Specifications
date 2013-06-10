﻿using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace DevCon2011.Specifications.Filters.ConditionalOperators
{
    /// <summary>
    /// Оператор "БОЛЬШЕ ИЛИ РАВНО"
    /// </summary>
    [DataContract]
    public class GreaterOrEqualOperator : BinaryOperator
    {
        public GreaterOrEqualOperator(string typeName, string propertyName, object value)
            : base(typeName, propertyName, value)
        {
        }

        public GreaterOrEqualOperator(string typeName, LambdaExpression propertyExpression, object value)
            : base(typeName, propertyExpression, value)
        {
        }

        protected override Expression CreateFilter(Expression property)
        {
            // property >= Value
            return Expression.GreaterThanOrEqual(property, CreateConstant(property.Type, Value));
        }
    }
}