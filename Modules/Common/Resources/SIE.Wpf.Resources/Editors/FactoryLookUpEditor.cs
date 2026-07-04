using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Resources.Enterprises;
using SIE.Wpf.Editors;

namespace SIE.Wpf.Resources.Editors
{
    /// <summary>
    /// 企业模型中类型为工厂的下拉框
    /// </summary>
    public class FactoryLookUpEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "FactoryLookUpEditor";

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
            return RT.Service.Resolve<EnterpriseController>().GetEnterprises(EnterpriseType.Plant, pagingInfo, keyword);
        }
    }
}
