using SIE;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Fixtures.Models
{
	/// <summary>
	/// 工治具编码（存储位置
	/// </summary>
	[ChildEntity, Serializable]
	[Label("工治具编码（存储位置）")]
	public partial class FixtureEncodeStorageLocation : DataEntity
	{
		#region 库位 StorageLocation
		/// <summary>
		/// 库位Id
		/// </summary>
		public static readonly IRefIdProperty StorageLocationIdProperty = P<FixtureEncodeStorageLocation>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<FixtureEncodeStorageLocation>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

		/// <summary>
		/// 库位
		/// </summary>
		public StorageLocation StorageLocation
		{
			get { return GetRefEntity(StorageLocationProperty); }
			set { SetRefEntity(StorageLocationProperty, value); }
		}
		#endregion

		#region 仓库 Warehouse
		/// <summary>
		/// 仓库Id
		/// </summary>
		public static readonly IRefIdProperty WarehouseIdProperty = P<FixtureEncodeStorageLocation>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<FixtureEncodeStorageLocation>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

		/// <summary>
		/// 仓库
		/// </summary>
		public Warehouse Warehouse
		{
			get { return GetRefEntity(WarehouseProperty); }
			set { SetRefEntity(WarehouseProperty, value); }
		}
		#endregion

		#region 工治具编码 FixtureEncode
		/// <summary>
		/// 工治具编码Id
		/// </summary>
		public static readonly IRefIdProperty FixtureEncodeIdProperty = P<FixtureEncodeStorageLocation>.RegisterRefId(e => e.FixtureEncodeId, ReferenceType.Parent);

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
		public static readonly RefEntityProperty<FixtureEncode> FixtureEncodeProperty = P<FixtureEncodeStorageLocation>.RegisterRef(e => e.FixtureEncode, FixtureEncodeIdProperty);

		/// <summary>
		/// 工治具编码
		/// </summary>
		public FixtureEncode FixtureEncode
		{
			get { return GetRefEntity(FixtureEncodeProperty); }
			set { SetRefEntity(FixtureEncodeProperty, value); }
		}
		#endregion


		#region 视图属性

		#region 仓库编码 FixtureWarehouseCode
		/// <summary>
		/// 仓库编码
		/// </summary>
		[Label("仓库编码")]
		public static readonly Property<string> FixtureWarehouseCodeProperty = P<FixtureEncodeStorageLocation>.RegisterView(e => e.FixtureWarehouseCode, p => p.Warehouse.Code);

		/// <summary>
		/// 仓库编码
		/// </summary>
		public string FixtureWarehouseCode
		{
			get { return this.GetProperty(FixtureWarehouseCodeProperty); }
		}
		#endregion

		#region 仓库名称 FixtureWarehouseName
		/// <summary>
		/// 仓库名称
		/// </summary>
		[Label("仓库名称")]
		public static readonly Property<string> FixtureWarehouseNameProperty = P<FixtureEncodeStorageLocation>.RegisterView(e => e.FixtureWarehouseName, p => p.Warehouse.Name);

		/// <summary>
		/// 仓库名称
		/// </summary>
		public string FixtureWarehouseName
		{
			get { return this.GetProperty(FixtureWarehouseNameProperty); }
		}
		#endregion

		#region 库位编码 FixtureStorageLocationCode
		/// <summary>
		/// 库位编码
		/// </summary>
		[Label("库位编码")]
		public static readonly Property<string> FixtureStorageLocationCodeProperty = P<FixtureEncodeStorageLocation>.RegisterView(e => e.FixtureStorageLocationCode, p => p.StorageLocation.Code);

		/// <summary>
		/// 库位编码
		/// </summary>
		public string FixtureStorageLocationCode
		{
			get { return this.GetProperty(FixtureStorageLocationCodeProperty); }
		}
		#endregion

		#region 库位名称 FixtureStorageLocationName
		/// <summary>
		/// 库位名称
		/// </summary>
		[Label("库位名称")]
		public static readonly Property<string> FixtureStorageLocationNameProperty = P<FixtureEncodeStorageLocation>.RegisterView(e => e.FixtureStorageLocationName, p => p.StorageLocation.Name);

		/// <summary>
		/// 库位名称
		/// </summary>
		public string FixtureStorageLocationName
		{
			get { return this.GetProperty(FixtureStorageLocationNameProperty); }
		}
		#endregion

		#endregion
	}

	/// <summary>
	/// 工治具编码（存储位置 实体配置
	/// </summary>
	internal class FixtureEncodeStorageLocationConfig : EntityConfig<FixtureEncodeStorageLocation>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_CODE_LOC").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}