using System;

namespace HexagonMap
{
    public static class Util
    {
        private const float sqrt3_2 = 0.8660254038f;
        private const float sqrt3_3 = 0.5773502692f;
        private const float sqrt3_6 = 0.2886751346f;

#if UNITY_EDITOR

        /// <summary>
        /// 2D坐标转换为真实二维坐标
        /// </summary>
        public static UnityEngine.Vector2 Pos2Vec(Position2D pos)
        {
            return new UnityEngine.Vector2(pos.X + (pos.Y % 2) * 0.5f, sqrt3_2 * pos.Y);
        }

        /// <summary>
        /// 真实二维坐标转换为2D坐标
        /// </summary>
        public static Position2D Vec2Pos(UnityEngine.Vector2 v)
        {
            int miny = (int) Math.Floor(v.y / sqrt3_2);
            int maxy = (int) Math.Ceiling(v.y / sqrt3_2);

            int minx, maxx;
            float limitH;
            if (miny % 2 == 0)
            {
                minx = (int) Math.Floor(v.x + 0.5f);
                maxx = (int) Math.Floor(v.x);

                limitH = minx <= maxx
                    ? miny * sqrt3_2 + sqrt3_3 - sqrt3_3 * (v.x - minx)
                    : miny * sqrt3_2 + sqrt3_6 + sqrt3_3 * (v.x - minx + 0.5f);
            }
            else
            {
                minx = (int) Math.Floor(v.x);
                maxx = (int) Math.Floor(v.x + 0.5f);

                limitH = minx < maxx
                    ? miny * sqrt3_2 + sqrt3_3 - sqrt3_3 * (v.x - maxx + 0.5f)
                    : miny * sqrt3_2 + sqrt3_6 + sqrt3_3 * (v.x - maxx);
            }

            return v.y < limitH ? new Position2D(minx, miny) : new Position2D(maxx, maxy);
        }

#endif

        /// <summary>
        /// 六边形地图二维坐标转三维坐标
        /// </summary>
        public static Position3D Pos223(Position2D pos)
        {
            int x = pos.X - (pos.Y >> 1);
            int y = pos.Y;
            return new Position3D(x, y, -x - y);
        }

        /// <summary>
        /// 六边形地图三维坐标转二维坐标
        /// </summary>
        public static Position2D Pos322(Position3D pos)
        {
            return new Position2D(pos.X + (pos.Y >> 1), pos.Y);
        }

        public static bool ValidPos2D(Position2D pos, Position2D limit)
        {
            return pos.X >= 0 && pos.X < limit.X && pos.Y >= 0 && pos.Y < limit.Y;
        }

        public static int LengthBy3D(Position3D a, Position3D b)
        {
            Position3D delta = a - b;
            return delta.X >= 0
                ? (delta.Y >= 0 ? -delta.Z : (delta.Z > 0 ? -delta.Y : delta.X))
                : (delta.Y <= 0 ? delta.Z : (delta.Z > 0 ? -delta.X : delta.Y));
        }

        public static int LengthBy2D(Position2D a, Position2D b)
        {
            Position3D a3 = Pos223(a);
            Position3D b3 = Pos223(b);
            return LengthBy3D(a3, b3);
        }
    }
}