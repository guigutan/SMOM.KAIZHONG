using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Dispatchs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.Dispatchs.ViewModels
{
    /// <summary>
    /// 资源已派任务
    /// </summary>
    public class ResourcesTasksViewModelView : WebViewConfig<ResourcesTasksViewModel>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(DispatchTask));
        }


        /// <summary>
        /// 配置列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands("SIE.Web.MES.TaskManagement.Dispatchs.Commands.ReportRefreshCommand");
            View.Property(p => p.No).ShowInList(120).Readonly();
            View.Property(p => p.ReportQty).ShowInList(100).Readonly();
            View.Property(p => p.DispatchQty).ShowInList(100).Readonly();
            View.Property(p => p.AssociatedWorkOrder).ShowInList(120).Readonly();
            View.Property(p => p.ProductCode).ShowInList(100).Readonly();
            View.Property(p => p.ProductName).ShowInList(100).Readonly();

            View.Property(p => p.ProcessStandardHour).ShowInList(150).Readonly();
            View.Property(p => p.ExpectedProductionTime).ShowInList(150).Readonly();
            View.Property(p => p.ProcessHourSum).ShowInList(150).Readonly();
            View.Property(p => p.RemainingTime).ShowInList(150).Readonly().DisableSort();

            View.Property(p => p.ProcessName).ShowInList(100).Readonly();
            View.Property(p => p.TaskStatus).ShowInList(100).Readonly();
            View.Property(p => p.TaskPerformer).ShowInList(100).Readonly();
            View.Property(p => p.PlanBeginTime).ShowInList(120).Readonly();
            View.Property(p => p.PlanEndTime).ShowInList(120).Readonly();
            View.Property(p => p.UpdateByName).ShowInList(120).Readonly();
            View.Property(p => p.UpdateDate).ShowInList(150).Readonly();
        }
    }
}
