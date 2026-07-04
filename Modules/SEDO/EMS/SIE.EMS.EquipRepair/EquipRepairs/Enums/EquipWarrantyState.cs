using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs.Enums
{
    /// <summary>
    /// 设备保修类型
    /// </summary>
    public enum EquipWarrantyState
    {
        /// <summary>
		/// 保修期内
		/// </summary>
		[Label("保修期内")]
        InnerWarranty = 0,

        /// <summary>
        /// 保修期外
        /// </summary>
        [Label("保修期外")]
        OuterWarranty = 1,

    }
}
