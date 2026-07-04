using SIE.MetaModel.View;
using SIE.Packages.Boxs; 

namespace SIE.WPF.Packages.Boxs
{
	/// <summary>
	/// 操作日志视图配置
	/// </summary>
	internal class TurnoverBoxActionLogViewConfig : WebViewConfig<TurnoverBoxActionLog>
	{
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.ClearCommands();
			View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
			View.Property(p => p.Sn);
			View.Property(p => p.Qty);
			View.Property(p => p.TurnoverType);
			View.Property(p => p.Item);
		}
	}
}