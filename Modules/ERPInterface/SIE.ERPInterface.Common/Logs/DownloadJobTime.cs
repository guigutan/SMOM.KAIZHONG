using SIE.Domain;
using SIE.ERPInterface.Common.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.Logs
{
    /// <summary>
    /// 下载任务时间戳
    /// </summary>
    [RootEntity, Serializable]

    [ConditionQueryType(typeof(DownloadJobTimeCriteria))]
    [Label("任务执行时间戳")]
    public partial class DownloadJobTime : DataEntity
    {
        #region 最后执行时间 LastDownloadDate
        /// <summary>
        /// 最后下载时间
        /// </summary>
        [Label("最后执行时间")]
        public static readonly Property<DateTime?> LastDownloadDateProperty = P<DownloadJobTime>.Register(e => e.LastDownloadDate);

        /// <summary>
        /// 最后下载时间
        /// </summary>
        public DateTime? LastDownloadDate
        {
            get { return GetProperty(LastDownloadDateProperty); }
            set { SetProperty(LastDownloadDateProperty, value); }
        }
        #endregion

        #region 最大重试次数 MaxRetryCount
        /// <summary>
        /// 最大重试次数
        /// </summary>
        [Required]
        [Label("最大重试次数")]
        public static readonly Property<int> MaxRetryCountProperty = P<DownloadJobTime>.Register(e => e.MaxRetryCount);

        /// <summary>
        /// 最大重试次数
        /// </summary>
        public int MaxRetryCount
        {
            get { return GetProperty(MaxRetryCountProperty); }
            set { SetProperty(MaxRetryCountProperty, value); }
        }
        #endregion

        #region 下载任务类型 JobType
        /// <summary>
        /// 下载任务类型
        /// </summary>
        [Label("任务类型")]
        public static readonly Property<JobType> JobTypeProperty = P<DownloadJobTime>.Register(e => e.JobType);

        /// <summary>
        /// 下载任务类型
        /// </summary>
        public JobType JobType
        {
            get { return GetProperty(JobTypeProperty); }
            set { SetProperty(JobTypeProperty, value); }
        }
        #endregion

        #region 明细 DetailList
        /// <summary>
        /// 明细
        /// </summary>
        public static readonly ListProperty<EntityList<DownloadJobTimeDetail>> DetailListProperty = P<DownloadJobTime>.RegisterList(e => e.DetailList);
        /// <summary>
        /// 明细
        /// </summary>
        public EntityList<DownloadJobTimeDetail> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion

        #region 任务类别 JobCate
        /// <summary>
        /// 任务类别
        /// </summary>
        [Label("任务类别")]
        public static readonly Property<JobCate> JobCateProperty = P<DownloadJobTime>.Register(e => e.JobCate);

        /// <summary>
        /// 任务类别
        /// </summary>
        public JobCate JobCate
        {
            get { return this.GetProperty(JobCateProperty); }
            set { this.SetProperty(JobCateProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 任务下载时间 实体配置
    /// </summary>
    internal class DownloadJobTimeConfig : EntityConfig<DownloadJobTime>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("DL_JOB_TIME").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}