using SIE.EMS.SpareParts.Configs;

namespace SIE.Web.EMS.Fixtures.Configs
{
    /// <summary>
    ///  备件入库单号视图配置
    /// </summary>
    public class SparePartStoreNoConfigValueViewConfig : WebViewConfig<SparePartStoreNoConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.CodeRule);
        }
    }
}
