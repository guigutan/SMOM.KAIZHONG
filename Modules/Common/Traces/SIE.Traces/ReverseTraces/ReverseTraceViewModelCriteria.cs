using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;

namespace SIE.Traces.ReverseTraces
{
    /// <summary>
    /// 反向追溯查询实体
    /// </summary>
    [QueryEntity, Serializable]
	public partial class ReverseTraceViewModelCriteria : Criteria
	{
		/// <summary>
		/// 
		/// </summary>
		public ReverseTraceViewModelCriteria() {

            ProductionDate = new DateRange();
            ProductionDate.DateTimePart = DateTimePart.Date;
            ProductionDate.DateRangeType = DateRangeType.All;
        }

        #region 产品编码 Product
        /// <summary>
        /// 产品编码Id
        /// </summary>
        [Label("产品编码")]
        public static readonly IRefIdProperty ProductIdProperty = P<ReverseTraceViewModelCriteria>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品编码Id
        /// </summary>
        public double? ProductId
        {
            get { return (double?)GetRefNullableId(ProductIdProperty); }
            set { SetRefNullableId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品编码
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty = P<ReverseTraceViewModelCriteria>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品编码
        /// </summary>
        public Item Product
        {
            get { return GetRefEntity(ProductProperty); }
            set { SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region Sn ProductSn
        /// <summary>
        /// Sn
        /// </summary>
        [Label("Sn")]
        public static readonly Property<string> ProductSnProperty = P<ReverseTraceViewModelCriteria>.Register(e => e.ProductSn);

        /// <summary>
        /// Sn
        /// </summary>
        public string ProductSn
        {
            get { return GetProperty(ProductSnProperty); }
            set { SetProperty(ProductSnProperty, value); }
        }
        #endregion

        #region 工单 WorkOrderNo
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
		public static readonly Property<string> WorkOrderNoProperty = P<ReverseTraceViewModelCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrderNo
        {
			get { return GetProperty(WorkOrderNoProperty); }
			set { SetProperty(WorkOrderNoProperty, value); }
		}
		#endregion

		#region ProductionDate
		/// <summary>
		/// 生产日期
		/// </summary>
		[Label("生产日期")]
		public static readonly Property<DateRange> ProductionDateProperty = P<ReverseTraceViewModelCriteria>.Register(e => e.ProductionDate);

		/// <summary>
		/// 生产日期
		/// </summary>
		public DateRange ProductionDate
		{
			get { return GetProperty(ProductionDateProperty); }
			set { SetProperty(ProductionDateProperty, value); }
		}
		#endregion
 
      
		/// <summary>
		/// 重写此方法实现查询
		/// </summary>
		protected override EntityList Fetch()
		{
            return RT.Service.Resolve<ReverseTraceController>().GetTraceProducts(this, this.PagingInfo);
        }
	}
}
