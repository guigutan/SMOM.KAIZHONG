using SIE.ControlChart.SpcUtils;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ControlChart.ChartDatas.SpcIMRCharts
{
    /// <summary>
    /// I-MR图数据处理
    /// </summary>
    [Serializable]
    public class ImrData : ChartData
    {
        /// <summary>
        /// I-MR图计算
        /// </summary>
        public override void Calculate()
        {
            if (AllDatas.IsNullOrEmpty())
                throw new ValidationException("没有数据无法计算".L10N());
            if (Datas.IsNullOrEmpty())
                throw new ValidationException("没有数据无法计算".L10N());

            //所有样本数值
            AllSample = AllDatas.SelectMany(c => c.Datas).ToList();
            //计算极差值
            for (int i = 1; i < Datas.Count; i++)
                Datas[i].R = Math.Abs(Datas[i].Avg - Datas[i - 1].Avg);
            //上下图均值
            UpChartAvg = Datas.Select(d => d.Avg).Average();
            if (Datas.Count > 1)
                DownChartAvg = Datas.Skip(1).Select(d => d.R).Average();
            else
                DownChartAvg = 0;
            //单个样本标准差
            Datas.ForEach(t => t.Std = CalculateUtil.CalculateStdDev(t.Datas));
            //标准差（整体）
            List<double> _samlpe = Datas.SelectMany(c => c.Datas).ToList();
            StdDev = CalculateUtil.CalculateStdDev(_samlpe);
            //估值方式
            GroupStdDev = CalculateUtil.CalculateStdMethod(StdEstimateMode, Datas, 1, ChartConst);
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

            if (DefineUpChartControlLine != null)
            {
                UpChartControlLine = DefineUpChartControlLine;
                if (DefineUpChartControlLine.Cl == double.MinValue)
                    UpChartControlLine.Cl = UpChartAvg;
            }
            else
            {
                if (StdEstimateMode == EstimateMode.IMedianEstimateStdDeviation)
                {
                    UpChartControlLine.Cl = UpChartAvg;
                    UpChartControlLine.Ucl = UpChartAvg + 3 * GroupStdDev;
                    UpChartControlLine.Lcl = UpChartAvg - 3 * GroupStdDev;
                }
                else
                {
                    UpChartControlLine.Cl = UpChartAvg;
                    UpChartControlLine.Ucl = UpChartAvg + DownChartAvg * ChartConst.E2[2].Value;
                    UpChartControlLine.Lcl = UpChartAvg - DownChartAvg * ChartConst.E2[2].Value;
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
                if (StdEstimateMode == EstimateMode.IMedianEstimateStdDeviation)
                {
                    double calValue = 3 * GroupStdDev * ChartConst.D3Nd[2].Value;
                    DownChartControlLine.Cl = ChartConst.D2Nd[2].Value * GroupStdDev;
                    DownChartControlLine.Ucl = DownChartControlLine.Cl + calValue;
                    DownChartControlLine.Lcl = DownChartControlLine.Cl - calValue;
                }
                else
                {
                    DownChartControlLine.Cl = DownChartAvg;
                    DownChartControlLine.Ucl = DownChartAvg * (ChartConst.D4[2].HasValue ? Convert.ToDouble(ChartConst.D4[2].Value) : 0);
                    DownChartControlLine.Lcl = DownChartAvg * (ChartConst.D3[2].HasValue ? Convert.ToDouble(ChartConst.D3[2].Value) : 0);
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
