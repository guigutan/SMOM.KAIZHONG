using SIE.Andon.Andons;
using SIE.MetaModel.View;
using SIE.Web.Andon.Andons.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons
{
    /// <summary>
    /// 安灯维护消息推送视图配置
    /// </summary>
    public class AndonMessageSendViewConfig : WebViewConfig<AndonMessageSend>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseChildrenAsHorizontal();
            View.UseCommands("SIE.Web.Andon.Andons.Commands.CopyAndonTypeMessageSendCommand",typeof(AndonMessageAddCommand).FullName,WebCommandNames.Edit,WebCommandNames.Delete);
            View.Property(p => p.Node);
            View.Property(p => p.Minute).UseSpinEditor(p => { p.DecimalPrecision = 1; p.MinValue = 0; });
            View.Property(p => p.PushPlug);
            View.Property(p => p.MessageTemplate).HasLabel("消息模板").UseTextButtonFieldEditor(p => { p.ExtendJsObj = "SIE.Web.Andon.Andons.Scripts.AndonTypeMessageTemplateEditor"; p.Editable = false; }).UseListSetting(p=>p.HelpInfo="超过4000字符将自动截断".L10N());
            View.ChildrenProperty(p => p.PushObjectList).HasLabel("推送对象").LazyLoad(false);
        }
    }
}
