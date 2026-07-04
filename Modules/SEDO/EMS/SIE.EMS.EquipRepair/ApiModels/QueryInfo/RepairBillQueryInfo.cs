using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 维修单查询实体
    /// </summary>
    [Serializable]
    public class RepairBillQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 维修状态
        /// 0:(ApplyRepair, 报修)
        /// 1:(WaitRepair, 待维修)
        /// 2:(Repairing, 维修中)
        /// 3:(WaitConfirm, 待确认)
        /// 4:(WaitScore, 待评分)
        /// 5:(Completed, 已完成)
        /// 6:(Suspending, 暂停中)
        /// 7:(Cancel, 取消)
        /// 8:(Closed, 关闭)
        /// </summary>
        public List<int> RepairStates { get; set; } = new List<int>();

        /// <summary>
        /// 维修类型
        /// 0:(EquipRepair, 设备维修)
        /// 1:(SparePartRepair, 备件维修)
        /// </summary>
        public int? EquipRepairType { get; set; }

        /// <summary>
        /// 操作类型
        /// 0:报修
        /// 1:接单
        /// 2:派工
        /// 4:执行
        /// 6:交机确认
        /// 7:工程确认
        /// </summary>
        public int Action { get; set; }
    }
}
