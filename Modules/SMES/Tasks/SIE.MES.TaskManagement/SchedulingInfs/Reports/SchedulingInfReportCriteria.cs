using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.SchedulingInfs.Reports
{
    /// <summary>
    /// 排程导入详情查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("排程导入详情查询实体")]
    public class SchedulingInfReportCriteria : Criteria
    {
        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<SchedulingInfReportCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<SchedulingInfReportCriteria>.Register(e => e.ProductCode);

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
        public static readonly Property<string> ProductNameProperty = P<SchedulingInfReportCriteria>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<SchedulingInfReportCriteria>.Register(e => e.ProcessCode);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
            set { this.SetProperty(ProcessCodeProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<SchedulingInfReportCriteria>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
            set { this.SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 是否已导入 IsImport
        /// <summary>
        /// 是否已导入
        /// </summary>
        [Label("是否已导入")]
        public static readonly Property<YesNo?> IsImportProperty = P<SchedulingInfReportCriteria>.Register(e => e.IsImport);

        /// <summary>
        /// 是否已导入
        /// </summary>
        public YesNo? IsImport
        {
            get { return this.GetProperty(IsImportProperty); }
            set { this.SetProperty(IsImportProperty, value); }
        }
        #endregion

        #region 工单状态 State
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<WorkOrderState?> StateProperty = P<SchedulingInfReportCriteria>.Register(e => e.State);

        /// <summary>
        /// 工单状态
        /// </summary>
        public WorkOrderState? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 旧料号 ShortDescription
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<SchedulingInfReportCriteria>.Register(e => e.ShortDescription);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
            set { this.SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion

        #region 下单时间 UpdateDate
        /// <summary>
        /// 下单时间
        /// </summary>
        [Label("下单时间")]
        public static readonly Property<DateRange> UpdateDateProperty = P<SchedulingInfReportCriteria>.Register(e => e.UpdateDate);

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateRange UpdateDate
        {
            get { return this.GetProperty(UpdateDateProperty); }
            set { this.SetProperty(UpdateDateProperty, value); }
        }
        #endregion

        #region 是否排程退回 IsSchedulingInfReturn
        /// <summary>
        /// 是否排程退回
        /// </summary>
        [Label("是否排程退回")]
        public static readonly Property<YesNo?> IsSchedulingInfReturnProperty = P<SchedulingInfReportCriteria>.Register(e => e.IsSchedulingInfReturn);

        /// <summary>
        /// 是否排程退回
        /// </summary>
        public YesNo? IsSchedulingInfReturn
        {
            get { return this.GetProperty(IsSchedulingInfReturnProperty); }
            set { this.SetProperty(IsSchedulingInfReturnProperty, value); }
        }
        #endregion

        #region 排程导入时间 ImportTime
        /// <summary>
        /// 排程导入时间
        /// </summary>
        [Label("排程导入时间")]
        public static readonly Property<DateRange> ImportTimeProperty = P<SchedulingInfReportCriteria>.Register(e => e.ImportTime);

        /// <summary>
        /// 排程导入时间
        /// </summary>
        public DateRange ImportTime
        {
            get { return this.GetProperty(ImportTimeProperty); }
            set { this.SetProperty(ImportTimeProperty, value); }
        }
        #endregion

        #region 校验是否通过 IsCheck
        /// <summary>
        /// 校验是否通过
        /// </summary>
        [Label("校验是否通过")]
        public static readonly Property<YesNo?> IsCheckProperty = P<SchedulingInfReportCriteria>.Register(e => e.IsCheck);

        /// <summary>
        /// 校验是否通过
        /// </summary>
        public YesNo? IsCheck
        {
            get { return this.GetProperty(IsCheckProperty); }
            set { this.SetProperty(IsCheckProperty, value); }
        }
        #endregion

        #region 是否已下发 IsGenerateTask
        /// <summary>
        /// 是否已下发
        /// </summary>
        [Label("是否已下发")]
        public static readonly Property<YesNo?> IsGenerateTaskProperty = P<SchedulingInfReportCriteria>.Register(e => e.IsGenerateTask);

        /// <summary>
        /// 是否已下发
        /// </summary>
        public YesNo? IsGenerateTask
        {
            get { return this.GetProperty(IsGenerateTaskProperty); }
            set { this.SetProperty(IsGenerateTaskProperty, value); }
        }
        #endregion

        #region MRP控制者 Mrb
        /// <summary>
        /// MRP控制者
        /// </summary>
        [Label("MRP控制者")]
        public static readonly Property<string> MrbProperty = P<SchedulingInfReportCriteria>.Register(e => e.Mrb);

        /// <summary>
        /// MRP控制者
        /// </summary>
        public string Mrb
        {
            get { return this.GetProperty(MrbProperty); }
            set { this.SetProperty(MrbProperty, value); }
        }
        #endregion


        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<SchedulingInfController>().CriteriaSchedulingInfReport(this);
        }
    }
}
