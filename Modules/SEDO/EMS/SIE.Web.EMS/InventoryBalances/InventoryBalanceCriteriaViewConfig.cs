using SIE.EMS.Enums;
using SIE.EMS.InventoryBalances;
using SIE.EMS.InventoryPlans;
using SIE.Web.Common;
using SIE.Web.Resources;

namespace SIE.Web.EMS.InventoryBalances
{
    /// <summary>
    /// 盘点平账查询
    /// </summary>
    internal class InventoryBalanceCriteriaViewConfig : WebViewConfig<InventoryBalanceCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.TaskNo);
            View.Property(p => p.FactoryId).UseFactoryEditor();
            View.Property(p => p.PlanNo);
            View.Property(p => p.InventoryAssetObject).DefaultValue(InventoryAssetObject.Equipment);
            View.Property(p => p.InventoryType).UseCatalogEditor(e => { e.CatalogType = InventoryPlan.InventoryTypeCatalog; e.ReloadDataOnPopping = true; });
            View.Property(p => p.ResponsibleId);
            View.Property(p => p.InventoryTaskStatus).Show(ShowInWhere.Hide);
            View.Property(p => p.ApprovalStatus);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}
