using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 转派参数实体
    /// </summary>
    [Serializable]
    public class TransfeRepairInfo : DispatchRepairInfo
    {
        /// <summary>
        /// 转派原因
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 原维修责任人ID
        /// </summary>
        public double OriginalRepairMasterId { get; set; }

        /// <summary>
        /// 原维修人员信息列表
        /// </summary>
        public List<RepairerResultInfo> OriginalRepairers { get; set; }
    }
}
