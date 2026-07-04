using SIE.MES.Projects;
using SIE.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Projects
{
    /// <summary>
    /// 项目参数查询实体视图配置
    /// </summary>
    public class ProjectParamCriteriaViewConfig : WebViewConfig<ProjectParamCriteria>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code).Show();
            View.Property(p => p.Name).Show();
            View.Property(p => p.Type).UseCatalogEditor(p => { p.CatalogType = ProjectParam.ProjectParamTypeCata; p.CatalogReloadData = true; }).Show();
        }
    }
}
