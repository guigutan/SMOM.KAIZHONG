using SIE.ControlChart.ChartDatas.SpcLineCharts;
using SIE.ControlChart.Common;
using SIE.ControlChart.SpcUtils;
using SIE.ControlChart.SpcWarningRull;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SIE.ControlChart.ChartDatas.SpcUCharts
{
    /// <summary>
    /// 缺陷率控制图
    /// </summary>
    [Serializable]
    public class UCharts : SpcNgCharts
    {
        /// <summary>
        /// U图构造函数
        /// </summary>
        public UCharts()
        {
            Title = "U-Chart";
            Type = SpcChartType.U;
        }
        /// <summary>
        /// 是否已绘制
        /// </summary>
        private bool Drawed { get; set; }

        /// <summary>
        /// 获取绘图数据
        /// </summary>
        public override void GetDrawData()
        {
            //计算数据
            ChartsData = new UData();
            ChartsData.AllDatas = AllSampleDatas;
            ChartsData.Datas = SampleDatas;
            ChartsData.DefineControlLine = DefineControlLine;//自定义控制线
            (ChartsData as UData).SubGroupSize = SubGroupSize;
            ChartsData.Calculate();
            ChartsData.ControlLine.Cl = SampleDatas.FirstOrDefault().ControlLine.Cl;
            if (Digits.HasValue)
                _Digits = Digits.Value;
            else
                _Digits = ChartsData.Digits;
            _Digits = Math.Max(_Digits, 2);

            var count = SubGroupCount.HasValue ? SubGroupCount.Value : SampleDatas.Count;
            if (SampleDatas.Count > count)
                SampleDatas = SampleDatas.Skip(SampleDatas.Count - count).ToList();
            Drawed = true;

            //设置预警信息
            SetRullsParams();
            foreach (var rull in Rulls)
            {
                var rule = rull as SpcOverValueRull;
                if (null != rule)
                    rule.SameSampleQty = false;//非恒定样本数
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
            foreach (NgSample sg in SampleDatas)
            {
                sg.Value = sg.NP;
            }
            #region 设置警示信息(判定数据点是否达到预警)
            //设置警示信息
            List<ViolateSpcRullEvent> violateRulls = new List<ViolateSpcRullEvent>();

            foreach (SpcRull rull in Rulls)
            {
                rull.Datas = SampleDatas.ToList<DataPoint>();
                violateRulls.AddRange(rull.GetDrawInfo());
            }
            if (violateRulls.IsNotEmpty())
            {
                RullWarningInfos = violateRulls;
            }
            #endregion

            #endregion;
        }
    }
}
