using SIE.MetaModel.View;
using SIE.Wpf.Barcodes.BatchBarcodes.Editors;
using SIE.Wpf.Barcodes.Editors;
using SIE.Wpf.Barcodes.WipBatchs.Editors;
using SIE.Wpf.Editors;
using System;

namespace SIE.Wpf.Barcodes
{
    /// <summary>
    /// 实体属性视图元数据扩展
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 使用批次数量编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseItemBatchRuleLookUpEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<EnumEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemBatchRuleLookUpEditor.EditorName;
            var config = new EnumEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用条码的编码规则编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseNumberRuleBarcodeLookUpEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = NumberRuleBarcodeLookUpEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用打印模板编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UsePrintTemplateLookUpEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = PrintTemplateLookUpEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用批次生成模板编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseBatchPrintTemplateLookUpEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = BatchPrintTemplateLookUpEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }
    }
}