using SIE.AbnormalInfo.AbnormalInfos;
using SIE.Defects;
using SIE.MetaModel.View;
using SIE.Web.ClientMetaModel;
using SIE.Web.MetaModelPortal.ClientMetaModel.UIBlock.Form;
using System;

namespace SIE.Web.AbnormalInfo._Extentions_
{
    /// <summary>
    /// 实体属性扩展编辑器
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 异常编码编辑器
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="meta">实体元数据</param>
        /// <param name="action">委托</param>
        /// <returns>实体元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseAbnormalCodeEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<TextButtonFieldConfig> action = null)
        {
            var config = new TextButtonFieldConfig();
            config.ExtendJsObj = "SIE.Web.AbnormalInfo.AbnormalInfos.Editors.AbnormalCodeEditor";
            action?.Invoke(config);
            meta.ViewMeta.Config = config;
            return meta;
        }

        /// <summary>
        /// 推送方式选择器，只选择异常信息插件的关联的推送方式
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseAbnormalPushersEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<AbnormalInfoController>().GetAbnormalPushers();
            });
            return meta;
        }

        /// <summary>
        /// 缺陷代码编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseDefectLookupEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.ViewMeta.SelectionViewMeta = new SelectionViewMeta();
            meta.ViewMeta.SelectionViewMeta.SelectionEntityType = typeof(Defect);
            meta.ViewMeta.SelectionViewMeta.DisplayMemberPath = Defect.CodeProperty;
            meta.ViewMeta.SelectionViewMeta.SelectedValuePath = Defect.CodeProperty;
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<DefectController>().GetDefectList(keyword, pagingInfo);
            }).UsePagingLookUpEditor(action);

            return meta;
        }
    }
}
