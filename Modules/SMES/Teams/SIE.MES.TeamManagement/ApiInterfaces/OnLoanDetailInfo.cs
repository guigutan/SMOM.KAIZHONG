using SIE.MES.TeamManagement.OnLoans;
using System;

namespace SIE.MES.TeamManagement.ApiInterfaces
{
    /// <summary>
    /// 借调单明细API信息类
    /// </summary>
    [Serializable]
    public class OnLoanDetailInfo
    {
        /// <summary>
        /// 行号
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 操作人姓名
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 审核状态(发起-同意-拒绝-修改-撤销-审核中-修改中)
        /// </summary>
        public string ApprovalState { get; set; }

        /// <summary>
        /// 审核状态(0-1-2-3-4-5-6)
        /// </summary>
        public ApprovalState ApprovalStateValue { get; set; }
    }
}
