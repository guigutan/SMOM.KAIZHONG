using SIE.EMS.SpecialEquipment.RegularInspections; 

namespace SIE.Web.EMS.SpecialEquipment.RegularInspections
{
	/// <summary>
	/// 设备定检附件视图配置
	/// </summary>
	[ManagedProperty.CompiledPropertyDeclarer]
	internal class RegularInspectionAttachmentViewConfig : WebViewConfig<RegularInspectionAttachment>
	{
		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.Property(p => p.FilePath).ShowInList(width: 20 * 25).Readonly();
			View.Property(p => p.FileName).ShowInList(width: 20 * 12).Readonly();
		}
	}
}