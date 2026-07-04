using SIE.MetaModel.View;
using SIE.Tech.VictoryStandards;

namespace SIE.Web.Tech.VictoryStandards
{
    /// <summary>
    /// 胜制方案-界面
    /// </summary>
    internal class VictoryStandardViewConfig : WebViewConfig<VictoryStandard>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands().RemoveCommands(WebCommandNames.Copy);
            View.ReplaceCommands(WebCommandNames.Add, "SIE.Web.Tech.VictoryStandards.Commands.VictoryStandardAddCommand");
            View.ReplaceCommands(WebCommandNames.Delete, "SIE.Web.Tech.VictoryStandards.Commands.VictoryStandardDeleteCommand");
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.MaxTestQty).Readonly();
            View.Property(p => p.State).Readonly();
            View.Property(p => p.Remark);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        /// <summary>
        ///配置下拉列表
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.MaxTestQty);
        }
    }
}
