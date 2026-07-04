using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Statistics.Entities
{
    /// <summary>
    /// 工序采集统计信息
    /// 手动维护库存组织
    /// </summary>
    [RootEntity, Serializable]
    [Label("工序采集统计")]
    public class ProcessStatisticsInfo : ProcessStatistics
    {
        #region 库存组织 InvOrgId
        /// <summary>
        /// 库存组织
        /// </summary>
        [Label("库存组织")]
        public static readonly Property<int?> InvOrgIdProperty = P<ProcessStatisticsInfo>.Register(e => e.InvOrgId);

        /// <summary>
        /// 库存组织
        /// </summary>
        public int? InvOrgId
        {
            get { return this.GetProperty(InvOrgIdProperty); }
            set { this.SetProperty(InvOrgIdProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工序采集统计 实体配置
    /// </summary>
    internal class ProcessStatisticsInfoConfig : EntityConfig<ProcessStatisticsInfo>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_STATS_PROCESS").MapAllProperties();
            Meta.DisableInvOrg();
        }
    }
}