using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.SpareParts
{
	/// <summary>
	/// 备件仓入库查询实体
	/// </summary>
	[QueryEntity, Serializable]
	[Label("备件仓入库查询实体")]
	public partial class SparePartStoreCriteria : Criteria
	{
		#region 备件入库单号 StoreCode
		/// <summary>
		/// 备件入库单号
		/// </summary>
		[Label("备件入库单号")]
		public static readonly Property<string> StoreCodeProperty = P<SparePartStoreCriteria>.Register(e => e.StoreCode);

		/// <summary>
		/// 备件入库单号
		/// </summary>
		public string StoreCode
		{
			get { return GetProperty(StoreCodeProperty); }
			set { SetProperty(StoreCodeProperty, value); }
		}
		#endregion

		#region 入库类型 InboundType
		/// <summary>
		/// 入库类型
		/// </summary>
		[Label("入库类型")]
		public static readonly Property<SparePartInboundType?> InboundTypeProperty = P<SparePartStoreCriteria>.Register(e => e.InboundType);

		/// <summary>
		/// 入库类型
		/// </summary>
		public SparePartInboundType? InboundType
		{
			get { return GetProperty(InboundTypeProperty); }
			set { SetProperty(InboundTypeProperty, value); }
		}
		#endregion

		#region 相关单号 LinkCode
		/// <summary>
		/// 相关单号
		/// </summary>
		[Label("相关单号")]
		public static readonly Property<string> LinkCodeProperty = P<SparePartStoreCriteria>.Register(e => e.LinkCode);

		/// <summary>
		/// 相关单号
		/// </summary>
		public string LinkCode
		{
			get { return GetProperty(LinkCodeProperty); }
			set { SetProperty(LinkCodeProperty, value); }
		}
		#endregion

		#region 接收单号 ReceiveNo
		/// <summary>
		/// 接收单号
		/// </summary>
		[Label("接收单号")]
		public static readonly Property<string> ReceiveNoProperty = P<SparePartStoreCriteria>.Register(e => e.ReceiveNo);

		/// <summary>
		/// 接收单号
		/// </summary>
		public string ReceiveNo
		{
			get { return GetProperty(ReceiveNoProperty); }
			set { SetProperty(ReceiveNoProperty, value); }
		}
		#endregion

		#region 验收单号 AcceptanceNo
		/// <summary>
		/// 验收单号
		/// </summary>
		[Label("验收单号")]
		public static readonly Property<string> AcceptanceNoProperty = P<SparePartStoreCriteria>.Register(e => e.AcceptanceNo);

		/// <summary>
		/// 验收单号
		/// </summary>
		public string AcceptanceNo
		{
			get { return GetProperty(AcceptanceNoProperty); }
			set { SetProperty(AcceptanceNoProperty, value); }
		}
		#endregion

		#region 处置单号 DisposalNo
		/// <summary>
		/// 处置单号
		/// </summary>
		[Label("处置单号")]
		public static readonly Property<string> DisposalNoProperty = P<SparePartStoreCriteria>.Register(e => e.DisposalNo);

		/// <summary>
		/// 处置单号
		/// </summary>
		public string DisposalNo
		{
			get { return this.GetProperty(DisposalNoProperty); }
			set { this.SetProperty(DisposalNoProperty, value); }
		}
		#endregion

		#region 状态 InboundStatus
		/// <summary>
		/// 状态
		/// </summary>
		[Label("状态")]
		public static readonly Property<InboundStatus?> InboundStatusProperty
			= P<SparePartStoreCriteria>.Register(e => e.InboundStatus);

		/// <summary>
		/// 状态
		/// </summary>
		public InboundStatus? InboundStatus
		{
			get { return GetProperty(InboundStatusProperty); }
			set { SetProperty(InboundStatusProperty, value); }
		}
		#endregion

		#region 创建时间 CreateDate
		/// <summary>
		/// 创建时间
		/// </summary>
		[Label("创建时间")]
		public static readonly Property<DateRange> CreateDateProperty = P<SparePartStoreCriteria>.Register(e => e.CreateDate);

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateRange CreateDate
		{
			get { return GetProperty(CreateDateProperty); }
			set { SetProperty(CreateDateProperty, value); }
		}
		#endregion

		#region 入库仓库 Warehouse
		/// <summary>
		/// 入库仓库Id
		/// </summary>
		[Label("入库仓库")]
		public static readonly IRefIdProperty WarehouseIdProperty
			= P<SparePartStoreCriteria>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

		/// <summary>
		/// 入库仓库Id
		/// </summary>
		public double? WarehouseId
		{
			get { return (double?)GetRefNullableId(WarehouseIdProperty); }
			set { SetRefNullableId(WarehouseIdProperty, value); }
		}

		/// <summary>
		/// 入库仓库
		/// </summary>
		public static readonly RefEntityProperty<Warehouse> WarehouseProperty
			= P<SparePartStoreCriteria>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

		/// <summary>
		/// 入库仓库
		/// </summary>
		public Warehouse Warehouse
		{
			get { return GetRefEntity(WarehouseProperty); }
			set { SetRefEntity(WarehouseProperty, value); }
		}
		#endregion

		#region 供应商 Supplier
		/// <summary>
		/// 供应商Id
		/// </summary>
		[Label("供应商")]
		public static readonly IRefIdProperty SupplierIdProperty = P<SparePartStoreCriteria>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

		/// <summary>
		/// 供应商Id
		/// </summary>
		public double? SupplierId
		{
			get { return (double?)GetRefNullableId(SupplierIdProperty); }
			set { SetRefNullableId(SupplierIdProperty, value); }
		}

		/// <summary>
		/// 供应商
		/// </summary>
		public static readonly RefEntityProperty<Supplier> SupplierProperty = P<SparePartStoreCriteria>.RegisterRef(e => e.Supplier, SupplierIdProperty);

		/// <summary>
		/// 供应商
		/// </summary>
		public Supplier Supplier
		{
			get { return GetRefEntity(SupplierProperty); }
			set { SetRefEntity(SupplierProperty, value); }
		}
		#endregion

		#region 备件基础数据 SparePart
		/// <summary>
		/// 备件基础数据Id
		/// </summary>
		[Label("备件编码")]
		public static readonly IRefIdProperty SparePartIdProperty = P<SparePartStoreCriteria>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

		/// <summary>
		/// 备件基础数据Id
		/// </summary>
		public double SparePartId
		{
			get { return (double)GetRefId(SparePartIdProperty); }
			set { SetRefId(SparePartIdProperty, value); }
		}

		/// <summary>
		/// 备件基础数据
		/// </summary>
		public static readonly RefEntityProperty<SparePart> SparePartProperty = P<SparePartStoreCriteria>.RegisterRef(e => e.SparePart, SparePartIdProperty);

		/// <summary>
		/// 备件基础数据
		/// </summary>
		public SparePart SparePart
		{
			get { return GetRefEntity(SparePartProperty); }
			set { SetRefEntity(SparePartProperty, value); }
		}
		#endregion

		#region 备件编码 SparePartCode
		/// <summary>
		/// 备件编码
		/// </summary>
		[Label("备件编码")]
		public static readonly Property<string> SparePartCodeProperty = P<SparePartStoreCriteria>.RegisterView(e => e.SparePartCode, p => p.SparePart.SparePartCode);

		/// <summary>
		/// 备件编码
		/// </summary>
		public string SparePartCode
		{
			get { return GetProperty(SparePartCodeProperty); }
			set { SetProperty(SparePartCodeProperty, value); }
		}
		#endregion

		#region 备件名称 SparePartName
		/// <summary>
		/// 备件名称
		/// </summary>
		[Label("备件名称")]
		public static readonly Property<string> SparePartNameProperty = P<SparePartStoreCriteria>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

		/// <summary>
		/// 备件名称
		/// </summary>
		public string SparePartName
		{
			get { return GetProperty(SparePartNameProperty); }
			set { SetProperty(SparePartNameProperty, value); }
		}
		#endregion

		/// <summary>
		/// 重写此方法实现查询
		/// </summary>
		protected override EntityList Fetch()
		{
			return RT.Service.Resolve<SparePartController>().GetSparePartStoreList(this);
		}
	}
}
