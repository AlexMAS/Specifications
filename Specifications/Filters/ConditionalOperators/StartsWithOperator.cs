﻿using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace DevCon2011.Specifications.Filters.ConditionalOperators
{
    /// <summary>
    /// Оператор "НАЧИНАЕТСЯ С"
    /// </summary>
    [DataContract]
    public class StartsWithOperator : BinaryOperator
    {
        public StartsWithOperator(string typeName, string propertyName, string value)
            : base(typeName, propertyName, value)
        {
        }

        public StartsWithOperator(string typeName, LambdaExpression propertyExpression, string value)
            : base(typeName, propertyExpression, value)
        {
        }

        protected override Expression CreateFilter(Expression property)
        {
            // property.StartsWith(Value)
            return Expression.Call(property, property.Type.GetMethod("StartsWith", new[] { typeof(string) }), Expression.Constant(Value ?? string.Empty));
        }
    }
}