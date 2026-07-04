using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Workbench.KeyPerformances.ProductFpy
{
    /// <summary>
    /// 产线统计数据视图
    /// </summary>
    public class LineDataViewModel : ViewModel
    {
        #region 产线Id LineId
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线Id")]
        public static readonly Property<double> LineIdProperty = P<LineDataViewModel>.Register(e => e.LineId);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double LineId
        {
            get { return this.GetProperty(LineIdProperty); }
            set { this.SetProperty(LineIdProperty, value); }
        }
        #endregion

        #region 产线名称 LineName
        /// <summary>
        /// 产线名称
        /// </summary>
        [Label("产线名称")]
        public static readonly Property<string> LineNameProperty = P<LineDataViewModel>.Register(e => e.LineName);

        /// <summary>
        /// 产线名称
        /// </summary>
        public string LineName
        {
            get { return this.GetProperty(LineNameProperty); }
            set { this.SetProperty(LineNameProperty, value); }
        }
        #endregion

        #region 统计日期 Date
        /// <summary>
        /// 统计日期
        /// </summary>
        [Label("统计日期")]
        public static readonly Property<DateTime> DateProperty = P<LineDataViewModel>.Register(e => e.Date);

        /// <summary>
        /// 统计日期
        /// </summary>
        public DateTime Date
        {
            get { return this.GetProperty(DateProperty); }
            set { this.SetProperty(DateProperty, value); }
        }
        #endregion

        #region 直通率 Percentage
        /// <summary>
        /// 直通率
        /// </summary>
        [Label("直通率")]
        public static readonly Property<double> PercentageProperty = P<LineDataViewModel>.Register(e => e.Percentage);

        /// <summary>
        /// 直通率
        /// </summary>
        public double Percentage
        {
            get { return this.GetProperty(PercentageProperty); }
            set { this.SetProperty(PercentageProperty, value); }
        }
        #endregion

    }
}
