using System.Text.RegularExpressions;

namespace Esprima.Ast
{
    public sealed class Literal : Expression
    {
        public string? StringValue => TokenType == TokenType.StringLiteral ? Value as string : null;
        public readonly double NumericValue;
        public bool BooleanValue => TokenType == TokenType.BooleanLiteral && NumericValue != 0;
        public Regex? RegexValue => TokenType == TokenType.RegularExpression ? (Regex?) Value : null;

        public readonly RegexValue? Regex;
        public readonly object? Value;
        public readonly Span Raw;
        public readonly TokenType TokenType;

        internal Literal(TokenType tokenType, object? value, Span raw) : base(Nodes.Literal)
        {
            TokenType = tokenType;
            Value = value;
            Raw = raw;
        }

        public Literal(string? value, Span raw) : this(TokenType.StringLiteral, value, raw)
        {
        }

        public Literal(bool value, Span raw) : this(TokenType.BooleanLiteral, value, raw)
        {
            NumericValue = value ? 1 : 0;
        }

        public Literal(double value, Span raw) : this(TokenType.NumericLiteral, value, raw)
        {
            NumericValue = value;
        }

        public Literal(Span raw) : this(TokenType.NullLiteral, null, raw)
        {
        }

        public Literal(string pattern, string flags, object? value, Span raw) : this(TokenType.RegularExpression, value, raw)
        {
            // value is null if a Regex object couldn't be created out of the pattern or options
            Regex = new RegexValue(pattern, flags);
        }

        public override NodeCollection ChildNodes => NodeCollection.Empty;
    }
}