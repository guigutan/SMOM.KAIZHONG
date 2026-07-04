using SIE.ControlChart.SpcUtils;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ControlChart.ChartDatas.SpcMRCharts
{
    /// <summary>
    /// 中值数据处理
    /// </summary>
    [Serializable]
    public class MRData : ChartData
    {
        /// <summary>
        /// 中值计算
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
            UpChartAvg = Datas.Select(d => d.Median).Average();
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
            //中值图控制线
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
                    double A5 = 3.7599 / Math.Sqrt(SubGroupSize);
                    double calValue = A5 * GroupStdDev;
                    UpChartControlLine.Cl = UpChartAvg;
                    UpChartControlLine.Ucl = UpChartAvg + calValue;
                    UpChartControlLine.Lcl = UpChartAvg - calValue;
                }
                else
                {
                    UpChartControlLine.Cl = UpChartAvg;
                    if (SubGroupSize >= ChartConst.MeA2.Length)
                        throw new ValidationException("找不到子组容量{0}对应的参数常量".L10nFormat(SubGroupSize));
                    UpChartControlLine.Ucl = UpChartAvg + DownChartAvg * ChartConst.MeA2[SubGroupSize].Value;
                    UpChartControlLine.Lcl = UpChartAvg - DownChartAvg * ChartConst.MeA2[SubGroupSize].Value;
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
                //极差图控制线
                if (StdEstimateMode == EstimateMode.CombinedStdDeviation)
                {
                    double calValue = 3 * GroupStdDev * ChartConst.D3Nd[SubGroupSize].Value;
                    DownChartControlLine.Cl = ChartConst.D2Nd[SubGroupSize].Value * GroupStdDev; ;
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
