using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs.Enums
{
    /// <summary>
    /// 故障等级
    /// </summary>
    public enum FaultLevel
    {
        /// <summary>
        /// 致命
        /// </summary>
        [Label("致命")]
        Deadly = 0,
        /// <summary>
        /// 严重
        /// </summary>
        [Label("严重")]
        Serious = 1,
        /// <summary>
        /// 轻微
        /// </summary>
        [Label("轻微")]
        Slight = 2,
    }
}
