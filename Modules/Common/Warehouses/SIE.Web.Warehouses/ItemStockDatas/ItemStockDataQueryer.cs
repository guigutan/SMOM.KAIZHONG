using SIE.Warehouses.ItemStockData;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Warehouses.ItemStockDatas
{
    /// <summary>
    /// 库存资料
    /// </summary>
    public class ItemStockDataQueryer : Data.DataQueryer
    {
        /// <summary>
        /// 检查是否序列号管理
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public bool CheckIsSer(double itemId)
        { 
           return RT.Service.Resolve<ItemStockBaseController>().CheckItemIsSer(itemId);
        }
    }
}
