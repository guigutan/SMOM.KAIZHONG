using SIE.Barcodes;
using SIE.MES.PanelBindings;

namespace SIE.Web.MES.PanelBindings
{
    /// <summary>
    /// MES工单条码绑定记录-界面
    /// </summary>
    internal class PanelBindingRecordViewConfig : WebViewConfig<PanelBindingRecord>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.No).ShowInList(width: 150);
            View.Property(p => p.ProductCode).ShowInList(width: 150);
            View.Property(p => p.ProductName);
            View.Property(p => p.PlanQty);
            View.Property(p => p.PanelQty);
            View.Property(p => p.BindQty);
            View.Property(p => p.WorkShopName);
            View.Property(p => p.ResourceName);
            View.Property(p => p.State);
            View.Property(p => p.PlanBeginDate).ShowInList(width: 150);

            View.AttachChildrenProperty(typeof(PanelAndBarcode), w =>
            {
                var args = w as ChildPagingDataArgs;
                var record = args.Parent as PanelBindingRecord;
                return RT.Service.Resolve<PanelBindingController>().GetBindingRecords(record.Id, args.PagingInfo, args.SortInfo);
            }, PanelAndBarcodeViewConfig.BindingView).HasLabel("绑定明细");

            View.AttachChildrenProperty(typeof(Barcode), w =>
            {
                var args = w as ChildPagingDataArgs;
                var record = args.Parent as PanelBindingRecord;
                return RT.Service.Resolve<PanelBindingController>().GetUnBindingRecords(record.Id, args.PagingInfo, args.SortInfo);
            }, BarcodeViewConfig.NoBindingView).HasLabel("未绑定明细");
        }
    }
}
