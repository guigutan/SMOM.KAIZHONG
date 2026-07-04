using System;

namespace SIE.Kit.APS.FactoryConfirms
{ /// <summary>
  /// 返回产能数据
  /// </summary>
    [Serializable]
    public class OutputCapacityInfo
    {

        /// <summary>
        /// 产能数量
        /// </summary>
        public decimal CapacityQty { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime HourDate { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 工厂ID
        /// </summary>
        public double FactoryId { get; set; }

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName { get; set; }

        /// <summary>
        /// 负载面积
        /// </summary>
        public decimal LoadArea { get; set; }

    }
}
