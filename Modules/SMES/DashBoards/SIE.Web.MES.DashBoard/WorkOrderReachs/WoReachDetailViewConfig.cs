using SIE.MES.DashBoard.WorkOrderReachs;
using SIE.MetaModel.View;

namespace SIE.Web.MES.DashBoard.WorkOrderReachs
{
    /// <summary>
    /// 达成率明细
    /// </summary>
    public class WoReachDetailViewConfig : WebViewConfig<WoReachDetailViewModel>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(WoReachReportViewModel));
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.MES.DashBoard.WoReachDetailBehavior");
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls);
            View.Property(p => p.No).ShowInList(120);
            View.Property(p => p.ProductCode).Readonly().HasLabel("产品编码");
            View.Property(p => p.WorkShopName).Readonly();
            View.Property(p => p.ResourceName).Readonly();
            View.Property(p => p.PlanQty).Readonly();
            View.Property(p => p.FinishQty).Readonly();
            View.Property(p => p.State).Readonly();
            View.Property(p => p.ReachState).Readonly();
            View.Property(p => p.PlanBeginDate).ShowInList(150).Readonly();
            View.Property(p => p.PlanEndDate).ShowInList(150).Readonly();
            View.Property(p => p.ActuStartDate).ShowInList(150).Readonly();
            View.Property(p => p.ActuFinishDate).ShowInList(150).Readonly();
        }
    }
}
