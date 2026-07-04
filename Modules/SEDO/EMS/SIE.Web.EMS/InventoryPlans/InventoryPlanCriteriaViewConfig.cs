using SIE.Domain;
using SIE.EMS.InventoryPlans;
using SIE.Resources.Employees;
using SIE.Web.Common;
using SIE.Web.Resources;

namespace SIE.Web.EMS.InventoryPlans
{
    /// <summary>
    /// 盘点计划查询
    /// </summary>
    internal class InventoryPlanCriteriaViewConfig : WebViewConfig<InventoryPlanCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.PlanNo);
            View.Property(p => p.FactoryId).UseFactoryEditor();
            View.Property(p => p.InventoryAssetObject);
            View.Property(p => p.InventoryType).UseCatalogEditor(e => { e.CatalogType = InventoryPlan.InventoryTypeCatalog; e.CatalogReloadData = true; });
            View.Property(p => p.ResponsibleId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
            });
            View.Property(p => p.ApprovalStatus);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}
