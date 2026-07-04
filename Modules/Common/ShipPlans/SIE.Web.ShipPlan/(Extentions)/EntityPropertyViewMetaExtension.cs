using SIE.CSM.Customers;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.ShipPlan;
using SIE.Web.ClientMetaModel;
using System;
using SIE.Inventory.Commom;

namespace SIE.Web.ShipPlan
{
    /// <summary>
    /// 实体属性视图元数据扩展
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 货主编码选择客户和货主数据编辑器，用于返回编码字符串
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="meta">meta</param>
        /// <param name="action">action</param>
        /// <returns>货主编码编辑器</returns>
        public static WebEntityPropertyViewMeta<T> UseDeliveryPlanStorerCodeEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            if (meta == null)
            {
                return null;
            }

            meta.ViewMeta.SelectionViewMeta = new SelectionViewMeta();
            meta.ViewMeta.SelectionViewMeta.SelectionEntityType = typeof(Customer);
            meta.ViewMeta.SelectionViewMeta.DisplayMemberPath = Customer.CodeProperty;
            meta.ViewMeta.SelectionViewMeta.SelectedValuePath = Customer.CodeProperty;
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var plan = source as DeliveryPlan;
                if (plan != null)
                {
                    return RT.Service.Resolve<CustomerController>().GetEnableCustomerDatas(pagingInfo, keyword, false);
                }

                return new EntityList<Customer>();
            }).UsePagingLookUpEditor(action);

            return meta;
        }

        /// <summary>
        /// 货主编码选择客户和货主数据编辑器，用于返回编码字符串
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="meta">meta</param>
        /// <param name="action">action</param>
        /// <returns>货主编码编辑器</returns>
        public static WebEntityPropertyViewMeta<T> UseDeliveryPlanLotEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            if (meta == null)
            {
                return null;
            }

            meta.ViewMeta.SelectionViewMeta = new SelectionViewMeta();
            meta.ViewMeta.SelectionViewMeta.SelectionEntityType = typeof(Lot);
            meta.ViewMeta.SelectionViewMeta.DisplayMemberPath = Lot.CodeProperty;
            meta.ViewMeta.SelectionViewMeta.SelectedValuePath = Lot.CodeProperty;
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var plan = source as DeliveryPlan;

                if (plan == null || plan.ItemId <= 0)
                {
                    return new EntityList<Lot>();
                }
                return RT.Service.Resolve<LotController>().GetLotsByItem(plan.ItemId, plan.ItemExtPropName, keyword, pagingInfo);
            }).UsePagingLookUpEditor(action);

            return meta;
        }
    }
}