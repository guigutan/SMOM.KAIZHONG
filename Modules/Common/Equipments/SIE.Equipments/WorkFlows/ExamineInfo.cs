using SIE.Equipments.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Equipments.WorkFlows
{
    /// <summary>
    /// 审核信息
    /// </summary>
    [Serializable]
    public class ExamineInfo
    {
        /// <summary>
        /// 审核结果
        /// </summary>
        public ApprovalResult? ApprovalResult { get; set; }

        /// <summary>
        /// 审核意见
        /// </summary>
        public string Remark { get; set; }
    }
}
