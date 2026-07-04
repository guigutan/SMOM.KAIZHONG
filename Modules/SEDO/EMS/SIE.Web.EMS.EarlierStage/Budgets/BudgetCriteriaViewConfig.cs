using SIE.Domain;
using SIE.EMS.EarlierStage.Budgets;
using SIE.Web.Common;
using SIE.Web.Core;
using SIE.Web.EMS.Extensions;
using SIE.Web.Resources;

namespace SIE.Web.EMS.EarlierStage.Budgets
{
    /// <summary>
    /// 预算查询界面
    /// </summary>
    internal class BudgetCriteriaViewConfig : WebViewConfig<BudgetCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.FactoryId).UseFactoryEditor();
            View.Property(p => p.DepartmentId).UseUserBudgetDepartmentEditor();
            View.Property(p => p.BudgetNo);
            View.Property(p => p.BudgeGrade);
            View.Property(p => p.InvestClass).UseCatalogEditor(e => { e.CatalogType = Budget.InvestClassify; e.CatalogReloadData = true; });
            View.Property(p => p.Year).UseYearEditor();
            View.Property(p => p.ApprovalStatus);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}
