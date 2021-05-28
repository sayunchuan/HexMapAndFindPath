using System.Collections.Generic;

namespace HexagonMap
{
    public struct Position2D
    {
        public static Position2D Zero => new Position2D(0, 0);

        public int X;
        public int Y;

        public Position2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(Position2D a, Position2D b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Position2D a, Position2D b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() << 2;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }

    public class Position2DCompare : IEqualityComparer<Position2D>
    {
        private readonly static Position2DCompare __ins = new Position2DCompare();

        public static Position2DCompare Default => __ins;

        public bool Equals(Position2D x, Position2D y)
        {
            return x == y;
        }

        public int GetHashCode(Position2D obj)
        {
            return obj.GetHashCode();
        }
    }
}