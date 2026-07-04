using SIE.Barcodes;
using SIE.Barcodes.Printables;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Web.Barcodes.WipBatchs.ViewModels;
using SIE.Web.ClientMetaModel;
using System;
using System.Linq;

namespace SIE.Web.Barcodes
{
    /// <summary>
    /// 视图扩展类
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 条码的编码规则编辑器扩展方法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseNumberRuleBarcodeLookUpEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<NumberRuleController>().GetNumberRule(RuleType.Barcode, keyword, pagingInfo);
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 打印模板编辑器扩展方法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UsePrintTemplateLookUpEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var model = source as BarcodePrintViewModel;
                var templates = new EntityList<PrintTemplate>();
                if (model == null || model.NumberRule == null)
                    return templates;

                return RT.Service.Resolve<BarcodeController>().GetPrintTemplatesByRuleId(model.NumberRuleId.Value, typeof(BarcodePrintable).GetQualifiedName(), pagingInfo, keyword);
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 批次打印模板编辑器扩展方法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseBatchPrintTemplateLookUpEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var model = source as BatchGeneratingViewModel;
                var templates = new EntityList<PrintTemplate>();
                if (model == null || model.NumberRule == null || (model.GenerateChildren && model.ChildNumberRuleId == null))
                    return templates;

                if (model.GenerateChildren)
                    return RT.Service.Resolve<BarcodeController>().GetPrintTemplatesByRuleId(model.ChildNumberRuleId.Value, typeof(WipBatchPrintable).GetQualifiedName(), pagingInfo, keyword);
                else
                    return RT.Service.Resolve<BarcodeController>().GetPrintTemplatesByRuleId(model.NumberRuleId.Value, typeof(WipBatchPrintable).GetQualifiedName(), pagingInfo, keyword);
            }).UsePagingLookUpEditor(action);
            return meta;
        }        
    }
}