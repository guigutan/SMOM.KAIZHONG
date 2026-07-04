using SIE.Core.Enums;
using SIE.Inventory.Transactions;
using System;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 接口上传失败统计
    /// </summary>
    [Serializable]
    public class UploadFailStatistics
    {
        /// <summary>
        /// 单据大类
        /// </summary>
        public OrderType OrderType { get; set; }

        /// <summary>
        /// 事务类型
        /// </summary>
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// 失败次数
        /// </summary>
        public int FailCount { get; set; }
    }
}
