using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rps.Domain.Model
{
    public struct GameMove
        : IEquatable<GameMove>
    {
        public static readonly GameMove Empty = new GameMove();

        public Token Token { get; private set; }
        public Point MoveTo { get; private set; }

        public GameMove(Token token, Point moveTo)
            : this()
        {
            this.Token = token;
            this.MoveTo = moveTo;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is GameMove))
            {
                return false;
            }

            return base.Equals((GameMove)obj);
        }

        public bool Equals(GameMove other)
        {
            return this.Token == other.Token && this.MoveTo == other.MoveTo;
        }

        public override int GetHashCode()
        {
            return (this.Token == null ? 0 : this.Token.GetHashCode()) ^ this.MoveTo.GetHashCode();
        }

        public static bool operator==(GameMove lhs, GameMove rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator!=(GameMove lhs, GameMove rhs)
        {
            return !(lhs == rhs);
        }
    }
}
