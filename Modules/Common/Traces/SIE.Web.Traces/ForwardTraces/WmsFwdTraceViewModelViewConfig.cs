using Newtonsoft.Json;
using SIE.Traces.ForwardTraces; 

namespace SIE.Web.Traces.ForwardTraces
{
	/// <summary>
	/// 库存追溯视图配置
	/// </summary>
	internal class WmsFwdTraceViewModelViewConfig : WebViewConfig<WmsFwdTraceViewModel>
	{
		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
            View.DisableEditing();
            View.ClearCommands();
            View.UseChildrenAsHorizontal();
            View.Property(p => p.WarehouseCode);
			View.Property(p => p.StorageLocationCode);
			View.Property(p => p.OnhandState);
			View.Property(p => p.Qty);
			View.Property(p => p.AsnNo);
			View.Property(p => p.SupplierName);
			View.Property(p => p.ProductionDate);
			View.Property(p => p.CollectDate);
            View.AttachChildrenProperty(typeof(WmsShippingViewModel), o =>
            {
                var args = o as ChildPagingDataWithParentEntityArgs;
                WmsFwdTraceViewModel parent = JsonConvert.DeserializeObject<WmsFwdTraceViewModel>(args.ParentEntity);
                var result = RT.Service.Resolve<ForwardTraceController>().GetWmsShippingTraceDatas(parent, args.PagingInfo);
                return result;
            }, WmsShippingViewModelViewConfig.ListView);
		}
	}
}