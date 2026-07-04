using SIE.Domain;
using SIE.Items;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.ProcessTransfers
{
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("工序交接记录")]
    public class ProcessTransferRecord  :DataEntity
    {
        #region 类型 Type
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<TransferType> TypeProperty = P<ProcessTransferRecord>.Register(e => e.Type);

        /// <summary>
        /// 类型
        /// </summary>
        public TransferType Type
        {
            get { return this.GetProperty(TypeProperty); }
            set { this.SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<ProcessTransferRecord>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)this.GetRefId(WorkOrderIdProperty); }
            set { this.SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<ProcessTransferRecord>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty =
            P<ProcessTransferRecord>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return (double)this.GetRefId(ProductIdProperty); }
            set { this.SetRefId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty =
            P<ProcessTransferRecord>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return this.GetRefEntity(ProductProperty); }
            set { this.SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 条码 Barcode
        /// <summary>
        /// 条码
        /// </summary>
        [Label("条码")]
        public static readonly Property<string> BarcodeProperty = P<ProcessTransferRecord>.Register(e => e.Barcode);

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 条码类型 BarcodeType
        /// <summary>
        /// 条码类型
        /// </summary>
        [Label("条码类型")]
        public static readonly Property<TransferBarcodeType> BarcodeTypeProperty = P<ProcessTransferRecord>.Register(e => e.BarcodeType);

        /// <summary>
        /// 条码类型
        /// </summary>
        public TransferBarcodeType BarcodeType
        {
            get { return this.GetProperty(BarcodeTypeProperty); }
            set { this.SetProperty(BarcodeTypeProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<ProcessTransferRecord>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
            P<ProcessTransferRecord>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<ProcessTransferRecord>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<ProcessTransferRecord>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<ProcessTransferRecord>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 操作人 OperateBy
        /// <summary>
        /// 操作人Id
        /// </summary>
        [Label("操作人")]
        public static readonly IRefIdProperty OperateByIdProperty = P<ProcessTransferRecord>.RegisterRefId(e => e.OperateById, ReferenceType.Normal);

        /// <summary>
        /// 操作人Id
        /// </summary>
        public double OperateById
        {
            get { return (double)GetRefId(OperateByIdProperty); }
            set { SetRefId(OperateByIdProperty, value); }
        }

        /// <summary>
        /// 操作人
        /// </summary>
        public static readonly RefEntityProperty<Employee> OperateByProperty = P<ProcessTransferRecord>.RegisterRef(e => e.OperateBy, OperateByIdProperty);

        /// <summary>
        /// 操作人
        /// </summary>
        public Employee OperateBy
        {
            get { return GetRefEntity(OperateByProperty); }
            set { SetRefEntity(OperateByProperty, value); }
        }
        #endregion

        #region 操作时间 OperateTime
        /// <summary>
        /// 操作时间
        /// </summary>
        [Label("操作时间")]
        public static readonly Property<DateTime?> OperateTimeProperty = P<ProcessTransferRecord>.Register(e => e.OperateTime);

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? OperateTime
        {
            get { return GetProperty(OperateTimeProperty); }
            set { SetProperty(OperateTimeProperty, value); }
        }
        #endregion

        #region Web视图属性
        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<ProcessTransferRecord>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion
        #region 产品编码 ProCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProCodeProperty = P<ProcessTransferRecord>.RegisterView(e => e.ProCode, p => p.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProCode
        {
            get { return this.GetProperty(ProCodeProperty); }
        }
        #endregion
        #region 产品名称 ProName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProNameProperty = P<ProcessTransferRecord>.RegisterView(e => e.ProName, p => p.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProName
        {
            get { return this.GetProperty(ProNameProperty); }
        }
        #endregion
        #region 工序名称  ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<ProcessTransferRecord>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion
        #endregion


    }
    internal class ProcessTransferRecordConfig : EntityConfig<ProcessTransferRecord>
    {
        /// <summary>
        /// 数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("wip_pro_transfer").MapAllProperties();
            Meta.Property(ProcessTransferRecord.BarcodeProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }

    public enum TransferBarcodeType
    {

        /// <summary>
        /// SN
        /// </summary>\
        [Label("SN")]
        SN = 0,
        /// <summary>
        /// 周转工具
        /// </summary>
        [Label("周转工具")]
        TrunoverTool = 1,
        /// <summary>
        /// 流程卡
        /// </summary>
        [Label("流程卡")]
        FlowCard = 2,
        /// <summary>
        /// 拼板码
        /// </summary>
        [Label("拼板码")]
        PanelCode = 3
    }
}
