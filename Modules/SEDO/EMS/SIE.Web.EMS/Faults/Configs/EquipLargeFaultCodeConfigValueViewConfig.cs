using SIE.EMS.Faults.Configs;

namespace SIE.Web.EMS.Configs
{
    /// <summary>
    /// 配置故障大类视图
    /// </summary>
    public class EquipLargeFaultCodeConfigValueViewConfig : WebViewConfig<EquipLargeFaultCodeConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.NumberRule).UsePagingLookUpEditor();
        }
    }
}
