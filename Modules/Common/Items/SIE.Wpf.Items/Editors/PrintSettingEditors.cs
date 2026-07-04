using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Core.Items;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Wpf.Editors;
using System.Linq;

namespace SIE.Wpf.Items.Editors
{
    /// <summary>
    /// 条码规则下拉编辑器
    /// </summary>
    public class NumberRuleLookupEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "NumberRuleLookupEditor";

        /// <summary>
        /// 重新加载数据源
        /// </summary>
        /// <param name="source">实体</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="titleProperty">托管属性标记</param>
        /// <returns>资源列表</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            return RT.Service.Resolve<NumberRuleController>().GetNumberRule(RuleType.Barcode, keyword, pagingInfo);
        }
    }

    /// <summary>
    /// 标签模板下拉编辑器
    /// </summary>
    public class BarcodeModelLookupEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "BarcodeModelLookupEditor";

        /// <summary>
        /// 重新加载数据源
        /// </summary>
        /// <param name="source">实体</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="titleProperty">托管属性标记</param>
        /// <returns>资源列表</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            var template = source as LabelPrintTemplate;
            var templates = new EntityList<PrintTemplate>();
            if (template == null || template.NumberRule == null)
                return templates;
            template.NumberRule.TemplateList.Where(p => p.Template.State == State.Enable).ForEach(p => templates.Add(p.Template));
            return templates;
        }
    }

    /// <summary>
    /// 包装模板下拉编辑器
    /// </summary>
    public class PackageModelLookupEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "PackageModelLookupEditor";

        /// <summary>
        /// 重新加载数据源
        /// </summary>
        /// <param name="source">实体</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="titleProperty">托管属性标记</param>
        /// <returns>资源列表</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            var template = source as LabelPrintTemplate;
            var templates = new EntityList<PrintTemplate>();
            if (template == null || template.NumberRule == null)
                return templates;
            template.NumberRule.TemplateList.Where(p => p.Template.State == State.Enable).ForEach(p => templates.Add(p.Template));
            return templates;
        }
    }
}