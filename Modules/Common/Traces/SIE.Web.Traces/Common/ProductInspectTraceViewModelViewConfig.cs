using SIE.Traces.Common; 

namespace SIE.Web.Traces.Common
{
	/// <summary>
	/// 产品检验追溯视图配置
	/// </summary>
	internal class ProductInspectTraceViewModelViewConfig : WebViewConfig<ProductInspectTraceViewModel>
	{
		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
            View.DisableEditing();
            View.ClearCommands(); 
			View.Property(p => p.Name);
			View.Property(p => p.LimitMax);
			View.Property(p => p.LimitLow);
			View.Property(p => p.InspectionValue);
			View.Property(p => p.Result);
			View.Property(p => p.Remarks);
		}
	}
}