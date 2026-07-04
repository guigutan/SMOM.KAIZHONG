using SIE.Domain;
using SIE.Inventory.Commom;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 批次比较数据
    /// </summary>
    public class LotComparer : IComparer<string>
    {
        /// <summary>
        /// 批次信息
        /// </summary>
        public EntityList<Lot> LotData { get; set; }

        /// <summary>
        /// 比较数据
        /// </summary>
        /// <param name="x">批次编号</param>
        /// <param name="y">批次编号</param>
        /// <returns></returns>
        public int Compare(string x, string y)
        {
            Lot xLot = LotData.FirstOrDefault(p => p.Code == x);
            Lot yLot = LotData.FirstOrDefault(p => p.Code == x);
            if (xLot == null) return 1;
            if (yLot == null) return -1;
            if (xLot.LotAtt03 > yLot.LotAtt03)
            {
                return 1;
            }
            return xLot.LotAtt03 == yLot.LotAtt03 ? 0 : -1;
        }
    }
}
