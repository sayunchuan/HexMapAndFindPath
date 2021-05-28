using System.Collections.Generic;
using HexagonMap.Collections;

namespace HexagonMap
{
    public class MapData
    {
        /// <summary>
        /// height (y)
        /// ^
        /// |
        /// |
        /// |
        /// +-----------> width (x)
        /// O(0,0)
        ///
        /// 尖朝上六边形地图
        ///    /\
        ///   | |
        ///  /\/\
        /// | | |
        /// \/\/
        /// O(0,0)
        /// </summary>
        private MapItem[] _items;

        public readonly int Width;
        public readonly int Height;

        /// <summary>
        /// 初始化数据数组
        /// </summary>
        public MapData(int width, int height)
        {
            Width = width;
            Height = height;
            _items = new MapItem[width * height];
            for (int y = 0; y < height; y++)
            {
                int d = y * width;
                for (int x = 0; x < width; x++)
                {
                    _items[x + d] = new MapItem(x + d, x, y);
                }
            }
        }

        public MapItem Get(int id)
        {
            return _items[id];
        }

        public MapItem Get(int x, int y)
        {
            return _items[x + y * Width];
        }

        public MapItem Get(Position2D pos)
        {
            return Get(pos.X, pos.Y);
        }

        private int endPos3DX;
        private int endPos3DY;
        private int endPos3DZ;

        /// <summary>
        /// OpenTable，存放待寻路节点
        /// </summary>
        private OpenTable openTable = new OpenTable();

        /// <summary>
        /// 寻路版本，每次寻路增1
        /// </summary>
        private int FindPathVersion = 0;

        public List<MapItem> res = new List<MapItem>();

        public void Find(Position2D begin, Position2D end)
        {
            // begin

            // CloseTable，存放已寻路成功节点
            MapItem closeTableLastItem = null;
            FindPathVersion++;

            MapItem _begin = Get(begin);
            MapItem _end = Get(end);

            int endId = _end.id;
            endPos3DX = _end.Pos3D.X;
            endPos3DY = _end.Pos3D.Y;
            endPos3DZ = _end.Pos3D.Z;

            _begin.Begin(FindPathVersion, __Weight(_begin.Pos3D));

            openTable.Add(_begin);

            // run
            while (true)
            {
                MapItem current = openTable.Dequeue();
                current.CloseTableVersion = FindPathVersion;
                closeTableLastItem = current;
                int currId = current.id;
                if (currId == endId)
                {
                    break;
                }

                int baseX = current.Pos3D.X;
                int baseY = current.Pos3D.Y;
                int x = 0, y = 0;
                for (int i = 0; i < 6; i++)
                {
                    switch (i)
                    {
                        case 0:
                            y = baseY + 1;
                            x = baseX + (y >> 1);
                            break;
                        case 1:
                            y = baseY;
                            x = baseX + 1 + (y >> 1);
                            break;
                        case 2:
                            y = baseY - 1;
                            x = baseX + 1 + (y >> 1);
                            break;
                        case 3:
                            y = baseY - 1;
                            x = baseX + (y >> 1);
                            break;
                        case 4:
                            y = baseY;
                            x = baseX - 1 + (y >> 1);
                            break;
                        case 5:
                            y = baseY + 1;
                            x = baseX - 1 + (y >> 1);
                            break;
                    }

                    if (x < 0 || x >= Width || y < 0 || y >= Height) continue;

                    var currNeighbour = _items[x + y * Width];
                    // 节点的FindPathVersion与组件内的FindPathVersion一致，则说明该节点已加入close table中
                    if (currNeighbour.CloseTableVersion == FindPathVersion) continue;

                    int gValue = current.HisWeight + 1; //__Weight(currNeighbour, current);
                    int hValue = __Weight(currNeighbour.Pos3D);
                    int fValue = gValue + hValue;

                    if (currNeighbour.OpenTableVersion == FindPathVersion)
                    {
                        if (currNeighbour.TotalWeight > fValue)
                        {
                            currNeighbour.RefreshOpen(gValue, fValue, currId);
                            openTable.Refresh(currNeighbour.OpenTableIndex);
                        }
                    }
                    else
                    {
                        currNeighbour.AddOpen(FindPathVersion, gValue, fValue, currId);
                        openTable.Add(currNeighbour);
                    }
                }
            }

            // end
            res.Clear();

            int beginId = _begin.id;
            while (true)
            {
                res.Add(closeTableLastItem);
                if (closeTableLastItem.BeforeItemId == beginId) break;
                closeTableLastItem = _items[closeTableLastItem.BeforeItemId];
            }

            int _l = 0, _r = res.Count - 1;
            while (_l < _r)
            {
                var tmp = res[_l];
                res[_l++] = res[_r];
                res[_r--] = tmp;
            }

            openTable.Clear();
        }

        private int __Weight(Position3D n1)
        {
            int X = n1.X - endPos3DX;
            int Y = n1.Y - endPos3DY;
            int Z = n1.Z - endPos3DZ;
            return X >= 0
                ? (Y >= 0 ? -Z : (Z > 0 ? -Y : X))
                : (Y <= 0 ? Z : (Z > 0 ? -X : Y));
        }
    }
}