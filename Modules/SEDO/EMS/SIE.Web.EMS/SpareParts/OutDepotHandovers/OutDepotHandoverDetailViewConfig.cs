using SIE.EMS.SpareParts.OutDepotHandovers;
using SIE.EMS.SpareParts.OutDepots;

namespace SIE.Web.EMS.SpareParts.OutDepotHandovers
{
	/// <summary>
	/// 备件交接明细视图配置
	/// </summary>
	public class OutDepotHandoverDetailViewConfig : WebViewConfig<OutDepotHandoverDetail>
	{
		/// <summary>
		/// 出库单接收明细视图
		/// </summary>
		public const string OutDepotHandoverDetailViewGroup = "OutDepotHandoverDetailViewGroup";

		/// <summary>
		/// 扫描交接-接收明细视图
		/// </summary>
		public const string ScanOutDepotHandoverDetailViewGroup = "ScanOutDepotHandoverDetailViewGroup";

		///<summary>
		/// 配置视图
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup(new string[] { OutDepotHandoverDetailViewGroup, ScanOutDepotHandoverDetailViewGroup });

			if (ViewGroup == OutDepotHandoverDetailViewGroup)
			{
				ConfigOutDepotHandoverDetailView();
			}

			if (ViewGroup == ScanOutDepotHandoverDetailViewGroup)
			{
				ConfigScanOutDepotHandoverDetailView();
			}
		}

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.AddBehavior("SIE.Web.EMS.SpareParts.OutDepotHandovers.Behaviors.HandoverDetailComandBehavior");
			View.UseCommand("SIE.Web.EMS.SpareParts.OutDepotHandovers.Commands.SearchHandoverDetailCommand");
			View.DisableEditing();
			View.Property(p => p.SparePartCode);
			View.Property(p => p.SparePartName);
			View.Property(p => p.Specification);
			View.Property(p => p.SpartType);
			View.Property(p => p.ControlMethod);
			View.Property(p => p.IsReplacement).Readonly();
			View.Property(p => p.BatchNo).ShowInList(width: 120);
			View.Property(p => p.SeriaNo).ShowInList(width: 120);
			View.Property(p => p.Qty);
			View.Property(p => p.ReceiveQty);
			View.Property(p => p.HandOverStatus);
			View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
			View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
			View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
			View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
		}

		///<summary>
		/// 配置出库单接收明细视图
		/// </summary>
		protected void ConfigOutDepotHandoverDetailView()
		{
			View.AddBehavior("SIE.Web.EMS.SpareParts.OutDepots.Behaviors.HandoverDetailComandBehavior");
			View.UseCommand("SIE.Web.EMS.SpareParts.OutDepots.Commands.SearchHandoverDetailCommand");
			View.AssignAuthorize(typeof(OutDepot));
			View.DisableEditing();
			using (View.OrderProperties()) 
			{
				View.Property(p => p.SparePartCode).Show();
				View.Property(p => p.SparePartName).Show();
				View.Property(p => p.Specification).Show();
				View.Property(p => p.SpartType).Show();
				View.Property(p => p.ControlMethod).Show();
				View.Property(p => p.IsReplacement).Readonly().Show();
				View.Property(p => p.BatchNo).ShowInList(width: 120).Show();
				View.Property(p => p.SeriaNo).ShowInList(width: 120).Show();
				View.Property(p => p.Qty).Show();
				View.Property(p => p.ReceiveQty).Show();
				View.Property(p => p.HandOverStatus).Show();
				View.Property(p => p.HandoverNo).ShowInList(width: 120).Show();
				View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
			}
		}

		///<summary>
		/// 配置扫描交接-接收明细视图
		/// </summary>
		protected void ConfigScanOutDepotHandoverDetailView()
		{
			View.DisableEditing();
			View.WithoutPaging();
			using (View.OrderProperties())
			{
				View.Property(p => p.SparePartCode).DisableSort().Show();
				View.Property(p => p.SparePartName).DisableSort().Show();
				View.Property(p => p.Specification).DisableSort().Show();
				View.Property(p => p.SpartType).DisableSort().Show();
				View.Property(p => p.ControlMethod).DisableSort().Show();
				View.Property(p => p.IsReplacement).Readonly().DisableSort().Show();
				View.Property(p => p.BatchNo).DisableSort().ShowInList(width: 120).Show();
				View.Property(p => p.SeriaNo).DisableSort().ShowInList(width: 120).Show();
				View.Property(p => p.Qty).DisableSort().Show();
				View.Property(p => p.ReceiveQty).DisableSort().Show();
				View.Property(p => p.HandOverStatus).DisableSort().Show();
				View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
			}
		}
	}
}