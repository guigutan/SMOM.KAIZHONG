using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.Models;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;
using System.Text;

namespace SIE.EMS.AssetScraps
{
	/// <summary>
	/// 资产报废工治具清单
	/// </summary>
	[ChildEntity, Serializable]	
	[Label("工治具清单")]
	public partial class AssetScrapFixture : DataEntity
	{
		/// <summary>
		/// 快码类型：报废类型
		/// </summary>
		public static string FixtureScrapType { get { return "SCRAP_TYPE"; } }

		#region 报废数量 Qty
		/// <summary>
		/// 报废数量
		/// </summary>
		[Label("报废数量")]
		public static readonly Property<int> QtyProperty = P<AssetScrapFixture>.Register(e => e.Qty);

		/// <summary>
		/// 报废数量
		/// </summary>
		public int Qty
		{
			get { return GetProperty(QtyProperty); }
			set { SetProperty(QtyProperty, value); }
		}
		#endregion

		#region 报废类型 ScrapType
		/// <summary>
		/// 报废类型
		/// </summary>
		[Label("报废类型")]
		public static readonly Property<string> ScrapTypeProperty = P<AssetScrapFixture>.Register(e => e.ScrapType);

		/// <summary>
		/// 报废类型
		/// </summary>
		public string ScrapType
		{
			get { return GetProperty(ScrapTypeProperty); }
			set { SetProperty(ScrapTypeProperty, value); }
		}
		#endregion

		#region 报废原因 Reason
		/// <summary>
		/// 报废原因
		/// </summary>
		[MaxLength(1000)]
		[Label("报废原因")]
		public static readonly Property<string> ReasonProperty = P<AssetScrapFixture>.Register(e => e.Reason);

		/// <summary>
		/// 报废原因
		/// </summary>
		public string Reason
		{
			get { return GetProperty(ReasonProperty); }
			set { SetProperty(ReasonProperty, value); }
		}
		#endregion

		#region 报废净值 ScrapNetValue
		/// <summary>
		/// 报废净值
		/// </summary>
		[Label("报废净值")]
		public static readonly Property<string> ScrapNetValueProperty = P<AssetScrapFixture>.Register(e => e.ScrapNetValue);

		/// <summary>
		/// 报废净值
		/// </summary>
		public string ScrapNetValue
		{
			get { return GetProperty(ScrapNetValueProperty); }
			set { SetProperty(ScrapNetValueProperty, value); }
		}
		#endregion

		#region 工治具编码 FixtureEncode
		/// <summary>
		/// 工治具编码Id
		/// </summary>
		[Label("工治具编码")]
		public static readonly IRefIdProperty FixtureEncodeIdProperty = P<AssetScrapFixture>.RegisterRefId(e => e.FixtureEncodeId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<FixtureEncode> FixtureEncodeProperty = P<AssetScrapFixture>.RegisterRef(e => e.FixtureEncode, FixtureEncodeIdProperty);

		/// <summary>
		/// 工治具编码
		/// </summary>
		public FixtureEncode FixtureEncode
		{
			get { return GetRefEntity(FixtureEncodeProperty); }
			set { SetRefEntity(FixtureEncodeProperty, value); }
		}
		#endregion

		#region 序列号 FixtureAccount
		/// <summary>
		/// 序列号Id
		/// </summary>
		[Label("序列号")]
		public static readonly IRefIdProperty FixtureAccountIdProperty = P<AssetScrapFixture>.RegisterRefId(e => e.FixtureAccountId, ReferenceType.Normal);

		/// <summary>
		/// 序列号Id
		/// </summary>
		public double? FixtureAccountId
		{
			get { return (double?)GetRefNullableId(FixtureAccountIdProperty); }
			set { SetRefNullableId(FixtureAccountIdProperty, value); }
		}

		/// <summary>
		/// 序列号
		/// </summary>
		public static readonly RefEntityProperty<FixtureAccount> FixtureAccountProperty = P<AssetScrapFixture>.RegisterRef(e => e.FixtureAccount, FixtureAccountIdProperty);

		/// <summary>
		/// 序列号
		/// </summary>
		public FixtureAccount FixtureAccount
		{
			get { return GetRefEntity(FixtureAccountProperty); }
			set { SetRefEntity(FixtureAccountProperty, value); }
		}
		#endregion

		#region 质量状态 FixtureQualityState
		/// <summary>
		/// 质量状态
		/// </summary>
		[Label("质量状态")]
		public static readonly Property<FixtureQualityState> FixtureQualityStateProperty = P<AssetScrapFixture>.Register(e => e.FixtureQualityState);

		/// <summary>
		/// 质量状态
		/// </summary>
		public FixtureQualityState FixtureQualityState
		{
			get { return GetProperty(FixtureQualityStateProperty); }
			set { SetProperty(FixtureQualityStateProperty, value); }
		}
		#endregion

		#region 库位 StorageLocation
		/// <summary>
		/// 库位Id
		/// </summary>
		[Label("库位")]
		public static readonly IRefIdProperty StorageLocationIdProperty = P<AssetScrapFixture>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<AssetScrapFixture>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

		/// <summary>
		/// 库位
		/// </summary>
		public StorageLocation StorageLocation
		{
			get { return GetRefEntity(StorageLocationProperty); }
			set { SetRefEntity(StorageLocationProperty, value); }
		}
		#endregion

		#region 报废后仓库 ScrapWarehouse
		/// <summary>
		/// 报废后仓库Id
		/// </summary>
		[Label("报废后仓库")]
		public static readonly IRefIdProperty ScrapWarehouseIdProperty = P<AssetScrapFixture>.RegisterRefId(e => e.ScrapWarehouseId, ReferenceType.Normal);

		/// <summary>
		/// 报废后仓库Id
		/// </summary>
		public double ScrapWarehouseId
		{
			get { return (double)GetRefId(ScrapWarehouseIdProperty); }
			set { SetRefId(ScrapWarehouseIdProperty, value); }
		}

		/// <summary>
		/// 报废后仓库
		/// </summary>
		public static readonly RefEntityProperty<Warehouse> ScrapWarehouseProperty = P<AssetScrapFixture>.RegisterRef(e => e.ScrapWarehouse, ScrapWarehouseIdProperty);

		/// <summary>
		/// 报废后仓库
		/// </summary>
		public Warehouse ScrapWarehouse
		{
			get { return GetRefEntity(ScrapWarehouseProperty); }
			set { SetRefEntity(ScrapWarehouseProperty, value); }
		}
		#endregion

		#region 报废后库位 ScrapLocation
		/// <summary>
		/// 报废后库位Id
		/// </summary>
		[Label("报废后库位")]
		public static readonly IRefIdProperty ScrapLocationIdProperty = P<AssetScrapFixture>.RegisterRefId(e => e.ScrapLocationId, ReferenceType.Normal);

		/// <summary>
		/// 报废后库位Id
		/// </summary>
		public double ScrapLocationId
		{
			get { return (double)GetRefId(ScrapLocationIdProperty); }
			set { SetRefId(ScrapLocationIdProperty, value); }
		}

		/// <summary>
		/// 报废后库位
		/// </summary>
		public static readonly RefEntityProperty<StorageLocation> ScrapLocationProperty = P<AssetScrapFixture>.RegisterRef(e => e.ScrapLocation, ScrapLocationIdProperty);

		/// <summary>
		/// 报废后库位
		/// </summary>
		public StorageLocation ScrapLocation
		{
			get { return GetRefEntity(ScrapLocationProperty); }
			set { SetRefEntity(ScrapLocationProperty, value); }
		}
		#endregion

		#region 资产报废单 AssetScrap
		/// <summary>
		/// 资产报废单Id
		/// </summary>
		[Label("资产报废单")]
		public static readonly IRefIdProperty AssetScrapIdProperty = P<AssetScrapFixture>.RegisterRefId(e => e.AssetScrapId, ReferenceType.Parent);

		/// <summary>
		/// 资产报废单Id
		/// </summary>
		public double AssetScrapId
		{
			get { return (double)GetRefId(AssetScrapIdProperty); }
			set { SetRefId(AssetScrapIdProperty, value); }
		}

		/// <summary>
		/// 资产报废单
		/// </summary>
		public static readonly RefEntityProperty<AssetScrap> AssetScrapProperty = P<AssetScrapFixture>.RegisterRef(e => e.AssetScrap, AssetScrapIdProperty);

		/// <summary>
		/// 资产报废单
		/// </summary>
		public AssetScrap AssetScrap
		{
			get { return GetRefEntity(AssetScrapProperty); }
			set { SetRefEntity(AssetScrapProperty, value); }
		}
		#endregion

		#region 视图属性

		#region 工治具编码 FixtureEncodeCode
		/// <summary>
		/// 工治具编码
		/// </summary>
		[Label("工治具编码")]
		public static readonly Property<string> FixtureEncodeCodeProperty = P<AssetScrapFixture>.RegisterView(e => e.FixtureEncodeCode, p => p.FixtureEncode.Code);

		/// <summary>
		/// 工治具编码
		/// </summary>
		public string FixtureEncodeCode
		{
			get { return this.GetProperty(FixtureEncodeCodeProperty); }
		}
		#endregion

		#region 型号编码 ModelCode
		/// <summary>
		/// 型号编码
		/// </summary>
		[Label("型号编码")]
		public static readonly Property<string> ModelCodeProperty = P<AssetScrapFixture>.RegisterView(e => e.ModelCode, p => p.FixtureEncode.FixtureModel.Code);

		/// <summary>
		/// 型号编码
		/// </summary>
		public string ModelCode
		{
			get { return this.GetProperty(ModelCodeProperty); }
		}
		#endregion

		#region 型号名称 ModelName
		/// <summary>
		/// 型号名称
		/// </summary>
		[Label("型号名称")]
		public static readonly Property<string> ModelNameProperty = P<AssetScrapFixture>.RegisterView(e => e.ModelName, p => p.FixtureEncode.FixtureModel.Name);

		/// <summary>
		/// 型号名称
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
		public static readonly Property<string> FixtureTypeProperty = P<AssetScrapFixture>.RegisterView(e => e.FixtureType, p => p.FixtureEncode.FixtureModel.FixtureType.Code);

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
		public static readonly Property<ManageMode?> ManageModeProperty = P<AssetScrapFixture>.RegisterView(e => e.ManageMode, p => p.FixtureEncode.FixtureModel.ManageMode);

		/// <summary>
		/// 管控方式
		/// </summary>
		public ManageMode? ManageMode
		{
			get { return this.GetProperty(ManageModeProperty); }
		}
		#endregion

		#region 单位 UnitName
		/// <summary>
		/// 单位
		/// </summary>
		[Label("单位")]
		public static readonly Property<string> UnitNameProperty = P<AssetScrapFixture>.RegisterView(e => e.UnitName, p => p.FixtureEncode.FixtureModel.Unit.Name);

		/// <summary>
		/// 单位
		/// </summary>
		public string UnitName
		{
			get { return this.GetProperty(UnitNameProperty); }
		}
		#endregion

		#region 固定资产编码 FixedAssetsAccountCode
		/// <summary>
		/// 固定资产编码
		/// </summary>
		[Label("固定资产编码")]
		public static readonly Property<string> FixedAssetsAccountCodeProperty
			= P<AssetScrapFixture>.RegisterView(e => e.FixedAssetsAccountCode, p => p.FixtureAccount.FixedAssetsAccount.Code);

		/// <summary>
		/// 固定资产编码
		/// </summary>
		public string FixedAssetsAccountCode
		{
			get { return this.GetProperty(FixedAssetsAccountCodeProperty); }
		}
		#endregion

		#region 固定资产名称 FixedAssetsAccountName
		/// <summary>
		/// 固定资产名称
		/// </summary>
		[Label("固定资产名称")]
		public static readonly Property<string> FixedAssetsAccountNameProperty
			= P<AssetScrapFixture>.RegisterView(e => e.FixedAssetsAccountName, p => p.FixtureAccount.FixedAssetsAccount.Name);

		/// <summary>
		/// 固定资产名称
		/// </summary>
		public string FixedAssetsAccountName
		{
			get { return this.GetProperty(FixedAssetsAccountNameProperty); }
		}
		#endregion

		#region 资产原值 OriginalAssetsValue
		/// <summary>
		/// 资产原值
		/// </summary>
		[Label("资产原值")]
		public static readonly Property<decimal> OriginalAssetsValueProperty = P<AssetScrapFixture>.RegisterView(e => e.OriginalAssetsValue, p => p.FixtureAccount.FixedAssetsAccount.OriginalAssetsValue);

		/// <summary>
		/// 资产原值
		/// </summary>
		public decimal OriginalAssetsValue
		{
			get { return this.GetProperty(OriginalAssetsValueProperty); }
		}
        #endregion

        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<AssetScrapFixture>.RegisterView(e => e.WarehouseCode, p => p.StorageLocation.Warehouse.Code);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion

        #region 库位编码 StorageLocationCode
        /// <summary>
        /// 库位编码
        /// </summary>
        [Label("库位编码")]
        public static readonly Property<string> StorageLocationCodeProperty = P<AssetScrapFixture>.RegisterView(e => e.StorageLocationCode, p => p.StorageLocation.Code);

        /// <summary>
        /// 库位编码
        /// </summary>
        public string StorageLocationCode
        {
            get { return this.GetProperty(StorageLocationCodeProperty); }
        }
        #endregion

        #endregion

        #region 不映射数据库的属性

		#region 库存可用数 StoreUsableQty
		/// <summary>
		/// 库存可用数
		/// </summary>
		[Label("库存可用数")]
		public static readonly Property<double?> StoreUsableQtyProperty = P<AssetScrapFixture>.Register(e => e.StoreUsableQty);

		/// <summary>
		/// 库存可用数
		/// </summary>
		public double? StoreUsableQty
		{
			get { return this.GetProperty(StoreUsableQtyProperty); }
			set { this.SetProperty(StoreUsableQtyProperty, value); }
		}
		#endregion

		#region 仓库 Warehouse
		/// <summary>
		/// 仓库Id
		/// </summary>
		[Label("仓库")]
		public static readonly IRefIdProperty WarehouseIdProperty = P<AssetScrapFixture>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<AssetScrapFixture>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

		/// <summary>
		/// 仓库
		/// </summary>
		public Warehouse Warehouse
		{
			get { return GetRefEntity(WarehouseProperty); }
			set { SetRefEntity(WarehouseProperty, value); }
		}
		#endregion

		#region 固定资产 IsFixAsset
		/// <summary>
		/// 固定资产
		/// </summary>
		[Label("固定资产")]
		public static readonly Property<bool> IsFixAssetProperty = P<AssetScrapFixture>.Register(e => e.IsFixAsset);

		/// <summary>
		/// 固定资产
		/// </summary>
		public bool IsFixAsset
		{
			get { return GetProperty(IsFixAssetProperty); }
			set { SetProperty(IsFixAssetProperty, value); }
		}
		#endregion
		#endregion
	}

	/// <summary>
	/// 资产报废工治具清单 实体配置
	/// </summary>
	internal class AssetScrapFixtureConfig : EntityConfig<AssetScrapFixture>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_ASET_SCRP_FIX").MapAllProperties();
			Meta.Property(AssetScrapFixture.ReasonProperty).ColumnMeta.HasLength(2000);
			Meta.Property(AssetScrapFixture.StoreUsableQtyProperty).DontMapColumn();
			Meta.Property(AssetScrapFixture.WarehouseIdProperty).DontMapColumn();
			Meta.Property(AssetScrapFixture.WarehouseProperty).DontMapColumn();
			Meta.Property(AssetScrapFixture.IsFixAssetProperty).DontMapColumn();
			Meta.EnablePhantoms();
		}
	}
}