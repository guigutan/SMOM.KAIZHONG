using SIE.Items;
using SIE.Wpf.Items.Editors;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 产品BOM明细物料属性值视图配置
    /// </summary>
    public class ProductBomDetailPropertyValueViewConfig : WPFViewConfig<ProductBomDetailPropertyValue>
    {
        /// <summary>
        /// 扩展视图
        /// </summary>
        public static readonly string BomPropertyLookupView = "BomPropertyLookupView";

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultBehaviors().InlineEdit();
            if (ViewGroup == BomPropertyLookupView)
            {
                ConfigBomPropertyLookupView();
            }
        }

        /// <summary>
        /// 默认表格视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(BomDetailPropertyValueAddCommand), WPFCommandNames.ListEdit, WPFCommandNames.ListSave, WPFCommandNames.ListDelete);
            using (View.OrderProperties())
            {
                View.Property(p => p.Definition).Show(ShowInWhere.All).UsePBDPDefinitionLookUpEditor(p => p.DisplayMember = ItemPropertyDefinition.NameProperty.Name);
                View.Property(p => p.Value).UseEditor(PBDPValueLookUpEditor.EditorName);
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
                View.Property(p => p.Definition).Show(ShowInWhere.All).UsePBDPDefinitionLookUpEditor(p => p.DisplayMember = ItemPropertyDefinition.NameProperty.Name);
                ////View.Property(p => p.Definition.Name).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Value).UseEditor(PBDPValueLookUpEditor.EditorName);
            }
        }

        /// <summary>
        /// 放大镜维护物料属性
        /// </summary>
        protected void ConfigBomPropertyLookupView()
        {
            //View.UseCommands(typeof(ListAddCommand), typeof(ListEditCommand), typeof(ListDeleteCommand));
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Definition).Show();
                View.Property(p => p.DefinitionName).Show();
                View.Property(p => p.Value).Show();
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            }
        }
    }
}
