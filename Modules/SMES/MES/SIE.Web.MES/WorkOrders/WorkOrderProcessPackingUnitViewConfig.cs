using SIE.MES.WorkOrders;
using SIE.Web.MES.WorkOrders;

namespace SIE.Web.MES
{
    /// <summary>
    /// 工序对应包装视图配置
    /// </summary>
    internal class WorkOrderProcessPackingUnitViewConfig : WebViewConfig<WorkOrderProcessPackingUnit>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(WorkOrderViewConfig.ReadonlyView);
            View.AssignAuthorize(typeof(WorkOrder));
            View.ClearCommands(false);
            if (ViewGroup == WorkOrderViewConfig.ListView)
            {
                View.InlineEdit();
                View.ClearCommands(true);
                View.UseCommands("SIE.Web.MES.WorkOrders.WoProcessPackUnitLookupCommand", "SIE.Web.MES.WorkOrders.Commands.WorkOrderDetailDelCommand");
            }

            using (View.OrderProperties())
            {
                View.Property(p => p.Process).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
