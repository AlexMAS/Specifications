using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace DevCon2011.Specifications.Filters.BooleanOperators
{
    /// <summary>
    /// Опетатор "НЕ"
    /// </summary>
    [DataContract]
    public class NotOperator : BooleanOperator
    {
        public NotOperator(string typeName, IFilterOperator filterOperator)
            : base(typeName, filterOperator)
        {
        }

        public override LambdaExpression CreateFilter()
        {
            var expression = Operators.First().CreateFilter();
            return Expression.Lambda(CreateFilterDelegateType(), Expression.Not(expression.Body), expression.Parameters);
        }
    }
}