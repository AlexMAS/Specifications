using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace DevCon2011.Specifications.Filters.ConditionalOperators
{
    /// <summary>
    /// Оператор "СОДЕРЖИТ"
    /// </summary>
    [DataContract]
    public class ContainsOperator : BinaryOperator
    {
        public ContainsOperator(string typeName, string propertyName, object value)
            : base(typeName, propertyName, value)
        {
        }

        public ContainsOperator(string typeName, LambdaExpression propertyExpression, object value)
            : base(typeName, propertyExpression, value)
        {
        }

        protected override Expression CreateFilter(Expression property)
        {
            // property.Contains(Value)

            Expression result = null;

            var propertyType = property.Type;
            var containsMethod = propertyType.GetMethod("Contains");

            if (containsMethod != null)
            {
                // Если тип свойства имеет метод Contains()
                result = Expression.Call(property, containsMethod, Expression.Constant(Value));
            }
            else if (typeof(IEnumerable).IsAssignableFrom(propertyType) && propertyType.IsGenericType)
            {
                // Поиск метода расширения Contains() для типа свойства
                containsMethod = typeof(Enumerable).GetMethods().Where(method => method.Name == "Contains")
                    .Select(method => new { method, parameters = method.GetParameters() })
                    .Where(m => m.parameters.Length == 2)
                    .Select(m => m.method)
                    .FirstOrDefault();

                if (containsMethod != null)
                {
                    containsMethod = containsMethod.MakeGenericMethod(propertyType.GetGenericArguments());
                    result = Expression.Call(containsMethod, property, Expression.Constant(Value));
                }
            }

            if (result == null)
            {
                throw new NotSupportedException();
            }

            return result;
        }
    }
}