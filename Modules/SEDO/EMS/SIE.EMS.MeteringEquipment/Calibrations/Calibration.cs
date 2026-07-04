using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Core.Enums;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.InspectionRules;
using SIE.EMS.MeteringEquipment.Calibrations.Criterias;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.MeteringEquipment.Calibrations
{
    /// <summary>
    /// 计量设备定检
    /// </summary>
    [RootEntity, Serializable]
	[ConditionQueryType(typeof(CalibrationCriteria))]
	[EntityWithConfig(typeof(NoConfig))]
	[EntityWithConfig(typeof(ApprovalConfig))]
	[Label("计量设备定检")]
	public partial class Calibration : DataEntity
	{
		#region 检验单号 InspectionNo
		/// <summary>
		/// 检验单号
		/// </summary>
		[Required]
		[NotDuplicate]
		[Label("检验单号")]
		public static readonly Property<string> InspectionNoProperty = P<Calibration>.Register(e => e.InspectionNo);

		/// <summary>
		/// 检验单号
		/// </summary>
		public string InspectionNo
		{
			get { return GetProperty(InspectionNoProperty); }
			set { SetProperty(InspectionNoProperty, value); }
		}
		#endregion

		#region 计划名称 PlanName
		/// <summary>
		/// 计划名称
		/// </summary>
		[Required]
		[Label("计划名称")]
		public static readonly Property<string> PlanNameProperty = P<Calibration>.Register(e => e.PlanName);

		/// <summary>
		/// 计划名称
		/// </summary>
		public string PlanName
		{
			get { return GetProperty(PlanNameProperty); }
			set { SetProperty(PlanNameProperty, value); }
		}
		#endregion

		#region 计划检验日期 PlanInspectionDate
		/// <summary>
		/// 计划检验日期
		/// </summary>
		[Label("计划检验日期")]
		public static readonly Property<DateTime> PlanInspectionDateProperty = P<Calibration>.Register(e => e.PlanInspectionDate);

		/// <summary>
		/// 计划检验日期
		/// </summary>
		public DateTime PlanInspectionDate
		{
			get { return GetProperty(PlanInspectionDateProperty); }
			set { SetProperty(PlanInspectionDateProperty, value); }
		}
		#endregion

		#region 实际检验日期 ActualInspectionDate
		/// <summary>
		/// 实际检验日期
		/// </summary>
		[Label("实际检验日期")]
		public static readonly Property<DateTime?> ActualInspectionDateProperty = P<Calibration>.Register(e => e.ActualInspectionDate);

		/// <summary>
		/// 实际检验日期
		/// </summary>
		public DateTime? ActualInspectionDate
		{
			get { return GetProperty(ActualInspectionDateProperty); }
			set { SetProperty(ActualInspectionDateProperty, value); }
		}
		#endregion

		#region 备注 Remark
		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(1000)]
		[Label("备注")]
		public static readonly Property<string> RemarkProperty = P<Calibration>.Register(e => e.Remark);

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			get { return GetProperty(RemarkProperty); }
			set { SetProperty(RemarkProperty, value); }
		}
		#endregion

		#region 检验说明 InspectionRemark
		/// <summary>
		/// 检验说明
		/// </summary>
		[MaxLength(1000)]
		[Label("检验说明")]
		public static readonly Property<string> InspectionRemarkProperty = P<Calibration>.Register(e => e.InspectionRemark);

		/// <summary>
		/// 检验说明
		/// </summary>
		public string InspectionRemark
		{
			get { return GetProperty(InspectionRemarkProperty); }
			set { SetProperty(InspectionRemarkProperty, value); }
		}
		#endregion

		#region 采购订单 PoNo
		/// <summary>
		/// 采购订单
		/// </summary>
		[Label("采购订单")]
		public static readonly Property<string> PoNoProperty = P<Calibration>.Register(e => e.PoNo);

		/// <summary>
		/// 采购订单
		/// </summary>
		public string PoNo
		{
			get { return GetProperty(PoNoProperty); }
			set { SetProperty(PoNoProperty, value); }
		}
		#endregion

		#region 单据来源 BillSourceType
		/// <summary>
		/// 单据来源
		/// </summary>
		[Label("单据来源")]
		public static readonly Property<BillSourceType> BillSourceTypeProperty = P<Calibration>.Register(e => e.BillSourceType);

		/// <summary>
		/// 单据来源
		/// </summary>
		public BillSourceType BillSourceType
		{
			get { return GetProperty(BillSourceTypeProperty); }
			set { SetProperty(BillSourceTypeProperty, value); }
		}
		#endregion

		#region 检验机构 Agency
		/// <summary>
		/// 检验机构Id
		/// </summary>
		public static readonly IRefIdProperty AgencyIdProperty = P<Calibration>.RegisterRefId(e => e.AgencyId, ReferenceType.Normal);

		/// <summary>
		/// 检验机构Id
		/// </summary>
		public double? AgencyId
		{
			get { return (double?)GetRefNullableId(AgencyIdProperty); }
			set { SetRefNullableId(AgencyIdProperty, value); }
		}

		/// <summary>
		/// 检验机构
		/// </summary>
		public static readonly RefEntityProperty<Supplier> AgencyProperty = P<Calibration>.RegisterRef(e => e.Agency, AgencyIdProperty);

		/// <summary>
		/// 检验机构
		/// </summary>
		public Supplier Agency
		{
			get { return GetRefEntity(AgencyProperty); }
			set { SetRefEntity(AgencyProperty, value); }
		}
		#endregion

		#region 检验人 Inspector
		/// <summary>
		/// 检验人Id
		/// </summary>
		public static readonly IRefIdProperty InspectorIdProperty = P<Calibration>.RegisterRefId(e => e.InspectorId, ReferenceType.Normal);

		/// <summary>
		/// 检验人Id
		/// </summary>
		public double? InspectorId
		{
			get { return (double?)GetRefNullableId(InspectorIdProperty); }
			set { SetRefNullableId(InspectorIdProperty, value); }
		}

		/// <summary>
		/// 检验人
		/// </summary>
		public static readonly RefEntityProperty<Employee> InspectorProperty = P<Calibration>.RegisterRef(e => e.Inspector, InspectorIdProperty);

		/// <summary>
		/// 检验人
		/// </summary>
		public Employee Inspector
		{
			get { return GetRefEntity(InspectorProperty); }
			set { SetRefEntity(InspectorProperty, value); }
		}
		#endregion

		#region 检验状态 InspectionStatus
		/// <summary>
		/// 检验状态
		/// </summary>
		[Label("检验状态")]
		public static readonly Property<InspectionStatus> InspectionStatusProperty = P<Calibration>.Register(e => e.InspectionStatus);

		/// <summary>
		/// 检验状态
		/// </summary>
		public InspectionStatus InspectionStatus
		{
			get { return GetProperty(InspectionStatusProperty); }
			set { SetProperty(InspectionStatusProperty, value); }
		}
		#endregion

		#region 检验规程 InspectionRule
		/// <summary>
		/// 检验规程Id
		/// </summary>
		public static readonly IRefIdProperty InspectionRuleIdProperty = P<Calibration>.RegisterRefId(e => e.InspectionRuleId, ReferenceType.Normal);

		/// <summary>
		/// 检验规程Id
		/// </summary>
		public double InspectionRuleId
		{
			get { return (double)GetRefId(InspectionRuleIdProperty); }
			set { SetRefId(InspectionRuleIdProperty, value); }
		}

		/// <summary>
		/// 检验规程
		/// </summary>
		public static readonly RefEntityProperty<InspectionRule> InspectionRuleProperty = P<Calibration>.RegisterRef(e => e.InspectionRule, InspectionRuleIdProperty);

		/// <summary>
		/// 检验规程
		/// </summary>
		public InspectionRule InspectionRule
		{
			get { return GetRefEntity(InspectionRuleProperty); }
			set { SetRefEntity(InspectionRuleProperty, value); }
		}
		#endregion

		#region 校验类型 CheckCategory
		/// <summary>
		/// 校验类型
		/// </summary>
		[Label("校验类型")]
		public static readonly Property<CheckCategory> CheckCategoryProperty = P<Calibration>.RegisterView(e => e.CheckCategory,e=>e.InspectionRule.CheckCategory);

		/// <summary>
		/// 校验类型
		/// </summary>
		public CheckCategory CheckCategory
		{
			get { return GetProperty(CheckCategoryProperty); }
			set { SetProperty(CheckCategoryProperty, value); }
		}
		#endregion

		#region 审核状态 ApprovalStatus
		/// <summary>
		/// 审核状态
		/// </summary>
		[Label("审核状态")]
		public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<Calibration>.Register(e => e.ApprovalStatus);

		/// <summary>
		/// 审核状态
		/// </summary>
		public ApprovalStatus ApprovalStatus
		{
			get { return GetProperty(ApprovalStatusProperty); }
			set { SetProperty(ApprovalStatusProperty, value); }
		}
		#endregion

		#region 检验结果 InspectionResult
		/// <summary>
		/// 检验结果
		/// </summary>
		[Label("检验结果")]
		public static readonly Property<InspectionResult?> InspectionResultProperty = P<Calibration>.Register(e => e.InspectionResult);

		/// <summary>
		/// 检验结果
		/// </summary>
		public InspectionResult? InspectionResult
		{
			get { return GetProperty(InspectionResultProperty); }
			set { SetProperty(InspectionResultProperty, value); }
		}
		#endregion

		#region 操作记录 CalibrationResumeList
		/// <summary>
		/// 操作记录
		/// </summary>
		public static readonly ListProperty<EntityList<CalibrationResume>> CalibrationResumeListProperty = P<Calibration>.RegisterList(e => e.CalibrationResumeList);
		/// <summary>
		/// 操作记录
		/// </summary>
		public EntityList<CalibrationResume> CalibrationResumeList
		{
			get { return this.GetLazyList(CalibrationResumeListProperty); }
		}
		#endregion

		#region 检验规程 CalibrationItemList
		/// <summary>
		/// 检验规程
		/// </summary>
		public static readonly ListProperty<EntityList<CalibrationItem>> CalibrationItemListProperty = P<Calibration>.RegisterList(e => e.CalibrationItemList);
		/// <summary>
		/// 检验规程
		/// </summary>
		public EntityList<CalibrationItem> CalibrationItemList
		{
			get { return this.GetLazyList(CalibrationItemListProperty); }
		}
		#endregion

		#region 检验明细 CalibrationDetailList
		/// <summary>
		/// 检验明细
		/// </summary>
		public static readonly ListProperty<EntityList<CalibrationDetail>> CalibrationDetailListProperty = P<Calibration>.RegisterList(e => e.CalibrationDetailList);
		/// <summary>
		/// 检验明细
		/// </summary>
		public EntityList<CalibrationDetail> CalibrationDetailList
		{
			get { return this.GetLazyList(CalibrationDetailListProperty); }
		}
		#endregion

		#region 设备明细 CalibrationEquipmentList
		/// <summary>
		/// 设备明细
		/// </summary>
		public static readonly ListProperty<EntityList<CalibrationEquipment>> CalibrationEquipmentListProperty = P<Calibration>.RegisterList(e => e.CalibrationEquipmentList);
		/// <summary>
		/// 设备明细
		/// </summary>
		public EntityList<CalibrationEquipment> CalibrationEquipmentList
		{
			get { return this.GetLazyList(CalibrationEquipmentListProperty); }
		}
		#endregion

		#region 附件 CalibrationAttachmentList
		/// <summary>
		/// 附件
		/// </summary>
		public static readonly ListProperty<EntityList<CalibrationAttachment>> CalibrationAttachmentListProperty = P<Calibration>.RegisterList(e => e.CalibrationAttachmentList);
		/// <summary>
		/// 附件
		/// </summary>
		public EntityList<CalibrationAttachment> CalibrationAttachmentList
		{
			get { return this.GetLazyList(CalibrationAttachmentListProperty); }
		}
		#endregion

		#region 审核意见 ApprovalInfo
		/// <summary>
		/// 审核意见
		/// </summary>
		[MaxLength(2000)]
		[Label("审核意见")]
		public static readonly Property<string> ApprovalInfoProperty = P<Calibration>.Register(e => e.ApprovalInfo);

		/// <summary>
		/// 审核意见
		/// </summary>
		public string ApprovalInfo
		{
			get { return this.GetProperty(ApprovalInfoProperty); }
			set { this.SetProperty(ApprovalInfoProperty, value); }
		}
		#endregion

		#region 引用属性

		#region 机构名称 AgencyName
		/// <summary>
		/// 机构名称
		/// </summary>
		[Label("机构名称")]
		public static readonly Property<string> AgencyNameProperty = P<Calibration>.RegisterView(e => e.AgencyName, e => e.Agency.Name);

		/// <summary>
		/// 设备名称
		/// </summary>
		public string AgencyName
		{
			get { return GetProperty(AgencyNameProperty); }
			set { SetProperty(AgencyNameProperty, value); }
		}
		#endregion

		#region 检验规程名称 InspectionRuleName
		/// <summary>
		/// 机构名称
		/// </summary>
		[Label("检验规程名称")]
		public static readonly Property<string> InspectionRuleNameProperty = P<Calibration>.RegisterView(e => e.InspectionRuleName, e => e.InspectionRule.Name);

		/// <summary>
		/// 检验规程名称
		/// </summary>
		public string InspectionRuleName
		{
			get { return GetProperty(InspectionRuleNameProperty); }
			set { SetProperty(InspectionRuleNameProperty, value); }
		}
		#endregion

		#endregion

	}

	/// <summary>
	/// 计量设备定检 实体配置
	/// </summary>
	internal class CalibrationConfig : EntityConfig<Calibration>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_CAL").MapAllProperties();
			Meta.Property(Calibration.RemarkProperty).ColumnMeta.HasLength(2000);
			Meta.Property(Calibration.InspectionRemarkProperty).ColumnMeta.HasLength(2000);
			Meta.EnablePhantoms();
		}
	}
}