using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.EquipRepair.EquipRepairs.ViewModels
{
    /// <summary>
    /// 维修完成命令实体
    /// </summary>
    [Serializable]
    public class EquipRepairFinishCommandInfo
    {
        /// <summary>
        /// 维修单
        /// </summary>
        public EquipRepairBill EquipRepair { get; set; }

        /// <summary>
        /// 是否填写完
        /// </summary>
        public bool IsFillinReport { get; set; }
    }
}
