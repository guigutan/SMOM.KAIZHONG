using SIE.MES.WorkOrders.Reworks;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ProductIntfc.InspLogs.Reworks
{
    /// <summary>
    /// 来源关联条码VeiwModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("来源关联条码")]
    public partial class InspUnionBarcodeView : UnionBarcodeCore
    {
    }

    /// <summary>
    /// 来源关联条码 实体配置
    /// </summary>
    internal class InspUnionBarcodeViewConfig : EntityConfig<InspUnionBarcodeView>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            //var workOrderMeta = RF.Find<WorkOrder>().EntityMeta;
            //string workOrderMeta_no = workOrderMeta.Property(WorkOrder.NoProperty).ColumnMeta.ColumnName;
            //string workOrderMeta_productid = workOrderMeta.Property(WorkOrder.ProductIdProperty).ColumnMeta.ColumnName;
            //string workOrderMeta_id = workOrderMeta.Property(WorkOrder.IdProperty).ColumnMeta.ColumnName;
            //string invOrgId = workOrderMeta.Property(InvOrgIdExtension.INV_ORG_IDProperty).ColumnMeta.ColumnName;

            //var productMeta = RF.Find<Item>().EntityMeta;
            //string productMeta_no = productMeta.Property(Item.CodeProperty).ColumnMeta.ColumnName;
            //string productMeta_id = productMeta.Property(Item.IdProperty).ColumnMeta.ColumnName;

            //var barcodeMeta = RF.Find<Barcode>().EntityMeta;
            //string barcodeMeta_sn = barcodeMeta.Property(Barcode.SnProperty).ColumnMeta.ColumnName;
            //string barcodeMeta_workOrderId = barcodeMeta.Property(Barcode.WorkOrderIdProperty).ColumnMeta.ColumnName;
            //string barcodeMeta_id = barcodeMeta.Property(Barcode.IdProperty).ColumnMeta.ColumnName;

            //var InspDetailMeta = RF.Find<InspBarcodeLog>().EntityMeta;
            //string InspDetailMeta_barcode = InspDetailMeta.Property(InspBarcodeLog.BarcodeProperty).ColumnMeta.ColumnName;
            //string InspDetailMeta_inspId = InspDetailMeta.Property(InspBarcodeLog.InspLogIdProperty).ColumnMeta.ColumnName;

            //var InspMeta = RF.Find<InspLog>().EntityMeta;
            //string InspMeta_id = InspMeta.Property(InspLog.IdProperty).ColumnMeta.ColumnName;
            //string InspMeta_checkNo = InspMeta.Property(InspLog.CheckNoProperty).ColumnMeta.ColumnName;
            //string InspMeta_result = InspMeta.Property(InspLog.InspectionResultProperty).ColumnMeta.ColumnName;
            const string view = "(select c.id, a.no Work_Order_No,b.code Item_No,c.sn Barcode,e.check_no Inspet_No,e.inspection_result Result,a.INV_ORG_ID Inv_Org_Id"
                          + "  from wo a  "
                          + "   left join ITEM b on a.product_id = b.id "
                          + "   left join BC_BARCODE c on a.id = c.work_order_id "
                          + "   left join INF_INSP_BARCODE_LOG d on d.barcode = c.sn "
                          + "   left join INF_INSP_LOG e on e.id = d.insp_log_id "
                          + "   where c.is_scraped = 0)";

            Meta.MapView(view).MapAllProperties();
        }
    }
}