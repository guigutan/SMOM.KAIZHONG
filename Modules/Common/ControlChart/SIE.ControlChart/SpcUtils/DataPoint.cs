using Newtonsoft.Json;
using SIE.ControlChart.SpcWarningRull;
using System;
using System.Collections.Generic;

namespace SIE.ControlChart.SpcUtils
{
    /// <summary>
    /// 数据点信息
    /// </summary>
    [Serializable]
    public class DataPoint
    {
        #region Point信息

        /// <summary>
        /// 抽样时间
        /// </summary>
        public DateTime? SamplingTime { get; set; }

        /// <summary>
        /// 数据时间
        /// </summary>
        public DateTime? DataTime { get; set; }

        /// <summary>
        /// 警告信息
        /// </summary>
        [JsonIgnore]
        public List<SimpleSpcRull> Warnings { get; set; } = new List<SimpleSpcRull>();
        /// <summary>
        /// 数值
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// 是否预警点
        /// </summary>
        public bool IsWarnPoint { get; set; }

        /// <summary>
        /// 是否预警点
        /// </summary>
        public bool IsWarnPointRchart { get; set; }
        /// <summary>
        /// 是否预警点连线
        /// </summary>
        public bool IsWarnLine { get; set; }
        /// <summary>
        /// 是否预警点连线
        /// </summary>
        public bool IsWarnLineRchart { get; set; }
        /// <summary>
        /// 是否是剔除点
        /// </summary>
        public bool IsRejectPoint { get; set; }
        /// <summary>
        /// 样本原始数据(原始来自DataRow的数据)
        /// </summary>
        [JsonIgnore]
        public List<Dictionary<object, object>> Source { get; set; }

        /// <summary>
        /// 点位置
        /// </summary>
        public int Number { get; set; }
        #endregion
    }
}
