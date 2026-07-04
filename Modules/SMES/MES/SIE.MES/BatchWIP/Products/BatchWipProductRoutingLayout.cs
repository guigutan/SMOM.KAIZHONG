using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Routings;
using System;

namespace SIE.MES.BatchWIP.Products
{
    /// <summary>
    /// 批次产品工艺路线布局
    /// </summary>
    [RootEntity, Serializable]
    [Label("批次产品工艺路线布局")]
    public partial class BatchWipProductRoutingLayout : RoutingLayout
    {
    }

    /// <summary>
    /// 批次产品工艺路线布局 实体配置
    /// </summary>
    internal class BatchWipProductRoutingLayoutConfig : EntityConfig<BatchWipProductRoutingLayout>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_RT_LAYOUT").MapAllProperties();
            Meta.EnableDiscriminator("BATCH");
            Meta.EnablePhantoms();
        }
    }
}