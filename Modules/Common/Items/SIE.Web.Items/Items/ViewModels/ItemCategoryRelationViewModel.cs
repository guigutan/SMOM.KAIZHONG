using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Items.Items;
using SIE.MetaModel;
using SIE.ObjectModel;

namespace SIE.Web.Items.Items.ViewModels
{
    /// <summary>
    /// 物料与分类关系 ViewModel
    /// </summary>
    [RootEntity]
    [Label("物料与分类关系")]
    [DisplayMember(nameof(Item))]
    public class ItemCategoryRelationViewModel : ViewModel
    {
        #region 分类类型 Type
        /// <summary>
        /// 分类类型
        /// </summary>
        [Label("分类类型")]
        public static readonly Property<CategoryType> TypeProperty = P<ItemCategoryRelationViewModel>.Register(e => e.Type);

        /// <summary>
        /// 分类类型
        /// </summary>
        public CategoryType Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 分类 ItemCategory
        /// <summary>
        /// 分类Id
        /// </summary>
        [Label("分类")]
        public static readonly IRefIdProperty ItemCategoryIdProperty = P<ItemCategoryRelationViewModel>.RegisterRefId(e => e.ItemCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 物料小类Id
        /// </summary>
        public double ItemCategoryId
        {
            get { return (double)GetRefId(ItemCategoryIdProperty); }
            set { SetRefId(ItemCategoryIdProperty, value); }
        }

        /// <summary>
        /// 分类
        /// </summary>
        public static readonly RefEntityProperty<ItemCategory> ItemCategoryProperty = P<ItemCategoryRelationViewModel>.RegisterRef(e => e.ItemCategory, ItemCategoryIdProperty);

        /// <summary>
        /// 分类
        /// </summary>
        public ItemCategory ItemCategory
        {
            get { return GetRefEntity(ItemCategoryProperty); }
            set { SetRefEntity(ItemCategoryProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 物料与分类关系 配置
    /// </summary>
    internal class ReprintViewModelConfig : EntityConfig<ItemCategoryRelationViewModel>
    {
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="rules">规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.Add(ItemCategoryRelationViewModel.ItemCategoryIdProperty, new RequiredRule());
            rules.Add(ItemCategoryRelationViewModel.TypeProperty, new RequiredRule());
        }
    }

    /// <summary>
    /// 物料与分类关系表 配置视图
    /// </summary>
    public class ItemCategoryRelationViewConfig : WebViewConfig<ItemCategoryRelationViewModel>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(Item));
            View.InlineEdit();
            View.Property(p => p.Type).Readonly(false);
            View.Property(p => p.ItemCategory);
        }
    }
}
