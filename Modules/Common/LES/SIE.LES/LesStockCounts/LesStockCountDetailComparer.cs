using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.LesStockCounts
{
    /// <summary>
    /// 线边仓盘点明细比较器
    /// </summary>
    public class LesStockCountDetailComparer : IEqualityComparer<LesStockCountDetail>
    {
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="x">库存盘点明细1</param>
        /// <param name="y">库存盘点明细2</param>
        /// <returns>bool</returns>
        public bool Equals(LesStockCountDetail x, LesStockCountDetail y)
        {
            if (x == null || y == null)
                return false;
            if (x.ItemId == y.ItemId && x.WarehouseId == y.WarehouseId && x.StorageLocationId == y.StorageLocationId
                && x.LabelNo == y.LabelNo && x.LotId == y.LotId && x.OnhandState == y.OnhandState && x.ItemExtProp == y.ItemExtProp)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <param name="obj">库存盘点明细</param>
        /// <returns></returns>
        public int GetHashCode(LesStockCountDetail obj)
        {
            if (obj == null)
                return 0;
            else
            {
                var item = obj.ItemId;
                var labelNo = obj.LabelNo == null ? string.Empty : obj.LabelNo;
                var location = obj.StorageLocationId.HasValue ? obj.StorageLocationId.Value : 0;
                var lot = obj.LotId.HasValue ? obj.LotId.Value : 0;
                var itemExtProp = obj.ItemExtProp == null ? string.Empty : obj.ItemExtProp;
                var Factory = obj.FactoryId.HasValue ? obj.FactoryId.Value : 0;
                var onhandState = obj.OnhandState.HasValue ? obj.OnhandState.Value : 0;
                return item.GetHashCode() ^ obj.WarehouseId.GetHashCode() ^ location.GetHashCode() ^ labelNo.GetHashCode() ^
                       lot.GetHashCode() ^ obj.OnhandState.GetHashCode() ^ itemExtProp.GetHashCode() ^ Factory.GetHashCode() ^ onhandState.GetHashCode();
            }
        }
    }
}
