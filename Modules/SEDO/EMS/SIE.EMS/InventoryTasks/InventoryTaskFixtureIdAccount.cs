using SIE.Domain;
using SIE.EMS.Enums;
using SIE.Equipments.Enums;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.Models;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;

namespace SIE.EMS.InventoryTasks
{
    /// <summary>
    /// 盘点任务工治具序列号明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("盘点任务工治具序列号明细")]
    public partial class InventoryTaskFixtureIdAccount : DataEntity
    {

        #region 审核状态 ApprovalStatus	
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<InventoryTaskFixtureIdAccount>.RegisterView(e => e.ApprovalStatus, p => p.InventoryTask.ApprovalStatus);

        /// <summary>
        /// 注释
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return this.GetProperty(ApprovalStatusProperty); }
        }
        #endregion

        #region 平帐方式 BalancingWay
        /// <summary>
        /// 平帐方式
        /// </summary>
        [Label("平帐方式")]
        public static readonly Property<BalancingWay?> BalancingWayProperty = P<InventoryTaskFixtureIdAccount>.Register(e => e.BalancingWay);

        /// <summary>
        /// 平帐方式
        /// </summary>
        public BalancingWay? BalancingWay
        {
            get { return this.GetProperty(BalancingWayProperty); }
            set { this.SetProperty(BalancingWayProperty, value); }
        }
        #endregion


        #region 工治具ID Sn
        /// <summary>
        /// 工治具ID
        /// </summary>
        [Label("工治具ID")]
        public static readonly Property<string> SnProperty = P<InventoryTaskFixtureIdAccount>.Register(e => e.Sn);

        /// <summary>
        /// 工治具ID
        /// </summary>
        public string Sn
        {
            get { return GetProperty(SnProperty); }
            set { SetProperty(SnProperty, value); }
        }
        #endregion

        #region 初盘时间 FirstDateTime
        /// <summary>
        /// 初盘时间
        /// </summary>
        [Label("初盘时间")]
        public static readonly Property<DateTime?> FirstDateTimeProperty = P<InventoryTaskFixtureIdAccount>.Register(e => e.FirstDateTime);

        /// <summary>
        /// 初盘时间
        /// </summary>
        public DateTime? FirstDateTime
        {
            get { return GetProperty(FirstDateTimeProperty); }
            set { SetProperty(FirstDateTimeProperty, value); }
        }
        #endregion

        #region 复盘时间 SecondDateTime
        /// <summary>
        /// 复盘时间
        /// </summary>
        [Label("复盘时间")]
        public static readonly Property<DateTime?> SecondDateTimeProperty = P<InventoryTaskFixtureIdAccount>.Register(e => e.SecondDateTime);

        /// <summary>
        /// 复盘时间
        /// </summary>
        public DateTime? SecondDateTime
        {
            get { return GetProperty(SecondDateTimeProperty); }
            set { SetProperty(SecondDateTimeProperty, value); }
        }
        #endregion

        #region 工治具状态 FixtureStatus
        /// <summary>
        /// 工治具状态
        /// </summary>
        [Label("工治具状态")]
        public static readonly Property<FixtureStatus> FixtureStatusProperty = P<InventoryTaskFixtureIdAccount>.Register(e => e.FixtureStatus);

        /// <summary>
        /// 状态
        /// </summary>
        public FixtureStatus FixtureStatus
        {
            get { return GetProperty(FixtureStatusProperty); }
            set { SetProperty(FixtureStatusProperty, value); }
        }
        #endregion

        #region 盘点状态 InventoryStatus
        /// <summary>
        /// 盘点状态
        /// </summary>
        [Label("盘点状态")]
        public static readonly Property<InventoryStatus> InventoryStatusProperty = P<InventoryTaskFixtureIdAccount>.Register(e => e.InventoryStatus);

        /// <summary>
        /// 盘点状态
        /// </summary>
        public InventoryStatus InventoryStatus
        {
            get { return GetProperty(InventoryStatusProperty); }
            set { SetProperty(InventoryStatusProperty, value); }
        }
        #endregion

        #region 初盘结果 FirstResult
        /// <summary>
        /// 初盘结果
        /// </summary>
        [Label("初盘结果")]
        public static readonly Property<InventoryResult?> FirstResultProperty = P<InventoryTaskFixtureIdAccount>.Register(e => e.FirstResult);

        /// <summary>
        /// 初盘结果
        /// </summary>
        public InventoryResult? FirstResult
        {
            get { return GetProperty(FirstResultProperty); }
            set { SetProperty(FirstResultProperty, value); }
        }
        #endregion

        #region 初盘人 FirstCounter
        /// <summary>
        /// 初盘人Id
        /// </summary>
        [Label("初盘人")]
        public static readonly IRefIdProperty FirstCounterIdProperty = P<InventoryTaskFixtureIdAccount>.RegisterRefId(e => e.FirstCounterId, ReferenceType.Normal);

        /// <summary>
        /// 初盘人Id
        /// </summary>
        public double? FirstCounterId
        {
            get { return (double?)GetRefNullableId(FirstCounterIdProperty); }
            set { SetRefNullableId(FirstCounterIdProperty, value); }
        }

        /// <summary>
        /// 初盘人
        /// </summary>
        public static readonly RefEntityProperty<Employee> FirstCounterProperty = P<InventoryTaskFixtureIdAccount>.RegisterRef(e => e.FirstCounter, FirstCounterIdProperty);

        /// <summary>
        /// 初盘人
        /// </summary>
        public Employee FirstCounter
        {
            get { return GetRefEntity(FirstCounterProperty); }
            set { SetRefEntity(FirstCounterProperty, value); }
        }
        #endregion

        #region 工治具编码 FixtureEncode
        /// <summary>
        /// 工治具编码Id
        /// </summary>
        [Label("工治具编码")]
        public static readonly IRefIdProperty FixtureEncodeIdProperty = P<InventoryTaskFixtureIdAccount>.RegisterRefId(e => e.FixtureEncodeId, ReferenceType.Normal);

        /// <summary>
        /// 工治具编码Id
        /// </summary>
        public double FixtureEncodeId
        {
            get { return (double)GetRefId(FixtureEncodeIdProperty); }
            set { SetRefId(FixtureEncodeIdProperty, value); }
        }

        /// <summary>
        /// 工治具编码
        /// </summary>
        public static readonly RefEntityProperty<FixtureEncode> FixtureEncodeProperty = P<InventoryTaskFixtureIdAccount>.RegisterRef(e => e.FixtureEncode, FixtureEncodeIdProperty);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public FixtureEncode FixtureEncode
        {
            get { return GetRefEntity(FixtureEncodeProperty); }
            set { SetRefEntity(FixtureEncodeProperty, value); }
        }
        #endregion

        #region 来源 InventoryAssetSource
        /// <summary>
        /// 来源
        /// </summary>
        [Label("盘点资产来源")]
        public static readonly Property<InventoryAssetSource> InventoryAssetSourceProperty = P<InventoryTaskFixtureIdAccount>.Register(e => e.InventoryAssetSource);

        /// <summary>
        /// 来源
        /// </summary>
        public InventoryAssetSource InventoryAssetSource
        {
            get { return GetProperty(InventoryAssetSourceProperty); }
            set { SetProperty(InventoryAssetSourceProperty, value); }
        }
        #endregion

        #region 初盘状态 FirstStatus
        /// <summary>
        /// 初盘状态
        /// </summary>
        [Label("初盘状态")]
        public static readonly Property<FixtureStatus?> FirstStatusProperty = P<InventoryTaskFixtureIdAccount>.Register(e => e.FirstStatus);

        /// <summary>
        /// 初盘状态
        /// </summary>
        public FixtureStatus? FirstStatus
        {
            get { return GetProperty(FirstStatusProperty); }
            set { SetProperty(FirstStatusProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<InventoryTaskFixtureIdAccount>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 库位Id
        /// </summary>
        public double? StorageLocationId
        {
            get { return (double?)GetRefNullableId(StorageLocationIdProperty); }
            set { SetRefNullableId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<InventoryTaskFixtureIdAccount>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 车间 Workshop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkshopIdProperty = P<InventoryTaskFixtureIdAccount>.RegisterRefId(e => e.WorkshopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkshopId
        {
            get { return (double?)GetRefNullableId(WorkshopIdProperty); }
            set { SetRefNullableId(WorkshopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkshopProperty = P<InventoryTaskFixtureIdAccount>.RegisterRef(e => e.Workshop, WorkshopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise Workshop
        {
            get { return GetRefEntity(WorkshopProperty); }
            set { SetRefEntity(WorkshopProperty, value); }
        }
        #endregion

        #region 复盘人 SecondCounter
        /// <summary>
        /// 复盘人Id
        /// </summary>
        [Label("复盘人")]
        public static readonly IRefIdProperty SecondCounterIdProperty = P<InventoryTaskFixtureIdAccount>.RegisterRefId(e => e.SecondCounterId, ReferenceType.Normal);

        /// <summary>
        /// 复盘人Id
        /// </summary>
        public double? SecondCounterId
        {
            get { return (double?)GetRefNullableId(SecondCounterIdProperty); }
            set { SetRefNullableId(SecondCounterIdProperty, value); }
        }

        /// <summary>
        /// 复盘人
        /// </summary>
        public static readonly RefEntityProperty<Employee> SecondCounterProperty = P<InventoryTaskFixtureIdAccount>.RegisterRef(e => e.SecondCounter, SecondCounterIdProperty);

        /// <summary>
        /// 复盘人
        /// </summary>
        public Employee SecondCounter
        {
            get { return GetRefEntity(SecondCounterProperty); }
            set { SetRefEntity(SecondCounterProperty, value); }
        }
        #endregion

        #region 复盘状态 SecondStatus
        /// <summary>
        /// 复盘状态
        /// </summary>
        [Label("复盘状态")]
        public static readonly Property<FixtureStatus?> SecondStatusProperty = P<InventoryTaskFixtureIdAccount>.Register(e => e.SecondStatus);

        /// <summary>
        /// 复盘状态
        /// </summary>
        public FixtureStatus? SecondStatus
        {
            get { return GetProperty(SecondStatusProperty); }
            set { SetProperty(SecondStatusProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<InventoryTaskFixtureIdAccount>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)GetRefNullableId(WarehouseIdProperty); }
            set { SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<InventoryTaskFixtureIdAccount>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 复盘结果 SecondResult
        /// <summary>
        /// 复盘结果
        /// </summary>
        [Label("复盘结果")]
        public static readonly Property<InventoryResult?> SecondResultProperty = P<InventoryTaskFixtureIdAccount>.Register(e => e.SecondResult);

        /// <summary>
        /// 复盘结果
        /// </summary>
        public InventoryResult? SecondResult
        {
            get { return GetProperty(SecondResultProperty); }
            set { SetProperty(SecondResultProperty, value); }
        }
        #endregion

        #region 产线 Resource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty ResourceIdProperty = P<InventoryTaskFixtureIdAccount>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<InventoryTaskFixtureIdAccount>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 盘点任务 InventoryTask
        /// <summary>
        /// 盘点任务Id
        /// </summary>
        [Label("盘点任务")]
        public static readonly IRefIdProperty InventoryTaskIdProperty = P<InventoryTaskFixtureIdAccount>.RegisterRefId(e => e.InventoryTaskId, ReferenceType.Parent);

        /// <summary>
        /// 盘点任务Id
        /// </summary>
        public double InventoryTaskId
        {
            get { return (double)GetRefId(InventoryTaskIdProperty); }
            set { SetRefId(InventoryTaskIdProperty, value); }
        }

        /// <summary>
        /// 盘点任务
        /// </summary>
        public static readonly RefEntityProperty<InventoryTask> InventoryTaskProperty = P<InventoryTaskFixtureIdAccount>.RegisterRef(e => e.InventoryTask, InventoryTaskIdProperty);

        /// <summary>
        /// 盘点任务
        /// </summary>
        public InventoryTask InventoryTask
        {
            get { return GetRefEntity(InventoryTaskProperty); }
            set { SetRefEntity(InventoryTaskProperty, value); }
        }
        #endregion


        #region 有初盘权限(界面属性) FirstPower
        /// <summary>
        /// 有初盘权限
        /// </summary>
        [Label("有初盘权限")]
        public static readonly Property<bool?> FirstPowerProperty = P<InventoryTaskFixtureIdAccount>.Register(e => e.FirstPower);

        /// <summary>
        /// 有初盘权限
        /// </summary>
        public bool? FirstPower
        {
            get { return this.GetProperty(FirstPowerProperty); }
            set { this.SetProperty(FirstPowerProperty, value); }
        }
        #endregion

        #region 有复盘权限(界面属性) SecondPower
        /// <summary>
        /// 有复盘权限
        /// </summary>
        [Label("有复盘权限")]
        public static readonly Property<bool?> SecondPowerProperty = P<InventoryTaskFixtureIdAccount>.Register(e => e.SecondPower);

        /// <summary>
        /// 有复盘权限
        /// </summary>
        public bool? SecondPower
        {
            get { return this.GetProperty(SecondPowerProperty); }
            set { this.SetProperty(SecondPowerProperty, value); }
        }
        #endregion


        #region 盘点任务状态 InventoryTaskStatus
        /// <summary>
        /// 盘点任务状态
        /// </summary>
        [Label("盘点任务状态")]
        public static readonly Property<InventoryTaskStatus> InventoryTaskStatusProperty = P<InventoryTaskFixtureIdAccount>.RegisterView(e => e.InventoryTaskStatus, p => p.InventoryTask.InventoryTaskStatus);

        /// <summary>
        /// 盘点任务状态
        /// </summary>
        public InventoryTaskStatus InventoryTaskStatus
        {
            get { return this.GetProperty(InventoryTaskStatusProperty); }
        }
        #endregion


        #region 计划Id InventoryPlanId	
        /// <summary>
        /// 计划Id
        /// </summary>
        [Label("计划Id")]
        public static readonly Property<double> InventoryPlanIdProperty = P<InventoryTaskFixtureIdAccount>.RegisterView(e => e.InventoryPlanId, p => p.InventoryTask.InventoryPlanId);

        /// <summary>
        /// 计划Id
        /// </summary>
        public double InventoryPlanId
        {
            get { return this.GetProperty(InventoryPlanIdProperty); }
        }
        #endregion


        #region 实盘合格数 PassQty
        /// <summary>
        /// 实盘合格数
        /// </summary>
        [Label("实盘合格数")]
        public static readonly Property<int> PassQtyProperty = P<InventoryTaskFixtureIdAccount>.Register(e => e.PassQty);

        /// <summary>
        /// 实盘合格数
        /// </summary>
        public int PassQty
        {
            get { return this.GetProperty(PassQtyProperty); }
            set { this.SetProperty(PassQtyProperty, value); }
        }
        #endregion


        #region 实盘不合格数 NgQty
        /// <summary>
        /// 实盘不合格数
        /// </summary>
        [Label("实盘不合格数")]
        public static readonly Property<int> NgQtyProperty = P<InventoryTaskFixtureIdAccount>.Register(e => e.NgQty);

        /// <summary>
        /// 实盘不合格数
        /// </summary>
        public int NgQty
        {
            get { return this.GetProperty(NgQtyProperty); }
            set { this.SetProperty(NgQtyProperty, value); }
        }
        #endregion


        #region 实盘在线数 OnlineQty
        /// <summary>
        /// 实盘在线数
        /// </summary>
        [Label("实盘在线数")]
        public static readonly Property<int> OnlineQtyProperty = P<InventoryTaskFixtureIdAccount>.Register(e => e.OnlineQty);

        /// <summary>
        /// 实盘在线数
        /// </summary>
        public int OnlineQty
        {
            get { return this.GetProperty(OnlineQtyProperty); }
            set { this.SetProperty(OnlineQtyProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 工治具编码 FixtureEncodeCode
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> FixtureEncodeCodeProperty = P<InventoryTaskFixtureIdAccount>.RegisterView(e => e.FixtureEncodeCode, p => p.FixtureEncode.Code);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string FixtureEncodeCode
        {
            get { return this.GetProperty(FixtureEncodeCodeProperty); }
        }
        #endregion

        #region 工治具型号名称 ModelName
        /// <summary>
        /// 工治具型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty = P<InventoryTaskFixtureIdAccount>.RegisterView(e => e.ModelName, p => p.FixtureEncode.FixtureModel.Name);

        /// <summary>
        /// 工治具型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
        }
        #endregion

        #region 工治具型号编码 ModelCode
        /// <summary>
        /// 工治具型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<InventoryTaskFixtureIdAccount>.RegisterView(e => e.ModelCode, p => p.FixtureEncode.FixtureModel.Code);

        /// <summary>
        /// 工治具型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 盘点任务工治具序列号明细 实体配置
    /// </summary>
    internal class InventoryTaskFixtureIdAccountConfig : EntityConfig<InventoryTaskFixtureIdAccount>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_INV_TSK_FIX_ID").MapAllProperties();
			Meta.Property(InventoryTaskFixtureIdAccount.FirstPowerProperty).DontMapColumn();
			Meta.Property(InventoryTaskFixtureIdAccount.SecondPowerProperty).DontMapColumn();

            Meta.Property(InventoryTaskFixtureIdAccount.PassQtyProperty).DontMapColumn();
            Meta.Property(InventoryTaskFixtureIdAccount.NgQtyProperty).DontMapColumn();
            Meta.Property(InventoryTaskFixtureIdAccount.OnlineQtyProperty).DontMapColumn();
            Meta.EnablePhantoms();
		}
	}
}