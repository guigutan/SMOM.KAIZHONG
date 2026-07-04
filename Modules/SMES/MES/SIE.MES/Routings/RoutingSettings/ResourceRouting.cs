using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Routings;
using System;

namespace SIE.MES.RoutingSettings
{
    /// <summary>
    /// 产线工艺路线
    /// 此处对应的视图配置为LineRoutingViewConfig
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("产线工艺路线设置")]
    public partial class ResourceRouting : DataEntity
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public ResourceRouting()
        {
            StartDate = DateTime.Now.Date;
        }
        #endregion

        #region 开始时间 StartDate
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateTime> StartDateProperty = P<ResourceRouting>
            .Register(e => e.StartDate, new PropertyMetadata<DateTime>() { DateTimePart = DateTimePart.Date });

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDate
        {
            get { return GetProperty(StartDateProperty); }
            set { SetProperty(StartDateProperty, value); }
        }
        #endregion

        #region 结束时间 EndDate
        /// <summary>
        /// 结束时间
        /// </summary>
        [Label("结束时间")]
        [Required]
        public static readonly Property<DateTime?> EndDateProperty = P<ResourceRouting>
            .Register(e => e.EndDate, new PropertyMetadata<DateTime?>() { DateTimePart = DateTimePart.Date });

        /// <summary>
        /// 结束时间
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
        public static readonly Property<WorkOrderType?> OrderTypeProperty = P<ResourceRouting>.Register(e => e.OrderType);

        /// <summary>
        /// 工单类型
        /// </summary>
        public WorkOrderType? OrderType
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
        public static readonly IRefIdProperty RoutingIdProperty = P<ResourceRouting>.RegisterRefId(e => e.RoutingId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Routing> RoutingProperty = P<ResourceRouting>.RegisterRef(e => e.Routing, RoutingIdProperty);

        /// <summary>
        /// 工艺路线
        /// </summary>
        public Routing Routing
        {
            get { return GetRefEntity(RoutingProperty); }
            set { SetRefEntity(RoutingProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源
        /// </summary>
        [Required]
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<ResourceRouting>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<ResourceRouting>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<ResourceRouting>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 工艺路线名称 RoutingName
        /// <summary>
        /// 工艺路线名称
        /// </summary>
        [Label("工艺路线")]
        public static readonly Property<string> RoutingNameProperty = P<ResourceRouting>.RegisterView(e => e.RoutingName, p => p.Routing.Name);

        /// <summary>
        /// 注释
        /// </summary>
        public string RoutingName
        {
            get { return this.GetProperty(RoutingNameProperty); }
        }
        #endregion 
        #endregion
    }

    /// <summary>
    /// 资源工艺路线 实体配置
    /// </summary>
    internal class ResourceRoutingConfig : EntityConfig<ResourceRouting>
    {
        /// <summary>
        /// 产线工艺路线设置校验规则
        /// </summary>
        /// <param name="rules">规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new HandlerRule()
            {
                Handler = (o, e) =>
                {
                    var rr = o.CastTo<ResourceRouting>();
                    if (rr.StartDate > rr.EndDate)
                        e.BrokenDescription = "结束时间不能小于开始时间".L10N();
                }
            });
        }

        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_RESOURCE_RT").MapAllProperties();
            Meta.Property(ResourceRouting.ResourceIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}