using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;

namespace SIE.Traces.ReverseTraces
{
    /// <summary>
    /// 正向追溯
    /// </summary>
    [RootEntity, Serializable]
	[ConditionQueryType(typeof(ReverseTraceViewModelCriteria))]
	public partial class ReverseTraceViewModel : ViewModel
	{
        #region 产品版本Id VersionId
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品Id")]
        public static readonly Property<double> VersionIdProperty = P<ReverseTraceViewModel>.Register(e => e.VersionId);
        /// <summary>
        /// 产品Id
        /// </summary>
        public double VersionId
        {
            get { return GetProperty(VersionIdProperty); }
            set { SetProperty(VersionIdProperty, value); }
        }
        #endregion

        #region 产品Id ProductId
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品Id")]
        public static readonly Property<double> ProductIdProperty = P<ReverseTraceViewModel>.Register(e => e.ProductId);
        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return GetProperty(ProductIdProperty); }
            set { SetProperty(ProductIdProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
		public static readonly Property<string> ProductCodeProperty = P<ReverseTraceViewModel>.Register(e => e.ProductCode);
		/// <summary>
		/// 产品编码
		/// </summary>
		public string ProductCode
		{
			get { return GetProperty(ProductCodeProperty); }
			set { SetProperty(ProductCodeProperty, value); }
		}
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<ReverseTraceViewModel>.Register(e => e.ProductName);
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
		{
			get { return GetProperty(ProductNameProperty); }
			set { SetProperty(ProductNameProperty, value); }
		}
        #endregion

        #region 生产批次 ProductionLot
        /// <summary>
        /// 生产批次
        /// </summary>
        [Label("生产批次")]
        public static readonly Property<string> ProductionLotProperty = P<ReverseTraceViewModel>.Register(e => e.ProductionLot);
        /// <summary>
        /// 生产批次
        /// </summary>
        public string ProductionLot
        {
			get { return GetProperty(ProductionLotProperty); }
			set { SetProperty(ProductionLotProperty, value); }
		}
        #endregion

        #region 产品扩展属性 ProductExtPropName
        /// <summary>
        /// 产品扩展属性
        /// </summary>
        [Label("产品扩展属性")]
        public static readonly Property<string> ProductExtPropNameProperty = P<ReverseTraceViewModel>.Register(e => e.ProductExtPropName);
        /// <summary>
        /// 产品扩展属性
        /// </summary>
        public string ProductExtPropName
        {
            get { return GetProperty(ProductExtPropNameProperty); }
            set { SetProperty(ProductExtPropNameProperty, value); }
        }
        #endregion

        #region Sn ProductSn
        /// <summary>
        /// Sn
        /// </summary>
        [Label("Sn")]
        public static readonly Property<string> ProductSnProperty = P<ReverseTraceViewModel>.Register(e => e.ProductSn);
        /// <summary>
        /// Sn
        /// </summary>
        public string ProductSn
        {
			get { return GetProperty(ProductSnProperty); }
			set { SetProperty(ProductSnProperty, value); }
		}
        #endregion

        #region 工单Id WorkOrderId
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单Id")]
        public static readonly Property<double> WorkOrderIdProperty = P<ReverseTraceViewModel>.Register(e => e.WorkOrderId);
        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return GetProperty(WorkOrderIdProperty); }
            set { SetProperty(WorkOrderIdProperty, value); }
        }
        #endregion

        #region 工单 WorkOrderNo
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WorkOrderNoProperty = P<ReverseTraceViewModel>.Register(e => e.WorkOrderNo);
        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrderNo
        {
            get { return GetProperty(WorkOrderNoProperty); }
            set { SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 车间 WorkShopName
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopNameProperty = P<ReverseTraceViewModel>.Register(e => e.WorkShopName);
        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShopName
        {
            get { return GetProperty(WorkShopNameProperty); }
            set { SetProperty(WorkShopNameProperty, value); }
        }
        #endregion

        #region 工艺路线 VersionName
        /// <summary>
        /// 工艺路线
        /// </summary>
        [Label("工艺路线")]
        public static readonly Property<string> VersionNameProperty = P<ReverseTraceViewModel>.Register(e => e.VersionName);

        /// <summary>
        /// 工艺路线
        /// </summary>
        public string VersionName
        {
            get { return GetProperty(VersionNameProperty); }
            set { SetProperty(VersionNameProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<int> QtyProperty = P<ReverseTraceViewModel>.Register(e => e.Qty);
        /// <summary>
        /// Sn
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 生产日期 ProductionDate
        /// <summary>
        /// 生产日期
        /// </summary>
        [Label("生产日期")]
        public static readonly Property<DateTime?> ProductionDateProperty = P<ReverseTraceViewModel>.Register(e => e.ProductionDate);
        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProductionDate
        {
            get { return GetProperty(ProductionDateProperty); }
            set { SetProperty(ProductionDateProperty, value); }
        }
        #endregion
    }
}