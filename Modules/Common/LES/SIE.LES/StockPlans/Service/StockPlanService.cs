using SIE.Core.Common.Service;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages;
using SIE.EventMessages.WMS.Receipt;
using SIE.Items;
using SIE.LES.Interfaces.Datas;
using SIE.LES.StockOrders;
using SIE.LES.StockOrders.Service;
using SIE.LES.StockPlans.Dao;
using SIE.LES.StockPlans.ViewModels;
using SIE.ShipPlan;
using SIE.Warehouses;
using SIE.Warehouses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.LES.StockPlans.Service
{
    /// <summary>
    /// 备料计划Service
    /// </summary>
    public class StockPlanService : DomainService
    {
        #region 属性 + 构造方法

        /// <summary>
        /// 月台维护数据访问
        /// </summary>
        private readonly StockPlansDao _stockPlansDao;

        /// <summary>
        /// 构造函数
        /// </summary>
        public StockPlanService(StockPlansDao stockPlansDao)
        {
            _stockPlansDao = stockPlansDao;
        }


        #endregion

        /// <summary>
        /// 备料单查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<StockPlan> GetStockPlans(StockPlanCriteria criteria)
        {
            return _stockPlansDao.GetStockPlans(criteria);
        }

        /// <summary>
        /// 获取备料计划
        /// </summary>
        /// <param name="nos">单号</param>
        /// <returns>备料计划</returns>
        public virtual EntityList<StockPlan> GetStockPlans(List<string> nos)
        {
            return _stockPlansDao.GetStockPlans(nos);
        }

        /// <summary>
        /// 获取备料计划
        /// </summary>
        /// <param name="no">计划号</param>
        /// <param name="elo">贪婪</param>
        /// <returns>备料计划</returns>
        public virtual StockPlan GetStockPlan(string no, EagerLoadOptions elo = null)
        {
            return _stockPlansDao.GetStockPlan(no, elo);
        }

        #region 备料计划齐套分析

        #endregion


        /// <summary>
        /// 发货计划强制完成操作
        /// </summary>
        /// <param name="planIds">发货计划ID集合</param>
        public virtual void ForceCompleteDeliveryPlans(List<double> planIds)
        {
            var plans = _stockPlansDao.GetStockPlansByIds(planIds);
            if (plans.Any(p => p.State != DeliveryState.Aduited && p.State != DeliveryState.Executing))
            {
                throw new ValidationException("发货计划必须是审核、执行中状态,才能进行强制完成操作!".L10N());
            }
            using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                var flag = false;
                plans.GroupBy(f => new { f.No }).ForEach(f =>
                   {
                       f.ForEach(p =>
                       {
                           //本次取消数
                           var nowCreateQty = p.NoCreateQty;
                           p.CancelQty += p.NoCreateQty;
                           p.NoCreateQty = 0;

                           if (p.CancelQty == p.RequireQty)
                           {
                               p.State = DeliveryState.Cancel;
                               if (p.SourceType == DeliverySourceType.External && p.OrderType == OrderType.WorkFeed)
                               {
                                   flag = true;
                                   RT.Service.Resolve<StockOrderDetailService>().UpdateDtlStateByPlans(p.No, p.LineNo.ToString(), StockState.Closed);
                               }
                           }

                           if (p.CancelQty < p.RequireQty)
                           {
                               p.State = DeliveryState.Finished;
                           }
                           p.CreateQty = 0;
                           if (p.IsMergeIssued)
                           {

                               UpdateMergeDtlCancelQty(p.OrderNo, nowCreateQty);
                           }
                           else
                           {
                               RT.Service.Resolve<StockOrderDetailService>().UpdateDtlCancelQtyByPlans(p.No, p.LineNo.ToString(), nowCreateQty);
                           }
                       });

                       var pl = f.First();
                       if (pl.IsMergeIssued)
                       {
                           UpdateMergeOrderState(pl.OrderNo);
                       }
                       else
                       {
                           var stock = RT.Service.Resolve<StockOrderService>().GetStockOrdersByNo(f.Key.No, null);
                           RT.Service.Resolve<StockOrderService>().UpdateStockOrderState(stock, stock.StockOrderDetailList);
                           RF.Save(stock);
                       }

                   });
                RF.Save(plans);
                tran.Complete();
            }
        }


        private void UpdateMergeDtlCancelQty(string orderNo, decimal qty)
        {
            var query = DB.Query<StockOrderDetail>("d1").Join<StockOrder>("s1", (x, y) => y.Id == x.StockOrderId);
            StringBuilder sql = new StringBuilder();
            var arrs = orderNo.Split(';');
            for (int i = 0; i < arrs.Length; i++)
            {
                string[] arr = arrs[i].Split(':');
                sql.Append(string.Format("s1.No = '{0}' and d1.Line_No = '{1}'", arr[0], arr[1]));
                if (i < arrs.Length - 1)
                {
                    sql.Append(" or ");
                }
            }
            var dtls = query.Where(p => p.SQL<bool>(sql.ToString())).ToList();
            dtls.ForEach(p =>
            {
                if (qty <= 0)
                    return;
                var canQty = p.Qty - p.ReceiveQty;
                p.CancelQty = qty >= canQty ? canQty : qty;
                qty -= canQty;
                RT.Service.Resolve<StockOrderDetailService>().UpdateDtlCancelStateByPlans(p);
            });
            RF.Save(dtls);
        }

        private void UpdateMergeOrderState(string orderNo)
        {
            var query = DB.Query<StockOrder>("s1");
            StringBuilder sql = new StringBuilder();
            var arrs = orderNo.Split(';');
            for (int i = 0; i < arrs.Length; i++)
            {
                string[] arr = arrs[i].Split(':');
                sql.Append(string.Format("s1.No = '{0}'", arr[0]));
                if (i < arrs.Length - 1)
                {
                    sql.Append(" or ");
                }
            }
            var stockOrders = query.Where(p => p.SQL<bool>(sql.ToString())).ToList();
            stockOrders.ForEach(stock =>
            {
                RT.Service.Resolve<StockOrderService>().UpdateStockOrderState(stock, stock.StockOrderDetailList);
                RF.Save(stock);
            });
        }


        /// <summary>
        /// 根据备料更新备料单明细状态
        /// </summary>
        /// <param name="planIds"></param>
        public virtual void UpdateStockDtlState(List<double> planIds)
        {
            var stockPlans = _stockPlansDao.GetStockPlansByIds(planIds);
            List<string> stockNos = new List<string>();
            var plans = stockPlans.Where(p => p.SourceType == DeliverySourceType.External && p.OrderType == OrderType.WorkFeed);
            plans.GroupBy(f => f.No).ForEach(f =>
                 {
                     f.Where(a => a.IsMergeIssued).ForEach(p =>
                     { //单据明细更新为拣配中    ,合并的单号需要进行拆分数据，返回备料单号                                               
                         var nos = RT.Service.Resolve<StockOrderDetailService>().UpdateDtlStateByMergeNo(p.OrderNo, StockState.PickStocking);
                         stockNos.AddRange(nos);
                     });
                     f.Where(a => !a.IsMergeIssued).ForEach(p =>
                     { //单据明细更新为拣配中，非合并的单，相关单号就是备料单号
                         RT.Service.Resolve<StockOrderDetailService>().UpdateDtlStateByPlans(p.OrderNo, p.LineNo.ToString(), StockState.PickStocking);
                         stockNos.Add(p.OrderNo);
                     });
                 });

            var stocks = RT.Service.Resolve<StockOrderService>().GetStockOrderByNos(stockNos);
            stockNos.Distinct().ForEach(p =>
            {
                var stock = stocks.FirstOrDefault(a => a.No == p);
                if (stock != null)
                {
                    RT.Service.Resolve<StockOrderService>().UpdateStockOrderState(stock, stock.StockOrderDetailList);
                    RF.Save(stock);
                }
            });
        }

        /// <summary>
        /// 创建发运订单
        /// </summary>
        public virtual void CreateShipment(List<double> planids)
        {
            using (var tran = DB.TransactionScope(ShipPlanEntityDataProvider.ConnectionStringName))
            {
                RT.Service.Resolve<DeliveryPlanController>().CreateSoByDeliveryPlans(planids, true);
                RT.Service.Resolve<StockPlanService>().UpdateStockDtlState(planids);
                tran.Complete();
            }
        }

        /// <summary>
        /// 强制关闭备料计划
        /// </summary>
        /// <param name="billNos">单据集合</param>
        /// <param name="lineNos">明细行号</param>
        public virtual void ForceCloseStockPlan(List<string> billNos, List<string> lineNos)
        {
            billNos.SplitDataExecute(tmpNos =>
            {
                DB.Update<StockPlan>().Set(p => p.State, DeliveryState.Cancel)
                                      .Where(p => tmpNos.Contains(p.OrderNo) && lineNos.Contains(p.OrderLineNo))
                                      .Execute();
            });
        }
    }
}
