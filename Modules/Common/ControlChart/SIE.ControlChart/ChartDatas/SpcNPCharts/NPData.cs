using SIE.ControlChart.Extension;
using SIE.ControlChart.SpcUtils;
using SIE.Domain.Validation;
using System;
using System.Linq;

namespace SIE.ControlChart.ChartDatas.SpcNPCharts
{
    /// <summary>
    /// NP图数据处理
    /// </summary>
    [Serializable]
    public class NPData : CountingChartData
    {
        /// <summary>
        /// 子组容量
        /// </summary>
        public int SubGroupSize { get; set; }

        /// <summary>
        /// NP图计算
        /// </summary>
        public override void Calculate()
        {
            if (AllDatas.IsNullOrEmpty())
                throw new ValidationException("没有数据无法计算".L10N());
            if (Datas.IsNullOrEmpty())
                throw new ValidationException("没有数据无法计算".L10N());

            int max_d = 0;
            Datas.ForEach(d =>
            {
                d.U = d.NgQty.ToDouble() / SubGroupSize;
                max_d = Math.Max(max_d, Math.Abs(d.NgQty.ToString().Length - SubGroupSize.ToString().Length));
            });
            Avg = Datas.Select(d => d.NgQty).Sum().ToDouble() / Datas.Count.ToDouble();
            var n = Datas[0].SampleQty; //被检验的零件数量
            if (DefineControlLine != null)
            {
                if (DefineControlLine.Cl != double.MinValue && DefineControlLine.Ucl == double.MinValue && DefineControlLine.Lcl == double.MinValue)
                {
                    //只设定均值，Ucl、Lcl按公式算
                    //20190312 hc Avg=CI np自定义平均不合格数
                    Avg = Math.Max(0, DefineControlLine.Cl); //此时自定义CI不是真正的CI，而是不合格品率，此时CI=n*不合格品率；最小为0
                    ControlLine.Cl = Avg;
                    ControlLine.Ucl = GetUcl(Avg, n);
                    ControlLine.Lcl = GetLcl(Avg, n);
                }
                else if (DefineControlLine.Cl == double.MinValue && DefineControlLine.Ucl != double.MinValue && DefineControlLine.Lcl != double.MinValue)
                {
                    ControlLine.Ucl = Math.Max(0, DefineControlLine.Ucl);
                    ControlLine.Lcl = Math.Max(0, DefineControlLine.Lcl);
                    ControlLine.Cl = ControlLine.Lcl + (ControlLine.Ucl - ControlLine.Lcl) / 2;
                }
                else if (DefineControlLine.Cl != double.MinValue && DefineControlLine.Ucl != double.MinValue && DefineControlLine.Lcl != double.MinValue)
                {
                    ControlLine.Ucl = Math.Max(0, DefineControlLine.Ucl);
                    ControlLine.Lcl = Math.Max(0, DefineControlLine.Lcl);
                    ControlLine.Cl = Math.Max(0, DefineControlLine.Cl);
                }
            }
            else
            {
                //均值图控制线
                ControlLine.Cl = Avg;
                ControlLine.Ucl = GetUcl(Avg, n);
                ControlLine.Lcl = GetLcl(Avg, n);
            }
            Digits = 3;

        }

        private double GetUcl(double avg, int n)
        {
            return avg + 3 * Math.Sqrt(Avg * (1 - avg / n));
        }

        private double GetLcl(double avg, int n)
        {
            return Math.Max(0, avg - 3 * Math.Sqrt(avg * (1 - avg / n)));
        }
    }
}
