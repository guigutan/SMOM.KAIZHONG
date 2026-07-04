using SIE.Domain;
using SIE.Items;
using SIE.Items.ViewModels;
using SIE.ManagedProperty;

namespace SIE.Wpf.Items.ViewModels
{
    /// <summary>
    /// 物料子视图，物料属性视图
    /// </summary>
    [CompiledPropertyDeclarer]
    public class ItemPropertyValueViewModelViewConfig : WPFViewConfig<PropertyValueViewModel>
    {
        /// <summary>
        /// 自定义ViewGroup
        /// </summary>
        internal const string ItemPropertyValueViewModelView = "ItemPropertyValueViewModelViewConfig";

        /// <summary>
        /// 物料属性值
        /// </summary>
        internal static readonly Property<string> ItemValueProperty = P<PropertyValueViewModel>.RegisterExtensionReadOnly("ItemValue", typeof(ItemPropertyValueViewModelViewConfig), GetValue, PropertyValueViewModel.ValuesProperty);

        /// <summary>
        /// 物料属性值
        /// </summary>
        /// <param name="me">属性值模板</param>
        /// <returns>string</returns>
        internal static string GetValue(PropertyValueViewModel me)
        {
            return string.Join(";", me.Values);
        }

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ItemPropertyValueViewModelView);

            if (ViewGroup == ItemPropertyValueViewModelView)
            {
                View.DisableEditing();
                View.UseCommands(typeof(ItemPropertyValueAddCommand), typeof(ItemPropertyValueDeleteCommand));
                using (View.OrderProperties())
                {
                    View.Property(p => p.DefinitionName).Show(ShowInWhere.All);
                    View.Property(p => p.Value).Show(ShowInWhere.Detail);
                    View.Property(ItemPropertyValueViewModelViewConfig.ItemValueProperty).HasLabel("值").Show(ShowInWhere.List);
                }
            }
        }
    }

    /// <summary>
    /// 产品BOM视图，物料属性值子视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class BomPropertyValueViewModelViewConfig : WPFViewConfig<PropertyValueViewModel>
    {
        /// <summary>
        /// 产品BOM物料属性子视图，ViewGroup
        /// </summary>
        internal const string BomPropertyValueViewModelListView = "BomPropertyValueViewModelListView";

        /// <summary>
        /// 产品BOM属性值拓展属性
        /// </summary>
        internal static readonly Property<string> BomValueProperty = P<PropertyValueViewModel>.RegisterExtensionReadOnly("BomValue", typeof(BomPropertyValueViewModelViewConfig),
            GetValue, PropertyValueViewModel.ValuesProperty);

        /// <summary>
        /// 产品BOM属性值拓展属性
        /// </summary>
        /// <param name="me">PropertyValueViewModel</param>
        /// <returns>string</returns>
        internal static string GetValue(PropertyValueViewModel me)
        {
            return string.Join(";", me.Values);
        }

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(BomPropertyValueViewModelListView);
            View.InlineEdit();
            if (ViewGroup == BomPropertyValueViewModelListView)
            {
                View.UseCommands(typeof(BomPropertyValueAddCommand), WPFCommandNames.ListEdit, typeof(BomPropertyValuesSaveCommand), typeof(BomPropertyValueDeleteCommand));
                using (View.OrderProperties())
                {
                    View.Property(p => p.Definition).UsePBPDefinitionLookUpEditor(p => p.DisplayMember = nameof(ItemPropertyDefinition.Name)).Show(ShowInWhere.All).HasLabel("属性");
                    View.Property(BomPropertyValueViewModelViewConfig.BomValueProperty).UsePropertyValueEditor().HasLabel("值").Show(ShowInWhere.List).Readonly(false);
                }
            }
        }
    }
}