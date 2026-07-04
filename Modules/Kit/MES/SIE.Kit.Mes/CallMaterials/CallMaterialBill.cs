using SIE.Common.Configs;
using SIE.Domain;
using SIE.Kit.MES.CallMaterials.Configs;
using SIE.Kit.MES.Storages;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;

namespace SIE.Kit.MES.CallMaterials
{
    /// <summary>
    /// 叫料单
    /// </summary>
    [ChildEntity, Serializable]
    [EntityWithConfig(typeof(CallMaterialConfig))]
    [EntityWithConfig(typeof(WareHouseConfig))]
    [Label("叫料单")]
    [DataAuth.EntityDataAuth(typeof(Resources.Employees.EmployeeEnterprise), nameof(FactoryId), true)]
    public partial class CallMaterialBill : DataEntity
    {
        #region 叫料单号 No
        /// <summary>
        /// 叫料单号
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("叫料单号")]
        public static readonly Property<string> NoProperty = P<CallMaterialBill>.Register(e => e.No);

        /// <summary>
        /// 叫料单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 需求时间 RequiredTime
        /// <summary>
        /// 需求时间
        /// </summary>		
        [Label("需求时间")]
        public static readonly Property<DateTime> RequiredTimeProperty = P<CallMaterialBill>.Register(e => e.RequiredTime);

        /// <summary>
        /// 需求时间
        /// </summary>
        public DateTime RequiredTime
        {
            get { return GetProperty(RequiredTimeProperty); }
            set { SetProperty(RequiredTimeProperty, value); }
        }
        #endregion

        #region 发料时间 SendingTime
        /// <summary>
        /// 发料时间
        /// </summary>
        [Label("发料时间")]
        public static readonly Property<DateTime?> SendingTimeProperty = P<CallMaterialBill>.Register(e => e.SendingTime);

        /// <summary>
        /// 发料时间
        /// </summary>
        public DateTime? SendingTime
        {
            get { return GetProperty(SendingTimeProperty); }
            set { SetProperty(SendingTimeProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1500)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<CallMaterialBill>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 所属资源 Resource
        /// <summary>
        /// 所属资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<CallMaterialBill>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 所属资源Id
        /// </summary>
        public double ResourceId
        {
            get { return (double)GetRefId(ResourceIdProperty); }
            set { SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 所属资源
        /// </summary>
        [Label("资源")]
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<CallMaterialBill>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 所属资源
        /// </summary>       
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 优先级 Priority
        /// <summary>
        /// 优先级
        /// </summary>
        [Label("优先级")]
        public static readonly Property<Priority> PriorityProperty = P<CallMaterialBill>.Register(e => e.Priority);

        /// <summary>
        /// 优先级
        /// </summary>
        public Priority Priority
        {
            get { return GetProperty(PriorityProperty); }
            set { SetProperty(PriorityProperty, value); }
        }
        #endregion

        #region 状态 Status
        /// <summary>
        /// 状态
        /// </summary>
        [Label("叫料单状态")]
        public static readonly Property<CallMaterialStatus> StatusProperty = P<CallMaterialBill>.Register(e => e.Status);

        /// <summary>
        /// 状态
        /// </summary>
        public CallMaterialStatus Status
        {
            get { return GetProperty(StatusProperty); }
            set { SetProperty(StatusProperty, value); }
        }
        #endregion

        #region 叫料方式 Mode
        /// <summary>
        /// 叫料方式
        /// </summary>
        [Label("叫料方式")]
        public static readonly Property<CallMaterialMode> ModeProperty = P<CallMaterialBill>.Register(e => e.Mode);

        /// <summary>
        /// 叫料方式
        /// </summary>
        public CallMaterialMode Mode
        {
            get { return this.GetProperty(ModeProperty); }
            set { this.SetProperty(ModeProperty, value); }
        }
        #endregion 

        #region 配送工位 Station
        /// <summary>
        /// 配送工位Id
        /// </summary>
        [Label("配送工位")]
        public static readonly IRefIdProperty StationIdProperty = P<CallMaterialBill>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 配送工位Id
        /// </summary>
        public double StationId
        {
            get { return (double)GetRefId(StationIdProperty); }
            set { SetRefId(StationIdProperty, value); }
        }

        /// <summary>
        /// 配送工位
        /// </summary>
        [Label("配送工位")]
        public static readonly RefEntityProperty<Station> StationProperty = P<CallMaterialBill>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 配送工位
        /// </summary>
        public Station Station
        {
            get { return GetRefEntity(StationProperty); }
            set { SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<CallMaterialBill>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)this.GetRefId(ProcessIdProperty); }
            set { this.SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<CallMaterialBill>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工位货区 StorageArea
        /// <summary>
        /// 工位货区Id
        /// </summary>
        [Label("工位货区")]
        public static readonly IRefIdProperty StorageAreaIdProperty = P<CallMaterialBill>.RegisterRefId(e => e.StorageAreaId, ReferenceType.Normal);

        /// <summary>
        /// 工位货区Id
        /// </summary>
        public double? StorageAreaId
        {
            get { return (double?)GetRefNullableId(StorageAreaIdProperty); }
            set { SetRefNullableId(StorageAreaIdProperty, value); }
        }

        /// <summary>
        /// 工位货区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> StorageAreaProperty = P<CallMaterialBill>.RegisterRef(e => e.StorageArea, StorageAreaIdProperty);

        /// <summary>
        /// 工位货区
        /// </summary>
        public StorageArea StorageArea
        {
            get { return GetRefEntity(StorageAreaProperty); }
            set { SetRefEntity(StorageAreaProperty, value); }
        }
        #endregion

        #region 货位 StorageLocation
        /// <summary>
        /// 货位Id
        /// </summary>
        [Label("货位")]
        public static readonly IRefIdProperty StorageLocationIdProperty =
            P<CallMaterialBill>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 货位Id
        /// </summary>
        public double? StorageLocationId
        {
            get { return (double?)this.GetRefNullableId(StorageLocationIdProperty); }
            set { this.SetRefNullableId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 货位
        /// </summary>
        [Label("货位")]
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty =
            P<CallMaterialBill>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 货位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return this.GetRefEntity(StorageLocationProperty); }
            set { this.SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 叫料工单 WorkOrder
        /// <summary>
        /// 叫料工单Id
        /// </summary>  
        [Label("叫料工单")]
        public static readonly IRefIdProperty CallWorkOrderIdProperty = P<CallMaterialBill>.RegisterRefId(e => e.CallWorkOrderId, ReferenceType.Parent);

        /// <summary>
        /// 叫料工单Id
        /// </summary>
        public double CallWorkOrderId
        {
            get { return (double)GetRefId(CallWorkOrderIdProperty); }
            set { SetRefId(CallWorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 叫料工单
        /// </summary>
        public static readonly RefEntityProperty<CallMaterialWorkOrder> WorkOrderProperty = P<CallMaterialBill>.RegisterRef(e => e.CallWorkOrder, CallWorkOrderIdProperty);

        /// <summary>
        /// 叫料工单
        /// </summary>
        public CallMaterialWorkOrder CallWorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 叫料单明细列表 DetailList
        /// <summary>
        /// 叫料单明细列表
        /// </summary>
        [Label("叫料明细")]
        public static readonly ListProperty<EntityList<CallMaterialBillDetail>> DetailListProperty = P<CallMaterialBill>.RegisterList(e => e.DetailList);

        /// <summary>
        /// 叫料单明细列表
        /// </summary>
        public EntityList<CallMaterialBillDetail> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion

        #region 叫料原因列表 ReasonList
        /// <summary>
        /// 叫料原因列表
        /// </summary>
        [Label("叫料原因列表")]
        public static readonly ListProperty<EntityList<UrgencyCallMeterialReason>> ReasonListProperty = P<CallMaterialBill>.RegisterList(e => e.ReasonList);

        /// <summary>
        /// 叫料原因列表
        /// </summary>
        public EntityList<UrgencyCallMeterialReason> ReasonList
        {
            get { return this.GetLazyList(ReasonListProperty); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        [Required]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<CallMaterialBill>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)this.GetRefNullableId(FactoryIdProperty); }
            set { this.SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Resources.Enterprises.Enterprise> FactoryProperty =
            P<CallMaterialBill>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Resources.Enterprises.Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<CallMaterialBill>.RegisterView(e => e.WorkOrderNo, p => p.CallWorkOrder.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 叫料单 实体配置
    /// </summary>
    internal class CallMaterialBillConfig : EntityConfig<CallMaterialBill>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_CALL_BILL").MapAllProperties();
            Meta.Property(CallMaterialBill.RemarkProperty).ColumnMeta.HasLength(3000);
            Meta.Property(CallMaterialBill.StorageLocationIdProperty).ColumnMeta.HasIndex();
            Meta.Property(CallMaterialBill.NoProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}