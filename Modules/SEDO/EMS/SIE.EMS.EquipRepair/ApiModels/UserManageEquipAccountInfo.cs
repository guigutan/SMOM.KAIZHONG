using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 用户管理台账的信息
    /// </summary>
    [Serializable]
    public class UserManageEquipAccountInfo
    {
        /// <summary>
        /// 管理台账数量
        /// </summary>
        public int EquipAccountCount { get; set; }
        /// <summary>
        /// 管理故障台账数量
        /// </summary>
        public int FaultEquipAccountCount { get; set; }
    }
}
