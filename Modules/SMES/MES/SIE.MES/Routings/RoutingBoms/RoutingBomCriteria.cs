using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Resources.ProcessSegments;
using SIE.Tech.Routings;
using System;

namespace SIE.MES.Routings.RoutingBoms
{
    /// <summary>
    /// 工艺路线版本工序bom维护主表
    /// </summary>
    [QueryEntity, Serializable]
	[Label("工艺路线版本工序bom维护主表查询体")]
	public partial class RoutingBomCriteria : Criteria
	{
		#region 工艺路线版本 RoutingVersion
		/// <summary>
		/// 工艺路线Id
		/// </summary>
		public static readonly IRefIdProperty RoutingVersionIdProperty = P<RoutingBomCriteria>.RegisterRefId(e => e.RoutingVersionId, ReferenceType.Normal);

		/// <summary>
		/// 工艺路线Id
		/// </summary>
		public double RoutingVersionId
		{
			get { return (double)GetRefId(RoutingVersionIdProperty); }
			set { SetRefId(RoutingVersionIdProperty, value); }
		}

		/// <summary>
		/// 工艺路线
		/// </summary>
		public static readonly RefEntityProperty<RoutingVersion> RoutingVersionProperty =
			P<RoutingBomCriteria>.RegisterRef(e => e.RoutingVersion, RoutingVersionIdProperty);

		/// <summary>
		/// 工艺路线
		/// </summary>
		public RoutingVersion RoutingVersion
		{
			get { return GetRefEntity(RoutingVersionProperty); }
			set { SetRefEntity(RoutingVersionProperty, value); }
		}
		#endregion

		#region 工艺路线 Routing
		/// <summary>
		/// 工艺路线Id
		/// </summary>
		public static readonly IRefIdProperty RoutingIdProperty = P<RoutingBomCriteria>.RegisterRefId(e => e.RoutingId, ReferenceType.Normal);

		/// <summary>
		/// 工艺路线Id
		/// </summary>
		public double RoutingId
		{
			get { return (double)GetRefId(RoutingIdProperty); }
			set { SetRefId(RoutingIdProperty, value); }
		}

		/// <summary>
		/// 工艺路线
		/// </summary>
		public static readonly RefEntityProperty<Routing> RoutingProperty = P<RoutingBomCriteria>.RegisterRef(e => e.Routing, RoutingIdProperty);

		/// <summary>
		/// 工艺路线
		/// </summary>
		public Routing Routing
		{
			get { return GetRefEntity(RoutingProperty); }
			set { SetRefEntity(RoutingProperty, value); }
		}
		#endregion

		#region 产品 Product
		/// <summary>
		/// 产品Id
		/// </summary>
		public static readonly IRefIdProperty ProductIdProperty = P<RoutingBomCriteria>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

		/// <summary>
		/// 产品Id
		/// </summary>
		public double ProductId
		{
			get { return (double)GetRefId(ProductIdProperty); }
			set { SetRefId(ProductIdProperty, value); }
		}

		/// <summary>
		/// 产品
		/// </summary>
		public static readonly RefEntityProperty<Item> ProductProperty = P<RoutingBomCriteria>.RegisterRef(e => e.Product, ProductIdProperty);

		/// <summary>
		/// 产品
		/// </summary>
		public Item Product
		{
			get { return GetRefEntity(ProductProperty); }
			set { SetRefEntity(ProductProperty, value); }
		}
		#endregion

		#region 工段 ProcessSegment
		/// <summary>
		/// 工段Id
		/// </summary>
		public static readonly IRefIdProperty ProcessSegmentIdProperty = P<RoutingBomCriteria>.RegisterRefId(e => e.ProcessSegmentId, ReferenceType.Normal);

		/// <summary>
		/// 工段Id
		/// </summary>
		public double? ProcessSegmentId
		{
			get { return (double?)GetRefNullableId(ProcessSegmentIdProperty); }
			set { SetRefNullableId(ProcessSegmentIdProperty, value); }
		}

		/// <summary>
		/// 工段
		/// </summary>
		public static readonly RefEntityProperty<ProcessSegment> ProcessSegmentProperty = P<RoutingBomCriteria>.RegisterRef(e => e.ProcessSegment, ProcessSegmentIdProperty);

		/// <summary>
		/// 工段
		/// </summary>
		public ProcessSegment ProcessSegment
		{
			get { return GetRefEntity(ProcessSegmentProperty); }
			set { SetRefEntity(ProcessSegmentProperty, value); }
		}
		#endregion

		#region 工序BOM RoutingBomDetailList
		/// <summary>
		/// 工序BOM
		/// </summary>
		public static readonly ListProperty<EntityList<RoutingBomDetail>> RoutingBomDetailListProperty =
			P<RoutingBomCriteria>.RegisterList(e => e.RoutingBomDetailList);

		/// <summary>
		/// 工序BOM
		/// </summary>
		public EntityList<RoutingBomDetail> RoutingBomDetailList
		{
			get { return this.GetLazyList(RoutingBomDetailListProperty); }
		}
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<RoutingBomCriteria>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
		#endregion

		#region 物料编码 ItemCode
		/// <summary>
		/// 物料编码
		/// </summary>
		[Label("物料编码")]
        public static readonly Property<string> NameProperty = P<RoutingBomCriteria>.Register(e => e.ItemCode);

		/// <summary>
		/// 物料编码
		/// </summary>
		public string ItemCode
		{
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion



        #region 视图属性

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
		public static readonly Property<string> ProductNameProperty = P<RoutingBomCriteria>.RegisterView(e => e.ProductName, p => p.Product.Name);

		/// <summary>
		/// 产品名称
		/// </summary>
		public string ProductName
		{
			get { return this.GetProperty(ProductNameProperty); }
		}
		#endregion
		#region 产品编码 ProductCode
		/// <summary>
		/// 产品名称
		/// </summary>
		[Label("产品编码")]
		public static readonly Property<string> ProductCodeProperty = P<RoutingBomCriteria>.RegisterView(e => e.ProductCode, p => p.Product.Code);

		/// <summary>
		/// 产品名称
		/// </summary>
		public string ProductCode
		{
			get { return this.GetProperty(ProductCodeProperty); }
		}
		#endregion
		#endregion

		/// <summary>
		/// 获取工治具需求清单列表
		/// </summary>
		protected override EntityList Fetch()
		{
			return RT.Service.Resolve<RoutingBomController>().GetRoutingBoms(this);
		}
	}
}
