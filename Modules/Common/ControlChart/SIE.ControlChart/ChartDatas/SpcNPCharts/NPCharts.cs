using SIE.ControlChart.ChartDatas.SpcLineCharts;
using SIE.ControlChart.Common;
using SIE.ControlChart.SpcUtils;
using SIE.ControlChart.SpcWarningRull;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ControlChart.ChartDatas.SpcNPCharts
{
    /// <summary>
    /// 不合格品数控制图
    /// </summary>
    [Serializable]
    public class NPCharts : SpcNgCharts
    {
        /// <summary>
        /// 不合格品数控制图构造函数
        /// </summary>
        public NPCharts()
        {
            Title = "NP-Chart";
            Type = SpcChartType.Np;
        }

        /// <summary>
        /// 获取绘制数据
        /// </summary>
        public override void GetDrawData()
        {
            ChartsData = new NPData();
            ChartsData.AllDatas = AllSampleDatas; 
            ChartsData.Datas = SampleDatas;
            ChartsData.DefineControlLine = DefineControlLine;//自定义控制线
            (ChartsData as NPData).SubGroupSize = SubGroupSize;
            ChartsData.Calculate();
            if (Digits.HasValue)
                _Digits = Digits.Value;
            else
                _Digits = ChartsData.Digits;
            _Digits = Math.Max(_Digits, 2);
            //返回要显示的数据分组
            GetNgSampleDatasBySubGroupCount();
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
            //封装数据点
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

            doUpChartNoWarnning();
        }
    }
}
