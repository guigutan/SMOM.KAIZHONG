using SIE.LES.StockOrders;
using SIE.MetaModel.View;

namespace SIE.Web.LES.StockOrders
{
    /// <summary>
    /// 接收记录视图配置
    /// </summary>
    public class StockOrderSnViewConfig : WebViewConfig<StockOrderSn>
    {
        /// <summary>
        /// 只读视图
        /// </summary>
        public const string ReadonlyView = "ReadonlyView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { ReadonlyView });
            View.AssignAuthorize(typeof(StockOrder));
            if (ViewGroup == ReadonlyView)
            {
                ConfigReadOnlyView();
            }
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo);
                View.Property(p => p.Sn).ShowInList(150);
                View.Property(p => p.ItemCode).ShowInList(120);
                View.Property(p => p.ItemName).ShowInList(120);
                View.Property(p => p.ItemExtPropName).ShowInList(180);
                View.Property(p => p.LotNo);
                View.Property(p => p.State);
                View.Property(p => p.Qty);
                View.Property(p => p.ShipQty);
                View.Property(p => p.SoNo);
                View.Property(p => p.SoLineNo);
                View.Property(p => p.WarehouseCode);
                View.Property(p => p.DistributionNo);
                View.Property(p => p.PackageNo).ShowInList(120).HasLabel("最上级标签");
                View.Property(p => p.ReceiveById);
                View.Property(p => p.ReceiveTime);                             
            }
        }

        /// <summary>
        /// 只读视图配置
        /// </summary>
        protected void ConfigReadOnlyView()
        {
            View.ClearCommands();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).ShowInList();
                View.Property(p => p.Sn).ShowInList(150);
                View.Property(p => p.ItemCode).ShowInList(120);
                View.Property(p => p.ItemName).ShowInList(120);
                View.Property(p => p.ItemExtPropName).ShowInList(180);
                View.Property(p => p.LotNo).ShowInList();
                View.Property(p => p.State).ShowInList();
                View.Property(p => p.Qty).ShowInList();
                View.Property(p => p.ShipQty).ShowInList();
                View.Property(p => p.SoNo).ShowInList();
                View.Property(p => p.SoLineNo).ShowInList();
                View.Property(p => p.WarehouseCode).ShowInList();
                View.Property(p => p.DistributionNo).ShowInList();
                View.Property(p => p.PackageNo).ShowInList(120).HasLabel("最上级标签");
                View.Property(p => p.ReceiveById).ShowInList();
                View.Property(p => p.ReceiveTime).ShowInList();   
            }
        }
    }
}