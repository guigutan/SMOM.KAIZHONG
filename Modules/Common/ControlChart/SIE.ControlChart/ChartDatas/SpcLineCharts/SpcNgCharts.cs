using Newtonsoft.Json;
using SIE.ControlChart.SpcUtils;
using SIE.ControlChart.SpcWarningRull;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ControlChart.ChartDatas.SpcLineCharts
{
    /// <summary>
    /// 缺陷类型控制图基类
    /// </summary>
    [Serializable]
    public class SpcNgCharts : SpcChartBase
    {
        /// <summary>
        /// 缺陷类型控制图基类
        /// </summary>
        public SpcNgCharts()
        {
            Rulls = new List<SpcRull>();
            AutoGroupName = true;
        }

        /// <summary>
        /// 控制图数据
        /// </summary>
        public CountingChartData ChartsData { get; set; }

        /// <summary>
        /// 因规则显示ABC区
        /// </summary>
        protected bool ShowAreaLineForRule;

        /// <summary>
        /// 判异常规则
        /// </summary>
        [JsonIgnore]
        public List<SpcRull> Rulls { get; set; }
        /// <summary>
        /// 自定义控制线
        /// </summary>
        [JsonIgnore]
        public ControlLine DefineControlLine { get; set; }
        /// <summary>
        /// 数据计算精确度 
        /// </summary>
        protected int _Digits = 2;
        /// <summary>
        /// 数据计算精确度 
        /// </summary>
        public int? Digits { get; set; }
        /// <summary>
        /// 子组容量
        /// </summary>
        public int SubGroupSize { get; set; }
        /// <summary>
        /// 是否自动显示分组名称（1，2，3，4，5...)
        /// </summary>
        public bool AutoGroupName { get; set; }
        /// <summary>
        /// 样本组数据
        /// </summary>
        [JsonIgnore]
        public List<NgSample> AllSampleDatas { get; set; } = new List<NgSample>();

        /// <summary>
        /// 子组数量样本组数据
        /// </summary>
        [JsonIgnore]
        public List<NgSample> SampleDatas { get; set; } = new List<NgSample>();

        /// <summary>
        /// 是否在图表浮动信息中显示抽样时间
        /// </summary>
        public bool ShowSampleTimeInDataInfo { get; set; }
        /// <summary>
        /// 子组数量
        /// </summary>
        public int? SubGroupCount { get; set; }
        /// <summary>
        /// 控制图标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 获取绘制控制图数据
        /// </summary>
        public override void GetDrawData()
        {

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
                    (rull as SpcKeepDownOfCLRull).Cl = ChartsData.ControlLine.Cl;
                }
                else if (rull is SpcKeepUpOfCLRull)
                {
                    (rull as SpcKeepUpOfCLRull).Cl = ChartsData.ControlLine.Cl;
                }
                else if (rull is SpcKeepSideInCRull)
                {
                    ShowAreaLineForRule = true;
                    var rule = rull as SpcKeepSideInCRull;
                    rule.Ucb = ChartsData.ControlLine.Ucb;
                    rule.Lcb = ChartsData.ControlLine.Lcb;
                }
                else if (rull is SpcKeepSideOutCRull)
                {
                    ShowAreaLineForRule = true;
                    var rule = rull as SpcKeepSideOutCRull;
                    rule.Ucb = ChartsData.ControlLine.Ucb;
                    rule.Lcb = ChartsData.ControlLine.Lcb;
                }
                else if (rull is SpcKeepSideOutDownARull)
                {
                    ShowAreaLineForRule = true;
                    (rull as SpcKeepSideOutDownARull).Lcl = ChartsData.ControlLine.Lcl;
                }
                else if (rull is SpcKeepSideOutUpARull)
                {
                    ShowAreaLineForRule = true;
                    (rull as SpcKeepSideOutUpARull).Ucl = ChartsData.ControlLine.Ucl;
                }
                else if (rull is SpcKeepOneSideOutCRull)
                {
                    ShowAreaLineForRule = true;
                    var rule = rull as SpcKeepOneSideOutCRull;
                    rule.Ucb = ChartsData.ControlLine.Ucb;
                    rule.Lcb = ChartsData.ControlLine.Lcb;
                }
                else if (rull is SpcKeepOneSideOutBRull)
                {
                    ShowAreaLineForRule = true;
                    var rule = rull as SpcKeepOneSideOutBRull;
                    rule.Uba = ChartsData.ControlLine.Uba;
                    rule.Lba = ChartsData.ControlLine.Lba;
                }
                else if (rull is SpcOverValueRull)
                {
                    ShowAreaLineForRule = true;
                    var rule = rull as SpcOverValueRull;
                    rule.CL = ChartsData.ControlLine.Cl;
                    rule.UCL = ChartsData.ControlLine.Ucl;
                    rule.LCL = ChartsData.ControlLine.Lcl;
                    rule.UpStd = ChartsData.ControlLine.Ucb - ChartsData.ControlLine.Cl;
                    rule.DownStd = ChartsData.ControlLine.Cl - ChartsData.ControlLine.Lcb;
                }
            }
        }


        /// <summary>
        /// 获取chart显示的数据
        /// </summary>
        protected void GetNgSampleDatasBySubGroupCount()
        {
            //返回要显示的数据分组
            var count = SubGroupCount.HasValue ? SubGroupCount.Value : SampleDatas.Count;
            if (SampleDatas.Count > count && ChartMaximum == 0)
            {
                SampleDatas = SampleDatas.Skip(SampleDatas.Count - count).ToList();
                XAreaStartLocal = SampleDatas.Count - count;
            }
            if (ChartMaximum != 0)
            {
                if (SampleDatas.Count > ChartMaximum)
                {
                    int lNum = (int)(MouseLocal - Math.Ceiling(ChartMaximum / 2));
                    if (lNum > 0)
                    {
                        var takeNum = ChartMaximum + lNum;
                        takeNum = takeNum > SampleDatas.Count ? SampleDatas.Count : ChartMaximum;
                        SampleDatas = SampleDatas.Skip(lNum).Take((int)takeNum).ToList();
                        XAreaStartLocal = lNum + 1;
                    }
                    else
                    {
                        SampleDatas = SampleDatas.Take((int)ChartMaximum).ToList();
                        XAreaStartLocal = 0;
                    }
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
                List<NgSample> groups = SampleDatas;
                foreach (NgSample d in groups)
                {
                    if (d.Number <= this.PointIndex) continue;
                    this.PointIndex = d.Number;
                    //计算预警点位置信息-用作稳定控制限
                    if (!d.IsWarnPoint && d.Warnings.Count == 0)
                    {
                        NoWarnPointSize++;
                    }
                    else
                        NoWarnPointSize = 0;
                    if (NoWarnPointSize >= StableValue)
                    {
                        // 稳定控制限
                        UpChartNoWarnning(null, null);
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
                StableControlLine = chart.ChartsData.ControlLine;
                chart.DefineControlLine = StableControlLine;
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
