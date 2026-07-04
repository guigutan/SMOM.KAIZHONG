using SIE.EMS.EquipRepair.ExperienceDepots;
using SIE.Web.Common;

namespace SIE.Web.EMS.EquipRepair.Repairs.ExperienceDepots
{
    /// <summary>
    /// 经验库查询视图
    /// </summary>
    public class ExperienceDepotCriteriaViewConfig : WebViewConfig<ExperienceDepotCriteria>
    {
        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.EquipAccountCode).Show(ShowInWhere.All);
            View.Property(p => p.EquipAccountName).Show(ShowInWhere.All);
            View.Property(p => p.EquipModelCode).Show(ShowInWhere.All);
            View.Property(p => p.SparePartCode).Show(ShowInWhere.All);
            View.Property(p => p.SparePartName).Show(ShowInWhere.All);
            View.Property(p => p.RepairNo).Show(ShowInWhere.All);
            View.Property(p => p.FaultPhenomenonId).Show(ShowInWhere.All);
            View.Property(p => p.FaultReson).UseCatalogEditor(p => { p.CatalogType = ExperienceDepotCriteria.expFaultReson; p.CatalogReloadData = true; });
            View.Property(p => p.EquipLargeFault).Show(ShowInWhere.All);
        }
    }
}
