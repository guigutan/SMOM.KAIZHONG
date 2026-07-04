using System;
using System.Linq;

namespace SIE.ControlChart.SpcUtils
{
    /// <summary>
    /// 样本组类
    /// </summary>
    [Serializable]
    public class SampleGroup : DataPoint
    {
        /// <summary>
        /// 样本组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 平均值
        /// </summary>
        public double Avg { get; set; }
        /// <summary>
        /// 极差值
        /// </summary>
        public double R { get; set; }
        /// <summary>
        /// 标准差
        /// </summary>
        public double Std { get; set; }
        /// <summary>
        /// 中值
        /// </summary>
        public double Median { get; set; }

        /// <summary>
        /// CPK
        /// </summary>
        public double? Cpk { get; set; }

        /// <summary>
        /// PPK
        /// </summary>
        public double? Ppk { get; set; }
        /// <summary>
        /// CP
        /// </summary>
        public double? Cp { get; set; }


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
                if (_Datas.IsNotEmpty())
                {
                    R = _Datas.Max() - _Datas.Min();
                    Avg = _Datas.Average();
                    GetMedian();
                }
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
