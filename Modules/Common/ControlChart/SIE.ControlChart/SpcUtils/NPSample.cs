using SIE.ControlChart.Extension;
using System;

namespace SIE.ControlChart.SpcUtils
{
    /// <summary>
    /// 缺陷类样本数
    /// </summary>
    [Serializable]
    public class NgSample : DataPoint
    {
        /// <summary>
        /// 组名
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 不合格数
        /// </summary>
        public int NgQty { get; set; }
        /// <summary>
        /// 样本数
        /// </summary>
        public int SampleQty { get; set; }
        /// <summary>
        /// 获取合格率
        /// </summary>
        public double NP
        {
            get
            {
                if (SampleQty == 0)
                    return 0;
                else
                    return NgQty.ToDouble() / SampleQty.ToDouble();
            }
        }

        /// <summary>
        /// 缺陷率
        /// </summary>
        public double U { get; set; }

        /// <summary>
        /// 缺陷数
        /// </summary>
        public double C 
        {
            get 
            {
                if (SampleQty == 0)
                    return 0;
                else
                    return U * SampleQty.ToDouble();
            }
        }

        /// <summary>
        /// 控制线
        /// </summary>
        public ControlLine ControlLine { get; set; }
    }
}
