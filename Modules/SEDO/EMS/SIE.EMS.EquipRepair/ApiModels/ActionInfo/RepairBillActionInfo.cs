using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 设备维修单操作信息
    /// </summary>
    [Serializable]
    public class RepairBillActionInfo
    {
        /// <summary>
        /// 维修单ID
        /// </summary>
        public double RepairId { get; set; }

        /// <summary>
        /// 生产状态(0:停机，1:生产)
        /// </summary>
        public int ProduceState { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
