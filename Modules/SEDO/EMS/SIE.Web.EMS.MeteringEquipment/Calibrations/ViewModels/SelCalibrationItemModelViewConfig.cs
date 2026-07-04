using SIE.EMS.MeteringEquipment.Calibrations.ViewModels;

namespace SIE.Web.EMS.MeteringEquipment.Calibrations.ViewModels
{
    /// <summary>
    /// 选择点检项目视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class SelCalibrationItemModelViewConfig : WebViewConfig<SelCalibrationItemModel>
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
