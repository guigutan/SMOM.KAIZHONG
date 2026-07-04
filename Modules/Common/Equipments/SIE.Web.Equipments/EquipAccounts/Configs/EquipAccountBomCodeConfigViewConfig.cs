using SIE.Equipments.Configs;

namespace SIE.Web.Equipments.EquipAccounts.Configs
{
    /// <summary>
    /// 设备型号BOM编码配置值视图
    /// </summary>
    public class EquipAccountBomCodeConfigViewConfig : WebViewConfig<EquipAccountBomCodeConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.NumberRule);
        }
    }
}
