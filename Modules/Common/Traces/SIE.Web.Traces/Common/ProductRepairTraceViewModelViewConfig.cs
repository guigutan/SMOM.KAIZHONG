using SIE.Traces.Common; 

namespace SIE.Web.Traces.Common
{
	/// <summary>
	/// 产品维修记录视图配置
	/// </summary>
	internal class ProductRepairTraceViewModelViewConfig : WebViewConfig<ProductRepairTraceViewModel>
	{
		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
            View.DisableEditing();
            View.ClearCommands(); 
			View.Property(p => p.RepairType);
			View.Property(p => p.RepairBy);
			View.Property(p => p.RepairTime);
			View.Property(p => p.RepairProcess);
			View.Property(p => p.RepairStation);
			View.Property(p => p.DefectDes);
			View.Property(p => p.DefectRemark);
		}
	}
}