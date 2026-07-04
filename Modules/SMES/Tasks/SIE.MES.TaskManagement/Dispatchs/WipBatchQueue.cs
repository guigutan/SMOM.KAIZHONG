using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;

namespace SIE.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 工序标签队列
    /// </summary>
    [RootEntity, Serializable]
    [Label("工序标签队列")]
    public class WipBatchQueue : DataEntity
    {
        #region 生产序号 Seq
        /// <summary>
        /// 生产序号
        /// </summary>
        [Label("生产序号")]
        public static readonly Property<int> SeqProperty = P<WipBatchQueue>.Register(e => e.Seq);

        /// <summary>
        /// 生产序号
        /// </summary>
        public int Seq
        {
            get { return this.GetProperty(SeqProperty); }
            set { this.SetProperty(SeqProperty, value); }
        }
        #endregion

        #region 生产资源 Resource
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<WipBatchQueue>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double ResourceId
        {
            get { return (double)this.GetRefId(ResourceIdProperty); }
            set { this.SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 生产资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<WipBatchQueue>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<WipBatchQueue>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)this.GetRefNullableId(ProcessIdProperty); }
            set { this.SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<WipBatchQueue>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工序标签 WipBatch
        /// <summary>
        /// 工序标签Id
        /// </summary>
        [Label("工序标签")]
        public static readonly IRefIdProperty WipBatchIdProperty =
            P<WipBatchQueue>.RegisterRefId(e => e.WipBatchId, ReferenceType.Normal);

        /// <summary>
        /// 工序标签Id
        /// </summary>
        public double? WipBatchId
        {
            get { return (double?)this.GetRefNullableId(WipBatchIdProperty); }
            set { this.SetRefNullableId(WipBatchIdProperty, value); }
        }

        /// <summary>
        /// 工序标签
        /// </summary>
        public static readonly RefEntityProperty<WipBatch> WipBatchProperty =
            P<WipBatchQueue>.RegisterRef(e => e.WipBatch, WipBatchIdProperty);

        /// <summary>
        /// 工序标签
        /// </summary>
        public WipBatch WipBatch
        {
            get { return this.GetRefEntity(WipBatchProperty); }
            set { this.SetRefEntity(WipBatchProperty, value); }
        }
        #endregion

        #region IOT设备数量 IotQty
        /// <summary>
        /// IOT设备数量
        /// </summary>
        [Label("IOT设备数量")]
        public static readonly Property<decimal> IotQtyProperty = P<WipBatchQueue>.Register(e => e.IotQty);

        /// <summary>
        /// IOT设备数量
        /// </summary>
        public decimal IotQty
        {
            get { return this.GetProperty(IotQtyProperty); }
            set { this.SetProperty(IotQtyProperty, value); }
        }

        #endregion

        #region 是否生产完成 IsFinished
        /// <summary>
        /// 是否生产完成
        /// </summary>
        [Label("是否生产完成")]
        public static readonly Property<bool> IsFinishedProperty = P<WipBatchQueue>.Register(e => e.IsFinished);

        /// <summary>
        /// 是否生产完成
        /// </summary>
        public bool IsFinished
        {
            get { return this.GetProperty(IsFinishedProperty); }
            set { this.SetProperty(IsFinishedProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 批次标签号 BatchNo
        /// <summary>
        /// 批次标签号
        /// </summary>
        [Label("批次标签号")]
        public static readonly Property<string> BatchNoProperty = P<WipBatchQueue>.RegisterView(e => e.BatchNo, p => p.WipBatch.BatchNo);

        /// <summary>
        /// 批次标签号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
        }

        #endregion

        #region 批量数量 BatchQty
        /// <summary>
        /// 批量数量
        /// </summary>
        [Label("批量数量")]
        public static readonly Property<decimal> BatchQtyProperty = P<WipBatchQueue>.RegisterView(e => e.BatchQty, p => p.WipBatch.Qty);

        /// <summary>
        /// 批量数量
        /// </summary>
        public decimal BatchQty
        {
            get { return this.GetProperty(BatchQtyProperty); }
        }

        #endregion

        #region 工单ID WorkOrderId
        /// <summary>
        /// 工单ID
        /// </summary>
        [Label("工单ID")]
        public static readonly Property<double> WorkOrderIdProperty = P<WipBatchQueue>.RegisterView(e => e.WorkOrderId, p => p.WipBatch.WorkOrderId);

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId
        {
            get { return this.GetProperty(WorkOrderIdProperty); }
        }

        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<WipBatchQueue>.RegisterView(e => e.WorkOrderNo, p => p.WipBatch.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion 

        #region 工单状态 WorkOrderState
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<Core.WorkOrders.WorkOrderState> WorkOrderStateProperty = P<WipBatchQueue>.RegisterView(e => e.WorkOrderState, p => p.WipBatch.WorkOrder.State);

        /// <summary>
        /// 工单状态
        /// </summary>
        public Core.WorkOrders.WorkOrderState WorkOrderState
        {
            get { return this.GetProperty(WorkOrderStateProperty); }
        }
        #endregion

        #region 工单是否暂停 IsPause
        /// <summary>
        /// 工单是否暂停
        /// </summary>
        [Label("是否暂停")]
        public static readonly Property<YesNo> IsPauseProperty = P<WipBatchQueue>.RegisterView(e => e.IsPause, p => p.WipBatch.WorkOrder.IsPause);

        /// <summary>
        /// 工单是否暂停
        /// </summary>
        public YesNo IsPause
        {
            get { return this.GetProperty(IsPauseProperty); }
        }
        #endregion

        #region 产品Id ProductId
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品Id")]
        public static readonly Property<double> ProductIdProperty = P<WipBatchQueue>.RegisterView(e => e.ProductId, p => p.WipBatch.WorkOrder.ProductId);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return this.GetProperty(ProductIdProperty); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<WipBatchQueue>.RegisterView(e => e.ProductCode, p => p.WipBatch.WorkOrder.Product.Code);

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
        public static readonly Property<string> ProductNameProperty = P<WipBatchQueue>.RegisterView(e => e.ProductName, p => p.WipBatch.WorkOrder.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 旧物料号 ShortDescription
        /// <summary>
        /// 旧物料号
        /// </summary>
        [Label("旧物料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<WipBatchQueue>.RegisterView(e => e.ShortDescription, p => p.WipBatch.WorkOrder.Product.ShortDescription);

        /// <summary>
        /// 旧物料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
        }

        #endregion

        #region 饼重 ItemWeight
        /// <summary>
        /// 饼重
        /// </summary>
        [Label("饼重")]
        public static readonly Property<string> ItemWeightProperty = P<WipBatchQueue>.RegisterView(e => e.ItemWeight, p => p.WipBatch.WorkOrder.Product.Weight);

        /// <summary>
        /// 饼重
        /// </summary>
        public string ItemWeight
        {
            get { return this.GetProperty(ItemWeightProperty); }
        }

        #endregion

        #region 净重单位 WeightUnit
        /// <summary>
        /// 净重单位
        /// </summary>
        [Label("净重单位")]
        public static readonly Property<string> WeightUnitProperty = P<WipBatchQueue>.RegisterView(e => e.WeightUnit, p => p.WipBatch.WorkOrder.Product.WeightUnit);

        /// <summary>
        /// 净重单位
        /// </summary>
        public string WeightUnit
        {
            get { return this.GetProperty(WeightUnitProperty); }
        }

        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<WipBatchQueue>.RegisterView(e => e.ProcessCode, p => p.Process.Code);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
        }

        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<WipBatchQueue>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion                

        #region 资源编码 ResourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> ResourceCodeProperty = P<WipBatchQueue>.RegisterView(e => e.ResourceCode, p => p.Resource.Code);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<WipBatchQueue>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion
                
        #endregion

        #region 不映射数据库

        #region 是否选中 IsSelected
        /// <summary>
        /// 是否选中
        /// </summary>
        [Label("是否选中")]
        public static readonly Property<bool> IsSelectedProperty = P<WipBatchQueue>.Register(e => e.IsSelected);

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get { return this.GetProperty(IsSelectedProperty); }
            set { this.SetProperty(IsSelectedProperty, value); }
        }
        #endregion

        #region 父级旧料号 ParShortDescription
        /// <summary>
        /// 父级旧料号
        /// </summary>
        [Label("父级旧料号")]
        public static readonly Property<string> ParShortDescriptionProperty = P<WipBatchQueue>.Register(e => e.ParShortDescription);

        /// <summary>
        /// 父级旧料号
        /// </summary>
        public string ParShortDescription
        {
            get { return this.GetProperty(ParShortDescriptionProperty); }
            set { this.SetProperty(ParShortDescriptionProperty, value); }
        }
        #endregion


        #endregion
    }


    /// <summary>
    /// 实体配置
    /// </summary>
    internal class WipBatchQueueEntityConfig : EntityConfig<WipBatchQueue>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TM_WIP_BATCH_QUEUE").MapAllProperties();
            Meta.Property(WipBatchQueue.IsSelectedProperty).DontMapColumn();
            Meta.Property(WipBatchQueue.ParShortDescriptionProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
