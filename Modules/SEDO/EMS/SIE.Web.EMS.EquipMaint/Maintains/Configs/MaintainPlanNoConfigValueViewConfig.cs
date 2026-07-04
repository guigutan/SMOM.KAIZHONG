using SIE.EMS.Maintains.Configs;

namespace SIE.Web.EMS.EquipMaint.Maintains.Configs
{
    /// <summary>
    /// 保养单号配置值界面
    /// </summary>
    public class MaintainPlanNoConfigValueViewConfig : WebViewConfig<MaintainPlanNoConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.NumberRule);
        }
    }
}
