using SIE.Items;
using SIE.Wpf.Tech.Routings.Commands;

namespace SIE.Wpf.Tech.Routings
{
    /// <summary>
    /// 工艺路线工序BOM视图配置
    /// </summary>
    internal class ProcessBomViewConfig : WPFViewConfig<Item>
    {
        /// <summary>
        /// 工序BOM视图
        /// </summary>
        public const string ProcessBomViewGroup = "ProcessBomConfigView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ProcessBomViewGroup);
            if (ViewGroup == ProcessBomViewGroup)
            {
                ProcessBomConfigView();
            }
        }

        /// <summary>
        /// 工序BOM配置视图
        /// </summary>
        private void ProcessBomConfigView()
        {
            View.InlineEdit();
            View.ClearCommands();
            View.UseCommands(typeof(AddProcessBomByCtgCommand), typeof(AddProcessBomByItemCommand), WPFCommandNames.ListDelete);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("物料编码").Show();
                View.Property(p => p.Name).HasLabel("物料名称").Show();
                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            }
        }
    }
}
