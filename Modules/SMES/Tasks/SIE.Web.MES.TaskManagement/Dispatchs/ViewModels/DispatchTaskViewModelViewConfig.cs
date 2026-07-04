using DevExpress.DataAccess.DataFederation;
using DocumentFormat.OpenXml.EMMA;
using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Dispatchs.ViewModels;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.Dispatchs.ViewModels
{
    public class DispatchTaskViewModelViewConfig : WebViewConfig<DispatchTaskViewModel>
    {
        protected override void ConfigDetailsView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WipResourceId).Show().UseDataSource((e, p, k) =>
                {
                    var entity = e as DispatchTaskViewModel;
                    if (entity == null)
                    {
                        return new EntityList<WipResource>();
                    }

                    var taskIds = entity.TaskId.Split(',').Select(p => Convert.ToDouble(p)).ToList();
                    var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasks(taskIds);
                    var task = tasks.FirstOrDefault();
                    var list = RT.Service.Resolve<WipResourceController>().GetWipResourcesByWorkCenterCode(entity.WorkCenterCode, tasks.Select(p => p.ProductId).Distinct().ToList(), k);
                    return list;
                });
            }
        }
    }
}
