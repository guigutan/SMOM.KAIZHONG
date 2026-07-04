using Newtonsoft.Json;
using SIE.ControlChart.SpcUtils;
using SIE.ControlChart.SpcWarningRull;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ControlChart.ChartDatas.SpcLineCharts
{
    /// <summary>
    /// 样本组类型控制图基类
    /// </summary>
    [Serializable]
    public class SpcSampleGroupCharts : SpcChartBase
    {
        /// <summary>
        /// 下图标题
        /// </summary>
        public string Chart2Title { get; set; }
        /// <summary>
        /// 上图标题
        /// </summary>
        public string Chart1Title { get; set; }

        /// <summary>
        /// 控制图数据
        /// </summary>
        public ChartData ChartsData { get; set; }

        /// <summary>
        /// 高阈值
        /// </summary>
        public decimal? CpkHigh { get; set; }

        /// <summary>
        /// 中阈值
        /// </summary>
        public decimal? CpkMid { get; set; }

        /// <summary>
        /// 目标下限
        /// </summary> 
        public double? Lsl { get; set; }
        /// <summary>
        /// 目标上限
        /// </summary>
        public double? Usl { get; set; }

        /// <summary>
        /// x轴文本倾斜度
        /// </summary>
        public decimal? LabelRotate { get; set; }

        /// <summary>
        /// 低阈值
        /// </summary>
        public decimal? CpkLow { get; set; }

        /// <summary>
        /// 是否显示CPK等级
        /// </summary>
        public bool IsdisplayCpk { get; set; }

        ///// <summary>
        ///// 是否超规格限预警（标颜色）
        ///// </summary>
        //public bool IsDataForecast { get; set; }

        /// <summary>
        /// 是否在图表浮动信息中显示抽样时间
        /// </summary>
        public bool ShowSampleTimeInDataInfo { get; set; }
        /// <summary>
        /// 抽样时间显示格式
        /// </summary>
        public string SampleTimeFormat { get; set; }
        /// <summary>
        /// 数据计算精确度 
        /// </summary>
        protected int _Digits = 2;
        /// <summary>
        /// 数据计算精确度
        /// </summary>
        public int? Digits { get; set; }
        /// <summary>
        /// 是否显示CPK（默认显示）
        /// </summary>
        public bool ShowIndex { get; set; }
        /// <summary>
        /// 是否绘制规格线
        /// </summary>
        public bool ShowSlLine { get; set; }
        /// <summary>
        /// 子组容量
        /// </summary>
        public int SubGroupSize { get; set; }
        /// <summary>
        /// 子组数量
        /// </summary>
        public int? SubGroupCount { get; set; }
        /// <summary>
        /// 所有样本组数据
        /// </summary>
        [JsonIgnore]
        public List<SampleGroup> AllSampleGroupDatas { get; set; }

        /// <summary>
        /// 子组数量样本组数据
        /// </summary>
        [JsonIgnore]
        public List<SampleGroup> SampleGroupDatas { get; set; }

        /// <summary>
        /// 是否可以选择数据（提供选择框，会触发SelectedData事件）
        /// </summary>
        public bool SelectEnabled { get; set; }
        /// <summary>
        /// 判异常规则
        /// </summary>
        [JsonIgnore]
        public List<SpcRull> Rulls { get; set; }

        /// <summary>
        /// 标准差估值方式
        /// </summary>
        public EstimateMode StdEstimateMode { get; set; } = EstimateMode.RBar;
        /// <summary>
        /// 自定义上图控制线
        /// </summary>
        [JsonIgnore]
        public ControlLine DefineUpChartControlLine { get; set; }
        /// <summary>
        /// 自定义下图控制线
        /// </summary>
        [JsonIgnore]
        public ControlLine DefineDownChartControlLine { get; set; }
        /// <summary>
        /// 因规则显示ABC区
        /// </summary>
        protected bool ShowAreaLineForRule;
        /// <summary>
        /// 是否自动显示分组名称（1，2，3，4，5...)
        /// </summary>
        public bool AutoGroupName { get; set; }

        /// <summary>
        /// 获取绘图需要的数据
        /// </summary>
        public override void GetDrawData()
        {
        }

        /// <summary>
        /// 获取数据计算精确度
        /// </summary>
        /// <returns></returns>
        public int GetRealDigits()
        {
            return _Digits;
        }

        /// <summary>
        /// 设置规则警示参数
        /// </summary>
        protected void SetRullsParams()
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
        /// 一个完整样本组数无预警（上控制图）
        /// </summary>
        protected void doUpChartNoWarnning()
        {

            if (StableControlLine == null && this.IsStableControlLine)
            {
                //无异常执行逻辑
                List<SampleGroup> groups = SampleGroupDatas;
                foreach (SampleGroup d in groups)
                {
                    if (d.Number <= this.PointIndex) continue;
                    this.PointIndex = d.Number;
                    Console.WriteLine("点位"+ this.PointIndex);
                    //计算预警点位置信息-用作稳定控制限
                    if (!d.IsWarnPoint && d.Warnings.Count==0)
                    {
                        NoWarnPointSize++;
                    }
                    else
                        NoWarnPointSize=0;
                    if (NoWarnPointSize >= StableValue)
                    {
                        // 稳定控制限
                        UpChartNoWarnning(null,null);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 稳定控制线
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        protected void UpChartNoWarnning(object o, EventArgs e)
        {
            if (StableControlLine == null)
            {
                var chart = this;
                StableControlLine = chart.ChartsData.UpChartControlLine;
                chart.DefineUpChartControlLine = StableControlLine;
            }
        }

        /// <summary>
        /// 获取chart显示的数据
        /// </summary>
        protected void GetSampleGroupDatasBySubGroupCount()
        {
            //返回要显示的数据分组
            var count = SubGroupCount.HasValue ? SubGroupCount.Value : SampleGroupDatas.Count;
            if (SampleGroupDatas.Count > count && ChartMaximum == 0)
            {
                SampleGroupDatas = SampleGroupDatas.Skip(SampleGroupDatas.Count - count).ToList();
                XAreaStartLocal = SampleGroupDatas.Count - count;
            }
            if (ChartMaximum != 0)
            {
                if (SampleGroupDatas.Count > ChartMaximum)
                {
                    int lNum = (int)(MouseLocal - Math.Ceiling(ChartMaximum / 2));
                    if (lNum > 0)
                    {
                        var takeNum = ChartMaximum + lNum;
                        takeNum = takeNum > SampleGroupDatas.Count ? SampleGroupDatas.Count : ChartMaximum;
                        SampleGroupDatas = SampleGroupDatas.Skip(lNum).Take((int)takeNum).ToList();
                        XAreaStartLocal = lNum + 1;
                    }
                    else
                    {
                        SampleGroupDatas = SampleGroupDatas.Take((int)ChartMaximum).ToList();
                        XAreaStartLocal = 0;
                    }
                }
            }
        }

        /// <summary>
        /// 自定义均值、标准差 设置控制限
        /// </summary>
        public override void SetDefineControlLineByCustomStdDeviation(double meanValue, double standardDeviation, int subGroupSize)
        {

        }
    }
}
