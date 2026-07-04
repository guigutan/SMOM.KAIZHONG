using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Workbench.KeyPerformances.Commons
{
    /// <summary>
    /// 图表数据结构
    /// </summary>
    [Serializable]
    public class BaseValueViewModel : ViewModel
    {
        #region 日期 Date
        /// <summary>
        /// 日期
        /// </summary>
        [Label("日期")]
        public static readonly Property<DateTime> DateProperty = P<BaseValueViewModel>.Register(e => e.Date);

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date
        {
            get { return this.GetProperty(DateProperty); }
            set { this.SetProperty(DateProperty, value); }
        }
        #endregion

        #region 效率 Efficiency
        /// <summary>
        /// 效率
        /// </summary>
        [Label("效率")]
        public static readonly Property<double> EfficiencyProperty = P<BaseValueViewModel>.Register(e => e.Efficiency);

        /// <summary>
        /// 效率
        /// </summary>
        public double Efficiency
        {
            get { return this.GetProperty(EfficiencyProperty); }
            set { this.SetProperty(EfficiencyProperty, value); }
        }
        #endregion
    }
}
