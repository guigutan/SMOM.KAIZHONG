using SIE.CSM.Customers;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.SO.SaleOrders;
using SIE.Web.ClientMetaModel;
using System;

namespace SIE.Web.SO
{
    public static class EntityPropertyViewMetaExtension
    {
        #region 销售订单
        /// <summary>
        /// 获取销售订单的客户下拉编辑器
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="meta">meta</param>
        /// <param name="action">action</param>
        /// <returns>客户列表</returns>
        public static WebEntityPropertyViewMeta<T> UseSalesOrderCustomerEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                if (source is SaleOrder)
                {
                    var order = source as SaleOrder;
                    if (order != null)
                    {
                        return RT.Service.Resolve<SaleOrderController>().GetSelfMadeOrOutMadeItem(pagingInfo, keyword);
                    }
                }

                return new EntityList<Customer>();
            });
            return meta;
        }

        /// <summary>
        /// 获取销售订单明细的物料下拉编辑器
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="meta">meta</param>
        /// <param name="action">action</param>
        /// <returns>物料列表</returns>
        public static WebEntityPropertyViewMeta<T> UseSalesOrderDetailItemEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                if (source is SaleOrderDetail)
                {
                    var order = source as SaleOrderDetail;
                    if (order != null)
                    {
                        return RT.Service.Resolve<SaleOrderDetailController>().GetSelfMadeOrOutMadeItem(pagingInfo,keyword);
                    }
                }
                return new EntityList<Customer>();
            });
            return meta;
        }


        /// <summary>
        /// 获取销售订单明细的销售下拉编辑器
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="meta">meta</param>
        /// <param name="action">action</param>
        /// <returns>物料列表</returns>
        public static WebEntityPropertyViewMeta<T> UseSalesOrderEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                if (source is SaleOrder)
                {
                    var order = source as SaleOrder;
                    if (order != null)
                    {
                        return RT.Service.Resolve<SaleOrderController>().GetSalesOrderList(pagingInfo, keyword);
                    }
                }
                return new EntityList<SaleOrder>();
            });
            return meta;
        }

        #endregion
    }
}

