using SIE.Traces.ForwardTraces; 

namespace SIE.Web.Traces.ForwardTraces
{
	/// <summary>
	/// 工序采集记录追溯视图配置
	/// </summary>
	internal class MesProcessKeyItemFwdViewModelViewConfig : WebViewConfig<MesProcessKeyItemFwdViewModel>
	{
		
		///<summary>
		/// 配置列表视图
		/// </summary>算
		protected override void ConfigListView()
		{
            View.DisableEditing();
            View.ClearCommands(); 
			View.Property(p => p.CollectSn);
			View.Property(p => p.StationName);
			View.Property(p => p.ProcessName);
			View.Property(p => p.Qty);
			View.Property(p => p.CollectBy);
			View.Property(p => p.CollectTime);
		}
	}
}