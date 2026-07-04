using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Web.ClientMetaModel;
using SIE.Web.MES.TeamManagement.Editors;
using System;
using System.Linq;

namespace SIE.Web.MES.TeamManagement
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
        /// 车间编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseResourceWorkShopEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var workshop = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(pagingInfo, keyword);
                if (workshop == null || workshop.Count <= 0)
                    return new EntityList<Enterprise>();
                workshop.ForEach(p => p.TreePId = null);
                return workshop;
            }).UsePagingLookUpEditor(action);
            return meta;
        }
    }
}