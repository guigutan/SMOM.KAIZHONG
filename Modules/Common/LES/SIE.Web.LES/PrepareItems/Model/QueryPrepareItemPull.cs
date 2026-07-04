using SIE.Warehouses;

namespace SIE.Web.LES.PrepareItems.Model
{
    /// <summary>
    /// 赋值类
    /// </summary>
    public class QueryPrepareItemPull : Data.DataQueryer
    {
        /// <summary>
        /// 获取最高库量
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="itemId">物料ID</param>
        public object QueryMaxStock(double warehouseId, double itemId)
        {
            var itemExt = RT.Service.Resolve<BaseItemExtController>().GetItemIOLimit(itemId,warehouseId);
            decimal qty = 0;
            if(itemExt != null)
            {
                qty = itemExt.MaxStockQty.Value;
            }
            return qty;
        }
    }
}
