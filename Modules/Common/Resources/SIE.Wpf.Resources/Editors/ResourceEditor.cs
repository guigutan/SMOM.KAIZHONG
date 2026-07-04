using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Resources.Enterprises;
using SIE.Wpf.Editors;

namespace SIE.Wpf.Resources.Editors
{
    /// <summary>
    /// 资源编辑器
    /// </summary>
    public class ResourceEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "ResourceEditor";

        /// <summary>
        /// 企业类型
        /// </summary>
        public const string Type = "EnterpriseType";

        /// <summary>
        /// 重新数据源
        /// </summary>
        /// <param name="source">实体</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">搜索关键词</param>
        /// <param name="titleProperty">IManagedProperty</param>
        /// <returns>EntityList</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            EnterpriseType enterpriseType = base.Config.GetPropertyOrDefault<EnterpriseType>(Type);
            return RT.Service.Resolve<EnterpriseController>().GetResources(enterpriseType, pagingInfo, keyword);
        }
    }
}
