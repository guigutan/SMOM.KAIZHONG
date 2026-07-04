using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Warehouses;
using SIE.Wpf.Editors;

namespace SIE.Wpf.Warehouses.Editors
{
    /// <summary>
    /// 根据仓库类型来获取仓库数据下拉编辑器
    /// </summary>
    public class WarehouseLookUpEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "WarehouseLookUpEditor";

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
            var storagearea = source as StorageArea;
            if (storagearea != null)
            {
                return RT.Service.Resolve<WarehouseController>().GetWarehouseDataList(storagearea.LibraryType, keyword, pagingInfo);
            }

            var storageLocation = source as StorageLocation;
            if (storageLocation != null)
            {
                return RT.Service.Resolve<WarehouseController>().GetWarehouseDataList(storageLocation.LibraryType, keyword, pagingInfo);
            }

            return new EntityList<Warehouse>();
        }
    }
}
