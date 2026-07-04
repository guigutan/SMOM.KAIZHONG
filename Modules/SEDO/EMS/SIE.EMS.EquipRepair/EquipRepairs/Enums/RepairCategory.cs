using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs.Enums
{
    /// <summary>
    /// 维修等级
    /// </summary>
    public enum RepairCategory
    {
        /// <summary>
        /// 常见故障维修
        /// </summary>
        [Label("常见故障维修")]
        Familiar = 0,
        /// <summary>
        /// 一般故障维修
        /// </summary>
        [Label("一般故障维修")]
        Ordinary = 1,
        /// <summary>
        /// 突发故障维修
        /// </summary>
        [Label("突发故障维修")]
        Sudden = 2,
        /// <summary>
        /// 不正当使用故障维修
        /// </summary>
        [Label("不正当使用故障维修")]
        ImproperUse= 3,
        /// <summary>
        /// 计划性维修
        /// </summary>
        [Label("计划性维修")]
        Plan = 4,
        /// <summary>
        /// 预防性维修
        /// </summary>
        [Label("预防性维修")]
        Prevent = 5,

    }
}
