using Rps.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rps.Domain.Model
{
    public struct Bounds
        : IEquatable<Bounds>
    {
        public static readonly Bounds Empty = new Bounds();

        public int Rows { get; private set; }
        public int Cols { get; private set; }

        public Bounds(int rows, int cols)
            : this()
        {
            this.Rows = rows;
            this.Cols = cols;
        }

        public bool Contains(Point point)
        {
            return point.X >= 0 &&
                   point.X < this.Cols &&
                   point.Y >= 0 &&
                   point.Y < this.Rows;
        }

        public void AssertContains(Point point)
        {
            if (!Contains(point))
            {
                throw new InvalidPointException(string.Format("Point {{ {0} }} does not fit in game bounds {{ {1} }}.", point, this));
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Bounds))
            {
                return false;
            }

            return this.Equals((Bounds)obj);
        }

        public bool Equals(Bounds other)
        {
            return this.Rows == other.Rows && this.Cols == other.Cols;
        }

        public override int GetHashCode()
        {
            return Rows.GetHashCode() ^ Rows.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Rows.ToString(), Rows.ToString());
        }

        public static bool operator ==(Bounds lhs, Bounds rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Bounds lhs, Bounds rhs)
        {
            return !(lhs == rhs);
        }
    }
}
