using SIE.Domain;
using SIE.Kit.MES.CallMaterials;
using SIE.MetaModel.View;
using SIE.Resources.WipResources;
using SIE.Web.ClientMetaModel;
using System;

namespace SIE.Web.Kit.MES._Extensions_
{
    /// <summary>
    /// 视图扩展类
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 加载资源列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseWipResourceCodeLookupEditor<T>(this WebEntityPropertyViewMeta<T> meta,
            Action<PagingLookUpBaseConfig> action = null)
        {
            meta.ViewMeta.SelectionViewMeta = new SelectionViewMeta();
            meta.ViewMeta.SelectionViewMeta.SelectionEntityType = typeof(WipResource);
            meta.ViewMeta.SelectionViewMeta.DisplayMemberPath = WipResource.CodeProperty;
            meta.ViewMeta.SelectionViewMeta.SelectedValuePath = WipResource.CodeProperty;

            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                if (source is CallMaterialWoCriteria)
                {
                    return RT.Service.Resolve<WipResourceController>().GetWipResourcesByEmp(pagingInfo, keyword);
                }
                return new EntityList<WipResource>();
            }).UsePagingLookUpEditor(action);
            return meta;
        }
    }
}
