using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Checks.ApiModels
{
    /// <summary>
    /// EDO首页数据
    /// </summary>
    [Serializable]
    public class EdoFrontPageInfo
    {
        /// <summary>
        /// 设备总数
        /// </summary>
        public int EquipQty { get; set; }

        /// <summary>
        /// 设备故障数
        /// </summary>
        public int EquipErrorQty { get; set; }

        /// <summary>
        /// 待处理任务数
        /// </summary>
        public int PendingTaskQty { get; set; }

        /// <summary>
        /// 处理中任务数
        /// </summary>
        public int ProcessingTaskQty { get; set; }

        /// <summary>
        /// 已完成任务数
        /// </summary>
        public int CompletedTaskQty { get; set; }
    }
}
