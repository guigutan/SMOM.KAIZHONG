using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs.Enums
{
    /// <summary>
    /// 评分项
    /// </summary>
    public enum EquipRepairScore
    {
        /// <summary>
		/// 5分
		/// </summary>
		[Label("5分")]
        Five = 5,

        /// <summary>
        /// 4分
        /// </summary>
        [Label("4分")]
        Four = 4,

        /// <summary>
        /// 3分
        /// </summary>
        [Label("3分")]
        Three = 3,

        /// <summary>
        /// 2分
        /// </summary>
        [Label("2分")]
        Two = 2,

        /// <summary>
        /// 1分
        /// </summary>
        [Label("1分")]
        One = 1

    }
}
