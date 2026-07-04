using SIE.Core.Common;
using SIE.Domain;
using SIE.EventMessages.RealTimeInventory;
using SIE.Inventory.Onhands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Inventory.Interfaces
{
    /// <summary>
    /// 查询实时库存信息接口
    /// </summary>
    public partial class RealTimeInventoryController : DomainController, IRealTimeInventory
    {
        /// <summary>
        /// 获取实时库存信息
        /// </summary>
        /// <param name="warehouseIds">仓库ID列表</param>
        /// <param name="itemIds">物料ID列表</param>
        /// <param name="date">日期</param>
        /// <returns>实时库存信息</returns>
        public virtual List<RealTimeInvInfo> GetRealTimeInvInfos(List<double> warehouseIds, List<double> itemIds, DateTime? date)
        {
            List<RealTimeInvInfo> invInfos = RT.Service.Resolve<InvOnhandController>().GetRealTimeInvInfos(warehouseIds, itemIds, date);
            return invInfos;
        }
    }
}
