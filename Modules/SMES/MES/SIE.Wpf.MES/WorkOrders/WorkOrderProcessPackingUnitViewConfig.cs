using SIE.MES.WorkOrders;
using SIE.Tech.Processs;
using SIE.Wpf.MES.WorkOrders;
using SIE.Wpf.MES.WorkOrders.Commands;

namespace SIE.Wpf.MES
{
    /// <summary>
    /// 工序对应包装视图配置
    /// </summary>
    internal class WorkOrderProcessPackingUnitViewConfig : WPFViewConfig<WorkOrderProcessPackingUnit>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(WorkOrderViewConfig.ReadonlyView);
            View.AssignAuthorize(typeof(WorkOrderPackageRuleDetail));
            View.ClearCommands(false);
            if (ViewGroup == WorkOrderViewConfig.ListView)
            {
                View.InlineEdit();
                View.ClearCommands(true);
                View.UseCommands(typeof(WoProcessPackUnitLookupCommand), WPFCommandNames.ListDelete);
            }

            using (View.OrderProperties())
            {
                View.Property(p => p.ProcessId).UsePagingLookUpEditor(p => p.DisplayMember = Process.NameProperty.Name).Show(ShowInWhere.All);
            }
        }
    }
}
