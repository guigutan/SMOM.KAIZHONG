using MimeKit.Encodings;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipLends.Enums
{
    /// <summary>
    /// 审核结果
    /// </summary>
    public enum ExamineResult
    {
        /// <summary>
        /// 通过
        /// </summary>
        [Label("通过")]
        Pass = 0,

        /// <summary>
        /// 驳回
        /// </summary>
        [Label("驳回")]
        Reject = 1,
    }
}
