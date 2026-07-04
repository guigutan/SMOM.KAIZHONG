using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel.View;
using SIE.RedCardManagment.RedCardApplyBills;
using SIE.Web.ClientMetaModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.RedCardManagment._Extentions_
{
    /// <summary>
    /// 扩展静态类，用于重写编辑器
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 供应商下拉选择编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseSupplierLookupEidtor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                if (source is RedCardApplyBill)
                {
                    var bill = source as RedCardApplyBill;
                    if (bill.Item != null)
                        return RT.Service.Resolve<SupplierController>().GetSuppliers(pagingInfo, keyword, bill.ItemId);
                    else
                        return RT.Service.Resolve<SupplierController>().GetSuppliers(pagingInfo, keyword, null);
                }

                return new EntityList<Supplier>();
            });
            return meta;
        }

        /// <summary>
        /// 红牌申请单 物料下拉选择编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseItemLookUpEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                if (source is RedCardApplyBill)
                {
                    var bill = source as RedCardApplyBill;
                    if (bill == null) return null;
                    if (bill.Supplier != null)
                    {
                            return RT.Service.Resolve<SupplierController>().GetItems(pagingInfo, keyword, bill.SupplierId);
                    }
                    else
                    {
                            return RT.Service.Resolve<SupplierController>().GetItems(pagingInfo, keyword, null);
                    }
                }

                return new EntityList<Item>();
            });
            return meta;
        }
    }
}
