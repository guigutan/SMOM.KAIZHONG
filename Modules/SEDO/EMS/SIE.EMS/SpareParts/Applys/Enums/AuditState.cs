using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SIE.EMS.SpareParts.Applys.Enums
{
    /// <summary>
    /// 审核状态
    /// </summary>
    [Label("审核状态")]
    public enum AuditState
    {
        /// <summary>
        /// 创建
        /// </summary>
        [Label("创建")]
        Create = 0,
        /// <summary>
        /// 待审核
        /// </summary>
        [Label("待审核")]
        StandAudit = 1,
        /// <summary>
        /// 已驳回
        /// </summary>
        [Label("已驳回")]
        Returned = 2,

        /// <summary>
        /// 待出库
        /// </summary>
        [Label("待出库")]
        Butbound = 3,
        /// <summary>
        /// 已出库
        /// </summary>
        [Label("已出库")]
        Butbounded = 4,
        /// <summary>
        /// 部分出库
        /// </summary>
        [Label("部分出库")]
        PartButbound = 5,
        /// <summary>
        /// 取消
        /// </summary>
        [Label("取消")]
        Cancel = 6,
    }
}