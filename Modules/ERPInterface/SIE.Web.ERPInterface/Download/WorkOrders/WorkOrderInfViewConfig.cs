using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.MetaModel.View;

namespace SIE.Web.ERPInterface.Download.WorkOrders
{
    /// <summary>
    /// 工单中间表视图配置
    /// </summary>
    internal class WorkOrderInfViewConfig : WebViewConfig<WorkOrderInf>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.Property(p => p.No);
            View.Property(p => p.PlanBeginDate);
            View.Property(p => p.PlanEndDate);
            View.Property(p => p.PlanQty);
            View.Property(p => p.CustomerOrderNo);
            View.Property(p => p.CustomerCode);
            View.Property(p => p.OrderQty);
            View.Property(p => p.MakeDate);
            View.Property(p => p.SaleOrderNo);
            View.Property(p => p.ProductCode);
            View.Property(p => p.WorkshopCode);
            View.Property(p => p.UpdateByCode);
            View.Property(p => p.ClosedCode);
            View.Property(p => p.MakerCode);
            View.Property(p => p.ResourceCode);
            View.Property(p => p.WorkOrderType);
            View.Property(p => p.WorkOrderState);
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.No);
        }
    }
}