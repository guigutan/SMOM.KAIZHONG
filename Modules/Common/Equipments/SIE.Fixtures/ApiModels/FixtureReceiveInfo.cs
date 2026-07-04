using System;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 工治具领用清单信息
    /// </summary>
    [Serializable]
    public class FixtureReceiveInfo
    {
        /// <summary>
        /// 需求单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 产线
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 类型数量
        /// </summary>
        public int TypeNum { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
