using SIE.Barcodes;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.MES.TaskManagement.Reports;
using SIE.Tech.Stations;
using SIE.Wpf.Common;
using SIE.Wpf.MES.TaskManagement.Reports.Commands;
using SIE.Wpf.MES.TaskManagement.Reports.Editors;
using System;

namespace SIE.Wpf.MES.TaskManagement.Reports
{
    /// <summary>
    /// 任务单报工 视图配置
    /// </summary>
    internal class TaskReportViewModelViewConfig : WPFViewConfig<TaskReportViewModel>
    {
        /// <summary>
        /// 任务单报检视图
        /// </summary>
        public const string dispatchTaskInspView = "DispatchTaskInspView";

        /// <summary>
        /// 报工打印视图
        /// </summary>
        public const string taskReportPrintView = "TaskReportPrintView";

        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(dispatchTaskInspView, taskReportPrintView);
            if (ViewGroup == dispatchTaskInspView)
            {
                DispatchTaskInspView();
                return;
            }
            if (ViewGroup == taskReportPrintView)
            {
                TaskReportPrintView();
                return;
            }

            View.AssignAuthorize(typeof(TaskReportViewModel));
            View.UseCommands(typeof(StartTaskReportCommand), typeof(FirstInspReportCommand), typeof(TaskReportCommand), typeof(TaskReportPrintCommand), typeof(RestartReportCommand));
            View.UseDetail(columnCount: 4);
            using (View.OrderProperties())
            {
                using (View.DeclareGroup(string.Empty))
                {
                    View.Property(p => p.Tips).UseTipsEditor().ShowInDetail(columnSpan: 4, height: 0, hideLabel: true);
                    View.Property(p => p.Error).UseErrorEditor().ShowInDetail(columnSpan: 4, height: 0, hideLabel: true);
                    View.Property(p => p.Barcode).UseBarcodeEditor().ShowInDetail(columnSpan: 4, height: 0, hideLabel: true);
                }
                using (View.DeclareGroup("任务单信息".L10N()))
                {
                    View.Property(p => p.DispatchTask.No).HasLabel("任务单号").ShowInDetail().Readonly();
                    View.Property(p => p.DispatchTask.DispatchQty).HasLabel("任务数量").ShowInDetail().Readonly();
                    View.Property(p => p.DispatchTask.AssociatedWorkOrder).HasLabel("工单号").ShowInDetail().Readonly();
                    View.Property(p => p.DispatchTask.Process.Name).HasLabel("工序").ShowInDetail().Readonly();

                    View.Property(p => p.DispatchTask.Product.Code).HasLabel("产品编码").ShowInDetail().Readonly();
                    View.Property(p => p.DispatchTask.Product.Name).HasLabel("产品名称").ShowInDetail().Readonly();
                    View.Property(p => p.DispatchTask.PlanBeginTime).HasLabel("计划开始时间").ShowInDetail().Readonly();
                    View.Property(p => p.DispatchTask.PlanEndTime).HasLabel("计划结束时间").ShowInDetail().Readonly();

                    View.Property(p => p.DispatchTask.Specification.Code).HasLabel("规格件编码").ShowInDetail().Readonly();
                    View.Property(p => p.DispatchTask.Specification.Name).HasLabel("规格件名称").ShowInDetail().Readonly();
                    View.Property(p => p.DispatchTask.VirtualPartCode).HasLabel("虚拟件编码").ShowInDetail().Readonly();
                    View.Property(p => p.DispatchTask.VirtualPartName).HasLabel("虚拟件名称").ShowInDetail().Readonly();
                }
                using (View.DeclareGroup("报工信息".L10N()))
                {
                    View.Property(p => p.TotalOkQty).ShowInDetail().HasLabel("累计合格数").Readonly();
                    View.Property(p => p.TotalNgQty).ShowInDetail().HasLabel("累计不合格数").Readonly();
                    View.Property(p => p.OkQty).UseSpinEditor(p => { p.MinValue = 0; p.Increment = 1; }).ShowInDetail().HasLabel("合格数量");
                    View.Property(p => p.NgQty).UseSpinEditor(p => { p.MinValue = 0; p.Increment = 1; }).ShowInDetail().HasLabel("不合格数量");

                    View.Property(p => p.Station).UseDataSource((e, p, s) =>
                    {
                        var record = e as TaskReportViewModel;
                        if (record == null)
                            return new EntityList<Station>();
                        double resourceId = record.DispatchTask?.Resource != null ? record.DispatchTask.ResourceId.Value : 0;
                        if (record.DispatchTask?.Process != null)
                            return RT.Service.Resolve<StationController>().GetStations(resourceId, record.DispatchTask.ProcessId.Value, p, s);
                        return RT.Service.Resolve<StationController>().GetStations(resourceId, s, p);
                    }).ShowInDetail().HasLabel("工位");
                    View.Property(p => p.Hour).UseSpinEditor(p => { p.MinValue = 0; p.Decimals = 1;  }).ShowInDetail().HasLabel("工时(H)");
                    View.Property(p => p.BatchNo).ShowInDetail().HasLabel("批次号");
                    View.Property(p => p.DefectNames).UseReportDefectEditor().Readonly().ShowInDetail().HasLabel("缺陷录入");
                    View.Property(p => p.Remark).ShowInDetail(columnSpan:2).HasLabel("备注");
                }
                View.ChildrenProperty(p => p.ReportRecordList).Show(ChildShowInWhere.All);
            }
        }

        /// <summary>
        /// 任务单报检视图
        /// </summary>
        void DispatchTaskInspView()
        {
            View.AssignAuthorize(typeof(TaskReportViewModel));
            View.Property(p => p.InspQty).UseSpinEditor(m => {  m.MinValue = 0; }).Show();
        }

        /// <summary>
        /// 打印窗口视图
        /// </summary>
        private void TaskReportPrintView()
        {
            View.AssignAuthorize(typeof(TaskReportViewModel));
            View.UseDetail(columnCount: 1);
            using (View.OrderProperties())
            {
                View.Property(p => p.Template).UseDataSource((e, p, k) =>
                {
                    var labelPrintName = typeof(ReportRecordPrintable).GetQualifiedName();
                    return RT.Service.Resolve<BarcodeController>().GetPrintTemplatesByType(labelPrintName, p, k);
                }).ShowInDetail(columnSpan: 1);
                View.Property(p => p.Printer).UsePrinterEditor().ShowInDetail(columnSpan: 1);
            }
        }
    }
}
