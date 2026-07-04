using DevExpress.CodeParser;
using SIE.MES.TaskManagement.SchedulingInfReports;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.SchedulingInfReports
{
    /// <summary>
    /// 排程状态查询表-Web视图
    /// </summary>
    public class SchedulingInfReportViewConfig : WebViewConfig<SIE.MES.TaskManagement.SchedulingInfReports.SchedulingInfReport>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigListView()
        {            
            View.UseCommand(WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {

                View.Property(p => p.IsImport).Show().Readonly().UseEnumEditor(p => p.ColumnXType = "ReportIsImportColorChange");
                View.Property(p => p.Factory).Show().Readonly();
                View.Property(p => p.WorkOrderNo).ShowInList(width: 180).Readonly();
                View.Property(p => p.Mrp).Show().Readonly();
                View.Property(p => p.ProcessCode).Show().Readonly();
                View.Property(p => p.ProcessName).Show().Readonly();
                View.Property(p => p.MachineCode).Show().Readonly();
                View.Property(p => p.MachineName).Show().Readonly();
                View.Property(p => p.StandardCapacity).Show().Readonly(); 

                View.Property(p => p.ProductCode).ShowInList(width: 180).Readonly();
                View.Property(p => p.ProductName).ShowInList(width: 200).Readonly();
                View.Property(p => p.ShortDescription).Show().Readonly();
                View.Property(p => p.PlanBeginDate).ShowInList(width: 200).Readonly();
                View.Property(p => p.PlanEndDate).ShowInList(width: 200).Readonly();

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
                View.Property(p => p.ImportTime).ShowInList(width: 200).Readonly();
                View.Property(p => p.IsCheck).Show().Readonly();
                View.Property(p => p.IsGenerateTask).Show().Readonly();
                View.Property(p => p.UpdateDate).ShowInList(width: 280).Readonly();

            }
        }
    }
}
