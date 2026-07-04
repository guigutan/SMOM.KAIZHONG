using System;
using System.Linq;

namespace SIE.ControlChart.SpcUtils
{
    /// <summary>
    /// 超出规格样本数
    /// </summary>
    [Serializable]
    public class ExceedRuleSample : DataPoint
    {
        /// <summary>
        /// 抽样时间
        /// </summary>
        public DateTime? SamplingTime { get; set; }

        /// <summary>
        /// 样本组名称
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
        /// 平均值
        /// </summary>
        public double Avg { get; set; }

        /// <summary>
        /// 极差值
        /// </summary>
        public double R { get; set; }

        /// <summary>
        /// 中值
        /// </summary>
        public double Median { get; set; }

        private double[] _Datas;
        /// <summary>
        /// 样本组数据
        /// </summary>
        public double[] Datas
        {
            get
            {
                return _Datas;
            }
            set
            {
                _Datas = value;
                _Datas = _Datas ?? new double[0];
                R = _Datas.Max() - _Datas.Min();
                Avg = _Datas.Average();
                GetMedian();
            }
        }

        /// <summary>
        /// 计算中值
        /// </summary>
        private void GetMedian()
        {
            var ds = _Datas.OrderBy(d => d).ToArray();
            if (ds.Count() % 2 == 0)
                Median = (ds[ds.Count() / 2 - 1] + ds[(ds.Count() + 2) / 2 - 1]) / 2;
            else
                Median = ds[(ds.Count() + 1) / 2 - 1];
        }
    }
}
