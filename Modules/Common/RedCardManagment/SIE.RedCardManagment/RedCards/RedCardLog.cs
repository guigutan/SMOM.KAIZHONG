using SIE;
using SIE.Common.Employees;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.RedCardManagment.RedCards
{
	/// <summary>
	/// 红牌操作日志
	/// </summary>
	[RootEntity, Serializable]
	[CriteriaQuery]
	[Label("红牌操作日志")]
	public partial class RedCardLog : DataEntity
	{
		#region 物料SN SN
		/// <summary>
		/// 物料SN
		/// </summary>
		[Label("物料SN")]
		public static readonly Property<string> SNProperty = P<RedCardLog>.Register(e => e.SN);

		/// <summary>
		/// 物料SN
		/// </summary>
		public string SN
		{
			get { return GetProperty(SNProperty); }
			set { SetProperty(SNProperty, value); }
		}
        #endregion

        #region 生产周期开始时间 ProductDateStart
        /// <summary>
        /// 生产周期开始时间
        /// </summary>
        [Label("生产周期开始时间")]
        public static readonly Property<DateTime?> ProductDateStartProperty = P<RedCardLog>.Register(e => e.ProductDateStart);

        /// <summary>
        /// 生产周期开始时间
        /// </summary>
        public DateTime? ProductDateStart
        {
            get { return GetProperty(ProductDateStartProperty); }
            set { SetProperty(ProductDateStartProperty, value); }
        }
        #endregion

        #region 生产周期结束时间 ProductDateEnd
        /// <summary>
        /// 生产周期结束时间
        /// </summary>
        [Label("生产周期结束时间")]
        public static readonly Property<DateTime?> ProductDateEndProperty = P<RedCardLog>.Register(e => e.ProductDateEnd);

        /// <summary>
        /// 生产周期结束时间
        /// </summary>
        public DateTime? ProductDateEnd
        {
            get { return GetProperty(ProductDateEndProperty); }
            set { SetProperty(ProductDateEndProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        public static readonly IRefIdProperty SupplierIdProperty = P<RedCardLog>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double? SupplierId
        {
            get { return (double?)GetRefId(SupplierIdProperty); }
            set { SetRefId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<RedCardLog>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        public static readonly IRefIdProperty ItemIdProperty = P<RedCardLog>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<RedCardLog>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 执行人 Applicant
        /// <summary>
        /// 执行人Id
        /// </summary>
        public static readonly IRefIdProperty ApplicantIdProperty = P<RedCardLog>.RegisterRefId(e => e.ApplicantId, ReferenceType.Normal);

        /// <summary>
        /// 执行人Id
        /// </summary>
        public double ApplicantId
        {
            get { return (double)GetRefId(ApplicantIdProperty); }
            set { SetRefId(ApplicantIdProperty, value); }
        }

        /// <summary>
        /// 执行人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ApplicantProperty = P<RedCardLog>.RegisterRef(e => e.Applicant, ApplicantIdProperty);

        /// <summary>
        /// 执行人
        /// </summary>
        public Employee Applicant
        {
            get { return GetRefEntity(ApplicantProperty); }
            set { SetRefEntity(ApplicantProperty, value); }
        }
        #endregion

        #region 红牌单号 RedCardNo
        /// <summary>
        /// 红牌单号
        /// </summary>
        [Required]
		[Label("红牌单号")]
		public static readonly Property<string> RedCardNoProperty = P<RedCardLog>.Register(e => e.RedCardNo);

		/// <summary>
		/// 红牌单号
		/// </summary>
		public string RedCardNo
		{
			get { return GetProperty(RedCardNoProperty); }
			set { SetProperty(RedCardNoProperty, value); }
		}
		#endregion

		#region 生产批次 Batch
		/// <summary>
		/// 生产批次
		/// </summary>
		[Label("生产批次")]
		public static readonly Property<string> BatchProperty = P<RedCardLog>.Register(e => e.Batch);

		/// <summary>
		/// 生产批次
		/// </summary>
		public string Batch
		{
			get { return GetProperty(BatchProperty); }
			set { SetProperty(BatchProperty, value); }
		}
		#endregion

		#region 物料批次 ItemBatch
		/// <summary>
		/// 物料批次
		/// </summary>
		[Label("物料批次")]
		public static readonly Property<string> ItemBatchProperty = P<RedCardLog>.Register(e => e.ItemBatch);

		/// <summary>
		/// 物料批次
		/// </summary>
		public string ItemBatch
		{
			get { return GetProperty(ItemBatchProperty); }
			set { SetProperty(ItemBatchProperty, value); }
		}
        #endregion

        #region 产品SN ProductSn
        /// <summary>
        /// 产品SN
        /// </summary>
        [Label("产品SN")]
        public static readonly Property<string> ProductSnProperty = P<RedCardLog>.Register(e => e.ProductSn);

        /// <summary>
        /// 产品SN
        /// </summary>
        public string ProductSn
        {
            get { return GetProperty(ProductSnProperty); }
            set { SetProperty(ProductSnProperty, value); }
        }
        #endregion

        #region 状态 Status
        /// <summary>
        /// 状态
        /// </summary>
        [Label("红牌状态")]
		public static readonly Property<RedCardState> StatusProperty = P<RedCardLog>.Register(e => e.Status);

		/// <summary>
		/// 状态
		/// </summary>
		public RedCardState Status
		{
			get { return GetProperty(StatusProperty); }
			set { SetProperty(StatusProperty, value); }
		}
        #endregion

        #region 执行时间 ApplyTime
        /// <summary>
        /// 执行时间
        /// </summary>
        [Label("执行时间")]
        public static readonly Property<DateTime?> ApplyTimeProperty = P<RedCardLog>.Register(e => e.ApplyTime);

        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime? ApplyTime
        {
            get { return GetProperty(ApplyTimeProperty); }
            set { SetProperty(ApplyTimeProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
		public static readonly Property<string> ItemCodeProperty = P<RedCardLog>.RegisterView(e => e.ItemCode, p => p.Item.Code);

		/// <summary>
		/// 物料编码
		/// </summary>
		public string ItemCode
		{
			get { return this.GetProperty(ItemCodeProperty); }
		}
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<RedCardLog >.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 供应商编号 SupplierCode
        /// <summary>
        /// 供应商编号
        /// </summary>
        [Label("供应商编号")]
        public static readonly Property<string> SupplierCodeProperty = P<RedCardLog>.RegisterView(e => e.SupplierCode, p => p.Supplier.Code);

        /// <summary>
        /// 供应商编号
        /// </summary>
        public string SupplierCode
        {
            get { return this.GetProperty(SupplierCodeProperty); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<RedCardLog>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #region 执行人名称 ApplicantName
        /// <summary>
        /// 执行人名称
        /// </summary>
        [Label("执行人")]
        public static readonly Property<string> ApplicantNameProperty = P<RedCardLog>.RegisterView(e => e.ApplicantName, p => p.Applicant.Name);

        /// <summary>
        /// 执行人名称
        /// </summary>
        public string ApplicantName
        {
            get { return this.GetProperty(ApplicantNameProperty); }
        }

		#endregion


	}

    /// <summary>
    /// 红牌操作日志 实体配置
    /// </summary>
    internal class RedCardLogConfig : EntityConfig<RedCardLog>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("QMS_RCD_LOG").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}