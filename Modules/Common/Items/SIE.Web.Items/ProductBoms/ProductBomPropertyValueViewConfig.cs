using SIE.Items;

namespace SIE.Web.Items.ProductBoms
{
    /// <summary>
    /// 产品BOM物料属性视图配置
    /// </summary>
    internal class ProductBomPropertyValueViewConfig : WebViewConfig<ProductBomPropertyValue>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
            View.UseDefaultCommands();
        }

        /// <summary>
        /// 默认表格视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Definition);////.UsePBPDefinitionLookUpEditor(p => p.DisplayMember = ItemPropertyDefinition.NameProperty.Name);
            View.Property(p => p.Value);////.UseEditor(PBPValueLookUpEditor.EditorName);
        }

        /// <summary>
        /// 默认表单视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.Definition);////.UsePBPDefinitionLookUpEditor(p => p.DisplayMember = ItemPropertyDefinition.NameProperty.Name);
            View.Property(p => p.Value);////.UseEditor(PBPValueLookUpEditor.EditorName);
        }
    }
}
