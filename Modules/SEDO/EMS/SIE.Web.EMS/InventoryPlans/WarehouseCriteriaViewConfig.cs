using SIE.EMS.InventoryPlans;
using SIE.MetaModel.View;
using SIE.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.EMS.InventoryPlans
{
    /// <summary>InventoryPlanSparePart
    /// 配置仓库查询实体视图
    /// </summary>
    internal class WarehouseCriteriaViewConfig : WebViewConfig<WarehouseCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>

        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
                View.Property(p => p.LibraryType).UseEnumEditor(p => p.IsEnumNull = true).Show();
                View.Property(p => p.Category).UseCatalogEditor(e => { e.CatalogType = Warehouse.CatalogCategory; e.CatalogReloadData = true; }).Show()
                    .UseListSetting(e => { e.HelpInfo = "仓库分类快码类型(WAREHOUSE_TYPE)"; });
                View.Property(p => p.IsFrozen).UseCheckDropDownEditor(p => p.AllowBlank = true).Show();
            }
        }
    }
}

