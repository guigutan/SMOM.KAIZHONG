using SIE.Core.Common.Dao;
using SIE.Domain;
using SIE.Items;
using SIE.LES.MaterialReceptions.APIModels;
using SIE.LES.StockOrders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.LES.MaterialReceptions.Dao
{
    /// <summary>
    /// 接收明细
    /// </summary>
    public class StockOrderSnDao : BaseDao<StockOrderSn>
    {
        /// <summary>
        /// 获取接收明细
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public virtual EntityList<StockOrderSn> GetStockOrderSns(List<double> Ids)
        {
            return Ids.SplitContains(temps =>
            {
                return DB.Query<StockOrderSn>().Where(m => temps.Contains(m.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }


        /// <summary>
        /// 根据关键编码获取接收记录
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="scanObjectType">扫描到物体的类型 1:物料编码 2：批次号 3：SN序列号</param>
        /// <returns></returns>
        public virtual List<StockOrderSn> GetStockOrderByItemCode(string keyWord, out ObjectType scanObjectType)
        {
            //扫描物料标签 要求SN为空 批次号为空

            //var res = Query().Where(m => m.State == ReceiveState.TobeReceived).
            //    Join<Item>((m, n) => n.Code == keyWord && m.Sn == "" && m.LotNo == "" && m.ItemId == n.Id).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //if (res.Any())
            //{
            //    scanObjectType = ObjectType.ItemCode;
            //    return res.ToList();
            //}
            ////扫描批次号 要求SN为空 批次号不为空
            //var resLot = Query().Where(m => m.State == ReceiveState.TobeReceived)
            //    .Where(m =>m.Sn == "" && m.LotNo == keyWord).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //if (resLot.Any())
            //{
            //    scanObjectType = ObjectType.Lot;
            //    return resLot.ToList();
            //}
            var resSn = Query().Where(m => m.State == ReceiveState.TobeReceived&&(m.ShipQty-m.Qty)>0)
                .Where(m => m.Sn == keyWord)
              
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (resSn.Any())
            {
                scanObjectType = ObjectType.SN;
                return resSn.ToList();
            }

            scanObjectType = 0;
            return new List<StockOrderSn>();
        }

        /// <summary>
        /// 根据关键词在指定备料单获取数据
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="stockOrderNo"></param>
        /// <param name="scanObjectType"></param>
        /// <returns></returns>

        public virtual List<StockOrderSn> GetStockOrderByKeAndNo(string keyWord, string stockOrderNo, out ObjectType scanObjectType)
        {
            //扫描物料标签 要求SN为空 批次号为空
            //var res = Query().Where(m => (m.State == ReceiveState.TobeReceived) && m.StockOrder.No == stockOrderNo).
            //    Where(m => m.Item.Code == keyWord && m.Sn.IsNullOrEmpty() && m.LotNo.IsNullOrEmpty()).ToList();
            //if (res != null)
            //{
            //    scanObjectType = ObjectType.ItemCode;
            //    return res.ToList();
            //}
            ////扫描批次号 要求SN为空 物料标签为空
            //var resLot = Query().Where(m => (m.State == ReceiveState.TobeReceived) && m.StockOrder.No == stockOrderNo)
            //    .Where(m => m.ItemId <= 0 && m.Sn.IsNullOrEmpty() && m.LotNo == keyWord).ToList();
            //if (resLot != null)
            //{
            //    scanObjectType = ObjectType.Lot;
            //    return resLot.ToList();
            //}
            var resSn = Query().Where(m => m.State == ReceiveState.TobeReceived&& m.StockOrder.No == stockOrderNo)
                .Where(m => m.Sn == keyWord).ToList();
            if (resSn != null)
            {
                scanObjectType = ObjectType.SN;
                return resSn.ToList();
            }
            scanObjectType = 0;
            return new List<StockOrderSn>();
        }


        /// <summary>
        /// 根据关键词获取备料单
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public virtual EntityList<StockOrder> GetStockOrderByKeyWord(string keyWord)
        {
            var stockOrderList = DB.Query<StockOrder>().Where(m => m.No == keyWord).Distinct().ToList(null, new EagerLoadOptions().LoadWith(StockOrder.StockOrderDetailListProperty).LoadWithViewProperty());
            if (!stockOrderList.Any())
            {
                stockOrderList = DB.Query<StockOrder>().Join<StockOrderSn>((x, y) => x.Id == y.StockOrderId && (y.SoNo == keyWord||y.DistributionNo == keyWord))
                    .Distinct().ToList(null, new EagerLoadOptions().LoadWith(StockOrder.StockOrderDetailListProperty).LoadWithViewProperty());
                if (stockOrderList.Any())
                {
                    return stockOrderList;
                }
            }
            else
            {
                return stockOrderList;
            }
            return new EntityList<StockOrder>();
        }

        /// <summary>
        /// 根据单号获取备料单及其接收记录
        /// </summary>
        /// <param name="stockOrderNo"></param>
        /// <returns></returns>
        public virtual Tuple<StockOrder, EntityList<StockOrderSn>> GetStockOrder(string stockOrderNo)
        {
            var order = DB.Query<StockOrder>().Where(m => m.No == stockOrderNo).FirstOrDefault(new EagerLoadOptions()
                  .LoadWith(StockOrder.StockOrderDetailListProperty)
                  .LoadWithViewProperty()
                  );
            if (order != null)
            {
                var stockOrderSnList = DB.Query<StockOrderSn>().Where(m => m.StockOrderId == order.Id).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                return new Tuple<StockOrder, EntityList<StockOrderSn>>(order, stockOrderSnList);

            }
            return null;
        }
    }
}
