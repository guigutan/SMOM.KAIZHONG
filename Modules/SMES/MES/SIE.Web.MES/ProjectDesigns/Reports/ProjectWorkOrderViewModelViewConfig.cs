using SIE.MES.ProjectDesigns.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.Reports
{
    /// <summary>
    /// 项目号关联工单视图配置
    /// </summary>
    public class ProjectWorkOrderViewModelViewConfig : WebViewConfig<ProjectWorkOrderViewModel>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.UseClientOrder();
            using (View.OrderProperties())
            {
                View.Property(p => p.WoNo).ShowInList(width: 150);
                View.Property(p => p.ProductCode).ShowInList(width: 120);
                View.Property(p => p.ProductName).ShowInList(width: 120);
                View.Property(p => p.WorkOrderState).ShowInList();
                View.Property(p => p.PlanQty).ShowInList();
                View.Property(p => p.FinishQty).ShowInList();
                View.Property(p => p.PlanBeginDate).ShowInList(width: 150);
                View.Property(p => p.PlanEndDate).ShowInList(width: 150);
            }
        }
    }
}
