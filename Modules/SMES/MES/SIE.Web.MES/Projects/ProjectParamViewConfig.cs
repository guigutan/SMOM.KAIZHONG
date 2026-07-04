using SIE.MES.Projects;
using SIE.MetaModel.View;
using SIE.Web.Common;
using SIE.Web.MES.Projects.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Projects
{
    /// <summary>
    /// 项目参数视图配置
    /// </summary>
    public class ProjectParamViewConfig : WebViewConfig<ProjectParam>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, typeof(ProjectParamSaveCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).ShowInList(width: 120);
                View.Property(p => p.Name).ShowInList(width: 120);
                View.Property(p => p.Type).UseCatalogEditor(p => { p.CatalogType = ProjectParam.ProjectParamTypeCata; p.CatalogReloadData = true; }).ShowInList(width: 120);
                View.Property(p => p.Description).ShowInList(width: 120);
            }
        }

        /// <summary>
        /// 选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Readonly().ShowInList(width: 120);
                View.Property(p => p.Name).Readonly().ShowInList(width: 120);
                View.Property(p => p.Type).Readonly().ShowInList(width: 120);
            }
        }
    }
}
