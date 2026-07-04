using SIE.EMS.Equipments.Boms;

namespace SIE.Web.EMS.SpecialEquipment.SpecialEquipmentAcounts.ExtensionViewConfig
{
    /// <summary>
    /// 扩展设备BOM明细视图
    /// </summary>
    public class  EquipBomDetailExtensionViewConfig : WebViewConfig<EquipBomDetail>
    {
		/// <summary>
		/// 设备台账设备BOM明细视图
		/// </summary>
		public readonly static string AccountEquipBomDetailViewGroup = "AccountEquipBomDetailViewGroup";

		///<summary>
		/// 配置视图
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup(new string[] { AccountEquipBomDetailViewGroup });

			if (ViewGroup == AccountEquipBomDetailViewGroup)
			{
				ConfigAccountEquipBomDetailView();
			}
		}
		/// <summary>
		/// 配置特种设备台账设备BOM明细视图
		/// </summary>
		public void ConfigAccountEquipBomDetailView()
		{
			using (View.OrderProperties())
			{
				View.Property(p => p.SparePartCode).Readonly().Show();
				View.Property(p => p.SparePartName).Readonly().Show();
				View.Property(p => p.SparePartType).Readonly().Show();
				View.Property(p => p.SparePartQty).Readonly().Show();
				View.Property(p => p.StockQty).Readonly().Show();
				View.Property(p => p.UnitName).Readonly().Show();
			}
		}
	}
}
