using SIE.Common.Menus;
using SIE.Inventory.Onhands;
using SIE.Inventory.Strategy;
using SIE.Inventory.Task;
using SIE.Inventory.TransactionProcessing;
using SIE.Inventory.Transactions;
using System.Collections.Generic;

namespace SIE.Web.Inventory.Menus
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class InventoryMenuInit : IWebMenuInit
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            var res = new List<MenuDto>();
            const string parName = "WMS.基础资料";
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "上架规则",
                EntityType = typeof(OnShelvesRule),
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "批次和LPN库存",
                EntityType = typeof(LotLpnOnhand),
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "批次库存",
                EntityType = typeof(LotOnhand),
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "库位库存",
                EntityType = typeof(LocationOnhand),
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "分配规则",
                EntityType = typeof(AssignRule),
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "周转规则",
                EntityType = typeof(TurnOverRule),
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "单据大类",
                EntityType = typeof(Function),
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "单据小类",
                EntityType = typeof(Transaction),
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "任务组管理",
                EntityType = typeof(TaskGroup),
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "任务管理",
                EntityType = typeof(TaskManagement),
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "事务交易",
                EntityType = typeof(InvTransaction),
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "垛表",
                EntityType = typeof(SIE.Inventory.Piles.Pile),
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "任务分配规则",
                EntityType = typeof(TaskAllotRule),
            });            
            return res;
        }

    }
}
