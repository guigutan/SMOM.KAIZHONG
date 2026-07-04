using SIE.MES.ProjectDesigns.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.Reports
{
    /// <summary>
    /// 项目号关联工序委外需求单
    /// </summary>
    public class ProjectOutsourcingViewModelViewConfig : WebViewConfig<ProjectOutsourcingViewModel>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.No).ShowInList(width: 150);
                View.Property(p => p.WoNo).ShowInList(width: 150);
                View.Property(p => p.ProductCode).ShowInList(width: 120);
                View.Property(p => p.ProductName).ShowInList(width: 120);
                View.Property(p => p.OutsourcingState).ShowInList();
                View.Property(p => p.RequestQty).ShowInList();
                View.Property(p => p.OutboundQty).ShowInList();
                View.Property(p => p.WarehousingQty).ShowInList();
                View.Property(p => p.BeginProcess).ShowInList(width: 120);
                View.Property(p => p.EndProcess).ShowInList(width: 120);
            }
        }
    }
}
