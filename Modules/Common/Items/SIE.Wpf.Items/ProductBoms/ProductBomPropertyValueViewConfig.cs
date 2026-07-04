using SIE.Items;
using SIE.Wpf.Items.Editors;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 产品BOM物料属性视图配置
    /// </summary>
    internal class ProductBomPropertyValueViewConfig : WPFViewConfig<ProductBomPropertyValue>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
            View.UseDefaultCommands().UseCommands(WPFCommandNames.ListSave);
            if (ViewGroup == DetailsView)
                View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Definition).Show(ShowInWhere.All).UsePBPDefinitionLookUpEditor(p => p.DisplayMember = ItemPropertyDefinition.NameProperty.Name);
                View.Property(p => p.Value).UseEditor(PBPValueLookUpEditor.EditorName).Show(ShowInWhere.All);
            }
        }

        /// <summary>
        /// 默认表格视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            using (View.OrderProperties())
            {
                View.Property(p => p.Definition).UsePBPDefinitionLookUpEditor(p => p.DisplayMember = ItemPropertyDefinition.NameProperty.Name);
                View.Property(p => p.Value).UseEditor(PBPValueLookUpEditor.EditorName);
            }
        }

        /// <summary>
        /// 默认表单视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Definition).UsePBPDefinitionLookUpEditor(p => p.DisplayMember = ItemPropertyDefinition.NameProperty.Name);
                View.Property(p => p.Value).UseEditor(PBPValueLookUpEditor.EditorName);
            }
        }
    }
}
