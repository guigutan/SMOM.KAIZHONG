using SIE;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Core.RedCardManagments;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.WorkFlow.Base.FlowDefinitions;
using SIE.WorkFlow.Base.FlowInstances;
using System;

namespace SIE.RedCardManagment.RedCardApplyBills
{
    /// <summary>
    /// 红牌申请单
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(RedCardApplyBillCriteria))]
    [Label("红牌申请单")]
    [EntityWithConfig(typeof(NoConfig))]
    public partial class RedCardApplyBill : DataEntity
    {
        #region 申请单号 No
        /// <summary>
        /// 申请单号
        /// </summary>
        [Label("申请单号")]
        public static readonly Property<string> NoProperty = P<RedCardApplyBill>.Register(e => e.No);

        /// <summary>
        /// 申请单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 问题描述 ProblemDescription
        /// <summary>
        /// 问题描述
        /// </summary>
        [Label("问题描述")]
        public static readonly Property<string> ProblemDescriptionProperty = P<RedCardApplyBill>.Register(e => e.ProblemDescription);

        /// <summary>
        /// 问题描述
        /// </summary>
        public string ProblemDescription
        {
            get { return GetProperty(ProblemDescriptionProperty); }
            set { SetProperty(ProblemDescriptionProperty, value); }
        }
        #endregion

        #region 生产开始日期 ProductDateStart
        /// <summary>
        /// 生产开始日期
        /// </summary>
        [Label("生产开始日期")]
        public static readonly Property<DateTime?> ProductDateStartProperty = P<RedCardApplyBill>.Register(e => e.ProductDateStart);

        /// <summary>
        /// 生产开始日期
        /// </summary>
        public DateTime? ProductDateStart
        {
            get { return GetProperty(ProductDateStartProperty); }
            set { SetProperty(ProductDateStartProperty, value); }
        }
        #endregion

        #region 生产结束日期 ProductDateEnd
        /// <summary>
        /// 生产结束日期
        /// </summary>
        [Label("生产结束日期")]
        public static readonly Property<DateTime?> ProductDateEndProperty = P<RedCardApplyBill>.Register(e => e.ProductDateEnd);

        /// <summary>
        /// 生产结束日期
        /// </summary>
        public DateTime? ProductDateEnd
        {
            get { return GetProperty(ProductDateEndProperty); }
            set { SetProperty(ProductDateEndProperty, value); }
        }
        #endregion

        #region 物料SN JoinBarcodes
        /// <summary>
        /// 物料SN
        /// </summary>
        [MaxLength(1000)]
        [Label("物料SN")]
        public static readonly Property<string> JoinBarcodesProperty = P<RedCardApplyBill>.Register(e => e.JoinBarcodes);

        /// <summary>
        /// 物料SN
        /// </summary>
        public string JoinBarcodes
        {
            get { return GetProperty(JoinBarcodesProperty); }
            set { SetProperty(JoinBarcodesProperty, value); }
        }
        #endregion

        #region 物料批次 JoinProductBatchs
        /// <summary>
        /// 物料批次
        /// </summary>
        [MaxLength(1000)]
        [Label("物料批次")]
        public static readonly Property<string> JoinProductBatchsProperty = P<RedCardApplyBill>.Register(e => e.JoinProductBatchs);

        /// <summary>
        /// 物料批次
        /// </summary>
        public string JoinProductBatchs
        {
            get { return GetProperty(JoinProductBatchsProperty); }
            set { SetProperty(JoinProductBatchsProperty, value); }
        }
        #endregion

        #region 审核意见 AuditOpinion
        /// <summary>
        /// 审核意见
        /// </summary>
        [MaxLength(1000)]
        [Label("审核意见")]
        public static readonly Property<string> AuditOpinionProperty = P<RedCardApplyBill>.Register(e => e.AuditOpinion);

        /// <summary>
        /// 审核意见
        /// </summary>
        public string AuditOpinion
        {
            get { return GetProperty(AuditOpinionProperty); }
            set { SetProperty(AuditOpinionProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料编码")]
        [Required]
        public static readonly IRefIdProperty ItemIdProperty = P<RedCardApplyBill>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)GetRefNullableId(ItemIdProperty); }
            set { SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<RedCardApplyBill>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商编码")]
        public static readonly IRefIdProperty SupplierIdProperty = P<RedCardApplyBill>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double? SupplierId
        {
            get { return (double?)GetRefNullableId(SupplierIdProperty); }
            set { SetRefNullableId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<RedCardApplyBill>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 添加方式 ApplyType
        /// <summary>
        /// 添加方式
        /// </summary>
        [Label("添加方式")]
        public static readonly Property<ApplyType> ApplyTypeProperty = P<RedCardApplyBill>.Register(e => e.ApplyType);

        /// <summary>
        /// 添加方式
        /// </summary>
        public ApplyType ApplyType
        {
            get { return GetProperty(ApplyTypeProperty); }
            set { SetProperty(ApplyTypeProperty, value); }
        }
        #endregion

        #region 来源单号 ApplySourceNo
        /// <summary>
        /// 来源单号
        /// </summary>
        [Label("来源单号")]
        public static readonly Property<string> ApplySourceNoProperty = P<RedCardApplyBill>.Register(e => e.ApplySourceNo);

        /// <summary>
        /// 来源单号
        /// </summary>
        public string ApplySourceNo
        {
            get { return GetProperty(ApplySourceNoProperty); }
            set { SetProperty(ApplySourceNoProperty, value); }
        }
        #endregion

        #region 申请来源 ApplySource
        /// <summary>
        /// 申请来源
        /// </summary>
        [Label("申请来源")]
        public static readonly Property<ApplySource> ApplySourceProperty = P<RedCardApplyBill>.Register(e => e.ApplySource);

        /// <summary>
        /// 申请来源
        /// </summary>
        public ApplySource ApplySource
        {
            get { return GetProperty(ApplySourceProperty); }
            set { SetProperty(ApplySourceProperty, value); }
        }
        #endregion

        #region   状态 BillStatus
        /// <summary>
        /// 
        /// </summary>
        [Label("状态")]
        public static readonly Property<BillStatus> BillStatusProperty = P<RedCardApplyBill>.Register(e => e.BillStatus);

        /// <summary>
        /// 
        /// </summary>
        public BillStatus BillStatus
        {
            get { return GetProperty(BillStatusProperty); }
            set { SetProperty(BillStatusProperty, value); }
        }
        #endregion

        #region 取消原因 CancelReason
        /// <summary>
        /// 取消原因
        /// </summary>
        [MaxLength(1000)]
        [Label("取消原因")]
        public static readonly Property<string> CancelReasonProperty = P<RedCardApplyBill>.Register(e => e.CancelReason);

        /// <summary>
        /// 取消原因
        /// </summary>
        public string CancelReason
        {
            get { return GetProperty(CancelReasonProperty); }
            set { SetProperty(CancelReasonProperty, value); }
        }
        #endregion

        #region 撤回原因 WithDrawReason
        /// <summary>
        /// 撤回原因
        /// </summary>
        [MaxLength(1000)]
        [Label("撤回原因")]
        public static readonly Property<string> WithDrawReasonProperty = P<RedCardApplyBill>.Register(e => e.WithDrawReason);

        /// <summary>
        /// 撤回原因
        /// </summary>
        public string WithDrawReason
        {
            get { return GetProperty(WithDrawReasonProperty); }
            set { SetProperty(WithDrawReasonProperty, value); }
        }
        #endregion

        #region 驳回原因 RejectReason
        /// <summary>
        /// 驳回原因
        /// </summary>
        [MaxLength(1000)]
        [Label("驳回原因")]
        public static readonly Property<string> RejectReasonProperty = P<RedCardApplyBill>.Register(e => e.RejectReason);

        /// <summary>
        /// 驳回原因（流程驳回到首节点原因）
        /// </summary>
        public string RejectReason
        {
            get { return GetProperty(RejectReasonProperty); }
            set { SetProperty(RejectReasonProperty, value); }
        }
        #endregion

        #region 流程实例 FlowInstance
        /// <summary>
        /// 流程实例Id
        /// </summary>
        [Label("流程实例")]
        public static readonly IRefIdProperty FlowInstanceIdProperty = P<RedCardApplyBill>.RegisterRefId(e => e.FlowInstanceId, ReferenceType.Normal);

        /// <summary>
        /// 流程实例Id
        /// </summary>
        public double? FlowInstanceId
        {
            get { return (double?)GetRefNullableId(FlowInstanceIdProperty); }
            set { SetRefNullableId(FlowInstanceIdProperty, value); }
        }

        /// <summary>
        /// 流程实例
        /// </summary>
        public static readonly RefEntityProperty<FlowInstance> FlowInstanceProperty = P<RedCardApplyBill>.RegisterRef(e => e.FlowInstance, FlowInstanceIdProperty);

        /// <summary>
        /// 流程实例
        /// </summary>
        public FlowInstance FlowInstance
        {
            get { return GetRefEntity(FlowInstanceProperty); }
            set { SetRefEntity(FlowInstanceProperty, value); }
        }
        #endregion

        #region 流程发起人 WorkflowStarter
        /// <summary>
        /// 流程发起人Id
        /// </summary>
        [Label("流程发起人")]
        public static readonly IRefIdProperty WorkflowStarterIdProperty = P<RedCardApplyBill>.RegisterRefId(e => e.WorkflowStarterId, ReferenceType.Normal);

        /// <summary>
        /// 流程发起人Id
        /// </summary>
        public double? WorkflowStarterId
        {
            get { return (double?)GetRefNullableId(WorkflowStarterIdProperty); }
            set { SetRefNullableId(WorkflowStarterIdProperty, value); }
        }

        /// <summary>
        /// 流程发起人
        /// </summary>
        public static readonly RefEntityProperty<Employee> WorkflowStarterProperty = P<RedCardApplyBill>.RegisterRef(e => e.WorkflowStarter, WorkflowStarterIdProperty);

        /// <summary>
        /// 流程发起人
        /// </summary>
        public Employee WorkflowStarter
        {
            get { return GetRefEntity(WorkflowStarterProperty); }
            set { SetRefEntity(WorkflowStarterProperty, value); }
        }
        #endregion

        #region 工作流【申请信息】节点GUID（用于恢复工作流时，指定节点，暂指【申请信息】节点） WorkFlowFirstActivityId
        /// <summary>
        /// 工作流【申请信息】节点GUID（用于恢复工作流时，指定节点，暂指【申请信息】节点）
        /// </summary>
        [Label("工作流节点GUID")]
        public static readonly Property<string> WorkFlowFirstActivityIdProperty = P<RedCardApplyBill>.Register(e => e.WorkFlowFirstActivityId);

        /// <summary>
        /// 工作流【申请信息】节点GUID（用于恢复工作流时，指定节点，暂指【申请信息】节点）
        /// </summary>
        public string WorkFlowFirstActivityId
        {
            get { return this.GetProperty(WorkFlowFirstActivityIdProperty); }
            set { this.SetProperty(WorkFlowFirstActivityIdProperty, value); }
        }
        #endregion

        

        #region 视图属性


        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<RedCardApplyBill>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<RedCardApplyBill>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 供应商编码 SupplierCode
        /// <summary>
        /// 供应商编码
        /// </summary>
        [Label("供应商编码")]
        public static readonly Property<string> SupplierCodeProperty = P<RedCardApplyBill>.RegisterView(e => e.SupplierCode, p => p.Supplier.Code);

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode
        {
            get { return this.GetProperty(SupplierCodeProperty); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<RedCardApplyBill>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #region 流程名称 FlowDefinitionName
        /// <summary>
        /// 流程名称
        /// </summary>
        [Label("流程名称")]
        public static readonly Property<string> FlowDefinitionNameProperty = P<RedCardApplyBill>.RegisterView(e => e.FlowDefinitionName, p => p.FlowInstance.FlowDefinition.Name);

        /// <summary>
        /// 流程名称
        /// </summary>
        public string FlowDefinitionName
        {
            get { return this.GetProperty(FlowDefinitionNameProperty); }
        }
        #endregion

        #region 流程编码 FlowInstanceCode
        /// <summary>
        /// 流程编码
        /// </summary>
        [Label("流程编码")]
        public static readonly Property<string> FlowInstanceCodeProperty = P<RedCardApplyBill>.RegisterView(e => e.FlowInstanceCode, p => p.FlowInstance.Code);

        /// <summary>
        /// 流程编码
        /// </summary>
        public string FlowInstanceCode
        {
            get { return this.GetProperty(FlowInstanceCodeProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 红牌申请单 实体配置
    /// </summary>
    internal class RedCardApplyBillConfig : EntityConfig<RedCardApplyBill>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("QMS_RED_CARD_APPLY_BILL").MapAllProperties();
            Meta.Property(RedCardApplyBill.ProblemDescriptionProperty).ColumnMeta.HasLength(3000);
            Meta.Property(RedCardApplyBill.JoinBarcodesProperty).ColumnMeta.HasLength(3000);
            Meta.Property(RedCardApplyBill.JoinProductBatchsProperty).ColumnMeta.HasLength(3000);
            Meta.Property(RedCardApplyBill.AuditOpinionProperty).ColumnMeta.HasLength(3000);
            Meta.Property(RedCardApplyBill.CancelReasonProperty).ColumnMeta.HasLength(3000);
            Meta.Property(RedCardApplyBill.RejectReasonProperty).ColumnMeta.HasLength(3000);
            Meta.EnablePhantoms();
        }
    }
}