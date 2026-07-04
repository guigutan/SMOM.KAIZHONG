using SIE.Domain;
using SIE.Items;
using SIE.ManagedProperty;

namespace SIE.Wpf.Items.Items
{
    /// <summary>
    /// 质量分类视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    public class QualityCategoryViewConfig : WPFViewConfig<QualityCategory>
    {
        #region 父级节点描述 Parents
        /// <summary>
        /// 父级节点描述
        /// </summary>
        public static readonly Property<string> ParentsProperty = P<QualityCategory>.RegisterExtensionReadOnly("Parents", typeof(QualityCategoryViewConfig),
            GetParents, QualityCategory.TreePIdProperty);

        /// <summary>
        /// 父级节点描述
        /// </summary>
        /// <param name="me">QualityCategory</param>
        /// <returns>string</returns>
        public static string GetParents(QualityCategory me)
        {
            string parents = string.Empty;
            if (me == null || me.TreePId == null || me.TreePId <= 0)
                return parents;
            var category = RF.GetById<ItemCategory>(me.TreePId);
            parents = FindParent(category, string.Empty);
            return parents.TrimEnd('>').TrimEnd('-');
        }

        /// <summary>
        /// FindParent
        /// </summary>
        /// <param name="category">category</param>
        /// <param name="parents">parents</param>
        /// <returns>string</returns>
        static string FindParent(ItemCategory category, string parents)
        {
            if (category == null)
                return parents;
            parents = category.Name + "->" + parents;
            var itemCategory = RF.GetById<ItemCategory>(category.TreePId);
            return FindParent(itemCategory, parents);
        }
        #endregion

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();
            View.DisableEditing();
            View.Property(p => p.Code).Show(ShowInWhere.All);
            View.Property(p => p.Name).Show(ShowInWhere.All);
            View.Property(p => p.Level).Show(ShowInWhere.Hide);
            View.Property(p => p.Type).Show(ShowInWhere.Hide);
            View.Property(QualityCategoryViewConfig.ParentsProperty).HasLabel("父级关系").Show(ShowInWhere.All);
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).Show(ShowInWhere.All);
            View.Property(p => p.Name).Show(ShowInWhere.All);
            View.Property(QualityCategoryViewConfig.ParentsProperty).HasLabel("父级关系").Show(ShowInWhere.All);
        }
    }
}