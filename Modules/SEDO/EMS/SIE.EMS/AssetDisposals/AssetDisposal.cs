using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.DataAuth;
using SIE.Domain;
using SIE.Domain.Validation;
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

namespace SIE.EMS.AssetDisposals
{
	/// <summary>
	/// 资产处置
	/// </summary>
	[RootEntity, Serializable]
	[ConditionQueryType(typeof(AssetDisposalCriteria))]
	[Label("资产处置")]
	[EntityWithConfig(typeof(NoConfig), "资产处置单号生成规则配置项", "资产处置单号生成规则")]
	[EntityWithConfig(typeof(ApprovalConfig))]
	[EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
	public partial class AssetDisposal : DataEntity
	{
		#region 处置单号 No
		/// <summary>
		/// 处置单号
		/// </summary>
		[Label("处置单号")]
		public static readonly Property<string> NoProperty = P<AssetDisposal>.Register(e => e.No);

		/// <summary>
		/// 处置单号
		/// </summary>
		public string No
		{
			get { return GetProperty(NoProperty); }
			set { SetProperty(NoProperty, value); }
		}
		#endregion

		#region 固定资产 IsFixAsset
		/// <summary>
		/// 固定资产
		/// </summary>
		[Label("固定资产")]
		public static readonly Property<bool> IsFixAssetProperty = P<AssetDisposal>.Register(e => e.IsFixAsset);

		/// <summary>
		/// 固定资产
		/// </summary>
		public bool IsFixAsset
		{
			get { return GetProperty(IsFixAssetProperty); }
			set { SetProperty(IsFixAssetProperty, value); }
		}
		#endregion

		#region 处置金额 DsiposalAmount
		/// <summary>
		/// 处置金额
		/// </summary>
		[Label("处置金额")]
		public static readonly Property<decimal> DsiposalAmountProperty = P<AssetDisposal>.Register(e => e.DsiposalAmount);

		/// <summary>
		/// 处置金额
		/// </summary>
		public decimal DsiposalAmount
		{
			get { return GetProperty(DsiposalAmountProperty); }
			set { SetProperty(DsiposalAmountProperty, value); }
		}
		#endregion

		#region 总净值 Amount
		/// <summary>
		/// 总净值
		/// </summary>
		[Label("总净值")]
		public static readonly Property<decimal> AmountProperty = P<AssetDisposal>.Register(e => e.Amount);

		/// <summary>
		/// 总净值
		/// </summary>
		public decimal Amount
		{
			get { return GetProperty(AmountProperty); }
			set { SetProperty(AmountProperty, value); }
		}
		#endregion

		#region 申请日期 ApplyDate
		/// <summary>
		/// 申请日期
		/// </summary>
		[Label("申请日期")]
		public static readonly Property<DateTime> ApplyDateProperty = P<AssetDisposal>.Register(e => e.ApplyDate);

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
		public static readonly Property<string> RemarkProperty = P<AssetDisposal>.Register(e => e.Remark);

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			get { return GetProperty(RemarkProperty); }
			set { SetProperty(RemarkProperty, value); }
		}
		#endregion

		#region 仓库 Warehouse
		/// <summary>
		/// 仓库Id
		/// </summary>
		[Label("仓库")]
		public static readonly IRefIdProperty WarehouseIdProperty = P<AssetDisposal>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<AssetDisposal>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

		/// <summary>
		/// 仓库
		/// </summary>
		public Warehouse Warehouse
		{
			get { return GetRefEntity(WarehouseProperty); }
			set { SetRefEntity(WarehouseProperty, value); }
		}
		#endregion

		#region 工厂 Factory
		/// <summary>
		/// 工厂Id
		/// </summary>
		[Label("工厂")]
		public static readonly IRefIdProperty FactoryIdProperty = P<AssetDisposal>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<AssetDisposal>.RegisterRef(e => e.Factory, FactoryIdProperty);

		/// <summary>
		/// 工厂
		/// </summary>
		public Enterprise Factory
		{
			get { return GetRefEntity(FactoryProperty); }
			set { SetRefEntity(FactoryProperty, value); }
		}
		#endregion

		#region 申请人 Applicant
		/// <summary>
		/// 申请人Id
		/// </summary>
		[Label("申请人")]
		public static readonly IRefIdProperty ApplicantIdProperty = P<AssetDisposal>.RegisterRefId(e => e.ApplicantId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Employee> ApplicantProperty = P<AssetDisposal>.RegisterRef(e => e.Applicant, ApplicantIdProperty);

		/// <summary>
		/// 申请人
		/// </summary>
		public Employee Applicant
		{
			get { return GetRefEntity(ApplicantProperty); }
			set { SetRefEntity(ApplicantProperty, value); }
		}
		#endregion

		#region 管理部门 ManageDept
		/// <summary>
		/// 管理部门Id
		/// </summary>
		[Label("管理部门")]
		public static readonly IRefIdProperty ManageDeptIdProperty = P<AssetDisposal>.RegisterRefId(e => e.ManageDeptId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Enterprise> ManageDeptProperty = P<AssetDisposal>.RegisterRef(e => e.ManageDept, ManageDeptIdProperty);

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
		public static readonly IRefIdProperty UseDeptIdProperty = P<AssetDisposal>.RegisterRefId(e => e.UseDeptId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Enterprise> UseDeptProperty = P<AssetDisposal>.RegisterRef(e => e.UseDept, UseDeptIdProperty);

		/// <summary>
		/// 使用部门
		/// </summary>
		public Enterprise UseDept
		{
			get { return GetRefEntity(UseDeptProperty); }
			set { SetRefEntity(UseDeptProperty, value); }
		}
		#endregion

		#region 审核状态 ApprovalStatus
		/// <summary>
		/// 审核状态
		/// </summary>
		[Label("审核状态")]
		public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<AssetDisposal>.Register(e => e.ApprovalStatus);

		/// <summary>
		/// 审核状态
		/// </summary>
		public ApprovalStatus ApprovalStatus
		{
			get { return GetProperty(ApprovalStatusProperty); }
			set { SetProperty(ApprovalStatusProperty, value); }
		}
		#endregion

		#region 备件回收 AssetDisposalSparePartList
		/// <summary>
		/// 备件回收
		/// </summary>
		public static readonly ListProperty<EntityList<AssetDisposalSparePart>> AssetDisposalSparePartListProperty = P<AssetDisposal>.RegisterList(e => e.AssetDisposalSparePartList);
		/// <summary>
		/// 备件回收
		/// </summary>
		public EntityList<AssetDisposalSparePart> AssetDisposalSparePartList
		{
			get { return this.GetLazyList(AssetDisposalSparePartListProperty); }
		}
		#endregion

		#region 资产对象 AssetObject
		/// <summary>
		/// 资产对象
		/// </summary>
		[Label("资产对象")]
		public static readonly Property<AssetObject> AssetObjectProperty = P<AssetDisposal>.Register(e => e.AssetObject);

		/// <summary>
		/// 资产对象
		/// </summary>
		public AssetObject AssetObject
		{
			get { return GetProperty(AssetObjectProperty); }
			set { SetProperty(AssetObjectProperty, value); }
		}
		#endregion

		#region 设备清单 AssetDisposalEquipmentList
		/// <summary>
		/// 设备清单
		/// </summary>
		public static readonly ListProperty<EntityList<AssetDisposalEquipment>> AssetDisposalEquipmentListProperty = P<AssetDisposal>.RegisterList(e => e.AssetDisposalEquipmentList);
		/// <summary>
		/// 设备清单
		/// </summary>
		public EntityList<AssetDisposalEquipment> AssetDisposalEquipmentList
		{
			get { return this.GetLazyList(AssetDisposalEquipmentListProperty); }
		}
		#endregion

		#region 工治具清单 AssetDisposalFixtureList
		/// <summary>
		/// 工治具清单
		/// </summary>
		public static readonly ListProperty<EntityList<AssetDisposalFixture>> AssetDisposalFixtureListProperty = P<AssetDisposal>.RegisterList(e => e.AssetDisposalFixtureList);
		/// <summary>
		/// 工治具清单
		/// </summary>
		public EntityList<AssetDisposalFixture> AssetDisposalFixtureList
		{
			get { return this.GetLazyList(AssetDisposalFixtureListProperty); }
		}
		#endregion

		#region 附件 AssetDisposalAttachmentList
		/// <summary>
		/// 附件
		/// </summary>
		public static readonly ListProperty<EntityList<AssetDisposalAttachment>> AssetDisposalAttachmentListProperty = P<AssetDisposal>.RegisterList(e => e.AssetDisposalAttachmentList);
		/// <summary>
		/// 附件
		/// </summary>
		public EntityList<AssetDisposalAttachment> AssetDisposalAttachmentList
		{
			get { return this.GetLazyList(AssetDisposalAttachmentListProperty); }
		}
		#endregion
	}

	/// <summary>
	/// 资产处置 实体配置
	/// </summary>
	internal class AssetDisposalConfig : EntityConfig<AssetDisposal>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_ASET_DSPO").MapAllProperties();
			Meta.Property(AssetDisposal.RemarkProperty).ColumnMeta.HasLength(4000);
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
					var para = o.CastTo<AssetDisposal>();
					StringBuilder sb = new StringBuilder();

					if (para.AssetObject == AssetObject.Equipment && (para.ManageDeptId == null || para.ManageDeptId == 0))
					{
						sb.AppendLine("资产对象为【设备】时【管理部门】不能为空！".L10N());
					}
					if (para.AssetObject == AssetObject.Equipment && (para.UseDeptId == null || para.UseDeptId == 0))
					{
						sb.AppendLine("资产对象为【设备】时【使用部门】不能为空！".L10N());
					}
					if ((para.AssetObject == AssetObject.Fixture) && (para.WarehouseId == null || para.WarehouseId == 0))
					{
						sb.AppendLine("资产对象为【工治具、模具】时【仓库】不能为空！".L10N());
					}
					if (para.DsiposalAmount < 0)
					{
						sb.AppendLine("【处置金额】不能小于0！".L10N());
					}

					e.BrokenDescription = sb.ToString();
				}
			}, new RuleMeta() { Scope = EntityStatusScopes.Add | EntityStatusScopes.Update });
		}
	}
}