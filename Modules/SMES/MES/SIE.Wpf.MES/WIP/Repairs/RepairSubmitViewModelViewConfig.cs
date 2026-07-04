using SIE.MES.WIP.Repairs;

namespace SIE.Wpf.MES.WIP.Repairs
{
    /// <summary>
    /// 返修提交视图配置
    /// </summary>
    internal class RepairSubmitViewModelViewConfig : WPFViewConfig<RepairSubmitViewModel>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.DomainName("选择维修工艺路线");
            View.HasDetailColumnsCount(1).ClearCommands();

            using (View.OrderProperties())
            {
                View.Property(p => p.OptionalPathViewModel)
                    .HasLabel("工艺路线").Show(ShowInWhere.All)
                    .UseDataSource(RepairSubmitViewModel.OptionalPathViewModelListProperty);
                View.Property(p => p.OptionalPathViewModel.PathDescription)
                    .Show(ShowInWhere.Detail)
                    .Readonly()
                    .UseMemoEditor()
                    .HasLabel("工艺路线描述");
            }
        }
    }
}
