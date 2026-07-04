using SIE.Tech.Stations;
using SIE.Wpf.Resources;

namespace SIE.Wpf.MES.LoadItems
{
    /// <summary>
    /// 工位挪料视图配置
    /// </summary>
    internal class MoveItemViewModelViewConfig : WPFViewConfig<MoveItemViewModel>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDetailColumnsCount(2);
            using (View.OrderProperties())
            {
                View.Property(p => p.Qty).HasLabel("配送数量").UseSpinEditor(e => e.MinValue = 0).Show(ShowInWhere.All);
                View.Property(p => p.Resource).UseEquipEnterpriseResourceEditor(p => p.ReloadDataOnPopping = true).HasLabel("目标产线").Show(ShowInWhere.All);
                View.Property(p => p.Process).UsePagingLookUpEditor().HasLabel("目标工序").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Station).UsePagingLookUpEditor(p => p.ReloadDataOnPopping = true).HasLabel("目标工位").Show(ShowInWhere.All).UseDataSource((e, p, c) =>
                    {
                        var model = e as MoveItemViewModel;
                        if (model.Process == null || model.Resource == null)
                            return new Domain.EntityList<Station>();
                        return RT.Service.Resolve<StationController>().GetStationsByResourceId(model.ResourceId, model.ProcessId);
                    });
            }
        }
    }
}
