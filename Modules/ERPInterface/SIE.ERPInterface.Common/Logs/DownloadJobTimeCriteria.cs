using SIE.Domain;
using SIE.ERPInterface.Common.Enums;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.Logs
{
    /// <summary>
    /// 接口任务时间戳查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("接口任务时间戳查询实体")]
    public partial class DownloadJobTimeCriteria : Criteria
    {
        #region 下载任务类型 JobType
        /// <summary>
        /// 下载任务类型
        /// </summary>
        [Label("任务类型")]
        public static readonly Property<JobType?> JobTypeProperty = P<DownloadJobTimeCriteria>.Register(e => e.JobType);

        /// <summary>
        /// 下载任务类型
        /// </summary>
        public JobType? JobType
        {
            get { return GetProperty(JobTypeProperty); }
            set { SetProperty(JobTypeProperty, value); }
        }
        #endregion

        #region 请求时间 RequestDate
        /// <summary>
        /// 请求时间
        /// </summary>
        [Label("请求时间")]
        public static readonly Property<DateRange> RequestDateProperty = P<DownloadJobTimeCriteria>.Register(e => e.RequestDate);

        /// <summary>
        /// 请求时间
        /// </summary>
        public DateRange RequestDate
        {
            get { return this.GetProperty(RequestDateProperty); }
            set { this.SetProperty(RequestDateProperty, value); }
        }
        #endregion

        /// <summary>
		/// 重写此方法实现查询
		/// </summary>
		protected override EntityList Fetch()
        {
            return RT.Service.Resolve<DownloadExcController>().GetDownloadJobTimes(this);
        }
    }
}
