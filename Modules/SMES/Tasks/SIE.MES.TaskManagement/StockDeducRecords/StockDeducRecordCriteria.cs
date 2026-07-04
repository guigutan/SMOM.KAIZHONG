using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.StockDeducRecords
{
    /// <summary>
    /// 扣料记录查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("扣料记录查询实体")]
    public class StockDeducRecordCriteria : Criteria
    {
        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<StockDeducRecordCriteria>.Register(e => e.WorkOrderNo);

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
        public static readonly Property<string> TaskNoProperty = P<StockDeducRecordCriteria>.Register(e => e.TaskNo);

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
        public static readonly Property<string> ProductCodeProperty = P<StockDeducRecordCriteria>.Register(e => e.ProductCode);

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
        public static readonly Property<string> ProductNameProperty = P<StockDeducRecordCriteria>.Register(e => e.ProductName);

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
        public static readonly Property<string> ShortDescriptionProperty = P<StockDeducRecordCriteria>.Register(e => e.ShortDescription);

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
        public static readonly Property<string> BatchNoProperty = P<StockDeducRecordCriteria>.Register(e => e.BatchNo);

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
        public static readonly Property<string> ResourceProperty = P<StockDeducRecordCriteria>.Register(e => e.Resource);

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
        public static readonly Property<string> ProcessProperty = P<StockDeducRecordCriteria>.Register(e => e.Process);

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
        public static readonly Property<string> ItemCodeProperty = P<StockDeducRecordCriteria>.Register(e => e.ItemCode);

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
        public static readonly Property<string> ItemNameProperty = P<StockDeducRecordCriteria>.Register(e => e.ItemName);

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
        public static readonly Property<string> ItemShortDescriptionProperty = P<StockDeducRecordCriteria>.Register(e => e.ItemShortDescription);

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
        public static readonly Property<DateRange> CreateDateProperty = P<StockDeducRecordCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<StockDeducRecordsController>().CriteriaStockDeducRecords(this);
        }

    }
}
