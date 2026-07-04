using SIE.Core.Enums;
using SIE.Inventory.Commom;
using SIE.Inventory.Onhands;
using SIE.Inventory.Task;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;

namespace SIE.Inventory.TransactionProcessing
{
    /// <summary>
    /// 交易记录相关数据
    /// </summary>
    [Serializable]
    public partial class BaseTransactionData
    {
        #region 构造函数        
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseTransactionData()
        {
            SnList = new List<string>();
        }

        #endregion

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
        /// 单号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 单号ID
        /// </summary>
        public double BillId { get; set; }

        /// <summary>
        /// 单明细ID
        /// </summary>
        public double BillDetailId { get; set; }

        /// <summary>
        /// 单据明细号
        /// </summary>
        public string BillDetailNo { get; set; }

        /// <summary>
        /// 关联单号
        /// </summary>
        public string RelationOrderNo { get; set; }

        /// <summary>
        /// 项目号
        /// </summary>
        private string projectNo = string.Empty;

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
        private string taskNo = string.Empty;

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo
        {
            get { return taskNo.IsNullOrEmpty() ? "*" : taskNo; }
            set { taskNo = value; }
        }

        /// <summary>
        /// 员工号
        /// </summary>
        public string EmployeeNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 来源单头ID（ERP）
        /// </summary>
        public string SourceBillId { get; set; }

        /// <summary>
        /// 来源单号（ERP）
        /// </summary>
        public string SourceBillNo { get; set; }

        /// <summary>
        /// 来源单行ID（ERP）
        /// </summary>
        public string SourceBillDtlId { get; set; }

        /// <summary>
        /// 来源单行号（ERP）
        /// </summary>
        public string SourceBillDtlNo { get; set; }

        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 单据大类
        /// </summary>
        public OrderType OrderType { get; set; }

        /// <summary>
        /// 播种方式
        /// </summary>
        public SowType? SowType { get; set; }

        /// <summary>
        /// 播种工位
        /// </summary>
        public string SowStationCode { get; set; }

        /// <summary>
        /// 单据小类
        /// </summary>
        public double? TransactionId { get; set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// 交易日期
        /// </summary>
        public DateTime? TransactionDate { get; set; }


        /// <summary>
        /// 任务优先级
        /// </summary>
        public TaskLevel TaskLevel { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public double? SupplierId { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public double? CustomerId { get; set; }

        /// <summary>
        /// 生产部门ID
        /// </summary>
        public double? EnterpriseId { get; set; }

        /// <summary>
        /// 序列号集合
        /// </summary>
        public List<string> SnList { get; set; }

        /// <summary>
        /// 特殊事务标记
        /// </summary>
        public SpecialTransMark SpecialTransMark { get; set; }

        /// <summary>
        /// 库存状态
        /// </summary>
        public OnhandState? OnhandState { get; set; }

        /// <summary>
        /// 相关订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 相关订单行号
        /// </summary>
        public string OrderLineNo { get; set; }

        /// <summary>
        /// 预期数量(采)
        /// </summary>
        public decimal? PurchaseQty { get; set; }

        /// <summary>
        /// 单位(采)
        /// </summary>
        public string PurchaseUnit { get; set; }

        /// <summary>
        /// 采购转主转换率
        /// </summary>
        public decimal ConvertFigre { get; set; }

        /// <summary>
        /// ERP库存组织名称
        /// </summary>
        public string ErpOrganizationName { get; set; }

        /// <summary>
        /// ERP业务实体
        /// </summary>
        public string ErpOrgName { get; set; }

        /// <summary>
        /// ERP子库
        /// </summary>
        public string ErpWarehouseCode { get; set; }

        /// <summary>
        /// 目标Erp子库
        /// </summary>
        public string TargetErpWarehouseCode { get; set; }

        /// <summary>
        /// 目标Erp库存组织
        /// </summary>
        public string TargetErpOrganizationName { get; set; }

        /// <summary>
        /// ERP账户别名
        /// </summary>
        public string ErpAccount { get; set; }

        /// <summary>
        /// 采购单ID
        /// </summary>
        public double? PoId { get; set; }

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PoNo { get; set; }

        /// <summary>
        /// 采购单行ID
        /// </summary>
        public double? PoLineId { get; set; }

        /// <summary>
        /// 采购单行号
        /// </summary>
        public string PoLineNo { get; set; }

        /// <summary>
        /// WMS收货单号
        /// </summary>
        public string AsnNo { get; set; }

        /// <summary>
        /// WMS收货单行号
        /// </summary>
        public string AsnLineNo { get; set; }

        /// <summary>
        /// 采购单分配ID
        /// </summary>
        public double? PodistributionId { get; set; }

        /// <summary>
        /// 采购单发运ID
        /// </summary>
        public double? PoLinelocationId { get; set; }

        /// <summary>
        /// 供应商地址ID
        /// </summary>
        public double? SupplierAddress { get; set; }

        /// <summary>
        /// 调拨模式
        /// </summary>
        public AllotModel? AllotModel { get; set; }

        /// <summary>
        /// 二级明细Id, 目前用于记录分配明细行的Id，也可用在其他主从孙结构
        /// </summary>
        public double? SecondDtlId { get; set; }

        /// <summary>
        /// 二级明细行号, 目前用于记录分配明细行的Id，也可用在其他主从孙结构
        /// </summary>
        public string SecondDtlLineNo { get; set; }
    }
}
