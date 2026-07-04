namespace SIE.Wpf.MES.Wip.Repairs
{
    /// <summary>
    /// 返修完工视图配置
    /// </summary>
    internal class GotoChildProcessViewModelViewConfig : WPFViewConfig<GotoChildProcessViewModel>
    {

        /// <summary>
        /// 弹出选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.PathName)
                .HasLabel("工艺路线")
                .Show(ShowInWhere.List);

            View.Property(p => p.PathDescription)
                .HasLabel("工艺路线描述")
                .ShowInList(gridWidth: 260);
        }
    }
}