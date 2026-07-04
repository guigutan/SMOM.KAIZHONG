using SIE.Core.Common.Dao;
using SIE.Domain;
using SIE.Items;
using SIE.Warehouses;
using System.Linq;

namespace SIE.LES.MaterialReceptions.Dao
{
    /// <summary>
    /// 数量属性持久层
    /// </summary>
    public class ItemStockDataBaseDao : BaseDao<ItemStockDataBase>
    {
        /// <summary>
        /// 获取物料扩展属性
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <returns></returns>
        public virtual ItemStockDataBase GetItemStockDataBase(double itemId)
        {
            return Query().Where(m => m.ItemId == itemId).FirstOrDefault();
        }

        /// <summary>
        /// 获取物料基本属性
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public virtual Item GetItemBaseData(double itemId)
        {
            return DB.Query<Item>().Where(m => m.Id == itemId).FirstOrDefault();
        }
    }

}
