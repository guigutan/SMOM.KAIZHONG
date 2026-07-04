using SIE.Core.ProjectMaintains;
using SIE.Items;
using SIE.MES.ProjectDesigns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns
{
    /// <summary>
    /// 项目号需求设计
    /// </summary>
    public class ProjectDesignCriteriaViewConfig : WebViewConfig<ProjectDesignCriteria>
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
                View.Property(p => p.State).Show();
                View.Property(p => p.ExamineStatus).Show();
                View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Month).Show();
            }
        }
    }
}
