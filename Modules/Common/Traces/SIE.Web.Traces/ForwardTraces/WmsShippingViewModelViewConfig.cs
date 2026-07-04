using SIE.Traces.ForwardTraces; 

namespace SIE.Web.Traces.ForwardTraces
{
	/// <summary>
	/// 库存发运单信息追溯视图配置
	/// </summary>
	internal class WmsShippingViewModelViewConfig : WebViewConfig<WmsShippingViewModel>
	{
		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
            View.DisableEditing();
            View.ClearCommands(); 
			View.Property(p => p.ShipQty);
			View.Property(p => p.ShippingOrderNo);
			View.Property(p => p.ReceiveByName);
			View.Property(p => p.ReceiveTime);
			View.Property(p => p.FactoryName);
			View.Property(p => p.WorkShopName);
			View.Property(p => p.ResourceName);
			View.Property(p => p.WorkOrderNo);
		}
	}
}