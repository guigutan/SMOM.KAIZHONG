using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.AndonStatisticsReports
{
    /// <summary>
    /// 分组层级
    /// </summary>
    public enum GroupLevel
    {
        /// <summary>
        ///工厂
        /// </summary>
        [Label("工厂")]
        Factory,
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        Workshop,

        /// <summary>
        /// 产线
        /// </summary>
        [Label("产线")]
        ProductionLine,
        /// <summary>
        /// 设备
        /// </summary>
        [Label("设备")]
        Equipment,
        /// <summary>
        /// 部门
        /// </summary>
        [Label("部门")]
        Department,

        /// <summary>
        /// 产品
        /// </summary>
        [Label("产品")]
        Product,

        /// <summary>
        /// 触发人
        /// </summary>
        [Label("触发人")]
        Trigger
    }
}
