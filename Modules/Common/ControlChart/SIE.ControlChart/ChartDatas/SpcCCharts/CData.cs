using SIE.ControlChart.SpcUtils;
using SIE.Domain.Validation;
using System;
using System.Linq;

namespace SIE.ControlChart.ChartDatas.SpcCCharts
{
    /// <summary>
    /// C图数据处理
    /// </summary>
    [Serializable]
    public class CData : CountingChartData
    {
        /// <summary>
        /// C图计算
        /// </summary>
        public override void Calculate()
        {
            if (Datas.IsNullOrEmpty())
                throw new ValidationException("没有数据无法计算".L10N());

            if (DefineControlLine != null)
            {
                if (DefineControlLine.Cl != double.MinValue && DefineControlLine.Ucl == double.MinValue && DefineControlLine.Lcl == double.MinValue)
                {
                    //只设定均值，Ucl、Lcl按公式算
                    Avg = Math.Max(0, DefineControlLine.Cl);
                    ControlLine.Cl = Avg;
                    ControlLine.Ucl = GetUcl(Avg);
                    ControlLine.Lcl = GetLcl(Avg);
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
                Avg = Datas.Select(d => d.NgQty).Average();
                //均值图控制线
                ControlLine.Cl = Avg;
                ControlLine.Ucl = GetUcl(Avg);
                ControlLine.Lcl = GetLcl(Avg);
            }
            Digits = 3;

        }

        private double GetUcl(double avg)
        {
            return avg + 3 * Math.Sqrt(avg);
        }

        private double GetLcl(double avg)
        {
            return Math.Max(0, avg - 3 * Math.Sqrt(avg));
        }
    }
}
