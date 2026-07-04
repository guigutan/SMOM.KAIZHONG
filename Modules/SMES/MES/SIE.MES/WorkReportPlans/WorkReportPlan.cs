using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WorkReportPlans
{
    /// <summary>
    /// 工单
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(WorkReportPlanCriteria))]
    [Label("报工方案配置")]
    [DisplayMember(nameof(PlanName))]
    public partial class WorkReportPlan : DataEntity
    {

        #region 工序 ProcessInfoList
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly ListProperty<EntityList<ProcessInfo>> ProcessInfoListProperty = P<WorkReportPlan>.RegisterList(e => e.ProcessInfoList);

        /// <summary>
        /// 工序
        /// </summary>
        public EntityList<ProcessInfo> ProcessInfoList
        {
            get { return this.GetLazyList(ProcessInfoListProperty); }
        }
        #endregion

        #region 方案编码 PlanCode
        /// <summary>
        /// 方案编码
        /// </summary>
        [Label("方案编码")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> PlanCodeProperty = P<WorkReportPlan>.Register(e => e.PlanCode);

        /// <summary>
        /// 方案编码
        /// </summary>
        public string PlanCode
        {
            get { return this.GetProperty(PlanCodeProperty); }
            set { this.SetProperty(PlanCodeProperty, value); }
        }
        #endregion

        #region 方案名称 PlanName
        /// <summary>
        /// 方案名称
        /// </summary>
        [Label("方案名称")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> PlanNameProperty = P<WorkReportPlan>.Register(e => e.PlanName);

        /// <summary>
        /// 方案名称
        /// </summary>
        public string PlanName
        {
            get { return this.GetProperty(PlanNameProperty); }
            set { this.SetProperty(PlanNameProperty, value); }
        }
        #endregion

        #region 方案描述 Description
        /// <summary>
        /// 方案描述
        /// </summary>
        [Label("方案描述")]
        [MaxLength(1000)]
        public static readonly Property<string> DescriptionProperty = P<WorkReportPlan>.Register(e => e.Description);

        /// <summary>
        /// 方案描述
        /// </summary>
        public string Description
        {
            get { return this.GetProperty(DescriptionProperty); }
            set { this.SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 模板名称 PlanTemplateName
        /// <summary>
        /// 模板名称 枚举
        /// </summary>
        [Label("模板名称")]
        [Required]
        public static readonly Property<TemplateNames> PlanTemplateNameProperty = P<WorkReportPlan>.Register(e => e.PlanTemplateName);

        /// <summary>
        /// 模板名称
        /// </summary>
        public TemplateNames PlanTemplateName
        {
            get { return this.GetProperty(PlanTemplateNameProperty); }
            set { this.SetProperty(PlanTemplateNameProperty, value); }
        }
        #endregion

        #region 启用 EnableStatus
        /// <summary>
        /// 启用 
        /// </summary>
        [Label("启用")]
        [Required]
        public static readonly Property<bool> EnableStatusProperty = P<WorkReportPlan>.Register(e => e.EnableStatus);

        /// <summary>
        /// 启用
        /// </summary>
        public bool EnableStatus
        {
            get { return this.GetProperty(EnableStatusProperty); }
            set { this.SetProperty(EnableStatusProperty, value); }
        }
        #endregion

        #region 是否默认 IsDefault
        /// <summary>
        /// 是否默认 
        /// </summary>
        [Label("是否默认")]
        [Required]
        public static readonly Property<bool> IsDefaultProperty = P<WorkReportPlan>.Register(e => e.IsDefault);

        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault
        {
            get { return this.GetProperty(IsDefaultProperty); }
            set { this.SetProperty(IsDefaultProperty, value); }
        }
        #endregion
    }
    internal class WorkReportPlanConfig : EntityConfig<WorkReportPlan>
    {
        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WO_REPORT_PLAN").MapAllProperties();
            Meta.Property(WorkReportPlan.PlanCodeProperty).ColumnMeta.HasIndex();
            Meta.Property(WorkReportPlan.DescriptionProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}
