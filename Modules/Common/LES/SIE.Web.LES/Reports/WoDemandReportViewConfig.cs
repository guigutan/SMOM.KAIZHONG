using SIE.LES.Reports;
using SIE.MetaModel.View;
using System;

namespace SIE.Web.LES.Reports
{
    /// <summary>
    /// 工单需求汇总报表 视图配置
    /// </summary>
    public class WoDemandReportViewConfig : WebViewConfig<WoDemandReport>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseClientOrder();
            View.DisableEditing();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.WoNo).ShowInList(width: 150);
                View.Property(p => p.ItemCode).ShowInList(width: 150);
                View.Property(p => p.ItemName).ShowInList(width: 200);
                View.Property(p => p.ItemExtProp).ShowInList(width: 150);
                View.Property(p => p.ItemType).ShowInList();

                View.Property(p => p.ReceivedQty).ShowInList(width: 100);
                View.Property(p => p.FeedQty).ShowInList(width: 100);

                using (View.DeclareBand("线边剩余数".L10nFormat()))
                {
                    View.Property(p => p.AvailableQty).ShowInList(width: 100);
                    View.Property(p => p.NgQty).ShowInList(width: 100);
                }
                View.Property(p => p.MovedOutQty).ShowInList();
                View.Property(p => p.MovedInQty).ShowInList();

                View.Property(p => p.ReturnQtyInTransit).ShowInList();
                View.Property(p => p.NgReturnQtyInTransit).ShowInList();

                View.Property(p => p.ReturnQty).ShowInList();
                View.Property(p => p.NgReturnQty).ShowInList();

                View.Property(p => p.WarehouseCode).ShowInList();
                View.Property(p => p.WarehouseName).ShowInList();

                View.Property(p => p.WorkShopName).ShowInList();
                View.Property(p => p.ResourceName).ShowInList();

            }
        }

    }
}
