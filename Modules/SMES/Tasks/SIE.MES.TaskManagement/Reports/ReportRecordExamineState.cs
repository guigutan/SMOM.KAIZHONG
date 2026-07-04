using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 报工记录审核状态
    /// </summary>
    public enum ReportRecordExamineState
    {
        /// <summary>
        /// 待确认
        /// </summary>
        [Label("待确认")]
        ToConfirm = 0,

        /// <summary>
        /// 已确认
        /// </summary>
        [Label("已确认")]
        Confirmed = 1,

        /// <summary>
        /// 驳回报工
        /// </summary>
        [Label("驳回报工")]
        Revoke = 2,
    }
}
