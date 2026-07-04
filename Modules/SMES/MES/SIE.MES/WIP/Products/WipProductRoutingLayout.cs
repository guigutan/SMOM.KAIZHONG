using SIE.MetaModel;
using SIE.Tech.Routings;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 产品工艺路线布局
    /// </summary>
    [RootEntity, Serializable]
    public partial class WipProductRoutingLayout : RoutingLayout
    {
    }

    /// <summary>
    /// 产品工艺路线布局 实体配置
    /// </summary>
    internal class WipProductRoutingLayoutConfig : EntityConfig<WipProductRoutingLayout>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_RT_LAYOUT").MapAllProperties();
            Meta.EnableDiscriminator("Product");
            Meta.EnablePhantoms();
        }
    }
}