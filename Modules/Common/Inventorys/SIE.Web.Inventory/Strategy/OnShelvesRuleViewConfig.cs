using SIE.Domain;
using SIE.Inventory.Strategy;
using SIE.MetaModel.View;
using SIE.Web.Inventory.Strategy.Commands;

namespace SIE.Web.Inventory.Strategy
{
    /// <summary>
    /// 上架规则视图配置
    /// </summary>
    internal class OnShelvesRuleViewConfig : WebViewConfig<OnShelvesRule>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.Add, typeof(AddOnShelvesRuleCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Delete, typeof(DeleteOnShelvesRuleCommand).FullName);
            View.RemoveCommands(WebCommandNames.Copy);
            View.UseCommands(typeof(SetIsDefaultOnShelvesRuleCommand).FullName);
            View.Property(p => p.Code).Readonly(p => p.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = string.Format("根据{0}(配置项--{0})生成{1}编码", "单号生成规则", "上架规则") + ",新增状态可编辑"; }).ShowInList(150);
            View.Property(p => p.Name);
            View.Property(p => p.Description);
            View.Property(p => p.Warehouse).Readonly(p => p.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; }).HasLabel("仓库");
            View.Property(p => p.State).Readonly();
            View.Property(p => p.IsDefault).Readonly();
            View.ChildrenProperty(p => p.DetailList).HasLabel("明细");
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
                View.Property(p => p.Description);
                View.Property(p => p.State);
            }
        }
    }
}
