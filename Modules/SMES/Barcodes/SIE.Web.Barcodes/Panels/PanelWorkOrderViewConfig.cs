using SIE.Barcodes.Panels;
using SIE.Domain;

namespace SIE.Web.Barcodes.Panels
{
    /// <summary>
    /// 拼板码打印视图
    /// </summary>
    public class PanelWorkOrderViewConfig : WebViewConfig<PanelWorkOrder>
    {
        /// <summary>
        /// 拼板码打印视图
        /// </summary>
        public static readonly string PanelPrintView = "PanelPrintView";

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(PanelWorkOrder));
            View.DeclareExtendViewGroup(PanelPrintView);
            if (ViewGroup == PanelPrintView)
                ConfigPanelPrintView();
        }

        /// <summary>
        /// 拼板码打印视图
        /// </summary>
        void ConfigPanelPrintView()
        {
            View.FormEdit();
            View.UseCommands("SIE.Web.Barcodes.Panels.Commands.PanelPrintCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.No).HasLabel("MES工单号").Readonly().ShowInList(width: 180);
                View.Property(p => p.WorkOrderProductCode).HasLabel("产品编码").Readonly().ShowInList(width: 150);
                View.Property(p => p.WorkOrderProductName).HasLabel("产品名称").Readonly().ShowInList(width: 150);
                View.Property(p => p.PlanQty).Readonly().Show();
                View.Property(p => p.PanelQty).Readonly().Show();
                View.Property(p => p.PanelPrintQty).HasLabel("已打印数量").Readonly().Show();
                View.Property(p => p.State).Readonly().Show();
                View.Property(p => p.PlanBeginDate).Readonly().ShowInList(width: 150);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.BomList).Show(ChildShowInWhere.Hide).HasLabel("工单BOM信息");
                View.AttachChildrenProperty(typeof(Panel), (e) =>
                {
                    var child = e as ChildPagingDataArgs;
                    var elecWorkOrder = e.Parent as PanelWorkOrder;
                    if (elecWorkOrder == null) return new EntityList<Panel>();
                    return RT.Service.Resolve<PanelController>().GetPanelsByWorkOrderId(elecWorkOrder.Id, child.PagingInfo, child.SortInfo);
                }).HasLabel("打印明细").Show(ChildShowInWhere.All);
            }
        }
    }
}
