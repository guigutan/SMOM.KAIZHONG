using SIE.EMS.Common.Configs;

namespace SIE.Web.EMS.Common
{
    /// <summary>
    /// 数据列配置项视图配置
    /// </summary>
    internal class ValueConfigValueViewConfig : WebViewConfig<ValueConfigValue>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
        }

        /// <summary>
        /// 表单视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.Qty);
        }
    }
}
