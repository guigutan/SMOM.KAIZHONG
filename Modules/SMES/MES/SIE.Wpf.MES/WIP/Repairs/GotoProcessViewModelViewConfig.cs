using SIE.MES.WIP.Repairs;

namespace SIE.Wpf.MES.WIP.Repairs
{
    /// <summary>
    /// 返修完工视图配置
    /// </summary>
    internal class GotoProcessViewModelViewConfig : WPFViewConfig<GotoProcessViewModel>
    {

        /// <summary>
        /// 弹出选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.PathName)
                .HasLabel("工序")
                .Show(ShowInWhere.List);

            View.Property(p => p.PathDescription)
                .HasLabel("后工序")
                .ShowInList(gridWidth: 260);
        }
    }
}