using SIE.MetaModel.View;
using SIE.Packages.QrCodeParseRules;
using SIE.Web.Packages.QrCodeParseRules.Commands;

namespace SIE.Web.Packages.QrCodeParseRules
{
    /// <summary>
    /// 二维码解析规则明细视图配置
    /// </summary>
    internal class QrCodeParseRuleDetailViewConfig : WebViewConfig<QrCodeParseRuleDetail>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(AddQrCodeParseRuleDetailCommand).FullName, typeof(EditQrCodeParseRuleDetailCommand).FullName, WebCommandNames.Delete);
            View.AddBehavior("SIE.Web.Packages.QrCodeParseRules.QrCodeParseRuleDetailBehavior");
            View.WithoutPaging();
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).Readonly();
                View.Property(p => p.ParseField);
                View.Property(p => p.InterceptStart).Readonly(p => p.InterceptWay == InterceptWay.Separator).UseListSetting(e => { e.HelpInfo = "汉字按两位计算)"; });
                View.Property(p => p.InterceptEnd).Readonly(p => p.InterceptWay == InterceptWay.Separator).UseListSetting(e => { e.HelpInfo = "汉字按两位计算)"; });
                View.Property(p => p.TestResult).Readonly().ShowInList(200);
            }
        }
    }
}