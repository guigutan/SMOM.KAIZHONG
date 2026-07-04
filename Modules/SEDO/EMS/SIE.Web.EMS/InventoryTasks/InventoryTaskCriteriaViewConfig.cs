using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.InventoryPlans;
using SIE.EMS.InventoryTasks;
using SIE.Resources.Employees;
using SIE.Web.Common;
using SIE.Web.Resources;
using System;

namespace SIE.Web.EMS.InventoryTasks
{
    /// <summary>
    /// 盘点任务查询
    /// </summary>
    internal class InventoryTaskCriteriaViewConfig : WebViewConfig<InventoryTaskCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.TaskNo);
            View.Property(p => p.FactoryId).UseFactoryEditor();
            View.Property(p => p.PlanNo);
            View.Property(p => p.InventoryAssetObject).DefaultValue(InventoryAssetObject.Equipment).HasLabel("资产对象".L10N()+"*");
            View.Property(p => p.InventoryType).UseCatalogEditor(e => { e.CatalogType = InventoryPlan.InventoryTypeCatalog; e.CatalogReloadData = true; });
            View.Property(p => p.ResponsibleId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
            });
            View.Property(p => p.InventoryTaskStatus);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}
