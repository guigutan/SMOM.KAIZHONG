using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.AbnormalInfo.Common;
using SIE.MetaModel.View;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors
{
	/// <summary>
	/// 推送对象视图配置
	/// </summary>
	internal class PushTargetViewConfig : WebViewConfig<PushTarget>
	{

		/// <summary>
		/// 列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.InlineEdit();
			View.ClearCommands();
			View.AssignAuthorize(typeof(SIE.AbnormalInfo.AbnormalMonitors.PushUpgradeRule));
			View.AddBehavior("SIE.AbnormalInfo.AbnormalMonitors.Behaviors.PushTargetBehavior");
			View.UseCommands("SIE.AbnormalInfo.AbnormalMonitors.Commands.PushTargetAddCommand", WebCommandNames.Edit, WebCommandNames.Delete);
			View.Property(p => p.TargetType);
			View.Property(p => p.TargetCode).UseTextButtonFieldEditor(p => { p.ExtendJsObj = "SIE.AbnormalInfo.AbnormalMonitors.Editors.PushTargetEditor"; p.Editable = false; })
				.Readonly(p => p.TargetType == PushTargetEnum.Custom);
			View.Property(p => p.TargetName).Readonly();
		}
	}
}