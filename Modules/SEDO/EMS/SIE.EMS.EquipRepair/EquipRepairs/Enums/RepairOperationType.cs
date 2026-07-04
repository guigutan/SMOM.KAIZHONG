using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs.Enums
{
    /// <summary>
    /// 设备维修操作类型
    /// </summary>
    public enum RepairOperationType
    {
        /// <summary>
        /// 报修
        /// </summary>
        [Label("报修")]
        ApplyRepair = 0,

        /// <summary>
        /// 接单
        /// </summary>
        [Label("接单")]
        Take = 1,

        /// <summary>
        /// 派工
        /// </summary>
        [Label("派工")]
        Dispatch = 2,

        /// <summary>
        /// 转派
        /// </summary>
        [Label("转派")]
        Transfer = 3,

        /// <summary>
        /// 开始维修
        /// </summary>
        [Label("开始维修")]
        Begin = 4,

        /// <summary>
        /// 维修完成
        /// </summary>
        [Label("维修完成")]
        Completed = 5,

        /// <summary>
        /// 交机确认
        /// </summary>
        [Label("交机确认")]
        HandoverConfirm = 6,

        /// <summary>
        /// 工程确认
        /// </summary>
        [Label("工程确认")]
        EngineerConfirm = 7,

        /// <summary>
        /// 维修暂停
        /// </summary>
        [Label("维修暂停")]
        Pause = 8,

        /// <summary>
        /// 继续维修
        /// </summary>
        [Label("继续维修")]
        Continue = 9,

        /// <summary>
        /// 取消
        /// </summary>
        [Label("取消")]
        Cancel = 10,

        /// <summary>
        /// 强制关单
        /// </summary>
        [Label("强制关单")]
        CompelClose = 11,

        /// <summary>
        /// 维修报告
        /// </summary>
        [Label("维修报告")]
        Report = 12,
    }
}
