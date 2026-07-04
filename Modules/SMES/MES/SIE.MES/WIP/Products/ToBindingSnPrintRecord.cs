using SIE.Barcodes;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 已绑定拼板码的产品条码打印记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("已绑定拼板码的产品条码打印记录")]
    public class ToBindingSnPrintRecord : DataEntity
    {
        #region 拼板码 PanelCode
        /// <summary>
        /// 拼板码
        /// </summary>
        [Label("拼板码")]
        public static readonly Property<string> PanelCodeProperty = P<ToBindingSnPrintRecord>.Register(e => e.PanelCode);

        /// <summary>
        /// 拼板码
        /// </summary>
        public string PanelCode
        {
            get { return this.GetProperty(PanelCodeProperty); }
            set { this.SetProperty(PanelCodeProperty, value); }
        }
        #endregion 

        #region 产品条码 Barcode
        /// <summary>
        /// 产品条码Id
        /// </summary>
        [Label("产品条码")]
        public static readonly IRefIdProperty BarcodeIdProperty =
            P<ToBindingSnPrintRecord>.RegisterRefId(e => e.BarcodeId, ReferenceType.Normal);

        /// <summary>
        /// 产品条码Id
        /// </summary>
        public double BarcodeId
        {
            get { return (double)this.GetRefId(BarcodeIdProperty); }
            set { this.SetRefId(BarcodeIdProperty, value); }
        }

        /// <summary>
        /// 产品条码
        /// </summary>
        public static readonly RefEntityProperty<Barcode> BarcodeProperty =
            P<ToBindingSnPrintRecord>.RegisterRef(e => e.Barcode, BarcodeIdProperty);

        /// <summary>
        /// 产品条码
        /// </summary>
        public Barcode Barcode
        {
            get { return this.GetRefEntity(BarcodeProperty); }
            set { this.SetRefEntity(BarcodeProperty, value); }
        }
        #endregion 

        #region 工单号 WorkOrderId
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单")]
        public static readonly Property<double> WorkOrderIdProperty = P<ToBindingSnPrintRecord>.Register(e => e.WorkOrderId);

        /// <summary>
        /// 工单号
        /// </summary>
        public double WorkOrderId
        {
            get { return this.GetProperty(WorkOrderIdProperty); }
            set { this.SetProperty(WorkOrderIdProperty, value); }
        }
        #endregion 
    }

    /// <summary>
    /// 已绑定拼板码的产品条码打印记录 实体配置
    /// </summary>
    class ToBindingSnPrintRecordConfig : EntityConfig<ToBindingSnPrintRecord>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_SN_PRINT_RECORD").MapAllProperties();
            Meta.DisablePhantoms();
        }
    }
}