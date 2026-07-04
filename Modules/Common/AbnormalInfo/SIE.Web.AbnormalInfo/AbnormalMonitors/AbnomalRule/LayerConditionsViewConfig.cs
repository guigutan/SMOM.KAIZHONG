using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.AbnormalInfo.Common;
using SIE.Web.AbnormalInfo.AbnormalMonitors._Extentions_;
using SIE.Web.AbnormalInfo.AbnormalMonitors.AbnomalRule.Commands;

namespace SIE.Web.AbnormalInfo.AnomalyMonitors
{
	/// <summary>
	/// 异常规则层别条件视图配置
	/// </summary>
	internal class LayerConditionsViewConfig : WebViewConfig<LayerConditions>
	{
		///<summary>
		/// 配置视图
		/// </summary>
		protected override void ConfigView()
		{
			View.UseDefaultCommands();
            View.AssignAuthorize(typeof(AbnormalDecisionRule));
        }
		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.AddBehavior("SIE.Web.AbnormalInfo.AnomalyMonitors.AbnomalRule.Behaviors.AbnomalRuleDetailBehavior");
			View.WithoutPaging();
			View.ClearCommands();
			View.UseCommands(typeof(AbnomalRuleDeleteCommand).FullName,typeof(SaveAbnormalRuleCommand).FullName);
			View.UseLayoutSize(0.4,0.6);
			View.Property(p => p.LayerName).Readonly();
			View.Property(p => p.PropDisTabName).HasLabel("数据表").Readonly();			
			View.Property(p => p.IsWhere);
			View.Property(p => p.IsGroup);
			View.Property(p => p.IsCacul);
			View.Property(p => p.FieldProp).Readonly(p => p.FieldProp != FieldProp.DateTime && p.FieldProp != FieldProp.EnumTime);
			View.Property(p => p.Value1).UseDynamicTypeEditor();
			View.Property(p => p.Value2).UseDynamicTypeEditor().Readonly(p=>p.FieldProp!= FieldProp.DateTime);
			View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
			View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
			View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
			View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
		}
		
	}
}