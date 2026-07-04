using SIE.Andon.Andons;
using SIE.MetaModel.View;
using SIE.Web.Andon.Andons.Commands;
using SIE.Web.Common.Commands;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SIE.Web.Andon.Andons
{
    /// <summary>
    /// 安灯类型维护视图配置
    /// </summary>
    public class AndonTypeViewConfig : WebViewConfig<AndonType>
    {
        /// <summary>
        /// 配置视图
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
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, typeof(AndonTypeSaveCommand).FullName);
            View.ReplaceCommands(EnableCommand.CommandName,typeof(AndonTypeEnableCommand).FullName);
            View.ReplaceCommands(DisableCommand.CommandName,typeof(AndonTypeDisableCommand).FullName);
            View.UseImportCommands();
            View.Property(p => p.AndonTypeCode);
            View.Property(p => p.AndonTypeName);
            View.Property(p => p.AndonTypeClass);
            View.Property(p => p.State).Readonly();
            //View.Property(p => p.PushPlug).HasLabel("推送模块");
            //View.Property(p => p.MessageTemplate).HasLabel("消息模板").UseTextButtonFieldEditor(p => { p.ExtendJsObj = "SIE.Web.Andon.Andons.Scripts.AndonTypeMessageTemplateEditor"; p.Editable = false; });
            View.ChildrenProperty(p => p.TriggerPowerList).HasLabel("触发权限").HasOrderNo(0);
            View.ChildrenProperty(p => p.MessageSendList).HasLabel("消息推送&升级机制").HasOrderNo(1);
        }

        /// <summary>
        /// 下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.AndonTypeCode);
            View.Property(p => p.AndonTypeName);
        }

        /// <summary>
        /// 配置导入
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.AndonTypeCode).ImportIndexer();
            View.Property(p => p.AndonTypeName);
            View.Property(p => p.AndonTypeClass).UseEnumEditor();
            View.Property(p => p.State);
        }

    }
}
