using SIE.Domain;
using SIE.MES.TaskManagement.StockDeducRecords;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.FeedingRecords
{
    /// <summary>
    /// 扣料记录查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("扣料记录查询实体")]
    public class DeductionRecordCriteria : Criteria
    {
        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<DeductionRecordCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 派工单 TaskNo
        /// <summary>
        /// 派工单
        /// </summary>
        [Label("派工单")]
        public static readonly Property<string> TaskNoProperty = P<DeductionRecordCriteria>.Register(e => e.TaskNo);

        /// <summary>
        /// 派工单
        /// </summary>
        public string TaskNo
        {
            get { return this.GetProperty(TaskNoProperty); }
            set { this.SetProperty(TaskNoProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<DeductionRecordCriteria>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
            set { this.SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<DeductionRecordCriteria>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 旧料号 ShortDescription
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<DeductionRecordCriteria>.Register(e => e.ShortDescription);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
            set { this.SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion

        #region 扣料批次 BatchNo
        /// <summary>
        /// 扣料批次
        /// </summary>
        [Label("扣料批次")]
        public static readonly Property<string> BatchNoProperty = P<DeductionRecordCriteria>.Register(e => e.BatchNo);

        /// <summary>
        /// 扣料批次
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 机台号 Resource
        /// <summary>
        /// 机台号
        /// </summary>
        [Label("机台号")]
        public static readonly Property<string> ResourceProperty = P<DeductionRecordCriteria>.Register(e => e.Resource);

        /// <summary>
        /// 机台号
        /// </summary>
        public string Resource
        {
            get { return this.GetProperty(ResourceProperty); }
            set { this.SetProperty(ResourceProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessProperty = P<DeductionRecordCriteria>.Register(e => e.Process);

        /// <summary>
        /// 工序
        /// </summary>
        public string Process
        {
            get { return this.GetProperty(ProcessProperty); }
            set { this.SetProperty(ProcessProperty, value); }
        }
        #endregion

        #region 扣料物料编码 ItemCode
        /// <summary>
        /// 扣料物料编码
        /// </summary>
        [Label("扣料物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<DeductionRecordCriteria>.Register(e => e.ItemCode);

        /// <summary>
        /// 扣料物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 扣料物料名称 ItemName
        /// <summary>
        /// 扣料物料名称
        /// </summary>
        [Label("扣料物料名称")]
        public static readonly Property<string> ItemNameProperty = P<DeductionRecordCriteria>.Register(e => e.ItemName);

        /// <summary>
        /// 扣料物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 扣料物料旧料号 ItemShortDescription
        /// <summary>
        /// 扣料物料旧料号
        /// </summary>
        [Label("扣料物料旧料号")]
        public static readonly Property<string> ItemShortDescriptionProperty = P<DeductionRecordCriteria>.Register(e => e.ItemShortDescription);

        /// <summary>
        /// 扣料物料旧料号
        /// </summary>
        public string ItemShortDescription
        {
            get { return this.GetProperty(ItemShortDescriptionProperty); }
            set { this.SetProperty(ItemShortDescriptionProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<DeductionRecordCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        #region 物料标签 ItemLabel
        /// <summary>
        /// 物料标签
        /// </summary>
        [Label("上料标签")]
        public static readonly Property<string> ItemLabelProperty = P<DeductionRecordCriteria>.Register(e => e.ItemLabel);

        /// <summary>
        /// 物料标签
        /// </summary>
        public string ItemLabel
        {
            get { return this.GetProperty(ItemLabelProperty); }
            set { this.SetProperty(ItemLabelProperty, value); }
        }
        #endregion

        #region 标签批次 ItemLabelLot
        /// <summary>
        /// 标签批次
        /// </summary>
        [Label("标签批次")]
        public static readonly Property<string> ItemLabelLotProperty = P<DeductionRecordCriteria>.Register(e => e.ItemLabelLot);

        /// <summary>
        /// 标签批次
        /// </summary>
        public string ItemLabelLot
        {
            get { return this.GetProperty(ItemLabelLotProperty); }
            set { this.SetProperty(ItemLabelLotProperty, value); }
        }
        #endregion

        #region 返回物料凭证 Mblnr
        /// <summary>
        /// 返回物料凭证
        /// </summary>
        [Label("返回物料凭证")]
        public static readonly Property<string> MblnrProperty = P<DeductionRecordCriteria>.Register(e => e.Mblnr);

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
        public static readonly Property<string> MjahrProperty = P<DeductionRecordCriteria>.Register(e => e.Mjahr);

        /// <summary>
        /// 返回物料凭证年度
        /// </summary>
        public string Mjahr
        {
            get { return this.GetProperty(MjahrProperty); }
            set { this.SetProperty(MjahrProperty, value); }
        }
        #endregion

        #region 上传结果 UploadResult
        /// <summary>
        /// 上传结果
        /// </summary>
        [Label("上传结果")]
        public static readonly Property<string> UploadResultProperty = P<DeductionRecordCriteria>.Register(e => e.UploadResult);

        /// <summary>
        /// 上传结果
        /// </summary>
        public string UploadResult
        {
            get { return this.GetProperty(UploadResultProperty); }
            set { this.SetProperty(UploadResultProperty, value); }
        }
        #endregion

        #region 供应商批次 Licha
        /// <summary>
        /// 供应商批次
        /// </summary>
        [Label("供应商批次")]
        public static readonly Property<string> LichaProperty = P<DeductionRecordCriteria>.Register(e => e.Licha);

        /// <summary>
        /// 供应商批次
        /// </summary>
        public string Licha
        {
            get { return this.GetProperty(LichaProperty); }
            set { this.SetProperty(LichaProperty, value); }
        }
        #endregion


        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<FeedingRecordController>().CriteriaDeductionRecords(this);
        }
    }
}
