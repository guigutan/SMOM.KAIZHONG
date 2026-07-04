using SIE.Alert;
using SIE.Alert.AlertManages;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.ProcessSegments;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;

namespace SIE.Equipments.Abnormal
{
    /// <summary>
    /// 异常停线查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("异常停线查询界面")]
    public partial class AbnormalCauseCriteria : Criteria
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AbnormalCauseCriteria()
        {
            BeginDate = new DateRange();
            BeginDate.DateTimePart = DateTimePart.Date;  //选择日期格式为天
            BeginDate.DateRangeType = DateRangeType.Week;  //默认日期为本周
        }

        #region 车间 Shop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty ShopIdProperty = P<AbnormalCauseCriteria>.RegisterRefId(e => e.ShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? ShopId
        {
            get { return (double?)GetRefNullableId(ShopIdProperty); }
            set { SetRefId(ShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ShopProperty = P<AbnormalCauseCriteria>.RegisterRef(e => e.Shop, ShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise Shop
        {
            get { return GetRefEntity(ShopProperty); }
            set { SetRefEntity(ShopProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线编码")]
        public static readonly IRefIdProperty LineIdProperty = P<AbnormalCauseCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)GetRefNullableId(LineIdProperty); }
            set { SetRefId(LineIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<AbnormalCauseCriteria>.RegisterRef(e => e.Resource, LineIdProperty);

        /// <summary>
        /// 资源
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
        public static readonly IRefIdProperty ProductIdProperty = P<AbnormalCauseCriteria>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ProductId
        {
            get { return (double?)GetRefNullableId(ProductIdProperty); }
            set { SetRefId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty = P<AbnormalCauseCriteria>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return GetRefEntity(ProductProperty); }
            set { SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<AbnormalCauseCriteria>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)GetRefNullableId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<AbnormalCauseCriteria>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 异常类型 AbnormalType
        /// <summary>
        /// 异常类型
        /// </summary>
        [Label("异常类型")]
        public static readonly Property<string> AbnormalTypeProperty = P<AbnormalCauseCriteria>.Register(e => e.AbnormalType);

        /// <summary>
        /// 异常类型
        /// </summary>
        public string AbnormalType
        {
            get { return GetProperty(AbnormalTypeProperty); }
            set { SetProperty(AbnormalTypeProperty, value); }
        }
        #endregion

        #region 异常停线来源 SourceType
        /// <summary>
        /// 异常停线来源
        /// </summary>
        [Label("停线来源")]
        public static readonly Property<ExceptionStopSourceType?> SourceTypeProperty = P<AbnormalCauseCriteria>.Register(e => e.SourceType);

        /// <summary>
        /// 异常停线来源
        /// </summary>
        public ExceptionStopSourceType? SourceType
        {
            get { return GetProperty(SourceTypeProperty); }
            set { SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 异常停线类别 ExceptionStopType
        /// <summary>
        /// 异常停线类别
        /// </summary>
        [Label("停线类别")]
        public static readonly Property<ExceptionStopType?> ExceptionStopTypeProperty = P<AbnormalCauseCriteria>.Register(e => e.ExceptionStopType);

        /// <summary>
        /// 异常停线类别
        /// </summary>
        public ExceptionStopType? ExceptionStopType
        {
            get { return GetProperty(ExceptionStopTypeProperty); }
            set { SetProperty(ExceptionStopTypeProperty, value); }
        }
        #endregion

        #region 设备 EquipAccount
        /// <summary>
        /// 设备Id
        /// </summary>
        [Label("设备编码")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<AbnormalCauseCriteria>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)GetRefNullableId(EquipAccountIdProperty); }
            set { SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>   
        /// 设备
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty =
            P<AbnormalCauseCriteria>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 预警管理 AlertManage
        /// <summary>
        /// 预警管理Id
        /// </summary>
        [Label("预警编码")]
        public static readonly IRefIdProperty AlertManageIdProperty =
            P<AbnormalCauseCriteria>.RegisterRefId(e => e.AlertManageId, ReferenceType.Normal);

        /// <summary>
        /// 预警管理Id
        /// </summary>
        public double? AlertManageId
        {
            get { return (double?)GetRefNullableId(AlertManageIdProperty); }
            set { SetRefNullableId(AlertManageIdProperty, value); }
        }

        /// <summary>
        /// 预警管理
        /// </summary>
        public static readonly RefEntityProperty<AlertManage> AlertManageProperty =
            P<AbnormalCauseCriteria>.RegisterRef(e => e.AlertManage, AlertManageIdProperty);

        /// <summary>
        /// 预警管理
        /// </summary>
        public AlertManage AlertManage
        {
            get { return GetRefEntity(AlertManageProperty); }
            set { SetRefEntity(AlertManageProperty, value); }
        }
        #endregion

        #region 预警配置 Alerter
        /// <summary>
        /// 预警配置Id
        /// </summary>
        [Label("预警配置编码")]
        public static readonly IRefIdProperty AlerterIdProperty =
            P<AbnormalCauseCriteria>.RegisterRefId(e => e.AlerterId, ReferenceType.Normal);

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
            P<AbnormalCauseCriteria>.RegisterRef(e => e.Alerter, AlerterIdProperty);

        /// <summary>
        /// 预警配置
        /// </summary>
        public Alerter Alerter
        {
            get { return GetRefEntity(AlerterProperty); }
            set { SetRefEntity(AlerterProperty, value); }
        }
        #endregion

        #region 停线发生时间 BeginDate
        /// <summary>
        /// 停线发生时间
        /// </summary>
        [Label("停线时间")]
        public static readonly Property<DateRange> BeginDateProperty = P<AbnormalCauseCriteria>.Register(e => e.BeginDate, new PropertyMetadata<DateRange>() { DateTimePart = DateTimePart.Date });

        /// <summary>
        /// 停线发生时间
        /// </summary>
        public DateRange BeginDate
        {
            get { return GetProperty(BeginDateProperty); }
            set { SetProperty(BeginDateProperty, value); }
        }
        #endregion

        #region 工步 Process
        /// <summary>
        /// 工步Id
        /// </summary>
        [Label("工步")]
        public static readonly IRefIdProperty ProcessIdProperty = P<AbnormalCauseCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Process> ProcessProperty = P<AbnormalCauseCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);

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
        public static readonly IRefIdProperty ProcessSegmentIdProperty = P<AbnormalCauseCriteria>.RegisterRefId(e => e.ProcessSegmentId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<ProcessSegment> ProcessSegmentProperty = P<AbnormalCauseCriteria>.RegisterRef(e => e.ProcessSegment, ProcessSegmentIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public ProcessSegment ProcessSegment
        {
            get { return GetRefEntity(ProcessSegmentProperty); }
            set { SetRefEntity(ProcessSegmentProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        /// <returns>异常停线列表</returns>
        protected override EntityList Fetch()
        {
            return AppRuntime.Service.Resolve<AbnormalCauseController>().GetAbnormalCauses(this);
        }
    }
}
