using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Wpf.Andon.Editors;
using SIE.Wpf.Editors;
using System;

namespace SIE.Wpf.Andon
{
    /// <summary>
    /// 编辑器扩展类
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 使用缺陷代码多选下拉框编辑器(目前只支持查询)
        /// </summary>
        /// <typeparam name="T"><see cref="Entity"/>实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>        
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>        
        public static WPFEntityPropertyViewMeta<T> UseDefectMultiSelectEditor<T>(this WPFEntityPropertyViewMeta<T> meta,
            Action<DefectMultiSelectEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = DefectMultiSelectEditor.EditorName;
            
            var config = new DefectMultiSelectEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }
    }
}