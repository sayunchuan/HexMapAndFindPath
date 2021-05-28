using System.Collections.Generic;

namespace HexagonMap
{
    public struct Position3D
    {
        public static Position3D Zero => new Position3D(0, 0, 0);

        public int X;
        public int Y;
        public int Z;

        public Position3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Position3D operator +(Position3D a, Position3D b)
        {
            return new Position3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Position3D operator -(Position3D a, Position3D b)
        {
            return new Position3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static bool operator ==(Position3D a, Position3D b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Position3D a, Position3D b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() << 2 ^ Z.GetHashCode() >> 2;
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }

    public class Position3DCompare : IEqualityComparer<Position3D>
    {
        private readonly static Position3DCompare __ins = new Position3DCompare();

        public static Position3DCompare Default => __ins;

        public bool Equals(Position3D x, Position3D y)
        {
            return x == y;
        }

        public int GetHashCode(Position3D obj)
        {
            return obj.GetHashCode();
        }
    }
}