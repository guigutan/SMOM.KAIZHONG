using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.FeedingRecords
{
    /// <summary>
    /// 上料记录查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("上料记录查询实体")]
    public class FeedingRecordCriteria : Criteria
    {
        #region 工单 WorkOrder
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WorkOrderProperty = P<FeedingRecordCriteria>.Register(e => e.WorkOrder);

        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrder
        {
            get { return this.GetProperty(WorkOrderProperty); }
            set { this.SetProperty(WorkOrderProperty, value); }
        }
        #endregion

        #region 派工单 TaskNo
        /// <summary>
        /// 派工单
        /// </summary>
        [Label("派工单")]
        public static readonly Property<string> TaskNoProperty = P<FeedingRecordCriteria>.Register(e => e.TaskNo);

        /// <summary>
        /// 派工单
        /// </summary>
        public string TaskNo
        {
            get { return this.GetProperty(TaskNoProperty); }
            set { this.SetProperty(TaskNoProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessProperty = P<FeedingRecordCriteria>.Register(e => e.Process);

        /// <summary>
        /// 工序
        /// </summary>
        public string Process
        {
            get { return this.GetProperty(ProcessProperty); }
            set { this.SetProperty(ProcessProperty, value); }
        }
        #endregion

        #region 机台号 WipResource
        /// <summary>
        /// 机台号
        /// </summary>
        [Label("机台号")]
        public static readonly Property<string> WipResourceProperty = P<FeedingRecordCriteria>.Register(e => e.WipResource);

        /// <summary>
        /// 机台号
        /// </summary>
        public string WipResource
        {
            get { return this.GetProperty(WipResourceProperty); }
            set { this.SetProperty(WipResourceProperty, value); }
        }
        #endregion

        #region 机台名称 WipResourceName
        /// <summary>
        /// 机台名称
        /// </summary>
        [Label("机台名称")]
        public static readonly Property<string> WipResourceNameProperty = P<FeedingRecordCriteria>.Register(e => e.WipResourceName);

        /// <summary>
        /// 机台名称
        /// </summary>
        public string WipResourceName
        {
            get { return this.GetProperty(WipResourceNameProperty); }
            set { this.SetProperty(WipResourceNameProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<FeedingRecordCriteria>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<FeedingRecordCriteria>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 旧料号 ShortDescription
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<FeedingRecordCriteria>.Register(e => e.ShortDescription);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
            set { this.SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion

        #region 标签号 Label
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> LabelProperty = P<FeedingRecordCriteria>.Register(e => e.Label);

        /// <summary>
        /// 标签号
        /// </summary>
        public string Label
        {
            get { return this.GetProperty(LabelProperty); }
            set { this.SetProperty(LabelProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<FeedingRecordCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        #region 标签批次 ItemLabelLot
        /// <summary>
        /// 
        /// </summary>
        [Label("标签批次")]
        public static readonly Property<string> ItemLabelLotProperty = P<FeedingRecordCriteria>.Register(e => e.ItemLabelLot);

        /// <summary>
        /// 
        /// </summary>
        public string ItemLabelLot
        {
            get { return this.GetProperty(ItemLabelLotProperty); }
            set { this.SetProperty(ItemLabelLotProperty, value); }
        }
        #endregion

        #region 供料区编码 FeedingAreaCode
        /// <summary>
        /// 供料区编码
        /// </summary>
        [Label("供料区编码")]
        public static readonly Property<string> FeedingAreaCodeProperty = P<FeedingRecordCriteria>.Register(e => e.FeedingAreaCode);

        /// <summary>
        /// 供料区编码
        /// </summary>
        public string FeedingAreaCode
        {
            get { return this.GetProperty(FeedingAreaCodeProperty); }
            set { this.SetProperty(FeedingAreaCodeProperty, value); }
        }
        #endregion

        #region 供料区名称 FeedingAreaName
        /// <summary>
        /// 供料区名称
        /// </summary>
        [Label("供料区名称")]
        public static readonly Property<string> FeedingAreaNameProperty = P<FeedingRecordCriteria>.Register(e => e.FeedingAreaName);

        /// <summary>
        /// 供料区名称
        /// </summary>
        public string FeedingAreaName
        {
            get { return this.GetProperty(FeedingAreaNameProperty); }
            set { this.SetProperty(FeedingAreaNameProperty, value); }
        }
        #endregion


        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<FeedingRecordController>().CriteriaFeedingRecords(this);
        }

    }
}
