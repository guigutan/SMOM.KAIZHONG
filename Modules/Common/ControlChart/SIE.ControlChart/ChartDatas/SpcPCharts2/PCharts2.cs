using SIE.ControlChart.ChartDatas.SpcLineCharts;
using SIE.ControlChart.Common;
using SIE.ControlChart.SpcUtils;
using SIE.ControlChart.SpcWarningRull;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ControlChart.ChartDatas.SpcPCharts2
{
    /// <summary>
    /// 不合格品率控制图
    /// </summary>
    [Serializable]
    public class PCharts2 : SpcNgCharts
    {
        /// <summary>
        /// 不合格品率控制图构造函数
        /// </summary>
        public PCharts2()
        {
            Title = "P-Chart";
            Type = SpcChartType.P;
        }

        /// <summary>
        /// 获取绘图数据
        /// </summary>
        public override void GetDrawData()
        {
            ChartsData = new PData2();
            ChartsData.AllDatas = AllSampleDatas;
            ChartsData.Datas = SampleDatas;
            ChartsData.DefineControlLine = DefineControlLine;//自定义控制线
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
        /// 绘制均值图表
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
            #endregion
        }
    }
}
