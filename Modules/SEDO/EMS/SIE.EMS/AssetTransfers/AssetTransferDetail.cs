using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;

namespace SIE.EMS.AssetTransfers
{
    /// <summary>
    /// 设备清单
    /// </summary>
    [ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("设备清单")]
	public partial class AssetTransferDetail : DataEntity
	{
		#region 位置 Location
		/// <summary>
		/// 位置
		/// </summary>
		[Label("位置")]
		public static readonly Property<string> LocationProperty = P<AssetTransferDetail>.Register(e => e.Location);

		/// <summary>
		/// 位置
		/// </summary>
		public string Location
		{
			get { return GetProperty(LocationProperty); }
			set { SetProperty(LocationProperty, value); }
		}
		#endregion

		#region 原位置 OriginalLocation
		/// <summary>
		/// 原位置
		/// </summary>
		[Label("原位置")]
		public static readonly Property<string> OriginalLocationProperty = P<AssetTransferDetail>.Register(e => e.OriginalLocation);

		/// <summary>
		/// 原位置
		/// </summary>
		public string OriginalLocation
		{
			get { return GetProperty(OriginalLocationProperty); }
			set { SetProperty(OriginalLocationProperty, value); }
		}
		#endregion

		#region 库位 StorageLocation
		/// <summary>
		/// 库位Id
		/// </summary>
		[Label("库位")]
		public static readonly IRefIdProperty StorageLocationIdProperty = P<AssetTransferDetail>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<AssetTransferDetail>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

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
		[Label("仓库")]
		public static readonly IRefIdProperty WarehouseIdProperty = P<AssetTransferDetail>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<AssetTransferDetail>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

		/// <summary>
		/// 仓库
		/// </summary>
		public Warehouse Warehouse
		{
			get { return GetRefEntity(WarehouseProperty); }
			set { SetRefEntity(WarehouseProperty, value); }
		}
		#endregion

		#region 责任人 Responsible
		/// <summary>
		/// 责任人Id
		/// </summary>
		[Label("责任人")]
		public static readonly IRefIdProperty ResponsibleIdProperty = P<AssetTransferDetail>.RegisterRefId(e => e.ResponsibleId, ReferenceType.Normal);

		/// <summary>
		/// 责任人Id
		/// </summary>
		public double? ResponsibleId
		{
			get { return (double?)GetRefNullableId(ResponsibleIdProperty); }
			set { SetRefNullableId(ResponsibleIdProperty, value); }
		}

		/// <summary>
		/// 责任人
		/// </summary>
		public static readonly RefEntityProperty<Employee> ResponsibleProperty = P<AssetTransferDetail>.RegisterRef(e => e.Responsible, ResponsibleIdProperty);

		/// <summary>
		/// 责任人
		/// </summary>
		public Employee Responsible
		{
			get { return GetRefEntity(ResponsibleProperty); }
			set { SetRefEntity(ResponsibleProperty, value); }
		}
		#endregion

		#region 车间 Workshop
		/// <summary>
		/// 车间Id
		/// </summary>
		[Label("车间")]
		public static readonly IRefIdProperty WorkshopIdProperty = P<AssetTransferDetail>.RegisterRefId(e => e.WorkshopId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Enterprise> WorkshopProperty = P<AssetTransferDetail>.RegisterRef(e => e.Workshop, WorkshopIdProperty);

		/// <summary>
		/// 车间
		/// </summary>
		public Enterprise Workshop
		{
			get { return GetRefEntity(WorkshopProperty); }
			set { SetRefEntity(WorkshopProperty, value); }
		}
		#endregion

		#region 产线 Resource
		/// <summary>
		/// 产线Id
		/// </summary>
		[Label("产线")]
		public static readonly IRefIdProperty ResourceIdProperty = P<AssetTransferDetail>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Enterprise> ResourceProperty = P<AssetTransferDetail>.RegisterRef(e => e.Resource, ResourceIdProperty);

		/// <summary>
		/// 产线
		/// </summary>
		public Enterprise Resource
		{
			get { return GetRefEntity(ResourceProperty); }
			set { SetRefEntity(ResourceProperty, value); }
		}
		#endregion

		#region 保管人 Keeper
		/// <summary>
		/// 保管人Id
		/// </summary>
		[Label("保管人")]
		public static readonly IRefIdProperty KeeperIdProperty = P<AssetTransferDetail>.RegisterRefId(e => e.KeeperId, ReferenceType.Normal);

		/// <summary>
		/// 保管人Id
		/// </summary>
		public double? KeeperId
		{
			get { return (double?)GetRefNullableId(KeeperIdProperty); }
			set { SetRefNullableId(KeeperIdProperty, value); }
		}

		/// <summary>
		/// 保管人
		/// </summary>
		public static readonly RefEntityProperty<Employee> KeeperProperty = P<AssetTransferDetail>.RegisterRef(e => e.Keeper, KeeperIdProperty);

		/// <summary>
		/// 保管人
		/// </summary>
		public Employee Keeper
		{
			get { return GetRefEntity(KeeperProperty); }
			set { SetRefEntity(KeeperProperty, value); }
		}
		#endregion

		#region 原保管人 OrignalKeeper
		/// <summary>
		/// 原保管人Id
		/// </summary>
		[Label("原保管人")]
		public static readonly IRefIdProperty OrignalKeeperIdProperty = P<AssetTransferDetail>.RegisterRefId(e => e.OrignalKeeperId, ReferenceType.Normal);

		/// <summary>
		/// 原保管人Id
		/// </summary>
		public double? OrignalKeeperId
		{
			get { return (double?)GetRefNullableId(OrignalKeeperIdProperty); }
			set { SetRefNullableId(OrignalKeeperIdProperty, value); }
		}

		/// <summary>
		/// 原保管人
		/// </summary>
		public static readonly RefEntityProperty<Employee> OrignalKeeperProperty = P<AssetTransferDetail>.RegisterRef(e => e.OrignalKeeper, OrignalKeeperIdProperty);

		/// <summary>
		/// 原保管人
		/// </summary>
		public Employee OrignalKeeper
		{
			get { return GetRefEntity(OrignalKeeperProperty); }
			set { SetRefEntity(OrignalKeeperProperty, value); }
		}
		#endregion

		#region 原车间 OriginalWorkshop
		/// <summary>
		/// 原车间Id
		/// </summary>
		[Label("原车间")]
		public static readonly IRefIdProperty OriginalWorkshopIdProperty = P<AssetTransferDetail>.RegisterRefId(e => e.OriginalWorkshopId, ReferenceType.Normal);

		/// <summary>
		/// 原车间Id
		/// </summary>
		public double? OriginalWorkshopId
		{
			get { return (double?)GetRefNullableId(OriginalWorkshopIdProperty); }
			set { SetRefNullableId(OriginalWorkshopIdProperty, value); }
		}

		/// <summary>
		/// 原车间
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> OriginalWorkshopProperty = P<AssetTransferDetail>.RegisterRef(e => e.OriginalWorkshop, OriginalWorkshopIdProperty);

		/// <summary>
		/// 原车间
		/// </summary>
		public Enterprise OriginalWorkshop
		{
			get { return GetRefEntity(OriginalWorkshopProperty); }
			set { SetRefEntity(OriginalWorkshopProperty, value); }
		}
		#endregion

		#region 设备编码 EquipAccount
		/// <summary>
		/// 设备Id
		/// </summary>
		[Label("设备编码")]
		public static readonly IRefIdProperty EquipAccountIdProperty = P<AssetTransferDetail>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

		/// <summary>
		/// 设备编码Id
		/// </summary>
		public double EquipAccountId
		{
			get { return (double)GetRefId(EquipAccountIdProperty); }
			set { SetRefId(EquipAccountIdProperty, value); }
		}

		/// <summary>
		/// 设备
		/// </summary>
		public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<AssetTransferDetail>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

		/// <summary>
		/// 设备
		/// </summary>
		public EquipAccountSelect EquipAccount
		{
			get { return GetRefEntity(EquipAccountProperty); }
			set { SetRefEntity(EquipAccountProperty, value); }
		}
		#endregion

		#region 原库位 OrinialStorageLocation
		/// <summary>
		/// 原库位Id
		/// </summary>
		[Label("原库位")]
		public static readonly IRefIdProperty OrinialStorageLocationIdProperty = P<AssetTransferDetail>.RegisterRefId(e => e.OrinialStorageLocationId, ReferenceType.Normal);

		/// <summary>
		/// 原库位Id
		/// </summary>
		public double? OrinialStorageLocationId
		{
			get { return (double?)GetRefNullableId(OrinialStorageLocationIdProperty); }
			set { SetRefNullableId(OrinialStorageLocationIdProperty, value); }
		}

		/// <summary>
		/// 原库位
		/// </summary>
		public static readonly RefEntityProperty<StorageLocation> OrinialStorageLocationProperty = P<AssetTransferDetail>.RegisterRef(e => e.OrinialStorageLocation, OrinialStorageLocationIdProperty);

		/// <summary>
		/// 原库位
		/// </summary>
		public StorageLocation OrinialStorageLocation
		{
			get { return GetRefEntity(OrinialStorageLocationProperty); }
			set { SetRefEntity(OrinialStorageLocationProperty, value); }
		}
		#endregion

		#region 原责任人 OriginalResponsible
		/// <summary>
		/// 原责任人Id
		/// </summary>
		[Label("原责任人")]
		public static readonly IRefIdProperty OriginalResponsibleIdProperty = P<AssetTransferDetail>.RegisterRefId(e => e.OriginalResponsibleId, ReferenceType.Normal);

		/// <summary>
		/// 原责任人Id
		/// </summary>
		public double? OriginalResponsibleId
		{
			get { return (double?)GetRefNullableId(OriginalResponsibleIdProperty); }
			set { SetRefNullableId(OriginalResponsibleIdProperty, value); }
		}

		/// <summary>
		/// 原责任人
		/// </summary>
		public static readonly RefEntityProperty<Employee> OriginalResponsibleProperty = P<AssetTransferDetail>.RegisterRef(e => e.OriginalResponsible, OriginalResponsibleIdProperty);

		/// <summary>
		/// 原责任人
		/// </summary>
		public Employee OriginalResponsible
		{
			get { return GetRefEntity(OriginalResponsibleProperty); }
			set { SetRefEntity(OriginalResponsibleProperty, value); }
		}
		#endregion

		#region 原仓库 OriginalWarehouse
		/// <summary>
		/// 原仓库Id
		/// </summary>
		[Label("原仓库")]
		public static readonly IRefIdProperty OriginalWarehouseIdProperty = P<AssetTransferDetail>.RegisterRefId(e => e.OriginalWarehouseId, ReferenceType.Normal);

		/// <summary>
		/// 原仓库Id
		/// </summary>
		public double? OriginalWarehouseId
		{
			get { return (double?)GetRefNullableId(OriginalWarehouseIdProperty); }
			set { SetRefNullableId(OriginalWarehouseIdProperty, value); }
		}

		/// <summary>
		/// 原仓库
		/// </summary>
		public static readonly RefEntityProperty<Warehouse> OriginalWarehouseProperty = P<AssetTransferDetail>.RegisterRef(e => e.OriginalWarehouse, OriginalWarehouseIdProperty);

		/// <summary>
		/// 原仓库
		/// </summary>
		public Warehouse OriginalWarehouse
		{
			get { return GetRefEntity(OriginalWarehouseProperty); }
			set { SetRefEntity(OriginalWarehouseProperty, value); }
		}
		#endregion

		#region 原产线 OriginalResource
		/// <summary>
		/// 原产线Id
		/// </summary>
		[Label("原产线")]
		public static readonly IRefIdProperty OriginalResourceIdProperty = P<AssetTransferDetail>.RegisterRefId(e => e.OriginalResourceId, ReferenceType.Normal);

		/// <summary>
		/// 原产线Id
		/// </summary>
		public double? OriginalResourceId
		{
			get { return (double?)GetRefNullableId(OriginalResourceIdProperty); }
			set { SetRefNullableId(OriginalResourceIdProperty, value); }
		}

		/// <summary>
		/// 原产线
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> OriginalResourceProperty = P<AssetTransferDetail>.RegisterRef(e => e.OriginalResource, OriginalResourceIdProperty);

		/// <summary>
		/// 原产线
		/// </summary>
		public Enterprise OriginalResource
		{
			get { return GetRefEntity(OriginalResourceProperty); }
			set { SetRefEntity(OriginalResourceProperty, value); }
		}
		#endregion

		#region 设备清单 AssetTransfer
		/// <summary>
		/// 设备清单Id
		/// </summary>
		[Label("设备清单")]
		public static readonly IRefIdProperty AssetTransferIdProperty = P<AssetTransferDetail>.RegisterRefId(e => e.AssetTransferId, ReferenceType.Parent);

		/// <summary>
		/// 设备清单Id
		/// </summary>
		public double AssetTransferId
		{
			get { return (double)GetRefId(AssetTransferIdProperty); }
			set { SetRefId(AssetTransferIdProperty, value); }
		}

		/// <summary>
		/// 设备清单
		/// </summary>
		public static readonly RefEntityProperty<AssetTransfer> AssetTransferProperty = P<AssetTransferDetail>.RegisterRef(e => e.AssetTransfer, AssetTransferIdProperty);

		/// <summary>
		/// 设备清单
		/// </summary>
		public AssetTransfer AssetTransfer
		{
			get { return GetRefEntity(AssetTransferProperty); }
			set { SetRefEntity(AssetTransferProperty, value); }
		}
        #endregion

        #region 视图属性

        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<AssetTransferDetail>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

		/// <summary>
		/// 设备编码
		/// </summary>
		public string EquipAccountCode
		{
            get { return this.GetProperty(EquipAccountCodeProperty); }
        }
        #endregion

        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<AssetTransferDetail>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

		/// <summary>
		/// 设备名称
		/// </summary>
		public string EquipAccountName
		{
            get { return this.GetProperty(EquipAccountNameProperty); }
        }
		#endregion

		#region 设备型号 EquipAccountModelCode
		/// <summary>
		/// 设备型号
		/// </summary>
		[Label("设备型号")]
        public static readonly Property<string> EquipAccountModelCodeProperty = P<AssetTransferDetail>.RegisterView(e => e.EquipAccountModelCode, p => p.EquipAccount.EquipModel.Code);

		/// <summary>
		/// 设备型号
		/// </summary>
		public string EquipAccountModelCode
		{
            get { return this.GetProperty(EquipAccountModelCodeProperty); }
        }
		#endregion

		#region 型号名称 EquipAccountModelName
		/// <summary>
		/// 设备型号
		/// </summary>
		[Label("型号名称")]
		public static readonly Property<string> EquipAccountModelNameProperty = P<AssetTransferDetail>.RegisterView(e => e.EquipAccountModelName, p => p.EquipAccount.EquipModel.Name);

		/// <summary>
		/// 设备型号
		/// </summary>
		public string EquipAccountModelName
		{
			get { return this.GetProperty(EquipAccountModelNameProperty); }
		}
		#endregion


		#region 设备别名 Alias	
		/// <summary>
		/// 设备别名
		/// </summary>
		[Label("设备别名")]
        public static readonly Property<string> AliasProperty = P<AssetTransferDetail>.RegisterView(e => e.Alias, p => p.EquipAccount.Alias);

		/// <summary>
		/// 设备别名
		/// </summary>
		public string Alias
		{
            get { return this.GetProperty(AliasProperty); }
        }
        #endregion


        #region 固定资产编码 FixedAssetCode
        /// <summary>
        /// 固定资产编码
        /// </summary>
        [Label("固定资产编码")]
        public static readonly Property<string> FixedAssetCodeProperty = P<AssetTransferDetail>.RegisterView(e => e.FixedAssetCode, p => p.EquipAccount.FixedAssetsAccount.Code);

        /// <summary>
        /// 固定资产编码
        /// </summary>
        public string FixedAssetCode
		{
            get { return this.GetProperty(FixedAssetCodeProperty); }
        }
		#endregion

		#region 固定资产名称 FixedAssetName
		/// <summary>
		/// 固定资产编码
		/// </summary>
		[Label("固定资产名称")]
		public static readonly Property<string> FixedAssetNameProperty = P<AssetTransferDetail>.RegisterView(e => e.FixedAssetName, p => p.EquipAccount.FixedAssetsAccount.Name);

		/// <summary>
		/// 固定资产编码
		/// </summary>
		public string FixedAssetName
		{
			get { return this.GetProperty(FixedAssetNameProperty); }
		}
		#endregion

		#region 技术规格 Specifications
		/// <summary>
		/// 技术规格
		/// </summary>
		[Label("技术规格")]
        public static readonly Property<string> SpecificationsProperty = P<AssetTransferDetail>.RegisterView(e => e.Specifications, p => p.EquipAccount.EquipModel.Specifications);

		/// <summary>
		/// 技术规格
		/// </summary>
		public string Specifications
		{
            get { return this.GetProperty(SpecificationsProperty); }
        }
		#endregion


		#region 原工厂Id SourceFactoryId
		/// <summary>
		/// 原工厂Id
		/// </summary>
		[Label("原工厂Id")]
        public static readonly Property<double> SourceFactoryIdProperty = P<AssetTransferDetail>.RegisterView(e => e.SourceFactoryId, p => p.AssetTransfer.SourceFactoryId);

		/// <summary>
		/// 原工厂Id
		/// </summary>
		public double SourceFactoryId
		{
            get { return this.GetProperty(SourceFactoryIdProperty); }
        }
		#endregion


		#region 原管理部门Id ManageDeptId
		/// <summary>
		/// 原管理部门Id
		/// </summary>
		[Label("原管理部门Id")]
		public static readonly Property<double> ManageDeptIdProperty = P<AssetTransferDetail>.RegisterView(e => e.ManageDeptId, p => p.AssetTransfer.ManageDeptId);

		/// <summary>
		/// 原管理部门Id
		/// </summary>
		public double ManageDeptId
		{
			get { return this.GetProperty(ManageDeptIdProperty); }
		}
		#endregion

		#region 原使用部门Id ManageDeptId
		/// <summary>
		/// 原使用部门Id
		/// </summary>
		[Label("原使用部门Id")]
		public static readonly Property<double?> UseDeptIdProperty = P<AssetTransferDetail>.RegisterView(e => e.UseDeptId, p => p.AssetTransfer.UseDeptId);

		/// <summary>
		/// 原使用部门Id
		/// </summary>
		public double? UseDeptId
		{
			get { return this.GetProperty(UseDeptIdProperty); }
		}
		#endregion


		#region 是否固定资产 ManageDeptId
		/// <summary>
		/// 是否固定资产
		/// </summary>
		[Label("是否固定资产")]
		public static readonly Property<bool> IsAssetProperty = P<AssetTransferDetail>.RegisterView(e => e.IsAsset, p => p.AssetTransfer.IsAsset);

		/// <summary>
		/// 是否固定资产
		/// </summary>
		public bool IsAsset
		{
			get { return this.GetProperty(IsAssetProperty); }
		}
		#endregion

		#region 父级目标工厂 ParentTargetFactoryId	
		/// <summary>
		/// 父级目标工厂  用于过滤下级车间
		/// </summary>
		[Label("父级目标工厂")]
        public static readonly Property<double> ParentTargetFactoryIdProperty = P<AssetTransferDetail>.RegisterView(e => e.ParentTargetFactoryId, p => p.AssetTransfer.TargetFactoryId);

        /// <summary>
        /// 注释
        /// </summary>
        public double ParentTargetFactoryId
		{
            get { return this.GetProperty(ParentTargetFactoryIdProperty); }
        }
        #endregion


        #endregion

    }

	/// <summary>
	/// 设备清单 实体配置
	/// </summary>
	internal class AssetTransferDetailConfig : EntityConfig<AssetTransferDetail>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_ASET_TRAN_DTL").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}