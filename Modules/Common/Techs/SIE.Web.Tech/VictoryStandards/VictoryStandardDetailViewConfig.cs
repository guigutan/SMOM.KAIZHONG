using SIE.MetaModel.View;
using SIE.Tech.VictoryStandards;

namespace SIE.Web.Tech.VictoryStandards
{
    /// <summary>
    /// 胜局标准-界面
    /// </summary>
    internal class VictoryStandardDetailViewConfig : WebViewConfig<VictoryStandardDetail>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);
            View.Property(p => p.Standard).UseListSetting(e => { e.HelpInfo = "必须为【0、1】组成的字符串,最后一位必须为：1，1代表合格"; });
            View.Property(p => p.Remark);
        }
    }
}
