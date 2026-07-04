using SIE.Core.Enums;
using SIE.Domain;
using SIE.EMS.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.InspectionRules
{
    /// <summary>
    /// 检验规程
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(InspectionRuleCriteria))]
    [DisplayMember(nameof(Code))]
    [Label("检验规程")]
    public partial class InspectionRule : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<InspectionRule>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<InspectionRule>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 周期(天) PeriodDays
        /// <summary>
        /// 周期(天)
        /// </summary>
        [Required]
        [Label("周期(天)")]
        public static readonly Property<int> PeriodDaysProperty = P<InspectionRule>.Register(e => e.PeriodDays);

        /// <summary>
        /// 周期(天)
        /// </summary>
        public int PeriodDays
        {
            get { return GetProperty(PeriodDaysProperty); }
            set { SetProperty(PeriodDaysProperty, value); }
        }
        #endregion

        #region 预警期(天) WarningPeriod
        /// <summary>
        /// 预警期(天)
        /// </summary>
        [Required]
        [Label("预警期(天)")]
        public static readonly Property<int> WarningPeriodProperty = P<InspectionRule>.Register(e => e.WarningPeriod);

        /// <summary>
        /// 预警期(天)
        /// </summary>
        public int WarningPeriod
        {
            get { return GetProperty(WarningPeriodProperty); }
            set { SetProperty(WarningPeriodProperty, value); }
        }
        #endregion

        #region 项目类型 InspectionRuleType
        /// <summary>
        /// 项目类型
        /// </summary>
        [Label("项目类型")]
        public static readonly Property<InspectionRuleType> InspectionRuleTypeProperty = P<InspectionRule>.Register(e => e.InspectionRuleType);

        /// <summary>
        /// 项目类型
        /// </summary>
        public InspectionRuleType InspectionRuleType
        {
            get { return GetProperty(InspectionRuleTypeProperty); }
            set { SetProperty(InspectionRuleTypeProperty, value); }
        }
        #endregion

        #region 校验类别 CheckCategory
        /// <summary>
        /// 校验类别
        /// </summary>
        [Label("校验类别")]
        public static readonly Property<CheckCategory> CheckCategoryProperty = P<InspectionRule>.Register(e => e.CheckCategory);

        /// <summary>
        /// 校验类别
        /// </summary>
        public CheckCategory CheckCategory
        {
            get { return GetProperty(CheckCategoryProperty); }
            set { SetProperty(CheckCategoryProperty, value); }
        }
        #endregion

        #region 检验项目列表 InspectionProjectItemList
        /// <summary>
        /// 检验项目列表
        /// </summary>
        public static readonly ListProperty<EntityList<InspectionProjectItem>> InspectionProjectItemListProperty = P<InspectionRule>.RegisterList(e => e.InspectionProjectItemList);
        /// <summary>
        /// 检验项目列表
        /// </summary>
        public EntityList<InspectionProjectItem> InspectionProjectItemList
        {
            get { return this.GetLazyList(InspectionProjectItemListProperty); }
        }
        #endregion
    }


    /// <summary>
    /// 检验规程 实体配置
    /// </summary>
    internal class InspectionRuleConfig : EntityConfig<InspectionRule>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_INS_RULE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}