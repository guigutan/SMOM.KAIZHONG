using SIE.ControlChart.SpcUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Domain.Validation;

namespace SIE.ControlChart.ChartDatas.SpcXBarRCharts
{
    /// <summary>
    /// XBAR-R图数据类
    /// </summary>
    [Serializable]
    public class XBarRData : ChartData
    {
        /// <summary>
        /// XBAR-R图计算
        /// </summary>
        public override void Calculate()
        {
            if (AllDatas.IsNullOrEmpty())
                throw new ValidationException("没有数据无法计算".L10N());
            if (Datas.IsNullOrEmpty())
                throw new ValidationException("没有数据无法计算".L10N());

            //所有样本数值
            AllSample = AllDatas.SelectMany(c => c.Datas).ToList();
            //上下图均值
            UpChartAvg = Datas.Select(d => d.Avg).Average();
            DownChartAvg = Datas.Select(d => d.R).Average();
            //单个样本标准差
            Datas.ForEach(t => t.Std = CalculateUtil.CalculateStdDev(t.Datas));
            //标准差（整体）
            List<double> _samlpe = Datas.SelectMany(c => c.Datas).ToList();
            StdDev = CalculateUtil.CalculateStdDev(_samlpe);
            //估值方式
            GroupStdDev = CalculateUtil.CalculateStdMethod(StdEstimateMode, Datas, SubGroupSize, ChartConst);
            //控制限
            CalculateControlLine();
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
                    UpChartControlLine.Ucl = UpChartAvg + DownChartAvg * ChartConst.A2[SubGroupSize].Value;
                    UpChartControlLine.Lcl = UpChartAvg - DownChartAvg * ChartConst.A2[SubGroupSize].Value;
                }

            }


            //极差图控制线
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
                    double calValue = 3 * GroupStdDev * ChartConst.D3Nd[SubGroupSize].Value;
                    DownChartControlLine.Cl = ChartConst.D2Nd[SubGroupSize].Value * GroupStdDev;
                    DownChartControlLine.Ucl = DownChartControlLine.Cl + calValue;
                    DownChartControlLine.Lcl = DownChartControlLine.Cl - calValue;
                }
                else
                {
                    DownChartControlLine.Cl = DownChartAvg;
                    DownChartControlLine.Ucl = DownChartAvg * (ChartConst.D4[SubGroupSize].HasValue ? Convert.ToDouble(ChartConst.D4[SubGroupSize].Value) : 0);
                    DownChartControlLine.Lcl = DownChartAvg * (ChartConst.D3[SubGroupSize].HasValue ? Convert.ToDouble(ChartConst.D3[SubGroupSize].Value) : 0);
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
