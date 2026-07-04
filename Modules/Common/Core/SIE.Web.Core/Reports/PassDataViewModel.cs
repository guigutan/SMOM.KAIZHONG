using SIE.Domain;
using SIE.ObjectModel;
using SIE.Web.Json;
using System;
using System.Collections.Generic;

namespace SIE.Web.Core.Reports
{
    /// <summary>
    /// 报表数据视图模型
    /// </summary>
    [Label("报表数据")]
    public class PassDataViewModel : ViewModel
    {
        #region 合格数 PassData
        /// <summary>
        /// 合格数
        /// </summary>
        [Label("合格数")]
        public static readonly Property<decimal> PassDataProperty = P<PassDataViewModel>.Register(e => e.PassData);

        /// <summary>
        /// 合格数
        /// </summary>
        public decimal PassData
        {
            get { return this.GetProperty(PassDataProperty); }
            set { this.SetProperty(PassDataProperty, value); }
        }
        #endregion

        #region 日期 Date
        /// <summary>
        /// 日期
        /// </summary>
        [Label("日期")]
        public static readonly Property<DateTime> DateProperty = P<PassDataViewModel>.Register(e => e.Date);

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date
        {
            get { return this.GetProperty(DateProperty); }
            set { this.SetProperty(DateProperty, value); }
        }
        #endregion

        #region 报表属性 ReportInfo
        /// <summary>
        /// 报表属性
        /// </summary>
        [Label("报表属性")]
        public static readonly Property<string> ReportInfoProperty = P<PassDataViewModel>.Register(e => e.ReportInfo);

        /// <summary>
        /// 报表属性
        /// </summary>
        public string ReportInfo
        {
            get { return this.GetProperty(ReportInfoProperty); }
            set { this.SetProperty(ReportInfoProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 报表
    /// </summary>
    [Serializable]
    public class ReportViewModel
    {
        /// <summary>
        /// 合格率数据集合
        /// </summary>
        public List<PassDataViewModel> PassDataList { get; } = new List<PassDataViewModel>();

        /// <summary>
        /// 图表数据
        /// </summary>
        public List<EntityJson> ChartJsonData { get; } = new List<EntityJson>();
    }
}
