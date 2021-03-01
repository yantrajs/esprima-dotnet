using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Esprima
{
    public static class SpanExtensions
    {
        public static Span Span(this string source, int offset, int length)
        {
            return new Span(source, offset, length);
        }
        public static Span Span(this in Span source, int offset, int length)
        {
            return new Span(source, offset, length);
        }


        public static string? ToLowerInvariant(this in Span source)
        {
            return source.Value?.ToLowerInvariant();
        }

        public static int ConvertToUtf32(this in Span source, int index)
        {
            return Char.ConvertToUtf32(source.Source, index + source.Offset);
        }

        public static bool IsLetter(this in Span source, int index)
        {
            return Char.IsLetter(source.Source, source.Offset + index);
        }

        public static long ToInt64(this in Span source, int radix)
        {
            return Convert.ToInt64(source.Value, radix);
        }
        public static ulong ToUInt64(this in Span source, int radix)
        {
            return Convert.ToUInt64(source.Value, radix);
        }
        public static long ToInt32(this in Span source, int radix)
        {
            return Convert.ToInt32(source.Value, radix);
        }

        public static long ToUInt32(this in Span source, int radix)
        {
            return Convert.ToUInt32(source.Value, radix);
        }
    }


    [DebuggerDisplay("{Value}")]
    public readonly struct Span :
        IEquatable<Span>,
        IEquatable<string>,
        IEnumerable<char>
    {

        public static readonly Span Empty = string.Empty;

        public readonly string? Source;
        public readonly int Offset;
        public readonly int Length;
        public Span(string? source)
        {
            this.Source = source;
            this.Offset = 0;
            this.Length = source?.Length ?? 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span(Span buffer, int offset, int length)
        {

            if (buffer.Source == null || (uint)offset > (uint)buffer.Length || (uint)length > (uint)(buffer.Length - (buffer.Offset + offset)))
            {
                throw new InvalidOperationException($"offset/length represents invalid string or string is null");
            }
            this.Source = buffer.Source;
            this.Offset = buffer.Offset + offset;
            this.Length = length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span(string? buffer, int offset, int length)
        {
            if (buffer == null || (uint)offset > (uint)buffer.Length || (uint)length > (uint)(buffer.Length - offset))
            {
                throw new InvalidOperationException($"offset/length represents invalid string or string is null");
            }
            this.Source = buffer;
            this.Offset = offset;
            this.Length = length;
        }

        public string? Value
        {
            get
            {
                if (Source == null)
                    return Source;
                if (Offset == 0 && Length == Source.Length)
                    return Source;
                return Source.Substring(Offset, Length);
            }
        }

        public unsafe char CharCodeAt(int index)
        {
            if (Source == null || (uint)index >= (uint)Length)
            {
                return char.MinValue;
            }
            fixed (char* src = Source)
            {
                char* charAt = src + Offset + index;
                return *charAt;
            }
        }

        public unsafe char this[int index]
        {
            get
            {
                if (Source == null || (uint)index >= (uint)Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                fixed (char* src = Source)
                {
                    char* charAt = src + Offset + index;
                    return *charAt;
                }
            }
        }

        public static int Compare(in Span a, in Span b, StringComparison comparisonType)
        {
            int minLength = Math.Min(a.Length, b.Length);
            int diff = string.Compare(a.Source, a.Offset, b.Source, b.Offset, minLength, comparisonType);
            if (diff == 0)
            {
                diff = a.Length - b.Length;
            }

            return diff;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is Span segment && Equals(in segment, StringComparison.Ordinal);
        }


        public override string ToString()
        {
            return Value ?? string.Empty;
        }

        internal bool IsNullOrWhiteSpace()
        {
            return Source == null || string.IsNullOrWhiteSpace(Value);
        }

        public bool IsEmpty => Length == 0;

        public static implicit operator Span(string source)
        {
            return new Span(source);
        }

        public static bool operator ==(in Span left, in Span right)
        {
            return left.Equals(in right, StringComparison.Ordinal);
        }
        public static bool operator !=(in Span left, in Span right)
        {
            return !left.Equals(in right, StringComparison.Ordinal);
        }

        public static Span operator +(in Span left, in Span right)
        {
            return new Span(left.Value + right.Value);
        }

        public static Span operator +(double left, in Span right)
        {
            return new Span(left.ToString() + right.Value);
        }


        public bool Equals(Span other) => Equals(other, StringComparison.Ordinal);

        public bool Equals(in Span other, StringComparison comparisonType)
        {
            if (Length != other.Length)
            {
                return false;
            }

            return string.Compare(Source, Offset, other.Source, other.Offset, other.Length, comparisonType) == 0;
        }

        public static bool Equals(in Span a, in Span b, StringComparison comparisonType)
        {
            return a.Equals(b, comparisonType);
        }
        public bool Equals(string other)
        {
            return Equals(other, StringComparison.Ordinal);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(string text, StringComparison comparisonType)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            int textLength = text.Length;
            if (Source == null || Length != textLength)
            {
                return false;
            }

            return string.Compare(Source, Offset, text, 0, textLength, comparisonType) == 0;
        }

        public int CompareTo(in Span other)
        {
            if (Source == null)
            {
                if (other.Source == null)
                    return 0;
                return 1;
            }
            if (other.Source == null)
                return -1;
            return string.Compare(Source, Offset, other.Source, other.Offset, Length, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return UnsafeGetHashCode();
            // return Value?.GetHashCode() ?? 0;
        }

        private unsafe int UnsafeGetHashCode()
        {
            unchecked
            {
                if (Source == null)
                    return 0;
                fixed (char* src = Source)
                {
                    int hash1 = 5381;
                    int hash2 = hash1;

                    int c;
                    char* s = src + Offset;
                    c = s[0];
                    for (int i = 0; i < Length; i++)
                    {
                        c = s[i];
                        hash1 = ((hash1 << 5) + hash1) ^ c;
                        if (i == Length - 1)
                            break;
                        c = s[i + 1];
                        hash2 = ((hash2 << 5) + hash2) ^ c;
                    }
                    //while ((c = s[0]) != 0)
                    //{
                    //    hash1 = ((hash1 << 5) + hash1) ^ c;
                    //    c = s[1];
                    //    if (c == 0)
                    //        break;
                    //    hash2 = ((hash2 << 5) + hash2) ^ c;
                    //    s += 2;
                    //}
                    return hash1 + (hash2 * 1566083941);
                }
            }
        }

        public Span Substring(int index)
        {
            return new Span(Source, Offset + index, Length - index);
        }

        public CharEnumerator GetEnumerator()
        {
            return new CharEnumerator(this);
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<char> IEnumerable<char>.GetEnumerator()
        {
            return GetEnumerator();
        }

        public struct CharEnumerator : IEnumerator<char>
        {
            private Span span;
            private int index;

            public CharEnumerator(in Span span)
            {
                this.span = span;
                this.index = -1;
            }

            public unsafe bool MoveNext(out char ch)
            {
                this.index++;
                if (this.index >= span.Length)
                {
                    ch = '\0';
                    return false;
                }
                fixed (char* start = span.Source)
                {
                    char* ch1 = start + (span.Offset + index);
                    ch = *ch1;
                    return true;
                }

            }

            public char Current => UnsafeChar();

            private unsafe char UnsafeChar()
            {
                fixed (char* start = span.Source)
                {
                    char* ch = start + (span.Offset + index);
                    return *ch;
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                // throw new NotImplementedException();
            }

            public bool MoveNext()
            {
                return ++this.index < span.Length;
            }

            public void Reset()
            {
                // throw new NotImplementedException();
            }
        }
    }
}
