using SIE.MES.WorkOrders.Configs;

namespace SIE.Web.MES.WorkOrders.Configs
{
    /// <summary>
    /// 工单工序bom属性配置试图
    /// </summary>
    public class ReferenceWoBomConfigValueViewConfig : WebViewConfig<ReferenceWoBomConfigValue>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ReferenceWoBom).Show(ShowInWhere.All);
            }
        }
    }
}
