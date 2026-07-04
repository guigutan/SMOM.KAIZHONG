using SIE.Barcodes;
using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 条码
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(WipProductBarcodeCriteria))]
    [Label("条码")]
    [DisplayMember(nameof(Sn))]
    public partial class WipProductBarcode : Core.Barcodes.Barcode
    {
        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty
            = P<WipProductBarcode>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public override double? WorkOrderId
        {
            get { return (double?)GetRefNullableId(WorkOrderIdProperty); }
            set { SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty
            = P<WipProductBarcode>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 工单号 WONo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WONoProperty
            = P<WipProductBarcode>.RegisterView(e => e.WONo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WONo
        {
            get { return this.GetProperty(WONoProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 条码 实体配置
    /// </summary>
    internal class WipProductBarcodeConfig : EntityConfig<WipProductBarcode>
    {
        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BC_BARCODE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
