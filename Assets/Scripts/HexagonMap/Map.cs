using System.Collections.Generic;
using System.Diagnostics;

namespace HexagonMap
{
    public class Map
    {
        /// <summary>
        /// 地图数据
        /// </summary>
        public MapData Data;

        /// <summary>
        /// 地图A星工具类
        /// </summary>
        private AStarToolkit _ast;

        public Map(int width, int height)
        {
            Data = new MapData(width, height);
            _ast = new AStarToolkit(Data);
        }

        // public void FindPath(Vector2 begin, Vector2 end, ref List<Vector2> path)
        // {
        //     path.Clear();
        //
        //     if (begin == end) return;
        //
        //     Position2D beginPos = Util.Vec2Pos(begin);
        //     Position2D endPos = Util.Vec2Pos(end);
        //
        //     if (beginPos == endPos)
        //     {
        //         path.Add(end);
        //         return;
        //     }
        //
        //     _ast.Find(beginPos, endPos);
        //
        //     for (int i = 0, imax = _ast.res.Count; i < imax; i++)
        //     {
        //         path.Add(Util.Pos2Vec(_ast.res[i].Pos2D));
        //     }
        // }

        // public void FindPath(Position2D begin, Position2D end, ref List<Vector2> path)
        // {
        //     path.Clear();
        //
        //     if (begin == end)
        //     {
        //         return;
        //     }
        //
        //     _ast.Find(begin, end);
        //
        //     for (int i = 0, imax = _ast.res.Count; i < imax; i++)
        //     {
        //         Debug.Log(_ast.res[i].Pos2D);
        //         path.Add(Util.Pos2Vec(_ast.res[i].Pos2D));
        //     }
        // }

        public void FindPath(Position2D begin, Position2D end, ref List<Position2D> path)
        {
            path.Clear();

            if (begin == end)
            {
                return;
            }

            Stopwatch s = new Stopwatch();
            s.Start();
            Data.Find(begin, end);
            s.Stop();
#if UNITY_EDITOR
            UnityEngine.Debug.Log($"Find path from {begin} to {end} cost {s.ElapsedMilliseconds} ms.");
#else
            System.Console.WriteLine($"Find path from {begin} to {end} cost {s.ElapsedMilliseconds} ms.");
#endif

            for (int i = 0, imax = Data.res.Count; i < imax; i++)
            {
                path.Add(Data.res[i].Pos2D);
            }
        }
    }
}