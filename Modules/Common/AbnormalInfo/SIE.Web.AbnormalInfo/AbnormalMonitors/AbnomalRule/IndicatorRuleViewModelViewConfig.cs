using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.AbnormalInfo.Common;

namespace SIE.Web.AbnormalInfo.AnomalyMonitors
{
	/// <summary>
	/// 规则计算指标条件视图配置
	/// </summary>
	internal class IndicatorRuleViewModelViewConfig : WebViewConfig<IndicatorRuleViewModel>
	{
		protected override void ConfigDetailsView()
		{
			View.HasDetailColumnsCount(2);
			View.Property(p => p.IndicatorName).ShowInDetail(columnSpan: 2).Show(ShowInWhere.All);
			View.Property(p => p.IndicatorOperation).UseMemoEditor().ShowInDetail(columnSpan: 2).Show(ShowInWhere.All);
			View.Property(p => p.Operator).HasLabel("运算符").ShowInDetail(columnSpan: 2).Show(ShowInWhere.All);
			View.Property(p => p.Value1).Show(ShowInWhere.All);
			View.Property(p => p.Value2).Show(ShowInWhere.All).Readonly(p=>p.Operator!= Operator.between);
			View.Property(p => p.IndicatorUnit).DefaultValue(IndicatorUnit.Number).ShowInDetail(columnSpan: 2).Show(ShowInWhere.All);
		}

	}
}