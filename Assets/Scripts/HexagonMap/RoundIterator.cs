namespace HexagonMap
{
    public struct RoundIterator
    {
        // private static Position3D[] _RoundDir =
        // {
        //     new Position3D(0, 1, -1),
        //     new Position3D(1, 0, -1),
        //     new Position3D(1, -1, 0),
        //     new Position3D(0, -1, 1),
        //     new Position3D(-1, 0, 1),
        //     new Position3D(-1, 1, 0)
        // };

        private Position3D _pos;
        private int limitX;

        private int limitY;

        // private Position2D _posLimit;
        private int _next;

        public RoundIterator(MapItem item, int maxX, int maxY)
        {
            _pos = item.Pos3D;
            limitX = maxX;
            limitY = maxY;
            // _posLimit = new Position2D(maxX, maxY);
            Current = Position2D.Zero;
            _next = -1;
        }

        public Position2D Current; // { get; private set; }

        public bool MoveNext()
        {
            int x, y;
            while (++_next < 6)
            {
                switch (_next)
                {
                    case 0:
                        y = _pos.Y + 1;
                        x = _pos.X + (y >> 1);
                        break;
                    case 1:
                        y = _pos.Y;
                        x = _pos.X + 1 + (y >> 1);
                        break;
                    case 2:
                        y = _pos.Y - 1;
                        x = _pos.X + 1 + (y >> 1);
                        break;
                    case 3:
                        y = _pos.Y - 1;
                        x = _pos.X + (y >> 1);
                        break;
                    case 4:
                        y = _pos.Y;
                        x = _pos.X - 1 + (y >> 1);
                        break;
                    case 5:
                        y = _pos.Y + 1;
                        x = _pos.X - 1 + (y >> 1);
                        break;
                    default:
                        return false;
                }

                if (x >= 0 && x < limitX && y >= 0 && y < limitY)
                {
                    Current.X = x;
                    Current.Y = y;
                    // Current = new Position2D(x, y);
                    return true;
                }
            }

            return false;
        }
    }
}