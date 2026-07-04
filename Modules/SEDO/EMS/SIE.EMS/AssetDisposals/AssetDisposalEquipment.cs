using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;

namespace SIE.EMS.AssetDisposals
{
	/// <summary>
	/// 资产处置设备清单
	/// </summary>
	[ChildEntity, Serializable]	
	[Label("设备清单")]
	public partial class AssetDisposalEquipment : DataEntity
	{
		#region 资产原值 OriginalValue
		/// <summary>
		/// 资产原值
		/// </summary>
		[Label("资产原值")]
		public static readonly Property<decimal> OriginalValueProperty = P<AssetDisposalEquipment>.Register(e => e.OriginalValue);

		/// <summary>
		/// 资产原值
		/// </summary>
		public decimal OriginalValue
		{
			get { return GetProperty(OriginalValueProperty); }
			set { SetProperty(OriginalValueProperty, value); }
		}
		#endregion

		#region 资产净值 NetValue
		/// <summary>
		/// 资产净值
		/// </summary>
		[Label("资产净值")]
		public static readonly Property<decimal> NetValueProperty = P<AssetDisposalEquipment>.Register(e => e.NetValue);

		/// <summary>
		/// 资产净值
		/// </summary>
		public decimal NetValue
		{
			get { return GetProperty(NetValueProperty); }
			set { SetProperty(NetValueProperty, value); }
		}
		#endregion

		#region 资产残值 ResidualValue
		/// <summary>
		/// 资产残值
		/// </summary>
		[Label("资产残值")]
		public static readonly Property<decimal> ResidualValueProperty = P<AssetDisposalEquipment>.Register(e => e.ResidualValue);

		/// <summary>
		/// 资产残值
		/// </summary>
		public decimal ResidualValue
		{
			get { return GetProperty(ResidualValueProperty); }
			set { SetProperty(ResidualValueProperty, value); }
		}
		#endregion

		#region 设备 EquipAccount
		/// <summary>
		/// 设备Id
		/// </summary>
		[Label("设备编码")]
		public static readonly IRefIdProperty EquipAccountIdProperty = P<AssetDisposalEquipment>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

		/// <summary>
		/// 设备Id
		/// </summary>
		public double EquipAccountId
		{
			get { return (double)GetRefId(EquipAccountIdProperty); }
			set { SetRefId(EquipAccountIdProperty, value); }
		}

		/// <summary>
		/// 设备
		/// </summary>
		public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<AssetDisposalEquipment>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

		/// <summary>
		/// 设备
		/// </summary>
		public EquipAccountSelect EquipAccount
		{
			get { return GetRefEntity(EquipAccountProperty); }
			set { SetRefEntity(EquipAccountProperty, value); }
		}
		#endregion

		#region 资产处置 AssetDisposal
		/// <summary>
		/// 资产处置Id
		/// </summary>
		[Label("资产处置")]
		public static readonly IRefIdProperty AssetDisposalIdProperty = P<AssetDisposalEquipment>.RegisterRefId(e => e.AssetDisposalId, ReferenceType.Parent);

		/// <summary>
		/// 资产处置Id
		/// </summary>
		public double AssetDisposalId
		{
			get { return (double)GetRefId(AssetDisposalIdProperty); }
			set { SetRefId(AssetDisposalIdProperty, value); }
		}

		/// <summary>
		/// 资产处置
		/// </summary>
		public static readonly RefEntityProperty<AssetDisposal> AssetDisposalProperty = P<AssetDisposalEquipment>.RegisterRef(e => e.AssetDisposal, AssetDisposalIdProperty);

		/// <summary>
		/// 资产处置
		/// </summary>
		public AssetDisposal AssetDisposal
		{
			get { return GetRefEntity(AssetDisposalProperty); }
			set { SetRefEntity(AssetDisposalProperty, value); }
		}
		#endregion

		#region 视图属性

		#region 设备编码 EquipAccountCode
		/// <summary>
		/// 设备编码
		/// </summary>
		[Label("设备编码")]
		public static readonly Property<string> EquipAccountCodeProperty = P<AssetDisposalEquipment>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

		/// <summary>
		/// 设备编码
		/// </summary>
		public string EquipAccountCode
		{
			get { return this.GetProperty(EquipAccountCodeProperty); }
			set { this.SetProperty(EquipAccountCodeProperty, value); }
		}
		#endregion

		#region 设备名称 EquipAccountName
		/// <summary>
		/// 设备名称
		/// </summary>
		[Label("设备名称")]
		public static readonly Property<string> EquipAccountNameProperty = P<AssetDisposalEquipment>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

		/// <summary>
		/// 设备名称
		/// </summary>
		public string EquipAccountName
		{
			get { return this.GetProperty(EquipAccountNameProperty); }
			set { this.SetProperty(EquipAccountNameProperty, value); }
		}
		#endregion

		#region 设备别名 Alias
		/// <summary>
		/// 设备别名
		/// </summary>
		[Label("设备别名")]
		public static readonly Property<string> AliasProperty = P<AssetDisposalEquipment>.RegisterView(e => e.Alias, p => p.EquipAccount.Alias);

		/// <summary>
		/// 设备别名
		/// </summary>
		public string Alias
		{
			get { return this.GetProperty(AliasProperty); }
			set { this.SetProperty(AliasProperty, value); }
		}
		#endregion

		#region 设备型号 EquipModelCode
		/// <summary>
		/// 设备型号
		/// </summary>
		[Label("设备型号")]
		public static readonly Property<string> EquipModelCodeProperty = P<AssetDisposalEquipment>.RegisterView(e => e.EquipModelCode, p => p.EquipAccount.EquipModel.Code);

		/// <summary>
		/// 设备型号
		/// </summary>
		public string EquipModelCode
		{
			get { return this.GetProperty(EquipModelCodeProperty); }
			set { this.SetProperty(EquipModelCodeProperty, value); }
		}
		#endregion

		#region 型号名称 EquipModelName
		/// <summary>
		/// 型号名称
		/// </summary>
		[Label("型号名称")]
		public static readonly Property<string> EquipModelNameProperty = P<AssetDisposalEquipment>.RegisterView(e => e.EquipModelName, p => p.EquipAccount.EquipModel.Name);

		/// <summary>
		/// 型号名称
		/// </summary>
		public string EquipModelName
		{
			get { return this.GetProperty(EquipModelNameProperty); }
			set { this.SetProperty(EquipModelNameProperty, value); }
		}
		#endregion

		#region 技术规格 Specifications
		/// <summary>
		/// 技术规格
		/// </summary>
		[Label("技术规格")]
		public static readonly Property<string> SpecificationsProperty = P<AssetDisposalEquipment>.RegisterView(e => e.Specifications, p => p.EquipAccount.EquipModel.Specifications);

		/// <summary>
		/// 技术规格
		/// </summary>
		public string Specifications
		{
			get { return this.GetProperty(SpecificationsProperty); }
			set { this.SetProperty(SpecificationsProperty, value); }
		}
		#endregion

		#region 固定资产编码 FixedAssetsAccountCode
		/// <summary>
		/// 固定资产编码
		/// </summary>
		[Label("固定资产编码")]
		public static readonly Property<string> FixedAssetsAccountCodeProperty
			= P<AssetDisposalEquipment>.RegisterView(e => e.FixedAssetsAccountCode, p => p.EquipAccount.FixedAssetsAccount.Code);

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
			= P<AssetDisposalEquipment>.RegisterView(e => e.FixedAssetsAccountName, p => p.EquipAccount.FixedAssetsAccount.Name);

		/// <summary>
		/// 固定资产名称
		/// </summary>
		public string FixedAssetsAccountName
		{
			get { return this.GetProperty(FixedAssetsAccountNameProperty); }
		}
		#endregion

		#endregion

		#region 不映射数据库的属性

		#region 工厂 Factory
		/// <summary>
		/// 工厂Id
		/// </summary>
		[Label("工厂")]
		public static readonly IRefIdProperty FactoryIdProperty =
			P<AssetDisposalEquipment>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Enterprise> FactoryProperty =
			P<AssetDisposalEquipment>.RegisterRef(e => e.Factory, FactoryIdProperty);

		/// <summary>
		/// 工厂
		/// </summary>
		public Enterprise Factory
		{
			get { return this.GetRefEntity(FactoryProperty); }
			set { this.SetRefEntity(FactoryProperty, value); }
		}
		#endregion

		#region 管理部门 ManageDept
		/// <summary>
		/// 管理部门Id
		/// </summary>
		[Label("管理部门")]
		public static readonly IRefIdProperty ManageDeptIdProperty = P<AssetDisposalEquipment>.RegisterRefId(e => e.ManageDeptId, ReferenceType.Normal);

		/// <summary>
		/// 管理部门Id
		/// </summary>
		public double? ManageDeptId
		{
			get { return (double?)GetRefNullableId(ManageDeptIdProperty); }
			set { SetRefNullableId(ManageDeptIdProperty, value); }
		}

		/// <summary>
		/// 管理部门
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> ManageDeptProperty = P<AssetDisposalEquipment>.RegisterRef(e => e.ManageDept, ManageDeptIdProperty);

		/// <summary>
		/// 管理部门
		/// </summary>
		public Enterprise ManageDept
		{
			get { return GetRefEntity(ManageDeptProperty); }
			set { SetRefEntity(ManageDeptProperty, value); }
		}
		#endregion

		#region 使用部门 UseDept
		/// <summary>
		/// 使用部门Id
		/// </summary>
		[Label("使用部门")]
		public static readonly IRefIdProperty UseDeptIdProperty = P<AssetDisposalEquipment>.RegisterRefId(e => e.UseDeptId, ReferenceType.Normal);

		/// <summary>
		/// 使用部门Id
		/// </summary>
		public double? UseDeptId
		{
			get { return (double?)GetRefNullableId(UseDeptIdProperty); }
			set { SetRefNullableId(UseDeptIdProperty, value); }
		}

		/// <summary>
		/// 使用部门
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> UseDeptProperty = P<AssetDisposalEquipment>.RegisterRef(e => e.UseDept, UseDeptIdProperty);

		/// <summary>
		/// 使用部门
		/// </summary>
		public Enterprise UseDept
		{
			get { return GetRefEntity(UseDeptProperty); }
			set { SetRefEntity(UseDeptProperty, value); }
		}
		#endregion

		#region 仓库 Warehouse
		/// <summary>
		/// 仓库Id
		/// </summary>
		[Label("仓库")]
		public static readonly IRefIdProperty WarehouseIdProperty =
			P<AssetDisposalEquipment>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

		/// <summary>
		/// 仓库Id
		/// </summary>
		public double? WarehouseId
		{
			get { return (double?)this.GetRefNullableId(WarehouseIdProperty); }
			set { this.SetRefNullableId(WarehouseIdProperty, value); }
		}

		/// <summary>
		/// 仓库
		/// </summary>
		public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
			P<AssetDisposalEquipment>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

		/// <summary>
		/// 仓库
		/// </summary>
		public Warehouse Warehouse
		{
			get { return this.GetRefEntity(WarehouseProperty); }
			set { this.SetRefEntity(WarehouseProperty, value); }
		}
		#endregion

		#region 固定资产 IsFixAsset
		/// <summary>
		/// 固定资产
		/// </summary>
		[Label("固定资产")]
		public static readonly Property<bool> IsFixAssetProperty = P<AssetDisposalEquipment>.Register(e => e.IsFixAsset);

		/// <summary>
		/// 固定资产
		/// </summary>
		public bool IsFixAsset
		{
			get { return GetProperty(IsFixAssetProperty); }
			set { SetProperty(IsFixAssetProperty, value); }
		}
		#endregion

		#region 报废类型 ScrapType
		/// <summary>
		/// 报废类型
		/// </summary>
		[Label("报废类型")]
		public static readonly Property<string> ScrapTypeProperty = P<AssetDisposalEquipment>.Register(e => e.ScrapType);

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
		[Label("报废原因")]
		public static readonly Property<string> ReasonProperty = P<AssetDisposalEquipment>.Register(e => e.Reason);

		/// <summary>
		/// 报废原因
		/// </summary>
		public string Reason
		{
			get { return GetProperty(ReasonProperty); }
			set { SetProperty(ReasonProperty, value); }
		}
		#endregion
		#endregion
	}

	/// <summary>
	/// 资产处置设备清单 实体配置
	/// </summary>
	internal class AssetDisposalEquipmentConfig : EntityConfig<AssetDisposalEquipment>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_ASET_DSPO_EQP").MapAllProperties();
			Meta.Property(AssetDisposalEquipment.FactoryIdProperty).DontMapColumn();
			Meta.Property(AssetDisposalEquipment.FactoryProperty).DontMapColumn();
			Meta.Property(AssetDisposalEquipment.ManageDeptIdProperty).DontMapColumn();
			Meta.Property(AssetDisposalEquipment.ManageDeptProperty).DontMapColumn();
			Meta.Property(AssetDisposalEquipment.UseDeptIdProperty).DontMapColumn();
			Meta.Property(AssetDisposalEquipment.UseDeptProperty).DontMapColumn();
			Meta.Property(AssetDisposalEquipment.WarehouseIdProperty).DontMapColumn();
			Meta.Property(AssetDisposalEquipment.WarehouseProperty).DontMapColumn();
			Meta.Property(AssetDisposalEquipment.IsFixAssetProperty).DontMapColumn();
			Meta.EnablePhantoms();
		}
	}
}