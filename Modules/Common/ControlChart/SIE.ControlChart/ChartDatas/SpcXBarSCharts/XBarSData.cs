using DocumentFormat.OpenXml.Drawing.Diagrams;
using SIE.ControlChart.SpcUtils;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ControlChart.ChartDatas.SpcXBarSCharts
{

    /// <summary>
    /// 均值标准差控制图数据类
    /// </summary>
    [Serializable]
    public class XBarSData : ChartData
    {
        /// <summary>
        /// 均值标准差控制图计算
        /// </summary>
        public override void Calculate()
        {
            if (SubGroupSize <= 0)
                throw new ValidationException("子组容量必须大于0".L10N());
            if (AllDatas.IsNullOrEmpty())
                throw new ValidationException("没有数据无法计算".L10N());
            if (Datas.IsNullOrEmpty())
                throw new ValidationException("没有数据无法计算".L10N());

            //所有样本数值
            AllSample = AllDatas.SelectMany(c => c.Datas).ToList();
            //上下图均值
            UpChartAvg = Datas.Select(d => d.Avg).Average();
            Datas.ForEach(t => t.Std = CalculateUtil.CalculateStdDev(t.Datas));
            DownChartAvg = Datas.Select(d => d.Std).Average();
            //单个样本标准差
            Datas.ForEach(t => t.Std = CalculateUtil.CalculateStdDev(t.Datas));
            //标准差（整体）
            List<double> _samlpe = Datas.SelectMany(c => c.Datas).ToList();
            StdDev = CalculateUtil.CalculateStdDev(_samlpe);
            //估值方式
            GroupStdDev = CalculateUtil.CalculateStdMethod(StdEstimateMode, Datas, SubGroupSize, ChartConst);
            //控制限
            this.CalculateControlLine();
            //计算各指数
            double _XAvg = Datas.Select(d => d.Avg).Average();
            CalculateExponent(_XAvg);
            //计算PPM
            CalculatePPM();

        }


        /// <summary>
        /// 计算控制限
        /// </summary>
        private void CalculateControlLine()
        {
            //上图控制限
            //均值图控制线
            if (DefineUpChartControlLine != null)
            {
                UpChartControlLine = DefineUpChartControlLine;
                if (DefineUpChartControlLine.Cl == double.MinValue)
                    UpChartControlLine.Cl = UpChartAvg;
            }
            else
            {
                if (StdEstimateMode == EstimateMode.CombinedStdDeviation)
                {
                    UpChartControlLine.Cl = UpChartAvg;
                    double calValue = 3 * GroupStdDev / Math.Sqrt(SubGroupSize);
                    UpChartControlLine.Ucl = UpChartAvg + calValue;
                    UpChartControlLine.Lcl = UpChartAvg - calValue;
                }
                else
                {
                    UpChartControlLine.Cl = UpChartAvg;
                    UpChartControlLine.Ucl = UpChartAvg + DownChartAvg * ChartConst.A3[SubGroupSize].Value;
                    UpChartControlLine.Lcl = UpChartAvg - DownChartAvg * ChartConst.A3[SubGroupSize].Value;
                }
            }
            if (DefineDownChartControlLine != null)
            {
                DownChartControlLine = DefineDownChartControlLine;
                if (DefineDownChartControlLine.Cl == double.MinValue)
                    DownChartControlLine.Cl = DownChartAvg;
            }
            else
            {
                if (StdEstimateMode == EstimateMode.CombinedStdDeviation)
                {
                    double C4 = ChartConst.C4[SubGroupSize].Value;
                    double calValue = 3 * GroupStdDev * Math.Sqrt((1 - C4 * C4));
                    DownChartControlLine.Cl = C4 * GroupStdDev;
                    DownChartControlLine.Ucl = DownChartControlLine.Cl + calValue;
                    DownChartControlLine.Lcl = DownChartControlLine.Cl - calValue;
                }
                else
                {
                    DownChartControlLine.Cl = DownChartAvg;
                    DownChartControlLine.Ucl = DownChartAvg * (ChartConst.B4[SubGroupSize].HasValue ? Convert.ToDouble(ChartConst.B4[SubGroupSize].Value) : 0);
                    DownChartControlLine.Lcl = DownChartAvg * (ChartConst.B3[SubGroupSize].HasValue ? Convert.ToDouble(ChartConst.B3[SubGroupSize].Value) : 0);
                }
            }
            if (DownChartControlLine.Lcl < 0)
                DownChartControlLine.Lcl = 0;
            if (Lsl.HasValue)
                UpChartControlLine.Lsl = Lsl;
            if (Usl.HasValue)
                UpChartControlLine.Usl = Usl;

        }
    }
}
