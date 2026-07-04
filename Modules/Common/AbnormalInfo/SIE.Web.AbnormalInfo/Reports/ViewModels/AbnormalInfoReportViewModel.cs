using SIE.Domain;
using SIE.AbnormalInfo.Reports;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Web.Json;
using System;
using System.Collections.Generic;

namespace SIE.Web.AbnormalInfo.Reports.ViewModels
{
    /// <summary>
    /// 异常信息报表
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(AbnormalInfoReportCriteria))]
    [Label("异常信息报表")]
    public class AbnormalInfoReportViewModel : ViewModel
    {

    }

    /// <summary>
    /// 异常信息关闭数数据
    /// </summary>
    [Serializable]
    public class ProcessDataViewModel : ViewModel
    {
        #region 关闭数 CloseData
        /// <summary>
        /// 关闭数
        /// </summary>
        [Label("关闭数")]
        public static readonly Property<decimal> PassDataProperty = P<ProcessDataViewModel>.Register(e => e.CloseData);

        /// <summary>
        /// 关闭数
        /// </summary>
        public decimal CloseData
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
        public static readonly Property<DateTime> DateProperty = P<ProcessDataViewModel>.Register(e => e.Date);

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
        public static readonly Property<string> ReportInfoProperty = P<ProcessDataViewModel>.Register(e => e.ReportInfo);

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
    /// 异常信息报表查询返回前端数据
    /// </summary>
    [Serializable]
    public class ReportViewModel
    {
        /// <summary>
        /// 异常信息数据
        /// </summary>
        public List<ProcessDataViewModel> ProcessDataList { get; } = new List<ProcessDataViewModel>();

        /// <summary>
        /// 图表数据
        /// </summary>
        public List<EntityJson> ChartJsonData { get; } = new List<EntityJson>();
    }
}
