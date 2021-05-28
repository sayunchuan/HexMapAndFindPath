namespace HexagonMap
{
    /// <summary>
    /// 地图单节点信息
    /// 除包含节点自身数据，也存储寻路所用数据
    /// </summary>
    public class MapItem
    {
        /// <summary>
        /// 实例数据唯一id
        /// </summary>
        public int id;

        /// <summary>
        /// 地图点位的三维坐标
        /// </summary>
        public Position3D Pos3D;

        /// <summary>
        /// 地图点位的二维坐标
        /// </summary>
        public Position2D Pos2D;

        #region Find Path Fields

        /// <summary>
        /// 已行走路径权重
        /// </summary>
        public int HisWeight;

        /// <summary>
        /// 总权重，由已走路径权重与期待权重加和构成
        /// </summary>
        public int TotalWeight;

        /// <summary>
        /// 路径前一节点id
        /// </summary>
        public int BeforeItemId;

        /// <summary>
        /// OpenTable中的位置id
        /// </summary>
        public int OpenTableIndex;

        /// <summary>
        /// 寻路版本号，用于标记OpenTable节点状态
        /// </summary>
        public int OpenTableVersion;

        /// <summary>
        /// 寻路版本号，用于标记CloseTable节点状态
        /// </summary>
        public int CloseTableVersion;

        #endregion

        public MapItem(int id, int x, int y)
        {
            this.id = id;
            Pos2D = new Position2D(x, y);
            Pos3D = Util.Pos223(Pos2D);

            HisWeight = 0;
            TotalWeight = 0;
            BeforeItemId = -1;
            OpenTableIndex = -1;
            OpenTableVersion = 0;
            CloseTableVersion = 0;
        }

        #region Find Path Methods

        public void Begin(int version, int weight)
        {
            TotalWeight = weight;
            BeforeItemId = -1;
            OpenTableVersion = version;
        }

        public void AddOpen(int version, int hisWeight, int totalWeight, int beforeId)
        {
            OpenTableVersion = version;
            HisWeight = hisWeight;
            TotalWeight = totalWeight;
            BeforeItemId = beforeId;
        }

        public void RefreshOpen(int hisWeight, int totalWeight, int beforeId)
        {
            HisWeight = hisWeight;
            TotalWeight = totalWeight;
            BeforeItemId = beforeId;
        }

        #endregion
    }
}