using SIE.MES.Workbench.Experiences;
using SIE.Wpf.Common.Sort;

namespace SIE.WPF.MES.Workbench.Experiences
{
    /// <summary>
    /// 经验明细视图配置
    /// </summary>
    internal class ExperienceDetailViewConfig : WPFViewConfig<ExperienceDetail>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultBehaviors();
            View.FormEdit();
        }

        protected override void ConfigListView()
        {
            View.UseCommands(WPFCommandNames.ListAdd, WPFCommandNames.ListEdit, WPFCommandNames.ListDelete, WPFCommandNames.ListSave, typeof(MoveUpCommand), typeof(MoveDownCommand), typeof(MoveTopCommand), typeof(MoveBottomCommand));
            View.Property(p => p.Description).UseMemoEditor();
            View.Property(p => p.IsRight);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.UseCommands(WPFCommandNames.FormSave);
            View.UseCommands(typeof(MoveUpCommand), typeof(MoveDownCommand), typeof(MoveTopCommand), typeof(MoveBottomCommand));
            View.Property(p => p.Description).UseMemoEditor();
            View.Property(p => p.IsRight);
            View.Property(p => p.Photo).UseImageEditor().ShowInDetail(columnSpan: 2, rowSpan: 4);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置下拉视图
        }
    }
}
