using SIE.Domain;
using SIE.EMS.Enums;
using SIE.Equipments.Enums;
using SIE.Fixtures;
using SIE.Fixtures.Models;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.InventoryTasks
{
	/// <summary>
	/// 盘点任务工治具编码明细
	/// </summary>
	[ChildEntity, Serializable]	
	[Label("盘点任务工治具编码明细")]
	public partial class InventoryTaskFixtureEncode : DataEntity
	{
		#region 平帐方式 BalancingWay
		/// <summary>
		/// 平帐方式
		/// </summary>
		[Label("平帐方式")]
        public static readonly Property<BalancingWay?> BalancingWayProperty = P<InventoryTaskFixtureEncode>.Register(e => e.BalancingWay);

        /// <summary>
        /// 平帐方式
        /// </summary>
        public BalancingWay? BalancingWay
        {
            get { return this.GetProperty(BalancingWayProperty); }
            set { this.SetProperty(BalancingWayProperty, value); }
        }
        #endregion


        #region 在库数 StockQty
        /// <summary>
        /// 在库数
        /// </summary>
        [Label("在库数")]
        public static readonly Property<int?> StockQtyProperty = P<InventoryTaskFixtureEncode>.Register(e => e.StockQty);

        /// <summary>
        /// 在库数
        /// </summary>
        public int? StockQty
        {
            get { return this.GetProperty(StockQtyProperty); }
            set { this.SetProperty(StockQtyProperty, value); }
        }
		#endregion

		#region 在库合格数 StockPassQty
		/// <summary>
		/// 在库合格数
		/// </summary>
		[Label("在库合格数")]
		public static readonly Property<int?> StockPassQtyProperty = P<InventoryTaskFixtureEncode>.Register(e => e.StockPassQty);

		/// <summary>
		/// 在库合格数
		/// </summary>
		public int? StockPassQty
		{
			get { return this.GetProperty(StockPassQtyProperty); }
			set { this.SetProperty(StockPassQtyProperty, value); }
		}
		#endregion

		#region 在库不合格数 StockNgQty
		/// <summary>
		/// 在库不合格数
		/// </summary>
		[Label("在库不合格数")]
		public static readonly Property<int?> StockNgQtyProperty = P<InventoryTaskFixtureEncode>.Register(e => e.StockNgQty);

		/// <summary>
		/// 在库不合格数
		/// </summary>
		public int? StockNgQty
		{
			get { return this.GetProperty(StockNgQtyProperty); }
			set { this.SetProperty(StockNgQtyProperty, value); }
		}
        #endregion

        #region 在库在线数 Online
        /// <summary>
        /// 在库在线数
        /// </summary>
        [Label("在库在线数")]
        public static readonly Property<int> OnlineProperty = P<InventoryTaskFixtureEncode>.Register(e => e.Online);

        /// <summary>
        /// 在库在线数
        /// </summary>
        public int Online
        {
            get { return GetProperty(OnlineProperty); }
            set { SetProperty(OnlineProperty, value); }
        }
        #endregion

        #region 总数 Total
        /// <summary>
        /// 总数
        /// </summary>
        [Label("总数")]
		public static readonly Property<int> TotalProperty = P<InventoryTaskFixtureEncode>.Register(e => e.Total);

		/// <summary>
		/// 总数
		/// </summary>
		public int Total
		{
			get { return GetProperty(TotalProperty); }
			set { SetProperty(TotalProperty, value); }
		}
		#endregion

		#region 初盘在库数 FirstStock
		/// <summary>
		/// 初盘在库数
		/// </summary>
		[Label("初盘在库数")]
        public static readonly Property<int?> FirstStockProperty = P<InventoryTaskFixtureEncode>.Register(e => e.FirstStock);

		/// <summary>
		/// 初盘在库数
		/// </summary>
		public int? FirstStock
		{
            get { return this.GetProperty(FirstStockProperty); }
            set { this.SetProperty(FirstStockProperty, value); }
        }
		#endregion

		#region 初盘在库合格数 FirstStockPassQty
		/// <summary>
		/// 初盘在库合格数
		/// </summary>
		[Label("初盘在库合格数")]
		public static readonly Property<int?> FirstStockPassQtyProperty = P<InventoryTaskFixtureEncode>.Register(e => e.FirstStockPassQty);

		/// <summary>
		/// 初盘在库合格数
		/// </summary>
		public int? FirstStockPassQty
		{
			get { return this.GetProperty(FirstStockPassQtyProperty); }
			set { this.SetProperty(FirstStockPassQtyProperty, value); }
		}
		#endregion

		#region 初盘在库不合格数 FirstStockNgQty
		/// <summary>
		/// 初盘在库不合格数
		/// </summary>
		[Label("初盘在库不合格数")]
		public static readonly Property<int?> FirstStockNgQtyProperty = P<InventoryTaskFixtureEncode>.Register(e => e.FirstStockNgQty);

		/// <summary>
		/// 初盘在库不合格数
		/// </summary>
		public int? FirstStockNgQty
		{
			get { return this.GetProperty(FirstStockNgQtyProperty); }
			set { this.SetProperty(FirstStockNgQtyProperty, value); }
		}
		#endregion

		#region 初盘在线数 FirstOnline
		/// <summary>
		/// 初盘在线数
		/// </summary>
		[Label("初盘在线数")]
		public static readonly Property<int?> FirstOnlineProperty = P<InventoryTaskFixtureEncode>.Register(e => e.FirstOnline);

		/// <summary>
		/// 初盘在线数
		/// </summary>
		public int? FirstOnline
		{
			get { return GetProperty(FirstOnlineProperty); }
			set { SetProperty(FirstOnlineProperty, value); }
		}
		#endregion

		#region 初盘总数 FirstTotal
		/// <summary>
		/// 初盘总数
		/// </summary>
		[Label("初盘总数")]
		public static readonly Property<int?> FirstTotalProperty = P<InventoryTaskFixtureEncode>.Register(e => e.FirstTotal);

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
		public static readonly Property<int?> FirstDiffProperty = P<InventoryTaskFixtureEncode>.Register(e => e.FirstDiff);

		/// <summary>
		/// 初盘差异数
		/// </summary>
		public int? FirstDiff
		{
			get { return GetProperty(FirstDiffProperty); }
			set { SetProperty(FirstDiffProperty, value); }
		}
		#endregion

		#region 复盘在库数 SecondStock	
		/// <summary>
		/// 复盘在库数
		/// </summary>
		[Label("复盘在库数")]
        public static readonly Property<int?> SecondStockProperty = P<InventoryTaskFixtureEncode>.Register(e => e.SecondStock);

		/// <summary>
		/// 复盘在库数
		/// </summary>
		public int? SecondStock
		{
            get { return this.GetProperty(SecondStockProperty); }
            set { this.SetProperty(SecondStockProperty, value); }
        }
		#endregion

		#region 复盘在库合格数 SecStockPassQty
		/// <summary>
		/// 复盘在库合格数
		/// </summary>
		[Label("复盘在库合格数")]
		public static readonly Property<int?> SecStockPassQtyProperty = P<InventoryTaskFixtureEncode>.Register(e => e.SecStockPassQty);

		/// <summary>
		/// 复盘在库合格数
		/// </summary>
		public int? SecStockPassQty
		{
			get { return this.GetProperty(SecStockPassQtyProperty); }
			set { this.SetProperty(SecStockPassQtyProperty, value); }
		}
		#endregion

		#region 复盘在库不合格数 SecStockNgQty
		/// <summary>
		/// 复盘在库不合格数
		/// </summary>
		[Label("复盘在库不合格数")]
		public static readonly Property<int?> SecStockNgQtyProperty = P<InventoryTaskFixtureEncode>.Register(e => e.SecStockNgQty);

		/// <summary>
		/// 复盘在库不合格数
		/// </summary>
		public int? SecStockNgQty
		{
			get { return this.GetProperty(SecStockNgQtyProperty); }
			set { this.SetProperty(SecStockNgQtyProperty, value); }
		}
		#endregion


		#region 复盘在线数 SecondOnline
		/// <summary>
		/// 复盘在线数
		/// </summary>
		[Label("复盘在线数")]
		public static readonly Property<int?> SecondOnlineProperty = P<InventoryTaskFixtureEncode>.Register(e => e.SecondOnline);

		/// <summary>
		/// 复盘在线数
		/// </summary>
		public int? SecondOnline
		{
			get { return GetProperty(SecondOnlineProperty); }
			set { SetProperty(SecondOnlineProperty, value); }
		}
		#endregion

		#region 复盘总数 SecondTotal
		/// <summary>
		/// 复盘总数
		/// </summary>
		[Label("复盘总数")]
		public static readonly Property<int?> SecondTotalProperty = P<InventoryTaskFixtureEncode>.Register(e => e.SecondTotal);

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
		public static readonly Property<int?> SecondDiffProperty = P<InventoryTaskFixtureEncode>.Register(e => e.SecondDiff);

		/// <summary>
		/// 复盘差异数
		/// </summary>
		public int? SecondDiff
		{
			get { return GetProperty(SecondDiffProperty); }
			set { SetProperty(SecondDiffProperty, value); }
		}
		#endregion

		#region 初盘时间 InventoryDateTime
		/// <summary>
		/// 初盘时间
		/// </summary>
		[Label("初盘时间")]
		public static readonly Property<DateTime?> InventoryDateTimeProperty = P<InventoryTaskFixtureEncode>.Register(e => e.InventoryDateTime);

		/// <summary>
		/// 初盘时间
		/// </summary>
		public DateTime? InventoryDateTime
		{
			get { return GetProperty(InventoryDateTimeProperty); }
			set { SetProperty(InventoryDateTimeProperty, value); }
		}
		#endregion

		#region 复盘时间 SecondDateTime
		/// <summary>
		/// 复盘时间
		/// </summary>
		[Label("复盘时间")]
		public static readonly Property<DateTime?> SecondDateTimeProperty = P<InventoryTaskFixtureEncode>.Register(e => e.SecondDateTime);

		/// <summary>
		/// 复盘时间
		/// </summary>
		public DateTime? SecondDateTime
		{
			get { return GetProperty(SecondDateTimeProperty); }
			set { SetProperty(SecondDateTimeProperty, value); }
		}
		#endregion

		#region 盘点资产来源 InventoryAssetSource
		/// <summary>
		/// 盘点资产来源
		/// </summary>
		[Label("盘点资产来源")]
		public static readonly Property<InventoryAssetSource> InventoryAssetSourceProperty = P<InventoryTaskFixtureEncode>.Register(e => e.InventoryAssetSource);

		/// <summary>
		/// 盘点资产来源
		/// </summary>
		public InventoryAssetSource InventoryAssetSource
		{
			get { return GetProperty(InventoryAssetSourceProperty); }
			set { SetProperty(InventoryAssetSourceProperty, value); }
		}
		#endregion

		#region 初盘结果 FirstResult
		/// <summary>
		/// 初盘结果
		/// </summary>
		[Label("初盘结果")]
		public static readonly Property<InventoryResult?> FirstResultProperty = P<InventoryTaskFixtureEncode>.Register(e => e.FirstResult);

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
		public static readonly IRefIdProperty FirstCounterIdProperty = P<InventoryTaskFixtureEncode>.RegisterRefId(e => e.FirstCounterId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Employee> FirstCounterProperty = P<InventoryTaskFixtureEncode>.RegisterRef(e => e.FirstCounter, FirstCounterIdProperty);

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
		public static readonly IRefIdProperty FixtureEncodeIdProperty = P<InventoryTaskFixtureEncode>.RegisterRefId(e => e.FixtureEncodeId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<FixtureEncode> FixtureEncodeProperty = P<InventoryTaskFixtureEncode>.RegisterRef(e => e.FixtureEncode, FixtureEncodeIdProperty);

		/// <summary>
		/// 工治具编码
		/// </summary>
		public FixtureEncode FixtureEncode
		{
			get { return GetRefEntity(FixtureEncodeProperty); }
			set { SetRefEntity(FixtureEncodeProperty, value); }
		}
		#endregion

		#region 盘点状态 InventoryStatus
		/// <summary>
		/// 盘点状态
		/// </summary>
		[Label("盘点状态")]
		public static readonly Property<InventoryStatus> InventoryStatusProperty = P<InventoryTaskFixtureEncode>.Register(e => e.InventoryStatus);

		/// <summary>
		/// 盘点状态
		/// </summary>
		public InventoryStatus InventoryStatus
		{
			get { return GetProperty(InventoryStatusProperty); }
			set { SetProperty(InventoryStatusProperty, value); }
		}
		#endregion

		#region 复盘结果 SecondResult
		/// <summary>
		/// 复盘结果
		/// </summary>
		[Label("复盘结果")]
		public static readonly Property<InventoryResult?> SecondResultProperty = P<InventoryTaskFixtureEncode>.Register(e => e.SecondResult);

		/// <summary>
		/// 复盘结果
		/// </summary>
		public InventoryResult? SecondResult
		{
			get { return GetProperty(SecondResultProperty); }
			set { SetProperty(SecondResultProperty, value); }
		}
		#endregion

		#region 复盘人 SecondCounter
		/// <summary>
		/// 复盘人Id
		/// </summary>
		[Label("复盘人")]
		public static readonly IRefIdProperty SecondCounterIdProperty = P<InventoryTaskFixtureEncode>.RegisterRefId(e => e.SecondCounterId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Employee> SecondCounterProperty = P<InventoryTaskFixtureEncode>.RegisterRef(e => e.SecondCounter, SecondCounterIdProperty);

		/// <summary>
		/// 复盘人
		/// </summary>
		public Employee SecondCounter
		{
			get { return GetRefEntity(SecondCounterProperty); }
			set { SetRefEntity(SecondCounterProperty, value); }
		}
		#endregion

		#region 盘点任务 InventoryTask
		/// <summary>
		/// 盘点任务Id
		/// </summary>
		[Label("盘点任务")]
		public static readonly IRefIdProperty InventoryTaskIdProperty = P<InventoryTaskFixtureEncode>.RegisterRefId(e => e.InventoryTaskId, ReferenceType.Parent);

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
		public static readonly RefEntityProperty<InventoryTask> InventoryTaskProperty = P<InventoryTaskFixtureEncode>.RegisterRef(e => e.InventoryTask, InventoryTaskIdProperty);

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
		public static readonly Property<bool?> FirstPowerProperty = P<InventoryTaskFixtureEncode>.Register(e => e.FirstPower);

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
		public static readonly Property<bool?> SecondPowerProperty = P<InventoryTaskFixtureEncode>.Register(e => e.SecondPower);

		/// <summary>
		/// 有复盘权限
		/// </summary>
		public bool? SecondPower
		{
			get { return this.GetProperty(SecondPowerProperty); }
			set { this.SetProperty(SecondPowerProperty, value); }
		}
		#endregion


		#region 视图属性
		#region 工治具型号编码 ModelCode
		/// <summary>
		/// 工治具型号编码
		/// </summary>
		[Label("型号编码")]
		public static readonly Property<string> ModelCodeProperty = P<InventoryTaskFixtureEncode>.RegisterView(e => e.ModelCode, p => p.FixtureEncode.FixtureModel.Code);

		/// <summary>
		/// 工治具型号编码
		/// </summary>
		public string ModelCode
		{
			get { return this.GetProperty(ModelCodeProperty); }
		}
        #endregion

        #region 工治具编码 FixtureEncodeCode
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> FixtureEncodeCodeProperty = P<InventoryTaskFixtureEncode>.RegisterView(e => e.FixtureEncodeCode, p => p.FixtureEncode.Code);

        /// <summary>
        /// 工治具型号编码
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
		public static readonly Property<string> ModelNameProperty = P<InventoryTaskFixtureEncode>.RegisterView(e => e.ModelName, p => p.FixtureEncode.FixtureModel.Name);

		/// <summary>
		/// 工治具型号名称
		/// </summary>
		public string ModelName
		{
			get { return this.GetProperty(ModelNameProperty); }
		}
		#endregion

		#region 工治具类型 FixtureType
		/// <summary>
		/// 工治具类型
		/// </summary>
		[Label("工治具类型")]
		public static readonly Property<string> FixtureTypeProperty = P<InventoryTaskFixtureEncode>.RegisterView(e => e.FixtureType, p => p.FixtureEncode.FixtureModel.FixtureType.Code);

		/// <summary>
		/// 工治具类型
		/// </summary>
		public string FixtureType
		{
			get { return this.GetProperty(FixtureTypeProperty); }
		}
        #endregion


        #region 管控方式 ManageMode
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ManageMode> ManageModeProperty = P<InventoryTaskFixtureEncode>.RegisterView(e => e.ManageMode, p => p.FixtureEncode.FixtureModel.ManageMode);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ManageMode ManageMode
        {
            get { return this.GetProperty(ManageModeProperty); }
        }
		#endregion


		#region 单位 Unit
		/// <summary>
		/// 单位
		/// </summary>
		[Label("单位")]
        public static readonly Property<string> UnitProperty = P<InventoryTaskFixtureEncode>.RegisterView(e => e.Unit, p => p.FixtureEncode.FixtureModel.Unit.Name);

		/// <summary>
		/// 单位
		/// </summary>
		public string Unit
        {
            get { return this.GetProperty(UnitProperty); }
        }
		#endregion


		#region 盘点任务状态 InventoryTaskStatus
		/// <summary>
		/// 盘点任务状态
		/// </summary>
		[Label("盘点任务状态")]
		public static readonly Property<InventoryTaskStatus> InventoryTaskStatusProperty = P<InventoryTaskFixtureEncode>.RegisterView(e => e.InventoryTaskStatus, p => p.InventoryTask.InventoryTaskStatus);

		/// <summary>
		/// 盘点任务状态
		/// </summary>
		public InventoryTaskStatus InventoryTaskStatus
		{
			get { return this.GetProperty(InventoryTaskStatusProperty); }
		}
		#endregion

		#region 审核状态 ApprovalStatus	
		/// <summary>
		/// 审核状态
		/// </summary>
		[Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<InventoryTaskFixtureEncode>.RegisterView(e => e.ApprovalStatus, p => p.InventoryTask.ApprovalStatus);

        /// <summary>
        /// 注释
        /// </summary>
        public ApprovalStatus ApprovalStatus
		{
            get { return this.GetProperty(ApprovalStatusProperty); }
        }
        #endregion


        #region 计划Id InventoryPlanId	
        /// <summary>
        /// 计划Id
        /// </summary>
        [Label("计划Id")]
        public static readonly Property<double> InventoryPlanIdProperty = P<InventoryTaskFixtureEncode>.RegisterView(e => e.InventoryPlanId, p => p.InventoryTask.InventoryPlanId);

        /// <summary>
        /// 注释
        /// </summary>
        public double InventoryPlanId
		{
            get { return this.GetProperty(InventoryPlanIdProperty); }
        }
        #endregion

        #region 实盘合格数 PassQty
        /// <summary>
        /// 实盘合格数(导入)
        /// </summary>
        [Label("实盘合格数")]
		public static readonly Property<int> PassQtyProperty = P<InventoryTaskFixtureEncode>.Register(e => e.PassQty);

		/// <summary>
		/// 实盘合格数
		/// </summary>
		public int PassQty
		{
			get { return this.GetProperty(PassQtyProperty); }
			set { this.SetProperty(PassQtyProperty, value); }
		}
        #endregion

        #region 在库合格数 LibPassQty
        /// <summary>
        /// 在库合格数
        /// </summary>
        [Label("在库合格数")]
        public static readonly Property<int> LibPassQtyProperty = P<InventoryTaskFixtureEncode>.Register(e => e.LibPassQty);

        /// <summary>
        /// 在库合格数
        /// </summary>
        public int LibPassQty
        {
            get { return this.GetProperty(LibPassQtyProperty); }
            set { this.SetProperty(LibPassQtyProperty, value); }
        }
        #endregion

        #region 复盘合格数 SecondPassQty
        /// <summary>
        /// 复盘合格数
        /// </summary>
        [Label("复盘合格数")]
        public static readonly Property<int> SecondPassQtyProperty = P<InventoryTaskFixtureEncode>.Register(e => e.SecondPassQty);

        /// <summary>
        /// 复盘合格数
        /// </summary>
        public int SecondPassQty
        {
            get { return this.GetProperty(SecondPassQtyProperty); }
            set { this.SetProperty(SecondPassQtyProperty, value); }
        }
        #endregion

        #region 实盘不合格数 NgQty
        /// <summary>
        /// 实盘不合格数(导入)
        /// </summary>
        [Label("实盘不合格数")]
		public static readonly Property<int> NgQtyProperty = P<InventoryTaskFixtureEncode>.Register(e => e.NgQty);

		/// <summary>
		/// 实盘不合格数
		/// </summary>
		public int NgQty
		{
			get { return this.GetProperty(NgQtyProperty); }
			set { this.SetProperty(NgQtyProperty, value); }
		}
        #endregion

        #region 在库不合格数 LibNgQty
        /// <summary>
        /// 在库不合格数
        /// </summary>
        [Label("在库不合格数")]
        public static readonly Property<int> LibNgQtyProperty = P<InventoryTaskFixtureEncode>.Register(e => e.LibNgQty);

        /// <summary>
        /// 在库不合格数
        /// </summary>
        public int LibNgQty
        {
            get { return this.GetProperty(LibNgQtyProperty); }
            set { this.SetProperty(LibNgQtyProperty, value); }
        }
        #endregion

        #region 复盘不合格数 SecondNgQty
        /// <summary>
        /// 复盘不合格数
        /// </summary>
        [Label("复盘不合格数")]
        public static readonly Property<int> SecondNgQtyProperty = P<InventoryTaskFixtureEncode>.Register(e => e.SecondNgQty);

        /// <summary>
        /// 复盘不合格数
        /// </summary>
        public int SecondNgQty
        {
            get { return this.GetProperty(SecondNgQtyProperty); }
            set { this.SetProperty(SecondNgQtyProperty, value); }
        }
        #endregion

        #region 实盘在线数 OnlineQty
        /// <summary>
        /// 实盘在线数(导入)
        /// </summary>
        [Label("实盘在线数")]
		public static readonly Property<int> OnlineQtyProperty = P<InventoryTaskFixtureEncode>.Register(e => e.OnlineQty);

		/// <summary>
		/// 实盘在线数
		/// </summary>
		public int OnlineQty
		{
			get { return this.GetProperty(OnlineQtyProperty); }
			set { this.SetProperty(OnlineQtyProperty, value); }
		}
		#endregion

		#region 工治具ID Sn
		/// <summary>
		/// 工治具ID(导入)
		/// </summary>
		[Label("工治具ID")]
		public static readonly Property<string> SnProperty = P<InventoryTaskFixtureEncode>.Register(e => e.Sn);

		/// <summary>
		/// 工治具ID
		/// </summary>
		public string Sn
		{
			get { return GetProperty(SnProperty); }
			set { SetProperty(SnProperty, value); }
		}
		#endregion


		#endregion
	}

	/// <summary>
	/// 盘点任务工治具编码明细 实体配置
	/// </summary>
	internal class InventoryTaskFixtureEncodeConfig : EntityConfig<InventoryTaskFixtureEncode>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_INV_TASK_FIX_CODE").MapAllProperties();
			Meta.Property(InventoryTaskFixtureEncode.FirstPowerProperty).DontMapColumn();
			Meta.Property(InventoryTaskFixtureEncode.SecondPowerProperty).DontMapColumn();

			//导入用
			Meta.Property(InventoryTaskFixtureEncode.SnProperty).DontMapColumn();
			Meta.Property(InventoryTaskFixtureEncode.PassQtyProperty).DontMapColumn();
			Meta.Property(InventoryTaskFixtureEncode.NgQtyProperty).DontMapColumn();
			Meta.Property(InventoryTaskFixtureEncode.OnlineQtyProperty).DontMapColumn();
			Meta.EnablePhantoms();
		}
	}
}