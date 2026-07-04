using SIE.Domain;
using SIE.Items;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.ItemLabels;
using SIE.Resources.WipResources;
using System;

namespace SIE.MES.TaskManagement.FeedingRecords
{
    /// <summary>
    /// 上料记录
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(FeedingRecordCriteria))]
    [Label("上料记录")]
    public class FeedingRecord : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FeedingRecord()
        {
            FeedingQty = 0;
            DeductedQty = 0;
            BlankingQty = 0;
            RemainingQty = 0;
        }

        #region 供料区 FeedingArea
        /// <summary>
        /// 供料区Id
        /// </summary>
        [Label("供料区")]
        public static readonly IRefIdProperty FeedingAreaIdProperty =
            P<FeedingRecord>.RegisterRefId(e => e.FeedingAreaId, ReferenceType.Normal);

        /// <summary>
        /// 供料区Id
        /// </summary>
        public double? FeedingAreaId
        {
            get { return (double?)this.GetRefNullableId(FeedingAreaIdProperty); }
            set { this.SetRefNullableId(FeedingAreaIdProperty, value); }
        }

        /// <summary>
        /// 供料区
        /// </summary>
        public static readonly RefEntityProperty<FeedingArea> FeedingAreaProperty =
            P<FeedingRecord>.RegisterRef(e => e.FeedingArea, FeedingAreaIdProperty);

        /// <summary>
        /// 供料区
        /// </summary>
        public FeedingArea FeedingArea
        {
            get { return this.GetRefEntity(FeedingAreaProperty); }
            set { this.SetRefEntity(FeedingAreaProperty, value); }
        }
        #endregion

        #region 派工任务单 DispatchTask
        /// <summary>
        /// 派工任务单Id
        /// </summary>
        [Label("派工任务单")]
        public static readonly IRefIdProperty DispatchTaskIdProperty =
            P<FeedingRecord>.RegisterRefId(e => e.DispatchTaskId, ReferenceType.Normal);

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
            P<FeedingRecord>.RegisterRef(e => e.DispatchTask, DispatchTaskIdProperty);

        /// <summary>
        /// 派工任务单
        /// </summary>
        public DispatchTask DispatchTask
        {
            get { return this.GetRefEntity(DispatchTaskProperty); }
            set { this.SetRefEntity(DispatchTaskProperty, value); }
        }
        #endregion

        #region 机台 Resource
        /// <summary>
        /// 机台Id
        /// </summary>
        [Label("机台")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<FeedingRecord>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 机台Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)this.GetRefNullableId(ResourceIdProperty); }
            set { this.SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 机台
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<FeedingRecord>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 机台
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<FeedingRecord>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)this.GetRefNullableId(ItemIdProperty); }
            set { this.SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<FeedingRecord>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 标签 ItemLabel
        /// <summary>
        /// 标签Id
        /// </summary>
        [Label("标签")]
        public static readonly IRefIdProperty ItemLabelIdProperty =
            P<FeedingRecord>.RegisterRefId(e => e.ItemLabelId, ReferenceType.Normal);

        /// <summary>
        /// 标签Id
        /// </summary>
        public double ItemLabelId
        {
            get { return (double)this.GetRefId(ItemLabelIdProperty); }
            set { this.SetRefId(ItemLabelIdProperty, value); }
        }

        /// <summary>
        /// 标签
        /// </summary>
        public static readonly RefEntityProperty<ItemLabel> ItemLabelProperty =
            P<FeedingRecord>.RegisterRef(e => e.ItemLabel, ItemLabelIdProperty);

        /// <summary>
        /// 标签
        /// </summary>
        public ItemLabel ItemLabel
        {
            get { return this.GetRefEntity(ItemLabelProperty); }
            set { this.SetRefEntity(ItemLabelProperty, value); }
        }
        #endregion

        #region 上料数量 FeedingQty
        /// <summary>
        /// 上料数量
        /// </summary>
        [Label("上料数量")]
        public static readonly Property<decimal?> FeedingQtyProperty = P<FeedingRecord>.Register(e => e.FeedingQty);

        /// <summary>
        /// 上料数量
        /// </summary>
        public decimal? FeedingQty
        {
            get { return this.GetProperty(FeedingQtyProperty); }
            set { this.SetProperty(FeedingQtyProperty, value); }
        }
        #endregion

        #region 剩余数量 RemainingQty
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Label("剩余数量")]
        public static readonly Property<decimal?> RemainingQtyProperty = P<FeedingRecord>.Register(e => e.RemainingQty);

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal? RemainingQty
        {
            get { return this.GetProperty(RemainingQtyProperty); }
            set { this.SetProperty(RemainingQtyProperty, value); }
        }
        #endregion

        #region 扣料数量 DeductedQty
        /// <summary>
        /// 扣料数量
        /// </summary>
        [Label("扣料数量")]
        public static readonly Property<decimal?> DeductedQtyProperty = P<FeedingRecord>.Register(e => e.DeductedQty);

        /// <summary>
        /// 扣料数量
        /// </summary>
        public decimal? DeductedQty
        {
            get { return this.GetProperty(DeductedQtyProperty); }
            set { this.SetProperty(DeductedQtyProperty, value); }
        }
        #endregion

        #region 下料数量 BlankingQty
        /// <summary>
        /// 下料数量
        /// </summary>
        [Label("下料数量")]
        public static readonly Property<decimal?> BlankingQtyProperty = P<FeedingRecord>.Register(e => e.BlankingQty);

        /// <summary>
        /// 下料数量
        /// </summary>
        public decimal? BlankingQty
        {
            get { return this.GetProperty(BlankingQtyProperty); }
            set { this.SetProperty(BlankingQtyProperty, value); }
        }
        #endregion

        #region 上料标签 FeedingItemLabel
        /// <summary>
        /// 上料标签
        /// </summary>
        [Label("上料标签")]
        public static readonly Property<string> FeedingItemLabelProperty = P<FeedingRecord>.Register(e => e.FeedingItemLabel);

        /// <summary>
        /// 上料标签
        /// </summary>
        public string FeedingItemLabel
        {
            get { return this.GetProperty(FeedingItemLabelProperty); }
            set { this.SetProperty(FeedingItemLabelProperty, value); }
        }
        #endregion

        #region 扣料排序 DeductionSeq
        /// <summary>
        /// 扣料排序
        /// </summary>
        [Label("扣料排序")]
        public static readonly Property<int> DeductionSeqProperty = P<FeedingRecord>.RegisterReadOnly(
            e => e.DeductionSeq, e => e.GetDeductionSeq(), FeedingAreaIdProperty, ResourceIdProperty);
        /// <summary>
        /// 扣料排序
        /// </summary>

        public int DeductionSeq
        {
            get { return this.GetProperty(DeductionSeqProperty); }
        }
        private int GetDeductionSeq()
        {
            return FeedingAreaId > 0 ? 0 : 1;
        }
        #endregion

        #region 视图属性

        #region 标签批次 ItemLabelLot
        /// <summary>
        /// 标签批次
        /// </summary>
        [Label("标签批次")]
        public static readonly Property<string> ItemLabelLotProperty = P<FeedingRecord>.RegisterView(e => e.ItemLabelLot, p => p.ItemLabel.Lot);

        /// <summary>
        /// 标签批次
        /// </summary>
        public string ItemLabelLot
        {
            get { return this.GetProperty(ItemLabelLotProperty); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<FeedingRecord>.RegisterView(e => e.WorkOrderNo, p => p.DispatchTask.WorkOrder.No);

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
        public static readonly Property<string> TaskNoProperty = P<FeedingRecord>.RegisterView(e => e.TaskNo, p => p.DispatchTask.No);

        /// <summary>
        /// 派工单
        /// </summary>
        public string TaskNo
        {
            get { return this.GetProperty(TaskNoProperty); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<FeedingRecord>.RegisterView(e => e.ProcessCode, p => p.DispatchTask.Process.Code);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
        }
        #endregion

        #region 机台号 ResourceCode
        /// <summary>
        /// 机台号
        /// </summary>
        [Label("机台号")]
        public static readonly Property<string> ResourceCodeProperty = P<FeedingRecord>.RegisterView(e => e.ResourceCode, p => p.Resource.Code);

        /// <summary>
        /// 机台号
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
        }
        #endregion

        #region 机台名称 ResourceName
        /// <summary>
        /// 机台名称
        /// </summary>
        [Label("机台名称")]
        public static readonly Property<string> ResourceNameProperty = P<FeedingRecord>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 机台名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion
                

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<FeedingRecord>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<FeedingRecord>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 旧料号 ShortDescription
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<FeedingRecord>.RegisterView(e => e.ShortDescription, p => p.ItemLabel.Item.ShortDescription);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
        }
        #endregion

        #region 标签号 Label
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> LabelProperty = P<FeedingRecord>.RegisterView(e => e.Label, p => p.ItemLabel.Label);

        /// <summary>
        /// 标签号
        /// </summary>
        public string Label
        {
            get { return this.GetProperty(LabelProperty); }
        }
        #endregion

        #region 供料区编码 FeedingAreaCode
        /// <summary>
        /// 供料区编码
        /// </summary>
        [Label("供料区编码")]
        public static readonly Property<string> FeedingAreaCodeProperty = P<FeedingRecord>.RegisterView(e => e.FeedingAreaCode, p => p.FeedingArea.Code);

        /// <summary>
        /// 供料区编码
        /// </summary>
        public string FeedingAreaCode
        {
            get { return this.GetProperty(FeedingAreaCodeProperty); }
        }

        #endregion

        #region 供料区名称 FeedingAreaName
        /// <summary>
        /// 供料区名称
        /// </summary>
        [Label("供料区名称")]
        public static readonly Property<string> FeedingAreaNameProperty = P<FeedingRecord>.RegisterView(e => e.FeedingAreaName, p => p.FeedingArea.Name);

        /// <summary>
        /// 供料区名称
        /// </summary>
        public string FeedingAreaName
        {
            get { return this.GetProperty(FeedingAreaNameProperty); }
        }

        #endregion

        #endregion
    }

    internal class FeedingRecordConfig : EntityConfig<FeedingRecord>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("FEEDING_RECORD").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
