using SIE.EMS.AssetScraps;
using SIE.EMS.InventoryBalances.Configs;
using SIE.Web.Common;

namespace SIE.Web.EMS.InventoryBalances.Configs
{
    /// <summary>
    /// 报废类型配置
    /// </summary>
    internal class BalanceScrapTypeConfigValueViewConfig : WebViewConfig<BalanceScrapTypeConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.ScrapType).UseCatalogEditor(e => { e.CatalogType = AssetScrapEquipment.EquipScrapType; e.ReloadDataOnPopping = true; }).Show(ShowInWhere.All);
        }
    }
}
