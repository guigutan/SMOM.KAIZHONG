using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.MES.Workbench.KeyPerformances
{
    /// <summary>
    /// 目标值设置类型
    /// </summary>
    public enum TargetSettingType
    {
        #region 效率类
        /// <summary>
        /// 生产计划达成率
        /// </summary>
        [Category("EFF")]
        [Label("车间计划达成率")]
        PlanReachedPercent,

        /// <summary>
        /// 生产直通率
        /// </summary>
        [Category("EFF")]
        [Label("生产直通率")]
        ProductFpy,

        /// <summary>
        /// OEE效率
        /// </summary>
        [Category("EFF")]
        [Label("OEE效率")]
        OEEfficiency,

        /// <summary>
        /// OPE效率
        /// </summary>
        [Category("EFF")]
        [Label("OPE效率")]
        OPEfficiency,

        /// <summary>
        /// MES工单完工率
        /// </summary>
        [Category("EFF")]
        [Label("MES工单完工率")]
        WorkOrderComplete,
        #endregion

        #region 品质类
        /// <summary>
        /// 来料批次合格率
        /// </summary>
        [Category("QMS")]
        [Label("来料批次合格率")]
        IQCBatch,

        /// <summary>
        /// 来料上线质量合格率
        /// </summary>
        [Category("QMS")]
        [Label("来料上线质量合格率")]
        IQCAssembly,

        /// <summary>
        /// 首检批次合格率
        /// </summary>
        [Category("QMS")]
        [Label("首检批次合格率")]
        FirstInspBatch,

        /// <summary>
        /// 品质直通率
        /// </summary>
        [Category("QMS")]
        [Label("品质直通率")]
        QualityFpy,

        /// <summary>
        /// 成品批次合格率
        /// </summary>
        [Category("QMS")]
        [Label("成品批次合格率")]
        ShipInspBatch,

        /// <summary>
        /// 报废率
        /// </summary>
        [Category("QMS")]
        [Label("报废率")]
        Scrap,

        /// <summary>
        /// 产品市场维修率
        /// </summary>
        [Category("QMS")]
        [Label("产品市场维修率")]
        ProductRepair,

        /// <summary>
        /// 质量问题改善关闭率
        /// </summary>
        [Category("QMS")]
        [Label("质量问题改善关闭率")]
        QualityProblemRepaired,
        #endregion

        #region 成本类
        /// <summary>
        /// 物料超耗率
        /// </summary>
        [Category("COST")]
        [Label("物料超耗率")]
        ItemOverConsumed,

        /// <summary>
        /// 库存周转率
        /// </summary>
        [Category("COST")]
        [Label("库存周转率")]
        InvTurnOver,

        /// <summary>
        /// 加班工时
        /// </summary>
        [Category("COST")]
        [Label("加班工时")]
        HoursOfOT,
        #endregion

        //TODO: 交期类、人员类
    }
}
