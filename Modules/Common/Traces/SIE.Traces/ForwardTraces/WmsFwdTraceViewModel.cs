using SIE.Domain;
using SIE.EventMessages.Common.Traces;
using SIE.ObjectModel;

namespace SIE.Traces.ForwardTraces
{
    /// <summary>
    /// 库存追溯
    /// </summary>
    [RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("库存信息追溯")]
	public partial class WmsFwdTraceViewModel : ViewModel
    {
        #region 追溯类型 TraceType
        /// <summary>
        /// 追溯类型
        /// </summary>
        [Label("追溯类型")]
        public static readonly Property<TraceType> TraceTypeProperty = P<WmsFwdTraceViewModel>.Register(e => e.TraceType);

        /// <summary>
        /// 追溯类型
        /// </summary>
        public TraceType TraceType
        {
            get { return GetProperty(TraceTypeProperty); }
            set { SetProperty(TraceTypeProperty, value); }
        }
        #endregion

        #region 库存Id LotLpnOnhandId
        /// <summary>
        /// 库存Id
        /// </summary>
        [Label("库存Id")]
        public static readonly Property<double?> LotLpnOnhandIdProperty = P<WmsFwdTraceViewModel>.Register(e => e.LotLpnOnhandId);

        /// <summary>
        /// 库存Id
        /// </summary>
        public double? LotLpnOnhandId
        {
            get { return GetProperty(LotLpnOnhandIdProperty); }
            set { SetProperty(LotLpnOnhandIdProperty, value); }
        }
        #endregion

        #region Sn Sn
        /// <summary>
        /// Sn
        /// </summary>
        [Label("Sn")]
        public static readonly Property<string> SnProperty = P<WmsFwdTraceViewModel>.Register(e => e.Sn);

        /// <summary>
        /// Sn
        /// </summary>
        public string Sn
        {
            get { return GetProperty(SnProperty); }
            set { SetProperty(SnProperty, value); }
        }
        #endregion

        #region 物料Id ItemId
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料Id")]
        public static readonly Property<double> ItemIdProperty = P<WmsFwdTraceViewModel>.Register(e => e.ItemId);
        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return GetProperty(ItemIdProperty); }
            set { SetProperty(ItemIdProperty, value); }
        }
        #endregion

        #region 物料批次 ItemLot
        /// <summary>
        /// 物料批次
        /// </summary>
        [Label("物料批次")]
        public static readonly Property<string> ItemLotProperty = P<WmsFwdTraceViewModel>.Register(e => e.ItemLot);
        /// <summary>
        /// 物料批次
        /// </summary>
        public string ItemLot
        {
            get { return GetProperty(ItemLotProperty); }
            set { SetProperty(ItemLotProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtPropName
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropNameProperty = P<WmsFwdTraceViewModel>.Register(e => e.ItemExtPropName);
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get { return GetProperty(ItemExtPropNameProperty); }
            set { SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion


        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
		public static readonly Property<string> WarehouseCodeProperty = P<WmsFwdTraceViewModel>.Register(e => e.WarehouseCode);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
			get { return GetProperty(WarehouseCodeProperty); }
			set { SetProperty(WarehouseCodeProperty, value); }
		}
        #endregion

        #region 库位编码 StorageLocationCode
        /// <summary>
        /// 库位编码
        /// </summary>
        [Label("库位编码")]
		public static readonly Property<string> StorageLocationCodeProperty = P<WmsFwdTraceViewModel>.Register(e => e.StorageLocationCode);

        /// <summary>
        /// 库位编码
        /// </summary>
        public string StorageLocationCode
        {
			get { return GetProperty(StorageLocationCodeProperty); }
			set { SetProperty(StorageLocationCodeProperty, value); }
		}
        #endregion

        #region 库存状态 OnhandState
        /// <summary>
        /// 库存状态
        /// </summary>
        [Label("库存状态")]
        public static readonly Property<string> OnhandStateProperty = P<WmsFwdTraceViewModel>.Register(e => e.OnhandState);

        /// <summary>
        /// 库存状态
        /// </summary>
        public string OnhandState
        {
            get { return GetProperty(OnhandStateProperty); }
            set { SetProperty(OnhandStateProperty, value); }
        }
        #endregion

        #region 现有量 Qty
        /// <summary>
        /// 现有量
        /// </summary>
        [Label("现有量")]
		public static readonly Property<decimal?> QtyProperty = P<WmsFwdTraceViewModel>.Register(e => e.Qty);

		/// <summary>
		/// 现有量
		/// </summary>
		public decimal? Qty
		{
			get { return GetProperty(QtyProperty); }
			set { SetProperty(QtyProperty, value); }
		}
        #endregion

        #region Asn单号 AsnNo
        /// <summary>
        /// Asn单号
        /// </summary>
        [Label("Asn单号")]
        public static readonly Property<string> AsnNoProperty = P<WmsFwdTraceViewModel>.Register(e => e.AsnNo);

        /// <summary>
        /// Asn单号
        /// </summary>
        public string AsnNo
        {
            get { return GetProperty(AsnNoProperty); }
            set { SetProperty(AsnNoProperty, value); }
        }
        #endregion

        #region 供应商 SupplierName
        /// <summary>
        /// 供应商
        /// </summary>
        [Label("供应商")]
        public static readonly Property<string> SupplierNameProperty = P<WmsFwdTraceViewModel>.Register(e => e.SupplierName);

        /// <summary>
        /// 供应商
        /// </summary>
        public string SupplierName
        {
            get { return GetProperty(SupplierNameProperty); }
            set { SetProperty(SupplierNameProperty, value); }
        }
        #endregion

        #region 生产批次 ProductBatch
        /// <summary>
        /// 供应商
        /// </summary>
        [Label("供应商")]
        public static readonly Property<string> ProductBatchProperty = P<WmsFwdTraceViewModel>.Register(e => e.ProductBatch);

        /// <summary>
        /// 供应商
        /// </summary>
        public string ProductBatch
        {
            get { return GetProperty(ProductBatchProperty); }
            set { SetProperty(ProductBatchProperty, value); }
        }
        #endregion

        #region 生产日期 ProductionDate
        /// <summary>
        /// 生产日期
        /// </summary>
        [Label("生产日期")]
        public static readonly Property<DateTime?> ProductionDateProperty = P<WmsFwdTraceViewModel>.Register(e => e.ProductionDate);

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProductionDate
        {
            get { return GetProperty(ProductionDateProperty); }
            set { SetProperty(ProductionDateProperty, value); }
        }
        #endregion

        #region 收货日期 CollectDate
        /// <summary>
        /// 收货日期
        /// </summary>
        [Label("收货日期")]
        public static readonly Property<DateTime?> CollectDateProperty = P<WmsFwdTraceViewModel>.Register(e => e.CollectDate);

        /// <summary>
        /// 收货日期
        /// </summary>
        public DateTime? CollectDate
        {
            get { return GetProperty(CollectDateProperty); }
            set { SetProperty(CollectDateProperty, value); }
        }
        #endregion
	}
}