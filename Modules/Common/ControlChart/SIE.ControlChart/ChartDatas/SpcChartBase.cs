using SIE.ControlChart.SpcUtils;
using SIE.ControlChart.SpcWarningRull;
using SIE.Core.QmsStaticConst;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SIE.ControlChart.ChartDatas
{
    /// <summary>
    /// 控制图抽象类
    /// </summary>
    [Serializable]
    public abstract class SpcChartBase
    {
        [JsonIgnore]
        private ChartCalConst _chartCalConst;
        /// <summary>
        /// 控制图参数
        /// </summary>
        [JsonIgnore]
        public ChartCalConst ChartConst
        {
            get {
                return _chartCalConst ?? RT.Service.Resolve<IChartCalConstInitializer>().Initialize(); 
            }
            set { _chartCalConst = value; }
        }

        /// <summary>
        ///  获取绘图需要的数据
        /// </summary>
        public abstract void GetDrawData();

        /// <summary>
        /// 控制图类型
        /// </summary>
        public string Type { get; set; }


        /// <summary>
        /// 稳定控制线
        /// </summary>
        protected ControlLine StableControlLine;

        /// <summary>
        /// chart横轴最大显示数
        /// </summary>
        public decimal ChartMaximum { get; set; }

        /// <summary>
        /// 鼠标滚动事件中，当前拾取的位置
        /// </summary>
        public decimal MouseLocal { get; set; }

        /// <summary>
        /// 横轴的开始位置
        /// </summary>
        public int XAreaStartLocal { get; set; }

        /// <summary>
        /// 判异规则提示信息汇总
        /// </summary>
        public string RullWarnings { get; set; }

        /// <summary>
        /// 判异规则提示信息
        /// </summary>
        [IgnoreProxy]
        public List<ViolateSpcRullEvent> RullWarningInfos { get; set; }

        /// <summary>
        /// 稳定控制限
        /// </summary>
        [IgnoreProxy]
        public bool IsStableControlLine { get; set; }


        /// <summary>
        /// 稳定控制限-子组数
        /// </summary>
        [IgnoreProxy]
        public int StableValue { get; set; }

        /// <summary>
        /// 控制图运行计算定位
        /// </summary>
        [IgnoreProxy]
        public int PointIndex { get; set; }

        /// <summary>
        /// 统计目前连续没有异常点数
        /// </summary>
        [IgnoreProxy]
        public int NoWarnPointSize { get; set; }

        /// <summary>
        /// 自定义均值
        /// </summary>
        /// <param name="meanValue"></param>
        /// <param name="standardDeviation"></param>
        /// <param name="subGroupSize"></param>
        public abstract void SetDefineControlLineByCustomStdDeviation(double meanValue,double standardDeviation,int subGroupSize);

    }
}
