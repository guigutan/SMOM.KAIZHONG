using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Statistics.Entities
{
    /// <summary>
    /// 资源半小时产能采集统计
    /// </summary>
    [RootEntity, Serializable]
    [Label("资源半小时产能采集统计")]
    public partial class ResourceStatisticsInfo : ResourceStatistics
    {
    }

    /// <summary>
    /// 资源半小时产能采集统计 实体配置
    /// </summary>
    internal class ResourceStatisticsInfoConfig : EntityConfig<ResourceStatisticsInfo>
    {
        /// <summary>
        /// 子类重写此方法，并完成对 Meta 属性的配置。
        /// 注意：
        /// * 为了给当前类的子类也运行同样的配置，这个方法可能会被调用多次。
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_STATS_RESOURCE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}