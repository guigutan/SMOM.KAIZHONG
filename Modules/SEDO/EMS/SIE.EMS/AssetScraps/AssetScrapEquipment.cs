using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;
using System.Text;

namespace SIE.EMS.AssetScraps
{
	/// <summary>
	/// 资产报废设备清单
	/// </summary>
	[ChildEntity, Serializable]	
	[Label("设备清单")]
	public partial class AssetScrapEquipment : DataEntity
	{
		/// <summary>
		/// 快码类型：报废类型
		/// </summary>
		public static string EquipScrapType { get { return "SCRAP_TYPE"; } }

		#region 报废类型 ScrapType
		/// <summary>
		/// 报废类型
		/// </summary>
		[Label("报废类型")]
		public static readonly Property<string> ScrapTypeProperty = P<AssetScrapEquipment>.Register(e => e.ScrapType);

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
		public static readonly Property<string> ReasonProperty = P<AssetScrapEquipment>.Register(e => e.Reason);

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
		public static readonly Property<decimal> ScrapNetValueProperty = P<AssetScrapEquipment>.Register(e => e.ScrapNetValue);

		/// <summary>
		/// 报废净值
		/// </summary>
		public decimal ScrapNetValue
		{
			get { return GetProperty(ScrapNetValueProperty); }
			set { SetProperty(ScrapNetValueProperty, value); }
		}
		#endregion

		#region 维修工时(年内) RepairHours
		/// <summary>
		/// 维修工时(年内)
		/// </summary>
		[Label("维修工时(年内)")]
		public static readonly Property<decimal> RepairHoursProperty = P<AssetScrapEquipment>.Register(e => e.RepairHours);

		/// <summary>
		/// 维修工时(年内)
		/// </summary>
		public decimal RepairHours
		{
			get { return GetProperty(RepairHoursProperty); }
			set { SetProperty(RepairHoursProperty, value); }
		}
		#endregion

		#region 保养工时(年内) MaintenanceHours
		/// <summary>
		/// 保养工时(年内)
		/// </summary>
		[Label("保养工时(年内)")]
		public static readonly Property<decimal> MaintenanceHoursProperty = P<AssetScrapEquipment>.Register(e => e.MaintenanceHours);

		/// <summary>
		/// 保养工时(年内)
		/// </summary>
		public decimal MaintenanceHours
		{
			get { return GetProperty(MaintenanceHoursProperty); }
			set { SetProperty(MaintenanceHoursProperty, value); }
		}
		#endregion

		#region 备件成本(年内) SparePartCost
		/// <summary>
		/// 备件成本(年内)
		/// </summary>
		[Label("备件成本(年内)")]
		public static readonly Property<decimal> SparePartCostProperty = P<AssetScrapEquipment>.Register(e => e.SparePartCost);

		/// <summary>
		/// 备件成本(年内)
		/// </summary>
		public decimal SparePartCost
		{
			get { return GetProperty(SparePartCostProperty); }
			set { SetProperty(SparePartCostProperty, value); }
		}
		#endregion

		#region 委外维修成本 OutRepairCost
		/// <summary>
		/// 委外维修成本
		/// </summary>
		[Label("委外维修成本")]
		public static readonly Property<decimal> OutRepairCostProperty = P<AssetScrapEquipment>.Register(e => e.OutRepairCost);

		/// <summary>
		/// 委外维修成本
		/// </summary>
		public decimal OutRepairCost
		{
			get { return GetProperty(OutRepairCostProperty); }
			set { SetProperty(OutRepairCostProperty, value); }
		}
		#endregion

		#region 累计维修工时 TotalRepairHours
		/// <summary>
		/// 累计维修工时
		/// </summary>
		[Label("累计维修工时")]
		public static readonly Property<decimal> TotalRepairHoursProperty = P<AssetScrapEquipment>.Register(e => e.TotalRepairHours);

		/// <summary>
		/// 累计维修工时
		/// </summary>
		public decimal TotalRepairHours
		{
			get { return GetProperty(TotalRepairHoursProperty); }
			set { SetProperty(TotalRepairHoursProperty, value); }
		}
		#endregion

		#region 累计维修成本 TotalSparePartCost
		/// <summary>
		/// 累计维修成本
		/// </summary>
		[Label("累计维修成本")]
		public static readonly Property<decimal> TotalSparePartCostProperty = P<AssetScrapEquipment>.Register(e => e.TotalSparePartCost);

		/// <summary>
		/// 累计维修成本
		/// </summary>
		public decimal TotalSparePartCost
		{
			get { return GetProperty(TotalSparePartCostProperty); }
			set { SetProperty(TotalSparePartCostProperty, value); }
		}
		#endregion

		#region 设备台账 EquipAccount
		/// <summary>
		/// 设备台账Id
		/// </summary>
		[Label("设备台账")]
		public static readonly IRefIdProperty EquipAccountIdProperty = P<AssetScrapEquipment>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

		/// <summary>
		/// 设备台账Id
		/// </summary>
		public double EquipAccountId
		{
			get { return (double)GetRefId(EquipAccountIdProperty); }
			set { SetRefId(EquipAccountIdProperty, value); }
		}

		/// <summary>
		/// 设备台账
		/// </summary>
		public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<AssetScrapEquipment>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

		/// <summary>
		/// 设备台账
		/// </summary>
		public EquipAccountSelect EquipAccount
		{
			get { return GetRefEntity(EquipAccountProperty); }
			set { SetRefEntity(EquipAccountProperty, value); }
		}
		#endregion

		#region 资产报废单 AssetScrap
		/// <summary>
		/// 资产报废单Id
		/// </summary>
		[Label("资产报废单")]
		public static readonly IRefIdProperty AssetScrapIdProperty = P<AssetScrapEquipment>.RegisterRefId(e => e.AssetScrapId, ReferenceType.Parent);

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
		public static readonly RefEntityProperty<AssetScrap> AssetScrapProperty = P<AssetScrapEquipment>.RegisterRef(e => e.AssetScrap, AssetScrapIdProperty);

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

		#region 设备编码 EquipAccountCode
		/// <summary>
		/// 设备编码
		/// </summary>
		[Label("设备编码")]
		public static readonly Property<string> EquipAccountCodeProperty = P<AssetScrapEquipment>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

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
		public static readonly Property<string> EquipAccountNameProperty = P<AssetScrapEquipment>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

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
		public static readonly Property<string> AliasProperty = P<AssetScrapEquipment>.RegisterView(e => e.Alias, p => p.EquipAccount.Alias);

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
		public static readonly Property<string> EquipModelCodeProperty = P<AssetScrapEquipment>.RegisterView(e => e.EquipModelCode, p => p.EquipAccount.EquipModel.Code);

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
		public static readonly Property<string> EquipModelNameProperty = P<AssetScrapEquipment>.RegisterView(e => e.EquipModelName, p => p.EquipAccount.EquipModel.Name);

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
		public static readonly Property<string> SpecificationsProperty = P<AssetScrapEquipment>.RegisterView(e => e.Specifications, p => p.EquipAccount.EquipModel.Specifications);

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
			= P<AssetScrapEquipment>.RegisterView(e => e.FixedAssetsAccountCode, p => p.EquipAccount.FixedAssetsAccount.Code);

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
			= P<AssetScrapEquipment>.RegisterView(e => e.FixedAssetsAccountName, p => p.EquipAccount.FixedAssetsAccount.Name);

		/// <summary>
		/// 固定资产名称
		/// </summary>
		public string FixedAssetsAccountName
		{
			get { return this.GetProperty(FixedAssetsAccountNameProperty); }
		}
		#endregion

		#region 预计使用年限 UsefulLife
		/// <summary>
		/// 预计使用年限
		/// </summary>
		[Label("预计使用年限")]
        public static readonly Property<string> UsefulLifeProperty = P<AssetScrapEquipment>.RegisterView(e => e.UsefulLife, p => p.EquipAccount.UsefulLife);

		/// <summary>
		/// 预计使用年限
		/// </summary>
		public string UsefulLife
		{
            get { return this.GetProperty(UsefulLifeProperty); }
        }
		#endregion

		#region 资产原值 OriginalAssetsValue
		/// <summary>
		/// 资产原值
		/// </summary>
		[Label("资产原值")]
        public static readonly Property<decimal> OriginalAssetsValueProperty = P<AssetScrapEquipment>.RegisterView(e => e.OriginalAssetsValue, p => p.EquipAccount.FixedAssetsAccount.OriginalAssetsValue);

		/// <summary>
		/// 资产原值
		/// </summary>
		public decimal OriginalAssetsValue
		{
            get { return this.GetProperty(OriginalAssetsValueProperty); }
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
			P<AssetScrapEquipment>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
			P<AssetScrapEquipment>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
		public static readonly IRefIdProperty ManageDeptIdProperty = P<AssetScrapEquipment>.RegisterRefId(e => e.ManageDeptId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Enterprise> ManageDeptProperty = P<AssetScrapEquipment>.RegisterRef(e => e.ManageDept, ManageDeptIdProperty);

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
		public static readonly IRefIdProperty UseDeptIdProperty = P<AssetScrapEquipment>.RegisterRefId(e => e.UseDeptId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Enterprise> UseDeptProperty = P<AssetScrapEquipment>.RegisterRef(e => e.UseDept, UseDeptIdProperty);

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
            P<AssetScrapEquipment>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
            P<AssetScrapEquipment>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

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
		public static readonly Property<bool> IsFixAssetProperty = P<AssetScrapEquipment>.Register(e => e.IsFixAsset);

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
	/// 资产报废设备清单 实体配置
	/// </summary>
	internal class AssetScrapEquipmentConfig : EntityConfig<AssetScrapEquipment>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_ASET_SCRP_EQP").MapAllProperties();
			Meta.Property(AssetScrapEquipment.FactoryIdProperty).DontMapColumn();
			Meta.Property(AssetScrapEquipment.FactoryProperty).DontMapColumn();
			Meta.Property(AssetScrapEquipment.ManageDeptIdProperty).DontMapColumn();
			Meta.Property(AssetScrapEquipment.ManageDeptProperty).DontMapColumn();
			Meta.Property(AssetScrapEquipment.UseDeptIdProperty).DontMapColumn();
			Meta.Property(AssetScrapEquipment.UseDeptProperty).DontMapColumn();
			Meta.Property(AssetScrapEquipment.WarehouseIdProperty).DontMapColumn();
			Meta.Property(AssetScrapEquipment.WarehouseProperty).DontMapColumn();
			Meta.Property(AssetScrapEquipment.IsFixAssetProperty).DontMapColumn();
			Meta.EnablePhantoms();
		}

		/// <summary>
		/// 校验规则
		/// </summary>
		/// <param name="rules">规则</param>
		protected override void AddValidations(IValidationDeclarer rules)
		{
			rules.AddRule(new HandlerRule()
			{
				Handler = (o, e) =>
				{
					var para = o.CastTo<AssetScrapEquipment>();
					StringBuilder sb = new StringBuilder();

					if (para.ScrapType.IsNullOrEmpty())
					{
						sb.AppendLine("【报废类型】不能为空！".L10N());
					}
					if (para.Reason.IsNullOrEmpty())
					{
						sb.AppendLine("【报废原因】不能为空！".L10N());
					}

					e.BrokenDescription = sb.ToString();
				}
			}, new RuleMeta() { Scope = EntityStatusScopes.Add | EntityStatusScopes.Update });
		}
	}
}