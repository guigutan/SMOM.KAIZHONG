using SIE.EMS.Purchases.FixtureAcceptances;
using System;

namespace SIE.Web.EMS.Purchases.FixtureAcceptances
{
	/// <summary>
	/// 序列号明细视图配置
	/// </summary>
	internal class FixtureAcceptanceSnViewConfig : WebViewConfig<FixtureAcceptanceSn>
	{
		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.UseCommand("SIE.Web.EMS.Purchases.FixtureAcceptances.Commands.OnekeyPassCommand");
			View.Property(p => p.PurOrderNo).ShowInList(100).Readonly();
			View.Property(p => p.OrderLineNo).ShowInList(100).Readonly();
			View.Property(p => p.ReceiveWh).ShowInList(80).Readonly();
			View.Property(p => p.Price).ShowInList(80).Readonly();
			View.Property(p => p.Sn).ShowInList(100).Readonly();
			View.Property(p => p.OriginalSn).ShowInList(100).Readonly();
			View.Property(p => p.ProductionDate).ShowInList(120).Readonly();
			View.Property(p => p.Maker).ShowInList(100).Readonly();
			View.Property(p => p.InspectionResult).ShowInList(80).HasLabel("验收状态".L10N()+"*");
			View.Property(p => p.Remark).ShowInList(200).Readonly();
		}

		/// <summary>
		/// 配置下拉列表
		/// </summary>

        protected override void ConfigSelectionView()
        {
			View.Property(p => p.PurOrderNo).ShowInList(100).Readonly();
			View.Property(p => p.OrderLineNo).ShowInList(100).Readonly();
			View.Property(p => p.ReceiveWh).ShowInList(80).Readonly();
			View.Property(p => p.Price).ShowInList(80).Readonly();
			View.Property(p => p.Sn).ShowInList(100).Readonly();
			View.Property(p => p.OriginalSn).ShowInList(100).Readonly();
			View.Property(p => p.ProductionDate).ShowInList(120).Readonly();
			View.Property(p => p.Maker).ShowInList(100).Readonly();
			View.Property(p => p.InspectionResult).ShowInList(80).HasLabel("验收状态");
			View.Property(p => p.Remark).ShowInList(200).Readonly();
		}
    }
}