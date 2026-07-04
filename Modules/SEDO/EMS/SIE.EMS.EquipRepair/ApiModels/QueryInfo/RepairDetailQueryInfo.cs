using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 维修单明细查询实体
    /// </summary>
    [Serializable]
    public class RepairDetailQueryInfo
    {
        /// <summary>
        /// 维修单ID
        /// </summary>
        public double RepairBillId { get; set; }

        /// <summary>
        /// 是否查询维修规程
        /// </summary>
        public bool IsQueryBillProject { get; set; }

        /// <summary>
        /// 是否查询维修工时
        /// </summary>
        public bool IsQueryWorkingHours { get; set; }

        /// <summary>
        /// 是否查询操作记录
        /// </summary>
        public bool IsQueryOperationRec { get; set; }

        /// <summary>
        /// 是否查询备件申请
        /// </summary>
        public bool IsQuerySparePartApl { get; set; }

        /// <summary>
        /// 是否查询备件更换
        /// </summary>
        public bool IsQuerySparePartChg { get; set; }

        /// <summary>
        /// 是否查询维修报告附件
        /// </summary>
        public bool IsQueryReportAttachment { get; set; }

        /// <summary>
        /// 是否查询报修附件
        /// </summary>
        public bool IsQueryApplyRepairAttachment { get; set; }
    }
}
