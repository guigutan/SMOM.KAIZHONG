using NPOI.XSSF.UserModel;
using SIE.Api;
using SIE.Common.Sender;
using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.EventMessages.Shipment;
using SIE.LES.BulletinBoards.MaterialPulls.APIModels;
using SIE.LES.StockOrders;
using SIE.LES.StockPlans;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.LES.BulletinBoards.MaterialPulls.Services
{
    /// <summary>
    /// 物料拉动看板（生产）服务
    /// </summary>
    public class MaterialPullWipService : DomainService
    {
        /// <summary>
        /// 获取物料拉通(生产)看板基本数据
        /// </summary>
        /// <param name="workShopIds"></param>
        /// <param name="resourceIds"></param>
        /// <returns></returns>
        public virtual List<MaterialPullWipInfo> GetMaterialPullWipInfos(List<double?> workShopIds, List<double?> resourceIds)
        {
            var materialPullWipList = new List<MaterialPullWipInfo>();
            // 备料单主表
            EntityList<StockOrder> stockOrderList = DB.Query<StockOrder>()
                .Where(p => workShopIds.Contains(p.WorkShopId) && resourceIds.Contains(p.ResourceId)
                && (p.StockState == StockState.PickStocking || p.StockState == StockState.TobeReceive || p.StockState == StockState.Issued)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var stockOrderIds = stockOrderList.Select(p => p.Id).ToList();
            // 物料需求明细
            var allStockDetail = stockOrderIds.SplitContains(tempIds =>
            {
                return DB.Query<StockOrderDetail>().Where(p => tempIds.Contains(p.StockOrderId) &&
                (p.StockState == StockState.PickStocking || p.StockState == StockState.TobeReceive || p.StockState == StockState.Issued)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            // 接收记录明细
            var allStockSn = stockOrderIds.SplitContains(tempIds =>
            {
                return DB.Query<StockOrderSn>().Where(p => tempIds.Contains(p.StockOrderId))
                .Where(p => p.State == ReceiveState.TobeReceived || p.State == ReceiveState.Received).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            // 发运单号列表
            var noList = allStockSn.Select(p => p.SoNo).ToList();
            var noWareDic = RT.Service.Resolve<IMaterialPull>().GetShippingWare(noList);
            // 符合车间资源的备料单
            stockOrderList.ForEach(stock =>
            {
                var stockId = stock.Id;
                // 备料单物料需求明细
                var stockDetail = allStockDetail.Where(p => p.StockOrderId == stockId).ToList();
                stockDetail.ForEach(detail =>
                {
                    MaterialPullWipInfo materialPullWipInfo = new MaterialPullWipInfo
                    {
                        No = stock.No,
                        NoLine = stock.No + '-' + detail.LineNo,
                        WoNo = stock.WoNo,
                        ResourceName = stock.ResourceName,
                        ItemName = detail.ItemName,
                        NeedQty = detail.Qty,
                        UnitName = detail.ItemUnit,
                        Time = detail.DemandTime.ToString(),
                    };
                    var detailId = detail.Id;
                    var stockSn = allStockSn.Where(p => p.StockOrderDetailId == detailId).ToList();
                    var goodList = CreateGroupByWare(stockSn, noWareDic);
                    materialPullWipInfo.GoodWareInfo = goodList;
                    materialPullWipList.Add(materialPullWipInfo);
                });
            });
            // 需求时间升序，备料单号升序
            var materialPullWipOrderList = materialPullWipList.OrderBy(p => p.Time).ThenBy(p => p.No).ToList();
            materialPullWipList = materialPullWipOrderList;
            return materialPullWipList;
        }

        /// <summary>
        /// 发货仓库相关信息
        /// </summary>
        /// <param name="stockSn">当前备料单接收记录</param>
        /// <param name="noWareDic">发运单发货仓库字典</param>
        /// <returns></returns>
        private List<GoodWare> CreateGroupByWare(List<StockOrderSn> stockSn, Dictionary<string, string> noWareDic)
        {
            List<TempDeliverInfo> tempDeliverInfos = new List<TempDeliverInfo>();
            List<GoodWare> goodWares = new List<GoodWare>();
            // 发运订单
            var snGroupBySo = stockSn.GroupBy(p => p.SoNo).ToList();
            snGroupBySo.ForEach(sn =>
            {
                var ware = noWareDic.ContainsKey(sn.Key)?noWareDic[sn.Key]:string.Empty;
                TempDeliverInfo tempDeliverInfo = new TempDeliverInfo()
                {
                    Ware = ware,
                    Send = sn.Sum(p => p.ShipQty),
                    Receive = sn.Sum(p => p.Qty),
                    NeedSend = sn.Where(p => p.DistributionNo.IsNotEmpty()).Sum(p => p.ShipQty),
                    NeedPick = sn.Where(p => p.DistributionNo.IsNullOrEmpty()).Sum(p => p.ShipQty),
                };
                tempDeliverInfos.Add(tempDeliverInfo);
            });
            var tempDeliverGroup = tempDeliverInfos.GroupBy(p => p.Ware).ToList();
            tempDeliverGroup.ForEach(temp =>
            {
                GoodWare goodWare = new GoodWare
                {
                    Ware = temp.Key,
                    Received = temp.Sum(p => p.Receive),
                    ToReceive = temp.Sum(p => p.Send) - temp.Sum(p => p.Receive),
                    Send = temp.Sum(p => p.NeedSend),
                    Pick = temp.Sum(p => p.NeedPick),
                };
                goodWares.Add(goodWare);
            });
            return goodWares;
        }
    }
}
