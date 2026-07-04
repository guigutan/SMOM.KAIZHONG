using SIE.Domain;
using SIE.MES.ItemChecker;
using SIE.MES.PackingQC;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.HeatTreatments;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.IOT
{
    /// <summary>
    /// IOT押出换轴记录
    /// </summary>
    [RootEntity, Serializable]
    //[CriteriaQuery]
    [ConditionQueryType(typeof(AxisChangeRecordCriterial))]
    [Label("IOT押出换轴记录")]
    public partial class AxisChangeRecord :DataEntity
    {
        #region IOT实体 IotEntity
        /// <summary>
        /// IOT实体
        /// </summary>
        [Label("IOT实体")]
        public static readonly Property<string> IotEntityProperty = P<AxisChangeRecord>.Register(e => e.IotEntity);

        /// <summary>
        /// IOT实体
        /// </summary>
        public string IotEntity
        {
            get { return this.GetProperty(IotEntityProperty); }
            set { this.SetProperty(IotEntityProperty, value); }
        }
        #endregion

        #region 换轴标识 ChangeFlag
        /// <summary>
        /// 换轴标识
        /// </summary>
        [Label("换轴标识")]
        public static readonly Property<bool> ChangeFlagProperty = P<AxisChangeRecord>.Register(e => e.ChangeFlag);

        /// <summary>
        /// 换轴标识
        /// </summary>
        public bool ChangeFlag
        {
            get { return this.GetProperty(ChangeFlagProperty); }
            set { this.SetProperty(ChangeFlagProperty, value); }
        }
        #endregion

        #region 换轴米数 AxisQty
        /// <summary>
        /// 换轴米数
        /// </summary>
        [Label("换轴米数")]
        public static readonly Property<decimal?> AxisQtyProperty = P<AxisChangeRecord>.Register(e => e.AxisQty);

        /// <summary>
        /// 换轴米数
        /// </summary>
        public decimal? AxisQty
        {
            get { return this.GetProperty(AxisQtyProperty); }
            set { this.SetProperty(AxisQtyProperty, value); }
        }
        #endregion

        #region 计米数量 MeterCount
        /// <summary>
        /// 计米数量
        /// </summary>
        [Label("计米数量")]
        public static readonly Property<decimal?> MeterCountProperty = P<AxisChangeRecord>.Register(e => e.MeterCount);

        /// <summary>
        /// 计米数量
        /// </summary>
        public decimal? MeterCount
        {
            get { return this.GetProperty(MeterCountProperty); }
            set { this.SetProperty(MeterCountProperty, value); }
        }
        #endregion

        #region 推送时间 CollectionTime
        /// <summary>
        /// 推送时间
        /// </summary>
        [Label("推送时间")]
        public static readonly Property<DateTime?> CollectionTimeProperty = P<AxisChangeRecord>.Register(e => e.CollectionTime);

        /// <summary>
        /// 推送时间
        /// </summary>
        public DateTime? CollectionTime
        {
            get { return this.GetProperty(CollectionTimeProperty); }
            set { this.SetProperty(CollectionTimeProperty, value); }
        }
        #endregion

        #region 任务单号 TaskNo
        /// <summary>
        /// 任务单号
        /// </summary>
        [Label("任务单号")]
        public static readonly Property<string> TaskNoProperty = P<AxisChangeRecord>.Register(e => e.TaskNo);

        /// <summary>
        /// 任务单号
        /// </summary>
        public string TaskNo
        {
            get { return this.GetProperty(TaskNoProperty); }
            set { this.SetProperty(TaskNoProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryProperty = P<AxisChangeRecord>.Register(e => e.Factory);

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory
        {
            get { return this.GetProperty(FactoryProperty); }
            set { this.SetProperty(FactoryProperty, value); }
        }
        #endregion

        #region 是否已报工 IsReport
        /// <summary>
        /// 是否已报工
        /// </summary>
        [Label("是否已报工")]
        public static readonly Property<bool> IsReportProperty = P<AxisChangeRecord>.Register(e => e.IsReport);

        /// <summary>
        /// 是否已报工
        /// </summary>
        public bool IsReport
        {
            get { return this.GetProperty(IsReportProperty); }
            set { this.SetProperty(IsReportProperty, value); }
        }
        #endregion

        #region 报工数量 ReportQty
        /// <summary>
        /// 报工数量
        /// </summary>
        [Label("报工数量")]
        public static readonly Property<decimal> ReportQtyProperty = P<AxisChangeRecord>.Register(e => e.ReportQty);

        /// <summary>
        /// 报工数量
        /// </summary>
        public decimal ReportQty
        {
            get { return this.GetProperty(ReportQtyProperty); }
            set { this.SetProperty(ReportQtyProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<AxisChangeRecord>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 派工单 DispatchTask
        /// <summary>
        /// 派工单Id
        /// </summary>
        [Label("派工单")]
        public static readonly IRefIdProperty DispatchTaskIdProperty =
            P<AxisChangeRecord>.RegisterRefId(e => e.DispatchTaskId, ReferenceType.Normal);

        /// <summary>
        /// 派工单Id
        /// </summary>
        public double? DispatchTaskId
        {
            get { return (double?)this.GetRefId(DispatchTaskIdProperty); }
            set { this.SetRefId(DispatchTaskIdProperty, value); }
        }

        /// <summary>
        /// 派工单
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> DispatchTaskProperty =
            P<AxisChangeRecord>.RegisterRef(e => e.DispatchTask, DispatchTaskIdProperty);

        /// <summary>
        /// 派工单
        /// </summary>
        public DispatchTask DispatchTask
        {
            get { return this.GetRefEntity(DispatchTaskProperty); }
            set { this.SetRefEntity(DispatchTaskProperty, value); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<AxisChangeRecord>.Register(e => e.ResourceName);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { this.SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 资源编码 ResourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> ResourceCodeProperty = P<AxisChangeRecord>.Register(e => e.ResourceCode);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
            set { this.SetProperty(ResourceCodeProperty, value); }
        }
        #endregion


        #region 视图属性

        #endregion

    }

    /// <summary>
    /// IOT押出换轴记录 实体配置
    /// </summary>
    internal class AxisChangeRecordEntityConfig : EntityConfig<AxisChangeRecord>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
        }
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("IOT_AXIS_CHANGE_RECORD").MapAllProperties();
            Meta.Property(AxisChangeRecord.RemarkProperty).ColumnMeta.HasLength(1000);
            Meta.Property(AxisChangeRecord.ResourceCodeProperty).DontMapColumn();
            Meta.Property(AxisChangeRecord.ResourceNameProperty).DontMapColumn();
            Meta.EnablePhantoms();
            Meta.DisableInvOrg();
        }
    }
}
