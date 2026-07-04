using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports;
using SIE.Web.MES.TaskManagement.Dispatchs;

namespace SIE.Web.MES.TaskManagement.Reports
{
    /// <summary>
    /// 生产任务统计报表视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    public class ProductionDispatchReportViewConfig : WebViewConfig<ProductionDispatchReport>
    {
        /// <summary>
        /// 生产任务统计报表视图
        /// </summary>
        public static readonly string productionDispatchReportView = "ProductionDispatchReportView";

        #region 显示工时 ReportHour
        /// <summary>
        /// 工时统计
        /// </summary>
        public static readonly Property<string> ReportHourProperty = P<ProductionDispatchReport>.RegisterExtensionReadOnly("ReportHour", typeof(ProductionDispatchReportViewConfig),
            GetReportHour, ProductionDispatchReport.RecordsProperty, ProductionDispatchReport.RecordsProperty);

        /// <summary>
        /// 显示工时
        /// </summary>
        /// <param name="me">任务单</param>
        /// <returns>工时</returns>
        public static string GetReportHour(ProductionDispatchReport me)
        {
            EntityList<ReportRecord> records = me.Records;
            if (records.Count == 0)
            {
                return "0";
            }
            else
            {
                decimal hour = 0;
                for (int i = 0; i < records.Count; i++)
                {
                    hour += records[i].Hour;
                }
                return hour.ToString("0.00");
            }

        }
        #endregion



        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            ////base.ConfigView();
            View.DeclareExtendViewGroup(productionDispatchReportView);
            if (ViewGroup == productionDispatchReportView)
            {
                ProductionDispatchReportView();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        void ProductionDispatchReportView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.No).ShowInList(120).Readonly();
                View.Property(p => p.DispatchQty).ShowInList().Readonly();
                View.Property(p => p.AssociatedWorkOrder).ShowInList().Readonly();
                View.Property(p => p.ProductCode).ShowInList().Readonly();
                View.Property(DispatchTask.PriorityProperty).ShowInList().Readonly();
                View.Property(p => p.WorkShopName).ShowInList().Readonly();
                View.Property(p => p.ResourceName).ShowInList().Readonly();
                View.Property(p => p.ProcessName).ShowInList().Readonly();
                View.Property(p => p.SpecificationName).ShowInList().Readonly();
                View.Property(p => p.OkQty).ShowInList().Readonly();
                View.Property(p => p.NgQty).ShowInList().Readonly();
                View.Property(ReportHourProperty).ShowInList().Readonly().HasLabel("工时");
                View.Property(p => p.TaskStatus).UseEnumEditor().ShowInList().Readonly().HasLabel("任务状态");
                View.Property(p => p.ReportMode).ShowInList().Readonly();
                View.Property(p => p.BeginTime).ShowInList().Readonly();
                View.Property(p => p.EndTime).ShowInList().Readonly();
                View.Property(p => p.PlanBeginTime).ShowInList().Readonly();
                View.Property(p => p.PlanEndTime).ShowInList().Readonly();
                View.AttachChildrenProperty(typeof(ReportRecord), (e) =>
                {
                    var entity = e.Parent as DispatchTask;
                    if (entity == null)
                    {
                        return new EntityList<ReportRecord>();
                    }
                    return RT.Service.Resolve<ReportController>().GetReportRecords(entity.Id, true);
                }, ListView).Show(ChildShowInWhere.List).HasLabel("生产记录").OrderNo = 20;

                View.AttachChildrenProperty(typeof(TaskProcessBom), (e) =>
                {
                    var entity = e.Parent as DispatchTask;
                    if (entity == null)
                    {
                        return new EntityList<TaskProcessBom>();
                    }
                    return RT.Service.Resolve<ReportController>().GetTaskProcessBoms(entity.Id);
                }, TaskProcessBomViewConfig.reportDispatchView).Show(ChildShowInWhere.List).HasLabel("任务物料需求").OrderNo = 30;
                //隐藏部分列
                //View.Property(p => p.VirtualPartCode).Show(ShowInWhere.Hide);
                //View.Property(p => p.BeginTime).Show(ShowInWhere.Hide);
                //View.Property(p => p.EndTime).Show(ShowInWhere.Hide);
                //View.Property(p => p.IsVirtualPart).Show(ShowInWhere.Hide);
                //View.Property(p => p.IsMainTask).Show(ShowInWhere.Hide);
                //View.Property(p => p.TechNo).Show(ShowInWhere.Hide);
                //View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                //View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                //View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                //View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.Details).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.Records).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.TaskList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.Boms).Show(ChildShowInWhere.Hide).HasLabel("工序BOM").OrderNo = 30;
                View.ChildrenProperty(p => p.Records).Show(ChildShowInWhere.Hide);
            }
        }

    }
}
