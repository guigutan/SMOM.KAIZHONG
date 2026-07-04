using SIE.Andon.Andons;
using SIE.Andon.Andons.Enum;
using SIE.MetaModel.View;

namespace SIE.Web.Andon.Andons
{
    /// <summary>
    /// 安灯类型维护推送对象视图配置
    /// </summary>
    public class AndonTypePushObjectViewConfig : WebViewConfig<SIE.Andon.Andons.AndonTypePushObject>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(SIE.Andon.Andons.AndonTypeMessageSend));
            View.AddBehavior("SIE.Web.Andon.Andons.Behaviors.AndonTypePushObjectBehavior");
            View.UseCommands("SIE.Web.Andon.Andons.Commands.AndonTypePushObjectAddCommand", WebCommandNames.Edit, WebCommandNames.Delete);
            View.Property(p => p.Type);
            View.Property(p => p.Code).UseTextButtonFieldEditor(p => { p.ExtendJsObj = "SIE.Web.Andon.Andons.Scripts.AndonTypePushObjectEditor"; p.Editable = false; })
                .Readonly(p => p.Type == SIE.Andon.Andons.Enum.PushObjectType.Trigger || p.Type == SIE.Andon.Andons.Enum.PushObjectType.Handler || p.Type == SIE.Andon.Andons.Enum.PushObjectType.WorkGroupCharge || p.Type == PushObjectType.AndonCharger);
            View.Property(p => p.Name).Readonly();
        }
    }
}
