using System;

namespace SIE.MES.WIP.PackRecombine
{
    /// <summary>
    /// 包装拆合信息
    /// </summary>
    [Serializable]
    public class RecombineInfo
    {
        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 新加入的外包装号
        /// </summary>
        public string NewPackingNo { get; set; }

        /// <summary>
        /// 新加入的外包装单位Id
        /// </summary>
        public double NewPackingUnitId { get; set; }

        /// <summary>
        /// 是否移除操作
        /// </summary>
        public bool IsRemove { get; set; }

        /// <summary>
        /// 移除条码（包装）的外包装号
        /// </summary>
        public string OldPackingNo { get; set; }

        /// <summary>
        /// 移除条码（包装）的外包装单位Id
        /// </summary>
        public double OldPackingUnitId { get; set; }

        /// <summary>
        /// 移除包装的包装单位
        /// 单体的为主单位
        /// </summary>
        public string PackingUnit { get; set; }

        /// <summary>
        /// 移除包装的包装单位Id
        /// 单体的为主单位Id
        /// </summary>
        public double PackingUnitId { get; set; }

        /// <summary>
        /// 是否满包装
        /// </summary>
        public bool IsFullPack { get; set; }

        /// <summary>
        /// 包装关系ID
        /// </summary>
        public double RelationId { get; set; }
    }
}