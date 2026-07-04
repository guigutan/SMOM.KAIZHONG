using SIE.EMS.Purchases.FixtureReceives;
using SIE.Web.EMS.Purchases.FixtureReceives.Commands;

namespace SIE.Web.EMS.Purchases.FixtureReceives
{
	/// <summary>
	/// 工治具接收序列号明细视图配置
	/// </summary>
	public class FixtureReceiveSnViewConfig : WebViewConfig<FixtureReceiveSn>
	{
		/// <summary>
		/// 扫描视图
		/// </summary>
		public static readonly string ScanView = "ScanView";

		/// <summary>
		/// 通用视图配置
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup(ScanView);
			if (ViewGroup == ScanView)
			{
				ConfigScanView();
			}
		}


		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			DefaultListView();
		}

		/// <summary>
		/// 配置扫描视图
		/// </summary>
		protected void ConfigScanView()
		{
			DefaultListView();
			View.ClearCommands();
			View.UseCommand(typeof(DeleteReceiveSnCommand).FullName);
		}


		/// <summary>
		/// 
		/// </summary>
		private void DefaultListView()
		{
			View.DisableEditing();
			View.WithoutPaging();
			View.UseGridSelectionModel();
			View.UseCommands("SIE.Web.EMS.Purchases.FixtureReceives.Commands.ScreenCommand");
			View.UseCommands("SIE.Web.EMS.Purchases.FixtureReceives.Commands.SnPrintCommand");
			using (View.OrderProperties())
			{
				View.Property(p => p.LineNo).ShowInList(60).Readonly();
				View.Property(p => p.PurOrderNo).ShowInList(100).Readonly();
				View.Property(p => p.OrderLineNo).ShowInList(100).Readonly();
				View.Property(p => p.SupplierCode).ShowInList(120).Readonly();
				View.Property(p => p.SupplierName).ShowInList(120).Readonly();
				View.Property(p => p.CustomerCode).ShowInList(100).Readonly();
				View.Property(p => p.CustomerName).ShowInList(100).Readonly();
				View.Property(p => p.FixtureEncodeCode).ShowInList(120).Readonly();
				View.Property(p => p.ModelCode).ShowInList(80).Readonly();
				View.Property(p => p.ModelName).ShowInList(80).Readonly();
				View.Property(p => p.ManageMode).ShowInList(80).Readonly();
				View.Property(p => p.Sn).ShowInList(120).HasLabel("序列号编码");
				View.Property(p => p.OriginalSn).ShowInList(100);
				View.Property(p => p.ProductionDate).ShowInList(100);
				View.Property(p => p.Maker).ShowInList(80);

				View.Property(p => p.CreateBy).Show( ShowInWhere.Hide);
				View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);

			}
		}
	}
}