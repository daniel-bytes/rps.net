using System;
using System.Diagnostics;

namespace Rps.Domain.Model
{
    [DebuggerDisplay("{ID} {TokenType} - {Point}")]
    public class Token
        : IEquatable<Token>
    {
        public long ID { get; private set; }
        public string PlayerID { get; private set; }
        public TokenType TokenType { get; private set; }
        public Point Point { get; private set; }

        public int Row { get { return Point.X; } }
        public int Col { get { return Point.Y; } }

        public Token(long id,
                     string playerID,
                     TokenType tokenType,
                     Point point)
        {
            this.ID = id;
            this.PlayerID = playerID;
            this.TokenType = tokenType;
            this.Point = point;
        }

        public Token(long id,
                     string playerID,
                     TokenType tokenType,
                     int row,
                     int col)
            : this(id, playerID, tokenType, new Point(row, col))
        {
        }

        public void SetPoint(Point point)
        {
            this.Point = point;
        }

        public bool Equals(Token other)
        {
            if (other == null)
            {
                return false;
            }

            return this.ID == other.ID
                && this.PlayerID == other.PlayerID
                && this.TokenType == other.TokenType
                && this.Point == other.Point;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Token);
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode()
                ^ (this.PlayerID ?? string.Empty).GetHashCode()
                ^ this.TokenType.GetHashCode()
                ^ this.Point.GetHashCode();
        }
    }
}
