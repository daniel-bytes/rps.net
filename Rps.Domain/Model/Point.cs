using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rps.Domain.Model
{
    public struct Point
        : IEquatable<Point>
    {
        public static readonly Point Empty = new Point();

        public int X { get; private set; }
        public int Y { get; private set; }

        public Point(int x, int y)
            : this()
        {
            this.X = x;
            this.Y = y;
        }

        public Point AddX(int x)
        {
            return new Point(X + x, Y);
        }

        public Point AddY(int y)
        {
            return new Point(X, Y + y);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point))
            {
                return false;
            }

            return this.Equals((Point)obj);
        }

        public bool Equals(Point other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", X.ToString(), Y.ToString());
        }

        public static bool operator==(Point lhs, Point rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator!=(Point lhs, Point rhs)
        {
            return !(lhs == rhs);
        }

        public static Point operator+(Point lhs, Point rhs)
        {
            return new Point(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }
    }
}
