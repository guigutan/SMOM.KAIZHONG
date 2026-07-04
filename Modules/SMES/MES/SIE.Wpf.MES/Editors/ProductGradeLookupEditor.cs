using SIE.Domain;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.MES.BatchWIP;
using SIE.Wpf.Editors;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// 产品等级编辑器类
    /// </summary>
    public class ProductGradeLookupEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 产品等级编辑器名称
        /// </summary>
        public const string EditorName = "ProductGradeLookupEditor";

        /// <summary>
        /// 获取产品等级集合
        /// </summary>
        /// <param name="source">ProductGradeSetting实体</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <param name="titleProperty">托管属性</param>
        /// <returns>产品等级集合</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            var curOutputBatch = source as OutputBatch;
            var curWorkOrder = curOutputBatch?.WorkOrder;
            var curItemId = curWorkOrder?.ProductId ?? 0;
            var result = RT.Service.Resolve<ItemController>().GetProductGrades(curItemId, pagingInfo, null); ////pagingInfo    GetProductGradeList
            return result;
        }
    }
}
