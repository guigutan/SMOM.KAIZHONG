using SIE.Alert;
using SIE.Alert.AlertManages;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.ProcessSegments;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;

namespace SIE.Equipments.Abnormal
{
    /// <summary>
    /// 异常停线
    /// </summary>
    [RootEntity, Serializable]
    [EntityWithConfig(typeof(NoConfig), "异常停线编码配置项", "异常停线编码配置规则")]
    [ConditionQueryType(typeof(AbnormalCauseCriteria))]
    [DisplayMember(nameof(Id))]
    [Label("异常停线")]
    public partial class AbnormalCause : DataEntity
    {
        /// <summary>
        /// 异常停线构造函数
        /// </summary>
        public AbnormalCause()
        {
            BeginDate = DateTime.Now;
            SourceType = ExceptionStopSourceType.UICreate;
        }

        /// <summary>
        /// 异常类型快码常量
        /// </summary>
        public static readonly string AbnormalTypeCatalog = "ABNORMAL_TYPE";

        #region 停线编码 Code
        /// <summary>
        /// 停线编码
        /// </summary>
        [Label("停线编码")]
        public static readonly Property<string> CodeProperty = P<AbnormalCause>.Register(e => e.Code);

        /// <summary>
        /// 停线编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 异常原因 AbnormalReason
        /// <summary>
        /// 异常原因
        /// </summary>
        [MaxLength(400)]
        [Label("停线原因")]
        public static readonly Property<string> AbnormalReasonProperty = P<AbnormalCause>.Register(e => e.AbnormalReason);

        /// <summary>
        /// 异常原因
        /// </summary>
        public string AbnormalReason
        {
            get { return GetProperty(AbnormalReasonProperty); }
            set { SetProperty(AbnormalReasonProperty, value); }
        }
        #endregion

        #region 停线发生时间 BeginDate
        /// <summary>
        /// 停线发生时间
        /// </summary>
        [Label("停线发生时间")]
        public static readonly Property<DateTime> BeginDateProperty = P<AbnormalCause>.Register(e => e.BeginDate, new PropertyMetadata<DateTime>() { DateTimePart = DateTimePart.DateTime });

        /// <summary>
        /// 停线发生时间
        /// </summary>
        public DateTime BeginDate
        {
            get { return GetProperty(BeginDateProperty); }
            set { SetProperty(BeginDateProperty, value); }
        }
        #endregion

        #region 停线结束时间 EndDate
        /// <summary>
        /// 停线结束时间
        /// </summary>
        [Label("停线结束时间")]
        public static readonly Property<DateTime?> EndDateProperty = P<AbnormalCause>.Register(e => e.EndDate);

        /// <summary>
        /// 停线结束时间
        /// </summary>
        public DateTime? EndDate
        {
            get { return GetProperty(EndDateProperty); }
            set { SetProperty(EndDateProperty, value); }
        }
        #endregion

        #region 异常类型 AbnormalType
        /// <summary>
        /// 异常类型
        /// </summary>
        [Label("异常类型")]
        public static readonly Property<string> AbnormalTypeProperty = P<AbnormalCause>.Register(e => e.AbnormalType);

        /// <summary>
        /// 异常类型
        /// </summary>
        public string AbnormalType
        {
            get { return GetProperty(AbnormalTypeProperty); }
            set { SetProperty(AbnormalTypeProperty, value); }
        }
        #endregion

        #region 异常停线类别 ExceptionStopType
        /// <summary>
        /// 异常停线类别
        /// </summary>
        [Required]
        [Label("停线类别")]
        public static readonly Property<ExceptionStopType> ExceptionStopTypeProperty = P<AbnormalCause>.Register(e => e.ExceptionStopType);

        /// <summary>
        /// 异常停线类别
        /// </summary>
        public ExceptionStopType ExceptionStopType
        {
            get { return GetProperty(ExceptionStopTypeProperty); }
            set { SetProperty(ExceptionStopTypeProperty, value); }
        }
        #endregion

        #region 产线 Line
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("产线编码")]
        public static readonly IRefIdProperty ResourceIdProperty = P<AbnormalCause>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<AbnormalCause>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty = P<AbnormalCause>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ProductId
        {
            get { return (double?)GetRefNullableId(ProductIdProperty); }
            set { SetRefNullableId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty = P<AbnormalCause>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return GetRefEntity(ProductProperty); }
            set { SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 车间 Shop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty ShopIdProperty = P<AbnormalCause>.RegisterRefId(e => e.ShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? ShopId
        {
            get { return (double?)GetRefNullableId(ShopIdProperty); }
            set { SetRefNullableId(ShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ShopProperty = P<AbnormalCause>.RegisterRef(e => e.Shop, ShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise Shop
        {
            get { return GetRefEntity(ShopProperty); }
            set { SetRefEntity(ShopProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<AbnormalCause>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)GetRefNullableId(WorkOrderIdProperty); }
            set { SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<AbnormalCause>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion  

        #region 异常停线来源 SourceType
        /// <summary>
        /// 异常停线来源
        /// </summary>
        [Label("停线来源")]
        public static readonly Property<ExceptionStopSourceType> SourceTypeProperty = P<AbnormalCause>.Register(e => e.SourceType);

        /// <summary>
        /// 异常停线来源
        /// </summary>
        public ExceptionStopSourceType SourceType
        {
            get { return GetProperty(SourceTypeProperty); }
            set { SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 预警配置 Alerter
        /// <summary>
        /// 预警配置Id
        /// </summary>
        [Label("预警配置")]
        public static readonly IRefIdProperty AlerterIdProperty =
            P<AbnormalCause>.RegisterRefId(e => e.AlerterId, ReferenceType.Normal);

        /// <summary>
        /// 预警配置Id
        /// </summary>
        public double? AlerterId
        {
            get { return (double?)GetRefNullableId(AlerterIdProperty); }
            set { SetRefNullableId(AlerterIdProperty, value); }
        }

        /// <summary>
        /// 预警配置
        /// </summary>
        public static readonly RefEntityProperty<Alerter> AlerterProperty =
            P<AbnormalCause>.RegisterRef(e => e.Alerter, AlerterIdProperty);

        /// <summary>
        /// 预警配置
        /// </summary>
        public Alerter Alerter
        {
            get { return GetRefEntity(AlerterProperty); }
            set { SetRefEntity(AlerterProperty, value); }
        }
        #endregion

        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备编码")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<AbnormalCause>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)GetRefNullableId(EquipAccountIdProperty); }
            set { SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty =
            P<AbnormalCause>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 状态说明 StateDescription
        /// <summary>
        /// 状态说明
        /// </summary>
        [Label("状态说明")]
        public static readonly Property<string> StateDescriptionProperty = P<AbnormalCause>.Register(e => e.StateDescription);

        /// <summary>
        /// 状态说明
        /// </summary>
        public string StateDescription
        {
            get { return GetProperty(StateDescriptionProperty); }
            set { SetProperty(StateDescriptionProperty, value); }
        }
        #endregion

        #region 预警编码 AlerterManage
        /// <summary>
        /// 预警编码Id
        /// </summary>
        [Label("预警编码")]
        public static readonly IRefIdProperty AlerterManageIdProperty =
            P<AbnormalCause>.RegisterRefId(e => e.AlerterManageId, ReferenceType.Normal);

        /// <summary>
        /// 预警编码Id
        /// </summary>
        public double? AlerterManageId
        {
            get { return (double?)GetRefNullableId(AlerterManageIdProperty); }
            set { SetRefNullableId(AlerterManageIdProperty, value); }
        }

        /// <summary>
        /// 预警编码
        /// </summary>
        public static readonly RefEntityProperty<AlertManage> AlerterManageProperty =
            P<AbnormalCause>.RegisterRef(e => e.AlerterManage, AlerterManageIdProperty);

        /// <summary>
        /// 预警编码
        /// </summary>
        public AlertManage AlerterManage
        {
            get { return GetRefEntity(AlerterManageProperty); }
            set { SetRefEntity(AlerterManageProperty, value); }
        }
        #endregion

        #region 停线恢复原因 RestoreReason
        /// <summary>
        /// 停线恢复原因
        /// </summary>
        [Label("停线恢复原因")]
        public static readonly Property<string> RestoreReasonProperty = P<AbnormalCause>.Register(e => e.RestoreReason);

        /// <summary>
        /// 停线恢复原因
        /// </summary>
        public string RestoreReason
        {
            get { return GetProperty(RestoreReasonProperty); }
            set { SetProperty(RestoreReasonProperty, value); }
        }
        #endregion

        #region 停线恢复人 Restorer
        /// <summary>
        /// 停线恢复人Id
        /// </summary>
        [Label("停线恢复人")]
        public static readonly IRefIdProperty RestorerIdProperty =
            P<AbnormalCause>.RegisterRefId(e => e.RestorerId, ReferenceType.Normal);

        /// <summary>
        /// 停线恢复人Id
        /// </summary>
        public double? RestorerId
        {
            get { return (double?)GetRefNullableId(RestorerIdProperty); }
            set { SetRefNullableId(RestorerIdProperty, value); }
        }

        /// <summary>
        /// 停线恢复人
        /// </summary>
        public static readonly RefEntityProperty<Employee> RestorerProperty =
            P<AbnormalCause>.RegisterRef(e => e.Restorer, RestorerIdProperty);

        /// <summary>
        /// 停线恢复人
        /// </summary>
        public Employee Restorer
        {
            get { return GetRefEntity(RestorerProperty); }
            set { SetRefEntity(RestorerProperty, value); }
        }
        #endregion

        #region 工步 Process
        /// <summary>
        /// 工步Id
        /// </summary>
        [Label("工步")]
        public static readonly IRefIdProperty ProcessIdProperty = P<AbnormalCause>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工步Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工步
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<AbnormalCause>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工步
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工序 ProcessSegment
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工段")]
        public static readonly IRefIdProperty ProcessSegmentIdProperty = P<AbnormalCause>.RegisterRefId(e => e.ProcessSegmentId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessSegmentId
        {
            get { return (double?)GetRefNullableId(ProcessSegmentIdProperty); }
            set { SetRefNullableId(ProcessSegmentIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<ProcessSegment> ProcessSegmentProperty = P<AbnormalCause>.RegisterRef(e => e.ProcessSegment, ProcessSegmentIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public ProcessSegment ProcessSegment
        {
            get { return GetRefEntity(ProcessSegmentProperty); }
            set { SetRefEntity(ProcessSegmentProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<AbnormalCause>.RegisterView(e => e.ProductCode, p => p.WorkOrder.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 设备名称 EquipName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipNameProperty = P<AbnormalCause>.RegisterView(e => e.EquipName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipName
        {
            get { return GetProperty(EquipNameProperty); }
        }
        #endregion

        #region 产线名称 LineName
        /// <summary>
        /// 产线名称
        /// </summary>
        [Label("产线名称")]
        public static readonly Property<string> LineNameProperty = P<AbnormalCause>.RegisterView(e => e.LineName, p => p.Resource.Name);

        /// <summary>
        /// 产线名称
        /// </summary>
        public string LineName
        {
            get { return GetProperty(LineNameProperty); }
        }
        #endregion

        #region 预警配置名称 AlerterName
        /// <summary>
        /// 预警配置名称
        /// </summary>
        [Label("预警配置名称")]
        public static readonly Property<string> AlerterNameProperty = P<AbnormalCause>.RegisterView(e => e.AlerterName, p => p.Alerter.Name);

        /// <summary>
        /// 预警配置名称
        /// </summary>
        public string AlerterName
        {
            get { return GetProperty(AlerterNameProperty); }
        }
        #endregion

        #region 预警模块 AlerterPlugName
        /// <summary>
        /// 预警模块
        /// </summary>
        [Label("预警模块")]
        public static readonly Property<string> AlerterPlugNameProperty = P<AbnormalCause>.RegisterView(e => e.AlerterPlugName, p => p.Alerter.AlertPlug.Name);

        /// <summary>
        /// 预警模块
        /// </summary>
        public string AlerterPlugName
        {
            get { return GetProperty(AlerterPlugNameProperty); }
        }
        #endregion

        #endregion

        #region 工单属性变更事件
        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="propertyName">属性变更名称</param>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(WorkOrder))
            {
                if (WorkOrder != null)
                {
                    Product = RF.GetById<Item>(WorkOrder.ProductId);
                }
            }
            else
            {
                if (propertyName == nameof(Shop))
                {
                    Resource = null;
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// 异常停线 实体配置
    /// </summary>
    internal class AbnormalCauseConfig : EntityConfig<AbnormalCause>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_ABNORMAL_CAUSE").MapAllProperties();
            Meta.Property(AbnormalCause.AbnormalReasonProperty).ColumnMeta.HasLength(500);
            Meta.Property(AbnormalCause.BeginDateProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}