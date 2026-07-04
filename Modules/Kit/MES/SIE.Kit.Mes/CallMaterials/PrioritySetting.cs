using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Kit.MES.CallMaterials
{
    /// <summary>
    /// 排序优先级设置
    /// </summary>
    [ChildEntity, Serializable]
    [Label("排序优先级设置")]
    public partial class PrioritySetting : DataEntity
    {
        #region 排序属性 SortPropertyName
        /// <summary>
        /// 排序属性
        /// </summary>
        [Label("排序属性")]
        public static readonly Property<string> SortPropertyNameProperty = P<PrioritySetting>.Register(e => e.SortPropertyName);

        /// <summary>
        /// 排序属性
        /// </summary>
        public string SortPropertyName
        {
            get { return this.GetProperty(SortPropertyNameProperty); }
            set { this.SetProperty(SortPropertyNameProperty, value); }
        }
        #endregion

        #region 条件 Condition
        /// <summary>
        /// 条件
        /// </summary>
        [Label("条件")]
        [Required]
        public static readonly Property<string> ConditionProperty = P<PrioritySetting>.Register(e => e.Condition);

        /// <summary>
        /// 条件
        /// </summary>
        public string Condition
        {
            get { return GetProperty(ConditionProperty); }
            set { SetProperty(ConditionProperty, value); }
        }
        #endregion

        #region 优先级，数值越大优先级越高 Priority
        /// <summary>
        /// 优先级，数值越大优先级越高
        /// </summary>
        [Label("优先级，数值越大优先级越高")]
        [Required]
        public static readonly Property<int> PriorityProperty = P<PrioritySetting>.Register(e => e.Priority);

        /// <summary>
        /// 优先级，数值越大优先级越高
        /// </summary>
        public int Priority
        {
            get { return GetProperty(PriorityProperty); }
            set { SetProperty(PriorityProperty, value); }
        }
        #endregion

        #region 排序模式 SortMode
        /// <summary>
        /// 排序模式
        /// </summary>
        [Label("排序模式")]
        [Required]
        public static readonly Property<SortMode> SortModeProperty = P<PrioritySetting>.Register(e => e.SortMode);

        /// <summary>
        /// 排序模式
        /// </summary>
        public SortMode SortMode
        {
            get { return GetProperty(SortModeProperty); }
            set { SetProperty(SortModeProperty, value); }
        }
        #endregion

        #region 排序方案设置 Solutions
        /// <summary>
        /// 排序方案设置Id
        /// </summary>
        public static readonly IRefIdProperty SolutionsIdProperty = P<PrioritySetting>.RegisterRefId(e => e.SolutionsId, ReferenceType.Parent);

        /// <summary>
        /// 排序方案设置Id
        /// </summary>
        public double SolutionsId
        {
            get { return (double)GetRefId(SolutionsIdProperty); }
            set { SetRefId(SolutionsIdProperty, value); }
        }

        /// <summary>
        /// 排序方案设置
        /// </summary>
        public static readonly RefEntityProperty<SortSolutionsSetting> SolutionsProperty = P<PrioritySetting>.RegisterRef(e => e.Solutions, SolutionsIdProperty);

        /// <summary>
        /// 排序方案设置
        /// </summary>
        public SortSolutionsSetting Solutions
        {
            get { return GetRefEntity(SolutionsProperty); }
            set { SetRefEntity(SolutionsProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 排序优先级设置 实体配置
    /// </summary>
    internal class PrioritySettingConfig : EntityConfig<PrioritySetting>
    {
        /// <summary>
        /// 配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_CALL_PRIORITY_SET").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}