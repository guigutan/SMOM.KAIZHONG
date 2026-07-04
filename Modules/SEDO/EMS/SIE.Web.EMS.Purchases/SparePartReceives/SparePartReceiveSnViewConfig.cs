using SIE.EMS.Purchases.SparePartReceives;
using SIE.Web.EMS.Purchases.SparePartReceives.Commands;

namespace SIE.Web.EMS.Purchases.SparePartReceives
{
    /// <summary>
    /// 序列号明细视图配置
    /// </summary>
    public class SparePartReceiveSnViewConfig : WebViewConfig<SparePartReceiveSn>
    {
        /// <summary>
        /// 扫描视图
        /// </summary>
        public const string ScanView = "ScanView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ScanView);
            if (ViewGroup == ScanView)
            {
                ConfigScanView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.WithoutPaging();
            View.UseGridSelectionModel();
            View.UseCommands("SIE.Web.EMS.Purchases.SparePartReceives.Commands.LotScreenCommand", "SIE.Web.EMS.Purchases.SparePartReceives.Commands.SnPrintCommand");
            View.Property(p => p.LineNo).ShowInList(50);
            View.Property(p => p.PurchaseOrderNo).ShowInList(130);
            View.Property(p => p.PurchaseOrderItemLineNo);
            View.Property(p => p.SupplierCode);
            View.Property(p => p.SupplierName).ShowInList(150);
            View.Property(p => p.PurchaseObjectType);
            View.Property(p => p.SparePartCode);
            View.Property(p => p.SparePartName).ShowInList(150);
            View.Property(p => p.ControlMethod);
            View.Property(p => p.Sn).ShowInList(150);
            View.Property(p => p.OriginalSn).ShowInList(150);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 扫描视图
        /// </summary>
        protected void ConfigScanView()
        {
            View.DisableEditing();
            View.WithoutPaging();
            View.UseGridSelectionModel();
            View.AssignAuthorize(typeof(SparePartReceive));
            View.UseCommands(typeof(DeleteReceiveSnCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).ShowInList(50);
                View.Property(p => p.PurchaseOrderNo).ShowInList(130);
                View.Property(p => p.PurchaseOrderItemLineNo).ShowInList();
                View.Property(p => p.SupplierCode).ShowInList();
                View.Property(p => p.SupplierName).ShowInList(150);
                View.Property(p => p.SparePartCode).ShowInList();
                View.Property(p => p.SparePartName).ShowInList(150);
                View.Property(p => p.ControlMethod).ShowInList();
                View.Property(p => p.Sn).ShowInList(150);
                View.Property(p => p.OriginalSn).ShowInList(150);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}