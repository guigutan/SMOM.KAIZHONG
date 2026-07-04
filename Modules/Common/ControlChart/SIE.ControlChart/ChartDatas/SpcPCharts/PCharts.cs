using SIE.ControlChart.ChartDatas.SpcLineCharts;
using SIE.ControlChart.Common;
using SIE.ControlChart.SpcUtils;
using SIE.ControlChart.SpcWarningRull;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ControlChart.ChartDatas.SpcPCharts
{
    /// <summary>
    /// 不合格品率控制图
    /// </summary>
    [Serializable]
    public class PCharts : SpcNgCharts
    {
        /// <summary>
        /// 不合格品率控制图构造函数
        /// </summary>
        public PCharts()
        {
            Title = "P-Chart";
            Type = SpcChartType.P;
        }

        /// <summary>
        /// 获取绘图数据
        /// </summary>
        public override void GetDrawData()
        {
            //计算数据
            ChartsData = new PData();
            ChartsData.AllDatas = AllSampleDatas;
            ChartsData.Datas = SampleDatas;
            (ChartsData as PData).SubGroupSize = SubGroupSize;
            ChartsData.Calculate();
            if (DefineControlLine != null)
            {
                ChartsData.ControlLine.Lcl = DefineControlLine.Lcl;
                ChartsData.ControlLine.Ucl = DefineControlLine.Ucl;
                ChartsData.ControlLine.Cl = DefineControlLine.Cl;
            }
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
            GetDrawXChartInfo();
        }

        /// <summary>
        /// 绘制均值图表
        /// </summary>
        public void GetDrawXChartInfo()
        {
            #region 设置数据点
            foreach (NgSample sg in SampleDatas)
            {
                sg.Value = sg.U;
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

            doUpChartNoWarnning();
        }
    }
}
