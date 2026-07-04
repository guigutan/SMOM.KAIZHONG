using SIE.Common.Catalogs;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Web.Common;
using System;
using System.Linq.Expressions;

namespace SIE.Web.Items.Items.ViewModels
{
    /// <summary>
    /// 物料属性值 ViewModel
    /// </summary>
    [RootEntity]
    [Label("物料属性值")]
    [DisplayMember(nameof(Item))]
    public class ItemPropertyValueViewModel : ViewModel
    {
        #region 物料属性定义 Definition
        /// <summary>
        /// 物料属性定义Id
        /// </summary>
        [Label("物料属性")]
        public static readonly IRefIdProperty DefinitionIdProperty = P<ItemPropertyValueViewModel>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

        /// <summary>
        /// 物料属性定义Id
        /// </summary>
        public double DefinitionId
        {
            get { return (double)GetRefId(DefinitionIdProperty); }
            set { SetRefId(DefinitionIdProperty, value); }
        }

        /// <summary>
        /// 物料属性定义
        /// </summary>
        [Label("物料属性定义")]
        public static readonly RefEntityProperty<ItemPropertyDefinition> DefinitionProperty = P<ItemPropertyValueViewModel>.RegisterRef(e => e.Definition, DefinitionIdProperty);

        /// <summary>
        /// 物料属性定义
        /// </summary>
        public ItemPropertyDefinition Definition
        {
            get { return GetRefEntity(DefinitionProperty); }
            set { SetRefEntity(DefinitionProperty, value); }
        }
        #endregion

        #region 快码值 CatalogValue
        /// <summary>
        /// 快码值
        /// </summary>
        [MaxLength(40)]
        [Label("快码值")]
        public static readonly Property<string> CatalogValueProperty = P<ItemPropertyValueViewModel>.Register(e => e.CatalogValue);

        /// <summary>
        /// 快码值
        /// </summary>
        public string CatalogValue
        {
            get { return GetProperty(CatalogValueProperty); }
            set { SetProperty(CatalogValueProperty, value); }
        }
        #endregion        

        #region 文本值 TextValue
        /// <summary>
        /// 文本值
        /// </summary>
        [Label("文本值")]
        public static readonly Property<string> TextValueProperty = P<ItemPropertyValueViewModel>.Register(e => e.TextValue);

        /// <summary>
        /// 文本值
        /// </summary>
        public string TextValue
        {
            get { return this.GetProperty(TextValueProperty); }
            set { this.SetProperty(TextValueProperty, value); }
        }
        #endregion

        #region 数值 NumberValue
        /// <summary>
        /// 数值
        /// </summary>
        [Label("数值")]
        public static readonly Property<decimal> NumberValueProperty = P<ItemPropertyValueViewModel>.Register(e => e.NumberValue);

        /// <summary>
        /// 数值
        /// </summary>
        public decimal NumberValue
        {
            get { return this.GetProperty(NumberValueProperty); }
            set { this.SetProperty(NumberValueProperty, value); }
        }
        #endregion

        #region 属性类型 PropertyType
        /// <summary>
        /// 属性类型
        /// </summary>
        [Label("属性类型")]
        public static readonly Property<ItemPropertyType> PropertyTypeProperty = P<ItemPropertyValueViewModel>.Register(e => e.PropertyType);

        /// <summary>
        /// 属性类型
        /// </summary>
        public ItemPropertyType PropertyType
        {
            set { this.SetProperty(PropertyTypeProperty, value); }
            get { return this.GetProperty(PropertyTypeProperty); }
        }
        #endregion

        #region 快码类型 CatalogType
        /// <summary>
        /// 快码类型
        /// </summary>
        [Label("快码类型")]
        public static readonly Property<string> CatalogTypeProperty = P<ItemPropertyValueViewModel>.Register(e => e.CatalogType);

        /// <summary>
        /// 快码类型
        /// </summary>
        public string CatalogType
        {
            get { return this.GetProperty(CatalogTypeProperty); }
            set { this.SetProperty(CatalogTypeProperty, value); }
        }
        #endregion

        #region 属性组 PropertyGroup
        /// <summary>
        /// 属性组
        /// </summary>
        [Label("属性组")]
        public static readonly Property<string> PropertyGroupProperty = P<ItemPropertyValueViewModel>.Register(e => e.PropertyGroup);

        /// <summary>
        /// 属性组
        /// </summary>
        public string PropertyGroup
        {
            get { return this.GetProperty(PropertyGroupProperty); }
            set { this.SetProperty(PropertyGroupProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 物料属性值 配置
    /// </summary>
    internal class ItemPropertyValueViewModelConfig : EntityConfig<ItemPropertyValueViewModel>
    {

    }

    /// <summary>
    /// 物料属性值 配置视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class ItemPropertyValueViewModelViewConfig : WebViewConfig<ItemPropertyValueViewModel>
    {
        /// <summary>
        /// 是否快码
        /// </summary>
        [Label("IsCatalogType")]
        private static Expression<Func<ItemPropertyValueViewModel, bool>> IsCatalogType = p => p.PropertyType == ItemPropertyType.Catalog;

        /// <summary>
        /// 是否文本
        /// </summary>
        [Label("IsTextType")]
        private static Expression<Func<ItemPropertyValueViewModel, bool>> IsTextType = p => p.PropertyType == ItemPropertyType.Text;

        /// <summary>
        /// 是否数字
        /// </summary>
        [Label("IsNumberType")]
        private static Expression<Func<ItemPropertyValueViewModel, bool>> IsNumberType = p => p.PropertyType == ItemPropertyType.Number;

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(Item));
            View.Property(p => p.Definition).UsePagingLookUpEditor(p => p.XType = "DefinitionComboList").Readonly(false);
            View.Property(p => p.CatalogValue).Visibility(IsCatalogType)
                .UseCatalogEditor(x =>
                {
                    x.CatalogReloadData = true;
                    x.MethodClassName = "SIE.Web.Items.Items.ViewModels.CatalogPagingLookUpMethod";
                    x.DisplayField = Catalog.CodeProperty.Name;
                })
                .UseSelectionViewMeta(new MetaModel.View.SelectionViewMeta()
                {
                    SelectedValuePath = Catalog.CodeProperty,
                    DisplayMemberPath = Catalog.CodeProperty
                })
                .UseDataSource((e, c, r) =>
                {
                    var model = e as ItemPropertyValueViewModel;
                    if (model != null && model.CatalogType.IsNotEmpty())
                    {
                        return RT.Service.Resolve<CatalogController>().GetCatalogList(model.CatalogType);
                    }
                    return new EntityList<Catalog>() { };
                })
                .UseListSetting(e => { e.HelpInfo = "属性类型等于快码可见,显示当前快码类型列表".L10N(); });
            View.Property(p => p.TextValue).Visibility(IsTextType)
                .HasLabel("属性值*")
                .UseListSetting(e => { e.HelpInfo = "属性类型等于文本可见".L10N(); });
            View.Property(p => p.NumberValue).UseSpinEditor().Visibility(IsNumberType)
                .UseListSetting(e => { e.HelpInfo = "属性类型等于数字可见".L10N(); });
            View.Property(p => p.PropertyGroup);

        }
    }
}
