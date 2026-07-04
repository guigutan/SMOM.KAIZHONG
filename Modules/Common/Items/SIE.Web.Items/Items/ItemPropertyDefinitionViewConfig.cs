using SIE.Domain;
using SIE.Items;

namespace SIE.Web.Items
{
    /// <summary>
    /// 物料属性定义视图配置
    /// </summary>
    public class ItemPropertyDefinitionViewConfig : WebViewConfig<ItemPropertyDefinition>
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
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultCommands();

            View.Property(p => p.Name);
            View.Property(p => p.PropertyType).UseListSetting(e => { e.HelpInfo = "更换类型清空快码组"; }).Cascade(p => p.CatalogType, null);
            View.Property(p => p.CatalogType)
                .UsePagingLookUpEditor(p => p.Editable = true)
                .Readonly(p => p.PropertyType != ItemPropertyType.Catalog)
                .UseListSetting(e => { e.HelpInfo = "物料属性类型为快码时可编辑"; });
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
            View.Property(p => p.PropertyType).UseEnumEditor(p => p.AllowBlank = true);
            View.Property(p => p.CatalogType).UsePagingLookUpEditor(p => p.Editable = true);
        }
    }
}
