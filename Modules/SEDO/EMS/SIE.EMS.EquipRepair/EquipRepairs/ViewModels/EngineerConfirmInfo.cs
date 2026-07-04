using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs.ViewModels
{
    /// <summary>
    /// 工程确认信息
    /// </summary>
    [Serializable]
    public class EngineerConfirmInfo
    {
        /// <summary>
        /// 设备维修单
        /// </summary>
        public EquipRepairBill repairBill { get; set; }

        /// <summary>
        /// 工程确认明细
        /// </summary>
        public EntityList<EngineerConfirmDetail> detailList { get; set; }
    }
}
