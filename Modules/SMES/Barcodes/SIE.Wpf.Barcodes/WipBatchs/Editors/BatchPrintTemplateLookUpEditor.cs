using SIE.Barcodes;
using SIE.Barcodes.Printables;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Wpf.Barcodes.WipBatchs.ViewModels;
using SIE.Wpf.Editors;
using System;

namespace SIE.Wpf.Barcodes.WipBatchs.Editors
{
    /// <summary>
    /// 批次生产模板编辑器
    /// </summary>
    public class BatchPrintTemplateLookUpEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 打印模板编辑器名称
        /// </summary>
        public const string EditorName = "BatchPrintTemplateLookUpEditor";

        /// <summary>
        /// 重新加载数据源
        /// </summary>
        /// <param name="source">实体</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="titleProperty">托管属性标记</param>
        /// <returns>打印模板列表</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            var model = source as BatchGeneratingViewModel;
            var templates = new EntityList<PrintTemplate>();
            if (model == null || model.NumberRule == null || (model.GenerateChildren && model.ChildNumberRuleId == null))
                return templates;

            if (model.GenerateChildren)
                return RT.Service.Resolve<BarcodeController>().GetPrintTemplatesByRuleId(model.ChildNumberRuleId.Value, typeof(WipBatchPrintable).GetQualifiedName(), pagingInfo, keyword);
            else
                return RT.Service.Resolve<BarcodeController>().GetPrintTemplatesByRuleId(model.NumberRuleId.Value, typeof(WipBatchPrintable).GetQualifiedName(), pagingInfo, keyword);
        }
    }
}
