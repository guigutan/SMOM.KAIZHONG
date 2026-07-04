using SIE.MES.ProjectDesigns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns
{
    /// <summary>
    /// 项目号需求设计操作日志视图配置
    /// </summary>
    public class ProjectDesignLogViewConfig : WebViewConfig<ProjectDesignLog>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.OperatePoint).ShowInList(width: 200);
                View.Property(p => p.Operater).ShowInList(width: 150);
                View.Property(p => p.OperateTime).ShowInList(width: 150);
                View.Property(p => p.OtherRemark).ShowInList(width: 150);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
