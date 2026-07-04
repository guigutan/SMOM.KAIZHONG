using SIE.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 分配匹配库存
    /// </summary>
    public class AssignParamMatch : DomainController
    {
        /// <summary>
        /// 分配库存匹配(第一次匹配)
        /// </summary>
        /// <param name="paparm">分配参数</param>
        /// <param name="preSelLotLpnOnhand">库存</param>
        /// <returns>匹配的库存</returns>
        public virtual List<LotLpnOnhand> AssignMatchFunction(AssignParam paparm, List<LotLpnOnhand> preSelLotLpnOnhand)
        {
            //20241104确认 供应商退货只能分配不合格库存，只能分配合格的库存    
            return preSelLotLpnOnhand.Where(d => d.ItemId == paparm.ItemId && d.WarehouseId == paparm.WarehouseId
                          //&& (paparm.OrderType == OrderType.SupplierReturn && d.State == OnhandState.Ng || paparm.OrderType != OrderType.SupplierReturn && d.State == OnhandState.Ok)通过分配规则明细匹配
                          && paparm.StorerCode == d.StorerCode && (paparm.TaskNo == d.TaskNo) && (paparm.ProjectNo == d.ProjectNo)
                          && (paparm.InvItemExtProp == d.ItemExtProp || paparm.IsIgnoreItemExtProp)
                          //匹配指定项
                          && (paparm.AppointStorageAreaId == null || paparm.AppointStorageAreaId == d.StorageAreaId)
                          && (paparm.AppointStorageLocationId == null || paparm.AppointStorageLocationId == d.StorageLocationId)
                          && (paparm.AppointLotId == null || paparm.AppointLotId == d.LotId)
                          && (paparm.AppointLpn.IsNullOrEmpty() || paparm.AppointLpn == d.Lpn)
                          )
                         .ToList();
        }

        /// <summary>
        /// 分配库存匹配(第二次匹配)
        /// </summary>
        /// <param name="paparm">分配参数</param>
        /// <param name="preSelLotLpnOnhand">库存</param>
        /// <param name="projectNo">项目号</param>
        /// <param name="taskNo">任务号</param>
        /// <returns>匹配的库存</returns>
        public virtual List<LotLpnOnhand> AssignMatchFunction(AssignParam paparm, List<LotLpnOnhand> preSelLotLpnOnhand, string taskNo, string projectNo)
        {
            //加入了taskNo、projectNo参数，匹配只有一个条件满足的库存
            return preSelLotLpnOnhand.Where(d => d.ItemId == paparm.ItemId && d.WarehouseId == paparm.WarehouseId
                            //&& (paparm.OrderType == OrderType.SupplierReturn && d.State == OnhandState.Ng || paparm.OrderType != OrderType.SupplierReturn && d.State == OnhandState.Ok)通过分配规则明细匹配
                            && paparm.StorerCode == d.StorerCode && (paparm.TaskNo == d.TaskNo || d.TaskNo == taskNo) && (paparm.ProjectNo == d.ProjectNo || d.ProjectNo == projectNo)
                          && (paparm.InvItemExtProp == d.ItemExtProp || paparm.IsIgnoreItemExtProp)
                          //匹配指定项
                          && (paparm.AppointStorageAreaId == null || paparm.AppointStorageAreaId == d.StorageAreaId)
                          && (paparm.AppointStorageLocationId == null || paparm.AppointStorageLocationId == d.StorageLocationId)
                          && (paparm.AppointLotId == null || paparm.AppointLotId == d.LotId)
                          && (paparm.AppointLpn.IsNullOrEmpty() || paparm.AppointLpn == d.Lpn)
                          )
                         .ToList();
        }
    }
}
