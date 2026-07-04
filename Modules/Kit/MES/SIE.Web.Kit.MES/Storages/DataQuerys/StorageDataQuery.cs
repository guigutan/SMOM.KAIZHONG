using SIE.Domain;
using SIE.Kit.MES.Storages;
using SIE.Web.Data;

namespace SIE.Web.Kit.MES.Storages.DataQuerys
{
    /// <summary>
    /// 工位货区查询器
    /// </summary>
    public class StorageDataQuery : DataQueryer
    {
        /// <summary>
        /// 获取产线物料货位属性值
        /// </summary>
        /// <param name="detailId">产线物料货位Id</param>
        /// <param name="itemId">物料id</param>
        /// <returns>产线物料货位属性值列表</returns>
        public EntityList<ItemStoragePropertyValue> GetItemStoragePropertyList(double detailId, double itemId)
        {
            return RT.Service.Resolve<StorageController>().GetItemStoragePropertyList(detailId, itemId);
        }

        /// <summary>
        /// 获取产线库存属性值
        /// </summary>
        /// <param name="detailId">物料库存Id</param>
        /// <param name="itemId">物料id</param>
        /// <returns>产线库存属性值列表</returns>
        public EntityList<StorageSaftyPropertyValue> GetStorageAreaPropertyList(double detailId, double itemId)
        {
            return RT.Service.Resolve<StorageController>().GetStorageAreaPropertyList(detailId, itemId);
        }
    }
}
