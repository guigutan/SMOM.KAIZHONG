using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Packages.Packages;
using SIE.Wpf.Editors;

namespace SIE.Wpf.Packages.Packages.Editors
{
    /// <summary>s
    /// 单位编辑器，过滤主单位
    /// </summary>
    public class PackageRuleDetailLookUpEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 物料编辑器名称
        /// </summary>
        public const string EditorName = "PackageRuleDetailLookUpEditor";

        /// <summary>
        /// 重新加载数据源
        /// </summary>
        /// <param name="source">对象</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <param name="titleProperty">托管属性标记</param>
        /// <returns>不含主单位的列表</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            return RT.Service.Resolve<PackingUnitController>().GetUnitExceptMaster(keyword, pagingInfo);
        }
    }
}
