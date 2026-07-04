using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipLends.Enums
{
    /// <summary>
    /// 审核类型
    /// </summary>
    public enum ExamineType
    {
        /// <summary>
        /// 借机
        /// </summary>
        [Label("借机")]
        Lend = 0,

        /// <summary>
        /// 归还
        /// </summary>
        [Label("归还")]
        Return = 1,
    }
}
