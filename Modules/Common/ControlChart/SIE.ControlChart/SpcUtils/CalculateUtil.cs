using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ControlChart.SpcUtils
{
    /// <summary>
    /// 计算帮助类
    /// </summary>
    public static class CalculateUtil
    {
        #region 标准差估算

        /// <summary>
        /// 计算组内标准差(subgroupsize为1时计算为单值移动的组内标准差)
        /// </summary>
        /// <param name="datas">样本组数据集合</param>
        /// <param name="subgroupsize">样本数</param>
        /// <param name="chartConst">控制图参数</param>
        /// <returns></returns>
        public static double CalculateGroupStdDevRBar(List<SampleGroup> datas, int subgroupsize, ChartCalConst chartConst)
        {
            double result = 0;
            if (subgroupsize == 1)
            {
                for (var i = 1; i < datas.Count; i++)
                    result += Math.Abs(datas[i].Avg - datas[i - 1].Avg);
                if (datas.Count <= 1)
                {
                    result = 0;
                }
                else
                {
                    result = result / (datas.Count - 1);
                    result = result / chartConst.D2Nd[2].Value;
                }
            }
            else
            {
                result = datas.Sum(d => d.R) / datas.Count;
                result = result / chartConst.D2Nd[subgroupsize].Value;
            }
            return result;
        }

        /// <summary>
        /// 计算组内标准差
        /// </summary>
        /// <param name="datas">样本组数据集合</param>
        /// <param name="subgroupsize">样本数</param>
        /// <param name="chartConst">控制图参数</param>
        /// <returns></returns>
        public static double CalculateGroupStdDevSBar(List<SampleGroup> datas, int subgroupsize, ChartCalConst chartConst)
        {
            double result = 0;
            if (subgroupsize == 1)
            {
                for (var i = 1; i < datas.Count; i++)
                    result += Math.Abs(datas[i].Avg - datas[i - 1].Avg);
                if (datas.Count <= 1)
                {
                    result = 0;
                }
                else
                {
                    result = result / (datas.Count - 1);
                    result = result / chartConst.D2Nd[2].Value;
                }
            }
            else
            {
                result = datas.Sum(d => CalculateStdDev(d.Datas)) / datas.Count;
                result = result / chartConst.C4[subgroupsize].Value;
            }
            return result;
        }


        /// <summary>
        /// 计算标准差（无偏估计）
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double CalculateStdDev(IEnumerable<double> values)
        {
            double ret = 0;
            if (values != null && values.Any())
            {
                //  计算平均数   
                double avg = values.Average();
                //  计算各数值与平均数的差值的平方，然后求和 
                double sum = values.Sum(d => Math.Pow(d - avg, 2));
                //  除以数量，然后开方
                if (sum != 0)
                    ret = Math.Sqrt(sum / (values.Count() - 1));
                else
                    ret = 0;
            }
            return ret;
        }

        /// <summary>
        /// 计算标准差（有偏估计）
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double CalculateStdDevP(IEnumerable<double> values)
        {
            double ret = 0;
            if (values != null && values.Any())
            {
                //  计算平均数   
                double avg = values.Average();
                //  计算各数值与平均数的差值的平方，然后求和 
                double sum = values.Sum(d => Math.Pow(d - avg, 2));
                //  除以数量，然后开方
                if (sum != 0)
                    ret = Math.Sqrt(sum / (values.Count()));
                else
                    ret = 0;
            }
            return ret;
        }

        /// <summary>
        /// 移动极差中位数估算法
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="chartConst"></param>
        /// <returns></returns>
        public static double CalculateMRStdDev(List<SampleGroup> datas, ChartCalConst chartConst)
        {
            List<double> mr = new List<double>();
            double result = 0;
            for (var i = 1; i < datas.Count; i++)
            {
                mr.Add(Math.Abs(datas[i].Avg - datas[i - 1].Avg));
            }
            if (datas.Count > 1)
            {
                var list = mr.OrderBy(c => c).ToList();
                var n = list.Count;
                if (n % 2 == 0)
                {
                    result = (list[n / 2 - 1] + list[n / 2]) / 2.0;
                }
                else
                {
                    result = list[n / 2];
                }
                result = result / chartConst.D4Nd[2].Value;
            }
            return result;
        }

        #region 合拼标准差

        /// <summary>
        /// 合拼标准差
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="subgroupsize"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static double CalculatePooledStdDev(List<SampleGroup> groups, int subgroupsize)
        {
            int n = groups.Sum(g => g.Datas.Count()); // 总样本数
            int k = groups.Count; // 组数

            double numerator = 0;

            for (int i = 0; i < k; i++)
            {
                var StdDev = CalculateStdDev(groups[i].Datas);
                numerator += (groups[i].Datas.Length - 1) * Math.Pow(StdDev, 2);
            }

            double denominator = n - k;
            int d = (subgroupsize - 1) * k;
            double sp = Math.Sqrt(numerator / denominator);
            var stdDev = sp / CalculateC4(d);
            return stdDev;

        }

        /// <summary>
        /// 计算C4(d+1):无偏常量C4
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double CalculateGamma(double x)
        {
            return SpecialFunctions.Gamma(x);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double CalculateC4(double d)
        {
            var c4 = Math.Sqrt(2.0 / d) * CalculateGamma((d + 1) / 2.0) / CalculateGamma(d / 2.0);
            return c4;
        }

        #endregion


        #endregion

        #region 标准差估算法

        /// <summary>
        /// 标准差估算法
        /// </summary>
        /// <param name="stdEstimateMode">估值方式</param>
        /// <param name="datas">数据集</param>
        /// <param name="subgroupsize">子组数</param>
        /// <param name="chartConst">无偏常量参数</param>
        /// <returns></returns>
        public static double CalculateStdMethod(EstimateMode stdEstimateMode, List<SampleGroup> datas, int subgroupsize, ChartCalConst chartConst)
        {
            double groupStdDev = 0;
            switch (stdEstimateMode)
            {
                case EstimateMode.RBar:
                    groupStdDev = CalculateGroupStdDevRBar(datas, subgroupsize, chartConst);
                    break;
                case EstimateMode.SBar:
                    groupStdDev = CalculateGroupStdDevSBar(datas, subgroupsize, chartConst);
                    break;
                case EstimateMode.IMREstimateStdDeviation:
                    groupStdDev = CalculateGroupStdDevRBar(datas, 1, chartConst);
                    break;
                case EstimateMode.CombinedStdDeviation:
                    groupStdDev = CalculatePooledStdDev(datas, subgroupsize);
                    break;
                case EstimateMode.IMedianEstimateStdDeviation:
                    groupStdDev = CalculateMRStdDev(datas, chartConst);
                    break;

            }
            return groupStdDev;
        }

        #endregion


    }
}
