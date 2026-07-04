using SIE.Items;

namespace SIE.Web.Items.ProductBoms
{
    /// <summary>
    /// 组合替代 物料属性视图 
    /// </summary>
    public class CombinationReplatePropertyValueViewConfig : WebViewConfig<CombinationReplatePropertyValue>
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
                ConfigBomPropertyLookupView();
        }

        /// <summary>
        /// 默认表格视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.Definition).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.Value).Readonly();
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
