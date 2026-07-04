using SIE.Domain;
using SIE.EMS.MainenanceProjects;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.InspectionRules
{
    /// <summary>
    /// 检验项目
    /// </summary>
    [RootEntity, Serializable]
    [Label("检验项目")]
    public partial class InspectionProjectItem : DataEntity
    {
        #region 标准 Standard
        /// <summary>
        /// 标准
        /// </summary>
        [Label("标准")]
        public static readonly Property<string> StandardProperty = P<InspectionProjectItem>.Register(e => e.Standard);

        /// <summary>
        /// 标准
        /// </summary>
        public string Standard
        {
            get { return GetProperty(StandardProperty); }
            set { SetProperty(StandardProperty, value); }
        }
        #endregion

        #region 最小值 MinValue
        /// <summary>
        /// 最小值
        /// </summary>
        [Label("最小值")]
        public static readonly Property<decimal?> MinValueProperty = P<InspectionProjectItem>.Register(e => e.MinValue);

        /// <summary>
        /// 最小值
        /// </summary>
        public decimal? MinValue
        {
            get { return GetProperty(MinValueProperty); }
            set { SetProperty(MinValueProperty, value); }
        }
        #endregion

        #region 最大值 MaxValue
        /// <summary>
        /// 最大值
        /// </summary>
        [Label("最大值")]
        public static readonly Property<decimal?> MaxValueProperty = P<InspectionProjectItem>.Register(e => e.MaxValue);

        /// <summary>
        /// 最大值
        /// </summary>
        public decimal? MaxValue
        {
            get { return GetProperty(MaxValueProperty); }
            set { SetProperty(MaxValueProperty, value); }
        }
        #endregion

        #region 点检保养项目 ProjectDetail
        /// <summary>
        /// 点检保养项目Id
        /// </summary>
        public static readonly IRefIdProperty ProjectDetailIdProperty = P<InspectionProjectItem>.RegisterRefId(e => e.ProjectDetailId, ReferenceType.Normal);

        /// <summary>
        /// 点检保养项目Id
        /// </summary>
        public double ProjectDetailId
        {
            get { return (double)GetRefId(ProjectDetailIdProperty); }
            set { SetRefId(ProjectDetailIdProperty, value); }
        }

        /// <summary>
        /// 点检保养项目
        /// </summary>
        public static readonly RefEntityProperty<ProjectDetail> ProjectDetailProperty = P<InspectionProjectItem>.RegisterRef(e => e.ProjectDetail, ProjectDetailIdProperty);

        /// <summary>
        /// 点检保养项目
        /// </summary>
        public ProjectDetail ProjectDetail
        {
            get { return GetRefEntity(ProjectDetailProperty); }
            set { SetRefEntity(ProjectDetailProperty, value); }
        }
        #endregion

        #region 检验规程 InspectionRule
        /// <summary>
        /// 检验规程Id
        /// </summary>
        public static readonly IRefIdProperty InspectionRuleIdProperty = P<InspectionProjectItem>.RegisterRefId(e => e.InspectionRuleId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<InspectionRule> InspectionRuleProperty = P<InspectionProjectItem>.RegisterRef(e => e.InspectionRule, InspectionRuleIdProperty);

        /// <summary>
        /// 检验规程
        /// </summary>
        public InspectionRule InspectionRule
        {
            get { return GetRefEntity(InspectionRuleProperty); }
            set { SetRefEntity(InspectionRuleProperty, value); }
        }
        #endregion

        #region 视图引用属性

        #region 项目名称 ProjectName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> ProjectNameProperty = P<InspectionProjectItem>.RegisterView(e => e.ProjectName, p => p.ProjectDetail.Name);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get { return this.GetProperty(ProjectNameProperty); }
            set { SetProperty(ProjectNameProperty, value); }
        }
        #endregion
                
        #region 部位 Part
        /// <summary>
        /// 部位
        /// </summary>
        [Label("部位")]
        public static readonly Property<string> PartProperty = P<InspectionProjectItem>.RegisterView(e => e.Part, p => p.ProjectDetail.Part);

        /// <summary>
        /// 部位
        /// </summary>
        public string Part
        {
            get { return this.GetProperty(PartProperty); }
            set { SetProperty(PartProperty, value); }
        }
        #endregion

        #region 项目耗材 Consumable
        /// <summary>
        /// 项目耗材
        /// </summary>
        [Label("项目耗材")]
        public static readonly Property<string> ConsumableProperty = P<InspectionProjectItem>.RegisterView(e => e.Consumable, p => p.ProjectDetail.Consumable);

        /// <summary>
        /// 项目耗材
        /// </summary>
        public string Consumable
        {
            get { return this.GetProperty(ConsumableProperty); }
            set { SetProperty(ConsumableProperty, value); }
        }
        #endregion

        #region 操作方法 Method
        /// <summary>
        /// 操作方法
        /// </summary>
        [Label("操作方法")]
        public static readonly Property<string> MethodProperty = P<InspectionProjectItem>.RegisterView(e => e.Method, p => p.ProjectDetail.Method);

        /// <summary>
        /// 操作方法
        /// </summary>
        public string Method
        {
            get { return this.GetProperty(MethodProperty); }
            set { SetProperty(MethodProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitProperty = P<InspectionProjectItem>.RegisterView(e => e.Unit, p => p.ProjectDetail.Unit);

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get { return this.GetProperty(UnitProperty); }
            set { SetProperty(UnitProperty, value); }
        }
        #endregion

        #region 用时(分钟) UseTime
        /// <summary>
        /// 用时(分钟)
        /// </summary>
        [Label("用时(分钟)")]
        public static readonly Property<string> UseTimeProperty = P<InspectionProjectItem>.RegisterView(e => e.UseTime, p => p.ProjectDetail.UseTime);

        /// <summary>
        /// 用时(分钟)
        /// </summary>
        public string UseTime
        {
            get { return GetProperty(UseTimeProperty); }
            set { SetProperty(UseTimeProperty, value); }
        }
        #endregion

        #region 项目类型 ProjectType
        /// <summary>
        /// 项目类型
        /// </summary>
        [Label("项目类型")]
        public static readonly Property<ProjectType> ProjectTypeProperty = P<InspectionProjectItem>.RegisterView(e => e.ProjectType, p => p.ProjectDetail.ProjectType);

        /// <summary>
        /// 项目类型
        /// </summary>
        public ProjectType ProjectType
        {
            get { return GetProperty(ProjectTypeProperty); }
            set { SetProperty(ProjectTypeProperty, value); }
        }
        #endregion

        #region 周期类型 CycleType
        /// <summary>
        /// 周期类型
        /// </summary>
        [Label("周期类型")]
        public static readonly Property<CycleType> CycleTypeProperty = P<InspectionProjectItem>.RegisterView(e => e.CycleType, p => p.ProjectDetail.CycleType);

        /// <summary>
        /// 周期类型
        /// </summary>
        public CycleType CycleType
        {
            get { return GetProperty(CycleTypeProperty); }
            set { SetProperty(CycleTypeProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 检验项目 实体配置
    /// </summary>
    internal class InspectionProjectItemConfig : EntityConfig<InspectionProjectItem>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_INS_RULE_ITEM").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}