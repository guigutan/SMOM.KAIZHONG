using SIE.Kit.MES.CallMaterials.Configs;

namespace SIE.Web.Kit.MES.CallMaterials.Configs
{
    /// <summary>
    /// 仓库 视图配置
    /// </summary>
    public class WareHouseConfigValueViewConfig : WebViewConfig<WareHouseConfigValue>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.Warehouse).ShowInDetail();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.Warehouse);
        }
    }
}
