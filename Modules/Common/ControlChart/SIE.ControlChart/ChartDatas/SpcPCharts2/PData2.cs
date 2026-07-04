using SIE.ControlChart.Extension;
using SIE.ControlChart.SpcUtils;
using SIE.Domain.Validation;
using System;
using System.Linq;

namespace SIE.ControlChart.ChartDatas.SpcPCharts2
{
    /// <summary>
    /// P图数据处理
    /// </summary>
    [Serializable]
    public class PData2 : CountingChartData
    {
        /// <summary>
        /// P图计算
        /// </summary>
        public override void Calculate()
        {
            if (AllDatas.IsNullOrEmpty())
                throw new ValidationException("没有数据无法计算".L10N());
            if (Datas.IsNullOrEmpty())
                throw new ValidationException("没有数据无法计算".L10N());

            var hasCustomControlLine = DefineControlLine != null;
            var hasCustomCI = hasCustomControlLine && DefineControlLine.Cl != double.MinValue
                && DefineControlLine.Ucl == double.MinValue && DefineControlLine.Lcl == double.MinValue;
            var hasCustomUclLcl = hasCustomControlLine && DefineControlLine.Cl == double.MinValue
                && DefineControlLine.Ucl != double.MinValue && DefineControlLine.Lcl != double.MinValue;
            var hasCustomAll = hasCustomControlLine && DefineControlLine.Cl != double.MinValue
                && DefineControlLine.Ucl != double.MinValue && DefineControlLine.Lcl != double.MinValue;

            var sumNgQty = Datas.Select(d => d.NgQty).Sum().ToDouble();
            var sumSampleQty = Datas.Select(d => d.SampleQty).Sum().ToDouble();
            Datas.ForEach(d =>
            {
                if (d.NgQty > d.SampleQty)
                    throw new ValidationException("不合格数不能大于样本数".L10N());
                if (d.SampleQty <= 0)
                    throw new ValidationException("样本数必须大于0".L10N());
                d.U = d.NgQty.ToDouble() / d.SampleQty;

                ControlLine cl = new ControlLine();
                if (hasCustomControlLine)
                {
                    if (hasCustomCI)
                    {
                        Avg = DefineControlLine.Cl;
                        cl.Cl = Avg;
                        cl.Ucl = GetUcl(Avg, d.SampleQty);
                        cl.Lcl = GetLcl(Avg, d.SampleQty);
                    }
                    else if (hasCustomUclLcl)
                    {
                        cl.Ucl = Math.Min(1, Math.Max(0, DefineControlLine.Ucl));
                        cl.Lcl = Math.Max(0, DefineControlLine.Lcl);
                        cl.Cl = cl.Lcl + (cl.Ucl - cl.Lcl) / 2;
                    }
                    else if (hasCustomAll)
                    {
                        cl.Ucl = Math.Min(1, Math.Max(0, DefineControlLine.Ucl));
                        cl.Lcl = Math.Max(0, DefineControlLine.Lcl);
                        cl.Cl = Math.Max(0, DefineControlLine.Cl);
                    }
                }
                else
                {
                    Avg = sumNgQty / sumSampleQty;
                    cl.Cl = Avg;
                    cl.Ucl = GetUcl(Avg, d.SampleQty);
                    cl.Lcl = GetLcl(Avg, d.SampleQty);
                }
                d.ControlLine = cl;
            });

            Digits = 3;

        }

        private double GetUcl(double avg, int sampleQty)
        {
            return Math.Min(1, avg + 3 * Math.Sqrt(avg * (1 - avg) / sampleQty.ToDouble()));
        }

        private double GetLcl(double avg, int sampleQty)
        {
            return Math.Max(0, avg - 3 * Math.Sqrt(avg * (1 - avg) / sampleQty.ToDouble()));
        }
    }
}
