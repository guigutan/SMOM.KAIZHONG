using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Warehouses.ItemStockData
{
    /// <summary>
    /// 库存资料控制器
    /// </summary>
    public class ItemStockBaseController : DomainController
    {
        /// <summary>
        /// 检查物料是否序列号管理
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <returns></returns>
        public virtual bool CheckItemIsSer(double itemId)
        {
            return Query<ItemStockDataBase>().Where(p => p.ItemId == itemId && p.IsSerialNumber == true).Count() > 0;
        }

        /// <summary>
        /// 检查物料是否批次管理
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <returns></returns>
        public virtual bool CheckItemIsBatch(double itemId)
        {
            return Query<ItemStockDataBase>().Where(p => p.ItemId == itemId && p.IsBatch == true).Count() > 0;
        }

        /// <summary>
        /// 获取库存资料
        /// </summary>
        /// <param name="itemIds">物料</param>
        /// <param name="elo">贪婪加载选项</param>
        /// <returns>库存资料</returns>
        public virtual EntityList<ItemStockDataBase> GetItemStockDataBases(List<double> itemIds, EagerLoadOptions elo= null)
        {
            return itemIds.SplitContains(item =>
             {
                 return Query<ItemStockDataBase>().Where(p => item.Contains(p.ItemId)).ToList(null, elo);
             });
        }
    }
}
