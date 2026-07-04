using SIE.EMS.SpecialEquipment.RegularInspections; 

namespace SIE.Web.EMS.SpecialEquipment.RegularInspections
{
	/// <summary>
	/// 定检检验数据视图配置
	/// </summary>
	internal class RegularInspectionValueViewConfig : WebViewConfig<RegularInspectionValue>
	{
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{ 	  
			View.UseDefaultCommands();
			View.Property(p => p.Index);
			View.Property(p => p.Value);
		}
	}
}