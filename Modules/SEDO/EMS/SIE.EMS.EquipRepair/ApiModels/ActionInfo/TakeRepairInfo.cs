using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 接单参数实体
    /// </summary>
    [Serializable]
    public class TakeRepairInfo
    {
        /// <summary>
        /// 维修单ID
        /// </summary>
        public double RepairBillId { get; set; }

        /// <summary>
        /// 维修责任人ID
        /// </summary>
        public double RepairMasterId { get; set; }

        /// <summary>
        /// 维修人员信息列表
        /// </summary>
        public List<RepairerResultInfo> Repairers { get; set; }

        /// <summary>
        /// 预计完成时间
        /// </summary>
        public DateTime? EstimateFinishDate { get; set; }

        /// <summary>
        /// 图片附件信息
        /// </summary>
        public List<RepairAttachmentInfo> PhotoInfos { get; set; } = new List<RepairAttachmentInfo>();
    }
}
