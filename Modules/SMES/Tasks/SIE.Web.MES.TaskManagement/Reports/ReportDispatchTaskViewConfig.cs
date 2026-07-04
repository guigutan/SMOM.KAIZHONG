using DocumentFormat.OpenXml.Wordprocessing;
using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports;
using SIE.Web.MES.TaskManagement.Reports.Commands;
using System.Management;

namespace SIE.Web.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 报工任务视图配置
    /// </summary>  
    public class ReportDispatchTaskViewConfig : WebViewConfig<ReportDispatchTask>
    {
        /// <summary>
        /// 个人任务详情
        /// </summary>
        public static readonly string dispatchTaskDetailView = "DispatchTaskDetailView";

        /// <summary>
        /// 报工任务单视图
        /// </summary>
        public static readonly string reportDispatchListView = "ReportDispatchListView";

        /// <summary>
        /// 任务单报检视图
        /// </summary>
        public static readonly string dispatchTaskInspView = "DispatchTaskInspView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ReportDispatchTask));
            View.DeclareExtendViewGroup(reportDispatchListView, dispatchTaskDetailView, dispatchTaskInspView);
            if (ViewGroup == reportDispatchListView)
            {
                ReportDispatchListView();
            }
            else if (ViewGroup == dispatchTaskDetailView)
            {
                DispatchTaskDetailView();
            }
            else if (ViewGroup == dispatchTaskInspView)
            {
                DispatchTaskInspView();
            }
            else
            {
                //
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        void ReportDispatchListView()
        {
            View.UseCommands("SIE.Web.MES.TaskManagement.Reports.ReportTaskRefreshCommand", ReportTaskStartWorkCommand.FullName);
            View.UseCommands(typeof(ReportTaskStopCommand).FullName, typeof(ReportTaskResumeCommand).FullName,typeof(ReportFinishConfirmCommand).FullName);
            View.AddBehavior("SIE.Web.MES.TaskManagement.Reports.ReportBehavior");
            using (View.OrderProperties())
            {
                View.UseClientOrder();
                View.Property(p => p.No).ShowInList(150).Readonly();
                View.Property(p => p.DispatchQty).ShowInList(80).Readonly();
                View.Property(p => p.AssociatedWorkOrder).ShowInList(130).Readonly().HasLabel("工单编号");
                View.Property(p => p.ProductCode).ShowInList(130).Readonly();
                View.Property(p => p.ProductName).ShowInList(130).Readonly();
                View.Property(p => p.Priority).UseEnumEditor(p => p.ColumnXType = "ReportDispatchPriorityComboBox").ShowInList(60).Readonly();
                View.Property(p => p.ProcessName).ShowInList(120).Readonly().HasLabel("工序");
                View.Property(p => p.ResourceName).ShowInList().Readonly().HasLabel("资源");
                View.Property(p => p.ExcessReportRatio).ShowInList().Readonly().HasLabel("超额比例");
                View.Property(p => p.ExcessReportQty).ShowInList().Readonly().HasLabel("超额数量");
                View.Property(p => p.ReportQty).UseListSetting(p => p.TipIndex = "DispatchQty")
                    .UseDisplayEditor(p => p.ColumnXType = "ReportDispatchTaskDisplay").ShowInList(320).Readonly().HasLabel("任务进度");
                View.Property(p => p.SpecificationCode).ShowInList().Readonly();
                View.Property(p => p.SpecificationName).ShowInList().Readonly();
                View.Property(p => p.IsVirtualPart).ShowInList().Readonly();
                View.Property(p => p.VirtualPartCode).ShowInList().Readonly();
                View.Property(p => p.VirtualPartName).ShowInList().Readonly();
                View.Property(p => p.TaskStatus).UseEnumEditor().ShowInList(80).Readonly().HasLabel("任务状态");
                View.Property(p => p.ReportMode).UseDisplayEditor(p => p.ColumnXType = "ReportModeColumn").ShowInList(120).Readonly().HasLabel("报工方式");
                View.Property(p => p.BeginTime).UseListSetting(p => p.HelpInfo = "任务单首次报工完成时写入").ShowInList(150).Readonly();
                View.Property(p => p.EndTime).UseListSetting(p => p.HelpInfo = "任务单状态更新为已完成时写入").ShowInList(150).Readonly();
                View.Property(p => p.CreateDate).ShowInList(150).Readonly().HasLabel("发布时间");
                View.Property(p => p.PlanBeginTime).ShowInList(150).Readonly();
                View.Property(p => p.PlanEndTime).ShowInList(150).Readonly();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.AssociateChildrenProperty(ReportExt.ReportRecordProperty, (e) =>
                {
                    var entity = e.Parent as DispatchTask;
                    if (entity == null)
                    {
                        return new ReportRecord();
                    }
                    return RT.Service.Resolve<ReportController>().GetNewReportRecord(entity.Id);
                }, DetailsView).HasLabel("报工").Show(ChildShowInWhere.List).OrderNo = 10;
                View.AttachChildrenProperty(typeof(ReportRecord), (e) =>
                {
                    var entity = e.Parent as DispatchTask;
                    if (entity == null)
                    {
                        return new EntityList<ReportRecord>();
                    }
                    return RT.Service.Resolve<ReportController>().GetReportRecords(entity.Id, null);
                }, ListView).Show(ChildShowInWhere.List).HasLabel("报工记录").OrderNo = 20;
                View.AttachChildrenProperty(typeof(TaskProcessBom), (e) =>
                {
                    var entity = e.Parent as DispatchTask;
                    if (entity == null)
                    {
                        return new EntityList<TaskProcessBom>();
                    }
                    return RT.Service.Resolve<ReportController>().GetTaskProcessBoms(entity.Id);
                }, TaskProcessBomViewConfig.reportDispatchView).Show(ChildShowInWhere.List).HasLabel("工序BOM").OrderNo = 30;
                View.AttachChildrenProperty(typeof(ReportOperateLog), (e) =>
                {
                    var entity = e.Parent as DispatchTask;
                    if (entity == null)
                    {
                        return new EntityList<ReportOperateLog>();
                    }
                    return RT.Service.Resolve<ReportController>().GetReportOptLog(entity.Id);
                }).Show(ChildShowInWhere.List).HasLabel("执行记录").OrderNo = 40;
                View.AttachChildrenProperty(typeof(ReportTransferLabel), (e) =>
                {
                    var entity = e.Parent as DispatchTask;
                    if (entity == null)
                    {
                        return new EntityList<ReportTransferLabel>();
                    }
                    return RT.Service.Resolve<ReportController>().GetReportTransferLabels(entity.Id);
                }).Show(ChildShowInWhere.List).HasLabel("转入标签").OrderNo = 50;
                View.ChildrenProperty(p => p.OptLogList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.Details).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.TaskList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.Boms).Show(ChildShowInWhere.Hide).HasLabel("工序BOM").OrderNo = 30;
                View.ChildrenProperty(p => p.Records).Show(ChildShowInWhere.Hide);
            }
        }

        ///<summary>
        /// 配置个人任务详情列表视图
        /// </summary>
        void DispatchTaskDetailView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.No).ShowInList(120).Readonly().DisSortable();
                View.Property(p => p.DispatchQty).ShowInList(80).Readonly().DisSortable();
                View.Property(p => p.WorkOrderNo).ShowInList(130).Readonly().HasLabel("工单编号");
                View.Property(p => p.ProductCode).ShowInList(130).Readonly().DisSortable();
                View.Property(p => p.Priority).UseEnumEditor(p => p.ColumnXType = "ReportDispatchPriorityComboBox").ShowInList(60).Readonly();
                View.Property(p => p.ProcessName).ShowInList(120).Readonly().HasLabel("工序");
                View.Property(p => p.ResourceName).ShowInList().Readonly().HasLabel("资源");
                View.Property(p => p.ReportQty).UseDisplayEditor(p => p.ColumnXType = "ReportDispatchTaskDisplay").ShowInList(120).Readonly().HasLabel("任务进度");
                View.Property(p => p.TaskStatus).UseEnumEditor().ShowInList(80).Readonly().HasLabel("任务状态");
                View.Property(p => p.SpecificationCode).ShowInList().Readonly();
                View.Property(p => p.SpecificationName).ShowInList().Readonly();
                View.Property(p => p.ReportMode).UseEnumEditor().ShowInList().Readonly().HasLabel("报工方式");
                View.Property(p => p.BeginTime).ShowInList(150).Readonly();
                View.Property(p => p.EndTime).ShowInList(150).Readonly();
                View.Property(p => p.PlanBeginTime).ShowInList(150).Readonly();
                View.Property(p => p.PlanEndTime).ShowInList(150).Readonly();
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        void DispatchTaskInspView() 
        {
            View.Property(p => p.InspQty).UseSpinEditor(m=> { m.AllowBlank = false; m.MinValue = 0; }).Show();
        }
    }
}