using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;

namespace SIE.Traces.Common
{
    /// <summary>
    /// 包装信息追溯
    /// </summary>
    [RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("包装信息追溯")]
	public partial class PackageTraceViewModel : ViewModel
    {
		#region 包装号 PackageNo
		/// <summary>
		/// 包装号
		/// </summary>
		[Label("包装号")]
		public static readonly Property<string> PackageNoProperty = P<PackageTraceViewModel>.Register(e => e.PackageNo);

		/// <summary>
		/// 包装号
		/// </summary>
		public string PackageNo
		{
			get { return GetProperty(PackageNoProperty); }
			set { SetProperty(PackageNoProperty, value); }
		}
        #endregion

        #region 包装单位 PackageUnitName
        /// <summary>
        /// 包装单位
        /// </summary>
        [Label("包装单位")]
		public static readonly Property<string> PackageUnitNameProperty = P<PackageTraceViewModel>.Register(e => e.PackageUnitName);

        /// <summary>
        /// 包装单位
        /// </summary>
        public string PackageUnitName
        {
			get { return GetProperty(PackageUnitNameProperty); }
			set { SetProperty(PackageUnitNameProperty, value); }
		}
		#endregion

		#region 数量 Qty
		/// <summary>
		/// 数量
		/// </summary>
		[Label("数量")]
		public static readonly Property<decimal?> QtyProperty = P<PackageTraceViewModel>.Register(e => e.Qty);

		/// <summary>
		/// 数量
		/// </summary>
		public decimal? Qty
		{
			get { return GetProperty(QtyProperty); }
			set { SetProperty(QtyProperty, value); }
		}
		#endregion

		#region 包装时间 PackageTime
		/// <summary>
		/// 包装时间
		/// </summary>
		[Label("包装时间")]
		public static readonly Property<DateTime?> PackageTimeProperty = P<PackageTraceViewModel>.Register(e => e.PackageTime);

		/// <summary>
		/// 包装时间
		/// </summary>
		public DateTime? PackageTime
		{
			get { return GetProperty(PackageTimeProperty); }
			set { SetProperty(PackageTimeProperty, value); }
		}
        #endregion

        #region 仓库 WarehouseName
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
		public static readonly Property<string> WarehouseNameProperty = P<PackageTraceViewModel>.Register(e => e.WarehouseName);

		/// <summary>
		/// 仓库
		/// </summary>
		public string WarehouseName
		{
			get { return GetProperty(WarehouseNameProperty); }
			set { SetProperty(WarehouseNameProperty, value); }
		}
		#endregion

		#region 库位 StationName
		/// <summary>
		/// 库位
		/// </summary>
		[Label("库位")]
		public static readonly Property<string> StationNameProperty = P<PackageTraceViewModel>.Register(e => e.StationName);

		/// <summary>
		/// 库位
		/// </summary>
		public string StationName
        {
			get { return GetProperty(StationNameProperty); }
			set { SetProperty(StationNameProperty, value); }
		}
        #endregion

        #region 发运单 ShippingOrderNo
        /// <summary>
        /// 发运单
        /// </summary>
        [Label("发运单")]
        public static readonly Property<string> ShippingOrderNoProperty = P<PackageTraceViewModel>.Register(e => e.ShippingOrderNo);

        /// <summary>
        /// 发运单
        /// </summary>
        public string ShippingOrderNo
        {
            get { return GetProperty(ShippingOrderNoProperty); }
            set { SetProperty(ShippingOrderNoProperty, value); }
        }
        #endregion

        #region 客户 CustomerName
        /// <summary>
        /// 客户
        /// </summary>
        [Label("客户")]
        public static readonly Property<string> CustomerNameProperty = P<PackageTraceViewModel>.Register(e => e.CustomerName);

        /// <summary>
        /// 客户
        /// </summary>
        public string CustomerName
        {
            get { return GetProperty(CustomerNameProperty); }
            set { SetProperty(CustomerNameProperty, value); }
        }
        #endregion

        #region 交货日期 DeliveryDate
        /// <summary>
        /// 交货日期
        /// </summary>
        [Label("交货日期")]
        public static readonly Property<string> DeliveryDateProperty = P<PackageTraceViewModel>.Register(e => e.DeliveryDate);

        /// <summary>
        /// 交货日期
        /// </summary>
        public string DeliveryDate
        {
            get { return GetProperty(DeliveryDateProperty); }
            set { SetProperty(DeliveryDateProperty, value); }
        }
        #endregion

        #region 入库条码 StorageBarcode
        /// <summary>
        /// 入库条码
        /// </summary>
        [Label("入库条码")]
        public static readonly Property<string> StorageBarcodeProperty = P<PackageTraceViewModel>.Register(e => e.StorageBarcode);

        /// <summary>
        /// 入库条码
        /// </summary>
        public string StorageBarcode
        {
            get { return GetProperty(StorageBarcodeProperty); }
            set { SetProperty(StorageBarcodeProperty, value); }
        }
        #endregion

    }

    /// <summary>
    ///  配置
    /// </summary>
    public class PackageTraceViewModelConfig : EntityConfig<PackageTraceViewModel>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.SupportTree();
        }
    }
}