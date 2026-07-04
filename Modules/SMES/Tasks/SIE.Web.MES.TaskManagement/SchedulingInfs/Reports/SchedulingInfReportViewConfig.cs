using SIE.MES.TaskManagement.SchedulingInfs.Reports;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.SchedulingInfs.Reports
{
    public class SchedulingInfReportViewConfig : WebViewConfig<SchedulingInfReport>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(SchedulingInfReport));
            base.ConfigView();
        }

        protected override void ConfigListView()
        {
            //View.AddBehavior("SIE.Web.MES.TaskManagement.SchedulingInfs.Scripts.SchedulingInfReportBehavior");
            //View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.UseCommands("SIE.Web.MES.TaskManagement.SchedulingInfs.Commands.SchedulingInfReportExportCommand", "SIE.Web.MES.TaskManagement.SchedulingInfs.Commands.SchedulingInfReportExportAllCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.IsImport).Show().Readonly().UseEnumEditor(p => p.ColumnXType = "ReportIsImportColorChange");
                View.Property(p => p.Factory).Show().Readonly();
                View.Property(p => p.WorkOrderNo).ShowInList(width:180).Readonly();
                View.Property(p => p.Mrb).Show().Readonly();
                View.Property(p => p.ProcessCode).Show().Readonly();
                View.Property(p => p.ProcessName).Show().Readonly();
                View.Property(p => p.MachineCode).Show().Readonly();
                View.Property(p => p.StandardCapacity).Show().Readonly();

                View.Property(p => p.ProductCode).ShowInList(width: 180).Readonly();
                View.Property(p => p.ProductName).ShowInList(width: 200).Readonly();
                View.Property(p => p.ShortDescription).Show().Readonly();
                View.Property(p => p.PlanBeginDate).Show().Readonly();
                View.Property(p => p.PlanEndDate).Show().Readonly();

                View.Property(p => p.WorkShop).Show().Readonly();
                View.Property(p => p.Type).Show().Readonly();
                View.Property(p => p.FinishQty).Show().Readonly();
                View.Property(p => p.ScrapQty).Show().Readonly();
                View.Property(p => p.ProcessQty).Show().Readonly();
                View.Property(p => p.ReportQty).Show().Readonly();
                View.Property(p => p.SchedulingQty).Show().Readonly();
                View.Property(p => p.WaitSchedulingQty).Show().Readonly();
                View.Property(p => p.TaskStatus).Show().Readonly();
                View.Property(p => p.PlanQty).Show().Readonly();
                View.Property(p => p.ImportQty).Show().Readonly();
                View.Property(p => p.State).Show().Readonly();
                View.Property(p => p.IsSchedulingInfReturn).Show().Readonly();
                View.Property(p => p.SchedulingInfReturnReason).Show().Readonly();
                View.Property(p => p.ImportTime).Show().Readonly();
                View.Property(p => p.IsCheck).Show().Readonly();
                View.Property(p => p.IsGenerateTask).Show().Readonly();
                View.Property(p => p.UpdateDate).Show().Readonly();
            }
        }
    }
}
