using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 入库明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("入库明细")]
    public partial class InStorageBarcodeDetail : DataEntity
    {
        #region 入库条码 Barcode
        /// <summary>
        /// 入库条码
        /// </summary>
        [Label("入库条码")]
        public static readonly Property<string> BarcodeProperty = P<InStorageBarcodeDetail>.Register(e => e.Barcode);

        /// <summary>
        /// 入库条码
        /// </summary>
        public string Barcode
        {
            get { return GetProperty(BarcodeProperty); }
            set { SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 产品数量 Qty
        /// <summary>
        /// 产品数量
        /// </summary>
        [Label("产品数量")]
        public static readonly Property<decimal> QtyProperty = P<InStorageBarcodeDetail>.Register(e => e.Qty);

        /// <summary>
        /// 产品数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 入库层级 Level
        /// <summary>
        /// 入库层级
        /// </summary>
        [Label("入库层级")]
        public static readonly Property<string> LevelProperty = P<InStorageBarcodeDetail>.Register(e => e.Level);

        /// <summary>
        /// 入库层级
        /// </summary>
        public string Level
        {
            get { return GetProperty(LevelProperty); }
            set { SetProperty(LevelProperty, value); }
        }
        #endregion

        #region 成品入库单 InStorageBill
        /// <summary>
        /// 成品入库单Id
        /// </summary>
        public static readonly IRefIdProperty InStorageBillIdProperty = P<InStorageBarcodeDetail>.RegisterRefId(e => e.InStorageBillId, ReferenceType.Parent);

        /// <summary>
        /// 成品入库单Id
        /// </summary>
        public double InStorageBillId
        {
            get { return (double)GetRefId(InStorageBillIdProperty); }
            set { SetRefId(InStorageBillIdProperty, value); }
        }

        /// <summary>
        /// 成品入库单
        /// </summary>
        public static readonly RefEntityProperty<InStorageBill> InStorageBillProperty = P<InStorageBarcodeDetail>.RegisterRef(e => e.InStorageBill, InStorageBillIdProperty);

        /// <summary>
        /// 成品入库单
        /// </summary>
        public InStorageBill InStorageBill
        {
            get { return GetRefEntity(InStorageBillProperty); }
            set { SetRefEntity(InStorageBillProperty, value); }
        }
        #endregion

        #region 生产批次 BatchBarcode
        /// <summary>
        /// 生产批次
        /// </summary>
        [Label("生产批次")]
        public static readonly Property<string> BatchBarcodeProperty = P<InStorageBarcodeDetail>.Register(e => e.BatchBarcode);

        /// <summary>
        /// 生产批次
        /// </summary>
        public string BatchBarcode
        {
            get { return GetProperty(BatchBarcodeProperty); }
            set { SetProperty(BatchBarcodeProperty, value); }
        }
        #endregion

        #region 入库状态 State
        /// <summary>
        /// 入库状态
        /// </summary>
        [Label("入库状态")]
        public static readonly Property<InStorageState?> StateProperty = P<InStorageBarcodeDetail>.Register(e => e.State);

        /// <summary>
        /// 入库状态
        /// </summary>
        public InStorageState? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion


    }

    /// <summary>
    /// 入库明细 实体配置
    /// </summary>
    internal class InStorageBarcodeDetailConfig : EntityConfig<InStorageBarcodeDetail>
    {
        /// <summary>
        /// 数据表Config
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("INF_IN_STO_BAR_DTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}