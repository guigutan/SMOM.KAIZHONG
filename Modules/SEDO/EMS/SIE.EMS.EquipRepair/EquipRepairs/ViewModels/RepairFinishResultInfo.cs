using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs.ViewModels
{
    /// <summary>
    /// 设备维修完成返回信息
    /// </summary>
    [Serializable]
    public class RepairFinishResultInfo
    {
        /// <summary>
        /// 是否工程确认
        /// </summary>
        public bool isEngineerConfirm { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }
    }
}
