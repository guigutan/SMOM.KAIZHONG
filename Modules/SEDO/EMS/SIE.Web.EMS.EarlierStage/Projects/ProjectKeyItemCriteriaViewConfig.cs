using SIE.Domain;
using SIE.EMS.EarlierStage.Projects;
using SIE.Web.Common;
using SIE.Web.Core;
using SIE.Web.EMS.Extensions;
using SIE.Web.Resources;

namespace SIE.Web.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 项目关键事项查询实体-界面
    /// </summary>
    internal class ProjectKeyItemCriteriaViewConfig : WebViewConfig<ProjectKeyItemCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.AddBehavior("SIE.Web.EMS.EarlierStage.Projects.ProjectKeyItemCriteriaBehavior");
            View.Property(p => p.FactoryId).UseFactoryEditor();
            View.Property(p => p.DepartmentId).UseUserBudgetDepartmentEditor();
            View.Property(p => p.No);
            View.Property(p => p.ProjectType).UseCatalogEditor(e => { e.CatalogType = Project.ProjectClassify; e.CatalogReloadData = true; });
            View.Property(p => p.Year).UseYearEditor();
            View.Property(p => p.ProjectStatus);
            View.Property(p => p.ApprovalStatus).HasLabel("审核状态");
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
            View.Property(p => p.BudgetNo).Show(ShowInWhere.Hide);
        }
    }
}
