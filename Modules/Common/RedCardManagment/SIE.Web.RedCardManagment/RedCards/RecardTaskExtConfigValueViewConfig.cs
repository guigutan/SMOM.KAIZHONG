using SIE.RedCardManagment.RedCards.Config;

namespace SIE.Web.RedCardManagment.RedCards
{
	/// <summary>
	/// 数据列配置项视图配置
	/// </summary>
	internal class RecardTaskExtConfigValueViewConfig : WebViewConfig<RecardTaskExtConfigValue>
    {

        /// <summary>
        /// 表单视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.IsDisabled);
        }
    }
}
