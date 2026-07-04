using SIE.MetaModel.View;
using SIE.Wpf.Editors;
using SIE.Wpf.MES.TaskManagement.Reports.Editors;
using System;

namespace SIE.Wpf.MES.TaskManagement
{
    /// <summary>
    /// 编辑器扩展类
    /// </summary>
    public static class EntityPropertyViewMetaExtension 
    {
        /// <summary>
        /// 录入报工缺陷编辑器扩展方法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseReportDefectEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<TextEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ReportDefectEditor.EditorName;
            var config = new TextEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }
    }
}
