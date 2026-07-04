using SIE.Domain;
using SIE.Items;

namespace SIE.Wpf.Items.Items
{
    /// <summary>
    /// 物料与分类关系表 配置视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class ItemCategoryRelationViewConfig : WPFViewConfig<ItemCategoryRelation>
    {
        #region 获取质量中类的编码
        /// <summary>
        /// 获取质量中类的编码
        /// </summary>
        public static readonly Property<string> MediumCateogryCodeProperty = P<ItemCategoryRelation>.RegisterExtensionReadOnly("MediumCateogryCode", typeof(ItemCategoryRelationViewConfig),
            GetMediumCateogryCode, ItemCategoryRelation.ItemCategoryProperty);

        /// <summary>
        /// 获取质量中类的编码
        /// </summary>
        /// <param name="me">QualityCategoryItem</param>
        /// <returns>string</returns>
        public static string GetMediumCateogryCode(ItemCategoryRelation me)
        {
            var parent = RF.GetById<QualityCategory>(me.ItemCategory?.TreePId);
            var code = (parent as ItemCategory)?.Code;
            return code;
        }
        #endregion

        #region 获取质量中类的名称
        /// <summary>
        /// 获取质量中类的名称
        /// </summary>
        public static readonly Property<string> MediumCategoryNameProperty = P<ItemCategoryRelation>.RegisterExtensionReadOnly("MediumCategoryName", typeof(ItemCategoryRelationViewConfig),
            GetMediumCategoryName, ItemCategoryRelation.ItemCategoryProperty);

        /// <summary>
        /// 获取质量中类的名称
        /// </summary>
        /// <param name="me">QualityCategoryItem</param>
        /// <returns>string</returns>
        public static string GetMediumCategoryName(ItemCategoryRelation me)
        {
            var parent = RF.GetById<QualityCategory>(me.ItemCategory?.TreePId);
            var name = (parent as ItemCategory)?.Name;
            return name;
        }
        #endregion

        #region 获取质量大类的编码
        /// <summary>
        /// 获取质量大类的编码
        /// </summary>
        public static readonly Property<string> LargeCateogryCodeProperty = P<ItemCategoryRelation>.RegisterExtensionReadOnly("LargeCateogryCode", typeof(ItemCategoryRelationViewConfig),
            GetLargeCateogryCode, ItemCategoryRelation.ItemCategoryProperty);

        /// <summary>
        /// 获取质量大类的编码
        /// </summary>
        /// <param name="me">QualityCategoryItem</param>
        /// <returns>string</returns>
        public static string GetLargeCateogryCode(ItemCategoryRelation me)
        {
            var mediumParent = RF.GetById<QualityCategory>(me.ItemCategory?.TreePId);
            var largeParent = RF.GetById<QualityCategory>(mediumParent?.TreePId);
            var largeCode = (largeParent as ItemCategory)?.Code;
            return largeCode;
        }
        #endregion

        #region 获取质量大类的名称
        /// <summary>
        /// 获取质量大类的名称
        /// </summary>
        public static readonly Property<string> LargeCategoryNameProperty = P<ItemCategoryRelation>.RegisterExtensionReadOnly("LargeCategoryName", typeof(ItemCategoryRelationViewConfig),
            GetLargeCategoryName, ItemCategoryRelation.ItemCategoryProperty);

        /// <summary>
        /// 获取质量大类的名称
        /// </summary>
        /// <param name="me">QualityCategoryItem</param>
        /// <returns>string</returns>
        public static string GetLargeCategoryName(ItemCategoryRelation me)
        {
            var mediumParent = RF.GetById<QualityCategory>(me.ItemCategory?.TreePId);
            var largeParent = RF.GetById<QualityCategory>(mediumParent?.TreePId);
            var largeName = (largeParent as ItemCategory)?.Name;
            return largeName;
        }
        #endregion

        ///// <summary>
        ///// 物料分类视图
        ///// </summary>
        //public const string ItemCategoryView = "ItemCategoryView";

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("质量分类").HasDelegate(ItemCategoryRelation.IdProperty);
            View.AssignAuthorize(typeof(Item));
            //View.FormEdit(); 
            //using (View.OrderProperties())
            //{
            //    View.Property(p => p.ItemCategory.Code).HasLabel("小类编码").Show(ShowInWhere.All);
            //    View.Property(p => p.ItemCategory.Name).HasLabel("小类名称").Show(ShowInWhere.All);
            //    View.Property(MediumCateogryCodeProperty).HasLabel("中类编码").Show(ShowInWhere.List);
            //    View.Property(MediumCategoryNameProperty).HasLabel("中类名称").Show(ShowInWhere.List);
            //    View.Property(LargeCateogryCodeProperty).HasLabel("大类编码").Show(ShowInWhere.List);
            //    View.Property(LargeCategoryNameProperty).HasLabel("大类名称").Show(ShowInWhere.List);
            //}
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultBehaviors();
            View.InlineEdit();
            View.UseCommands(WPFCommandNames.ListEdit);
            View.Property(p => p.Type).UseEnumEditor().Readonly();
            View.Property(p => p.ItemCategory).UseItemCategoryEditor(p => { p.ReloadDataOnPopping = true; p.DisplayMember = ItemCategory.CodeProperty.Name; }).HasLabel("分类编码");
            View.Property(p => p.ItemCategory.Name).HasLabel("分类名称");
        }
    }
}