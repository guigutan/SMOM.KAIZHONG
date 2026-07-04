using SIE.EMS.SpecialEquipment.RegularInspections.ViewModels;

namespace SIE.Web.EMS.SpecialEquipment.RegularInspections.ViewModels
{
    /// <summary>
    /// 设备定检附件视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
	internal class SelInspectionProjectViewModelViewConfig : WebViewConfig<SelInspectionProjectViewModel> 
    {
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.Property(p => p.Name).HasLabel("项目名称").Show(ShowInWhere.All).Readonly();
			View.Property(p => p.Part).HasLabel("部位").Show(ShowInWhere.All).Readonly();
			View.Property(p => p.Consumable).HasLabel("项目耗材").Show(ShowInWhere.All).Readonly();
			View.Property(p => p.Method).Show(ShowInWhere.All).Readonly();
			View.Property(p => p.Standard).Show(ShowInWhere.All);
			View.Property(p => p.MinValue).HasLabel("最小值").Show(ShowInWhere.All);
			View.Property(p => p.MaxValue).HasLabel("最大值").Show(ShowInWhere.All);
			View.Property(p => p.Unit).Show(ShowInWhere.All).Readonly();
			View.Property(p => p.UseTime).Show(ShowInWhere.All).Readonly();
			View.Property(p => p.CycleType).HasLabel("周期类型").Show(ShowInWhere.All).Readonly();
		}
	}
}
