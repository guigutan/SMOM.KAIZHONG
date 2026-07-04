using SIE.AbnormalInfo.AbnormalInfos;
using SIE.MetaModel.View;
using SIE.Web.AbnormalInfo.AbnormalInfos.Commands;

namespace SIE.Web.AbnormalInfo.AbnormalInfos
{
    /// <summary>
    /// 异常定义配置关联处理人  视图配置
    /// </summary>
    class AbnormalInfoDefinitionEmployeeViewConfig : WebViewConfig<AbnormalInfoDefinitionEmployee>
    {
        /// <summary>
        /// 主视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
            View.HasDelegate(AbnormalInfoDefinition.HandlerListProperty);
            View.UseCommands(typeof(AbnormalInfoDefHandlerSelectCommand).FullName, WebCommandNames.Delete,
                WebCommandNames.ExportXls);
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }
}