using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{

    /// <summary>
    /// 设备维修操作记录查询结果 信息实体
    /// </summary>
    [Serializable]
    public class RepairOperationRecResultInfo
    {
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operationer { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperationDate { get; set; }

        /// <summary>
        /// 操作类型
        /// 0:报修
        /// 1:接单
        /// 2:派工
        /// 3:转派
        /// 4:开始维修
        /// 5:维修完成
        /// 6:交机确认
        /// 7:工程确认
        /// 8:维修暂停
        /// 9:继续维修
        /// 10:取消
        /// 11:强制关单
        /// 12:维修报告
        /// </summary>
        public int OperationType { get; set; }

        /// <summary>
        /// 备注(原因)
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 交机确认结果(0:OK;1:NG)
        /// </summary>
        public int? HandoverConfirmResult { get; set; }

        /// <summary>
        /// 工程确认结果(0:未确认;1:已确认)
        /// </summary>
        public int? EngineerConfirmResult { get; set; }
    }
}
