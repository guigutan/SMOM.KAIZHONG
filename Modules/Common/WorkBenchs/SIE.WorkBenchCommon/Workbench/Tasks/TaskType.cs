using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.WorkBenchCommon.Workbench.Tasks
{
    /// <summary>
    /// 
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("任务类型"), DisplayMember(nameof(Name))]
    public partial class TaskType : DataEntity
    {
        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<TaskType>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 提供者 Provider
        /// <summary>
        /// 提供者
        /// </summary>
        [Label("提供者")]
        public static readonly Property<string> ProviderProperty = P<TaskType>.Register(e => e.Provider);

        /// <summary>
        /// 提供者
        /// </summary>
        public string Provider
        {
            get { return GetProperty(ProviderProperty); }
            set { SetProperty(ProviderProperty, value); }
        }
        #endregion

        #region 模块类型 TaskTypeCategory
        /// <summary>
        /// 任务类型分类
        /// </summary>
        [Label("模块类型")]
        public static readonly Property<ModuleCategory> ModuleCategoryProperty = P<TaskType>.Register(e => e.ModuleCategory);

        /// <summary>
        /// 任务类型分类
        /// </summary>
        public ModuleCategory ModuleCategory
        {
            get { return GetProperty(ModuleCategoryProperty); }
            set { SetProperty(ModuleCategoryProperty, value); }
        }
        #endregion

        #region 图标 Icon
        /// <summary>
        /// 图标
        /// </summary>
        [Label("图标")]
        public static readonly Property<string> IconProperty = P<TaskType>.Register(e => e.Icon);

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon
        {
            get { return this.GetProperty(IconProperty); }
            set { this.SetProperty(IconProperty, value); }
        }
        #endregion
    } 

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class TaskTypeConfig : EntityConfig<TaskType>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WB_TASK_TYPE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}