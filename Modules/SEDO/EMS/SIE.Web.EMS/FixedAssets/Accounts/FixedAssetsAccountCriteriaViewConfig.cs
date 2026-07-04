using SIE.Equipments.EquipTypes;
using SIE.EMS.FixedAssets.Accounts;
using SIE.Web.Common;
using SIE.Equipments.EquipAccounts;

namespace SIE.Web.EMS.FixedAssets.Accounts
{
    /// <summary>
    /// 固定资产查询台账配置页面
    /// </summary>
    public class FixedAssetsAccountCriteriaViewConfig : WebViewConfig<FixedAssetsAccountCriteria>
    {
        /// <summary>
        /// 配置查询
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.AssetsCategory).UseCatalogEditor(p => { p.CatalogType = EquipType.EquipTypeCatalogType; p.CatalogReloadData = true; });
            View.Property(p => p.AssetsSource);
            View.Property(p => p.ManageStatus);
            View.Property(p => p.CreationDate).UseDateRangeEditor(p=>p.DateRangeType= ObjectModel.DateRangeType.All);
        }
    }
}
