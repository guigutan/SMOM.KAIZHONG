using SIE.Core.Common;
using SIE.CSM.Common;
using SIE.CSM.Customers;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Web.ClientMetaModel;
using SIE.Web.CSM.Suppliers.ViewModels;
using System;
using System.Linq;

namespace SIE.Web.CSM._Extentions_
{
    /// <summary>
    /// 扩展编辑器
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 获取区域下拉编辑器
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="meta">meta</param>
        /// <param name="type">1、国家； 2、省； 3、市； 4、区</param>
        /// <param name="action">action</param>
        /// <returns>省列表</returns>
        public static WebEntityPropertyViewMeta<T> UseSupplierProvinceEditor<T>(this WebEntityPropertyViewMeta<T> meta, int type, Action<PagingLookUpBaseConfig> action = null)
        {
            string upperLevelRegion = string.Empty;
            string upperLevel2Region = string.Empty;
            if (meta == null)
                return meta;
            meta.ViewMeta.SelectionViewMeta = new SelectionViewMeta();
            meta.ViewMeta.SelectionViewMeta.SelectionEntityType = typeof(RegionalInfo);//实体
            meta.ViewMeta.SelectionViewMeta.DisplayMemberPath = RegionalInfo.RegionProperty;//显示属性
            meta.ViewMeta.SelectionViewMeta.SelectedValuePath = RegionalInfo.RegionProperty;//选择值属性
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var sourceList = new EntityList<RegionalInfo>();
                dynamic regional;
                if (source is RegionalViewModel)
                {
                    regional = source as RegionalViewModel;
                }
                else
                {
                    regional = source as BaseRegionalInfo;
                }
                //国家省市的控制
                switch (type)
                {
                    case 2:
                        upperLevelRegion = regional.Country;
                        upperLevel2Region = null;
                        break;
                    case 3:
                        upperLevelRegion = regional.Province;
                        upperLevel2Region = regional.Country;
                        break;
                    case 4: upperLevelRegion = regional.City; 
                        upperLevel2Region = regional.Province; 
                        break;
                    default: break;
                }
                if (type != 1 && upperLevelRegion.IsNullOrEmpty() && upperLevel2Region.IsNullOrEmpty())
                    return sourceList;
                sourceList.AddRange(RT.Service.Resolve<RegionalInfoController>().GetRegionalInfos(upperLevelRegion, upperLevel2Region, pagingInfo, keyword));
                sourceList.ForEach(p => p.TreePId = null);
                return sourceList;
            }).UsePagingLookUpEditor(action);

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
    }
}
