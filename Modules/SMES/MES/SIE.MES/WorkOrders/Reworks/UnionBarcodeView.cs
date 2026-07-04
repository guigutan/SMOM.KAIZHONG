using SIE.Common;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 来源关联条码视图
    /// </summary>
    [RootEntity, Serializable]
    [Label("来源关联条码")]
    public partial class UnionBarcodeView : UnionBarcodeCore
    {
    }

    /// <summary>
    /// 来源关联条码 实体配置
    /// </summary>
    internal class UnionBarcodeViewConfig : EntityConfig<UnionBarcodeView>
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

            //var productMeta = RF.Find<Item>().EntityMeta;
            //string productMeta_no = productMeta.Property(Item.CodeProperty).ColumnMeta.ColumnName;
            //string productMeta_id = productMeta.Property(Item.IdProperty).ColumnMeta.ColumnName;

            //var barcodeMeta = RF.Find<Barcode>().EntityMeta;
            //string barcodeMeta_sn = barcodeMeta.Property(Barcode.SnProperty).ColumnMeta.ColumnName;
            //string barcodeMeta_workOrderId = barcodeMeta.Property(Barcode.WorkOrderIdProperty).ColumnMeta.ColumnName;
            //string barcodeMeta_id = barcodeMeta.Property(Barcode.IdProperty).ColumnMeta.ColumnName;
            /*
            Func<IQuery> view = () => DB.Query<WorkOrder>("v1")
                                    .LeftJoin<Item>("v2",(a,b)=>a.Product.Id==b.Id&& b.SQL<int>("v2.INV_ORG_ID") == RT.InvOrg)
                                    .LeftJoin<Barcode>("v3",(x,y)=>x.Id==y.WorkOrder.Id && y.SQL<int>("v3.INV_ORG_ID") == RT.InvOrg)
                                    .LeftJoin<Barcode,InspBarcodeLog>("v4", (x, y) => x.Sn == y.Barcode && y.SQL<int>("v4.INV_ORG_ID") == RT.InvOrg)
                                    .LeftJoin<InspBarcodeLog,InspLog>("v5", (x, y) => x.InspLogId == y.Id && y.SQL<int>("v5.INV_ORG_ID") == RT.InvOrg)
                                    .Select<WorkOrder, Item, Barcode, InspBarcodeLog, InspLog >((t1, t2, t3, t4, t5) =>
                                    new
                                    {
                                        WorkOrderNo = t1.No,
                                        ItemNo = t2.Code,
                                        Barcode = t3.Sn,
                                        InspetNo = t5.InspNo,
                                        Result = t5.InspectionResult
                                    }).Where(p => p.SQL<int>("v3.INV_ORG_ID") == RT.InvOrg)
                                    .Where(p => p.SQL<string>("v3.IS_PHANTOM") == "0")
                                    .Where(p => p.SQL<string>("v3.is_scraped") == "0").ToQuery();
            */
            const string view = "(select c.id, a.no Work_Order_No,b.code Item_No,c.sn Barcode,'' Inspet_No,'' Result"
                           + "  from wo a  "
                           + "   left join ITEM b on a.product_id = b.id "
                           + "   left join BC_BARCODE c on a.id = c.work_order_id)";

            Meta.MapView(view).MapAllProperties();
        }
    }

    /// <summary>
    /// 来源关联条码基类
    /// </summary>
    [RootEntity, Serializable]
    [Label("来源关联条码")]
    [ConditionQueryType(typeof(UnionBarcodeViewCriteria))]
    public partial class UnionBarcodeCore : Entity<double>
    {
        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<UnionBarcodeCore>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 产品编码 ItemNo
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ItemNoProperty = P<UnionBarcodeCore>.Register(e => e.ItemNo);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ItemNo
        {
            get { return this.GetProperty(ItemNoProperty); }
            set { this.SetProperty(ItemNoProperty, value); }
        }
        #endregion

        #region 条码 Barcode
        /// <summary>
        /// 条码
        /// </summary>
        [Label("条码")]
        public static readonly Property<string> BarcodeProperty = P<UnionBarcodeCore>.Register(e => e.Barcode);

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 成品检验单号 InspetNo
        /// <summary>
        /// 成品检验单号
        /// </summary>
        [Label("成品检验单号")]
        public static readonly Property<string> InspetNoProperty = P<UnionBarcodeCore>.Register(e => e.InspetNo);

        /// <summary>
        /// 成品检验单号
        /// </summary>
        public string InspetNo
        {
            get { return this.GetProperty(InspetNoProperty); }
            set { this.SetProperty(InspetNoProperty, value); }
        }
        #endregion

        #region 检验结果 Result
        /// <summary>
        /// 检验结果
        /// </summary>
        [Label("检验结果")]
        public static readonly Property<InspectionResult?> ResultProperty = P<UnionBarcodeCore>.Register(e => e.Result);

        /// <summary>
        /// 检验结果
        /// </summary>
        public InspectionResult? Result
        {
            get { return this.GetProperty(ResultProperty); }
            set { this.SetProperty(ResultProperty, value); }
        }
        #endregion

        #region 库存InvOrgId InvOrgId
        /// <summary>
        /// 库存InvOrgId
        /// </summary>
        [Label("库存InvOrgId")]
        public static readonly Property<int?> InvOrgIdProperty = P<UnionBarcodeCore>.Register(e => e.InvOrgId);

        /// <summary>
        /// 库存InvOrgId
        /// </summary>
        public int? InvOrgId
        {
            get { return this.GetProperty(InvOrgIdProperty); }
            set { this.SetProperty(InvOrgIdProperty, value); }
        }
        #endregion

    }
}