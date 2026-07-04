using SIE.MES.Workbench.Experiences;
using SIE.Wpf.MES.Workbench.Experiences.Commands;

namespace SIE.Wpf.MES.Workbench.Experiences
{
    /// <summary>
    /// 历史经验库视图配置
    /// </summary>
    internal class HistoryExperienceViewConfig : WPFViewConfig<HistoryExperience>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
            View.UseDefaultBehaviors();
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(ExperienceAddCommand), typeof(ExperienceDeleteCommand), WPFCommandNames.ListSave);
            View.Property(p => p.Item).HasLabel("物料编码").UsePagingLookUpEditor();
            View.Property(p => p.Item.Name).HasLabel("物料名称").Readonly(true);
            View.ChildrenProperty(p => p.ExperienceDetailList).Show(ChildShowInWhere.List);
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Item).HasLabel("物料");
        }
    }
}
