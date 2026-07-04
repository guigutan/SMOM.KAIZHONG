using SIE.Inventory.Onhands;
using System;
using System.Collections.Generic;

namespace SIE.Inventory.Task
{
    /// <summary>
    /// 任务API交互信息
    /// </summary>
    public class TaskData
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public double TaskId { get; set; }

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 任务释放时间
        /// </summary>
        public string ReleaseDate { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 物料id
        /// </summary>
        public double? ItemId { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SpecificationModel { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitCode { get; set; }

        /// <summary>
        /// 辅单位
        /// </summary>
        public string SecondUnitName { get; set; }

        /// <summary>
        /// 包装
        /// </summary>
        public string PackRuleCode { get; set; }

        /// <summary>
        /// 包装规则名称
        /// </summary>
        public string PackRuleName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 数量（辅单位）
        /// </summary>
        public decimal SecondQty { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string Lot { get; set; }

        /// <summary>
        /// 源库位编码
        /// </summary>
        public string FromLocCode { get; set; }

        /// <summary>
        /// 源库位Id
        /// </summary>
        public double? FromLocId { get; set; }

        /// <summary>
        /// 源LPN
        /// </summary>
        private string fromLpn;

        /// <summary>
        /// 源LPN
        /// </summary>
        public string FromLpn
        {
            get
            {
                return fromLpn.IsNullOrEmpty() ? "*" : fromLpn;
            }
            set { fromLpn = value; }
        }

        /// <summary>
        /// 建议目标库位编码
        /// </summary>
        public string ToLocCode { get; set; }

        /// <summary>
        /// 建议目标LPN
        /// </summary>
        private string toLpn;

        /// <summary>
        /// 建议目标LPN
        /// </summary>
        public string ToLpn
        {
            get
            {
                return toLpn.IsNullOrEmpty() ? "*" : toLpn;
            }
            set { toLpn = value; }
        }

        /// <summary>
        /// 实际来源库位
        /// </summary>
        public string ActualFromLoc { get; set; }

        /// <summary>
        /// 实际来源LPN
        /// </summary>
        public string ActualFromLpn { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public string LevelLabel { get; set; }

        /// <summary>
        /// 单据Id
        /// </summary>
        public double BillId { get; set; }

        /// <summary>
        /// 单据号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 单据明细Id
        /// </summary>
        public double BillDtlId { get; set; }

        /// <summary>
        /// 单据明细行号
        /// </summary>
        public string BillDtlLineNo { get; set; }

        /// <summary>
        /// 二级明细号
        /// </summary>
        public string SecondBillDtlNo { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public TaskState TaskState { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 货主
        /// </summary>
        public string StorerCode { get; set; }

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 任务号
        /// </summary>
        public string Task_No { get; set; }

        /// <summary>
        /// 库存状态
        /// </summary>
        public OnhandState? OnhandState { get; set; }

        /// <summary>
        /// 库存状态
        /// </summary>
        public string OnhandStateStr { get; set; }

        /// <summary>
        /// 已经完工的任务
        /// </summary>
        public bool IsFinishTask { get; set; }

        /// <summary>
        /// 拣货分配的库存
        /// </summary>
        public double? PickOnhandId { get; set; }

        /// <summary>
        /// 是否按包装分配
        /// </summary>
        public bool IsPickByPackage { get; set; }

        /// <summary>
        /// 允许超发数
        /// </summary>
        public decimal AllowOutQty { get; set; }

        /// <summary>
        /// 允许超发数（辅单位）
        /// </summary>
        public decimal SecondAllowOutQty { get; set; }

        /// <summary>
        /// 是否允许拆分
        /// </summary>
        public bool IsAllowSplit { get; set; }

        /// <summary>
        /// 分子
        /// </summary>
        public decimal Numerator { get; set; }

        /// <summary>
        /// 分母
        /// </summary>
        public decimal Denominator { get; set; }

        /// <summary>
        /// 精度
        /// </summary>
        public int? SecondPrecision { get; set; }

        /// <summary>
        /// 进位
        /// </summary>
        public int? Carry { get; set; }
    }

    /// <summary>
    /// 任务列表
    /// </summary>
    public class AllTaskData : TaskData
    {
        /// <summary>
        /// 优先级描述
        /// </summary>
        public string LevelDesc { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public string OperationType { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 操作源ID
        /// </summary>
        public double TaskSourceId { get; set; }

    }

    /// <summary>
    /// 任务列表
    /// </summary>
    public class AllTaskDataList
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AllTaskDataList()
        {
            TaskList = new List<AllTaskData>();
        }

        /// <summary>
        /// 任务列表
        /// </summary>
        public List<AllTaskData> TaskList { get; set; }

        /// <summary>
        /// 任务数
        /// </summary>
        public int TaskCount { get; set; }
    }

    /// <summary>
    /// 提交任务API交互信息
    /// </summary>
    public class SubmitTaskData
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public double TaskId { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 目标库位
        /// </summary>
        public string ToLocCode { get; set; }

        /// <summary>
        /// 目标LPN
        /// </summary>
        public string ToLpn { get; set; }

        /// <summary>
        /// 单据Id
        /// </summary>
        public double BillId { get; set; }

        /// <summary>
        /// 单据明细Id
        /// </summary>
        public double BillDtlId { get; set; }

        /// <summary>
        /// 任务数量
        /// </summary>
        public decimal Qty { get; set; }
    }

    /// <summary>
    /// 扫描上架
    /// </summary>
    public class ScanSubmitTaskData : SubmitTaskData
    {
        /// <summary>
        /// 标签关系
        /// </summary>
        public List<SnRelation> SnRelation { get; set; }

        /// <summary>
        /// 来源LPN
        /// </summary>
        public string FromLpn { get; set; }

        /// <summary>
        /// 上架数量
        /// </summary>
        public new decimal Qty { get; set; }

        /// <summary>
        /// 来源库位
        /// </summary>
        public string FromLoc { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 实际目标库位
        /// </summary>
        public string ActualToLocCode { get; set; }

        /// <summary>
        /// 实际目标LPN
        /// </summary>
        public string ActualToLpn { get; set; }
    }

    /// <summary>
    /// 标签关系
    /// </summary>
    public class SnRelation
    {
        /// <summary>
        /// 最上级条码
        /// </summary>
        public string TopSn { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public List<string> RIDs { get; set; }
    }

    /// <summary>
    /// 工作统计API交互数据
    /// </summary>
    public class StatisticsTaskData
    {
        /// <summary>
        /// 任务数据
        /// </summary>
        public StatisticsTaskData()
        {
            StatisticsDataList = new List<StatisticsData>();
        }

        /// <summary>
        /// 分数
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 统计数据集合
        /// </summary>
        public List<StatisticsData> StatisticsDataList { get; set; }
    }

    /// <summary>
    /// 统计信息
    /// </summary>
    public class StatisticsData
    {
        /// <summary>
        /// 项目类型 1：任务工作数量 2：任务工作时长 3：任务操作数量
        /// </summary>
        public int ProjectType { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public string Project { get; set; }

        /// <summary>
        /// 个人数
        /// </summary>
        public decimal PersonalQty { get; set; }

        /// <summary>
        /// 平均数
        /// </summary>
        public decimal AvgQty { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public decimal SumQty { get; set; }
    }
}
