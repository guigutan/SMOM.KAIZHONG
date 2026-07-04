using SIE.EMS.Purchases.FixtureAcceptances;
using SIE.Fixtures;

namespace SIE.Web.EMS.Purchases.FixtureAcceptances
{
	/// <summary>
	/// 工治具验收明细视图配置
	/// </summary>
	internal class FixtureAcceptanceDetailViewConfig : WebViewConfig<FixtureAcceptanceDetail>
	{
		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.AddBehavior("SIE.Web.EMS.Purchases.FixtureAcceptances.FixtureAcceptanceDetBehavior");
			View.Property(p => p.PurOrderNo).ShowInList(100).Readonly();
			View.Property(p => p.OrderLineNo).ShowInList(100).Readonly();
			View.Property(p => p.ReceiveWh).ShowInList(80).Readonly();
			View.Property(p => p.Price).ShowInList(80).Readonly();
			View.Property(p => p.ReceiveQty).ShowInList(80).Readonly(); 
			View.Property(p => p.PassQty).Readonly(p=>p.ManageMode== ManageMode.Number).ShowInList(80);
			View.Property(p => p.UnqualifiedQty).Readonly(p => p.ManageMode == ManageMode.Number).ShowInList(80);
			View.Property(p => p.Remark).ShowInList(200).Readonly(); 
			View.ChildrenProperty(p => p.FixtureAcceptanceSnList).HasLabel("序列号明细").Show( ChildShowInWhere.Hide);
		}
	}
}