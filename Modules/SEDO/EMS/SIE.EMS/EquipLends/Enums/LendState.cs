using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipLends.Enums
{
    /// <summary>
    /// 借还状态
    /// </summary>
    public enum LendState
    {
        /// <summary>
        /// 保存
        /// </summary>
        [Label("保存")]
        Saved = 0,

        /// <summary>
        /// 借出待审核
        /// </summary>
        [Label("借出待审核")]
        LendExamine = 1,

        /// <summary>
        /// 已借出
        /// </summary>
        [Label("已借出")]
        HasLended = 2,

        /// <summary>
        /// 归还待审核
        /// </summary>
        [Label("归还待审核")]
        ReturnExamine = 3,

        /// <summary>
        /// 已归还
        /// </summary>
        [Label("已归还")]
        HasReturned = 4,
    }
}
