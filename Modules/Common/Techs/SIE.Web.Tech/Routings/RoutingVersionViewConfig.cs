using SIE.ManagedProperty;
using SIE.Tech.Routings;

namespace SIE.Web.Tech.Routings
{
    /// <summary>
    /// 工艺路线版本视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    internal class RoutingVersionViewConfig : WebViewConfig<RoutingVersion>
    {
        /// <summary>
        /// 列表配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Routing).Show(ShowInWhere.DropDown);
                View.Property(p => p.Name).Show(ShowInWhere.DropDown);
                View.Property(p => p.EffectiveDate).ShowInList();
                View.Property(p => p.ReferenceQty).ShowInList();
                View.Property(p => p.State).Show(ShowInWhere.DropDown);
                View.Property(p => p.IsDefault).Show(ShowInWhere.DropDown);
            }
        }

        /// <summary>
        /// 表单配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Routing).Show(ShowInWhere.DropDown);
                View.Property(p => p.Name).Show(ShowInWhere.DropDown);
                View.Property(p => p.EffectiveDate).ShowInList();
                View.Property(p => p.ReferenceQty).ShowInList();
                View.Property(p => p.State).Show(ShowInWhere.DropDown);
                View.Property(p => p.IsDefault).Show(ShowInWhere.DropDown);
            }
        }

        /// <summary>
        /// 下拉配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.RoutingName).HasLabel("工艺路线");
            View.Property(p => p.Name);
            View.Property(p => p.EffectiveDate);
            View.Property(p => p.ReferenceQty);
            View.Property(p => p.State);
            View.Property(p => p.IsDefault);
        }
    }
}