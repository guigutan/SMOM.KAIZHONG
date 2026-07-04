using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Items.ViewModels;
using SIE.MetaModel;
using System.Text;

namespace SIE.Wpf.Items.ViewModels
{
    /// <summary>
    /// 物料属性 ViewModel 实体配置
    /// </summary>
    internal class ItemPropertyViewModelConfig : EntityConfig<ItemPropertyViewModel>
    {
        /// <summary>
        /// 重写增加验证方法
        /// </summary>
        /// <param name="rules">规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.Add((o, e) =>
            {
                ItemPropertyViewModel itempropertyViewModel = o as ItemPropertyViewModel;
                if (itempropertyViewModel.IsCatalog && itempropertyViewModel.Catalog == null)
                {
                    e.BrokenDescription = "属性值不能为空";
                }
            });
            base.AddValidations(rules);
        }
    }

    /// <summary>
    /// 物料属性 ViewModel 视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class ItemPropertyViewModelViewConfig : WPFViewConfig<ItemPropertyViewModel>
    {
        /// <summary>
        /// 托管属性值，有相同属性的值，串在一起
        /// </summary>
        internal static readonly Property<string> DisValuesProperty = P<ItemPropertyViewModel>.RegisterExtensionReadOnly("DisValues", typeof(ItemPropertyViewModelViewConfig), GetValues, ItemPropertyViewModel.DefinitionIdProperty);

        /// <summary>
        /// 托管属性值
        /// </summary>
        /// <param name="me">物料属性模板</param>
        /// <returns>属性值字符串</returns>
        internal static string GetValues(ItemPropertyViewModel me)
        {
            StringBuilder values = new StringBuilder();
            foreach (var value in me.Values)
            {
                values.Append(value);
            }
            return values.ToString();
        }

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
            View.UseCommands(typeof(ItemPropertyValueAddCommand), WPFCommandNames.ListEdit);
        }

        /// <summary>
        /// 默认List视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Definition).Readonly();
                View.Property(p => p.DefinitionName).Readonly();
                View.Property(ItemPropertyViewModelViewConfig.DisValuesProperty).UseEditor("ItemPropertyEditor").HasLabel("属性值");
            }
        }

        /// <summary>
        /// 默认表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Definition).UsePagingLookUpEditor(p => p.DisplayMember = ItemPropertyDefinition.NameProperty.Name);
                ////View.Property(p => p.Definition.Name).Readonly();
                View.Property(p => p.Catalog).UseItemPropertyCatalogEditor().Visibility(ItemPropertyViewModel.IsCatalogProperty);
                View.Property(p => p.Value).Visibility(ItemPropertyViewModel.IsStringProperty);
                View.Property(p => p.NumberValue).Visibility(ItemPropertyViewModel.IsNumberProperty);
                View.Property(p => p.PropertyGroup);
            }
        }

        /// <summary>
        /// 默认查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Definition);
            View.Property(p => p.DefinitionName);
        }
    }
}
