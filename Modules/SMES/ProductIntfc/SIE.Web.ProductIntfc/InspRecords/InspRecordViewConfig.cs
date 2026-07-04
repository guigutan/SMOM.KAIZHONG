using SIE.Domain;
using SIE.MetaModel.View;
using SIE.ProductIntfc.InspLogs;
using SIE.ProductIntfc.InspRecords;
using SIE.ProductIntfc.InspSettings;
using SIE.Web.ProductIntfc.InspLogs;

namespace SIE.Web.ProductIntfc.InspRecords
{
    /// <summary>
    /// 报检记录视图配置
    /// </summary>
    internal class InspRecordViewConfig : WebViewConfig<InspRecord>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls);
            View.Property(p => p.WorkOrderNo).Readonly().HasLabel("工单号");
            View.Property(p => p.WorkOrderProductCode).Readonly().HasLabel("产品编码");
            View.Property(p => p.WorkOrderProductName).Readonly().HasLabel("产品名称");
            View.Property(p => p.WorkOrderType).Readonly().HasLabel("工单类型").UseEnumEditor();
            View.Property(p => p.WorkOrderPlanQty).Readonly().HasLabel("计划数量");
            View.Property(p => p.ShopName).Readonly();
            View.Property(p => p.ResourceName).Readonly();
            View.Property(p => p.WorkOrderState).Readonly().HasLabel("工单状态").UseEnumEditor();
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
            }).HasLabel("待报检");
            View.AttachChildrenProperty(typeof(InspLog), (c) =>
            {
                var child = c as ChildPagingDataArgs;
                var entity = child.Parent as InspRecord;
                var inspRecord = RF.GetById<InspRecord>(entity.Id);
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
