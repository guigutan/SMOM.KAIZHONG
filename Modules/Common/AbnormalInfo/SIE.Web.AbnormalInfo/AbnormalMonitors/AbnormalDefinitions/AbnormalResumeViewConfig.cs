using SIE.AbnormalInfo.AbnormalMonitors;

namespace SIE.Web.AbnormalInfo.AnomalyMonitors
{
	/// <summary>
	/// 异常履历视图配置
	/// </summary>
	internal class AbnormalResumeViewConfig : WebViewConfig<AbnormalResume>
	{
		
		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{ 	  
			View.ClearCommands(); 
			View.Property(p => p.AbnormalTask);
		}
	}
}