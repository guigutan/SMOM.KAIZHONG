using SIE.Core.ProjectMaintains;
using SIE.Domain;
using SIE.Items;
using SIE.MES.Routings.RoutingBoms.ImportBoms;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.ProcessSegments;
using SIE.Tech.Routings;
using System;

namespace SIE.MES.Routings.RoutingBoms
{
    /// <summary>
    /// 工序bom主表(工艺路线版本)
    /// </summary>
    [RootEntity, Serializable]
	[ConditionQueryType(typeof(RoutingBomCriteria))]
	[Label("工序bom维护主表")]
	public partial class RoutingBom : DataEntity
	{
		#region 工艺路线版本 RoutingVersion
		/// <summary>
		/// 工艺路线版本Id
		/// </summary>
		[Label("工艺路线版本")]
		public static readonly IRefIdProperty RoutingVersionIdProperty = P<RoutingBom>.RegisterRefId(e => e.RoutingVersionId, ReferenceType.Normal);

		/// <summary>
		/// 工艺路线版本Id
		/// </summary>
		public double RoutingVersionId
		{
			get { return (double)GetRefId(RoutingVersionIdProperty); }
			set { SetRefId(RoutingVersionIdProperty, value); }
		}

		/// <summary>
		/// 工艺路线版本
		/// </summary>
		public static readonly RefEntityProperty<RoutingVersion> RoutingVersionProperty =
			P<RoutingBom>.RegisterRef(e => e.RoutingVersion, RoutingVersionIdProperty);

		/// <summary>
		/// 工艺路线版本
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
		[Label("工艺路线")]
		public static readonly IRefIdProperty RoutingIdProperty = P<RoutingBom>.RegisterRefId(e => e.RoutingId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Routing> RoutingProperty = P<RoutingBom>.RegisterRef(e => e.Routing, RoutingIdProperty);

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
		[Label("产品")]
		public static readonly IRefIdProperty ProductIdProperty = P<RoutingBom>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Item> ProductProperty = P<RoutingBom>.RegisterRef(e => e.Product, ProductIdProperty);

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
		[Label("工段")]
		public static readonly IRefIdProperty ProcessSegmentIdProperty = P<RoutingBom>.RegisterRefId(e => e.ProcessSegmentId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<ProcessSegment> ProcessSegmentProperty = P<RoutingBom>.RegisterRef(e => e.ProcessSegment, ProcessSegmentIdProperty);

		/// <summary>
		/// 工段
		/// </summary>
		public ProcessSegment ProcessSegment
		{
			get { return GetRefEntity(ProcessSegmentProperty); }
			set { SetRefEntity(ProcessSegmentProperty, value); }
		}
		#endregion

		#region 工序BOM明细 RoutingBomDetailList
		/// <summary>
		/// 工序BOM明细
		/// </summary>
		[Label("工序BOM明细")]
		public static readonly ListProperty<EntityList<RoutingBomDetail>> RoutingBomDetailListProperty =
			P<RoutingBom>.RegisterList(e => e.RoutingBomDetailList);

		/// <summary>
		/// 工序BOM
		/// </summary>
		public EntityList<RoutingBomDetail> RoutingBomDetailList
		{
			get { return this.GetLazyList(RoutingBomDetailListProperty); }
		}
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<RoutingBom>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 物料扩展属性显示值 ItemExtPropName
        /// <summary>
        /// 物料扩展属性显示值
        /// </summary>
        [Label("物料扩展属性值")]
        public static readonly Property<string> ItemExtPropNameProperty = P<RoutingBom>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性显示值
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 扩展属性编辑 IsAllowEdit
        /// <summary>
        /// 扩展属性编辑
        /// </summary>
        [Label("扩展属性编辑")]
        public static readonly Property<bool> IsAllowEditProperty = P<RoutingBom>.Register(e => e.IsAllowEdit);

        /// <summary>
        /// 扩展属性编辑
        /// </summary>
        public bool IsAllowEdit
        {
            get { return this.GetProperty(IsAllowEditProperty); }
            set { this.SetProperty(IsAllowEditProperty, value); }
        }
        #endregion

        #region Bom附件 Attachments
        /// <summary>
        /// CAD附件
        /// </summary>
        [Label("Bom附件")]
		public static readonly ListProperty<EntityList<RoutingBomAttachment>> AttachmentsProperty = P<RoutingBom>.RegisterList(e => e.Attachments);

		/// <summary>
		/// Bom附件
		/// </summary>
		public EntityList<RoutingBomAttachment> Attachments
		{
			get { return this.GetLazyList(AttachmentsProperty); }
		}
		#endregion

		#region 项目号 ProjectMaintain
		/// <summary>
		/// 项目号Id
		/// </summary>
		[Label("项目号")]
		public static readonly IRefIdProperty ProjectMaintainIdProperty =
			P<RoutingBom>.RegisterRefId(e => e.ProjectMaintainId, ReferenceType.Normal);

		/// <summary>
		/// 项目号Id
		/// </summary>
		public double? ProjectMaintainId
		{
			get { return (double?)this.GetRefNullableId(ProjectMaintainIdProperty); }
			set { this.SetRefNullableId(ProjectMaintainIdProperty, value); }
		}

		/// <summary>
		/// 项目号
		/// </summary>
		public static readonly RefEntityProperty<ProjectMaintain> ProjectMaintainProperty =
			P<RoutingBom>.RegisterRef(e => e.ProjectMaintain, ProjectMaintainIdProperty);

		/// <summary>
		/// 项目号
		/// </summary>
		public ProjectMaintain ProjectMaintain
		{
			get { return this.GetRefEntity(ProjectMaintainProperty); }
			set { this.SetRefEntity(ProjectMaintainProperty, value); }
		}
		#endregion

		#region 视图属性

		#region 产品名称 ProductName
		/// <summary>
		/// 产品名称
		/// </summary>
		[Label("产品名称")]
		public static readonly Property<string> ProductNameProperty = P<RoutingBom>.RegisterView(e => e.ProductName, p => p.Product.Name);

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
		public static readonly Property<string> ProductCodeProperty = P<RoutingBom>.RegisterView(e => e.ProductCode, p => p.Product.Code);

		/// <summary>
		/// 产品名称
		/// </summary>
		public string ProductCode
		{
			get { return this.GetProperty(ProductCodeProperty); }
		}
		#endregion

		#endregion
	}

	/// <summary>
	/// 产品工艺路线版本 实体配置
	/// </summary>
	internal class RoutingBomConfig : EntityConfig<RoutingBom>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("TECH_ROUTING_BOM").MapAllProperties();
			Meta.Property(RoutingBom.RoutingVersionIdProperty).ColumnMeta.HasIndex();
			Meta.EnablePhantoms();
		}
	}
}