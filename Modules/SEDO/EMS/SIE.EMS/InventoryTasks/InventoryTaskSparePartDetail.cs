using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Warehouses;
using System;

namespace SIE.EMS.InventoryTasks
{
    /// <summary>
    /// 盘点任务备件清单
    /// </summary>
    [ChildEntity, Serializable]
    [Label("盘点任务备件清单")]
    public partial class InventoryTaskSparePartDetail : DataEntity
    {
        #region 良品数 GoodQty
        /// <summary>
        /// 良品数
        /// </summary>
        [Label("良品数")]
        public static readonly Property<int> GoodQtyProperty = P<InventoryTaskSparePartDetail>.Register(e => e.GoodQty);

        /// <summary>
        /// 良品数
        /// </summary>
        public int GoodQty
        {
            get { return GetProperty(GoodQtyProperty); }
            set { SetProperty(GoodQtyProperty, value); }
        }
        #endregion

        #region 不良品数 NgQty
        /// <summary>
        /// 不良品数
        /// </summary>
        [Label("不良品数")]
        public static readonly Property<int> NgQtyProperty = P<InventoryTaskSparePartDetail>.Register(e => e.NgQty);

        /// <summary>
        /// 不良品数
        /// </summary>
        public int NgQty
        {
            get { return GetProperty(NgQtyProperty); }
            set { SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 总数量 Total
        /// <summary>
        /// 总数量
        /// </summary>
        [Label("总数量")]
        public static readonly Property<int> TotalProperty = P<InventoryTaskSparePartDetail>.Register(e => e.Total);

        /// <summary>
        /// 总数量
        /// </summary>
        public int Total
        {
            get { return GetProperty(TotalProperty); }
            set { SetProperty(TotalProperty, value); }
        }
        #endregion

        #region 初盘良品数 FirstGood
        /// <summary>
        /// 初盘良品数
        /// </summary>
        [Label("初盘良品数")]
        public static readonly Property<int?> FirstGoodProperty = P<InventoryTaskSparePartDetail>.Register(e => e.FirstGood);

        /// <summary>
        /// 初盘良品数
        /// </summary>
        public int? FirstGood
        {
            get { return GetProperty(FirstGoodProperty); }
            set { SetProperty(FirstGoodProperty, value); }
        }
        #endregion

        #region 初盘不良品数 FirstNg
        /// <summary>
        /// 初盘不良品数
        /// </summary>
        [Label("初盘不良品数")]
        public static readonly Property<int?> FirstNgProperty = P<InventoryTaskSparePartDetail>.Register(e => e.FirstNg);

        /// <summary>
        /// 初盘不良品数
        /// </summary>
        public int? FirstNg
        {
            get { return GetProperty(FirstNgProperty); }
            set { SetProperty(FirstNgProperty, value); }
        }
        #endregion

        #region 初盘总数 FirstTotal
        /// <summary>
        /// 初盘总数
        /// </summary>
        [Label("初盘总数")]
        public static readonly Property<int?> FirstTotalProperty = P<InventoryTaskSparePartDetail>.Register(e => e.FirstTotal);

        /// <summary>
        /// 初盘总数
        /// </summary>
        public int? FirstTotal
        {
            get { return GetProperty(FirstTotalProperty); }
            set { SetProperty(FirstTotalProperty, value); }
        }
        #endregion

        #region 初盘差异数 FirstDiff
        /// <summary>
        /// 初盘差异数
        /// </summary>
        [Label("初盘差异数")]
        public static readonly Property<int?> FirstDiffProperty = P<InventoryTaskSparePartDetail>.Register(e => e.FirstDiff);

        /// <summary>
        /// 初盘差异数
        /// </summary>
        public int? FirstDiff
        {
            get { return GetProperty(FirstDiffProperty); }
            set { SetProperty(FirstDiffProperty, value); }
        }
        #endregion

        #region 复盘良品数 SecondGoodQty
        /// <summary>
        /// 复盘良品数
        /// </summary>
        [Label("复盘良品数")]
        public static readonly Property<int?> SecondGoodQtyProperty = P<InventoryTaskSparePartDetail>.Register(e => e.SecondGoodQty);

        /// <summary>
        /// 复盘良品数
        /// </summary>
        public int? SecondGoodQty
        {
            get { return GetProperty(SecondGoodQtyProperty); }
            set { SetProperty(SecondGoodQtyProperty, value); }
        }
        #endregion

        #region 复盘不良品数 SecondNgQty
        /// <summary>
        /// 复盘不良品数
        /// </summary>
        [Label("复盘不良品数")]
        public static readonly Property<int?> SecondNgQtyProperty = P<InventoryTaskSparePartDetail>.Register(e => e.SecondNgQty);

        /// <summary>
        /// 复盘不良品数
        /// </summary>
        public int? SecondNgQty
        {
            get { return GetProperty(SecondNgQtyProperty); }
            set { SetProperty(SecondNgQtyProperty, value); }
        }
        #endregion

        #region 复盘总数 SecondTotal
        /// <summary>
        /// 复盘总数
        /// </summary>
        [Label("复盘总数")]
        public static readonly Property<int?> SecondTotalProperty = P<InventoryTaskSparePartDetail>.Register(e => e.SecondTotal);

        /// <summary>
        /// 复盘总数
        /// </summary>
        public int? SecondTotal
        {
            get { return GetProperty(SecondTotalProperty); }
            set { SetProperty(SecondTotalProperty, value); }
        }
        #endregion

        #region 复盘差异数 SecondDiff
        /// <summary>
        /// 复盘差异数
        /// </summary>
        [Label("复盘差异数")]
        public static readonly Property<int?> SecondDiffProperty = P<InventoryTaskSparePartDetail>.Register(e => e.SecondDiff);

        /// <summary>
        /// 复盘差异数
        /// </summary>
        public int? SecondDiff
        {
            get { return GetProperty(SecondDiffProperty); }
            set { SetProperty(SecondDiffProperty, value); }
        }
        #endregion

        #region 批次号 LotNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotNoProperty = P<InventoryTaskSparePartDetail>.Register(e => e.LotNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotNo
        {
            get { return GetProperty(LotNoProperty); }
            set { SetProperty(LotNoProperty, value); }
        }
        #endregion

		#region 批次号 StoreSummaryLot
		/// <summary>
		/// 批次号Id
		/// </summary>
		[Label("批次号")]
		public static readonly IRefIdProperty StoreSummaryLotIdProperty =
			P<InventoryTaskSparePartDetail>.RegisterRefId(e => e.StoreSummaryLotId, ReferenceType.Normal);

		/// <summary>
		/// 批次号Id
		/// </summary>
		public double? StoreSummaryLotId
		{
			get { return (double?)this.GetRefNullableId(StoreSummaryLotIdProperty); }
			set { this.SetRefNullableId(StoreSummaryLotIdProperty, value); }
		}

		/// <summary>
		/// 批次号
		/// </summary>
		public static readonly RefEntityProperty<StoreSummaryLot> StoreSummaryLotProperty =
			P<InventoryTaskSparePartDetail>.RegisterRef(e => e.StoreSummaryLot, StoreSummaryLotIdProperty);

		/// <summary>
		/// 批次号
		/// </summary>
		public StoreSummaryLot StoreSummaryLot
		{
			get { return this.GetRefEntity(StoreSummaryLotProperty); }
			set { this.SetRefEntity(StoreSummaryLotProperty, value); }
		}
		#endregion

        #region 序列号 Sn
        /// <summary>
        /// 序列号
        /// </summary>
        [Label("序列号")]
        public static readonly Property<string> SnProperty = P<InventoryTaskSparePartDetail>.Register(e => e.Sn);

		/// <summary>
		/// 序列号
		/// </summary>
		public string Sn
		{
			get { return GetProperty(SnProperty); }
			set { SetProperty(SnProperty, value); }
		}
		#endregion

		#region 序列号 StoreSummaryDetail
		/// <summary>
		/// 序列号Id
		/// </summary>
		[Label("序列号")]
		public static readonly IRefIdProperty StoreSummaryDetailIdProperty =
			P<InventoryTaskSparePartDetail>.RegisterRefId(e => e.StoreSummaryDetailId, ReferenceType.Normal);

		/// <summary>
		/// 序列号Id
		/// </summary>
		public double? StoreSummaryDetailId
		{
			get { return (double?)this.GetRefNullableId(StoreSummaryDetailIdProperty); }
			set { this.SetRefNullableId(StoreSummaryDetailIdProperty, value); }
		}

		/// <summary>
		/// 序列号
		/// </summary>
		public static readonly RefEntityProperty<StoreSummaryDetail> StoreSummaryDetailProperty =
			P<InventoryTaskSparePartDetail>.RegisterRef(e => e.StoreSummaryDetail, StoreSummaryDetailIdProperty);

		/// <summary>
		/// 序列号
		/// </summary>
		public StoreSummaryDetail StoreSummaryDetail
		{
			get { return this.GetRefEntity(StoreSummaryDetailProperty); }
			set { this.SetRefEntity(StoreSummaryDetailProperty, value); }
		}
		#endregion

        #region 初盘时间 FirstDateTime
        /// <summary>
        /// 初盘时间
        /// </summary>
        [Label("初盘时间")]
        public static readonly Property<DateTime?> FirstDateTimeProperty = P<InventoryTaskSparePartDetail>.Register(e => e.FirstDateTime);

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
        public static readonly Property<DateTime?> SecondDateTimeProperty = P<InventoryTaskSparePartDetail>.Register(e => e.SecondDateTime);

        /// <summary>
        /// 复盘时间
        /// </summary>
        public DateTime? SecondDateTime
        {
            get { return GetProperty(SecondDateTimeProperty); }
            set { SetProperty(SecondDateTimeProperty, value); }
        }
        #endregion

        #region 状态 InventoryStatus
        /// <summary>
        /// 状态
        /// </summary>
        [Label("盘点状态")]
        public static readonly Property<InventoryStatus> InventoryStatusProperty = P<InventoryTaskSparePartDetail>.Register(e => e.InventoryStatus);

        /// <summary>
        /// 状态
        /// </summary>
        public InventoryStatus InventoryStatus
        {
            get { return GetProperty(InventoryStatusProperty); }
            set { SetProperty(InventoryStatusProperty, value); }
        }
        #endregion

        #region 来源 InventoryAssetSource
        /// <summary>
        /// 来源
        /// </summary>
        [Label("盘点资产来源")]
        public static readonly Property<InventoryAssetSource> InventoryAssetSourceProperty = P<InventoryTaskSparePartDetail>.Register(e => e.InventoryAssetSource);

        /// <summary>
        /// 来源
        /// </summary>
        public InventoryAssetSource InventoryAssetSource
        {
            get { return GetProperty(InventoryAssetSourceProperty); }
            set { SetProperty(InventoryAssetSourceProperty, value); }
        }
        #endregion

        #region 初盘人 FirstCounter
        /// <summary>
        /// 初盘人Id
        /// </summary>
        [Label("初盘人")]
        public static readonly IRefIdProperty FirstCounterIdProperty = P<InventoryTaskSparePartDetail>.RegisterRefId(e => e.FirstCounterId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> FirstCounterProperty = P<InventoryTaskSparePartDetail>.RegisterRef(e => e.FirstCounter, FirstCounterIdProperty);

        /// <summary>
        /// 初盘人
        /// </summary>
        public Employee FirstCounter
        {
            get { return GetRefEntity(FirstCounterProperty); }
            set { SetRefEntity(FirstCounterProperty, value); }
        }
        #endregion

        #region 初盘结果 FirstResult
        /// <summary>
        /// 初盘结果
        /// </summary>
        [Label("初盘结果")]
        public static readonly Property<InventoryResult?> FirstResultProperty = P<InventoryTaskSparePartDetail>.Register(e => e.FirstResult);

        /// <summary>
        /// 初盘结果
        /// </summary>
        public InventoryResult? FirstResult
        {
            get { return GetProperty(FirstResultProperty); }
            set { SetProperty(FirstResultProperty, value); }
        }
        #endregion

        #region 备件 SparePart
        /// <summary>
        /// 备件Id
        /// </summary>
        [Label("备件")]
        public static readonly IRefIdProperty SparePartIdProperty = P<InventoryTaskSparePartDetail>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件Id
        /// </summary>
        public double SparePartId
        {
            get { return (double)GetRefId(SparePartIdProperty); }
            set { SetRefId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty = P<InventoryTaskSparePartDetail>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件
        /// </summary>
        public SparePart SparePart
        {
            get { return GetRefEntity(SparePartProperty); }
            set { SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<InventoryTaskSparePartDetail>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<InventoryTaskSparePartDetail>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 复盘人 SecondCounter
        /// <summary>
        /// 复盘人Id
        /// </summary>
        [Label("复盘人")]
        public static readonly IRefIdProperty SecondCounterIdProperty = P<InventoryTaskSparePartDetail>.RegisterRefId(e => e.SecondCounterId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> SecondCounterProperty = P<InventoryTaskSparePartDetail>.RegisterRef(e => e.SecondCounter, SecondCounterIdProperty);

        /// <summary>
        /// 复盘人
        /// </summary>
        public Employee SecondCounter
        {
            get { return GetRefEntity(SecondCounterProperty); }
            set { SetRefEntity(SecondCounterProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<InventoryTaskSparePartDetail>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<InventoryTaskSparePartDetail>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

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
        public static readonly Property<InventoryResult?> SecondResultProperty = P<InventoryTaskSparePartDetail>.Register(e => e.SecondResult);

        /// <summary>
        /// 复盘结果
        /// </summary>
        public InventoryResult? SecondResult
        {
            get { return GetProperty(SecondResultProperty); }
            set { SetProperty(SecondResultProperty, value); }
        }
        #endregion

        #region 盘点任务 InventoryTask
        /// <summary>
        /// 盘点任务Id
        /// </summary>
        [Label("盘点任务")]
        public static readonly IRefIdProperty InventoryTaskIdProperty = P<InventoryTaskSparePartDetail>.RegisterRefId(e => e.InventoryTaskId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<InventoryTask> InventoryTaskProperty = P<InventoryTaskSparePartDetail>.RegisterRef(e => e.InventoryTask, InventoryTaskIdProperty);

        /// <summary>
        /// 盘点任务
        /// </summary>
        public InventoryTask InventoryTask
        {
            get { return GetRefEntity(InventoryTaskProperty); }
            set { SetRefEntity(InventoryTaskProperty, value); }
        }
        #endregion

		#region 平账方式 SparePartProcessMethod
		/// <summary>
		///  平账方式
		/// </summary>
		[Label("平账方式")]
		public static readonly Property<SparePartProcessMethod?> SparePartProcessMethodProperty = P<InventoryTaskSparePartDetail>.Register(e => e.SparePartProcessMethod);

		/// <summary>
		/// 平账方式
		/// </summary>
		public SparePartProcessMethod? SparePartProcessMethod
		{
			get { return GetProperty(SparePartProcessMethodProperty); }
			set { SetProperty(SparePartProcessMethodProperty, value); }
		}
		#endregion

		#region 视图属性
		#region 审核状态 ApprovalStatus
		/// <summary>
		/// 审核状态
		/// </summary>
		[Label("审核状态")]
		public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<InventoryTaskSparePartDetail>.RegisterView(e => e.ApprovalStatus, p => p.InventoryTask.ApprovalStatus);

		/// <summary>
		/// 审核状态
		/// </summary>
		public ApprovalStatus ApprovalStatus
		{
			get { return this.GetProperty(ApprovalStatusProperty); }
		}
		#endregion

        #region 盘点单状态 InventoryTaskStatus
        /// <summary>
        /// 盘点单状态
        /// </summary>
        [Label("盘点单状态")]
        public static readonly Property<InventoryTaskStatus> InventoryTaskStatusProperty = P<InventoryTaskSparePartDetail>.RegisterView(e => e.InventoryTaskStatus, p => p.InventoryTask.InventoryTaskStatus);

        /// <summary>
        /// 盘点单状态
        /// </summary>
        public InventoryTaskStatus InventoryTaskStatus
        {
            get { return this.GetProperty(InventoryTaskStatusProperty); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<InventoryTaskSparePartDetail>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod> ControlMethodProperty = P<InventoryTaskSparePartDetail>.RegisterView(e => e.ControlMethod, p => p.SparePart.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
        }
        #endregion

        #region 型号规格 Specification
        /// <summary>
        /// 型号规格
        /// </summary>
        [Label("型号规格")]
        public static readonly Property<string> SpecificationProperty = P<InventoryTaskSparePartDetail>.RegisterView(e => e.Specification, p => p.SparePart.Specification);

        /// <summary>
        /// 型号规格
        /// </summary>
        public string Specification
        {
            get { return this.GetProperty(SpecificationProperty); }
        }
        #endregion

        #region 分类层级 ItemCategoryName
        /// <summary>
        /// 分类层级
        /// </summary>
        [Label("分类层级")]
        public static readonly Property<string> ItemCategoryNameProperty = P<InventoryTaskSparePartDetail>.RegisterView(e => e.ItemCategoryName, p => p.SparePart.ItemCategory.Name);

        /// <summary>
        /// 分类层级
        /// </summary>
        public string ItemCategoryName
        {
            get { return this.GetProperty(ItemCategoryNameProperty); }
        }
        #endregion

        #region 类型 SpartType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<SparePartType> SpartTypeProperty = P<InventoryTaskSparePartDetail>.RegisterView(e => e.SpartType, p => p.SparePart.SpartType);

        /// <summary>
        /// 类型
        /// </summary>
        public SparePartType SpartType
        {
            get { return this.GetProperty(SpartTypeProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<InventoryTaskSparePartDetail>.RegisterView(e => e.UnitName, p => p.SparePart.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 备件任务单号 InventoryTaskNo
        /// <summary>
        /// 备件任务单号
        /// </summary>
        [Label("盘点任务单号")]
        public static readonly Property<string> InventoryTaskNoProperty = P<InventoryTaskSparePartDetail>.RegisterView(e => e.InventoryTaskNo, p => p.InventoryTask.TaskNo);

        /// <summary>
        /// 备件任务单号
        /// </summary>
        public string InventoryTaskNo
        {
            get { return this.GetProperty(InventoryTaskNoProperty); }
        }
        #endregion

        #region 库位编码 StorageLocationName	
        /// <summary>
        /// 库位编码
        /// </summary>
        [Label("库位编码")]
        public static readonly Property<string> StorageLocationNameProperty = P<InventoryTaskSparePartDetail>.RegisterView(e => e.StorageLocationName, p => p.StorageLocation.Name);

        /// <summary>
        /// 库位编码
        /// </summary>
        public string StorageLocationName
        {
            get { return this.GetProperty(StorageLocationNameProperty); }
        }
        #endregion

        #region 备件编码 SparePartCode	
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<InventoryTaskSparePartDetail>.RegisterView(e => e.SparePartCode, p => p.SparePart.SparePartCode);

        /// <summary>
        /// 注释
        /// </summary>
        public string SparePartCode
        {
            get { return this.GetProperty(SparePartCodeProperty); }
        }
        #endregion

        #region 执行类型 InventoryExecuteType	
        /// <summary>
        /// 执行类型
        /// </summary>
        [Label("执行类型")]
        public static readonly Property<InventoryExecuteType> InventoryExecuteTypeProperty = P<InventoryTaskSparePartDetail>.RegisterView(e => e.InventoryExecuteType, p => p.InventoryTask.InventoryExecuteType);

        /// <summary>
        /// 执行类型
        /// </summary>
        public InventoryExecuteType InventoryExecuteType
        {
            get { return this.GetProperty(InventoryExecuteTypeProperty); }
        }
        #endregion

        #endregion

        #region 不映射数据库
        #region 有初盘权限(界面属性) FirstPower
        /// <summary>
        /// 有初盘权限
        /// </summary>
        [Label("有初盘权限")]
        public static readonly Property<bool?> FirstPowerProperty = P<InventoryTaskSparePartDetail>.Register(e => e.FirstPower);

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
        public static readonly Property<bool?> SecondPowerProperty = P<InventoryTaskSparePartDetail>.Register(e => e.SecondPower);

        /// <summary>
        /// 有复盘权限
        /// </summary>
        public bool? SecondPower
        {
            get { return this.GetProperty(SecondPowerProperty); }
            set { this.SetProperty(SecondPowerProperty, value); }
        }
        #endregion

        #region 库位编码 StorageLocationCode	
        /// <summary>
        /// 库位编码
        /// </summary>
        [Label("库位编码")]
        public static readonly Property<string> StorageLocationCodeProperty = P<InventoryTaskSparePartDetail>.Register(e => e.StorageLocationCode);

        /// <summary>
        /// 库位编码
        /// </summary>
        public string StorageLocationCode
        {
            get { return this.GetProperty(StorageLocationCodeProperty); }
            set { this.SetProperty(StorageLocationCodeProperty, value); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 盘点任务备件清单 实体配置
    /// </summary>
    internal class InventoryTaskSparePartDetailConfig : EntityConfig<InventoryTaskSparePartDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_INV_TSK_SP_DTL").MapAllProperties();
            Meta.Property(InventoryTaskSparePartDetail.FirstPowerProperty).DontMapColumn();
            Meta.Property(InventoryTaskSparePartDetail.SecondPowerProperty).DontMapColumn();
            Meta.Property(InventoryTaskSparePartDetail.StorageLocationCodeProperty).DontMapColumn();

            Meta.EnablePhantoms();
        }
    }
}