using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ExperienceDepots.Enums
{
    /// <summary>
    /// 维修类型
    /// </summary>
    [Label("维修类型")]
    public enum ExpRepairType
    {
        /// <summary>
        /// 设备维修
        /// </summary>
        [Label("设备维修")]
        Account=0,
        /// <summary>
        /// 备件维修
        /// </summary>
        [Label("备件维修")]
        SparePart = 1,
    }
}
