using System;
using System.Diagnostics;
using Rps.Domain.Extensions;

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

        public bool IsMovable()
        {
            return this.TokenType.IsMovable();
        }

        public GameMoveResultType Attack(Token defender)
        {
            var attacker = this;

            if (attacker.TokenType != Model.TokenType.Rock &&
                attacker.TokenType != Model.TokenType.Paper &&
                attacker.TokenType != Model.TokenType.Scissor)
            {
                throw new Exceptions.InvalidMoveException(
                    string.Format("Invalid move for token with ID {0}.  Cannot attack with Token Type {1}.", attacker.ID, attacker.TokenType)
                    );
            }

            if (defender == null || defender.TokenType == TokenType.Empty)
            {
                return GameMoveResultType.TokenMove;
            }
            else
            {
                switch (defender.TokenType)
                {
                    case TokenType.Bomb:
                        return GameMoveResultType.BothLose;

                    case TokenType.Flag:
                        return GameMoveResultType.FlagCapturedByAttacker;

                    case TokenType.Rock:
                        if (attacker.TokenType == TokenType.Paper)
                            return GameMoveResultType.AttackerWins;
                        else if (attacker.TokenType == TokenType.Scissor)
                            return GameMoveResultType.DefenderWins;
                        else
                            return GameMoveResultType.BothLose;

                    case TokenType.Paper:
                        if (attacker.TokenType == TokenType.Scissor)
                            return GameMoveResultType.AttackerWins;
                        else if (attacker.TokenType == TokenType.Rock)
                            return GameMoveResultType.DefenderWins;
                        else
                            return GameMoveResultType.BothLose;

                    case TokenType.Scissor:
                        if (attacker.TokenType == TokenType.Rock)
                            return GameMoveResultType.AttackerWins;
                        else if (attacker.TokenType == TokenType.Paper)
                            return GameMoveResultType.DefenderWins;
                        else
                            return GameMoveResultType.BothLose;
                }

                throw new InvalidOperationException(string.Format("Invalid move, unknown state.  Token {0} to Token {1}.", attacker.ID, defender.ID));
            }
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
