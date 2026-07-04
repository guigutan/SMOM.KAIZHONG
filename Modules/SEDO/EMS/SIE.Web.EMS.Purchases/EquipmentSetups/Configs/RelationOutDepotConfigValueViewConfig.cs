using SIE.EMS.Purchases.EquipmentSetups.Configs;

namespace SIE.Web.EMS.Purchases.EquipmentSetups.Configs
{
    /// <summary>
    /// 备件使用是否必须关联出库单
    /// </summary>
    internal class RelationOutDepotConfigValueViewConfig : WebViewConfig<RelationOutDepotConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.Relation).Show(ShowInWhere.All);
        }
    }
}
