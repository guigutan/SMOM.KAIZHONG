using SIE.Domain;
using SIE.ERPInterface.Common.Enums;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.Logs
{
    /// <summary>
    /// 接口下载异常查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("接口下载异常查询")]
    public partial class DownloadExcViewModelCriteria : Criteria
    {
        #region 任务类型 JobType
        /// <summary>
        /// 任务类型
        /// </summary>
        [Label("任务类型")]
        public static readonly Property<JobType?> JobTypeProperty = P<DownloadExcViewModelCriteria>.Register(e => e.JobType);

        /// <summary>
        /// 任务类型
        /// </summary>
        public JobType? JobType
        {
            get { return this.GetProperty(JobTypeProperty); }
            set { this.SetProperty(JobTypeProperty, value); }
        }
        #endregion

        #region 日期 LogDate
        /// <summary>
        /// 日期
        /// </summary>
        [Label("日期")]
        public static readonly Property<DateRange> LogDateProperty = P<DownloadExcViewModelCriteria>.Register(e => e.LogDate);

        /// <summary>
        /// 日期
        /// </summary>
        public DateRange LogDate
        {
            get { return this.GetProperty(LogDateProperty); }
            set { this.SetProperty(LogDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns>进出存汇总数据</returns>
        protected override EntityList Fetch()
        {
            var list = new EntityList<DownloadExcReportViewModel>();
            list.Add(RT.Service.Resolve<DownloadExcController>().GetDownloadExcs(this));
            return list;
        }
    }
}
