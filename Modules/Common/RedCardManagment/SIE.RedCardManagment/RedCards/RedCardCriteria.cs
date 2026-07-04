using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.RedCardManagment.RedCards
{
    /// <summary>
    /// 红牌管理-查询
    /// </summary>
    [QueryEntity, Serializable]
	[Label("红牌管理")]
	public partial class RedCardCriteria : Criteria
	{
		#region 红牌单号 No
		/// <summary>
		/// 红牌单号
		/// </summary>
		[Required]
		[NotDuplicate]
		[Label("红牌单号")]
		public static readonly Property<string> NoProperty = P<RedCardCriteria>.Register(e => e.No);

		/// <summary>
		/// 红牌单号
		/// </summary>
		public string No
		{
			get { return GetProperty(NoProperty); }
			set { SetProperty(NoProperty, value); }
		}
		#endregion

		#region 物料 ItemName
		/// <summary>
		/// 物料
		/// </summary>
		[Label("物料")]
		public static readonly Property<string> ItemNameProperty = P<RedCardCriteria>.Register(e => e.ItemName);

		/// <summary>
		/// 物料
		/// </summary>
		public string ItemName
		{
			get { return GetProperty(ItemNameProperty); }
			set { SetProperty(ItemNameProperty, value); }
		}
		#endregion

		#region 供应商 Supplier
		/// <summary>
		/// 供应商
		/// </summary>
		[Label("供应商")]
		public static readonly Property<string> SupplierProperty = P<RedCardCriteria>.Register(e => e.Supplier);

		/// <summary>
		/// 供应商
		/// </summary>
		public string Supplier
		{
			get { return GetProperty(SupplierProperty); }
			set { SetProperty(SupplierProperty, value); }
		}
		#endregion

		#region 物料SN ItemSN
		/// <summary>
		/// 物料SN
		/// </summary>
		[Label("物料SN")]
		public static readonly Property<string> ItemSNProperty = P<RedCardCriteria>.Register(e => e.ItemSN);

		/// <summary>
		/// 物料SN
		/// </summary>
		public string ItemSN
		{
			get { return GetProperty(ItemSNProperty); }
			set { SetProperty(ItemSNProperty, value); }
		}
		#endregion

		#region 执行时间 ApplyTime
		/// <summary>
		/// 执行时间
		/// </summary>
		[Label("执行时间")]
		public static readonly Property<DateRange> ApplyTimeProperty = P<RedCardCriteria>.Register(e => e.ApplyTime);

		/// <summary>
		/// 执行时间
		/// </summary>
		public DateRange ApplyTime
		{
			get { return GetProperty(ApplyTimeProperty); }
			set { SetProperty(ApplyTimeProperty, value); }
		}
		#endregion

		#region 申请单号 ApplyBillNo
		/// <summary>
		/// 申请单号
		/// </summary>
		[Label("申请单号")]
		public static readonly Property<string> ApplyBillNoProperty = P<RedCardCriteria>.Register(e => e.ApplyBillNo);

		/// <summary>
		/// 申请单号
		/// </summary>
		public string ApplyBillNo
		{
			get { return GetProperty(ApplyBillNoProperty); }
			set { SetProperty(ApplyBillNoProperty, value); }
		}
		#endregion

		#region 物料批次 ItemBatch
		/// <summary>
		/// 物料批次
		/// </summary>
		[Label("物料批次")]
		public static readonly Property<string> ItemBatchProperty = P<RedCardCriteria>.Register(e => e.ItemBatch);

		/// <summary>
		/// 物料批次
		/// </summary>
		public string ItemBatch
		{
			get { return GetProperty(ItemBatchProperty); }
			set { SetProperty(ItemBatchProperty, value); }
		}
		#endregion

		#region 状态 Status
		/// <summary>
		/// 状态
		/// </summary>
		[Label("状态")]
		public static readonly Property<RedCardState?> StatusProperty = P<RedCardCriteria>.Register(e => e.Status);

		/// <summary>
		/// 状态
		/// </summary>
		public RedCardState? Status
		{
			get { return GetProperty(StatusProperty); }
			set { SetProperty(StatusProperty, value); }
		}
		#endregion

		#region 添加方式 AddWay
		/// <summary>
		/// 添加方式
		/// </summary>
		[Label("添加方式")]
		public static readonly Property<RecordAddWay?> AddWayProperty = P<RedCardCriteria>.Register(e => e.AddWay);

		/// <summary>
		/// 添加方式
		/// </summary>
		public RecordAddWay? AddWay
		{
			get { return GetProperty(AddWayProperty); }
			set { SetProperty(AddWayProperty, value); }
		}
		#endregion

		#region 创建时间 CreateDate
		/// <summary>
		/// 创建时间
		/// </summary>
		[Label("创建时间")]
		public static readonly Property<DateRange> CreateDateProperty = P<RedCardCriteria>.Register(e => e.CreateDate);

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateRange CreateDate
		{
			get { return this.GetProperty(CreateDateProperty); }
			set { this.SetProperty(CreateDateProperty, value); }
		}
		#endregion



		protected override EntityList Fetch()
		{
			return RT.Service.Resolve<RedCardService>().GetRedCards(this);
		}
	}
    
}