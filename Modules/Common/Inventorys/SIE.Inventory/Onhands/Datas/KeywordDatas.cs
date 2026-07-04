using System;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 查询关键字
    /// </summary>
    [Serializable]
    public class KeywordDatas
    {
        /// <summary>
        /// 静态实例化
        /// </summary>
        public static readonly KeywordDatas Empty = new KeywordDatas();

        /// <summary>
        /// 库位编码
        /// </summary>
        public string LocationCode { get; set; }

        /// <summary>
        /// LPN编号
        /// </summary>
        public string LpnCode { get; set; }

        /// <summary>
        /// 货主
        /// </summary>
        public string StorerCode { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 物料编码或物料名称
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 库存状态
        /// </summary>
        public int? OnhandState { get; set; }

        /// <summary>
        /// 库存查询数量类型 1：可用量 2：分配量 3可用量+分配量
        /// </summary>
        public int? QtyType { get; set; } = 1;


        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 库位ID
        /// </summary>
        public double LocationId { get; set; }
    }
}
