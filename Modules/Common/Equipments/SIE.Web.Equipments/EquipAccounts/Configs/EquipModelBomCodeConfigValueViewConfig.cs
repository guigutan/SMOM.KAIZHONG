using SIE.Equipments.Configs;

namespace SIE.Web.Equipments.EquipAccounts.Configs
{
    /// <summary>
    /// 设备型号BOM编码视图
    /// </summary>
    public class EquipModelBomCodeConfigValueViewConfig : WebViewConfig<EquipModelBomCodeConfigValue>
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
