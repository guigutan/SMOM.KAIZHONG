using SIE.Equipments.DeviceIOTParas.Details;
using SIE.MetaModel.View;

namespace SIE.Web.Equipments.DeviceIOTParas.Details
{
    /// <summary>
    /// 设备清单视图配置
    /// </summary>
    internal class FacilityDetailViewConfig : WebViewConfig<FacilityDetail>
	{
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{			
			View.UseCommands(
                "SIE.Web.Equipments.DeviceIOTParas.Commands.SelectStandBookCommand",
                WebCommandNames.Delete);
			View.Property(p => p.EquipAccount).Readonly();
			View.Property(p => p.EquipmentName).Readonly();
			View.Property(p => p.UnitType).Readonly();
			View.Property(p => p.ModelName).Readonly();
			View.Property(p => p.DeviceType).Readonly();
            View.Property(p => p.Workshop).Readonly();
            View.Property(p => p.ProductLine).Readonly();
			View.Property(p => p.Process).Readonly();
			View.Property(p => p.Local).Readonly();
		}
	}
}
