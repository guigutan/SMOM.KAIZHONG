using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs.ViewModels
{
    /// <summary>
    /// 交机确认信息
    /// </summary>
    [Serializable]
    public class HandoverConfirmInfo
    {
        /// <summary>
        /// 设备维修单
        /// </summary>
        public EquipRepairBill repairBill { get; set; }

        /// <summary>
        /// 交机确认明细
        /// </summary>
        public EntityList<HandoverConfirmDetail> detailList { get; set; }
    }
}
