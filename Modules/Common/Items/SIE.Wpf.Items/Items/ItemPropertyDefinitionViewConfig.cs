using SIE.Domain;
using SIE.Items;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 物料属性定义视图配置
    /// </summary>
    public class ItemPropertyDefinitionViewConfig : WPFViewConfig<ItemPropertyDefinition>
    {
        #region 类型是否为快码
        /// <summary>
        /// 类型是否为快码
        /// </summary>
        internal static readonly Property<bool> IsCatalogProperty = P<ItemPropertyDefinition>
            .RegisterExtensionReadOnly("IsCatalog", typeof(ItemPropertyDefinitionViewConfig), GetIsCatalog, ItemPropertyDefinition.PropertyTypeProperty);

        /// <summary>
        /// 类型是否为快码
        /// </summary>
        /// <param name="me">物料属性定义</param>
        /// <returns>是否为快码</returns>
        internal static bool GetIsCatalog(ItemPropertyDefinition me)
        {
            if (me.PropertyType != ItemPropertyType.Catalog)
            {
                me.CatalogType = null;
                return true;
            }
            else
                return false;
        }
        #endregion

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            // 默认视图配置
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultCommands().UseDefaultBehaviors();
            View.Property(p => p.Name);
            View.Property(p => p.PropertyType);
            View.Property(p => p.CatalogType).Readonly(IsCatalogProperty);
        }

        /// <summary>
        /// 下拉选择列表视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.Name);
            View.Property(p => p.PropertyType);
            View.Property(p => p.CatalogType);
        }

        /// <summary>
        /// 默认查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.PropertyType);
            View.Property(p => p.CatalogType);
        }
    }
}
