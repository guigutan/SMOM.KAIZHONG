using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Routings;
using System;

namespace SIE.MES.BatchWIP.Products
{
    /// <summary>
    /// 工艺路线布局,只有拆分出来的子批次才会记录
    /// 记录拆分子批次后的工艺路线布局，批次产品工艺路线使用
    /// </summary>
    [RootEntity, Serializable]
    [Label("批次产品运行时工艺路线布局")]
    public partial class BatchWipRTProductRoutingLayout : RoutingLayout
    {
    }

    /// <summary>
    /// 批次产品运行时工艺路线布局 实体配置
    /// </summary>
    internal class BatchWipRTProductRoutingLayoutConfig : EntityConfig<BatchWipRTProductRoutingLayout>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_RT_LAYOUT").MapAllProperties();
            Meta.EnableDiscriminator("RTBATCH");
            Meta.EnablePhantoms();
        }
    }
}