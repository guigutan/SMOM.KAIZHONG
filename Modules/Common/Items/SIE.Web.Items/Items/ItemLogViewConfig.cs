using SIE.Items;

namespace SIE.Web.Items.Items
{
    /// <summary>
    /// 物料更新日记 配置视图
    /// </summary>
    internal class ItemLogViewConfig : WebViewConfig<ItemLog>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.OperatDate).Show(ShowInWhere.All);
                View.Property(p => p.OperatType).Show(ShowInWhere.All);
                View.Property(p => p.OperatDescription).Show(ShowInWhere.All);
            }
        }
    }
}
