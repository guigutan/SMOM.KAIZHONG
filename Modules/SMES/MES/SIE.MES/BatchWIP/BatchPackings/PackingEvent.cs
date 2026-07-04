using System;

namespace SIE.MES.BatchWIP.BatchPackings
{
    /// <summary>
    /// 批次打包事件
    /// </summary>
    [Serializable]
    public class PackingEvent
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stationId">工位ID</param>
        /// <param name="relationIdList">包装关系ID数组</param>
        public PackingEvent(double stationId, double[] relationIdList)
        {
            StationId = stationId;
            RelationIdList = relationIdList;
        }

        /// <summary>
        /// 工位ID
        /// </summary>
        public double StationId { get; }

        /// <summary>
        /// 包装关系ID数组
        /// </summary> 
        public double[] RelationIdList { get; }
    }
}