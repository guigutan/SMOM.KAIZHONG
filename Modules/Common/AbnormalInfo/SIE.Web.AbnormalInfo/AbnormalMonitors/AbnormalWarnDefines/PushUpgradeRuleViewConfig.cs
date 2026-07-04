using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.MetaModel.View;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors
{
	/// <summary>
	/// 视图配置
	/// </summary>
	internal class PushUpgradeRuleViewConfig : WebViewConfig<PushUpgradeRule>
	{
		///<summary>
		/// 配置视图
		/// </summary>
		protected override void ConfigView()
		{
		}
		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.UseChildrenAsHorizontal();
			View.InlineEdit();
			View.ClearCommands();
			View.UseCommands(WebCommandNames.Add,WebCommandNames.Edit,WebCommandNames.Delete);
			View.Property(p => p.AbnormalNode).Readonly(c=>c.PersistenceStatus!=Domain.PersistenceStatus.New).UseEnumEditor("PushUpgradeRuleNode");
			View.Property(p => p.Time).UseSpinEditor(p => { p.MinValue = 0.1; p.AllowNegative = false; p.AllowDecimals = true; p.DecimalPrecision = 1; }).DefaultValue(1); 
			View.Property(p => p.UnitType);	
			View.Property(p => p.Pusher).HasLabel("推送方式");
			View.ChildrenProperty(p => p.TargetList).HasLabel("推送对象");
		
		}
	}
}