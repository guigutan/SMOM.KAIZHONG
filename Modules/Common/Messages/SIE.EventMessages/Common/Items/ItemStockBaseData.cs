using SIE.WMS.Common;
using System;

namespace SIE.EventMessages.Common.Items
{
    /// <summary>
    /// 库存资料基本信息
    /// </summary>
    [Serializable]
    public class ItemStockBaseData
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 是否批次
        /// </summary>
        public bool? IsBatch { get; set; }

        /// <summary>
        /// 批次方案Id
        /// </summary>
        public double? ItemBatchProgramId { get; set; }

        /// <summary>
        /// 是否序列号管理
        /// </summary>
        public bool IsSerialNumber { get; set; }

        /// <summary>
        /// 序列号管理模式
        /// </summary>
        public SerialModel SerialModel { get; set; }
    }
}
