using SIE.AbnormalInfo.AbnormalMonitors;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors.AbnomalRule
{
    /// <summary>
    /// 异常来源视图配置
    /// </summary>
    internal class AbnormalEntityMetadataViewConfig : WebViewConfig<AbnormalEntityMetadata>
    {


		///<summary>
		/// 配置下拉视图
		/// </summary>
		protected override void ConfigSelectionView()
        {
            View.Property(p => p.Name).ShowInList(width: 150);
            View.Property(p => p.Type).ShowInList(width: 350).Readonly();
            View.Property(p => p.TableName).ShowInList(width: 150).Readonly();
        }
    }
}