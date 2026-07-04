using SIE.Core.Enums;
using SIE.Core.Enums;
using System;
using System.Collections.Generic;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 分配参数
    /// </summary>
    public class AssignParam
    {
        /// <summary>
        /// 需要分配物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 分配规则
        /// </summary>
        public double? AssignRuleId { get; set; }

        /// <summary>
        /// 周转规则
        /// </summary>
        public double? TurnOverRuleId { get; set; }

        /// <summary>
        /// 指定库区Id
        /// </summary>
        public double? AppointStorageAreaId { get; set; }

        /// <summary>
        /// 指定库位Id
        /// </summary>
        public double? AppointStorageLocationId { get; set; }

        /// <summary>
        /// 指定批次
        /// </summary>
        public double? AppointLotId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 指定Lpn
        /// </summary>
        public string AppointLpn { get; set; }

        /// <summary>
        /// 货主
        /// </summary>
        private string storerCode = string.Empty;

        /// <summary>
        /// 货主
        /// </summary>
        public string StorerCode
        {
            get { return storerCode.IsNullOrEmpty() ? "*" : storerCode; }
            set { storerCode = value; }
        }

        /// <summary>
        /// 项目号
        /// </summary>
        string projectNo = string.Empty;

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo
        {
            get { return projectNo.IsNullOrEmpty() ? "*" : projectNo; }
            set { projectNo = value; }
        }

        /// <summary>
        /// 任务号
        /// </summary>
        string taskNo = string.Empty;

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo
        {
            get { return taskNo.IsNullOrEmpty() ? "*" : taskNo; }
            set { taskNo = value; }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderType? OrderType { get; set; }

        /// <summary>
        /// 单据小类
        /// </summary>
        public double? TransactionId { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public double? CustomerId { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public double? DepartmentId { get; set; }

        /// <summary>
        /// 物料扩展属性（格式DefinitionId:Value;DefinitionId2:Value2）
        /// </summary>
        public string InvItemExtProp { get; set; }

        /// <summary>
        /// 忽略扩展属性
        /// </summary>
        public bool IsIgnoreItemExtProp { get; set; }

        /// <summary>
        /// 指定批次属性1
        /// </summary>
        public DateTime? LotAtt01 { get; set; }

        /// <summary>
        /// 指定批次属性2
        /// </summary>
        public DateTime? LotAtt02 { get; set; }

        /// <summary>
        /// 指定批次属性3
        /// </summary>
        public DateTime? LotAtt03 { get; set; }

        /// <summary>
        /// 指定批次属性4
        /// </summary>
        public string LotAtt04 { get; set; }

        /// <summary>
        /// 指定批次属性5
        /// </summary>
        public decimal? LotAtt05 { get; set; }

        /// <summary>
        /// 指定批次属性6
        /// </summary>
        public decimal? LotAtt06 { get; set; }

        /// <summary>
        /// 指定批次属性7
        /// </summary>
        public bool? LotAtt07 { get; set; }

        /// <summary>
        /// 指定批次属性8
        /// </summary>
        public string LotAtt08 { get; set; }

        /// <summary>
        /// 指定批次属性9
        /// </summary>
        public string LotAtt09 { get; set; }

        /// <summary>
        /// 指定批次属性10
        /// </summary>
        public string LotAtt10 { get; set; }

        /// <summary>
        /// 指定批次属性11
        /// </summary>
        public DateTime? LotAtt11 { get; set; }

        /// <summary>
        /// 指定批次属性12
        /// </summary>
        public DateTime? LotAtt12 { get; set; }

        /// <summary>
        /// 是否允许发*
        /// </summary>
        public bool IsAllow { get; set; }

        /// <summary>
        /// 是否默认规则
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 库存使用数量
        /// </summary>
       public List<LotLpnOnhandAssignData> OnhandUseData { get; set; } = new List<LotLpnOnhandAssignData>();

        /// <summary>
        /// 是否批次管理
        /// </summary>
        public bool IsBatch { get; set; }
    }

    /// <summary>
    /// 记录分配过程产生的数据
    /// </summary>
    public class LotLpnOnhandAssignData
    {
        /// <summary>
        /// 库区Id
        /// </summary>
        public double AreaId { get; set; }

        /// <summary>
        /// 库存Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 要用到的数量
        /// </summary>
        public decimal UseQty { get; set; }

        /// <summary>
        /// 整包分配
        /// </summary>
        public bool IsPack { get; set; }

        /// <summary>
        /// 单个包装数量
        /// </summary>
        public decimal PackQty { get; set; }
    }
}
