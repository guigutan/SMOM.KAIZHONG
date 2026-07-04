using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Tech.Routings
{
    /// <summary>
    /// 工艺路线布局
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("工艺路线布局")]
    public partial class RoutingLayout : DataEntity
    {
        #region 工艺流程 Layout
        /// <summary>
        /// 工艺流程
        /// </summary>
        public static readonly Property<string> LayoutProperty = P<RoutingLayout>.Register(e => e.Layout);

        /// <summary>
        /// 工艺流程
        /// </summary>
        public string Layout
        {
            get { return GetProperty(LayoutProperty); }
            set { SetProperty(LayoutProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工艺路线布局 实体配置
    /// </summary>
    internal class RoutingLayoutConfig : EntityConfig<RoutingLayout>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_RT_LAYOUT").MapAllProperties();
            Meta.Property(RoutingLayout.LayoutProperty).ColumnMeta.HasLength("MAX");
            Meta.EnableDiscriminator("Routing");
            Meta.EnablePhantoms();
        }
    }
}