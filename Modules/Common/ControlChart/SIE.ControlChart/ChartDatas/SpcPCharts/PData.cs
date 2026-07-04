using SIE.ControlChart.Extension;
using SIE.ControlChart.SpcUtils;
using SIE.Domain.Validation;
using System;
using System.Linq;

namespace SIE.ControlChart.ChartDatas.SpcPCharts
{
    /// <summary>
    /// P图数据
    /// </summary>
    [Serializable]
    public class PData : CountingChartData
    {
        /// <summary>
        /// 子组容量
        /// </summary>
        public int SubGroupSize { get; set; }

        /// <summary>
        /// P图计算
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
                if (d.NgQty > SubGroupSize)
                    throw new ValidationException("不合格数不能大于子组容量".L10N());
            });

            Avg = Datas.Select(d => d.NgQty).Sum().ToDouble() / (Datas.Count * SubGroupSize).ToDouble();
            if (DefineControlLine != null
                && DefineControlLine.Cl != double.MinValue)
            {
                Avg = DefineControlLine.Cl;
                DefineControlLine = null;
            }
            if (DefineControlLine != null && DefineControlLine.Ucl != double.MinValue && DefineControlLine.Lcl != double.MinValue)
            {
                ControlLine.Ucl = Math.Min(1, DefineControlLine.Ucl);
                ControlLine.Lcl = Math.Max(0, DefineControlLine.Lcl);
                ControlLine.Cl = ControlLine.Lcl + (ControlLine.Ucl - ControlLine.Lcl) / 2;
            }
            else
            {
                //均值图控制线
                ControlLine.Cl = Avg;
                ControlLine.Ucl = Math.Min(1, Avg + 3 * Math.Sqrt(Avg * (1 - Avg) / SubGroupSize));
                ControlLine.Lcl = Math.Max(0, Avg - 3 * Math.Sqrt(Avg * (1 - Avg) / SubGroupSize));
            }
            Digits = max_d + 3;

        }
    }
}
