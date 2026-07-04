using SIE.ControlChart.ChartDatas.SpcLineCharts;
using SIE.ControlChart.Common;
using SIE.ControlChart.SpcUtils;
using SIE.ControlChart.SpcWarningRull;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ControlChart.ChartDatas.SpcCCharts
{
    /// <summary>
    /// 缺陷数控制图
    /// </summary>
    [Serializable]
    public class CCharts : SpcNgCharts
    {
        /// <summary>
        /// 缺陷数控制图构造函数
        /// </summary>
        public CCharts()
        {
            Title = "C-Chart";
            Type = SpcChartType.C;
        }

        /// <summary>
        /// 绘制
        /// </summary>
        public override void GetDrawData()
        {
            //计算数据
            ChartsData = new CData();
            ChartsData.Datas = SampleDatas;
            ChartsData.DefineControlLine = DefineControlLine;//自定义控制线
            ChartsData.Calculate();
            if (Digits.HasValue)
                _Digits = Digits.Value;
            else
                _Digits = ChartsData.Digits;
            _Digits = Math.Max(_Digits, 2);

            var count = SubGroupCount.HasValue ? SubGroupCount.Value : SampleDatas.Count;
            if (SampleDatas.Count > count)
                SampleDatas = SampleDatas.Skip(SampleDatas.Count - count).ToList();

            //设置预警信息
            SetRullsParams();
            DrawCChart();
        }

        /// <summary>
        /// 绘制均值图表
        /// </summary>
        private void DrawCChart()
        {
            #region 设置数据点
            foreach (NgSample sg in SampleDatas)
            {
                sg.Value = sg.NgQty;
            }
            #region 判定数据点是否达到预警
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
