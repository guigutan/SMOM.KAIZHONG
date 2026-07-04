using SIE.EMS.IdleArchives.Configs;

namespace SIE.Web.EMS.IdleArchives.Configs
{
    /// <summary>
    /// 配置项视图
    /// </summary>
    public class IdleArchivesMaximumTermViewConfig : WebViewConfig<IdleArchivesMaximumTermConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.MaximumTerm).UseSpinEditor(m=> { m.MinValue = 0; m.AllowDecimals = false; }).Show();
        }
    }
}
