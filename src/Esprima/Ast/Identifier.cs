namespace Esprima.Ast
{
    public sealed class Identifier : Expression
    {
        public readonly Span? Name;

        public Identifier(Span? name) : base(Nodes.Identifier)
        {
            Name = name;
        }

        public override NodeCollection ChildNodes => NodeCollection.Empty;
    }
}