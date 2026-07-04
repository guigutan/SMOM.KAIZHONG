using SIE.EMS.EquipRepair.EquipRepairs;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs
{
    /// <summary>
    /// 设备维修单维修规程视图配置
    /// </summary>
    public class EquipRepairBillProjectViewConfig : WebViewConfig<EquipRepairBillProject>
	{

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.FormEdit();
			View.ClearCommands();
			using (View.OrderProperties())
			{
				View.Property(p => p.ProjectDetailId).HasLabel("项目名称").Show(ShowInWhere.All);
				View.Property(p => p.Part).Show(ShowInWhere.All);
				View.Property(p => p.Consumable).Show(ShowInWhere.All);
				View.Property(p => p.Method).Show(ShowInWhere.All);
				View.Property(p => p.Standard).Show(ShowInWhere.All);
				View.Property(p => p.MinValue).UseSpinEditor().Show(ShowInWhere.All);
				View.Property(p => p.MaxValue).UseSpinEditor().Show(ShowInWhere.All);
				View.Property(p => p.Unit).Show(ShowInWhere.All);
				View.Property(p => p.UseTime).Show(ShowInWhere.All);
			}
		}
	}
}