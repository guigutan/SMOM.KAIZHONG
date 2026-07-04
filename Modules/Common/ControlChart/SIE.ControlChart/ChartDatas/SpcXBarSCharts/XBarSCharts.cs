using SIE.ControlChart.ChartDatas.SpcLineCharts;
using SIE.ControlChart.Common;
using SIE.ControlChart.Extension;
using SIE.ControlChart.SpcUtils;
using SIE.ControlChart.SpcWarningRull;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ControlChart.ChartDatas.SpcXBarSCharts
{
    [Serializable]
    /// <summary>
    /// 均值标准差控制图
    /// </summary>
    public class XBarSCharts : SpcSampleGroupCharts
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
        /// 是否显示极差图
        /// </summary>
        public bool ShowRChart { get; set; }

        /// <summary>
        /// 均值标准差控制图构造函数
        /// </summary>
        public XBarSCharts()
        {
            ShowRChart = true;
            ShowIndex = true;
            Chart1Title = "XBar-Chart";
            Chart2Title = "S-Chart";
            Type = SpcChartType.Xbar_S;
        }

        /// <summary>
        /// 均值标准差控制图获取绘制数据
        /// </summary>
        public override void GetDrawData()
        {
            ChartsData = new XBarSData();
            ChartsData.StdEstimateMode = StdEstimateMode;
            ChartsData.DefineUpChartControlLine = DefineUpChartControlLine;
            ChartsData.DefineDownChartControlLine = DefineDownChartControlLine;
            ChartsData.AllDatas = AllSampleGroupDatas;
            ChartsData.Datas = SampleGroupDatas;
            ChartsData.SubGroupSize = SubGroupSize;
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

            GetSampleGroupDatasBySubGroupCount();

            SetRullsParamsAsXbarS();
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
            GetDrawXChartInfo();
        }

        /// <summary>
        /// 获取绘制X控制图信息
        /// </summary>
        public void GetDrawXChartInfo()
        {
            #region 设置数据点
            //封装数据点
            foreach (SampleGroup sg in SampleGroupDatas)
            {
                sg.Value = sg.Avg;
            }
            #region 判定数据点是否达到预警
            List<ViolateSpcRullEvent> violateRulls = new List<ViolateSpcRullEvent>();

            foreach (SpcRull rull in Rulls)
            {
                rull.Datas = SampleGroupDatas.ToList<DataPoint>();
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
        protected void SetRullsParamsAsXbarS()
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
            if (ChartConst.A.Length < subGroupSize + 1 || ChartConst.A[subGroupSize] == null) return;
            if (ChartConst.C4.Length < subGroupSize + 1 || ChartConst.C4[subGroupSize] == null) return;
            if (ChartConst.B6.Length < subGroupSize + 1) return;
            if (ChartConst.B5.Length < subGroupSize + 1 || ChartConst.B5[subGroupSize] == null) return;


            //计算上图控制限

            double A = ChartConst.A[subGroupSize].Value;

            DefineUpChartControlLine = new ControlLine();
            DefineUpChartControlLine.Cl = meanValue;
            DefineUpChartControlLine.Ucl = DefineUpChartControlLine.Cl + standardDeviation * A;
            DefineUpChartControlLine.Lcl = DefineUpChartControlLine.Cl - standardDeviation * A;

            //计算下图控制限
     

            double C4 = ChartConst.C4[subGroupSize].Value;
            double B6 = ChartConst.B6[subGroupSize].Value;
            double B5 = ChartConst.B5[subGroupSize].Value;

            DefineDownChartControlLine = new ControlLine();
            DefineDownChartControlLine.Cl = C4 * standardDeviation;
            DefineDownChartControlLine.Ucl = B6 * standardDeviation;
            DefineDownChartControlLine.Lcl = B5 * standardDeviation;

        }

    }
}
