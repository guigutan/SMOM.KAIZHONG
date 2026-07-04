using DocumentFormat.OpenXml.Presentation;
using SIE.ControlChart.ChartDatas.SpcLineCharts;
using SIE.ControlChart.Common;
using SIE.ControlChart.Extension;
using SIE.ControlChart.SpcUtils;
using SIE.ControlChart.SpcWarningRull;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ControlChart.ChartDatas.SpcIMRCharts
{
    /// <summary>
    /// I-MR图
    /// </summary>
    [Serializable]
    public class ImrCharts : SpcSampleGroupCharts
    {
        /// <summary>
        /// 是否实时监控
        /// </summary>
        public bool IsRta { get; set; }
        /// <summary>
        /// 实时监控项目名称
        /// </summary>
        public string RtaItemName { get; set; }
        /// <summary>
        /// 是否显示移动极差图
        /// </summary>
        public bool ShowMRChart { get; set; }
        /// <summary>
        /// 是否显示CA指数
        /// </summary>
        public bool ShowCa { get; set; }

        /// <summary>
        /// I-MR图构造函数
        /// </summary>
        public ImrCharts()
        {
            ShowIndex = true;
            ShowMRChart = true;
            Chart2Title = "MR-Chart";
            Chart1Title = "I-Chart";
            Type = SpcChartType.I_MR;
        }

        /// <summary>
        /// 获取绘图需要的数据
        /// </summary>
        public override void GetDrawData()
        {
            //计算数据
            ChartsData = new ImrData();
            ChartsData.StdEstimateMode = StdEstimateMode;
            ChartsData.DefineUpChartControlLine = DefineUpChartControlLine;
            ChartsData.DefineDownChartControlLine = DefineDownChartControlLine;
            ChartsData.AllDatas = AllSampleGroupDatas;
            ChartsData.Datas = SampleGroupDatas;
            ChartsData.Lsl = Lsl;
            ChartsData.Usl = Usl;
            ChartsData.CpkHigh = Convert.ToDouble(CpkHigh);
            ChartsData.CpkMid = Convert.ToDouble(CpkMid);
            ChartsData.CpkLow = Convert.ToDouble(CpkLow);
            ChartsData.IsdisplayCpk = IsdisplayCpk;
            ChartsData.SubGroupCount = SubGroupCount;
            ChartsData.ChartConst = ChartConst;
            ChartsData.Calculate();
            if (Digits.HasValue)
                _Digits = Digits.Value;
            else
                _Digits = ChartsData.Digits;
            //_Digits = Math.Max(_Digits, 2); 根据设置的精度要求来显示去掉最小两位

            //设置预警信息
            SetRullsParams();

            //计算CPK等级
            if (ChartsData.Cpk >= ChartsData.CpkHigh)
            {
                ChartsData.CPKGrade = "优秀".L10N();
            }
            else if (ChartsData.Cpk >= ChartsData.CpkMid && ChartsData.Cpk < ChartsData.CpkHigh)
            {
                ChartsData.CPKGrade = "充足".L10N();
            }
            else if (ChartsData.Cpk >= ChartsData.CpkLow && ChartsData.Cpk < ChartsData.CpkMid)
            {
                ChartsData.CPKGrade = "一般".L10N();
            }
            else
            {
                ChartsData.CPKGrade = "不足".L10N();
            }

            //返回要显示的数据分组
            GetSampleGroupDatasBySubGroupCount();

            //设置预警信息
            SetRullsParams();

            GetDrawXChartInfo();

        }

        /// <summary>
        /// 绘制均值图表
        /// </summary>
        private void GetDrawXChartInfo()
        {
            #region 设置数据点
            foreach (SampleGroup sg in SampleGroupDatas)
            {
                sg.Value = sg.Avg;
                sg.R = sg.R;
            }
            #region 判定数据点是否达到预警
            List<ViolateSpcRullEvent> violateRulls = new List<ViolateSpcRullEvent>();

            foreach (SpcRull rull in Rulls)
            {
                rull.Datas = SampleGroupDatas.ToList<DataPoint>();
                rull.GetDrawInfo();
                violateRulls.AddRange(rull.GetDrawInfo());
            }
            if (violateRulls.IsNotEmpty())
            {
                RullWarningInfos = violateRulls;
            }
            #endregion
            #endregion;
            //如果当前绘图中没有异常点，触发事件
            doUpChartNoWarnning();
        }

        /// <summary>
        /// 设置规则警示参数
        /// </summary>
        protected new void SetRullsParams()
        {
            ShowAreaLineForRule = false;
            foreach (SpcRull rull in Rulls)
            {
                if (rull is SpcKeepDownOfCLRull)
                {
                    (rull as SpcKeepDownOfCLRull).Cl = ChartsData.UpChartControlLine.Cl;
                }
                else if (rull is SpcKeepUpOfCLRull)
                {
                    (rull as SpcKeepUpOfCLRull).Cl = ChartsData.UpChartControlLine.Cl;
                }
                else if (rull is SpcKeepSideInCRull)
                {
                    ShowAreaLineForRule = true;
                    var rule = rull as SpcKeepSideInCRull;
                    rule.Ucb = ChartsData.UpChartControlLine.Ucb;
                    rule.Lcb = ChartsData.UpChartControlLine.Lcb;
                }
                else if (rull is SpcKeepSideOutCRull)
                {
                    ShowAreaLineForRule = true;
                    var rule = rull as SpcKeepSideOutCRull;
                    rule.Ucb = ChartsData.UpChartControlLine.Ucb;
                    rule.Lcb = ChartsData.UpChartControlLine.Lcb;
                }
                else if (rull is SpcKeepSideOutDownARull)
                {
                    ShowAreaLineForRule = true;
                    (rull as SpcKeepSideOutDownARull).Lcl = ChartsData.UpChartControlLine.Lcl;
                }
                else if (rull is SpcKeepSideOutUpARull)
                {
                    ShowAreaLineForRule = true;
                    (rull as SpcKeepSideOutUpARull).Ucl = ChartsData.UpChartControlLine.Ucl;
                }
                else if (rull is SpcKeepOneSideOutCRull)
                {
                    ShowAreaLineForRule = true;
                    var rule = rull as SpcKeepOneSideOutCRull;
                    rule.Ucb = ChartsData.UpChartControlLine.Ucb;
                    rule.Lcb = ChartsData.UpChartControlLine.Lcb;
                }
                else if (rull is SpcKeepOneSideOutBRull)
                {
                    ShowAreaLineForRule = true;
                    var rule = rull as SpcKeepOneSideOutBRull;
                    rule.Uba = ChartsData.UpChartControlLine.Uba;
                    rule.Lba = ChartsData.UpChartControlLine.Lba;
                }
                else if (rull is SpcOverValueRull)
                {
                    ShowAreaLineForRule = true;
                    var rule = rull as SpcOverValueRull;
                    rule.CL = ChartsData.UpChartControlLine.Cl;
                    rule.UCL = ChartsData.UpChartControlLine.Ucl;
                    rule.LCL = ChartsData.UpChartControlLine.Lcl;
                    rule.rUCL = ChartsData.DownChartControlLine.Ucl;
                    rule.UpStd = ChartsData.UpChartControlLine.Ucb - ChartsData.UpChartControlLine.Cl;
                    rule.DownStd = ChartsData.UpChartControlLine.Cl - ChartsData.UpChartControlLine.Lcb;
                }
                else if (rull is SpcOverLslRull)
                {
                    (rull as SpcOverLslRull).Lsl = ChartsData.UpChartControlLine.Lsl;
                }
                else if (rull is SpcOverUslRull)
                {
                    (rull as SpcOverUslRull).Usl = ChartsData.UpChartControlLine.Usl;
                }
            }
        }


        /// <summary>
        /// 自定义均值、标准差 设置控制限
        /// </summary>
        public override void SetDefineControlLineByCustomStdDeviation(double meanValue, double standardDeviation, int subGroupSize)
        {
            //计算上图控制限
            DefineUpChartControlLine = new ControlLine();
            DefineUpChartControlLine.Cl = meanValue;
            DefineUpChartControlLine.Ucl = DefineUpChartControlLine.Cl + standardDeviation * 3;
            DefineUpChartControlLine.Lcl = DefineUpChartControlLine.Cl - standardDeviation * 3;


            //计算下图控制限
            DefineDownChartControlLine = new ControlLine();
            DefineDownChartControlLine.Cl = 1.128 * standardDeviation;
            DefineDownChartControlLine.Ucl = 3.686 * standardDeviation;
            DefineDownChartControlLine.Lcl = 0;

        }

    }
}
