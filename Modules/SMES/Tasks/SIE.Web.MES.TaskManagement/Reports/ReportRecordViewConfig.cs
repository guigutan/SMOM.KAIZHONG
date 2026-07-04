using SIE.Domain;
using SIE.MES.TaskManagement.Reports;
using SIE.Tech.Stations;
using SIE.Web.MES.TaskManagement.Reports.Commands;

namespace SIE.Web.MES.TaskManagement.Reports
{
    /// <summary>
    /// 报工记录视图配置
    /// </summary>
    internal class ReportRecordViewConfig : WebViewConfig<ReportRecord>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            // 配置视图
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseChildrenAsHorizontal().UseLayoutSize(-8, -2);
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.PrincipalName).Show().HasLabel("责任人").Readonly();
                View.Property(p => p.ReportQty).Show().Readonly();
                View.Property(p => p.OkQty).Show().Readonly();
                View.Property(p => p.NgQty).Show().Readonly();
                View.Property(p => p.ReportQty).Show().Readonly();
                View.Property(p => p.IsRework).Show().Readonly();
                View.Property(p => p.Hour).ShowInList(120).Readonly();
                View.Property(p => p.ExamineState).Show().Readonly();
                //View.Property(p => p.InspectionStatus).Show().Readonly();
                //View.Property(p => p.InspectionResult).Show().Readonly();
                //View.Property(p => p.ProcessMode).Show().Readonly();
                View.Property(p => p.StationName).Show().HasLabel("工位").Readonly();
                View.Property(p => p.BatchNo).Show().Readonly();
                View.Property(p => p.WipBatchNos).Show().Readonly();
                View.Property(p => p.WorkGroupName).Show().Readonly();
                View.Property(p => p.ShiftName).Show().Readonly();
                View.Property(p => p.ReportTime).ShowInList(150).Readonly();
                View.Property(p => p.Remark).Readonly();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.Defects);
                View.ChildrenProperty(p => p.ReportWipBatchs);
            }
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            const string widthQty = "250px";
            View.AddBehavior("SIE.Web.MES.TaskManagement.Reports.ReportRecordBehavior");
            View.HasDetailColumnsCount(6);
            View.UseCommands("SIE.Web.MES.TaskManagement.Reports.ReportRefreshCommand", typeof(ReportFirstInspCommand).FullName, ReportCommand.FullName, ReportPrintCommand.FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.PrincipalName).UseDisplayEditor().ShowInDetail(columnSpan: 2).HasLabel("责任人");
                View.Property(p => p.TotalOkQty).UseDisplayEditor().ShowInDetail(columnSpan: 2).HasLabel("累计合格数");
                View.Property(p => p.TotalNgQty).UseDisplayEditor().ShowInDetail(columnSpan: 2).HasLabel("累计不合格数");
                View.Property(p => p.OkQty).UseSpinEditor(p => { p.MinValue = 0; p.Step = 1; p.XType = "ReportOkQtyNumber"; }).ShowInDetail(columnSpan: 3, width: widthQty).HasLabel("报工数量(合格)");
                View.Property(p => p.Station).UseDataSource((e, p, s) =>
                {
                    var record = e as ReportRecord;
                    if (record == null)
                        return new EntityList<Station>();
                    double resourceId = record.DispatchTask.ResourceId.HasValue ? record.DispatchTask.ResourceId.Value : 0;
                    if (record.ProcessId.HasValue)
                        return RT.Service.Resolve<StationController>().GetStations(resourceId, record.ProcessId.Value, p, s);
                    return RT.Service.Resolve<StationController>().GetStations(resourceId, s, p);
                }).ShowInDetail(columnSpan: 3, width: widthQty).HasLabel("工位");
                View.Property(p => p.NgQty).UseSpinEditor(p => { p.MinValue = 0; p.Step = 1; p.XType = "ReportNgQtyNumber"; }).ShowInDetail(columnSpan: 3, width: widthQty).HasLabel("报工数量(不合格)");
                View.Property(p => p.Hour).UseSpinEditor(p => { p.MinValue = 0; p.DecimalPrecision = 1; p.AllowDecimals = true; }).ShowInDetail(columnSpan: 5, width: widthQty).HasLabel("统计工时(小时)");
                View.Property(p => p.BatchNo).ShowInDetail(columnSpan: 3, width: widthQty).HasLabel("批次号");
                View.Property(p => p.DefectNamesDisplay).UseDisplayEditor(p => p.XType = "ReportDefectNamesDisplay").ShowInDetail(columnSpan: 3, width: widthQty);
                View.Property(p => p.Remark).UseMemoEditor(p => p.Height = "70px").ShowInDetail(columnSpan: 6).HasLabel("备注");
            }
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置下拉视图
        }
    }
}