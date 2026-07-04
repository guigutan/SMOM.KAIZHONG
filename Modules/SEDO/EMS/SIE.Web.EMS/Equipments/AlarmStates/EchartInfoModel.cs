using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.Equipments.AlarmStates
{
    /// <summary>
    /// EChart数据模型
    /// </summary>
    [Serializable]
    public class EchartInfoModel
    {
        /// <summary>
        /// X 轴
        /// </summary>
        public List<DateTime> XaxisList { get; set; }

        /// <summary>
        /// 数据系列列表
        /// </summary>
        public List<ChartSerie> ChartSeries { get; set; }
    }

    /// <summary>
    /// 数据系统
    /// </summary>
    [Serializable]
    public class ChartSerie
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据值列表
        /// </summary>
        public List<decimal> DataValues { get; set; }
    }
}
