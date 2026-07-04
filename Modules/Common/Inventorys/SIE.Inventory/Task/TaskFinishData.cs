using SIE.Core.Enums;
using SIE.Core.Enums;
using SIE.Inventory.Transactions;
using System.Collections.Generic;

namespace SIE.Inventory.Task
{
    /// <summary>
    /// 任务完工提交数据
    /// </summary>
    public class TaskFinishData
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TaskFinishData()
        {
            OrderType = new OrderType();
            EmployeeIdList = new List<double>();
            StockTrans = new StockTrans();
        }

        /// <summary>
        /// 单据类型
        /// </summary>
        public OrderType OrderType { get; set; }

        /// <summary>
        /// 员工列表
        /// </summary>
        public List<double> EmployeeIdList { get; set; }

        /// <summary>
        /// 单据明细ID
        /// </summary>
        public double BillDtlId { get; set; }

        /// <summary>
        /// 最终产生任务的来源ID
        /// </summary>
        public double TaskSourceId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public double LotId { get; set; }

        /// <summary>
        /// 交易双方参数
        /// </summary>
        public StockTrans StockTrans { get; set; } 
    }
}
