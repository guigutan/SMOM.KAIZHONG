using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs.Enums
{
    /// <summary>
    /// 送修方式
    /// </summary>
    public enum SendRepairWay
    {
        /// <summary>
		/// 厂外维修
		/// </summary>
		[Label("厂外维修")]
        OutSideRepair = 0,

        /// <summary>
        /// 现场维修
        /// </summary>
        [Label("现场维修")]
        SceneRepair = 1,

    }
}
