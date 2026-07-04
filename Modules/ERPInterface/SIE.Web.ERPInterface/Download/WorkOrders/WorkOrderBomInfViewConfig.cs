using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.MetaModel.View;

namespace SIE.Web.ERPInterface.Download.WorkOrders
{
    /// <summary>
    /// 工单BOM中间表视图配置
    /// </summary>
    internal class WorkOrderBomInfViewConfig : WebViewConfig<WorkOrderBomInf>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.Property(p => p.RequireQty);
            View.Property(p => p.SingleQty);
            View.Property(p => p.IsRecoilItem);
            View.Property(p => p.IsVritualItem);
            View.Property(p => p.Remark);
            View.Property(p => p.ItemCode);
            View.Property(p => p.WoNo);
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.WoNo);
            View.Property(p => p.ItemCode);
        }
    }
}