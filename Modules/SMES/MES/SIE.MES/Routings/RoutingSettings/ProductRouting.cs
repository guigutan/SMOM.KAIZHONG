using SIE.Core.ProjectMaintains;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.Routings.RoutingSettings;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.ProcessSegments;
using SIE.Tech.Routings;
using System;

namespace SIE.MES.RoutingSettings
{
    /// <summary>
    /// 产品工艺路线设置
    /// </summary>
    [RootEntity, Serializable]
   // [CriteriaQuery]
    [ConditionQueryType(typeof(ProductRoutingCriteria))]
    [Label("产品工艺路线设置")]
    public partial class ProductRouting : DataEntity
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductRouting()
        {
            StartDate = DateTime.Now.Date;
        }
        #endregion

        #region 开始日期 StartDate
        /// <summary>
        /// 开始日期
        /// </summary>
        [Label("开始日期")]
        public static readonly Property<DateTime> StartDateProperty = P<ProductRouting>
            .Register(e => e.StartDate, new PropertyMetadata<DateTime>() { DateTimePart = DateTimePart.Date });

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime StartDate
        {
            get { return GetProperty(StartDateProperty); }
            set { SetProperty(StartDateProperty, value); }
        }
        #endregion

        #region 结束日期 EndDate
        /// <summary>
        /// 结束日期
        /// </summary>
        [Label("结束日期")]
        [Required]
        public static readonly Property<DateTime?> EndDateProperty = P<ProductRouting>
            .Register(e => e.EndDate, new PropertyMetadata<DateTime?>() { DateTimePart = DateTimePart.Date });

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate
        {
            get { return GetProperty(EndDateProperty); }
            set { SetProperty(EndDateProperty, value); }
        }
        #endregion

        #region 工单类型 OrderType
        /// <summary>
        /// 工单类型
        /// </summary>
        [Required]
        [Label("工单类型")]
        public static readonly Property<WorkOrderType> OrderTypeProperty = P<ProductRouting>.Register(e => e.OrderType);

        /// <summary>
        /// 工单类型
        /// </summary>
        public WorkOrderType OrderType
        {
            get { return GetProperty(OrderTypeProperty); }
            set { SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 工艺路线 Routing
        /// <summary>
        /// 工艺路线Id
        /// </summary>
        [Required]
        [Label("工艺路线")]
        public static readonly IRefIdProperty RoutingIdProperty = P<ProductRouting>.RegisterRefId(e => e.RoutingId, ReferenceType.Normal);

        /// <summary>
        /// 工艺路线Id
        /// </summary>
        public double? RoutingId
        {
            get { return (double?)GetRefNullableId(RoutingIdProperty); }
            set { SetRefNullableId(RoutingIdProperty, value); }
        }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public static readonly RefEntityProperty<Routing> RoutingProperty = P<ProductRouting>.RegisterRef(e => e.Routing, RoutingIdProperty);

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
        [Required]
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty = P<ProductRouting>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ProductId
        {
            get { return (double?)GetRefNullableId(ProductIdProperty); }
            set { SetRefNullableId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty = P<ProductRouting>.RegisterRef(e => e.Product, ProductIdProperty);

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
        public static readonly IRefIdProperty ProcessSegmentIdProperty = P<ProductRouting>.RegisterRefId(e => e.ProcessSegmentId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<ProcessSegment> ProcessSegmentProperty = P<ProductRouting>.RegisterRef(e => e.ProcessSegment, ProcessSegmentIdProperty);

        /// <summary>
        /// 工段
        /// </summary>
        public ProcessSegment ProcessSegment
        {
            get { return GetRefEntity(ProcessSegmentProperty); }
            set { SetRefEntity(ProcessSegmentProperty, value); }
        }
        #endregion

        #region 项目号 ProjectMaintain
        /// <summary>
        /// 项目号Id
        /// </summary>
        [Label("项目号")]
        public static readonly IRefIdProperty ProjectMaintainIdProperty =
            P<ProductRouting>.RegisterRefId(e => e.ProjectMaintainId, ReferenceType.Normal);

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
            P<ProductRouting>.RegisterRef(e => e.ProjectMaintain, ProjectMaintainIdProperty);

        /// <summary>
        /// 项目号
        /// </summary>
        public ProjectMaintain ProjectMaintain
        {
            get { return this.GetRefEntity(ProjectMaintainProperty); }
            set { this.SetRefEntity(ProjectMaintainProperty, value); }
        }
        #endregion

        #region 版本 RoutingVersion
        /// <summary>
        /// 版本Id
        /// </summary>
        [Label("版本")]
        public static readonly IRefIdProperty RoutingVersionIdProperty =
            P<ProductRouting>.RegisterRefId(e => e.RoutingVersionId, ReferenceType.Normal);

        /// <summary>
        /// 版本Id
        /// </summary>
        public double? RoutingVersionId
        {
            get { return (double?)this.GetRefNullableId(RoutingVersionIdProperty); }
            set { this.SetRefNullableId(RoutingVersionIdProperty, value); }
        }

        /// <summary>
        /// 版本
        /// </summary>
        public static readonly RefEntityProperty<RoutingVersion> RoutingVersionProperty =
            P<ProductRouting>.RegisterRef(e => e.RoutingVersion, RoutingVersionIdProperty);

        /// <summary>
        /// 版本
        /// </summary>
        public RoutingVersion RoutingVersion
        {
            get { return this.GetRefEntity(RoutingVersionProperty); }
            set { this.SetRefEntity(RoutingVersionProperty, value); }
        }
        #endregion


        #region 视图属性
        #region 工艺路线名称 RoutingName
        /// <summary>
        /// 工艺路线名称
        /// </summary>
        [Label("工艺路线")]
        public static readonly Property<string> RoutingNameProperty = P<ProductRouting>.RegisterView(e => e.RoutingName, p => p.Routing.Name);

        /// <summary>
        /// 注释
        /// </summary>
        public string RoutingName
        {
            get { return this.GetProperty(RoutingNameProperty); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<ProductRouting>.RegisterView(e => e.ProductName, p => p.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 产品工艺路线 实体配置
    /// </summary>
    internal class ProductRoutingConfig : EntityConfig<ProductRouting>
    {
        /// <summary>
        /// 产品工艺路线设置校验规则
        /// </summary>
        /// <param name="rules">规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new HandlerRule()
            {
                Handler = (o, e) =>
                {
                    var pr = o.CastTo<ProductRouting>();
                    if (pr.StartDate > pr.EndDate)
                        e.BrokenDescription = "结束时间不能小于开始时间".L10N();
                }
            });
        }

        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_PRODUCT_RT").MapAllProperties();
            Meta.IndexGroupOnProperties(ProductRouting.ProductIdProperty, ProductRouting.ProcessSegmentIdProperty);
            Meta.EnablePhantoms();
        }
    }
}