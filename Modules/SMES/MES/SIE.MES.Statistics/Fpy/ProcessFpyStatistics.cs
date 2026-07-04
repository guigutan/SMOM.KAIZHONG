using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Statistics.Fpy
{
    /// <summary>
    /// 工序直通率统计
    /// </summary>
    [RootEntity, Serializable]
    [Label("工序直通率统计")]
    public partial class ProcessFpyStatistics : FpyStatistics
    {
        #region 工序ID ProcessId
        /// <summary>
        /// 工序ID
        /// </summary>
        [Label("工序ID")]
        public static readonly Property<double> ProcessIdProperty = P<ProcessFpyStatistics>.Register(e => e.ProcessId);

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId
        {
            get { return GetProperty(ProcessIdProperty); }
            set { SetProperty(ProcessIdProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<ProcessFpyStatistics>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return GetProperty(ProcessNameProperty); }
            set { SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 库存组织 InvOrgId
        /// <summary>
        /// 库存组织
        /// </summary>
        [Label("库存组织")]
        public static readonly Property<int?> InvOrgIdProperty = P<ProcessFpyStatistics>.Register(e => e.InvOrgId);

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
    /// 工序直通率统计 实体配置
    /// </summary>
    internal class ProcessFpyStatisticsConfig : EntityConfig<ProcessFpyStatistics>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_STATS_FPY").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.DisableInvOrg();
            Meta.Property(ProcessFpyStatistics.WorkOrderIdProperty).ColumnMeta.HasIndex();
        }
    }
}