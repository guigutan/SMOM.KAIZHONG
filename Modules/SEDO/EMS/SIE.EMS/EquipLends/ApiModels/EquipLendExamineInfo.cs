using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipLends.ApiModels
{
    /// <summary>
    /// 设备借还审核信息
    /// </summary>
    [Serializable]
    public class EquipLendExamineInfo
    {
        /// <summary>
        /// 审核结果 10-通过 20-驳回
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// 审核意见
        /// </summary>
        public string Remark { get; set; }
    }
}
