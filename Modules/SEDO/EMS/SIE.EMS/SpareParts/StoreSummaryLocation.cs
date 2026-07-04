using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 物料编码明细
    /// </summary>
    [ChildEntity, Serializable]
	[Label("物料编码明细")]
	public partial class StoreSummaryLocation : DataEntity
	{
		#region 不良品数 RotNumber
		/// <summary>
		/// 不良品数
		/// </summary>
		[Label("不良品数")]
		public static readonly Property<int> RotNumberProperty = P<StoreSummaryLocation>.Register(e => e.RotNumber);

		/// <summary>
		/// 不良品数
		/// </summary>
		public int RotNumber
		{
			get { return GetProperty(RotNumberProperty); }
			set { SetProperty(RotNumberProperty, value); }
		}
		#endregion

		#region 可用库存 GoodNumber
		/// <summary>
		/// 可用库存
		/// </summary>
		[Label("可用库存")]
		public static readonly Property<int> GoodNumberProperty = P<StoreSummaryLocation>.Register(e => e.GoodNumber);

		/// <summary>
		/// 可用库存
		/// </summary>
		public int GoodNumber
		{
			get { return GetProperty(GoodNumberProperty); }
			set { SetProperty(GoodNumberProperty, value); }
		}
		#endregion

		#region 总库存 SumNumber
		/// <summary>
		/// 总库存
		/// </summary>
		[Label("总库存")]
		public static readonly Property<int> SumNumberProperty = P<StoreSummaryLocation>.Register(e => e.SumNumber);

		/// <summary>
		/// 总库存
		/// </summary>
		public int SumNumber
		{
			get { return GetProperty(SumNumberProperty); }
			set { SetProperty(SumNumberProperty, value); }
		}
		#endregion

		#region 仓库 Warehouse
		/// <summary>
		/// 仓库Id
		/// </summary>
		[Label("仓库编码")]
		public static readonly IRefIdProperty WarehouseIdProperty = P<StoreSummaryLocation>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

		/// <summary>
		/// 仓库Id
		/// </summary>
		public double WarehouseId
		{
			get { return (double)GetRefId(WarehouseIdProperty); }
			set { SetRefId(WarehouseIdProperty, value); }
		}

		/// <summary>
		/// 仓库
		/// </summary>
		public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<StoreSummaryLocation>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

		/// <summary>
		/// 仓库
		/// </summary>
		public Warehouse Warehouse
		{
			get { return GetRefEntity(WarehouseProperty); }
			set { SetRefEntity(WarehouseProperty, value); }
		}
		#endregion

		#region 库位 StorageLocation
		/// <summary>
		/// 库位Id
		/// </summary>
		[Label("库位编码")]
		public static readonly IRefIdProperty StorageLocationIdProperty = P<StoreSummaryLocation>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

		/// <summary>
		/// 库位Id
		/// </summary>
		public double StorageLocationId
		{
			get { return (double)GetRefId(StorageLocationIdProperty); }
			set { SetRefId(StorageLocationIdProperty, value); }
		}

		/// <summary>
		/// 库位
		/// </summary>
		public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<StoreSummaryLocation>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

		/// <summary>
		/// 库位
		/// </summary>
		public StorageLocation StorageLocation
		{
			get { return GetRefEntity(StorageLocationProperty); }
			set { SetRefEntity(StorageLocationProperty, value); }
		}
		#endregion

		#region 库存汇总 StoreSummary
		/// <summary>
		/// 库存汇总Id
		/// </summary>
		public static readonly IRefIdProperty StoreSummaryIdProperty = P<StoreSummaryLocation>.RegisterRefId(e => e.StoreSummaryId, ReferenceType.Parent);

		/// <summary>
		/// 库存汇总Id
		/// </summary>
		public double StoreSummaryId
		{
			get { return (double)GetRefId(StoreSummaryIdProperty); }
			set { SetRefId(StoreSummaryIdProperty, value); }
		}

		/// <summary>
		/// 库存汇总
		/// </summary>
		public static readonly RefEntityProperty<StoreSummary> StoreSummaryProperty = P<StoreSummaryLocation>.RegisterRef(e => e.StoreSummary, StoreSummaryIdProperty);

		/// <summary>
		/// 库存汇总
		/// </summary>
		public StoreSummary StoreSummary
		{
			get { return GetRefEntity(StoreSummaryProperty); }
			set { SetRefEntity(StoreSummaryProperty, value); }
		}
		#endregion

		#region 零成本仓 IsZeroCost
		/// <summary>
		/// 零成本仓
		/// </summary>
		[Label("零成本仓")]
        public static readonly Property<bool> IsZeroCostProperty = P<StoreSummaryLocation>.Register(e => e.IsZeroCost);

        /// <summary>
        /// 零成本仓
        /// </summary>
        public bool IsZeroCost
		{
            get { return this.GetProperty(IsZeroCostProperty); }
            set { this.SetProperty(IsZeroCostProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 仓库名称 WarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
		public static readonly Property<string> WarehouseNameProperty = P<StoreSummaryLocation>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

		/// <summary>
		/// 仓库名称
		/// </summary>
		public string WarehouseName
		{
			get { return this.GetProperty(WarehouseNameProperty); }
		}
		#endregion

		#region 分类 LibraryType
		/// <summary>
		/// 分类
		/// </summary>
		[Label("分类")]
		public static readonly Property<LibraryType> LibraryTypeProperty = P<StoreSummaryLocation>.RegisterView(e => e.LibraryType, p => p.Warehouse.LibraryType);

		/// <summary>
		/// 分类
		/// </summary>
		public LibraryType LibraryType
		{
			get { return this.GetProperty(LibraryTypeProperty); }
		}
		#endregion

		#region 库位名称 StorageLocationName
		/// <summary>
		/// 库位名称
		/// </summary>
		[Label("库位名称")]
        public static readonly Property<string> StorageLocationNameProperty = P<StoreSummaryLocation>.RegisterView(e => e.StorageLocationName, p => p.StorageLocation.Name);

        /// <summary>
        /// 库位名称
        /// </summary>
        public string StorageLocationName
		{
            get { return this.GetProperty(StorageLocationNameProperty); }
        }
		#endregion

		#region 备件Id SparePartId
		/// <summary>
		/// 备件Id
		/// </summary>
		[Label("备件Id")]
        public static readonly Property<double> SparePartIdProperty = P<StoreSummaryLocation>.RegisterView(e => e.SparePartId, p => p.StoreSummary.SparePartId);

        /// <summary>
        /// 备件Id
        /// </summary>
        public double SparePartId
		{
            get { return this.GetProperty(SparePartIdProperty); }
        }
        #endregion

        #endregion

    }

	/// <summary>
	/// 备件库位明细 实体配置
	/// </summary>
	internal class StoreSummaryLocationConfig : EntityConfig<StoreSummaryLocation>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_SPARE_PART_SUMR_LOC").MapAllProperties();
			Meta.Property(StoreSummaryLocation.IsZeroCostProperty).DontMapColumn();
			Meta.EnablePhantoms();
		}
	}
}