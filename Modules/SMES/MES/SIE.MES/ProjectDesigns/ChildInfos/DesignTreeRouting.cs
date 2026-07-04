using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Routings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.ChildInfos
{
    /// <summary>
    /// 工艺资料-产品工艺路线
    /// </summary>
    [RootEntity, Serializable]
    [Label("工艺资料-产品工艺路线")]
    public class DesignTreeRouting : DesignProduct
    {
        /// <summary>
        /// 构造
        /// </summary>
        public DesignTreeRouting()
        {
            StartDate = DateTime.Now;
        }

        #region 需求设计 ProjectDesign
        /// <summary>
        /// 需求设计Id
        /// </summary>
        [Label("需求设计")]
        public static readonly IRefIdProperty ProjectDesignIdProperty =
            P<DesignTreeRouting>.RegisterRefId(e => e.ProjectDesignId, ReferenceType.Normal);

        /// <summary>
        /// 需求设计Id
        /// </summary>
        public double ProjectDesignId
        {
            get { return (double)this.GetRefId(ProjectDesignIdProperty); }
            set { this.SetRefId(ProjectDesignIdProperty, value); }
        }

        /// <summary>
        /// 需求设计
        /// </summary>
        public static readonly RefEntityProperty<ProjectDesignDetail> ProjectDesignProperty =
            P<DesignTreeRouting>.RegisterRef(e => e.ProjectDesign, ProjectDesignIdProperty);

        /// <summary>
        /// 需求设计
        /// </summary>
        public ProjectDesignDetail ProjectDesign
        {
            get { return this.GetRefEntity(ProjectDesignProperty); }
            set { this.SetRefEntity(ProjectDesignProperty, value); }
        }
        #endregion

        #region 类型 OrderType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<WorkOrderType?> OrderTypeProperty = P<DesignTreeRouting>.Register(e => e.OrderType);

        /// <summary>
        /// 类型
        /// </summary>
        public WorkOrderType? OrderType
        {
            get { return this.GetProperty(OrderTypeProperty); }
            set { this.SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 版本 RoutingVersion
        /// <summary>
        /// 版本Id
        /// </summary>
        [Label("版本")]
        public static readonly IRefIdProperty RoutingVersionIdProperty =
            P<DesignTreeRouting>.RegisterRefId(e => e.RoutingVersionId, ReferenceType.Normal);

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
            P<DesignTreeRouting>.RegisterRef(e => e.RoutingVersion, RoutingVersionIdProperty);

        /// <summary>
        /// 版本
        /// </summary>
        public RoutingVersion RoutingVersion
        {
            get { return this.GetRefEntity(RoutingVersionProperty); }
            set { this.SetRefEntity(RoutingVersionProperty, value); }
        }
        #endregion

        #region 工艺路线 Routing
        /// <summary>
        /// 工艺路线Id
        /// </summary>
        [Label("工艺路线")]
        public static readonly IRefIdProperty RoutingIdProperty =
            P<DesignTreeRouting>.RegisterRefId(e => e.RoutingId, ReferenceType.Normal);

        /// <summary>
        /// 工艺路线Id
        /// </summary>
        public double? RoutingId
        {
            get { return (double?)this.GetRefNullableId(RoutingIdProperty); }
            set { this.SetRefNullableId(RoutingIdProperty, value); }
        }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public static readonly RefEntityProperty<Routing> RoutingProperty =
            P<DesignTreeRouting>.RegisterRef(e => e.Routing, RoutingIdProperty);

        /// <summary>
        /// 工艺路线
        /// </summary>
        public Routing Routing
        {
            get { return this.GetRefEntity(RoutingProperty); }
            set { this.SetRefEntity(RoutingProperty, value); }
        }
        #endregion

        #region 开始日期 StartDate
        /// <summary>
        /// 开始日期
        /// </summary>
        [Label("开始日期")]
        public static readonly Property<DateTime?> StartDateProperty = P<DesignTreeRouting>.Register(e => e.StartDate);

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartDate
        {
            get { return this.GetProperty(StartDateProperty); }
            set { this.SetProperty(StartDateProperty, value); }
        }
        #endregion

        #region 结束日期 EndDate
        /// <summary>
        /// 结束日期
        /// </summary>
        [Label("结束日期")]
        public static readonly Property<DateTime?> EndDateProperty = P<DesignTreeRouting>.Register(e => e.EndDate, new PropertyMetadata<DateTime?>() { DateTimePart = DateTimePart.Date });

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate
        {
            get { return this.GetProperty(EndDateProperty); }
            set { this.SetProperty(EndDateProperty, value); }
        }
        #endregion

        #region 是否存在更新工序清单 HasRoutingDetail
        /// <summary>
        /// 是否存在更新工序清单
        /// </summary>
        [Label("是否存在更新工序清单")]
        public static readonly Property<bool?> HasRoutingDetailProperty = P<DesignTreeRouting>.Register(e => e.HasRoutingDetail);

        /// <summary>
        /// 是否存在更新工序清单
        /// </summary>
        public bool? HasRoutingDetail
        {
            get { return this.GetProperty(HasRoutingDetailProperty); }
            set { this.SetProperty(HasRoutingDetailProperty, value); }
        }
        #endregion

        #region 是否已更新 HasUp
        /// <summary>
        /// 是否已更新
        /// </summary>
        [Label("是否已更新")]
        public static readonly Property<bool?> HasUpProperty = P<DesignTreeRouting>.Register(e => e.HasUp);

        /// <summary>
        /// 是否已更新
        /// </summary>
        public bool? HasUp
        {
            get { return this.GetProperty(HasUpProperty); }
            set { this.SetProperty(HasUpProperty, value); }
        }
        #endregion

        #region 产品工艺路线明细 RoutingDetailList
        /// <summary>
        /// 产品工艺路线明细
        /// </summary>
        [Label("产品工艺路线明细")]
        public static readonly ListProperty<EntityList<DesignTreeRoutingDetail>> RoutingDetailListProperty = P<DesignTreeRouting>.RegisterList(e => e.RoutingDetailList);

        /// <summary>
        /// 产品工艺路线明细
        /// </summary>
        public EntityList<DesignTreeRoutingDetail> RoutingDetailList
        {
            get { return this.GetLazyList(RoutingDetailListProperty); }
        }
        #endregion

        #region 工序BOM明细 RoutingProBomList
        /// <summary>
        /// 工序BOM明细
        /// </summary>
        [Label("工序BOM明细")]
        public static readonly ListProperty<EntityList<DesignTreeRoutingProBom>> RoutingProBomListProperty = P<DesignTreeRouting>.RegisterList(e => e.RoutingProBomList);

        /// <summary>
        /// 工序BOM明细
        /// </summary>
        public EntityList<DesignTreeRoutingProBom> RoutingProBomList
        {
            get { return this.GetLazyList(RoutingProBomListProperty); }
        }
        #endregion

        #region 参数明细 RoutingParamList
        /// <summary>
        /// 参数明细
        /// </summary>
        [Label("参数明细")]
        public static readonly ListProperty<EntityList<DesignTreeRoutingParamer>> RoutingParamListProperty = P<DesignTreeRouting>.RegisterList(e => e.RoutingParamList);

        /// <summary>
        /// 参数明细
        /// </summary>
        public EntityList<DesignTreeRoutingParamer> RoutingParamList
        {
            get { return this.GetLazyList(RoutingParamListProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class DesignTreeRoutingConfig : EntityConfig<DesignTreeRouting>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_PRODES_PROTREE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
