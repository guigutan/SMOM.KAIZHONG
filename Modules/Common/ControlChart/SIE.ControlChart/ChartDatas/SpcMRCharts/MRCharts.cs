using SIE.ControlChart.ChartDatas.SpcLineCharts;
using SIE.ControlChart.Common;
using SIE.ControlChart.Extension;
using SIE.ControlChart.SpcUtils;
using SIE.ControlChart.SpcWarningRull;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ControlChart.ChartDatas.SpcMRCharts
{
    /// <summary>
    /// 中值极差控制图
    /// </summary>
    [Serializable]
    public class MRCharts : SpcSampleGroupCharts
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
        /// 中值极差图构造函数
        /// </summary>
        public MRCharts()
        {
            ShowRChart = true;
            ShowIndex = true;
            Chart2Title = "R-Chart";
            Chart1Title = "M-Chart";
            Type = SpcChartType.Me_R;
        }

        /// <summary>
        /// 获取绘图数据
        /// </summary>
        public override void GetDrawData()
        {
            ChartsData = new MRData();
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
            ChartsData.SubGroupSize = SubGroupSize;
            ChartsData.ChartConst = ChartConst;
            ChartsData.Calculate();
            if (Digits.HasValue)
                _Digits = Digits.Value;
            else
                _Digits = ChartsData.Digits;
            //_Digits = Math.Max(_Digits, 2); 根据设置的精度要求来显示去掉最小两位

            GetSampleGroupDatasBySubGroupCount();

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
            GetDrawXChartInfo();
        }

        /// <summary>
        /// 获取绘制X图信息
        /// </summary>
        public void GetDrawXChartInfo()
        {
            #region 设置数据点
            //封装数据点 
            foreach (SampleGroup sg in SampleGroupDatas)
            {
                sg.Value = sg.Median;
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
        /// 自定义均值、标准差 设置控制限
        /// </summary>
        public override void SetDefineControlLineByCustomStdDeviation(double meanValue, double standardDeviation, int subGroupSize)
        {
            //找不到参数，不处理，顾问要求
            if (ChartConst.D2Nd.Length < subGroupSize + 1 || ChartConst.D2Nd[subGroupSize] == null) return;
            if (ChartConst.D1.Length < subGroupSize + 1 || ChartConst.D1[subGroupSize] == null) return;
            if (ChartConst.D2.Length < subGroupSize + 1 || ChartConst.D2[subGroupSize] == null) return;


            //计算上图控制限
            DefineUpChartControlLine = new ControlLine();
            DefineUpChartControlLine.Cl = meanValue;
            DefineUpChartControlLine.Ucl = DefineUpChartControlLine.Cl + standardDeviation * 3;
            DefineUpChartControlLine.Lcl = DefineUpChartControlLine.Cl - standardDeviation * 3;

            //计算下图控制限
            if (ChartConst.D2Nd.Length < subGroupSize + 1)
                throw new ValidationException("统计系数表中n={0}的d₂参数未维护".L10nFormat(subGroupSize));
            if (ChartConst.D1.Length < subGroupSize + 1)
                throw new ValidationException("统计系数表中n={0}的D1参数未维护".L10nFormat(subGroupSize));
            if (ChartConst.D2.Length < subGroupSize + 1)
                throw new ValidationException("统计系数表中n={0}的D2参数未维护".L10nFormat(subGroupSize));

            double d2 = ChartConst.D2Nd[subGroupSize].Value;
            double D1 = ChartConst.D1[subGroupSize].Value;
            double D2 = ChartConst.D2[subGroupSize].Value;

            DefineDownChartControlLine = new ControlLine();
            DefineDownChartControlLine.Cl = d2 * standardDeviation;
            DefineDownChartControlLine.Ucl = D2 * standardDeviation;
            DefineDownChartControlLine.Lcl = D1 * standardDeviation;


        }




    }
}
