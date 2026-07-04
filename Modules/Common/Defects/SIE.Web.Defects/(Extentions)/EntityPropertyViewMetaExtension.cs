using SIE.MetaModel.View;
using SIE.Web.Defects.Editors;
using System;

namespace SIE.Web.Defects
{
    /// <summary>
    /// 缺陷视图属性扩展类
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 检验类型枚举筛选编辑器（暂时隐藏型式检验和联机检验）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseInspectionTypeEnumEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<SelectEnumConfig> action = null)
        {
            meta.ViewMeta.EditorName = WebEditorNames.Enum;
            var config = new SelectEnumConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }
    }
}
