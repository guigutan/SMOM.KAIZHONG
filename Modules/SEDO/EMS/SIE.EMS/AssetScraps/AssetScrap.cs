using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.DataAuth;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.AssetRequisitions.Configs;
using SIE.EMS.AssetScraps.Configs;
using SIE.EMS.Enums;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;
using System.Text;

namespace SIE.EMS.AssetScraps
{
	/// <summary>
	/// 资产报废
	/// </summary>
	[RootEntity, Serializable]
	[ConditionQueryType(typeof(AssetScrapCriteria))]
	[Label("资产报废")]
	[EntityWithConfig(typeof(NoConfig), "资产报废单号生成规则配置项", "资产报废单号生成规则")]
	[EntityWithConfig(typeof(ApprovalConfig))]
	[EntityWithConfig(typeof(EnableAssetDisposalConfig))]
	[EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
	public partial class AssetScrap : DataEntity
	{
		#region 报废单号 No
		/// <summary>
		/// 报废单号
		/// </summary>
		[Label("报废单号")]
		public static readonly Property<string> NoProperty = P<AssetScrap>.Register(e => e.No);

		/// <summary>
		/// 报废单号
		/// </summary>
		public string No
		{
			get { return GetProperty(NoProperty); }
			set { SetProperty(NoProperty, value); }
		}
		#endregion

		#region 工厂 Factory
		/// <summary>
		/// 工厂Id
		/// </summary>
		[Label("工厂")]
		public static readonly IRefIdProperty FactoryIdProperty = P<AssetScrap>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

		/// <summary>
		/// 工厂Id
		/// </summary>
		public double FactoryId
		{
			get { return (double)GetRefId(FactoryIdProperty); }
			set { SetRefId(FactoryIdProperty, value); }
		}

		/// <summary>
		/// 工厂
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<AssetScrap>.RegisterRef(e => e.Factory, FactoryIdProperty);

		/// <summary>
		/// 工厂
		/// </summary>
		public Enterprise Factory
		{
			get { return GetRefEntity(FactoryProperty); }
			set { SetRefEntity(FactoryProperty, value); }
		}
		#endregion

		#region 审核状态 ApprovalStatus
		/// <summary>
		/// 审核状态
		/// </summary>
		[Label("审核状态")]
		public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<AssetScrap>.Register(e => e.ApprovalStatus);

		/// <summary>
		/// 审核状态
		/// </summary>
		public ApprovalStatus ApprovalStatus
		{
			get { return GetProperty(ApprovalStatusProperty); }
			set { SetProperty(ApprovalStatusProperty, value); }
		}
		#endregion

		#region 资产对象 AssetObject
		/// <summary>
		/// 资产对象
		/// </summary>
		[Label("资产对象")]
		public static readonly Property<AssetObject> AssetObjectProperty = P<AssetScrap>.Register(e => e.AssetObject);

		/// <summary>
		/// 资产对象
		/// </summary>
		public AssetObject AssetObject
		{
			get { return GetProperty(AssetObjectProperty); }
			set { SetProperty(AssetObjectProperty, value); }
		}
		#endregion

		#region 管理部门 ManageDept
		/// <summary>
		/// 管理部门Id
		/// </summary>
		[Label("管理部门")]
		public static readonly IRefIdProperty ManageDeptIdProperty = P<AssetScrap>.RegisterRefId(e => e.ManageDeptId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Enterprise> ManageDeptProperty = P<AssetScrap>.RegisterRef(e => e.ManageDept, ManageDeptIdProperty);

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
		public static readonly IRefIdProperty UseDeptIdProperty = P<AssetScrap>.RegisterRefId(e => e.UseDeptId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Enterprise> UseDeptProperty = P<AssetScrap>.RegisterRef(e => e.UseDept, UseDeptIdProperty);

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
            P<AssetScrap>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
            P<AssetScrap>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

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
		public static readonly Property<bool> IsFixAssetProperty = P<AssetScrap>.Register(e => e.IsFixAsset);

		/// <summary>
		/// 固定资产
		/// </summary>
		public bool IsFixAsset
		{
			get { return GetProperty(IsFixAssetProperty); }
			set { SetProperty(IsFixAssetProperty, value); }
		}
		#endregion

		#region 净值汇总 Amount
		/// <summary>
		/// 净值汇总
		/// </summary>
		[Label("净值汇总")]
		public static readonly Property<decimal?> AmountProperty = P<AssetScrap>.Register(e => e.Amount);

		/// <summary>
		/// 净值汇总
		/// </summary>
		public decimal? Amount
		{
			get { return GetProperty(AmountProperty); }
			set { SetProperty(AmountProperty, value); }
		}
		#endregion

		#region 申请人 Applicant
		/// <summary>
		/// 申请人Id
		/// </summary>
		[Label("申请人")]
		public static readonly IRefIdProperty ApplicantIdProperty = P<AssetScrap>.RegisterRefId(e => e.ApplicantId, ReferenceType.Normal);

		/// <summary>
		/// 申请人Id
		/// </summary>
		public double ApplicantId
		{
			get { return (double)GetRefId(ApplicantIdProperty); }
			set { SetRefId(ApplicantIdProperty, value); }
		}

		/// <summary>
		/// 申请人
		/// </summary>
		public static readonly RefEntityProperty<Employee> ApplicantProperty = P<AssetScrap>.RegisterRef(e => e.Applicant, ApplicantIdProperty);

		/// <summary>
		/// 申请人
		/// </summary>
		public Employee Applicant
		{
			get { return GetRefEntity(ApplicantProperty); }
			set { SetRefEntity(ApplicantProperty, value); }
		}
		#endregion

		#region 申请日期 ApplyDate
		/// <summary>
		/// 申请日期
		/// </summary>
		[Label("申请日期")]
		public static readonly Property<DateTime> ApplyDateProperty = P<AssetScrap>.Register(e => e.ApplyDate);

		/// <summary>
		/// 申请日期
		/// </summary>
		public DateTime ApplyDate
		{
			get { return GetProperty(ApplyDateProperty); }
			set { SetProperty(ApplyDateProperty, value); }
		}
		#endregion

		#region 备注 Remark
		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(1000)]
		[Label("备注")]
		public static readonly Property<string> RemarkProperty = P<AssetScrap>.Register(e => e.Remark);

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			get { return GetProperty(RemarkProperty); }
			set { SetProperty(RemarkProperty, value); }
		}
		#endregion

		#region 设备清单 AssetScrapEquipmentList
		/// <summary>
		/// 设备清单
		/// </summary>
		public static readonly ListProperty<EntityList<AssetScrapEquipment>> AssetScrapEquipmentListProperty = P<AssetScrap>.RegisterList(e => e.AssetScrapEquipmentList);
		/// <summary>
		/// 设备清单
		/// </summary>
		public EntityList<AssetScrapEquipment> AssetScrapEquipmentList
		{
			get { return this.GetLazyList(AssetScrapEquipmentListProperty); }
		}
		#endregion

		#region 工治具清单 AssetScrapFixtureList
		/// <summary>
		/// 工治具清单
		/// </summary>
		public static readonly ListProperty<EntityList<AssetScrapFixture>> AssetScrapFixtureListProperty = P<AssetScrap>.RegisterList(e => e.AssetScrapFixtureList);
		/// <summary>
		/// 工治具清单
		/// </summary>
		public EntityList<AssetScrapFixture> AssetScrapFixtureList
		{
			get { return this.GetLazyList(AssetScrapFixtureListProperty); }
		}
		#endregion

		#region 附件 AssetScrapAttachmentList
		/// <summary>
		/// 附件
		/// </summary>
		public static readonly ListProperty<EntityList<AssetScrapAttachment>> AssetScrapAttachmentListProperty = P<AssetScrap>.RegisterList(e => e.AssetScrapAttachmentList);
		/// <summary>
		/// 附件
		/// </summary>
		public EntityList<AssetScrapAttachment> AssetScrapAttachmentList
		{
			get { return this.GetLazyList(AssetScrapAttachmentListProperty); }
		}
        #endregion

        #region 不映射数据库的属性

        #region 报废后库位Id ScrapLocationId
        /// <summary>
        /// 报废后库位Id
        /// </summary>
        [Label("报废后库位Id")]
        public static readonly Property<double?> ScrapLocationIdProperty = P<AssetScrap>.Register(e => e.ScrapLocationId);

        /// <summary>
        /// 报废后库位Id
        /// </summary>
        public double? ScrapLocationId
        {
            get { return this.GetProperty(ScrapLocationIdProperty); }
            set { this.SetProperty(ScrapLocationIdProperty, value); }
        }
		#endregion

		#region 报废后库位编码 ScrapLocationCode
		/// <summary>
		/// 报废后库位编码
		/// </summary>
		[Label("报废后库位编码")]
        public static readonly Property<string> ScrapLocationCodeProperty = P<AssetScrap>.Register(e => e.ScrapLocationCode);

		/// <summary>
		/// 报废后库位编码
		/// </summary>
		public string ScrapLocationCode
		{
            get { return this.GetProperty(ScrapLocationCodeProperty); }
            set { this.SetProperty(ScrapLocationCodeProperty, value); }
        }
        #endregion

        #endregion

    }

    /// <summary>
    /// 资产报废 实体配置
    /// </summary>
    internal class AssetScrapConfig : EntityConfig<AssetScrap>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_ASET_SCRP").MapAllProperties();
			Meta.Property(AssetScrap.RemarkProperty).ColumnMeta.HasLength(2000);
			Meta.Property(AssetScrap.ScrapLocationIdProperty).DontMapColumn();
			Meta.Property(AssetScrap.ScrapLocationCodeProperty).DontMapColumn();
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
					var para = o.CastTo<AssetScrap>();
					StringBuilder sb = new StringBuilder();

					if (para.AssetObject == AssetObject.Equipment && (para.ManageDeptId == null || para.ManageDeptId == 0))
					{
						sb.AppendLine("资产对象为【设备】时【管理部门】不能为空！".L10N());
					}
					if (para.AssetObject == AssetObject.Equipment && (para.UseDeptId == null || para.UseDeptId == 0))
					{
						sb.AppendLine("资产对象为【设备】时【使用部门】不能为空！".L10N());
					}
					if (para.AssetObject != AssetObject.Equipment && (para.WarehouseId == null || para.WarehouseId == 0))
					{
						sb.AppendLine("资产对象为【工治具、模具】时【仓库】不能为空！".L10N());
					}

					e.BrokenDescription = sb.ToString();
				}
			}, new RuleMeta() { Scope = EntityStatusScopes.Add | EntityStatusScopes.Update });
		}
	}
}