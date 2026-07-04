using SIE.Common;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.MES.TaskManagement.Configs;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports.Enums;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.ShiftTypes;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;
using System.Collections.Generic;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 报工扫描标签记录
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("报工扫描标签记录")]
    public partial class ReportScanLabelLog : DataEntity
    {
        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<ReportScanLabelLog>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)this.GetRefId(WorkOrderIdProperty); }
            set { this.SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<ReportScanLabelLog>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion


        #region 派工任务 DispatchTask
        /// <summary>
        /// 派工任务Id
        /// </summary>
        [Label("派工任务")]
        public static readonly IRefIdProperty DispatchTaskIdProperty = P<ReportScanLabelLog>.RegisterRefId(e => e.DispatchTaskId, ReferenceType.Normal);

        /// <summary>
        /// 派工任务Id
        /// </summary>
        public double DispatchTaskId
        {
            get { return (double)GetRefId(DispatchTaskIdProperty); }
            set { SetRefId(DispatchTaskIdProperty, value); }
        }

        /// <summary>
        /// 派工任务
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> DispatchTaskProperty = P<ReportScanLabelLog>.RegisterRef(e => e.DispatchTask, DispatchTaskIdProperty);

        /// <summary>
        /// 派工任务
        /// </summary>
        public DispatchTask DispatchTask
        {
            get { return GetRefEntity(DispatchTaskProperty); }
            set { SetRefEntity(DispatchTaskProperty, value); }
        }
        #endregion


        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<ReportScanLabelLog>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)this.GetRefId(ProcessIdProperty); }
            set { this.SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<ReportScanLabelLog>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion


        #region 标签号 LabelNo
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> LabelNoProperty = P<ReportScanLabelLog>.Register(e => e.LabelNo);

        /// <summary>
        /// 标签号
        /// </summary>
        public string LabelNo
        {
            get { return this.GetProperty(LabelNoProperty); }
            set { this.SetProperty(LabelNoProperty, value); }
        }
        #endregion


        #region 标签数量 LabelQty
        /// <summary>
        /// 标签数量
        /// </summary>
        [Label("标签数量")]
        public static readonly Property<decimal> LabelQtyProperty = P<ReportScanLabelLog>.Register(e => e.LabelQty);

        /// <summary>
        /// 标签数量
        /// </summary>
        public decimal LabelQty
        {
            get { return this.GetProperty(LabelQtyProperty); }
            set { this.SetProperty(LabelQtyProperty, value); }
        }
        #endregion

        #region 良品数量 GoodQty
        /// <summary>
        /// 良品数量
        /// </summary>
        [Label("良品数量")]
        public static readonly Property<decimal> GoodQtyProperty = P<ReportScanLabelLog>.Register(e => e.GoodQty);

        /// <summary>
        /// 良品数量
        /// </summary>
        public decimal GoodQty
        {
            get { return this.GetProperty(GoodQtyProperty); }
            set { this.SetProperty(GoodQtyProperty, value); }
        }
        #endregion

        #region 可疑品数量 SuspectQty
        /// <summary>
        /// 可疑品数量
        /// </summary>
        [Label("可疑品数量")]
        public static readonly Property<decimal> SuspectQtyProperty = P<ReportScanLabelLog>.Register(e => e.SuspectQty);

        /// <summary>
        /// 可疑品数量
        /// </summary>
        public decimal SuspectQty
        {
            get { return this.GetProperty(SuspectQtyProperty); }
            set { this.SetProperty(SuspectQtyProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<ReportScanLabelLog>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 报工扫描标签记录 实体配置
    /// </summary>
    internal class ReportScanLabelLogConfig : EntityConfig<ReportScanLabelLog>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TM_REPORT_SCAN_LABEL_LOG").MapAllProperties();
            Meta.Property(ReportScanLabelLog.LabelNoProperty).ColumnMeta.HasIndex();
            Meta.Property(ReportScanLabelLog.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}