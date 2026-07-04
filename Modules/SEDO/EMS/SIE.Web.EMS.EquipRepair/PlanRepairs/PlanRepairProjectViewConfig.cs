using SIE.EMS.EquipRepair.PlanRepairs;
using SIE.MetaModel.View;
using SIE.Web.EMS.EquipRepair.PlanRepairs.Commands;

namespace SIE.Web.EMS.EquipRepair.PlanRepairs
{
	/// <summary>
	/// 维修规程视图配置
	/// </summary>
	public class PlanRepairProjectViewConfig : WebViewConfig<PlanRepairProject>
	{
		/// <summary>
		/// 编辑视图
		/// </summary>
		public const string EditView = "EditView";

		/// <summary>
		/// 配置视图
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup(EditView);
			if (ViewGroup == EditView)
			{
				DetailListView();
			}
		}

		/// <summary>
		/// 编辑列表视图
		/// </summary>
		protected void DetailListView()
		{
			View.UseCommands(typeof(SelPlanRepairsProjectCommand).FullName,WebCommandNames.Delete);
			using (View.OrderProperties())
			{
				View.DisableEditing();
				View.Property(p => p.ProjectDetail).ShowInList(120);				
				View.Property(p => p.DepartmentId).HasLabel("责任部门").ShowInList(80);
				View.Property(p => p.Part).ShowInList(80);
				View.Property(p => p.Consumable).ShowInList(80);
				View.Property(p => p.Method).ShowInList(150);
				View.Property(p => p.Standard).ShowInList(150);
				View.Property(p => p.MinValue).ShowInList(80);
				View.Property(p => p.MaxValue).ShowInList(80);
				View.Property(p => p.Unit).ShowInList(60);
				View.Property(p => p.UseTime).ShowInList(120);
				View.Property(p => p.CreateBy);
			}

		}

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.DisableEditing();
			View.Property(p => p.ProjectDetail);			
			View.Property(p => p.DepartmentId).HasLabel("责任部门");
			View.Property(p => p.Part);
			View.Property(p => p.Consumable);
			View.Property(p => p.Method);
			View.Property(p => p.Standard);
			View.Property(p => p.MinValue);
			View.Property(p => p.MaxValue);
			View.Property(p => p.Unit);
			View.Property(p => p.UseTime);
			View.Property(p => p.CreateBy);
		}
	}
}