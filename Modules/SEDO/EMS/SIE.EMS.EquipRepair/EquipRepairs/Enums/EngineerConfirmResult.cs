using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs.Enums
{
    /// <summary>
    /// 工程确认结果
    /// </summary>
    public enum EngineerConfirmResult
    {
        /// <summary>
		/// 未确认
		/// </summary>
		[Label("未确认")]
        NotConfirm = 0,

        /// <summary>
        /// 已确认
        /// </summary>
        [Label("已确认")]
        Confirmed = 1,

    }
}
