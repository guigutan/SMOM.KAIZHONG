using SIE.Domain;
using SIE.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.Interfaces.ApsTasks
{
    /// <summary>
    /// 物料数据拥有者
    /// </summary>
    public class ItemDataOwner
    {
        /// <summary>
        /// BOM的物料
        /// </summary>
        private readonly EntityList<Item> items = new EntityList<Item>();

        /// <summary>
        /// 获取物料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Item GetItem(double id)
        {
            return items.FirstOrDefault(x => x.Id == id);
        }
        /// <summary>
        /// 获取物料
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Item GetItemByCode(string code)
        {
            return items.FirstOrDefault(x => x.Code == code);
        }

        /// <summary>
        /// 获取物料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ExistsItem(double id)
        {
            return items.Any(x => x.Id == id);
        }

        /// <summary>
        /// 获取物料
        /// </summary>
        /// <param name="itemIds"></param>
        public void GetItemsAndCache(List<double> itemIds)
        {
            if (items.Any())
            {
                var existsItemIds = items.Select(x => x.Id).ToList();
                itemIds = itemIds.Except(existsItemIds).ToList();
            }

            if (itemIds.Any())
            {
                items.AddRange(RT.Service.Resolve<ItemController>().GetItemListNoViewProperty(itemIds));
            }            
        }

        /// <summary>
        /// 根据物料编码获取物料
        /// </summary>
        /// <param name="itemCodes"></param>
        public void GetItemsAndCacheByCode(List<string> itemCodes)
        {
            if (items.Any())
            {
                var existsCodes = items.Select(x => x.Code).ToList();
                itemCodes = itemCodes.Except(existsCodes).ToList();
            }

            if (itemCodes.Any())
            {
                items.AddRange(RT.Service.Resolve<ItemController>().GetItemListByCodeNoViewProperty(itemCodes));
            }
        }

    }
}
