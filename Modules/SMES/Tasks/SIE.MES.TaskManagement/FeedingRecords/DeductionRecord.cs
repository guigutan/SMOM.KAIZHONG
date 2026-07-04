using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.StockDeducRecords;
using SIE.MES.WorkOrders;
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
    /// 扣料记录
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(DeductionRecordCriteria))]
    [Label("扣料记录")]
    public class DeductionRecord : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DeductionRecord()
        {
            DeductedQty = 0;
        }

        #region 报工记录 ReportRecord
        /// <summary>
        /// 报工记录Id
        /// </summary>
        [Label("报工记录")]
        public static readonly IRefIdProperty ReportRecordIdProperty =
            P<DeductionRecord>.RegisterRefId(e => e.ReportRecordId, ReferenceType.Normal);

        /// <summary>
        /// 报工记录Id
        /// </summary>
        public double? ReportRecordId
        {
            get { return (double?)this.GetRefNullableId(ReportRecordIdProperty); }
            set { this.SetRefNullableId(ReportRecordIdProperty, value); }
        }

        /// <summary>
        /// 报工记录
        /// </summary>
        public static readonly RefEntityProperty<ReportRecord> ReportRecordProperty =
            P<DeductionRecord>.RegisterRef(e => e.ReportRecord, ReportRecordIdProperty);

        /// <summary>
        /// 报工记录
        /// </summary>
        public ReportRecord ReportRecord
        {
            get { return this.GetRefEntity(ReportRecordProperty); }
            set { this.SetRefEntity(ReportRecordProperty, value); }
        }
        #endregion

        #region 机台 Resource
        /// <summary>
        /// 机台Id
        /// </summary>
        [Label("机台")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<DeductionRecord>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
            P<DeductionRecord>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 机台
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 标签 ItemLabel
        /// <summary>
        /// 标签Id
        /// </summary>
        [Label("标签")]
        public static readonly IRefIdProperty ItemLabelIdProperty =
            P<DeductionRecord>.RegisterRefId(e => e.ItemLabelId, ReferenceType.Normal);

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
            P<DeductionRecord>.RegisterRef(e => e.ItemLabel, ItemLabelIdProperty);

        /// <summary>
        /// 标签
        /// </summary>
        public ItemLabel ItemLabel
        {
            get { return this.GetRefEntity(ItemLabelProperty); }
            set { this.SetRefEntity(ItemLabelProperty, value); }
        }
        #endregion

        #region 扣料数量 DeductedQty
        /// <summary>
        /// 扣料数量
        /// </summary>
        [Label("扣料数量")]
        public static readonly Property<decimal?> DeductedQtyProperty = P<DeductionRecord>.Register(e => e.DeductedQty);

        /// <summary>
        /// 扣料数量
        /// </summary>
        public decimal? DeductedQty
        {
            get { return this.GetProperty(DeductedQtyProperty); }
            set { this.SetProperty(DeductedQtyProperty, value); }
        }
        #endregion

        #region 单位耗用量 SingleQty
        /// <summary>
        /// 单位耗用量
        /// </summary>
        [Label("单位耗用量")]
        public static readonly Property<decimal?> SingleQtyProperty = P<DeductionRecord>.Register(e => e.SingleQty);

        /// <summary>
        /// 单位耗用量
        /// </summary>
        public decimal? SingleQty
        {
            get { return GetProperty(SingleQtyProperty); }
            set { SetProperty(SingleQtyProperty, value); }
        }
        #endregion

        #region 取样净重 Weight
        /// <summary>
        /// 取样净重
        /// </summary>
        [Label("取样净重")]
        public static readonly Property<decimal?> WeightProperty = P<DeductionRecord>.Register(e => e.Weight);

        /// <summary>
        /// 取样净重
        /// </summary>
        public decimal? Weight
        {
            get { return this.GetProperty(WeightProperty); }
            set { this.SetProperty(WeightProperty, value); }
        }
        #endregion

        #region 是否已上传 UploadFlag
        /// <summary>
        /// 是否已上传
        /// </summary>
        [Label("是否已上传")]
        public static readonly Property<bool?> UploadFlagProperty = P<DeductionRecord>.Register(e => e.UploadFlag);

        /// <summary>
        /// 是否已上传
        /// </summary>
        public bool? UploadFlag
        {
            get { return this.GetProperty(UploadFlagProperty); }
            set { this.SetProperty(UploadFlagProperty, value); }
        }
        #endregion

        #region 上传结果 UploadResult
        /// <summary>
        /// 上传结果
        /// </summary>
        [Label("上传结果")]
        public static readonly Property<string> UploadResultProperty = P<DeductionRecord>.Register(e => e.UploadResult);

        /// <summary>
        /// 上传结果
        /// </summary>
        public string UploadResult
        {
            get { return this.GetProperty(UploadResultProperty); }
            set { this.SetProperty(UploadResultProperty, value); }
        }
        #endregion

        #region 返回物料凭证 Mblnr
        /// <summary>
        /// 返回物料凭证
        /// </summary>
        [Label("返回物料凭证")]
        public static readonly Property<string> MblnrProperty = P<DeductionRecord>.Register(e => e.Mblnr);

        /// <summary>
        /// 返回物料凭证
        /// </summary>
        public string Mblnr
        {
            get { return this.GetProperty(MblnrProperty); }
            set { this.SetProperty(MblnrProperty, value); }
        }
        #endregion

        #region 返回物料凭证年度 Mjahr
        /// <summary>
        /// 返回物料凭证年度
        /// </summary>
        [Label("返回物料凭证年度")]
        public static readonly Property<string> MjahrProperty = P<DeductionRecord>.Register(e => e.Mjahr);

        /// <summary>
        /// 返回物料凭证年度
        /// </summary>
        public string Mjahr
        {
            get { return this.GetProperty(MjahrProperty); }
            set { this.SetProperty(MjahrProperty, value); }
        }
        #endregion

        #region 上料标签 FeedingItemLabel
        /// <summary>
        /// 上料标签
        /// </summary>
        [Label("上料标签")]
        public static readonly Property<string> FeedingItemLabelProperty = P<DeductionRecord>.Register(e => e.FeedingItemLabel);

        /// <summary>
        /// 上料标签
        /// </summary>
        public string FeedingItemLabel
        {
            get { return this.GetProperty(FeedingItemLabelProperty); }
            set { this.SetProperty(FeedingItemLabelProperty, value); }
        }
        #endregion

        #region 修改数量 EditQty
        /// <summary>
        /// 修改数量
        /// </summary>
        [Label("修改数量")]
        public static readonly Property<decimal?> EditQtyProperty = P<DeductionRecord>.Register(e => e.EditQty);

        /// <summary>
        /// 修改数量
        /// </summary>
        public decimal? EditQty
        {
            get { return this.GetProperty(EditQtyProperty); }
            set { this.SetProperty(EditQtyProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<DeductionRecord>.RegisterView(e => e.WorkOrderNo, p => p.ReportRecord.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #region 标签批次 ItemLabelLot
        /// <summary>
        /// 标签批次
        /// </summary>
        [Label("标签批次")]
        public static readonly Property<string> ItemLabelLotProperty = P<DeductionRecord>.RegisterView(e => e.ItemLabelLot, p => p.ItemLabel.Lot);

        /// <summary>
        /// 标签批次
        /// </summary>
        public string ItemLabelLot
        {
            get { return this.GetProperty(ItemLabelLotProperty); }
        }
        #endregion

        #region 派工单 TaskNo
        /// <summary>
        /// 派工单
        /// </summary>
        [Label("派工单")]
        public static readonly Property<string> TaskNoProperty = P<DeductionRecord>.RegisterView(e => e.TaskNo, p => p.ReportRecord.DispatchTask.No);

        /// <summary>
        /// 派工单
        /// </summary>
        public string TaskNo
        {
            get { return this.GetProperty(TaskNoProperty); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<DeductionRecord>.RegisterView(e => e.ProductCode, p => p.ReportRecord.WorkOrder.Product.Code);

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
        public static readonly Property<string> ProductNameProperty = P<DeductionRecord>.RegisterView(e => e.ProductName, p => p.ReportRecord.WorkOrder.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 机台号 ResourceCode
        /// <summary>
        /// 机台号
        /// </summary>
        [Label("机台号")]
        public static readonly Property<string> ResourceCodeProperty = P<DeductionRecord>.RegisterView(e => e.ResourceCode, p => p.Resource.Code);

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
        public static readonly Property<string> ResourceNameProperty = P<DeductionRecord>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 机台名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 扣料批次 BatchNo
        /// <summary>
        /// 扣料批次
        /// </summary>
        [Label("扣料批次")]
        public static readonly Property<string> BatchNoProperty = P<DeductionRecord>.RegisterView(e => e.BatchNo, p => p.ReportRecord.BatchNo);

        /// <summary>
        /// 扣料批次
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<DeductionRecord>.RegisterView(e => e.ProcessCode, p => p.ReportRecord.Process.Code);

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
        public static readonly Property<string> ProcessNameProperty = P<DeductionRecord>.RegisterView(e => e.ProcessName, p => p.ReportRecord.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 物料ID ItemId
        /// <summary>
        /// 物料ID
        /// </summary>
        [Label("物料ID")]
        public static readonly Property<double> ItemIdProperty = P<DeductionRecord>.RegisterView(e => e.ItemId, p => p.ItemLabel.ItemId);

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId
        {
            get { return this.GetProperty(ItemIdProperty); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<DeductionRecord>.RegisterView(e => e.ItemCode, p => p.ItemLabel.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<DeductionRecord>.RegisterView(e => e.ItemName, p => p.ItemLabel.Item.Name);

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
        public static readonly Property<string> ShortDescriptionProperty = P<DeductionRecord>.RegisterView(e => e.ShortDescription, p => p.ItemLabel.Item.ShortDescription);

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
        public static readonly Property<string> LabelProperty = P<DeductionRecord>.RegisterView(e => e.Label, p => p.ItemLabel.Label);

        /// <summary>
        /// 标签号
        /// </summary>
        public string Label
        {
            get { return this.GetProperty(LabelProperty); }
        }
        #endregion

        #region 工单Id WorkOrderId
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单Id")]
        public static readonly Property<double> WorkOrderIdProperty = P<DeductionRecord>.RegisterView(e => e.WorkOrderId, p => p.ReportRecord.WorkOrderId);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return this.GetProperty(WorkOrderIdProperty); }
        }
        #endregion

        #region 工序流水码 Vornr
        /// <summary>
        /// 工序流水码
        /// </summary>
        [Label("工序流水码")]
        public static readonly Property<string> VornrProperty = P<DeductionRecord>.RegisterView(e => e.Vornr, p => p.ReportRecord.Vornr);

        /// <summary>
        /// 工序流水码
        /// </summary>
        public string Vornr
        {
            get { return this.GetProperty(VornrProperty); }
        }
        #endregion

        #region 库存地 Lgort
        /// <summary>
        /// 库存地
        /// </summary>
        [Label("库存地")]
        public static readonly Property<string> LgortProperty = P<DeductionRecord>.RegisterView(e => e.Lgort, p => p.ItemLabel.Lgort);

        /// <summary>
        /// 库存地
        /// </summary>
        public string Lgort
        {
            get { return this.GetProperty(LgortProperty); }
        }
        #endregion

        #region 供应商批次 Licha
        /// <summary>
        /// 供应商批次
        /// </summary>
        [Label("供应商批次")]
        public static readonly Property<string> LichaProperty = P<DeductionRecord>.RegisterView(e => e.Licha, p => p.ItemLabel.Licha);

        /// <summary>
        /// 供应商批次
        /// </summary>
        public string Licha
        {
            get { return this.GetProperty(LichaProperty); }
        }
        #endregion


        #endregion
    }

    internal class DeductionRecordConfig : EntityConfig<DeductionRecord>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("DEDUCTION_RECORD").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
