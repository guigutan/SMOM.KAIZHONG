using SIE.EMS.EquipRepair.EquipRepairs.Enums;
using SIE.EMS.EquipRepairs.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 维修PDA统计信息
    /// </summary>
    [Serializable]
    public class RepairPDACountInfo
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public RepairOperationType Type { get; set; }

        /// <summary>
        /// 维修状态
        /// </summary>
        public EquipRepairState State { get; set; }

        /// <summary>
        /// 统计数量
        /// </summary>
        public int Count { get; set; }
    }

    /// <summary>
    /// 状态统计
    /// </summary>
    [Serializable]
    public class StateCount
    {
        /// <summary>
        /// 维修状态
        /// </summary>
        public EquipRepairState State { get; set; }

        /// <summary>
        /// 统计数量
        /// </summary>
        public int Count { get; set; }
    }
}
