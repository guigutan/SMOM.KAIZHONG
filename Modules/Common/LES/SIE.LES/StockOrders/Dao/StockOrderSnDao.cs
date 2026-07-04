using SIE.Core.Common;
using SIE.Core.Common.Dao;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.LES.StockOrders.Dao
{
    /// <summary>
    /// 接受标签DAO
    /// </summary>
    public class StockOrderSnDao : BaseDao<StockOrderSn>
    {
        /// <summary>
        /// 获取备料需求明细里的序列号
        /// </summary>
        /// <param name="dId">备料需求ID</param>
        /// <param name="elo">贪婪加载选项</param>
        /// <returns></returns>
        public virtual EntityList<StockOrderSn> GetStockOrderSnByDtId(double dId, EagerLoadOptions elo)
        {
            return Query().Where(p => p.StockOrderDetailId == dId).ToList(null, elo);
        }

        /// <summary>
        /// 获取备料单的接收记录数据
        /// </summary>
        /// <param name="billId">备料单ID</param>
        /// <param name="paging">分页信息</param>
        /// <returns>接收记录数据</returns>
        public virtual EntityList<StockOrderSn> GetStockOrderSns(double billId, PagingInfo paging)
        {
            return Query().Where(p => p.StockOrderId == billId).ToList(paging, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取接收记录序列号
        /// </summary>
        /// <param name="sns">序列号</param>
        /// <returns></returns>
        public virtual EntityList<StockOrderSn> GetStockOrderSnsForReturn(List<string> sns)
        {
            List<StockState> states = new List<StockState>() {
                 StockState.Audit,
              StockState.Created,
              StockState.Closed,
               StockState.ReCall,
            };
            return sns.SplitContains(sons =>
            {
                var query = Query().Where(p => !states.Contains(p.StockOrder.StockState))
                    .Where(p => sons.Contains(p.Sn));
                return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取接收记录序列号
        /// </summary>
        /// <param name="lots">批次号</param>
        /// <returns></returns>
        public virtual EntityList<StockOrderSn> GetStockOrderSnsForReturnByLot(List<string> lots)
        {
            List<StockState> states = new List<StockState>() {
                 StockState.Audit,
              StockState.Created,
              StockState.Closed,
               StockState.ReCall
            };
            return lots.SplitContains(lot =>
            {
                var query = Query().Where(p => !states.Contains(p.StockOrder.StockState))
                    .Where(p => lot.Contains(p.LotNo));
                return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取接收记录序列号
        /// </summary>
        /// <param name="itemIds">物料</param>
        /// <returns></returns>
        public virtual EntityList<StockOrderSn> GetStockOrderSnsForReturnByItemIds(List<double> itemIds)
        {
            List<StockState> states = new List<StockState>() {
                 StockState.Audit,
              StockState.Created,
              StockState.Closed,
               StockState.ReCall
            };
            return itemIds.SplitContains(item =>
            {
                var query = Query().Where(p => !states.Contains(p.StockOrder.StockState))
                    .Where(p => item.Contains(p.ItemId));
                return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 根据Sn获取接收记录数据
        /// </summary>
        /// <param name="sn">物料标签</param>
        /// <param name="elo">贪婪加载选项</param>
        /// <returns>接收记录数据</returns>
        public virtual StockOrderSn GetStockOrderSnBySn(string sn, EagerLoadOptions elo)
        {
            return Query().Where(p => p.Sn == sn && p.State == ReceiveState.TobeReceived).FirstOrDefault(elo);
        }

        /// <summary>
        /// 根据发运单号和行号获取已接收的备料接收记录
        /// </summary>
        /// <param name="orderNo">发运单号</param>
        /// <param name="lineNos">行号</param>
        /// <returns></returns>
        public virtual EntityList<StockOrderSn> GetStockOrderByOrder(string orderNo, List<string> lineNos, EagerLoadOptions elo = null)
        {
            return lineNos.SplitContains(sons =>
            {
                var query = Query().Where(p => p.SoNo == orderNo && lineNos.Contains(p.SoLineNo));
                return query.ToList(null, elo);
            });
        }

        /// <summary>
        /// 根据单号和明细删除备料记录
        /// </summary>
        /// <param name="orderNo">发运单号</param>
        /// <param name="lineNos">行号</param>
        public virtual void DeleteStockSnByOrder(string orderNo, List<string> lineNos)
        {
            var newlineNos = lineNos.Distinct().ToList();
            if (newlineNos.Count == 0)
            {
                return;
            }
            DB.Delete<StockOrderSn>().Where(p => p.SoNo == orderNo && lineNos.Contains(p.SoLineNo)).Execute();
        }

        /// <summary>
        /// 获取接收记录序列号
        /// </summary>
        /// <param name="sns">序列号</param>
        /// <returns></returns>
        public virtual EntityList<StockOrderSn> GetOrderReturnSnBySn(List<string> sns)
        {
            //这个list包含的内容可能要匹配批次号 可能要匹配物料编码 可能要匹配标签号
            return sns.SplitContains(sons =>
            {
                var query = Query().Where(p => sons.Contains(p.Sn) && p.State == ReceiveState.Received);
                return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }
    }
}
