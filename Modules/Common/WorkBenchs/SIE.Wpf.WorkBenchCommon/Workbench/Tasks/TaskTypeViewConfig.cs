using SIE.WorkBenchCommon.Workbench.Tasks;

namespace SIE.Wpf.WorkBenchCommon.Workbench.Tasks
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class TaskTypeViewConfig : WPFViewConfig<TaskType>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.Name);
            View.Property(p => p.Provider);
            View.Property(p => p.ModuleCategory);
            View.Property(p => p.Icon).UseEditor(WPFEditorNames.Icon);
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.Name);
            View.Property(p => p.Provider);
            View.Property(p => p.ModuleCategory);
            View.Property(p => p.Icon).UseIconEditor();
        }

        protected override void ConfigQueryView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.ModuleCategory).UseEnumEditor(p => p.AllowNullInput = true);
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.ModuleCategory);
        }
    }
}
