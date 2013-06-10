﻿using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace DevCon2011.Specifications.Filters.ConditionalOperators
{
    /// <summary>
    /// Оператор "МЕНЬШЕ ИЛИ РАВНО"
    /// </summary>
    [DataContract]
    public class LessOrEqualOperator : BinaryOperator
    {
        public LessOrEqualOperator(string typeName, string propertyName, object value)
            : base(typeName, propertyName, value)
        {
        }

        public LessOrEqualOperator(string typeName, LambdaExpression propertyExpression, object value)
            : base(typeName, propertyExpression, value)
        {
        }

        protected override Expression CreateFilter(Expression property)
        {
            // property <= Value
            return Expression.LessThanOrEqual(property, CreateConstant(property.Type, Value));
        }
    }
}