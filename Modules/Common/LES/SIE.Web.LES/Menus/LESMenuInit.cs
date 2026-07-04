using SIE.Common.Menus;
using SIE.LES;
using SIE.LES.LesStockCounts;
using SIE.LES.LinesideWarehouses;
using SIE.LES.MaterialMoves;
using SIE.LES.MaterialPreparations;
using SIE.LES.MaterialPreparations.ViewModels;
using SIE.LES.MaterialReceives;
using SIE.LES.MaterialReturnApplys;
using SIE.LES.Reports;
using SIE.LES.StockOrders;
using SIE.LES.StockPlans;
using System.Collections.Generic;

namespace SIE.Web.LES
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class LESMenuInit : IWebMenuInit
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            var res = new List<MenuDto>();
            const string parName = "WMS";
            //res.Add(new MenuDto()
            //{
            //    TreeKey = parName,
            //    Label = "备料单",
            //    EntityType = typeof(StockOrder),
            //});
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "备料需求单",
                EntityType = typeof(MaterialPreparation),
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "工单备料汇总",
                EntityType = typeof(WorkOrderMpViewModel),
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "退料申请",
                EntityType = typeof(MaterialReturnApply),
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "工单挪料记录",
                EntityType = typeof(MaterialMoveRecord),
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "备料模式维护-拉式",
                EntityType = typeof(PrepareItemPull),
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "备料模式维护-推式",
                EntityType = typeof(PrepareItemPush),
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "产线线边仓维护",
                EntityType = typeof(LinesideWarehouse),
            });
            //res.Add(new MenuDto()
            //{
            //    TreeKey = parName,
            //    Label = "备料计划",
            //    EntityType = typeof(StockPlan),
            //});
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "物料接收",
                EntityType = typeof(MaterialReceive)
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "物料接收记录",
                EntityType = typeof(MaterialReceiveRecord)
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "工单需求汇总报表",
                EntityType = typeof(WoDemandReport)
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "线边仓盘点",
                EntityType = typeof(LesStockCount)
            });            
            //res.Add(new MenuDto()
            //{
            //    TreeKey = parName,
            //    Label = "备料单合并下发规则",
            //    EntityType = typeof(StockOrderMergeIssued)
            //});
            return res;
        }

    }
}
