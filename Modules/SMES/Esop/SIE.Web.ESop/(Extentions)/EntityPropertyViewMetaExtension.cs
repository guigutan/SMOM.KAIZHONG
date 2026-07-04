using SIE.MetaModel.View;
using SIE.Resources.WipResources;
using SIE.Web.ClientMetaModel;
using System;
using System.Collections.Generic;

namespace SIE.Web.ESop
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
        public static WebEntityPropertyViewMeta<T> UseEnterpriseResourceEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                var wipResources = RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, new List<SyncSourceType>() { SyncSourceType.Enterprise }, pagingInfo, keyword);
                return wipResources;
            }).UsePagingLookUpEditor(action);
            return meta;
        }
    }
}
