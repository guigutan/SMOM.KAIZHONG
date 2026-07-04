using SIE.Dock.DockRunMts;
using SIE.MetaModel.View;

namespace SIE.Web.Dock.DockRunMts
{
	/// <summary>
	/// 工作时段视图配置
	/// </summary>
	internal class WorkTimeViewConfig : WebViewConfig<WorkTime>
	{
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.ClearCommands();
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);
            using (View.OrderProperties())
			{
                View.Property(p => p.BeginTime).UseTimeEditor();
                View.Property(p => p.EndTime).UseTimeEditor();
                View.Property(p => p.Remark);
            }
		}
	}
}