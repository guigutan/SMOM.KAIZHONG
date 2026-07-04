using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Resources.Enterprises;
using SIE.Wpf.Editors;

namespace SIE.Wpf.Items.Editors
{
    /// <summary>
    /// 获取产线产能的产线编码/产线名称编辑器
    /// </summary>
    public class ProductModelLineCapacityLookUpEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "ProductModelLineCapacityLookUpEditor";

        /// <summary>
        /// 获取产线产能的产线编码/产线名称
        /// </summary>
        /// <param name="source">实体</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键词</param>
        /// <param name="titleProperty">托管属性</param>
        /// <returns></returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            return RT.Service.Resolve<EnterpriseController>().GetEnterprises(EnterpriseType.Line, pagingInfo, keyword);
        }
    }
}
