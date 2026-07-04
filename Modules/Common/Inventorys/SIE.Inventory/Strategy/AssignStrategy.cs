using SIE.Inventory.Onhands;
using System.Collections.Generic;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 分配策略
    /// 抽象策略角色
    /// </summary>
    public abstract class AssignStrategy
    {
        /// <summary>
        /// 获取分配库存
        /// </summary>
        /// <returns></returns>
        public abstract List<LotLpnOnhand> GetAssignLotLpnOnhand(List<AssignParam> paramList);
    }
}
