using SIE.Traces.Common; 

namespace SIE.Web.Traces.Common
{
	/// <summary>
	/// 包装信息追溯视图配置
	/// </summary>
	internal class PackageTraceViewModelViewConfig : WebViewConfig<PackageTraceViewModel>
	{
		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.DisableEditing();
			View.ClearCommands(); 
			View.Property(p => p.PackageNo).ShowInList(width:200).DisableSort();
			View.Property(p => p.PackageUnitName).DisableSort();
			View.Property(p => p.Qty).DisableSort();
			View.Property(p => p.PackageTime).DisableSort();
			View.Property(p => p.WarehouseName).DisableSort();
			View.Property(p => p.StationName).DisableSort();
			View.Property(p => p.ShippingOrderNo).DisableSort();
			View.Property(p => p.CustomerName).DisableSort();
			View.Property(p => p.DeliveryDate).DisableSort();
		}
	}
}