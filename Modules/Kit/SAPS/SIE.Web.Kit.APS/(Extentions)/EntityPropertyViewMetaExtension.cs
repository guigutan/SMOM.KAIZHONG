using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Kit.APS.Common;
using SIE.Kit.APS.ProductLocations;
using SIE.MetaModel.View;
using SIE.SO.SaleOrders;
using SIE.Web.ClientMetaModel;
using System;

namespace SIE.Web.Kit.APS
{
    /// <summary>
    /// 
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        #region 客户
        /// <summary>
        /// 获取所有可用的客户
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="meta">meta</param>
        /// <param name="action">action</param>
        /// <returns>客户列表</returns>
        public static WebEntityPropertyViewMeta<T> GetEnableCustomersEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<CustomerController>().GetEnableCustomers(pagingInfo, keyword);
            });
            return meta;
        }

        /// <summary>
        /// 获取分类值下拉编辑器
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="meta">meta</param>
        /// <param name="action">action</param>
        /// <returns>列表</returns>
        public static WebEntityPropertyViewMeta<T> UseProductLocationTypeEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.ViewMeta.SelectionViewMeta = new SelectionViewMeta();
            meta.ViewMeta.SelectionViewMeta.SelectionEntityType = typeof(ClassificationInfo);
            meta.ViewMeta.SelectionViewMeta.DisplayMemberPath = ClassificationInfo.ValueProperty;
            meta.ViewMeta.SelectionViewMeta.SelectedValuePath = ClassificationInfo.KeyProperty;
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var sourceList = new EntityList<ClassificationInfo>();
                dynamic regional;
                if (source is ProductLocation)
                {
                    regional = source as ProductLocation;
                }
                else
                {
                    regional = source as ProductLocationCriteria;
                }
                if (regional.Classification != null)
                {
                    Classification upperLevel = regional.Classification;
                    sourceList = RT.Service.Resolve<ClassificationInfoController>().GetClassificationInfoList(upperLevel, pagingInfo, keyword);
                }
                return sourceList;
            }).UsePagingLookUpEditor(action);

            return meta;
        }

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
    }
   #endregion
    
}
