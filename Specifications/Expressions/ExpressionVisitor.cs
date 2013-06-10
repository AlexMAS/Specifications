using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace DevCon2011.Specifications.Expressions
{
    /// <summary>
    /// Базовый класс для рекурсивного обхода дерева выражений с целью просмотра или перезаписи
    /// </summary>
    /// <remarks>
    /// Этот класс предназначен для наследования, чтобы создавать более специализированные классы, функциональные 
    /// возможности которых нуждаются в обходе, просмотре или копировании дерева выражения. Код приведенный ниже,
    /// был взят с сайта MSDN (см. раздел How to: Implement an Expression Tree Visitor).
    /// 
    /// Реализация данного класса присутствует в .NET Framework 4.0 (см. System.Linq.Expressions.ExpressionVisitor),
    /// поэтому при переходе на новую платформу стоит сменить данную реализацию стандартной.
    /// </remarks>
    public abstract class ExpressionVisitor
    {
        /// <summary>
        /// Осуществить обход дерева заданного выражения
        /// </summary>
        /// <param name="expression">Выражение</param>
        /// <remarks>
        /// В этой реализации обхода дерева выражения метод Visit(), который должен быть вызван первым, направляет переданное выражение 
        /// в один или несколько специализированных методов обхода в классе, основываясь на типе выражения. Специализированные методы обхода 
        /// служат для обхода поддерева выражения, которому они были переданы. Если вложенное выражение меняется после обхода, например,
        /// посредством переопределения метода в производном классе, специализированные методы обхода создают новое выражение, которое 
        /// включает изменения в поддереве. В противном случае они возвращают выражения, которое было им передано. Это рекурсивное поведение 
        /// позволяет построить новое дерево выражения, которое является той же или измененной версией исходного выражения, переданного в Visit().
        /// </remarks>
        protected virtual Expression Visit(Expression expression)
        {
            if (expression == null)
            {
                return expression;
            }

            switch (expression.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                    return VisitUnary((UnaryExpression)expression);
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                    return VisitBinary((BinaryExpression)expression);
                case ExpressionType.TypeIs:
                    return VisitTypeIs((TypeBinaryExpression)expression);
                case ExpressionType.Conditional:
                    return VisitConditional((ConditionalExpression)expression);
                case ExpressionType.Constant:
                    return VisitConstant((ConstantExpression)expression);
                case ExpressionType.Parameter:
                    return VisitParameter((ParameterExpression)expression);
                case ExpressionType.MemberAccess:
                    return VisitMemberAccess((MemberExpression)expression);
                case ExpressionType.Call:
                    return VisitMethodCall((MethodCallExpression)expression);
                case ExpressionType.Lambda:
                    return VisitLambda((LambdaExpression)expression);
                case ExpressionType.New:
                    return VisitNew((NewExpression)expression);
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    return VisitNewArray((NewArrayExpression)expression);
                case ExpressionType.Invoke:
                    return VisitInvocation((InvocationExpression)expression);
                case ExpressionType.MemberInit:
                    return VisitMemberInit((MemberInitExpression)expression);
                case ExpressionType.ListInit:
                    return VisitListInit((ListInitExpression)expression);
                default:
                    throw new ArgumentException(string.Format("Unhandled expression type: '{0}'", expression.NodeType));
            }
        }


        /// <summary>
        /// Унарный оператор
        /// </summary>
        protected virtual Expression VisitUnary(UnaryExpression unaryExpression)
        {
            var operand = Visit(unaryExpression.Operand);

            return (operand != unaryExpression.Operand)
                        ? Expression.MakeUnary(unaryExpression.NodeType, operand, unaryExpression.Type, unaryExpression.Method)
                        : unaryExpression;
        }


        /// <summary>
        /// Бинарный оператор
        /// </summary>
        protected virtual Expression VisitBinary(BinaryExpression binary)
        {
            var left = Visit(binary.Left);
            var right = Visit(binary.Right);
            var conversion = Visit(binary.Conversion);

            if (left != binary.Left || right != binary.Right || conversion != binary.Conversion)
            {
                return (binary.NodeType == ExpressionType.Coalesce)
                            ? Expression.Coalesce(left, right, conversion as LambdaExpression)
                            : Expression.MakeBinary(binary.NodeType, left, right, binary.IsLiftedToNull, binary.Method);
            }

            return binary;
        }


        /// <summary>
        /// Операция между выражением и типом
        /// </summary>
        /// <remarks>
        /// Например, value is String
        /// </remarks>
        protected virtual Expression VisitTypeIs(TypeBinaryExpression typeExpression)
        {
            var expression = Visit(typeExpression.Expression);

            return (expression != typeExpression.Expression)
                        ? Expression.TypeIs(expression, typeExpression.TypeOperand)
                        : typeExpression;
        }


        /// <summary>
        /// Условный оператор
        /// </summary>
        /// <remarks>
        /// Например, if (condition) { ... } else { ... }
        /// </remarks>
        protected virtual Expression VisitConditional(ConditionalExpression conditionalExpression)
        {
            var test = Visit(conditionalExpression.Test);
            var ifTrue = Visit(conditionalExpression.IfTrue);
            var ifFalse = Visit(conditionalExpression.IfFalse);

            return (test != conditionalExpression.Test || ifTrue != conditionalExpression.IfTrue || ifFalse != conditionalExpression.IfFalse)
                        ? Expression.Condition(test, ifTrue, ifFalse)
                        : conditionalExpression;
        }


        /// <summary>
        /// Постоянное значение
        /// </summary>
        /// <remarks>
        /// Это константа, объявленная в выражении
        /// </remarks>
        protected virtual Expression VisitConstant(ConstantExpression constantExpression)
        {
            return constantExpression;
        }


        /// <summary>
        /// Именованное выражение параметра
        /// </summary>
        /// <remarks>
        /// Имя параметра, например, в лябда-выражении: param => ...
        /// </remarks>
        protected virtual Expression VisitParameter(ParameterExpression parameterExpression)
        {
            return parameterExpression;
        }


        /// <summary>
        /// Обращение к полю или свойству
        /// </summary>
        /// <remarks>
        /// Например, value.SomeProperty
        /// </remarks>
        protected virtual Expression VisitMemberAccess(MemberExpression memberExpression)
        {
            var expression = Visit(memberExpression.Expression);

            return (expression != memberExpression.Expression)
                        ? Expression.MakeMemberAccess(expression, memberExpression.Member)
                        : memberExpression;
        }


        /// <summary>
        /// Вызов статического метода или метода экземпляра
        /// </summary>
        /// <remarks>
        /// Например, value.SomeMethod(1, 2)
        /// </remarks>
        protected virtual Expression VisitMethodCall(MethodCallExpression callExpression)
        {
            var obj = Visit(callExpression.Object);
            var args = VisitExpressionList(callExpression.Arguments);

            return (obj != callExpression.Object || args != callExpression.Arguments)
                        ? Expression.Call(obj, callExpression.Method, args)
                        : callExpression;
        }

        /// <summary>
        /// Получить список выражений
        /// </summary>
        /// <param name="original">Оригинальный список выражений</param>
        protected virtual ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original)
        {
            List<Expression> list = null;

            var originalCount = original.Count;

            for (var i = 0; i < originalCount; i++)
            {
                var expression = Visit(original[i]);

                if (list != null)
                {
                    list.Add(expression);
                }
                else if (expression != original[i])
                {
                    list = new List<Expression>(originalCount);

                    for (var j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }

                    list.Add(expression);
                }
            }

            return (list != null)
                        ? list.AsReadOnly()
                        : original;
        }


        /// <summary>
        /// Ламбда-выражение
        /// </summary>
        /// <remarks>
        /// Например, arg => (arg + 1)
        /// </remarks>
        protected virtual Expression VisitLambda(LambdaExpression lambdaExpression)
        {
            var body = Visit(lambdaExpression.Body);

            return (body != lambdaExpression.Body)
                        ? Expression.Lambda(lambdaExpression.Type, body, lambdaExpression.Parameters)
                        : lambdaExpression;
        }


        /// <summary>
        /// Вызов конструктора
        /// </summary>
        /// <remarks>
        /// Например, new SomeClass()
        /// </remarks>
        protected virtual NewExpression VisitNew(NewExpression newExpression)
        {
            var args = VisitExpressionList(newExpression.Arguments);

            if (args != newExpression.Arguments)
            {
                return (newExpression.Members != null)
                            ? Expression.New(newExpression.Constructor, args, newExpression.Members)
                            : Expression.New(newExpression.Constructor, args);
            }

            return newExpression;
        }


        /// <summary>
        /// Создание нового массива и возможная инициализация элемента нового массива
        /// </summary>
        /// <remarks>
        /// Например, new[] { 1, 2, 3 }
        /// </remarks>
        protected virtual Expression VisitNewArray(NewArrayExpression newArrayExpression)
        {
            var expressions = VisitExpressionList(newArrayExpression.Expressions);

            if (expressions != newArrayExpression.Expressions)
            {
                return newArrayExpression.NodeType == ExpressionType.NewArrayInit
                            ? Expression.NewArrayInit(newArrayExpression.Type.GetElementType(), expressions)
                            : Expression.NewArrayBounds(newArrayExpression.Type.GetElementType(), expressions);
            }

            return newArrayExpression;
        }


        /// <summary>
        /// Делегат или лямбда-выражение для списка аргументов
        /// </summary>
        /// <remarks>
        /// Например, (num1, num2) => (num1 + num2) > 1000
        /// </remarks>
        protected virtual Expression VisitInvocation(InvocationExpression invocationExpression)
        {
            var arguments = VisitExpressionList(invocationExpression.Arguments);
            var expression = Visit(invocationExpression.Expression);

            return arguments != invocationExpression.Arguments || expression != invocationExpression.Expression
                        ? Expression.Invoke(expression, arguments)
                        : invocationExpression;
        }


        /// <summary>
        /// Вызов конструктора, который содержит инициализатор коллекции
        /// </summary>
        /// <remarks>
        /// Например, new Dictionary[int, string] { { 1, "1" }, { 2, "2" } }
        /// </remarks>
        protected virtual Expression VisitListInit(ListInitExpression listInitExpression)
        {
            var newExpression = VisitNew(listInitExpression.NewExpression);
            var initializers = VisitElementInitializerList(listInitExpression.Initializers);

            if (newExpression != listInitExpression.NewExpression || initializers != listInitExpression.Initializers)
            {
                return Expression.ListInit(newExpression, initializers);
            }

            return listInitExpression;
        }

        /// <summary>
        /// Получить список инициализаторов отдельных элементов коллекции
        /// </summary>
        /// <param name="original">Оригинальный список инициализаторов элементов коллекции</param>
        protected virtual IEnumerable<ElementInit> VisitElementInitializerList(ReadOnlyCollection<ElementInit> original)
        {
            List<ElementInit> list = null;

            var originalCount = original.Count;

            for (var i = 0; i < originalCount; i++)
            {
                var init = VisitElementInitializer(original[i]);

                if (list != null)
                {
                    list.Add(init);
                }
                else if (init != original[i])
                {
                    list = new List<ElementInit>(originalCount);

                    for (var j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }

                    list.Add(init);
                }
            }

            return (list != null)
                        ? (IEnumerable<ElementInit>)list
                        : original;
        }

        /// <summary>
        /// Получить инициализатор элемента коллекции
        /// </summary>
        protected virtual ElementInit VisitElementInitializer(ElementInit initializer)
        {
            var arguments = VisitExpressionList(initializer.Arguments);

            return (arguments != initializer.Arguments)
                        ? Expression.ElementInit(initializer.AddMethod, arguments)
                        : initializer;
        }


        /// <summary>
        /// Вызов конструктора и инициализация одного или нескольких членов нового объекта
        /// </summary>
        /// <remarks>
        /// Например, new SomeClass() { Field1 = 1, Field2 = 2 } 
        /// </remarks>
        protected virtual Expression VisitMemberInit(MemberInitExpression memberInitExpression)
        {
            var newExpression = VisitNew(memberInitExpression.NewExpression);
            var bindings = VisitBindingList(memberInitExpression.Bindings);

            return (newExpression != memberInitExpression.NewExpression || bindings != memberInitExpression.Bindings)
                        ? Expression.MemberInit(newExpression, bindings)
                        : memberInitExpression;
        }

        /// <summary>
        /// Получить список привязок для операций инициализации
        /// </summary>
        /// <param name="original">Оригинальный список привязок</param>
        /// <remarks>
        /// Привязки бывают разных видов и ассоциированы с операциями инициализации. Например, операция присваивания 
        /// для поля или свойства объекта, инициализация элементов коллекции созданного объекта, инициализация элементов
        /// элемента созданного объекта и т.д.
        /// </remarks>
        protected virtual IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> original)
        {
            List<MemberBinding> list = null;

            var originalCount = original.Count;

            for (var i = 0; i < originalCount; i++)
            {
                var memberBinding = VisitBinding(original[i]);

                if (list != null)
                {
                    list.Add(memberBinding);
                }
                else if (memberBinding != original[i])
                {
                    list = new List<MemberBinding>(originalCount);

                    for (var j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }

                    list.Add(memberBinding);
                }
            }

            return (list != null)
                        ? (IEnumerable<MemberBinding>)list
                        : original;
        }

        /// <summary>
        /// Получить привязку для выражения инициализации
        /// </summary>
        protected virtual MemberBinding VisitBinding(MemberBinding binding)
        {
            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    return VisitMemberAssignment((MemberAssignment)binding);
                case MemberBindingType.MemberBinding:
                    return VisitMemberMemberBinding((MemberMemberBinding)binding);
                case MemberBindingType.ListBinding:
                    return VisitMemberListBinding((MemberListBinding)binding);
                default:
                    throw new ArgumentException(string.Format("Unhandled binding type '{0}'", binding.BindingType));
            }
        }

        /// <summary>
        /// Инициализация поля или свойства объекта
        /// </summary>
        protected virtual MemberAssignment VisitMemberAssignment(MemberAssignment memberAssignment)
        {
            var expression = Visit(memberAssignment.Expression);

            return (expression != memberAssignment.Expression)
                        ? Expression.Bind(memberAssignment.Member, expression)
                        : memberAssignment;
        }

        /// <summary>
        /// Инициализация элементов элемента созданного объекта
        /// </summary>
        protected virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding memberMemberBinding)
        {
            var bindings = VisitBindingList(memberMemberBinding.Bindings);

            return (bindings != memberMemberBinding.Bindings)
                        ? Expression.MemberBind(memberMemberBinding.Member, bindings)
                        : memberMemberBinding;
        }

        /// <summary>
        /// Инициализация элементов коллекции созданного объекта
        /// </summary>
        protected virtual MemberListBinding VisitMemberListBinding(MemberListBinding memberListBinding)
        {
            var initializers = VisitElementInitializerList(memberListBinding.Initializers);

            return (initializers != memberListBinding.Initializers)
                        ? Expression.ListBind(memberListBinding.Member, initializers)
                        : memberListBinding;
        }
    }
}