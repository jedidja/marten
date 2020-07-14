using System;
using System.Linq.Expressions;
using LamarCodeGeneration;
using Remotion.Linq.Parsing;

namespace Marten.V4Internals.Linq
{
    public partial class LinqHandlerBuilder
    {
        public class SelectorVisitor: ExpressionVisitor
        {
            private readonly LinqHandlerBuilder _parent;

            public SelectorVisitor(LinqHandlerBuilder parent)
            {
                _parent = parent;
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                _parent.CurrentStatement.ToScalar(node);
                return null;
            }

            protected override Expression VisitMemberInit(MemberInitExpression node)
            {
                _parent.CurrentStatement.ToSelectTransform(node);
                return null;
            }

            protected override Expression VisitNew(NewExpression node)
            {
                _parent.CurrentStatement.ToSelectTransform(node);
                return null;
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                var method = node.Method;

                bool matched = false;
                foreach (var matcher in _methodMatchers)
                {
                    if (matcher.TryMatch(node, this, out var op))
                    {
                        _parent.AddResultOperator(op);
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    throw new NotImplementedException($"Marten does not (yet) support the {method.DeclaringType.FullNameInCode()}.{method.Name}() method as a Linq selector");
                }

                return null;
            }


        }
    }
}