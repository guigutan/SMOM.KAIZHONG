using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.Logs
{
    /// <summary>
    /// 任务下载记录
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("任务下载记录")]
    public partial class DownloadJobLog : DataEntity
    {
        #region 开始时间 BeginDate
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateTime> BeginDateProperty = P<DownloadJobLog>.Register(e => e.BeginDate);

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginDate
        {
            get { return GetProperty(BeginDateProperty); }
            set { SetProperty(BeginDateProperty, value); }
        }
        #endregion

        #region 结束时间 EndDate
        /// <summary>
        /// 结束时间
        /// </summary>
        [Label("结束时间")]
        public static readonly Property<DateTime> EndDateProperty = P<DownloadJobLog>.Register(e => e.EndDate);

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate
        {
            get { return GetProperty(EndDateProperty); }
            set { SetProperty(EndDateProperty, value); }
        }
        #endregion

        #region 数据量 DataCount
        /// <summary>
        /// 数据量
        /// </summary>
        [Label("数据量")]
        public static readonly Property<int> DataCountProperty = P<DownloadJobLog>.Register(e => e.DataCount);

        /// <summary>
        /// 数据量
        /// </summary>
        public int DataCount
        {
            get { return GetProperty(DataCountProperty); }
            set { SetProperty(DataCountProperty, value); }
        }
        #endregion

        #region 成功数 SuccessCount
        /// <summary>
        /// 成功数
        /// </summary>
        [Label("成功数")]
        public static readonly Property<int> SuccessCountProperty = P<DownloadJobLog>.Register(e => e.SuccessCount);

        /// <summary>
        /// 成功数
        /// </summary>
        public int SuccessCount
        {
            get { return GetProperty(SuccessCountProperty); }
            set { SetProperty(SuccessCountProperty, value); }
        }
        #endregion

        #region 失败数 FailCount
        /// <summary>
        /// 失败数
        /// </summary>
        [Label("失败数")]
        public static readonly Property<int> FailCountProperty = P<DownloadJobLog>.Register(e => e.FailCount);

        /// <summary>
        /// 失败数
        /// </summary>
        public int FailCount
        {
            get { return GetProperty(FailCountProperty); }
            set { SetProperty(FailCountProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<DownloadJobLog>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 中间表ID InfId
        /// <summary>
        /// 中间表ID
        /// </summary>
        [Label("中间表ID")]
        public static readonly Property<string> InfIdProperty = P<DownloadJobLog>.Register(e => e.InfId);

        /// <summary>
        /// 中间表ID
        /// </summary>
        public string InfId
        {
            get { return GetProperty(InfIdProperty); }
            set { SetProperty(InfIdProperty, value); }
        }
        #endregion

        #region ERP主键 ErpKey
        /// <summary>
        /// ERP主键
        /// </summary>
        [Label("ERP主键")]
        public static readonly Property<string> ErpKeyProperty = P<DownloadJobLog>.Register(e => e.ErpKey);

        /// <summary>
        /// ERP主键
        /// </summary>
        public string ErpKey
        {
            get { return GetProperty(ErpKeyProperty); }
            set { SetProperty(ErpKeyProperty, value); }
        }
        #endregion

        #region 操作类型 OperationType
        /// <summary>
        /// 操作类型
        /// </summary>
        [Label("操作类型")]
        public static readonly Property<OperationType> OperationTypeProperty = P<DownloadJobLog>.Register(e => e.OperationType);

        /// <summary>
        /// 操作类型
        /// </summary>
        public OperationType OperationType
        {
            get { return GetProperty(OperationTypeProperty); }
            set { SetProperty(OperationTypeProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("处理状态")]
        public static readonly Property<ProcessState> StateProperty = P<DownloadJobLog>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public ProcessState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 任务模式 JobMode
        /// <summary>
        /// 任务模式
        /// </summary>
        [Label("任务模式")]
        public static readonly Property<JobMode> JobModeProperty = P<DownloadJobLog>.Register(e => e.JobMode);

        /// <summary>
        /// 任务模式
        /// </summary>
        public JobMode JobMode
        {
            get { return this.GetProperty(JobModeProperty); }
            set { this.SetProperty(JobModeProperty, value); }
        }
        #endregion

        #region 下载任务类型 JobType
        /// <summary>
        /// 下载任务类型
        /// </summary>
        [Label("任务类型")]
        public static readonly Property<JobType> JobTypeProperty = P<DownloadJobLog>.Register(e => e.JobType);

        /// <summary>
        /// 下载任务类型
        /// </summary>
        public JobType JobType
        {
            get { return GetProperty(JobTypeProperty); }
            set { SetProperty(JobTypeProperty, value); }
        }
        #endregion

        #region 任务方向 JobDirection
        /// <summary>
        /// 任务方向
        /// </summary>
        [Label("任务方向")]
        public static readonly Property<JobDirection> JobDirectionProperty = P<DownloadJobLog>.Register(e => e.JobDirection);

        /// <summary>
        /// 任务方向
        /// </summary>
        public JobDirection JobDirection
        {
            get { return GetProperty(JobDirectionProperty); }
            set { SetProperty(JobDirectionProperty, value); }
        }
        #endregion

        #region 来源数据 SourceData
        /// <summary>
        /// 来源数据
        /// </summary>
        [Label("来源数据")]
        public static readonly Property<string> SourceDataProperty = P<DownloadJobLog>.Register(e => e.SourceData);

        /// <summary>
        /// 来源数据
        /// </summary>
        public string SourceData
        {
            get { return this.GetProperty(SourceDataProperty); }
            set { this.SetProperty(SourceDataProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 任务下载记录 实体配置
    /// </summary>
    internal class DownloadJobLogConfig : EntityConfig<DownloadJobLog>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(DownloadJobLog.RemarkProperty, new StringLengthRangeRule() { Max = 4000 });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("DL_JOB_LOG").MapAllProperties();
            Meta.Property(DownloadJobLog.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.Property(DownloadJobLog.SourceDataProperty).ColumnMeta.HasLength("Max");
            Meta.EnablePhantoms();
        }
    }
}