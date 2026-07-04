using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.ViewModels.Enums
{
    /// <summary>
    /// 保修期类型
    /// </summary>
    [Label("保修期类型")]
    public enum GuaranteeRangeType
    {
        /// <summary>
        /// 保修期内
        /// </summary>
        [Label("保修期内")]
        GuaranteeRangeIn = 1,
        /// <summary>
        /// 保修期外
        /// </summary>
        [Label("保修期外")]
        GuaranteeRangeOut = 2
    }
}
