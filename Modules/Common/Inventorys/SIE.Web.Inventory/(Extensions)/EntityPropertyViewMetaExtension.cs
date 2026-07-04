using SIE.Common.Prints;
using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Inventory.Strategy;
using SIE.Inventory.Task;
using SIE.Inventory.Task.ViewModels;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.ClientMetaModel;
using SIE.Web.Inventory.Common;
using System;

namespace SIE.Web.Inventory
{
    /// <summary>
    /// 实体属性视图元数据扩展
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 使用物料扩展属性编辑器（必须指定FunctionType功能类型）
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="meta">实体属性视图元数据 <see cref="WebEntityPropertyViewMeta"/></param>
        /// <param name="action">委托</param>
        /// <returns>实体属性视图元数据</returns>
        ///  <example> View.Property(p => p.Text).UseTextButtonFieldEditor();</example>
        public static WebEntityPropertyViewMeta<T> UseItemExtPropertyFieldEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<ItemExtPropertyFieldConfig> action = null)
        {
            var config = new ItemExtPropertyFieldConfig
            {
                ExtendJsObj = "SIE.Web.Inventory.Editors.ItemExtPropertyEditor"
            };
            action?.Invoke(config);
            meta.ViewMeta.Config = config;
            return meta;
        }

        /// <summary>
        /// D/C输入编辑器
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="meta">视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>实体属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseDCInputEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<DCInputFieldConfig> action = null)
        {
            var config = new DCInputFieldConfig
            {
                ExtendJsObj = "SIE.Web.Inventory.Editors.DCInputEditor"
            };
            action?.Invoke(config);
            meta.ViewMeta.Config = config;
            return meta;
        }

        /// <summary>
        /// 使用多分类过滤枚举编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>Web视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseMultiFilterEnumEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<MultiFilterEnumBoxConfig> action = null)
        {
            meta.ViewMeta.EditorName = MultiFilterEnumBoxConfig.EditorName;
            var config = new MultiFilterEnumBoxConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 获取单据大类枚举编辑器
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="meta">meta</param>
        /// <param name="action">action</param>
        /// <returns>单据大类</returns>
        public static WebEntityPropertyViewMeta<T> UseSelectEnumEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<SelectEnumConfig> action = null)
        {
            meta.ViewMeta.EditorName = WebEditorNames.Enum;
            var config = new SelectEnumConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 选择货主编辑器
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="meta">meta</param>
        /// <param name="action">action</param>
        /// <returns>货主编辑器</returns>
        public static WebEntityPropertyViewMeta<T> UseSelectStorerCodeEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.ViewMeta.SelectionViewMeta = new SelectionViewMeta
            {
                SelectionEntityType = typeof(Customer),
                DisplayMemberPath = Customer.CodeProperty,
                SelectedValuePath = Customer.CodeProperty
            };
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                if (source is not TaskAllotRule)
                {
                    return new EntityList<Customer>();
                }

                return RT.Service.Resolve<CustomerController>().GetEnableCustomerDatas(pagingInfo, keyword);

            }).UsePagingLookUpEditor(action);

            return meta;
        }

        /// <summary>
        /// 使用打印单据规则编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseBillPrintEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                if (source is TaskManagementPrintViewModel)
                {
                    string qualifiedName = typeof(TaskManagementPrintable).GetQualifiedName();
                    return RT.Service.Resolve<WarehouseController>().GetPrintTemplatesByType(qualifiedName, keyword, pagingInfo, PrintType.Bill);
                }

                return new EntityList<PrintTemplate>();
            }).UsePagingLookUpEditor(action);
            return meta;
        }
    }
}
