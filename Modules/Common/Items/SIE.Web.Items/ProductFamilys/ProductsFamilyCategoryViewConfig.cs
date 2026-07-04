using SIE.Domain;
using SIE.Items;

namespace SIE.Web.Items
{
    /// <summary>
    /// 产品族分类 视图配置
    /// </summary>
    internal class ProductFamilyCategoryViewConfig : WebViewConfig<ProductFamilyCategory>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ProductFamily));
            View.InlineEdit().UseDefaultCommands();
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Code).ShowInList(150).Readonly(p => p.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; });
            View.Property(p => p.Name).ShowInList(150);
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate).ShowInList(150);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate).ShowInList(150);
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate).ShowInList(150);
        }
    }
}
