using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rps.Domain.Model
{
    public struct GameProperties
        : IEquatable<GameProperties>
    {
        public Bounds Bounds { get; private set; }
        public int RowsPerPlayer { get; private set; }
        public int BombsPerPlayer { get; private set; }

        public GameProperties(Bounds bounds, int rowsPerPlayer, int bombsPerPlayer)
            : this()
        {
            this.Bounds = bounds;
            this.RowsPerPlayer = rowsPerPlayer;
            this.BombsPerPlayer = bombsPerPlayer;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is GameProperties))
            {
                return false;
            }

            return this.Equals((GameProperties)obj);
        }

        public bool Equals(GameProperties other)
        {
            return this.Bounds == other.Bounds &&
                this.RowsPerPlayer == other.RowsPerPlayer &&
                this.BombsPerPlayer == other.BombsPerPlayer;
        }

        public override int GetHashCode()
        {
            return Bounds.GetHashCode() ^ RowsPerPlayer.GetHashCode() ^ BombsPerPlayer.GetHashCode();
        }

        public static bool operator ==(GameProperties lhs, GameProperties rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(GameProperties lhs, GameProperties rhs)
        {
            return !(lhs == rhs);
        }
    }
}
