using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;

namespace SIE.Traces.ForwardTraces
{
    /// <summary>
    /// 正向追溯查询实体
    /// </summary>
    [QueryEntity, Serializable]
	public partial class ForwardTraceViewModelCriteria : Criteria
	{
		/// <summary>
		/// 
		/// </summary>
		public ForwardTraceViewModelCriteria() {

            ProductionDate = new DateRange();
            ProductionDate.DateTimePart = DateTimePart.Date;
            ProductionDate.DateRangeType = DateRangeType.All;

            ReceiptDate = new DateRange();
            ReceiptDate.DateTimePart = DateTimePart.Date;
            ReceiptDate.DateRangeType = DateRangeType.Month;
        }

		#region Lot
		/// <summary>
		/// 物料批次
		/// </summary>
		[Label("物料批次")]
		public static readonly Property<string> LotProperty = P<ForwardTraceViewModelCriteria>.Register(e => e.Lot);

		/// <summary>
		/// 物料批次
		/// </summary>
		public string Lot
		{
			get { return GetProperty(LotProperty); }
			set { SetProperty(LotProperty, value); }
		}
		#endregion

		#region Sn
		/// <summary>
		/// 序列号
		/// </summary>
		[Label("序列号")]
		public static readonly Property<string> SnProperty = P<ForwardTraceViewModelCriteria>.Register(e => e.Sn);

		/// <summary>
		/// 序列号
		/// </summary>
		public string Sn
		{
			get { return GetProperty(SnProperty); }
			set { SetProperty(SnProperty, value); }
		}
		#endregion

		#region ProductionDate
		/// <summary>
		/// 生产日期
		/// </summary>
		[Label("生产日期")]
		public static readonly Property<DateRange> ProductionDateProperty = P<ForwardTraceViewModelCriteria>.Register(e => e.ProductionDate);

		/// <summary>
		/// 生产日期
		/// </summary>
		public DateRange ProductionDate
		{
			get { return GetProperty(ProductionDateProperty); }
			set { SetProperty(ProductionDateProperty, value); }
		}
		#endregion

		#region ReceiptDate
		/// <summary>
		/// 收货日期
		/// </summary>
		[Label("收货日期")]
		public static readonly Property<DateRange> ReceiptDateProperty = P<ForwardTraceViewModelCriteria>.Register(e => e.ReceiptDate);

		/// <summary>
		/// 收货日期
		/// </summary>
		public DateRange ReceiptDate
		{
			get { return GetProperty(ReceiptDateProperty); }
			set { SetProperty(ReceiptDateProperty, value); }
		}
		#endregion

		#region 物料编码 Item
		/// <summary>
		/// 物料编码Id
		/// </summary>
		[Label("物料编码")]
		public static readonly IRefIdProperty ItemIdProperty = P<ForwardTraceViewModelCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

		/// <summary>
		/// 物料编码Id
		/// </summary>
		public double? ItemId
		{
			get { return (double?)GetRefNullableId(ItemIdProperty); }
			set { SetRefNullableId(ItemIdProperty, value); }
		}

		/// <summary>
		/// 物料编码
		/// </summary>
		public static readonly RefEntityProperty<Item> ItemProperty = P<ForwardTraceViewModelCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

		/// <summary>
		/// 物料编码
		/// </summary>
		public Item Item
		{
			get { return GetRefEntity(ItemProperty); }
			set { SetRefEntity(ItemProperty, value); }
		}
        #endregion

        #region 物料扩展属性 ItemExtPropName
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropNameProperty = P<ForwardTraceViewModelCriteria>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get { return GetProperty(ItemExtPropNameProperty); }
            set { SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty = P<ForwardTraceViewModelCriteria>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

		/// <summary>
		/// 供应商Id
		/// </summary>
		public double? SupplierId
		{
			get { return (double?)GetRefNullableId(SupplierIdProperty); }
			set { SetRefNullableId(SupplierIdProperty, value); }
		}

		/// <summary>
		/// 供应商
		/// </summary>
		public static readonly RefEntityProperty<Supplier> SupplierProperty = P<ForwardTraceViewModelCriteria>.RegisterRef(e => e.Supplier, SupplierIdProperty);

		/// <summary>
		/// 供应商
		/// </summary>
		public Supplier Supplier
		{
			get { return GetRefEntity(SupplierProperty); }
			set { SetRefEntity(SupplierProperty, value); }
		}
		#endregion

		/// <summary>
		/// 重写此方法实现查询
		/// </summary>
		protected override EntityList Fetch()
		{
            return RT.Service.Resolve<ForwardTraceController>().GetTraceItems(this, this.PagingInfo);
        }
	}
}
