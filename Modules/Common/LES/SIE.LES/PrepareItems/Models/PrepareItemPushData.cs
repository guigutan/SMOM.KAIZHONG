using SIE.LES.Commons;
using System;

namespace SIE.LES.PrepareItems.Models
{
    /// <summary>
    /// 备料需求数据
    /// </summary>
    [Serializable]
    public class PrepareItemPushData
    {
        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
		/// 触发方式
		/// </summary>
		public TriggerMode TriggerType { get; set; }

        /// <summary>
		/// 需求计算方式
		/// </summary>
		public DemandMode DemandType { get; set; }

        /// <summary>
		/// 固定量
		/// </summary>
		public decimal? FixedQuantity { get; set; }

        /// <summary>
		/// 是否手动接收物料
		/// </summary>
		public bool IsManualReception { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double WipResourceId { get; set; }

        /// <summary>
        /// 提前小时
        /// </summary>
        public decimal? AdvanceHours { get; set; }

        /// <summary>
        /// 最短满足时间（小时）
        /// </summary>
        public decimal? SatisfactionTime { get; set; }

        /// <summary>
        /// 最小备料时间（小时)
        /// </summary>
        public decimal? PreparationTime { get; set; }
    }
}
