using SIE.Inventory.Commom;
using SIE.Inventory.TransactionProcessing;
using SIE.Inventory.Transactions;
using SIE.Items;
using System;
using System.Collections.Generic;

namespace SIE.Inventory.Task
{
    /// <summary>
    /// 任务采集数据
    /// </summary>
    [Serializable]
    public class TaskCollectData
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TaskCollectData()
        {
            EmployeeIdList = new List<double>();
            StockTransList = new Dictionary<double, StockTrans>();
            BaseTransactionData = new BaseTransactionData();
        }

        /// <summary>
        /// 操作类型
        /// </summary>
        public OperationType operationType { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public TaskState? TaskState { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public Lot Lot { get; set; }

        /// <summary>
        /// 是否运行空批次
        /// </summary>
        public bool IsAllowEmptyLot { get; set; }

        /// <summary>
        /// 交易双方参数 Key:最终产生任务的来源ID，用于向上追溯 Value:交易双方参数
        /// </summary>
        public Dictionary<double, StockTrans> StockTransList { get; set; }

        /// <summary>
        /// 操作员工列表
        /// </summary>
        public List<double> EmployeeIdList { get; set; }

        /// <summary>
        /// 任务释放时间
        /// </summary>
        public DateTime? ReleaseDate { get; set; }

        /// <summary>
        /// 任务领取人
        /// </summary>
        public double? GetById { get; set; }

        /// <summary>
        /// 交易记录相关数据
        /// </summary>
        public BaseTransactionData BaseTransactionData { get; set; }

        /// <summary>
        /// 来源仓库ID
        /// </summary>
        public double? FromWarehouseId { get; set; }

        /// <summary>
        /// 目标仓库
        /// </summary>
        public double? ToWarehouseId { get; set; }

        /// <summary>
        /// 来源库区ID
        /// </summary>
        public double? FromAreaId { get; set; }

        /// <summary>
        /// 任务组
        /// </summary>
        public double? TaskGroupId { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        /// <remarks>物料扩展属性</remarks>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 二级明细号
        /// </summary>
        public string SecondLineNo { get; set; }

        /// <summary>
        /// 建议来源站台、站台组
        /// </summary>
        public string SuggestFromStation { get; set; }

        /// <summary>
        /// 实际来源站台、站台组
        /// </summary>
        public string ActualFromStation { get; set; }

        /// <summary>
        /// 建议目标站台、站台组
        /// </summary>
        public string SuggestToStation { get; set; }

        /// <summary>
        /// 实际目标站台、站台组
        /// </summary>
        public string ActualToStaion { get; set; }
    }
}
