using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Kit.MES.CallMaterials
{
    /// <summary>
    /// 排序方案设置
    /// </summary>
    [RootEntity, Serializable]
    [Label("排序方案设置")]
    public partial class SortSolutionsSetting : DataEntity
    {
        #region 方案名称 Name
        /// <summary>
        /// 方案名称
        /// </summary>
        [Label("方案名称")]
        [MaxLength(20)]
        [Required, NotDuplicate]
        public static readonly Property<string> NameProperty = P<SortSolutionsSetting>.Register(e => e.Name);

        /// <summary>
        /// 方案名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 方案概述 Description
        /// <summary>
        /// 方案概述
        /// </summary>
        [Label("方案概述")]
        [Required]
        [MaxLength(100)]
        public static readonly Property<string> DescriptionProperty = P<SortSolutionsSetting>.Register(e => e.Description);

        /// <summary>
        /// 方案概述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 是否缺省 IsDefault
        /// <summary>
        /// 是否缺省
        /// </summary>
        [Label("是否缺省")]
        public static readonly Property<bool> IsDefaultProperty = P<SortSolutionsSetting>.Register(e => e.IsDefault);

        /// <summary>
        /// 是否缺省
        /// </summary>
        public bool IsDefault
        {
            get { return GetProperty(IsDefaultProperty); }
            set { SetProperty(IsDefaultProperty, value); }
        }
        #endregion

        #region 排序优先级设置列表 PriorityList
        /// <summary>
        /// 排序优先级设置列表
        /// </summary>
        public static readonly ListProperty<EntityList<PrioritySetting>> PriorityListProperty = P<SortSolutionsSetting>.RegisterList(e => e.PriorityList);

        /// <summary>
        /// 排序优先级设置列表
        /// </summary>
        public EntityList<PrioritySetting> PriorityList
        {
            get { return this.GetLazyList(PriorityListProperty); }
        }
        #endregion

        #region 排序优先级模型列表 PriorityVMList
        /// <summary>
        /// 排序优先级模型列表
        /// </summary>
        [Label("排序优先级模型列表")]
        public static readonly Property<string> PriorityVMListProperty = P<SortSolutionsSetting>.Register(e => e.PriorityVMList);

        /// <summary>
        /// 排序优先级模型列表
        /// </summary>
        public string PriorityVMList
        {
            get { return GetProperty(PriorityVMListProperty); }
            set { SetProperty(PriorityVMListProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 排序方案设置 实体配置
    /// </summary>
    internal class SortSolutionsSettingConfig : EntityConfig<SortSolutionsSetting>
    {
        /// <summary>
        /// 配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_CALL_SORT_SET").MapAllPropertiesExcept(SortSolutionsSetting.PriorityVMListProperty);
            Meta.Property(SortSolutionsSetting.DescriptionProperty).ColumnMeta.HasLength(2400);
            Meta.EnablePhantoms();
        }
    }
}