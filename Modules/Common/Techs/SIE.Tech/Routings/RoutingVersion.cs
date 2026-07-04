using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Tech.Routings
{
    /// <summary>
    /// 工艺路线版本
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工艺路线版本")]
    [DisplayMember(nameof(Name))]
    public partial class RoutingVersion : DataEntity
    {
        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(40)]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<RoutingVersion>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 生效时间 EffectiveDate
        /// <summary>
        /// 生效时间
        /// </summary>
        [Label("生效时间")]
        public static readonly Property<DateTime> EffectiveDateProperty = P<RoutingVersion>.Register(e => e.EffectiveDate);

        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime EffectiveDate
        {
            get { return GetProperty(EffectiveDateProperty); }
            set { SetProperty(EffectiveDateProperty, value); }
        }
        #endregion

        #region 引用数量 ReferenceQty
        /// <summary>
        /// 引用数量
        /// </summary>
        [Label("引用数量")]
        public static readonly Property<int> ReferenceQtyProperty = P<RoutingVersion>.Register(e => e.ReferenceQty);

        /// <summary>
        /// 引用数量
        /// </summary>
        public int ReferenceQty
        {
            get { return GetProperty(ReferenceQtyProperty); }
            set { SetProperty(ReferenceQtyProperty, value); }
        }
        #endregion

        #region 是否默认 IsDefault
        /// <summary>
        /// 是否默认
        /// </summary>
        [Label("是否默认")]
        public static readonly Property<YesNo> IsDefaultProperty = P<RoutingVersion>.Register(e => e.IsDefault);

        /// <summary>
        /// 是否默认
        /// </summary>
        public YesNo IsDefault
        {
            get { return GetProperty(IsDefaultProperty); }
            set { SetProperty(IsDefaultProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<RoutingState> StateProperty = P<RoutingVersion>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public RoutingState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 工艺路线布局 Layout
        /// <summary>
        /// 工艺路线布局Id
        /// </summary>
        [Label("工艺路线布局")]
        public static readonly IRefIdProperty LayoutIdProperty = P<RoutingVersion>.RegisterRefId(e => e.LayoutId, ReferenceType.Normal);

        /// <summary>
        /// 工艺路线布局Id
        /// </summary>
        public double? LayoutId
        {
            get { return (double?)GetRefNullableId(LayoutIdProperty); }
            set { SetRefNullableId(LayoutIdProperty, value); }
        }

        /// <summary>
        /// 工艺路线布局
        /// </summary>
        public static readonly RefEntityProperty<RoutingLayout> LayoutProperty = P<RoutingVersion>.RegisterRef(e => e.Layout, LayoutIdProperty);

        /// <summary>
        /// 工艺路线布局
        /// </summary>
        public RoutingLayout Layout
        {
            get { return GetRefEntity(LayoutProperty); }
            set { SetRefEntity(LayoutProperty, value); }
        }
        #endregion

        #region 工序清单列表 ProcessList
        /// <summary>
        /// 工序清单列表
        /// </summary>
        public static readonly ListProperty<EntityList<RoutingProcess>> ProcessListProperty = P<RoutingVersion>.RegisterList(e => e.ProcessList);

        /// <summary>
        /// 工序清单列表
        /// </summary>
        public EntityList<RoutingProcess> ProcessList
        {
            get { return this.GetLazyList(ProcessListProperty); }
        }
        #endregion

        #region 工艺路线 Routing
        /// <summary>
        /// 工艺路线Id
        /// </summary>
        [Label("工艺路线")]
        public static readonly IRefIdProperty RoutingIdProperty = P<RoutingVersion>.RegisterRefId(e => e.RoutingId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<Routing> RoutingProperty = P<RoutingVersion>.RegisterRef(e => e.Routing, RoutingIdProperty);

        /// <summary>
        /// 工艺路线
        /// </summary>
        public Routing Routing
        {
            get { return GetRefEntity(RoutingProperty); }
            set { SetRefEntity(RoutingProperty, value); }
        }
        #endregion

        #region 工艺路线名称 RoutingName
        /// <summary>
        /// 工艺路线名称
        /// </summary>
        [Label("工艺路线")]
        public static readonly Property<string> RoutingNameViewProperty = P<RoutingVersion>.RegisterView(e => e.RoutingName, e => e.Routing.Name);

        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public string RoutingName
        {
            get { return GetProperty(RoutingNameViewProperty); }
            set { SetProperty(RoutingNameViewProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工艺路线版本 实体配置
    /// </summary>
    internal class RoutingVersionConfig : EntityConfig<RoutingVersion>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_RT_VERSION").MapAllProperties();
            Meta.Property(RoutingVersion.RoutingIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}