using SIE.Domain;
using SIE.MES.Common;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.PreStartupSetupRecords
{
    /// <summary>
    /// 开机准备记录查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("开机准备记录查询实体")]
    public class PreStartupSetupRecordCriteria : Criteria
    {
        #region 工单 WorkOrderNo
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WorkOrderNoProperty = P<PreStartupSetupRecordCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单
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
        public static readonly Property<string> TaskNoProperty = P<PreStartupSetupRecordCriteria>.Register(e => e.TaskNo);

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
        public static readonly Property<string> ProcessProperty = P<PreStartupSetupRecordCriteria>.Register(e => e.Process);

        /// <summary>
        /// 工序
        /// </summary>
        public string Process
        {
            get { return this.GetProperty(ProcessProperty); }
            set { this.SetProperty(ProcessProperty, value); }
        }
        #endregion

        #region 机台号 ResourceCode
        /// <summary>
        /// 机台号
        /// </summary>
        [Label("机台号")]
        public static readonly Property<string> ResourceCodeProperty = P<PreStartupSetupRecordCriteria>.Register(e => e.ResourceCode);

        /// <summary>
        /// 机台号
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
            set { this.SetProperty(ResourceCodeProperty, value); }
        }
        #endregion

        #region 机台名称 ResourceName
        /// <summary>
        /// 机台名称
        /// </summary>
        [Label("机台名称")]
        public static readonly Property<string> ResourceNameProperty = P<PreStartupSetupRecordCriteria>.Register(e => e.ResourceName);

        /// <summary>
        /// 机台名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { this.SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<PreStartupSetupRecordCriteria>.Register(e => e.ItemCode);

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
        public static readonly Property<string> ItemNameProperty = P<PreStartupSetupRecordCriteria>.Register(e => e.ItemName);

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
        public static readonly Property<string> ShortDescriptionProperty = P<PreStartupSetupRecordCriteria>.Register(e => e.ShortDescription);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
            set { this.SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion

        #region 编码 ToolCode
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> ToolCodeProperty = P<PreStartupSetupRecordCriteria>.Register(e => e.ToolCode);

        /// <summary>
        /// 编码
        /// </summary>
        public string ToolCode
        {
            get { return this.GetProperty(ToolCodeProperty); }
            set { this.SetProperty(ToolCodeProperty, value); }
        }
        #endregion

        #region 名称 ToolName
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> ToolNameProperty = P<PreStartupSetupRecordCriteria>.Register(e => e.ToolName);

        /// <summary>
        /// 名称
        /// </summary>
        public string ToolName
        {
            get { return this.GetProperty(ToolNameProperty); }
            set { this.SetProperty(ToolNameProperty, value); }
        }
        #endregion

        #region 类型 CheckerFixtureType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<CheckerFixtureType?> CheckerFixtureTypeProperty = P<PreStartupSetupRecordCriteria>.Register(e => e.CheckerFixtureType);

        /// <summary>
        /// 类型
        /// </summary>
        public CheckerFixtureType? CheckerFixtureType
        {
            get { return this.GetProperty(CheckerFixtureTypeProperty); }
            set { this.SetProperty(CheckerFixtureTypeProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<PreStartupSetupRecordCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        #region 图号 DrawingNo
        /// <summary>
        /// 图号
        /// </summary>
        [Label("图号")]
        public static readonly Property<string> DrawingNoProperty = P<PreStartupSetupRecordCriteria>.Register(e => e.DrawingNo);

        /// <summary>
        /// 图号
        /// </summary>
        public string DrawingNo
        {
            get { return this.GetProperty(DrawingNoProperty); }
            set { this.SetProperty(DrawingNoProperty, value); }
        }
        #endregion


        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<PreStartupSetupRecordsController>().CriteriaPreStartupSetupRecord(this);
        }
    }
}
