using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.ViewModels.Enums
{
    /// <summary>
    /// 派工类型
    /// </summary>
    [Label("派工类型")]
    public enum DispatchType
    {
        /// <summary>
        /// 内修
        /// </summary>
        [Label("内修")]
        RepairIn = 1,
        /// <summary>
        /// 外修
        /// </summary>
        [Label("外修")]
        RepairOut = 2,
    }
}
