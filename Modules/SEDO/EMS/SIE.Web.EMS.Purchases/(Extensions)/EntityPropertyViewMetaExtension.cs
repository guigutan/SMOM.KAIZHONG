using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.MetaModel.View;
using SIE.Web.ClientMetaModel;
using System;

namespace SIE.Web.EMS.Purchases._Extensions_
{
    /// <summary>
    /// 扩展编辑器
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 使用采购对象编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UsePurchaseObjectEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PurchaseObjectBoxConfig> action = null)
        {
            meta.ViewMeta.EditorName = WebEditorNames.Enum;
            var config = new PurchaseObjectBoxConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用客户信息编辑器
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="meta">meta</param>
        /// <param name="customerType">客户类型</param>
        /// <param name="action">action</param>
        /// <returns>客户列表</returns>
        public static WebEntityPropertyViewMeta<T> UseCustomerEditor<T>(this WebEntityPropertyViewMeta<T> meta, CustomerType customerType, Action<PagingLookUpBaseConfig, T> action = null) where T : new()
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<CustomerController>().GetCustomer(customerType, keyword, pagingInfo);
            }).UsePagingLookUpEditor(action);
            return meta;
        }


        /// <summary>
        /// 使用供应商信息编辑器
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="meta">meta</param>
        /// <param name="action">action</param>
        /// <returns>客户列表</returns>
        public static WebEntityPropertyViewMeta<T> UseSupplierEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig, T> action = null) where T : new()
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<SupplierController>().GetSuppliers(pagingInfo, keyword);
            }).UsePagingLookUpEditor(action);
            return meta;
        }
    }
}
