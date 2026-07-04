using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Warehouses;
using SIE.Wpf.Editors;

namespace SIE.Wpf.Warehouses.Editors
{
    /// <summary>
    /// 库区编辑器
    /// </summary>
    public class StorageAreaEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 库区编辑器名称
        /// </summary>
        public const string EditorName = "StorageAreaEditor";

        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="source">实体</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键词</param>
        /// <param name="titleProperty">托管属性</param>
        /// <returns>EntityList</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            var storageLocation = source as StorageLocation;
            if (storageLocation == null) return new EntityList<StorageArea>();
            return RT.Service.Resolve<WarehouseController>().GetStorageArea(storageLocation.LibraryType, storageLocation.WarehouseId, keyword, pagingInfo);
        }
    }
}
