using SIE.MetaModel.View;
using SIE.Resources.WipResources;
using SIE.Web.ClientMetaModel;
using SIE.Web.Tech.Editors;
using SIE.Web.Tech.Routings.Editors;
using System;
using System.Collections.Generic;

namespace SIE.Web.Tech
{
    /// <summary>
    /// 视图视图元数据扩展类
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 生产资源编辑器：企业模型、设备台账
        /// 排除自定义类型的生产资源
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">wpf实体属性视图元数据（参数）</param>
        /// <param name="action">委托</param>
        /// <returns>wpf实体属性视图元数据（返回）</returns>
        public static WebEntityPropertyViewMeta<T> UseEnterpriseEquipmentResourceEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
            var typeList = new List<SyncSourceType>() { SyncSourceType.Enterprise, SyncSourceType.Equipment };
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, typeList, pagingInfo, keyword);
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        public static WebEntityPropertyViewMeta<T> UseRoutingDisplayEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<RoutingEditorConfig> action = null)
        {
            var config = new RoutingEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        public static WebEntityPropertyViewMeta<T> UseProcessConditionEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<ProcessConditionEditorConfig> action = null)
        {
            var config = new ProcessConditionEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }
    }
}
