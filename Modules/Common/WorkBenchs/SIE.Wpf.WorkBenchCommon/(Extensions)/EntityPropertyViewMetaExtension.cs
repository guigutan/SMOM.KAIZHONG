using SIE.MetaModel.View;
using SIE.Wpf.WorkBenchCommon.Editors;
using System;

namespace SIE.Wpf.WorkBenchCommon
{
    /// <summary>
    /// 编辑器扩展类
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 数值维度编辑器扩展方法
        /// 年、月、周，数字格式化显示
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseDimensionEditorEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<DimensionConfig> action = null)
        {
            meta.ViewMeta.EditorName = DimensionEditor.EditorName;
            var config = new DimensionConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 指标分类编辑器扩展方法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseQuotaCategoryEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<QuotaCategoryConfig> action = null)
        {
            meta.ViewMeta.EditorName = QuotaCategoryEditor.EditorName;
            var config = new QuotaCategoryConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 指标名称编辑器扩展方法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseQuotaNameEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<QuotaNameEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = QuotaNameEditor.EditorName;
            var config = new QuotaNameEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }
    }
}