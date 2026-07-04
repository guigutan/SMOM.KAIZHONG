using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Resources.CalendarSchemes;
using SIE.Resources.Enterprises;
using SIE.Web.ClientMetaModel;
using System;
using System.Linq;

namespace SIE.Web.Resources
{
    /// <summary>
    /// 扩展编辑器
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 资源车间下拉编辑器扩展方法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseShopEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var enterprises = RT.Service.Resolve<EnterpriseController>().GetEnterprises(EnterpriseType.Shop, pagingInfo, keyword);
                enterprises.ForEach(enterprise => { enterprise.TreePId = null; });
                return enterprises;
            }).UsePagingLookUpEditor(action);
            return meta;
        }
        /// <summary>
        /// 资源车间下拉编辑器扩展方法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UsePlantEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var enterprises = RT.Service.Resolve<EnterpriseController>().GetEnterprises(EnterpriseType.Plant, pagingInfo, keyword);
                enterprises.ForEach(enterprise => { enterprise.TreePId = null; });
                return enterprises;
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 资源车间下拉编辑器扩展方法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseEnterpriseShopEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var enterprises = RT.Service.Resolve<EnterpriseController>().GetEnterpriseShops(EnterpriseType.Shop, pagingInfo, keyword);
                enterprises.ForEach(enterprise => { enterprise.TreePId = null; });
                return enterprises;
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 资源日历方案下拉编辑器扩展方法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseSchemeLookUpEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var result = RT.Service.Resolve<CalendarSchemeController>().GetEnableCalendarSchemeList(pagingInfo, keyword);
                if (result == null) return new EntityList<CalendarScheme>();
                return result;
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 资源车间下拉编辑器扩展方法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseWipWorkShopEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var enterprises = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(pagingInfo, keyword);
                enterprises.ForEach(enterprise => { enterprise.TreePId = null; });
                return enterprises;
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 工厂下拉编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseFactoryEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var enterpriseList = RT.Service.Resolve<EnterpriseController>().GetEmployeeFactoriesList(pagingInfo, keyword);
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
    }
}
