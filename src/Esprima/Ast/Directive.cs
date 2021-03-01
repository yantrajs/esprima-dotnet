namespace Esprima.Ast
{
    public sealed class Directive : ExpressionStatement
    {
        public readonly Span Directiv;

        public Directive(Expression expression, Span directive) : base(expression)
        {
            Directiv = directive;
        }
    }
}
