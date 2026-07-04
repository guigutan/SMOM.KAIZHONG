using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Resources.Enterprises;
using SIE.Wpf.Editors;

namespace SIE.Wpf.Resources.Editors
{
    /// <summary>
    /// 生产资源车间编辑器
    /// </summary>
    public class ResourceWorkShopEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "ResourceWorkShopEditor";

        /// <summary>
        /// 重新数据源
        /// </summary>
        /// <param name="source">实体</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">搜索关键词</param>
        /// <param name="titleProperty">IManagedProperty</param>
        /// <returns>车间列表</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            return RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(pagingInfo, keyword);
        }
    }
}