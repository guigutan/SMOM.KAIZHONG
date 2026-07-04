using SIE.Barcodes;
using SIE.Barcodes.Printables;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Wpf.Barcodes.ViewModels;
using SIE.Wpf.Editors;
using System;

namespace SIE.Wpf.Barcodes.Editors
{
    /// <summary>
    /// 打印模板编辑器
    /// </summary>
    public class PrintTemplateLookUpEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 打印模板编辑器名称
        /// </summary>
        public const string EditorName = "PrintTemplateLookUpEditor";

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
            var model = source as BarcodePrintViewModel;
            var templates = new EntityList<PrintTemplate>();
            if (model == null || model.NumberRule == null)
                return templates;

            return RT.Service.Resolve<BarcodeController>().GetPrintTemplatesByRuleId(model.NumberRuleId, typeof(BarcodePrintable).GetQualifiedName(), pagingInfo, keyword);
        }
    }
}