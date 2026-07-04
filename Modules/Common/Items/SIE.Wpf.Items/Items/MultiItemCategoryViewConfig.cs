using SIE.Domain;
using SIE.Items.Items;
using SIE.ManagedProperty;

namespace SIE.Wpf.Items.Items
{
    /// <summary>
    /// 物料分类视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    public class MultiItemCategoryViewConfig : WPFViewConfig<MultiItemCategory>
    {
        #region 父级节点描述 Parents
        /// <summary>
        /// 父级节点描述
        /// </summary>
        public static readonly Property<string> ParentsProperty = P<MultiItemCategory>.RegisterExtensionReadOnly("Parents", typeof(ItemSmallCategoryViewConfig),
            GetParents, MultiItemCategory.TreePIdProperty);

        /// <summary>
        /// 父级节点描述
        /// </summary>
        /// <param name="me">ItemSmallCategory</param>
        /// <returns>string</returns>
        public static string GetParents(MultiItemCategory me)
        {
            string parents = string.Empty;
            if (me.TreePId == null)
                return parents;
            var category = RF.Find<MultiItemCategory>().GetById(me.TreePId) as MultiItemCategory;
            parents = FindParent(category, string.Empty);
            return parents.TrimEnd('>').TrimEnd('-');
        }

        /// <summary>
        /// FindParent
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="parents">parents</param>
        /// <returns>string</returns>
        static string FindParent(MultiItemCategory category, string parents)
        {
            if (category == null)
                return parents;
            parents = category.Name + "->" + parents;
            return FindParent(RF.Find<MultiItemCategory>().GetById(category.TreePId) as MultiItemCategory, parents);
        }
        #endregion

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(ParentsProperty).HasLabel("父级关系");
        }
    }
}
