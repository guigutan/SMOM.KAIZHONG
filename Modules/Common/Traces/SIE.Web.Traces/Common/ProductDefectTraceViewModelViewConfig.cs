using SIE.Traces.Common; 

namespace SIE.Web.Traces.Common
{
	/// <summary>
	/// 产品缺陷追溯视图配置
	/// </summary>
	internal class ProductDefectTraceViewModelViewConfig : WebViewConfig<ProductDefectTraceViewModel>
	{
		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
            View.DisableEditing();
            View.ClearCommands(); 
			View.Property(p => p.Process);
			View.Property(p => p.DefectCode);
			View.Property(p => p.DefectDescription);
			View.Property(p => p.InspItemName);
			View.Property(p => p.BoardNo);
			View.Property(p => p.Sn);
			View.Property(p => p.Location);
			View.Property(p => p.FixedBy);
			View.Property(p => p.IsMisjudgment);
		}
	}
}