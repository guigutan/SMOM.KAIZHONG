using SIE.ProductIntfc.InspLogs;
using SIE.ProductIntfc.InspRecords;
using SIE.ProductIntfc.InspSettings;
using SIE.ProductIntfc.ProductInsps;
using SIE.Wpf.ProductIntfc.InspLogs;
using SIE.Wpf.ProductIntfc.InspRecords;

namespace SIE.Wpf.ProductIntfc.ProductInsps
{
    /// <summary>
    /// 成品报检记录视图配置
    /// </summary>
    internal class ProductInspViewConfig : WPFViewConfig<ProductInsp>
    {
        /// <summary>
        /// 成品报检
        /// </summary>
        public static string ProductInspViewGroup { get; } = "ProductInspView";

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ProductInspViewGroup);
            if (ViewGroup == ProductInspViewGroup)
                ConfigProductInspView();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected void ConfigProductInspView()
        {
            View.UseCommands(WPFCommandNames.Export);
            View.Property(p => p.WorkOrderNo).HasLabel("工单号");
            View.Property(p => p.WorkOrderProductCode).HasLabel("产品编码");
            View.Property(p => p.WorkOrderProductName).HasLabel("产品名称");
            View.Property(p => p.WorkOrderType).HasLabel("工单类型").UseEnumEditor();
            View.Property(p => p.WorkOrderPlanQty).HasLabel("计划数量");
            View.Property(p => p.ShopName);
            View.Property(p => p.ResourceName);
            View.Property(p => p.WorkOrderState).HasLabel("工单状态").UseEnumEditor();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.InspBarcodeList).HasLabel("待报检").Show(ChildShowInWhere.Hide);
            View.AttachChildrenProperty(typeof(InspBarcode), (c) =>
            {
                var child = c as ChildPagingDataArgs;
                var inspRecord = child.Parent as InspRecord;
                return RT.Service.Resolve<InspRecordController>().GetInspBarcodes(inspRecord.Id, child.PagingInfo, child.SortInfo);
            }, InspBarcodeViewConfig.ProductInspViewGroup).HasLabel("待报检");
            View.AttachChildrenProperty(typeof(InspLog), (c) =>
            {
                var child = c as ChildPagingDataArgs;
                var inspRecord = child.Parent as InspRecord;
                return RT.Service.Resolve<InspRecordController>().GetInspLogs(inspRecord.WorkOrderId, inspRecord.ShopId, inspRecord.ResourceId, InspType.Product, child.PagingInfo, child.SortInfo);
            }, InspLogViewConfig.ShippingInspLogListView).HasLabel("已报检");
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            //方法重写
        }
    }
}