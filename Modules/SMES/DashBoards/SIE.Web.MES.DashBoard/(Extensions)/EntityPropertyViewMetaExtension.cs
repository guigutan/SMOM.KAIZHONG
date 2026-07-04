using SIE.Domain;
using SIE.Items;
using SIE.Items.ProductModels;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Web.ClientMetaModel;
using SIE.Web.MES.DashBoard.Common;
using System;

namespace SIE.Web.MES.DashBoard
{
    /// <summary>
    /// 班组管理编辑器类
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
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
        /// 资源车间下拉编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseResourceWorkShopEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var enterpriseList = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(pagingInfo, keyword);
                if (enterpriseList == null || enterpriseList.Count <= 0)
                    return new EntityList<Enterprise>();
                for (var i = 0; i < enterpriseList.Count; i++)
                {
                    enterpriseList[i].TreePId = null;
                }

                return enterpriseList;
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 班组下拉编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseWorkGroupEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var workGroupList = RT.Service.Resolve<WorkGroupController>().GetWorkGroups(pagingInfo, keyword);
                if (workGroupList == null || workGroupList.Count <= 0)
                    return new EntityList<WorkGroup>();
                return workGroupList;
            }).UsePagingLookUpEditor(action);
            return meta;
        }
        /// <summary>
        /// 产品机型下拉编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseProductModelEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var productModelList = RT.Service.Resolve<ProductModelController>().GetProductModels(keyword, pagingInfo);
                if (productModelList == null || productModelList.Count <= 0)
                    return new EntityList<ProductModel>();
                return productModelList;
            }).UsePagingLookUpEditor(action);
            return meta;
        }
    }
}