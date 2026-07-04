using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs.Enums
{
    /// <summary>
 /// 维修等级
 /// </summary>
    public enum RepairLevel
    {/// <summary>
     /// 大修
     /// </summary>
        [Label("大修")]
        Big = 0,
        /// <summary>
        /// 中修
        /// </summary>
        [Label("中修")]
        Medium = 1,
        /// <summary>
        /// 小修
        /// </summary>
        [Label("小修")]
        Large = 2,
    }
}
