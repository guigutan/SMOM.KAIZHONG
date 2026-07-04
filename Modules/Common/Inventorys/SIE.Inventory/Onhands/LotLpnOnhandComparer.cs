using System.Collections.Generic;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 批次库存比较数据
    /// </summary>
    public class LotLpnOnhandComparer : IEqualityComparer<LotLpnOnhand>
    {
        /// <summary>
        /// 批次库存比较
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(LotLpnOnhand x, LotLpnOnhand y)
        {
            if (x == null || y == null)
                return false;
            if (x.Lpn == y.Lpn && x.StorageLocationId == y.StorageLocationId)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获取批次库存哈希编码
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(LotLpnOnhand obj)
        {
            if (obj == null)
                return 0;
            else
                return obj.Lpn.GetHashCode() ^ obj.StorageLocationId.GetHashCode();
        }
    }
}
