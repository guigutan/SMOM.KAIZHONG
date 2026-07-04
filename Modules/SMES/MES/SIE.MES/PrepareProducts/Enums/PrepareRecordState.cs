using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SIE.MES.PrepareProducts.Enums
{
    /// <summary>
    /// 产前准备记录状态
    /// </summary>
    public enum PrepareRecordState
    {
        /// <summary>
        /// 待确认
        /// </summary>
        [Label("待确认")]
        [Category("Criteria")]
        ToConfirm = 0,

        /// <summary>
        /// 已确认
        /// </summary>
        [Label("已确认")]
        [Category("Criteria")]
        Confirm = 1,

        /// <summary>
        /// 全部
        /// </summary>
        [Label("全部")]
        All = 2,
    }
}
