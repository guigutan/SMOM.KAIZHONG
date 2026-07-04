using SIE.Core.Enums;
using System;

namespace SIE.Fixtures.FixtureDemands.ViewModels
{
    /// <summary>
    /// 回传工单信息
    /// </summary>

    [Serializable]
    public class BindWoInfo
    {
        /// <summary>
        /// 产品
        /// </summary>
        public double ProductId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 工段ID
        /// </summary>
        public double SegmentId { get; set; }

        /// <summary>
        /// 工艺面
        /// </summary>
        public Deck? Desk { get; set; }

        /// <summary>
        /// 工单当前工段
        /// </summary>
        public double? ProcessSegmentId { get; set; }

        /// <summary>
        /// 工段
        /// </summary>
        public string ProcessSegment_Display { get; set; }

        /// <summary>
        /// 计划时间
        /// </summary>
        public DateTime? PlanDateTime { get; set; }
    }
}
