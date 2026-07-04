using SIE.Core.ProjectMaintains;
using SIE.Items;
using SIE.MES.ProjectDesigns.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.Reports
{
    /// <summary>
    /// 项目号跟踪报表查询实体视图配置
    /// </summary>
    public class ProjectDesignReportCriteriaViewConfig : WebViewConfig<ProjectDesignReportCriteria>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ProjectMaintain).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ProjectMaintainController>().GetProjectMaintains(p, k);
                }).Show();
                View.Property(p => p.Product).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ItemController>().GetItemList(k, p);
                }).Show();
                View.Property(p => p.SaleOrderNo).Show();
                View.Property(p => p.DesignStatus).Show();
                View.Property(p => p.ProduceStatus).Show();
                View.Property(p => p.DeliveryDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Month).Show();
            }
        }
    }
}
