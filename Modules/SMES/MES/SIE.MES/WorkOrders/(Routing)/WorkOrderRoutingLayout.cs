using SIE.MetaModel;
using SIE.Tech.Routings;
using System;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工单工艺路线布局
    /// </summary>
    [RootEntity, Serializable]
    public partial class WorkOrderRoutingLayout : RoutingLayout
    {
    }

    /// <summary>
    /// 工单工艺路线布局 实体配置
    /// </summary>
    internal class WorkOrderRoutingLayoutConfig : EntityConfig<WorkOrderRoutingLayout>
    {
        /// <summary>
        /// 属性元数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_RT_LAYOUT").MapAllProperties();
            Meta.EnableDiscriminator("WorkOrder");
            Meta.EnablePhantoms();
        }
    }
}