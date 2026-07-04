using SIE.ControlChart.SpcUtils;
using System;
using System.Collections.Generic;

namespace SIE.ControlChart.SpcWarningRull
{
    /// <summary>
    /// Spc判异规则-一个点大于或小于n个标准差 
    /// </summary>
    [Serializable]
    public class SpcOverValueRull : SpcRull
    {
        /// <summary>
        /// 上控制线
        /// </summary>
        public double UCL { get; set; }
        /// <summary>
        /// 下控制线
        /// </summary>
        public double LCL { get; set; }
        /// <summary>
        /// 中线
        /// </summary>
        public double CL { get; set; }
        /// <summary>
        /// 上标准线
        /// </summary>
        public double UpStd { get; set; }
        /// <summary>
        /// 下标准线
        /// </summary>
        public double DownStd { get; set; }
        /// <summary>
        /// 非恒定样本数
        /// </summary>
        public bool SameSampleQty { get; set; } = true;
        /// <summary>
        /// 下图上控制线
        /// </summary>
        public double? rUCL { get; set; }
        /// <summary>
        /// 一个点大于或小于n个标准差构造函数
        /// </summary>
        /// <param name="n"></param>
        public SpcOverValueRull(int? n = null)
        {
            RullDescription = "1个点，距离中心线距离大于{0}个标准差".L10nFormat(n);
            if (n.HasValue)
                N = n.Value;
        }
        /// <summary>
        /// 获取绘制控制图信息
        /// </summary>
        public override List<ViolateSpcRullEvent> GetDrawInfo()
        {
            var violateRullEvents = new List<ViolateSpcRullEvent>();
            if (SameSampleQty)
            {
                for (var i = 0; i < Datas.Count; i++)
                {
                    if (Datas[i].Value > GetUpBorder() || Datas[i].Value < GetDownBorder())
                    {
                        Datas[i].Warnings.Add(GetSimple());
                        Datas[i].IsWarnPoint = true;
                        violateRullEvents.Add(new ViolateSpcRullEvent(RullDescription, N, new List<double>() { Datas[i].Value }, Datas[i].SamplingTime));
                    }
                    if (rUCL != null)
                    {
                        //只有XBarR、I-MR、ME-R有rUCL
                        violateRullEvents =  RangeExceedsUclRule(((SampleGroup)Datas[i]).R, Datas[i], violateRullEvents);
                    }

                }
            }
            else
            {
                for (var i = 0; i < Datas.Count; i++)
                {
                    var sample = Datas[i] as NgSample;
                    DownStd = sample.ControlLine.Cl - sample.ControlLine.Lcb;
                    UpStd = sample.ControlLine.Ucb - sample.ControlLine.Cl;
                    UCL = sample.ControlLine.Ucl;
                    LCL = sample.ControlLine.Lcl;
                    CL = sample.ControlLine.Cl;
                    if (Datas[i].Value > GetUpBorder() || Datas[i].Value < GetDownBorder())
                    {
                        Datas[i].Warnings.Add(GetSimple());
                        Datas[i].IsWarnPoint = true;
                        violateRullEvents.Add(new ViolateSpcRullEvent(RullDescription, N, new List<double>() { Datas[i].Value }, Datas[i].SamplingTime));
                    }
                }
            }
            return violateRullEvents;
        }

        private double GetUpBorder()
        {
            if (N == 3)
                return UCL;
            else
                return N * UpStd + CL;
        }

        private double GetDownBorder()
        {
            if (N == 3)
                return LCL;
            else
                return CL - N * DownStd;
        }

        /// <summary>
        /// 判断数据是否触发判异规则
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public override List<ViolateSpcRullEvent> JudgeViolationRull(List<DataPoint> datas)
        {
            var violateRullEvents = new List<ViolateSpcRullEvent>();
            if (null == datas || datas.Count == 0)
                return violateRullEvents;
            if (SameSampleQty)
            {
                for (var i = 0; i < datas.Count; i++)
                {
                    if (datas[i].Value > GetUpBorder() || datas[i].Value < GetDownBorder())
                    {
                        violateRullEvents.Add(new ViolateSpcRullEvent(RullDescription, N, new List<double>() { datas[i].Value }, Datas[i].SamplingTime));
                    }
                    if (rUCL != null && ((SampleGroup)datas[i]).R > rUCL)
                    {
                        violateRullEvents.Add(new ViolateSpcRullEvent("R值超出上控制限", null, new List<double>() { datas[i].Value }, Datas[i].SamplingTime));
                    }
                }
            }
            else
            {
                for (var i = 0; i < datas.Count; i++)
                {
                    var sample = datas[i] as NgSample;
                    DownStd = sample.ControlLine.Cl - sample.ControlLine.Lcb;
                    UpStd = sample.ControlLine.Ucb - sample.ControlLine.Cl;
                    UCL = sample.ControlLine.Ucl;
                    LCL = sample.ControlLine.Lcl;
                    CL = sample.ControlLine.Cl;
                    if (datas[i].Value > GetUpBorder() || datas[i].Value < GetDownBorder())
                    {
                        violateRullEvents.Add(new ViolateSpcRullEvent(RullDescription, N, new List<double>() { datas[i].Value }, Datas[i].SamplingTime));
                    }
                }
            }
            return violateRullEvents;
        }

        /// <summary>
        ///  获取绘制R图信息
        /// </summary>
        /// <param name="R"></param>
        /// <param name="data"></param>
        /// <param name="violateRullEvents"></param>
        /// <returns></returns>
        public List<ViolateSpcRullEvent> RangeExceedsUclRule(double R , DataPoint data, List<ViolateSpcRullEvent> violateRullEvents) {
            if (R > rUCL)
            {
                data.Warnings.Add(GetSimple());
                data.IsWarnPointRchart = true;
                data.IsWarnLineRchart = true;
                violateRullEvents.Add(new ViolateSpcRullEvent("R值超出上控制限", null, new List<double>() { data.Value }, data.SamplingTime));
            }
            return violateRullEvents;
        }
    }
}
