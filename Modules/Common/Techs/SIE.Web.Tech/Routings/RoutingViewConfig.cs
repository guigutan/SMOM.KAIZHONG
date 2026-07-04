using SIE.Tech.Processs.Scripts;
using SIE.Tech.Routings;

namespace SIE.Web.Tech.Routings
{
    /// <summary>
    /// 工艺路线 视图配置
    /// </summary>
    internal class RoutingViewConfig : WebViewConfig<Routing>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.RequierModels(typeof(ScriptCondition));
            View.UseDefaultCommands();
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(Commands.SaveRoutingCommand).FullName);
            View.UseCommands(typeof(Commands.PublishCommand).FullName);
            View.UseCommands(typeof(Commands.EditRouting).FullName);
            View.UseCommands("SIE.Web.Tech.Routings.Commands.SelectItemCommand");
            View.UseCommands("SIE.Web.Tech.Routings.Commands.SelectCategoryCommand");
            View.Property(p => p.Name);
            View.Property(p => p.Description);
            View.Property(p => p.Category).HasLabel("产品族").Readonly();
        }

        /// <summary>
        /// 明细视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.Description);
            View.Property(p => p.Category).HasLabel("产品族").Readonly();
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.Description);
            View.Property(p => p.Category).HasLabel("产品族").Readonly();
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.Description);
            View.Property(p => p.Category).HasLabel("产品族").Readonly();
        }
    }
}
