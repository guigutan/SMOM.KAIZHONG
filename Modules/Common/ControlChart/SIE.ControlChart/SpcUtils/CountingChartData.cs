using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SIE.ControlChart.SpcUtils
{
    /// <summary>
    /// 控制图数据
    /// </summary>
    [Serializable]
    public class CountingChartData : ChartDataBase
    {
        /// <summary>
        /// 自定义控制线
        /// </summary>
        public ControlLine DefineControlLine { get; set; }

        /// <summary>
        /// 所有样本数据
        /// </summary>
        [JsonIgnore]
        public List<NgSample> AllDatas { get; set; }
        /// <summary>
        /// 子组数量样本数据
        /// </summary>
        public List<NgSample> Datas { get; set; }
        /// <summary>
        /// 均值图平均数
        /// </summary>
        public double Avg { get; set; }

        /// <summary>
        /// 控制线
        /// </summary>
        public ControlLine ControlLine { get; set; } = new ControlLine();

        /// <summary>
        /// 计算
        /// </summary>
        public virtual void Calculate()
        {

        }
    }
}
