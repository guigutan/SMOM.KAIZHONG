using SIE.Domain;
using SIE.Items;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.StockDeducRecords
{
    /// <summary>
    /// 扣料记录
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(StockDeducRecordCriteria))]
    [Label("扣料记录")]
    public class StockDeducRecord : DataEntity
    {
        #region 派工单 DispatchTask
        /// <summary>
        /// 派工单Id
        /// </summary>
        [Label("派工单")]
        public static readonly IRefIdProperty DispatchTaskIdProperty =
            P<StockDeducRecord>.RegisterRefId(e => e.DispatchTaskId, ReferenceType.Normal);

        /// <summary>
        /// 派工单Id
        /// </summary>
        public double DispatchTaskId
        {
            get { return (double)this.GetRefId(DispatchTaskIdProperty); }
            set { this.SetRefId(DispatchTaskIdProperty, value); }
        }

        /// <summary>
        /// 派工单
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> DispatchTaskProperty =
            P<StockDeducRecord>.RegisterRef(e => e.DispatchTask, DispatchTaskIdProperty);

        /// <summary>
        /// 派工单
        /// </summary>
        public DispatchTask DispatchTask
        {
            get { return this.GetRefEntity(DispatchTaskProperty); }
            set { this.SetRefEntity(DispatchTaskProperty, value); }
        }
        #endregion

        #region 扣料批次 BatchNo
        /// <summary>
        /// 扣料批次
        /// </summary>
        [Label("扣料批次")]
        public static readonly Property<string> BatchNoProperty = P<StockDeducRecord>.Register(e => e.BatchNo);

        /// <summary>
        /// 扣料批次
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 批次数量 BatchQty
        /// <summary>
        /// 批次数量
        /// </summary>
        [Label("批次数量")]
        public static readonly Property<decimal> BatchQtyProperty = P<StockDeducRecord>.Register(e => e.BatchQty);

        /// <summary>
        /// 批次数量
        /// </summary>
        public decimal BatchQty
        {
            get { return this.GetProperty(BatchQtyProperty); }
            set { this.SetProperty(BatchQtyProperty, value); }
        }
        #endregion

        #region 扣料物料 Item
        /// <summary>
        /// 扣料物料Id
        /// </summary>
        [Label("扣料物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<StockDeducRecord>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 扣料物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 扣料物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<StockDeducRecord>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 扣料物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 扣料数量 DeductedQty
        /// <summary>
        /// 扣料数量
        /// </summary>
        [Label("扣料数量")]
        public static readonly Property<decimal> DeductedQtyProperty = P<StockDeducRecord>.Register(e => e.DeductedQty);

        /// <summary>
        /// 扣料数量
        /// </summary>
        public decimal DeductedQty
        {
            get { return this.GetProperty(DeductedQtyProperty); }
            set { this.SetProperty(DeductedQtyProperty, value); }
        }
        #endregion

        #region 扣料明细 StockDeducRecordDetailList
        /// <summary>
        /// 扣料明细
        /// </summary>
        [Label("扣料明细")]
        public static readonly ListProperty<EntityList<StockDeducRecordDetail>> StockDeducRecordDetailListProperty = P<StockDeducRecord>.RegisterList(e => e.StockDeducRecordDetailList);

        /// <summary>
        /// 扣料明细
        /// </summary>
        public EntityList<StockDeducRecordDetail> StockDeducRecordDetailList
        {
            get { return this.GetLazyList(StockDeducRecordDetailListProperty); }
        }
        #endregion

        #region 视图属性

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<StockDeducRecord>.RegisterView(e => e.WorkOrderNo, p => p.DispatchTask.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #region 派工单 TaskNo
        /// <summary>
        /// 派工单
        /// </summary>
        [Label("派工单")]
        public static readonly Property<string> TaskNoProperty = P<StockDeducRecord>.RegisterView(e => e.TaskNo, p => p.DispatchTask.No);

        /// <summary>
        /// 派工单
        /// </summary>
        public string TaskNo
        {
            get { return this.GetProperty(TaskNoProperty); }
        }
        #endregion

        #region 机台号 ResourceName
        /// <summary>
        /// 机台号
        /// </summary>
        [Label("机台号")]
        public static readonly Property<string> ResourceNameProperty = P<StockDeducRecord>.RegisterView(e => e.ResourceName, p => p.DispatchTask.Resource.Name);

        /// <summary>
        /// 机台号
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<StockDeducRecord>.RegisterView(e => e.ProductCode, p => p.DispatchTask.Product.Code);

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
        public static readonly Property<string> ProductNameProperty = P<StockDeducRecord>.RegisterView(e => e.ProductName, p => p.DispatchTask.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<StockDeducRecord>.RegisterView(e => e.ProcessCode, p => p.DispatchTask.Process.Code);

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
        public static readonly Property<string> ProcessNameProperty = P<StockDeducRecord>.RegisterView(e => e.ProcessName, p => p.DispatchTask.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 扣料物料编码 ItemCode
        /// <summary>
        /// 扣料物料编码
        /// </summary>
        [Label("扣料物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<StockDeducRecord>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 扣料物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 扣料物料名称 ItemName
        /// <summary>
        /// 扣料物料名称
        /// </summary>
        [Label("扣料物料名称")]
        public static readonly Property<string> ItemNameProperty = P<StockDeducRecord>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 扣料物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 扣料物料旧料号 ItemShortDescription
        /// <summary>
        /// 扣料物料旧料号
        /// </summary>
        [Label("扣料物料旧料号")]
        public static readonly Property<string> ItemShortDescriptionProperty = P<StockDeducRecord>.RegisterView(e => e.ItemShortDescription, p => p.Item.ShortDescription);

        /// <summary>
        /// 扣料物料旧料号
        /// </summary>
        public string ItemShortDescription
        {
            get { return this.GetProperty(ItemShortDescriptionProperty); }
        }
        #endregion


        #endregion
    }

    internal class StockDeducRecordConfig : EntityConfig<StockDeducRecord>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("STOCK_DEDUC_RECORD").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
