using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.LES.StockOrders.Dao;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.StockOrders.Service
{
    /// <summary>
    /// 接受记录Server
    /// </summary>
    public class StockOrderSnService : DomainService
    {
        #region 属性 + 构造方法

        /// <summary>
        /// 月台维护数据访问
        /// </summary>
        private readonly StockOrderSnDao _StockOrderSnDao;

        /// <summary>
        /// 构造函数
        /// </summary>
        public StockOrderSnService(StockOrderSnDao stockOrderSnDao)
        {
            _StockOrderSnDao = stockOrderSnDao;
        }
        #endregion
        /// <summary>
        /// 获取备料需求明细里的序列号
        /// </summary>
        /// <param name="dId">备料需求ID</param>
        /// <param name="elo">贪婪加载选项</param>
        /// <returns></returns>
        public virtual EntityList<StockOrderSn> GetStockOrderSnByDtId(double dId, EagerLoadOptions elo)
        {
            return _StockOrderSnDao.GetStockOrderSnByDtId(dId, elo);
        }

        /// <summary>
        /// 获取备料单的接收记录数据
        /// </summary>
        /// <param name="billId">备料单ID</param>
        /// <param name="paging">分页信息</param>
        /// <returns>接收记录数据</returns>
        public virtual EntityList<StockOrderSn> GetStockOrderSns(double billId, PagingInfo paging)
        {
            return _StockOrderSnDao.GetStockOrderSns(billId, paging);
        }


        /// <summary>
        /// 获取接收记录序列号
        /// </summary>
        /// <param name="sns">序列号</param>
        /// <returns></returns>
        public virtual EntityList<StockOrderSn> GetStockOrderSnsForReturn(List<string> sns)
        {
            return _StockOrderSnDao.GetStockOrderSnsForReturn(sns);
        }


        /// <summary>
        /// 获取接收记录序列号
        /// </summary>
        /// <param name="lots">批次号</param>
        /// <returns></returns>
        public virtual EntityList<StockOrderSn> GetStockOrderSnsForReturnByLot(List<string> lots)
        {
            return _StockOrderSnDao.GetStockOrderSnsForReturnByLot(lots);
        }

        /// <summary>
        /// 获取接收记录序列号
        /// </summary>
        /// <param name="itemIds">物料</param>
        /// <returns></returns>
        public virtual EntityList<StockOrderSn> GetStockOrderSnsForReturnByItemIds(List<double> itemIds)
        {
            return _StockOrderSnDao.GetStockOrderSnsForReturnByItemIds(itemIds);
        }

        /// <summary>
        /// 根据Sn获取接收记录数据
        /// </summary>
        /// <param name="sn">物料标签</param>
        /// <param name="elo">贪婪加载选项</param>
        /// <returns>接收记录数据</returns>
        public virtual StockOrderSn GetStockOrderSnBySn(string sn, EagerLoadOptions elo)
        {
            return _StockOrderSnDao.GetStockOrderSnBySn(sn, elo);
        }

        /// <summary>
        /// 取消发货删除备料单接收记录
        /// </summary>
        public virtual void DeleteStockSnRecord(string orderNo, List<string> lineNos)
        {
            var newLineNos = lineNos.ToList();
            var DtlRecord = _StockOrderSnDao.GetStockOrderByOrder(orderNo, newLineNos);
            var receiveRecord = DtlRecord.FirstOrDefault(p => p.State == ReceiveState.Received);
            if (receiveRecord != null)
            {
                throw new ValidationException("发运单【{0}】下行号【{1}】的备料记录已接收，无法删除".L10nFormat(receiveRecord.SoNo, receiveRecord.LineNo));
            }
            var stockIds = DtlRecord.Select(p => p.StockOrderId).Distinct().ToList();
            var stockDtls = DtlRecord.Select(p => p.StockOrderDetailId).Distinct().ToList();
            //更新备料单数据
            RT.Service.Resolve<StockOrderService>().UpDateStockOrderByCancelShip(stockIds, stockDtls, DtlRecord);
            //删除备料单接收记录
            _StockOrderSnDao.DeleteStockSnByOrder(orderNo, newLineNos);
        }

        /// <summary>
        /// 获取接收记录序列号
        /// </summary>
        /// <param name="sns">序列号</param>
        /// <returns></returns>
        public virtual EntityList<StockOrderSn> GetOrderReturnSnBySn(List<string> sns)
        {
            return _StockOrderSnDao.GetOrderReturnSnBySn(sns);
        }
    }
}