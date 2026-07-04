using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Wpf.Editors;

namespace SIE.Wpf.Barcodes.Editors
{
    /// <summary>
    /// 条码打印编辑器
    /// </summary>
    public class NumberRuleBarcodeLookUpEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 条码打印编辑器名称
        /// </summary>
        public const string EditorName = "NumberRuleBarcodeLookUpEditor";

        /// <summary>
        /// 重新加载数据源
        /// </summary>
        /// <param name="source">实体</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="titleProperty">托管属性标记</param>
        /// <returns>条码打印列表</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            return RT.Service.Resolve<NumberRuleController>().GetNumberRule(RuleType.Barcode, keyword, pagingInfo);
        }
    }
}
