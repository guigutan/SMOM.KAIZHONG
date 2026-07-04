using SIE.Wpf.Editors;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Warehouses;

namespace SIE.Wpf.Warehouses.Editors
{
    /// <summary>
    /// 可用非冻结仓库编辑器
    /// </summary>
    public class AvailableWarehouseEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "AvailableWarehouseEditor";

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
            return RT.Service.Resolve<WarehouseController>().GetAvailableWarehouses(pagingInfo, keyword + "%");
        }
    }
}
