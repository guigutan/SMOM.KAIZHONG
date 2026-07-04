using SIE.MetaModel.View;
using SIE.Packages.QrCodeParseRules;
using SIE.Web.Common.Commands;
using SIE.Web.Packages.QrCodeParseRules.Commands;

namespace SIE.Web.Packages.QrCodeParseRules
{
    /// <summary>
    /// 二维码解析规则视图配置
    /// </summary>
    internal class QrCodeParseRuleViewConfig : WebViewConfig<QrCodeParseRule>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.Add, typeof(EditQrCodeParseRuleCommand).FullName, typeof(DeleteQrCodeParseRuleCommand).FullName, typeof(SaveQrCodeParseRuleCommand).FullName, 
                typeof(EnableQrCodeParseRuleCommand).FullName, DisableCommand.CommandName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Readonly(p => p.CreateBy > 0);
                View.Property(p => p.Name);
                View.Property(p => p.State).DefaultValue(0).Readonly();
                View.Property(p => p.InterceptWay).Readonly(p => p.CreateBy > 0);
                View.Property(p => p.SeparatorType).Readonly(p => p.InterceptWay == InterceptWay.InterceptDigit);
                View.Property(p => p.Desc);
                View.ChildrenProperty(p => p.QrCodeParseRuleDetailList);
            }
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
                View.Property(p => p.State).UseEnumEditor(p => p.AllowBlank = true);
                View.Property(p => p.InterceptWay).UseEnumEditor(p => p.AllowBlank = true);
            }
        }
    }
}