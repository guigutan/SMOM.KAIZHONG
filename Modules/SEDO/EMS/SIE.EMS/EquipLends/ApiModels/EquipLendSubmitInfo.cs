using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipLends.ApiModels
{
    /// <summary>
    /// 设备借还提交信息
    /// </summary>
    [Serializable]
    public class EquipLendSubmitInfo
    {
        /// <summary>
        /// 借还数据Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 借还单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 借还单状态 0-保存 1-借出待审核 2-已借出 3-归还待审核 4-已归还
        /// </summary>
        public int LendState { get; set; }

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double EquipAccountId { get; set; }

        /// <summary>
        /// 借机部门Id
        /// </summary>
        public double? LendEnterpriseId { get; set; }

        /// <summary>
        /// 借机人Id
        /// </summary>
        public double? LendEmployeeId { get; set; }

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double? SupplierId { get; set; }

        /// <summary>
        /// 借出对象
        /// </summary>
        public int LendObject { get; set; }

        /// <summary>
        /// 借出原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件内容
        /// </summary>
        public string Content { get; set; }
    }
}
