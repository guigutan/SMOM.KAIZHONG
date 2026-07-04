using SIE.Domain;
using SIE.MES.ProjectDesigns.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.Reports
{
    /// <summary>
    /// 项目号跟踪报表视图配置
    /// </summary>
    public class ProjectDesignReportViewConfig : WebViewConfig<ProjectDesignReport>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.MES.ProjectDesigns.Reports.Scripts.ReportBehavior");
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.ProjectMaintain).ShowInList(width: 120);
                View.Property(p => p.ProductCode).ShowInList(width: 120);
                View.Property(p => p.ProductName).ShowInList(width: 120);
                View.Property(p => p.SpecificationModel).ShowInList(width: 120);
                View.Property(p => p.SaleOrderNo).ShowInList(width: 120);
                View.Property(p => p.CustomerCode).ShowInList(width: 120);
                View.Property(p => p.CustomerName).ShowInList(width: 120);
                View.Property(p => p.Qty).ShowInList();
                View.Property(p => p.Unit).ShowInList();
                View.Property(p => p.DeliveryDate).ShowInList(width: 120);
                View.Property(p => p.DesignStatus).ShowInList(width: 120);
                View.Property(p => p.ProduceStatus).ShowInList(width: 120);
                View.AttachChildrenProperty(typeof(ProjectWorkOrderViewModel), e =>
                {
                    var args = e as ChildPagingDataArgs;
                    var report = args.Parent.CastTo<ProjectDesignReport>();
                    if (report == null)
                    {
                        return new EntityList<ProjectWorkOrderViewModel>();
                    }
                    return RT.Service.Resolve<ProjectDesignReportController>().GetProjectWorkOrderViewModels(report.Id, args.PagingInfo);
                });
                View.AttachChildrenProperty(typeof(ProjectOutsourcingViewModel), e =>
                {
                    var args = e as ChildPagingDataArgs;
                    var report = args.Parent.CastTo<ProjectDesignReport>();
                    if (report == null)
                    {
                        return new EntityList<ProjectOutsourcingViewModel>();
                    }
                    return RT.Service.Resolve<ProjectDesignReportController>().GetProjectOutsourcingViewModels(report.Id, args.PagingInfo);
                });
            }
        }
    }
}
