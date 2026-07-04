using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.ItemLabels;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.FeedingRecords
{
    /// <summary>
    /// 余料称重记录
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ScrapWeighingRecordCriteria))]
    [Label("余料称重记录")]
    public class ScrapWeighingRecord : DataEntity
    {
        #region 物料标签号 Sn
        /// <summary>
        /// 物料标签号
        /// </summary>
        [Label("物料标签号")]
        public static readonly Property<string> SnProperty = P<ScrapWeighingRecord>.Register(e => e.Sn);

        /// <summary>
        /// 物料标签号
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 物料标签 ItemLabel
        /// <summary>
        /// 物料标签Id
        /// </summary>
        [Label("物料标签")]
        public static readonly IRefIdProperty ItemLabelIdProperty =
            P<ScrapWeighingRecord>.RegisterRefId(e => e.ItemLabelId, ReferenceType.Normal);

        /// <summary>
        /// 物料标签Id
        /// </summary>
        public double ItemLabelId
        {
            get { return (double)this.GetRefId(ItemLabelIdProperty); }
            set { this.SetRefId(ItemLabelIdProperty, value); }
        }

        /// <summary>
        /// 物料标签
        /// </summary>
        public static readonly RefEntityProperty<ItemLabel> ItemLabelProperty =
            P<ScrapWeighingRecord>.RegisterRef(e => e.ItemLabel, ItemLabelIdProperty);

        /// <summary>
        /// 物料标签
        /// </summary>
        public ItemLabel ItemLabel
        {
            get { return this.GetRefEntity(ItemLabelProperty); }
            set { this.SetRefEntity(ItemLabelProperty, value); }
        }
        #endregion

        #region 上料记录 FeedingRecord
        /// <summary>
        /// 上料记录Id
        /// </summary>
        [Label("上料记录")]
        public static readonly IRefIdProperty FeedingRecordIdProperty =
            P<ScrapWeighingRecord>.RegisterRefId(e => e.FeedingRecordId, ReferenceType.Normal);

        /// <summary>
        /// 上料记录Id
        /// </summary>
        public double FeedingRecordId
        {
            get { return (double)this.GetRefId(FeedingRecordIdProperty); }
            set { this.SetRefId(FeedingRecordIdProperty, value); }
        }

        /// <summary>
        /// 上料记录
        /// </summary>
        public static readonly RefEntityProperty<FeedingRecord> FeedingRecordProperty =
            P<ScrapWeighingRecord>.RegisterRef(e => e.FeedingRecord, FeedingRecordIdProperty);

        /// <summary>
        /// 上料记录
        /// </summary>
        public FeedingRecord FeedingRecord
        {
            get { return this.GetRefEntity(FeedingRecordProperty); }
            set { this.SetRefEntity(FeedingRecordProperty, value); }
        }
        #endregion

        #region 实际称重数量 ActualQty
        /// <summary>
        /// 实际称重数量
        /// </summary>
        [Label("实际称重数量")]
        public static readonly Property<decimal> ActualQtyProperty = P<ScrapWeighingRecord>.Register(e => e.ActualQty);

        /// <summary>
        /// 实际称重数量
        /// </summary>
        public decimal ActualQty
        {
            get { return this.GetProperty(ActualQtyProperty); }
            set { this.SetProperty(ActualQtyProperty, value); }
        }
        #endregion

        #region 理论剩余数量 RemainingQty
        /// <summary>
        /// 理论剩余数量
        /// </summary>
        [Label("理论剩余数量")]
        public static readonly Property<decimal> RemainingQtyProperty = P<ScrapWeighingRecord>.Register(e => e.RemainingQty);

        /// <summary>
        /// 理论剩余数量
        /// </summary>
        public decimal RemainingQty
        {
            get { return this.GetProperty(RemainingQtyProperty); }
            set { this.SetProperty(RemainingQtyProperty, value); }
        }
        #endregion

        #region 差异数量 DiffQty
        /// <summary>
        /// 差异数量
        /// </summary>
        [Label("差异数量")]
        public static readonly Property<decimal> DiffQtyProperty = P<ScrapWeighingRecord>.Register(e => e.DiffQty);

        /// <summary>
        /// 差异数量
        /// </summary>
        public decimal DiffQty
        {
            get { return this.GetProperty(DiffQtyProperty); }
            set { this.SetProperty(DiffQtyProperty, value); }
        }
        #endregion

        #region 原数量 EditQty
        /// <summary>
        /// 原数量
        /// </summary>
        [Label("原数量")]
        public static readonly Property<decimal?> EditQtyProperty = P<ScrapWeighingRecord>.Register(e => e.EditQty);

        /// <summary>
        /// 原数量
        /// </summary>
        public decimal? EditQty
        {
            get { return this.GetProperty(EditQtyProperty); }
            set { this.SetProperty(EditQtyProperty, value); }
        }
        #endregion


        #region 视图属性

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<ScrapWeighingRecord>.RegisterView(e => e.ItemCode, p => p.ItemLabel.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<ScrapWeighingRecord>.RegisterView(e => e.ItemName, p => p.ItemLabel.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 状态 ItemLabelState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<ItemLabelState?> ItemLabelStateProperty = P<ScrapWeighingRecord>.RegisterView(e => e.ItemLabelState, p => p.ItemLabel.ItemLabelState);

        /// <summary>
        /// 状态
        /// </summary>
        public ItemLabelState? ItemLabelState
        {
            get { return this.GetProperty(ItemLabelStateProperty); }
        }
        #endregion

        #region 物料批次号 Lot
        /// <summary>
        /// 物料批次号
        /// </summary>
        [Label("物料批次号")]
        public static readonly Property<string> LotProperty = P<ScrapWeighingRecord>.RegisterView(e => e.Lot, p => p.ItemLabel.Lot);

        /// <summary>
        /// 物料批次号
        /// </summary>
        public string Lot
        {
            get { return this.GetProperty(LotProperty); }
        }
        #endregion


        #endregion

        #region 上传SAP

        #region 任务单 DispatchTask
        /// <summary>
        /// 任务单Id
        /// </summary>
        [Label("任务单")]
        public static readonly IRefIdProperty DispatchTaskIdProperty =
            P<ScrapWeighingRecord>.RegisterRefId(e => e.DispatchTaskId, ReferenceType.Normal);

        /// <summary>
        /// 任务单Id
        /// </summary>
        public double? DispatchTaskId
        {
            get { return (double?)this.GetRefNullableId(DispatchTaskIdProperty); }
            set { this.SetRefNullableId(DispatchTaskIdProperty, value); }
        }

        /// <summary>
        /// 任务单
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> DispatchTaskProperty =
            P<ScrapWeighingRecord>.RegisterRef(e => e.DispatchTask, DispatchTaskIdProperty);

        /// <summary>
        /// 任务单
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
            P<ScrapWeighingRecord>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
            P<ScrapWeighingRecord>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 机台
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 扣料数量 DeductedQty
        /// <summary>
        /// 扣料数量
        /// </summary>
        [Label("扣料数量")]
        public static readonly Property<decimal?> DeductedQtyProperty = P<ScrapWeighingRecord>.Register(e => e.DeductedQty);

        /// <summary>
        /// 扣料数量
        /// </summary>
        public decimal? DeductedQty
        {
            get { return this.GetProperty(DeductedQtyProperty); }
            set { this.SetProperty(DeductedQtyProperty, value); }
        }
        #endregion

        #region 已上传SAP UploadFlag
        /// <summary>
        /// 已上传SAP
        /// </summary>
        [Label("已上传事务")]
        public static readonly Property<bool?> UploadFlagProperty = P<ScrapWeighingRecord>.Register(e => e.UploadFlag);

        /// <summary>
        /// 已上传SAP
        /// </summary>
        public bool? UploadFlag
        {
            get { return this.GetProperty(UploadFlagProperty); }
            set { this.SetProperty(UploadFlagProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 工单Id WorkOrderId
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单Id")]
        public static readonly Property<double?> WorkOrderIdProperty = P<ScrapWeighingRecord>.RegisterView(e => e.WorkOrderId, p => p.DispatchTask.WorkOrderId);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return this.GetProperty(WorkOrderIdProperty); }
        }
        #endregion

        #region ItemId ItemId
        /// <summary>
        /// ItemId
        /// </summary>
        [Label("属性名")]
        public static readonly Property<double?> ItemIdProperty = P<ScrapWeighingRecord>.RegisterView(e => e.ItemId,p=>p.ItemLabel.ItemId);

        /// <summary>
        /// ItemId
        /// </summary>
        public double? ItemId
        {
            get { return this.GetProperty(ItemIdProperty); }
        }
        #endregion

        #region ProcessCode ProcessCode
        /// <summary>
        /// ProcessCode
        /// </summary>
        [Label("ProcessCode")]
        public static readonly Property<string> ProcessCodeProperty = P<ScrapWeighingRecord>.RegisterView(e => e.ProcessCode, p => p.DispatchTask.Process.Code);

        /// <summary>
        /// ProcessCode
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
        }
        #endregion

        #endregion

        #endregion

    }

    internal class ScrapWeighingRecordConfig : EntityConfig<ScrapWeighingRecord>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("SCRAP_WEIGHING_RECORD").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
