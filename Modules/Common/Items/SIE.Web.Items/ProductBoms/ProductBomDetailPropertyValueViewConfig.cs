using SIE.Items;

namespace SIE.Web.Items.ProductBoms
{
    /// <summary>
    /// 产品BOM明细物料属性值视图配置
    /// </summary>
    public class ProductBomDetailPropertyValueViewConfig : WebViewConfig<ProductBomDetailPropertyValue>
    {
        /// <summary>
        /// 扩展视图
        /// </summary>
        internal const string BomPropertyLookupView = "BomPropertyLookupView";

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
            View.AssignAuthorize(typeof(ProductBom));
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
            View.UseDefaultCommands();
            View.Property(p => p.Definition).Show(ShowInWhere.All).Readonly();////.UsePBDPDefinitionLookUpEditor(p => p.DisplayMember = ItemPropertyDefinition.NameProperty.Name);
            View.Property(p => p.Value).Readonly();////.UseEditor(PBDPValueLookUpEditor.EditorName);
            View.Property(p => p.PropertyGroup).Readonly();
        }

        /// <summary>
        /// 默认表单视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.Definition).UsePagingLookUpEditor(p => 
            { 
                p.DataSourceProperty = "true"; 
                p.XType = "WoProcessBomDefinition"; 
            }).Show(ShowInWhere.All);
            View.Property(p => p.Value);
        }

        /// <summary>
        /// 放大镜维护物料属性
        /// </summary>
        protected void ConfigBomPropertyLookupView()
        {
            View.ClearCommands();
            View.UseGridSelectionModel();
            using (View.OrderProperties())
            {
                View.Property(p => p.Definition).Show().Readonly();
                View.Property(p => p.Value).Show().Readonly();
                View.Property(p => p.PropertyGroup).Show().Readonly();
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            }
        }
    }
}