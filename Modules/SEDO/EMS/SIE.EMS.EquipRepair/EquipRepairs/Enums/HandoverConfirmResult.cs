using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs.Enums
{
    /// <summary>
    /// 交机确认结果
    /// </summary>
    public enum HandoverConfirmResult
    {
        /// <summary>
		/// OK
		/// </summary>
		[Label("OK")]
        OK = 0,

        /// <summary>
        /// NG
        /// </summary>
        [Label("NG")]
        NG = 1,

    }
}
