using SIE.Domain;
using SIE.Inventory.Commom;
using SIE.Inventory.Onhands;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Inventory.Piles
{
    /// <summary>
    /// 垛明细
    /// </summary>
    [ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("垛明细")]
	public partial class PileDetail : DataEntity
	{
		#region 条码 Sn
		/// <summary>
		/// 条码
		/// </summary>
		[Label("条码")]
		public static readonly Property<string> SnProperty = P<PileDetail>.Register(e => e.Sn);

		/// <summary>
		/// 条码
		/// </summary>
		public string Sn
		{
			get { return GetProperty(SnProperty); }
			set { SetProperty(SnProperty, value); }
		}
		#endregion

		#region 物料扩展属性 ItemExtProp
		/// <summary>
		/// 物料扩展属性
		/// </summary>
		[Label("物料扩展属性")]
		public static readonly Property<string> ItemExtPropProperty = P<PileDetail>.Register(e => e.ItemExtProp);

		/// <summary>
		/// 物料扩展属性
		/// </summary>
		public string ItemExtProp
		{
			get { return GetProperty(ItemExtPropProperty); }
			set { SetProperty(ItemExtPropProperty, value); }
		}
		#endregion

		#region 物料扩展属性 ItemExtPropName
		/// <summary>
		/// 物料扩展属性
		/// </summary>
		[Label("物料扩展属性")]
		public static readonly Property<string> ItemExtPropNameProperty = P<PileDetail>.Register(e => e.ItemExtPropName);

		/// <summary>
		/// 物料扩展属性
		/// </summary>
		public string ItemExtPropName
		{
			get { return GetProperty(ItemExtPropNameProperty); }
			set { SetProperty(ItemExtPropNameProperty, value); }
		}
		#endregion

		#region 货主 StorerCode
		/// <summary>
		/// 货主
		/// </summary>
		[Label("货主")]
		public static readonly Property<string> StorerCodeProperty = P<PileDetail>.Register(e => e.StorerCode);

		/// <summary>
		/// 货主
		/// </summary>
		public string StorerCode
		{
			get { return GetProperty(StorerCodeProperty); }
			set { SetProperty(StorerCodeProperty, value); }
		}
		#endregion

		#region 项目号 ProjectNo
		/// <summary>
		/// 项目号
		/// </summary>
		[Label("项目号")]
		public static readonly Property<string> ProjectNoProperty = P<PileDetail>.Register(e => e.ProjectNo);

		/// <summary>
		/// 项目号
		/// </summary>
		public string ProjectNo
		{
			get { return GetProperty(ProjectNoProperty); }
			set { SetProperty(ProjectNoProperty, value); }
		}
		#endregion

		#region 任务号 TaskNo
		/// <summary>
		/// 任务号
		/// </summary>
		[Label("任务号")]
		public static readonly Property<string> TaskNoProperty = P<PileDetail>.Register(e => e.TaskNo);

		/// <summary>
		/// 任务号
		/// </summary>
		public string TaskNo
		{
			get { return GetProperty(TaskNoProperty); }
			set { SetProperty(TaskNoProperty, value); }
		}
		#endregion

		#region 数量 Qty
		/// <summary>
		/// 数量
		/// </summary>
		[Label("数量")]
		public static readonly Property<decimal> QtyProperty = P<PileDetail>.Register(e => e.Qty);

		/// <summary>
		/// 数量
		/// </summary>
		public decimal Qty
		{
			get { return GetProperty(QtyProperty); }
			set { SetProperty(QtyProperty, value); }
		}
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<PileDetail>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)this.GetRefNullableId(ItemIdProperty); }
            set { this.SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<PileDetail>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 库存状态 OnhandState
        /// <summary>
        /// 库存状态
        /// </summary>
        [Label("库存状态")]
		public static readonly Property<OnhandState> OnhandStateProperty = P<PileDetail>.Register(e => e.OnhandState);

		/// <summary>
		/// 库存状态
		/// </summary>
		public OnhandState OnhandState
		{
			get { return GetProperty(OnhandStateProperty); }
			set { SetProperty(OnhandStateProperty, value); }
		}
		#endregion

		#region 批次 Lot
		/// <summary>
		/// 批次Id
		/// </summary>
		public static readonly IRefIdProperty LotIdProperty = P<PileDetail>.RegisterRefId(e => e.LotId, ReferenceType.Normal);

		/// <summary>
		/// 批次Id
		/// </summary>
		public double? LotId
		{
			get { return (double?)GetRefNullableId(LotIdProperty); }
			set { SetRefNullableId(LotIdProperty, value); }
		}

		/// <summary>
		/// 批次
		/// </summary>
		public static readonly RefEntityProperty<Lot> LotProperty = P<PileDetail>.RegisterRef(e => e.Lot, LotIdProperty);

		/// <summary>
		/// 批次
		/// </summary>
		public Lot Lot
		{
			get { return GetRefEntity(LotProperty); }
			set { SetRefEntity(LotProperty, value); }
		}
		#endregion

		#region 物料状态 ItemState
		/// <summary>
		/// 物料状态
		/// </summary>
		[Label("物料状态")]
		public static readonly Property<ItemState> ItemStateProperty = P<PileDetail>.Register(e => e.ItemState);

		/// <summary>
		/// 物料状态
		/// </summary>
		public ItemState ItemState
		{
			get { return GetProperty(ItemStateProperty); }
			set { SetProperty(ItemStateProperty, value); }
		}
		#endregion

		#region 垛表 Pile
		/// <summary>
		/// 垛表Id
		/// </summary>
		public static readonly IRefIdProperty PileIdProperty = P<PileDetail>.RegisterRefId(e => e.PileId, ReferenceType.Parent);

		/// <summary>
		/// 垛表Id
		/// </summary>
		public double PileId
		{
			get { return (double)GetRefId(PileIdProperty); }
			set { SetRefId(PileIdProperty, value); }
		}

		/// <summary>
		/// 垛表
		/// </summary>
		public static readonly RefEntityProperty<Pile> PileProperty = P<PileDetail>.RegisterRef(e => e.Pile, PileIdProperty);

		/// <summary>
		/// 垛表
		/// </summary>
		public Pile Pile
		{
			get { return GetRefEntity(PileProperty); }
			set { SetRefEntity(PileProperty, value); }
		}
		#endregion

		#region 仓库 Warehouse
		/// <summary>
		/// 仓库Id
		/// </summary>
		[Label("仓库")]
		public static readonly IRefIdProperty WarehouseIdProperty = P<PileDetail>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<PileDetail>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

		/// <summary>
		/// 仓库
		/// </summary>
		public Warehouse Warehouse
		{
			get { return GetRefEntity(WarehouseProperty); }
			set { SetRefEntity(WarehouseProperty, value); }
		}
		#endregion

		#region 库区 StorageArea
		/// <summary>
		/// 库区Id
		/// </summary>
		[Label("库区")]
		public static readonly IRefIdProperty StorageAreaIdProperty = P<PileDetail>.RegisterRefId(e => e.StorageAreaId, ReferenceType.Normal);

		/// <summary>
		/// 库区Id
		/// </summary>
		public double? StorageAreaId
		{
			get { return (double?)GetRefNullableId(StorageAreaIdProperty); }
			set { SetRefNullableId(StorageAreaIdProperty, value); }
		}

		/// <summary>
		/// 库区
		/// </summary>
		public static readonly RefEntityProperty<StorageArea> StorageAreaProperty = P<PileDetail>.RegisterRef(e => e.StorageArea, StorageAreaIdProperty);

		/// <summary>
		/// 库区
		/// </summary>
		public StorageArea StorageArea
		{
			get { return GetRefEntity(StorageAreaProperty); }
			set { SetRefEntity(StorageAreaProperty, value); }
		}
		#endregion

		#region 库位 StorageLocation
		/// <summary>
		/// 库位Id
		/// </summary>
		[Label("库位")]
		public static readonly IRefIdProperty StorageLocationIdProperty = P<PileDetail>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<PileDetail>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

		/// <summary>
		/// 库位
		/// </summary>
		public StorageLocation StorageLocation
		{
			get { return GetRefEntity(StorageLocationProperty); }
			set { SetRefEntity(StorageLocationProperty, value); }
		}
        #endregion

        #region 视图属性
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<PileDetail>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<PileDetail>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 物料主单位 ItemUnitName
        /// <summary>
        /// 物料主单位
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemUnitNameProperty = P<PileDetail>.RegisterView(e => e.ItemUnitName, p => p.Item.Unit.Name);

        /// <summary>
        /// 物料主单位
        /// </summary>
        public string ItemUnitName
        {
            get { return this.GetProperty(ItemUnitNameProperty); }
        }
        #endregion

        #region 批次号 LotCode
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotCodeProperty = P<PileDetail>.RegisterView(e => e.LotCode, p => p.Lot.Code);

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode
        {
            get { return this.GetProperty(LotCodeProperty); }
        }
        #endregion 
        #endregion
    }

	/// <summary>
	/// 垛明细 实体配置
	/// </summary>
	internal class PileDetailConfig : EntityConfig<PileDetail>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("PILE_DTL").MapAllProperties();
			Meta.EnablePhantoms();
			Meta.DisableDataSync();
		}
	}
}