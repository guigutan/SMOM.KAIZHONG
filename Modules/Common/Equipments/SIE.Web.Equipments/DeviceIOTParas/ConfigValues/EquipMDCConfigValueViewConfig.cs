using SIE.Equipments.DeviceIOTParas.ConfigValues;

namespace SIE.Web.Equipments.DeviceIOTParas.ConfigValues
{
    /// <summary>
    /// 视图配置
    /// </summary>
    public class EquipMDCConfigValueViewConfig : WebViewConfig<EquipMDCConfigValue>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.Url).Show();
        }
    }
}
