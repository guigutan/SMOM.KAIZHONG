using SIE.Andon.Andons;
using SIE.Andon.Andons.Enum;
using SIE.MetaModel.View;
using SIE.Web.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons
{
    /// <summary>
    /// 安灯维护推送对象视图配置
    /// </summary>
    public class AndonPushObjectViewConfig : WebViewConfig<AndonPushObject>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.Andon.Andons.Behaviors.AndonPushObjectBehavior");
            View.UseCommands("SIE.Web.Andon.Andons.Commands.AndonPushObjectAddCommand", WebCommandNames.Edit, WebCommandNames.Delete);
            View.Property(p => p.AndonLevel)
              .UseCatalogEditor(p =>
              {
                  p.CatalogType = AndonSesp.LevelCatalogType; p.CatalogReloadData = true;
              }).UseListSetting(p => p.HelpInfo = "“来源快码ANDONSESP_LEVEL");
            //View.Property(p => p.Type);
            //View.Property(p => p.Code).UseTextButtonFieldEditor(p => { p.ExtendJsObj = "SIE.Web.Andon.Andons.Scripts.AndonTypePushObjectEditor"; p.Editable = false; })
            //    .Readonly(p => p.Type == SIE.Andon.Andons.Enum.PushObjectType.Trigger || p.Type == SIE.Andon.Andons.Enum.PushObjectType.Handler || p.Type == SIE.Andon.Andons.Enum.PushObjectType.WorkGroupCharge || p.Type == PushObjectType.AndonCharger);
            //View.Property(p => p.Name).Readonly();
        }
    }
}
