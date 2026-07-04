using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 应用经验库参数实体
    /// </summary>
    [Serializable]
    public class ApplyExpDepotInfo
    {
        /// <summary>
        /// 维修单ID
        /// </summary>
        public double RepairBillId { get; set; }

        /// <summary>
        /// 维修经验库ID
        /// </summary>
        public double ExpDepotId { get; set; }
    }
}
