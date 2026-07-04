using SIE.Domain;
using SIE.Items.Items;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;

namespace SIE.MES.TaskManagement.SuspectProductLabels
{
    /// <summary>
    /// 可疑品标签
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(SuspectProductLabelCriteria))]
    [Label("可疑品标签")]
    [DisplayMember(nameof(BatchNo))]
    public class SuspectProductLabel : DataEntity
    {
        #region 可疑品标签 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("可疑品标签")]
        public static readonly Property<string> BatchNoProperty = P<SuspectProductLabel>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return GetProperty(BatchNoProperty); }
            set { SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<SuspectProductLabel>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 派工任务单 DispatchTask
        /// <summary>
        /// 派工任务单Id
        /// </summary>
        [Label("派工任务单")]
        public static readonly IRefIdProperty DispatchTaskIdProperty =
            P<SuspectProductLabel>.RegisterRefId(e => e.DispatchTaskId, ReferenceType.Normal);

        /// <summary>
        /// 派工任务单Id
        /// </summary>
        public double? DispatchTaskId
        {
            get { return (double?)this.GetRefNullableId(DispatchTaskIdProperty); }
            set { this.SetRefNullableId(DispatchTaskIdProperty, value); }
        }

        /// <summary>
        /// 派工任务单
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> DispatchTaskProperty =
            P<SuspectProductLabel>.RegisterRef(e => e.DispatchTask, DispatchTaskIdProperty);

        /// <summary>
        /// 派工任务单
        /// </summary>
        public DispatchTask DispatchTask
        {
            get { return this.GetRefEntity(DispatchTaskProperty); }
            set { this.SetRefEntity(DispatchTaskProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<SuspectProductLabel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<SuspectProductLabel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<SuspectProductLabel>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)this.GetRefId(ProcessIdProperty); }
            set { this.SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<SuspectProductLabel>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 来源标签 ProcessBatchNo
        /// <summary>
        /// 来源标签
        /// </summary>
        [Label("来源标签")]
        public static readonly Property<string> ProcessBatchNoProperty = P<SuspectProductLabel>.Register(e => e.ProcessBatchNo);

        /// <summary>
        /// 来源标签
        /// </summary>
        public string ProcessBatchNo
        {
            get { return this.GetProperty(ProcessBatchNoProperty); }
            set { this.SetProperty(ProcessBatchNoProperty, value); }
        }
        #endregion

        #region 良品数量 GoodQty
        /// <summary>
        /// 良品数量
        /// </summary>
        [Label("良品数量")]
        public static readonly Property<decimal> GoodQtyProperty = P<SuspectProductLabel>.Register(e => e.GoodQty);

        /// <summary>
        /// 良品数量
        /// </summary>
        public decimal GoodQty
        {
            get { return this.GetProperty(GoodQtyProperty); }
            set { this.SetProperty(GoodQtyProperty, value); }
        }
        #endregion

        #region 报废数量 ScrapQty
        /// <summary>
        /// 报废数量
        /// </summary>
        [Label("报废数量")]
        public static readonly Property<decimal> ScrapQtyProperty = P<SuspectProductLabel>.Register(e => e.ScrapQty);

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty
        {
            get { return this.GetProperty(ScrapQtyProperty); }
            set { this.SetProperty(ScrapQtyProperty, value); }
        }
        #endregion

        #region 返工数量 RepairQty
        /// <summary>
        /// 返工数量
        /// </summary>
        [Label("返工数量")]
        public static readonly Property<decimal> RepairQtyProperty = P<SuspectProductLabel>.Register(e => e.RepairQty);

        /// <summary>
        /// 返工数量
        /// </summary>
        public decimal RepairQty
        {
            get { return this.GetProperty(RepairQtyProperty); }
            set { this.SetProperty(RepairQtyProperty, value); }
        }
        #endregion

        #region 是否需要MRB报告 NeedMrbReport
        /// <summary>
        /// 是否需要MRB报告
        /// </summary>
        [Label("是否需要MRB报告")]
        public static readonly Property<bool> NeedMrbReportProperty = P<SuspectProductLabel>.Register(e => e.NeedMrbReport);

        /// <summary>
        /// 是否需要MRP报告
        /// </summary>
        public bool NeedMrbReport
        {
            get { return this.GetProperty(NeedMrbReportProperty); }
            set { this.SetProperty(NeedMrbReportProperty, value); }
        }
        #endregion

        #region 处理状态 HandleState
        /// <summary>
        /// 处理状态
        /// </summary>
        [Label("处理状态")]
        public static readonly Property<SuspectHandleState> HandleStateProperty = P<SuspectProductLabel>.Register(e => e.HandleState);

        /// <summary>
        /// 处理状态
        /// </summary>
        public SuspectHandleState HandleState
        {
            get { return this.GetProperty(HandleStateProperty); }
            set { this.SetProperty(HandleStateProperty, value); }
        }
        #endregion

        #region 标签类型 LabelType
        /// <summary>
        /// 标签类型
        /// </summary>
        [Label("标签类型")]
        public static readonly Property<LabelType> LabelTypeProperty = P<SuspectProductLabel>.Register(e => e.LabelType);

        /// <summary>
        /// 标签类型
        /// </summary>
        public LabelType LabelType
        {
            get { return this.GetProperty(LabelTypeProperty); }
            set { this.SetProperty(LabelTypeProperty, value); }
        }
        #endregion

        #region 明细 DetailList
        /// <summary>
        /// 明细
        /// </summary>
        [Label("明细")]
        public static readonly ListProperty<EntityList<SuspectProductLabelDetail>> DetailListProperty = P<SuspectProductLabel>.RegisterList(e => e.DetailList);

        /// <summary>
        /// 明细
        /// </summary>
        public EntityList<SuspectProductLabelDetail> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion

        #region 附件 AttachmentList
        /// <summary>
        /// 附件
        /// </summary>
        [Label("附件")]
        public static readonly ListProperty<EntityList<SuspectProductLabelAttachment>> AttachmentListProperty = P<SuspectProductLabel>.RegisterList(e => e.AttachmentList);

        /// <summary>
        /// 附件
        /// </summary>
        public EntityList<SuspectProductLabelAttachment> AttachmentList
        {
            get { return this.GetLazyList(AttachmentListProperty); }
        }
        #endregion

        #region 资源 WipResource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<SuspectProductLabel>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? WipResourceId
        {
            get { return (double?)this.GetRefNullableId(WipResourceIdProperty); }
            set { this.SetRefNullableId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty =
            P<SuspectProductLabel>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 报工记录Id ReportRecordId
        /// <summary>
        /// 报工记录Id
        /// </summary>
        [Label("报工记录Id")]
        public static readonly Property<double?> ReportRecordIdProperty = P<SuspectProductLabel>.Register(e => e.ReportRecordId);

        /// <summary>
        /// 报工记录Id
        /// </summary>
        public double? ReportRecordId
        {
            get { return this.GetProperty(ReportRecordIdProperty); }
            set { this.SetProperty(ReportRecordIdProperty, value); }
        }
        #endregion

        #region 不映射数据库

        #region 父级旧料号 Bismt
        /// <summary>
        /// 父级旧料号
        /// </summary>
        [Label("父级旧料号")]
        public static readonly Property<string> BismtProperty = P<SuspectProductLabel>.Register(e => e.Bismt);

        /// <summary>
        /// 父级旧料号
        /// </summary>
        public string Bismt
        {
            get { return this.GetProperty(BismtProperty); }
            set { this.SetProperty(BismtProperty, value); }
        }
        #endregion


        #endregion

        #region 视图属性
        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<SuspectProductLabel>.RegisterView(e => e.ProductCode, p => p.WorkOrder.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<SuspectProductLabel>.RegisterView(e => e.ProductName, p => p.WorkOrder.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 产品描述 ProductDesc
        /// <summary>
        /// 产品描述
        /// </summary>
        [Label("产品描述")]
        public static readonly Property<string> ProductDescProperty = P<SuspectProductLabel>.RegisterView(e => e.ProductDesc, p => p.WorkOrder.Product.Description);

        /// <summary>
        /// 产品描述
        /// </summary>
        public string ProductDesc
        {
            get { return this.GetProperty(ProductDescProperty); }
        }
        #endregion

        #region 旧料号 ProductShortDescription
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> ProductShortDescriptionProperty = P<SuspectProductLabel>.RegisterView(e => e.ProductShortDescription, p => p.WorkOrder.Product.ShortDescription);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ProductShortDescription
        {
            get { return this.GetProperty(ProductShortDescriptionProperty); }
        }
        #endregion

        #region 产品Id ProductId
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品Id")]
        public static readonly Property<double> ProductIdProperty = P<SuspectProductLabel>.RegisterView(e => e.ProductId, p => p.WorkOrder.ProductId);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return this.GetProperty(ProductIdProperty); }
        }
        #endregion

        #region 车间名称 WorkShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间名称")]
        public static readonly Property<string> WorkShopNameProperty = P<SuspectProductLabel>.RegisterView(e => e.WorkShopName, p => p.DispatchTask.WorkShop.Name);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
        }
        #endregion

        #region 待处理数量 RemainingQry
        /// <summary>
        /// 待处理数量
        /// </summary>
        [Label("待处理数量")]
        public static readonly Property<decimal> RemainingQryProperty = P<SuspectProductLabel>.RegisterReadOnly(
            e => e.RemainingQry, e => e.GetRemainingQry(), QtyProperty, GoodQtyProperty, ScrapQtyProperty, RepairQtyProperty);
        /// <summary>
        /// 待处理数量
        /// </summary>

        public decimal RemainingQry
        {
            get { return this.GetProperty(RemainingQryProperty); }
        }
        private decimal GetRemainingQry()
        {
            return Qty - GoodQty - ScrapQty - RepairQty;
        }
        #endregion

        #region 资源名称 WipResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> WipResourceNameProperty = P<SuspectProductLabel>.RegisterView(e => e.WipResourceName, p => p.WipResource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string WipResourceName
        {
            get { return this.GetProperty(WipResourceNameProperty); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<SuspectProductLabel>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 可疑品标签 实体配置
    /// </summary>
    internal class SuspectProductLabelEntityConfig : EntityConfig<SuspectProductLabel>
    {
        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_SUSPECT_PROD_LABEL").MapAllProperties();
            Meta.Property(SuspectProductLabel.BatchNoProperty).ColumnMeta.HasIndex();
            Meta.Property(SuspectProductLabel.BismtProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
