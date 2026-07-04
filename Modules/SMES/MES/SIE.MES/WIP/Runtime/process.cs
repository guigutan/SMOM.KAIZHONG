using SIE.Common;
using SIE.MES.WIP.Products;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using System;
using System.Collections.Generic;

namespace SIE.MES.WIP.Runtime
{
    /// <summary>
    /// 采集运行时工序
    /// </summary>
    [Serializable]
    public class process
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public process()
        {
            Boms = new List<bom>();
            Next = new Dictionary<ResultType, List<double>>();
            Script = new Dictionary<double, string>();
            OptionalPathDictionary = new Dictionary<double, string>();
        }

        /// <summary>
        /// 工序ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 源工序Id
        /// </summary>
        public double? ProcessId { get; set; }

        /// <summary>
        /// 是否可选
        /// </summary>
        public bool Optional { get; set; }

        /// <summary>
        /// 是否创建SKU
        /// </summary>
        public bool CreateSku { get; set; }

        /// <summary>
        /// 是否计产
        /// </summary>
        public bool? IsCalculate { get; set; }

        /// <summary>
        /// 是否生成任务单
        /// </summary>
        public bool IsGenerateTask { get; set; }

        /// <summary>
        /// 是否需求任务清单
        /// </summary>
        public bool IsRequirementTask { get; set; }

        /// <summary>
        /// 是否扣料
        /// </summary>
        public bool IsBuckleMaterial { get; set; }

        /// <summary>
        /// 起始工序
        /// </summary>
        public double? StartProcess { get; set; }

        /// <summary>
        /// 正常胜制编码
        /// </summary>
        public double? NormalVictory { get; set; }

        /// <summary>
        /// 维修胜制编码
        /// </summary>
        public double? RepairVictory { get; set; }

        /// <summary>
        /// 是否加严
        /// </summary>
        public bool IsStricter { get; set; }

        /// <summary>
        /// 超时时间（分钟）
        /// </summary>
        public int? Overtime { get; set; }

        /// <summary>
        /// 是否在局中（结束工序局数判定使用）
        /// </summary>
        public bool InInning { get; set; }

        /// <summary>
        /// 直通率取值
        /// </summary>
        public bool IsPassRate { get; set; }

        /// <summary>
        /// 绑定
        /// </summary>
        public bool IsBinding { get; set; }

        /// <summary>
        /// 解绑
        /// </summary>
        public bool IsUnBinding { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 是否重复过站
        /// </summary>
        public bool Repeat { get; set; }

        /// <summary>
        /// 工序类型
        /// </summary>
        public ProcessType Type { get; set; }

        /// <summary>
        /// 下级工序 
        /// </summary>
        public Dictionary<ResultType, List<double>> Next { get; }

        /// <summary>
        /// 工序脚本字典
        /// key为下一工序ID，value为判断脚本
        /// </summary>
        public Dictionary<double, string> Script { get; }

        /// <summary>
        /// 可选路径字典
        /// </summary>
        public Dictionary<double, string> OptionalPathDictionary { get; }

        /// <summary>
        /// 工序物料清单 
        /// </summary>
        public IList<bom> Boms { get; }

        /// <summary>
        /// 工序标记
        /// </summary>
        public RoutingProcessSign Sign { get; set; }

        /// <summary>
        /// 是否结束工序
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public bool IsEnd
        {
            get { return (Sign & RoutingProcessSign.End) == RoutingProcessSign.End; }
        }

        /// <summary>
        /// 是否开始工序
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public bool IsStart
        {
            get { return (Sign & RoutingProcessSign.Start) == RoutingProcessSign.Start; }
        }

        /// <summary>
        /// 最大过站次数
        /// </summary>
        public int? MaxPassNum { get; set; }

        /// <summary>
        /// 已过站次数
        /// </summary>
        public int PassNum { get; set; }

        /// <summary>
        /// 工序已通过
        /// </summary>
        public bool IsPass { get; set; }

        /// <summary>
        /// 是否启用入站控制
        /// </summary>
        public bool? EnableMoveInControl { get; set; }

        /// <summary>
        /// 工序交接
        /// </summary>
        public TransferType? TransferType { get; set; }

        /// <summary>
        /// 入站状态
        /// </summary>
        public WipProductProcessState WipProductProcessState { get; set; }

        /// <summary>
        /// 父节点标识ID
        /// </summary>
        public string ParentNodeId { get; set; }

        /// <summary>
        /// 采集结果 Result
        /// </summary>
        public ResultType? Result { get; set; }

        /// <summary>
        /// 是否工序组
        /// </summary>
        public bool? IsGroup { get; set; }

        /// <summary>
        /// 工序组Id
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// 是否委外
        /// </summary>
        public bool Outsourcing { get; set; }

        /// <summary>
        ///是否下工序入站
        /// </summary>
        public  bool? IsNextMoveIn { get; set; }
    }
}
