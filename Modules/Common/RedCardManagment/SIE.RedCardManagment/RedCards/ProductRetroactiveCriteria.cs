using SIE;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.RedCardManagment.RedCards
{
	/// <summary>
	/// 产品追溯清单-查询
	/// </summary>
	[QueryEntity, Serializable]
	[Label("产品追溯清单")]
	public partial class ProductRetroactiveCriteria : Criteria
	{
		#region 所属工单号 WorkNo
		/// <summary>
		/// 所属工单号
		/// </summary>
		[Label("所属工单号")]
		public static readonly Property<string> WorkNoProperty = P<ProductRetroactiveCriteria>.Register(e => e.WorkNo);

		/// <summary>
		/// 所属工单号
		/// </summary>
		public string WorkNo
		{
			get { return GetProperty(WorkNoProperty); }
			set { SetProperty(WorkNoProperty, value); }
		}
		#endregion

		#region 执行时间 ApplyTime
		/// <summary>
		/// 执行时间
		/// </summary>
		[Label("执行时间")]
		public static readonly Property<DateRange> ApplyTimeProperty = P<ProductRetroactiveCriteria>.Register(e => e.ApplyTime);

		/// <summary>
		/// 执行时间
		/// </summary>
		public DateRange ApplyTime
		{
			get { return GetProperty(ApplyTimeProperty); }
			set { SetProperty(ApplyTimeProperty, value); }
		}
		#endregion

		#region 产品 Product
		/// <summary>
		/// 产品Id
		/// </summary>
		public static readonly IRefIdProperty ProductIdProperty = P<ProductRetroactiveCriteria>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

		/// <summary>
		/// 产品Id
		/// </summary>
		public double? ProductId
		{
			get { return (double?)GetRefId(ProductIdProperty); }
			set { SetRefId(ProductIdProperty, value); }
		}

		/// <summary>
		/// 产品
		/// </summary>
		public static readonly RefEntityProperty<Item> ProductProperty = P<ProductRetroactiveCriteria>.RegisterRef(e => e.Product, ProductIdProperty);

		/// <summary>
		/// 产品
		/// </summary>
		public Item Product
		{
			get { return GetRefEntity(ProductProperty); }
			set { SetRefEntity(ProductProperty, value); }
		}
		#endregion

		#region 状态 Status
		/// <summary>
		/// 状态
		/// </summary>
		[Label("红牌状态")]
		public static readonly Property<RedCardState?> StatusProperty = P<ProductRetroactiveCriteria>.Register(e => e.Status);

		/// <summary>
		/// 状态
		/// </summary>
		public RedCardState? Status
		{
			get { return GetProperty(StatusProperty); }
			set { SetProperty(StatusProperty, value); }
		}
		#endregion

		#region 执行人 Applicant
		/// <summary>
		/// 执行人Id
		/// </summary>
		public static readonly IRefIdProperty ApplicantIdProperty = P<ProductRetroactiveCriteria>.RegisterRefId(e => e.ApplicantId, ReferenceType.Normal);

		/// <summary>
		/// 执行人Id
		/// </summary>
		public double? ApplicantId
		{
			get { return (double?)GetRefNullableId(ApplicantIdProperty); }
			set { SetRefNullableId(ApplicantIdProperty, value); }
		}

		/// <summary>
		/// 执行人
		/// </summary>
		public static readonly RefEntityProperty<Employee> ApplicantProperty = P<ProductRetroactiveCriteria>.RegisterRef(e => e.Applicant, ApplicantIdProperty);

		/// <summary>
		/// 执行人
		/// </summary>
		public Employee Applicant
		{
			get { return GetRefEntity(ApplicantProperty); }
			set { SetRefEntity(ApplicantProperty, value); }
		}
		#endregion

		#region 物料批次 ItemBatch
		/// <summary>
		/// 物料批次
		/// </summary>
		[Label("物料批次")]
		public static readonly Property<string> ItemBatchProperty = P<ProductRetroactiveCriteria>.Register(e => e.ItemBatch);

		/// <summary>
		/// 物料批次
		/// </summary>
		public string ItemBatch
		{
			get { return GetProperty(ItemBatchProperty); }
			set { SetProperty(ItemBatchProperty, value); }
		}
		#endregion

		#region 物料批次 BatchList		
		/// <summary>
		/// 物料批次
		/// </summary>
		[Label("物料批次")]
		public static readonly Property<string> BatchListProperty = P<ProductRetroactiveCriteria>.Register(e => e.BatchList);

		/// <summary>
		/// 物料批次
		/// </summary>
		public string BatchList
		{
			get { return GetProperty(BatchListProperty); }
			set { SetProperty(BatchListProperty, value); }
		}
		#endregion

		#region 序列号 SN
		/// <summary>
		/// 序列号
		/// </summary>
		[Label("SN")]
		public static readonly Property<string> SNProperty = P<ProductRetroactiveCriteria>.Register(e => e.SN);

		/// <summary>
		/// 序列号
		/// </summary>
		public string SN
		{
			get { return GetProperty(SNProperty); }
			set { SetProperty(SNProperty, value); }
		}
		#endregion

		#region 序列号 SN
		/// <summary>
		/// 序列号
		/// </summary>
		[Label("SN")]
		public static readonly Property<string> SnListProperty = P<ProductRetroactiveCriteria>.Register(e => e.SnList);

		/// <summary>
		/// 序列号
		/// </summary>
		public string SnList
		{
			get { return GetProperty(SnListProperty); }
			set { SetProperty(SnListProperty, value); }
		}
		#endregion

		#region 产品SN ProductSn
		/// <summary>
		/// 产品SN
		/// </summary>
		[Label("产品SN")]
		public static readonly Property<string> ProductSnProperty = P<ProductRetroactiveCriteria>.Register(e => e.ProductSn);

		/// <summary>
		/// 产品SN
		/// </summary>
		public string ProductSn
		{
			get { return GetProperty(ProductSnProperty); }
			set { SetProperty(ProductSnProperty, value); }
		}
		#endregion

		#region 红牌 RedCard
		/// <summary>
		/// 红牌Id
		/// </summary>
		public static readonly IRefIdProperty RedCardIdProperty = P<ProductRetroactiveCriteria>.RegisterRefId(e => e.RedCardId, ReferenceType.Normal);

		/// <summary>
		/// 红牌Id
		/// </summary>
		public double? RedCardId
		{
			get { return (double?)GetRefId(RedCardIdProperty); }
			set { SetRefId(RedCardIdProperty, value); }
		}

		/// <summary>
		/// 红牌
		/// </summary>
		public static readonly RefEntityProperty<RedCard> RedCardProperty = P<ProductRetroactiveCriteria>.RegisterRef(e => e.RedCard, RedCardIdProperty);

		/// <summary>
		/// 红牌
		/// </summary>
		public RedCard RedCard
		{
			get { return GetRefEntity(RedCardProperty); }
			set { SetRefEntity(RedCardProperty, value); }
		}
		#endregion

		protected override EntityList Fetch()
		{
			return RT.Service.Resolve<RedCardService>().GetProductRetroactives(this);
		}
	}
    
}