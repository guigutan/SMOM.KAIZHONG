using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs.Enums
{
    /// <summary>
    /// 交机确认异常情况
    /// </summary>
    public enum HandoverConfirmAbnormal
    {
        /// <summary>
		/// 原故障未解决
		/// </summary>
		[Label("原故障未解决")]
        NotResolve = 0,

        /// <summary>
        /// 新故障
        /// </summary>
        [Label("新故障")]
        NewAbnormal = 1,

    }
}
