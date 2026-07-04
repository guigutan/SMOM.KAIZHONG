using DocumentFormat.OpenXml.Bibliography;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Common.Sender;
using SIE.Core.Common.Service;
using SIE.Core.Enums;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.WorkOrders;
using SIE.Items;
using SIE.LES.StockOrders.Configs;
using SIE.LES.StockOrders.Dao;
using SIE.LES.StockOrders.WorkOrders;
using SIE.LES.StockPlans;
using SIE.LES.StockPlans.Service;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.ShipPlan;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.LES.StockOrders.Service
{
    /// <summary>
    /// 备料单Service
    /// </summary>
    public partial class StockOrderService : DomainService
    {
        #region 属性 + 构造方法

        /// <summary>
        /// 数据访问
        /// </summary>
        private readonly StockOrderDao _stockOrderDao;

        /// <summary>
        /// 构造函数
        /// </summary>
        public StockOrderService(StockOrderDao stockOrderDao)
        {
            _stockOrderDao = stockOrderDao;
        }
        #endregion

        /// <summary>
        /// 获取备料单数据
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>备料单数据</returns>
        public virtual EntityList<StockOrder> GetStockOrders(StockOrderCriteria criteria)
        {
            return _stockOrderDao.GetStockOrders(criteria);
        }

        /// <summary>
        /// 获取备料单数据
        /// </summary>
        /// <param name="stockOrderIds">ID集合</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>备料单数据</returns>
        public virtual EntityList<StockOrder> GetStockOrderByIds(List<double> stockOrderIds, EagerLoadOptions elo = null)
        {
            return _stockOrderDao.GetStockOrderByIds(stockOrderIds, elo);
        }
        /// <summary>
        /// 获取备料单明细
        /// </summary>
        /// <param name="stockOrderIds"></param>
        /// <param name="elo"></param>
        /// <returns></returns>
        public virtual EntityList<StockOrderDetail> GetStockOrderDetailByOrderIds(List<double> stockOrderIds, EagerLoadOptions elo = null)
        {
            return _stockOrderDao.GetStockOrderDetailByOrderIds(stockOrderIds, elo);
        }

        /// <summary>
        /// 获取同工单同物料同物料属性是否存在相同备料明细
        /// </summary>
        /// <param name="curId"></param>
        /// <param name="woId"></param>
        /// <param name="itemId"></param>
        /// <param name="itemExprop"></param>
        /// <returns></returns>
        public virtual bool IsSameWoItemItemExpExsited(double curId, double? woId, double itemId, string itemExprop)
        {
            return _stockOrderDao.IsSameWoItemItemExpExsited(curId, woId, itemId, itemExprop);
        }

        /// <summary>
        /// 是否存在相同仓库同物料扩展属性的备料单明细
        /// </summary>
        /// <param name="curId"></param>
        /// <param name="whId"></param>
        /// <param name="itemId"></param>
        /// <param name="itemExprop"></param>
        /// <returns></returns>
        public virtual bool IsSameWhItemItemExpExsited(double curId, double? whId, double itemId, string itemExprop)
        {
            return _stockOrderDao.IsSameWhItemItemExpExsited(curId, whId, itemId, itemExprop);
        }


        /// <summary>
        /// 创建备料单号
        /// </summary>
        /// <returns>备料单号</returns>
        public virtual string GetStockOrderNo()
        {
            NoConfigValue config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(StockOrder));
            return config == null || config.BacodeRule == null
                ? throw new ValidationException("未找到备料单号生成规则,请检查规则配置".L10N())
                : RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 创建备料单号
        /// </summary>
        /// <returns>备料单号</returns>
        public virtual List<string> GetStockOrderNoList(int length)
        {
            NoConfigValue config = ConfigService.GetConfig(new NoConfig(), typeof(StockOrder));
            return config == null || config.BacodeRule == null
                ? throw new ValidationException("未找到备料单号生成规则,请检查规则配置".L10N())
                : RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, length).ToList();
        }

        /// <summary>
        /// 获取配料单手工单据是否需要审核配置
        /// </summary>
        /// <returns>是否需要审核</returns>
        public virtual StockOrderIsAuditConfigValue GetStockOrderConfig()
        {
            return ConfigService.GetConfig<StockOrderIsAuditConfigValue>(new StockOrderIsAuditConfig(), typeof(StockOrder));
        }

        /// <summary>
        /// 通过备料单单号获取备料单信息
        /// </summary>
        /// <param name="No"></param>
        /// <param name="elo">贪婪加载参数</param>
        /// <returns>备料单信息</returns>
        public virtual StockOrder GetStockOrdersByNo(string No, EagerLoadOptions elo)
        {
            return _stockOrderDao.GetStockOrdersByNo(No, elo);
        }

        /// <summary>
        /// 通过备料单单号集合获取备料单信息
        /// </summary>
        /// <param name="nos">单号集合</param>
        /// <param name="elo">贪婪加载选项</param>
        /// <returns></returns>
        public virtual EntityList<StockOrder> GetStockOrderByNos(List<string> nos, EagerLoadOptions elo = null)
        {
            return _stockOrderDao.GetStockOrderByNos(nos, elo);
        }

        /// <summary>
        /// 通过状态号获取备料单
        /// </summary>
        /// <param name="states">状态</param>
        /// <param name="elo"></param>
        /// <returns></returns>
        public virtual EntityList<StockOrder> GetStockOrderByStates(List<StockState> states, EagerLoadOptions elo = null)
        {
            return _stockOrderDao.GetStockOrderByStates(states, elo);
        }

        /// <summary>
        /// 获取工单BOM下物料
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键值</param>
        /// <returns>物料列表</returns>
        public virtual EntityList<Item> GetWorkOrderBomItems(double workOrderId, PagingInfo pagingInfo, string keyword)
        {
            IEntityQueryer<Item> query = DB.Query<Item>().Join<WorkOrderBom>((x, y) => x.Id == y.ItemId && y.WorkOrderId == workOrderId && x.ConsumeMode == ConsumeMode.Push);
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 更细备料单状态
        /// </summary>
        /// <param name="stockOrder">备料单</param>
        /// <param name="stockOrderDetails">备料单需求明细</param>
        public virtual void UpdateStockOrderState(StockOrder stockOrder, EntityList<StockOrderDetail> stockOrderDetails)
        {
            _stockOrderDao.UpdateStockOrderState(stockOrder, stockOrderDetails);
        }

        /// <summary>
        /// 获取调度触发的备料单状态的配置项值
        /// </summary>
        /// <returns>是否需要审核</returns>
        public virtual SchedulingTriggeredStatusConfigValue GetSchedulingTriggeredStatusConfigValue()
        {
            return ConfigService.GetConfig(new SchedulingTriggeredStatusConfig(), typeof(StockOrder));
        }

        /// <summary>
        /// 获取工厂或车间下启用的生产资源
        /// </summary>
        /// <param name="workShopId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<WipResource> GetActiveWipResource(double? workShopId = null, PagingInfo pagingInfo = null, string keyword = null)
        {
            return _stockOrderDao.GetWipResources(workShopId, pagingInfo, keyword);
        }

        #region 业务逻辑
        /// <summary>
        /// 撤回备料单
        /// </summary>
        /// <param name="stockOrderIds">备料单ID集合</param>
        public virtual void ReCallStockOrders(List<double> stockOrderIds)
        {
            EntityList<StockOrder> stockOrders = GetStockOrderByIds(stockOrderIds);
            if (stockOrders.Any(t => t.StockState != StockState.Created && t.StockState != StockState.Audit))
            {
                throw new ValidationException("备料单不是[已创建]或[待审核]状态，不能进行撤回操作".L10N());
            }
            using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                stockOrderIds.SplitDataExecute(tmpIds =>
                {
                    DB.Update<StockOrder>().Set(p => p.StockState, StockState.ReCall)
                                           .Where(p => tmpIds.Contains(p.Id))
                                           .Execute();

                    DB.Update<StockOrderDetail>().Set(p => p.StockState, StockState.ReCall)
                                                 .Where(p => tmpIds.Contains(p.StockOrderId))
                                                 .Execute();
                });

                tran.Complete();
            }
        }

        /// <summary>
        /// 审核备料单
        /// </summary>
        /// <param name="stockOrderIds">备料单ID集合</param>
        public virtual void AduitStockOrders(List<double> stockOrderIds)
        {
            EntityList<StockOrder> stockOrders = GetStockOrderByIds(stockOrderIds);
            if (stockOrders.Any(t => t.StockState != StockState.Audit))
            {
                throw new ValidationException("备料单不是[待审核]状态，不能进行审核操作".L10N());
            }

            using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                stockOrderIds.SplitDataExecute(tmpIds =>
                 {
                     DB.Update<StockOrder>().Set(p => p.StockState, StockState.Submitted)
                                            .Where(p => tmpIds.Contains(p.Id))
                                            .Execute();

                     DB.Update<StockOrderDetail>().Set(p => p.StockState, StockState.Submitted)
                                                  .Where(p => tmpIds.Contains(p.StockOrderId))
                                                  .Execute();
                 });
                tran.Complete();
            }
        }

        /// <summary>
        /// 提交备料单
        /// </summary>
        /// <param name="stockOrderIds">备料单ID集合</param>
        public virtual void SubmitStockOrders(List<double> stockOrderIds)
        {
            EntityList<StockOrder> stockOrders = GetStockOrderByIds(stockOrderIds);
            if (stockOrders.Any(t => t.StockState != StockState.Created))
            {
                throw new ValidationException("备料单不是[已创建]状态，不能进行提交操作".L10N());
            }
            //若同工单同物料同拓展属性 存在（待审核、已提交、拣配中、待接收）的需求明细，存在则不能提交
            EntityList<StockOrderDetail> stockOrderDetails = GetStockOrderDetailByOrderIds(stockOrderIds);
            if (!stockOrderDetails.Any())
            {
                throw new ValidationException("备料单不存在明细，不能进行提交操作".L10N());
            }

            foreach (StockOrderDetail stockOrderDetail in stockOrderDetails)
            {
                if (stockOrderDetail.StockOrder.StockType == PrepareItemType.Push)
                {
                    bool hadSame = IsSameWoItemItemExpExsited(stockOrderDetail.Id, stockOrderDetail.StockOrder.WorkOrderId, stockOrderDetail.ItemId, stockOrderDetail.ItemExtProp);
                    if (hadSame)
                    {
                        throw new ValidationException("同工单同物料同物料拓展属性 存在（待审核、已提交、拣配中、待接收）的需求明细，不能进行提交操作".L10N());
                    }
                }
                if (stockOrderDetail.StockOrder.StockType == PrepareItemType.Pull)
                {
                    bool hadSame = IsSameWhItemItemExpExsited(stockOrderDetail.Id, stockOrderDetail.WarehouseId, stockOrderDetail.ItemId, stockOrderDetail.ItemExtProp);
                    if (hadSame)
                    {
                        throw new ValidationException("同工单同物料同物料拓展属性 存在（待审核、已提交、拣配中、待接收）的需求明细，不能进行提交操作".L10N());
                    }
                }

            }

            StockOrderIsAuditConfigValue config = GetStockOrderConfig();
            bool isRequireAudit = config?.IsAudit ?? false;

            using Data.Transaction.SingleTransactionScope tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName);
            //如果需审核，则将物料需求更新为待审核。如果不需审核，将单据状态改为已提交，并调用备料计划的接口
            StockState stockState = isRequireAudit ? StockState.Audit : StockState.Submitted;

            stockOrderIds.SplitDataExecute(tmpIds =>
            {
                DB.Update<StockOrder>().Set(p => p.StockState, stockState)
                                       .Where(p => tmpIds.Contains(p.Id))
                                       .Execute();

                DB.Update<StockOrderDetail>().Set(p => p.StockState, stockState)
                                             .Where(p => tmpIds.Contains(p.StockOrderId))
                                             .Execute();
            });
            tran.Complete();
        }

        /// <summary>
        /// 下发备料单
        /// </summary>       
        /// <exception cref="ValidationException"></exception>
        public virtual void IssuedStockOrdersJob()
        {
            var stockOrders = GetStockOrderByStates(new List<StockState>() { StockState.Submitted });

            using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                //创建LES的备料计划
                CreateStockPlan(stockOrders);
                tran.Complete();
            }
        }

        /// <summary>
        /// 下发备料单
        /// </summary>
        /// <param name="stockOrderIds"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void IssuedStockOrders(List<double> stockOrderIds)
        {
            EntityList<StockOrder> stockOrders = GetStockOrderByIds(stockOrderIds);
            if (stockOrders.Any(t => t.StockState != StockState.Submitted))
            {
                throw new ValidationException("单据状态已改变，请刷新界面".L10N());
            }
            //若同工单同物料同拓展属性 存在（待审核、已提交、拣配中、待接收）的需求明细，存在则不能提交
            EntityList<StockOrderDetail> stockOrderDetails = GetStockOrderDetailByOrderIds(stockOrderIds, new EagerLoadOptions().LoadWith(StockOrderDetail.StockOrderProperty));
            if (!stockOrderDetails.Any())
            {
                throw new ValidationException("备料单不存在明细，不能进行下发操作".L10N());
            }
            //郭锐去掉这个验证,20231012,禅道Id 2339
            //foreach (StockOrderDetail stockOrderDetail in stockOrderDetails)
            //{
            //    if (stockOrderDetail.StockOrder.StockType== PrepareItemType.Push)
            //    {
            //        bool hadSame = IsSameWoItemItemExpExsited(stockOrderDetail.Id, stockOrderDetail.StockOrder.WorkOrderId, stockOrderDetail.ItemId, stockOrderDetail.ItemExtProp);
            //        if (hadSame)
            //        {
            //            throw new ValidationException("同工单同物料同物料扩展属性 存在（待审核、已提交、拣配中、待接收）的需求明细，不能进行下发操作".L10N());
            //        }
            //    }
            //}
            using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                //创建LES的备料计划
                CreateStockPlan(stockOrders);
                tran.Complete();
            }
        }


        /// <summary>
        /// 强制关闭备料单
        /// </summary>
        /// <param name="stockOrderIds">备料单ID集合</param>
        public virtual void ForceCloseStockOrders(List<double> stockOrderIds)
        {
            EntityList<StockOrder> stockOrders = GetStockOrderByIds(stockOrderIds, new EagerLoadOptions().LoadWith(StockOrder.StockOrderDetailListProperty).LoadWithViewProperty());
            if (stockOrders.Any(t => t.StockState == StockState.Submitted
            || t.StockState == StockState.Issued
            || t.StockState == StockState.PickStocking
            || t.StockState == StockState.Issued
            || t.StockState == StockState.TobeReceive
            || t.StockState == StockState.Received))
            {

                foreach (var stockOrder in stockOrders)
                {
                    foreach (var item in stockOrder.StockOrderDetailList)
                    {
                        if (item.ShipQty != item.ReceiveQty)
                        {
                            throw new ValidationException("物料编码[{0}]已发运数量不等于已接收数量，存在已发运未接收数量，无法强制关闭".L10nFormat(item.Item.Code));
                        }
                    }
                }

                List<string> billNos = stockOrders.Select(p => p.No).ToList();
                //获取所有备料单的备料计划

                var stockPlans = billNos.SplitContains(nos =>
                {//备料单物料需求关联的备料计划不为已完成且不为取消，则提示“物料编码XXX关联的备料计划状态不是已完成或取消，无法强制关闭”
                    return DB.Query<StockPlan>().Where(m => nos.Contains(m.OrderNo) && (m.State != DeliveryState.Finished && m.State != DeliveryState.Cancel)).ToList();
                });
                if (stockPlans.Any())
                {
                    throw new ValidationException("所选备料单中存在关联的备料计划状态不是已完成或取消，无法强制关闭");
                }
                using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
                {
                    stockOrderIds.SplitDataExecute(tmpIds =>
                     {
                         DB.Update<StockOrder>().Set(p => p.StockState, StockState.Closed)
                                                .Where(p => tmpIds.Contains(p.Id))
                                                .Execute();

                         DB.Update<StockOrderDetail>().Set(p => p.StockState, StockState.Closed)
                                                       .Set(p => p.CancelQty, p => p.Qty - p.ReceiveQty - p.CancelQty)
                                                      .Where(p => tmpIds.Contains(p.StockOrderId))
                                                      .Execute();
                     });

                    //同步关闭备料计划
                    List<string> lineNos = stockOrders.SelectMany(p => p.StockOrderDetailList.Select(t => t.LineNo)).Distinct().ToList();
                    RT.Service.Resolve<StockPlanService>().ForceCloseStockPlan(billNos, lineNos);
                    tran.Complete();
                }
            }
            else
            {
                throw new ValidationException("备料单不是【已提交】、【已下发】、【已拣配】、【拣配中】、【待接收】、【已接收】状态，不能进行强制关闭操作".L10N());
            }
        }

        /// <summary>
        /// 设置合并的明细数据
        /// </summary>
        /// <param name="stockOrders">备料单</param>
        /// <param name="stockOrderDetails">明细</param>
        /// <param name="orderMergeIssueds">合并Id</param>
        private void SetMerDtlIds(EntityList<StockOrder> stockOrders, EntityList<StockOrderDetail> stockOrderDetails, List<StockOrderMergeIssued> orderMergeIssueds)
        {
            EntityList<StockOrderMergeIssued> stockOrderMerges = RT.Service.Resolve<StockOrderMergeIssuedController>().GetMergeIssued(State.Enable);
            stockOrders.ForEach(p =>
            {
                p.StockState = StockState.Issued;
                p.StockOrderDetailList.ForEach(d =>
                {
                    d.StockOrder = p;
                    stockOrderDetails.Add(d);
                    StockOrderMergeIssued stockOrderMerge = stockOrderMerges.FirstOrDefault(p => p.WarehouseId == d.WarehouseId && p.WipResourceId == d.ResourceId
                    && p.StockModel == d.StockType);
                    if (stockOrderMerge != null && !orderMergeIssueds.Any(p => p.Id == stockOrderMerge.Id))
                    {
                        orderMergeIssueds.Add(stockOrderMerge);
                    }
                });
            });
        }
        /// <summary>
        /// 创建合并备料计划
        /// </summary>
        /// <param name="ri"></param>
        /// <param name="planParams"></param>
        private void MergeCreateStockPlan(List<StockOrderDetail> ri, List<ShipPlanParam> planParams)
        {
            ri = ri.OrderBy(p => p.StockOrderId).ToList();
            string planOrderNo = RT.Service.Resolve<StockOrderService>().GetStockOrderNo();
            string orderNo = string.Empty;
            double id = 0;
            ri.ForEach(p =>
            {
                p.StockState = StockState.Issued;
                if (p.StockOrderId != id)
                {
                    id = p.StockOrderId;
                    if (!string.IsNullOrEmpty(orderNo))
                    {
                        orderNo += ";";
                    }
                    orderNo += p.StockOrderNo + ":" + p.LineNo;
                }
                else
                {
                    orderNo += "," + p.LineNo;
                }
            });
            planParams.Add(new ShipPlanParam()
            {
                OrderType = (int)OrderType.WorkFeed,
                ItemCode = ri[0].ItemCode,
                EnterpriseCode = ri[0].EnterpriseCode,
                DeliveryDate = DateTime.Now,
                OrderNo = orderNo,
                OrderLineNo = "",
                PlanOrderNo = "HB" + planOrderNo,
                PlanDtlLineNo = 1,
                ResourceId = ri[0].ResourceId,
                ItemExtProp = ri[0].ItemExtProp,
                ItemExtPropName = ri[0].ItemExtPropName,
                Qty = ri.Sum(p => p.Qty),
                IsMergeIssued = true
            });
        }
        /// <summary>
        /// 创建备料计划
        /// </summary>
        /// <param name="stockOrders">备料单列表</param>
        public virtual void CreateStockPlan(EntityList<StockOrder> stockOrders)
        {
            List<ShipPlanParam> planParams = new List<ShipPlanParam>();
            EntityList<StockOrderDetail> stockOrderDetails = new EntityList<StockOrderDetail>();
            List<StockOrderMergeIssued> orderMergeIssueds = new List<StockOrderMergeIssued>();
            //所有需要下发的需求明细
            SetMerDtlIds(stockOrders, stockOrderDetails, orderMergeIssueds);
            List<double> merDtlIds = new List<double>();
            //合并下发
            orderMergeIssueds.ForEach(d =>
            {
                d.StockOrderMergeTimesList.ForEach(t =>
                {
                    DateTime start = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + t.StartTime.ToString("HH:mm:00"));
                    DateTime end = DateTime.Parse(DateTime.Now.AddDays(t.IsCrossDay ? 1 : 0).ToString("yyyy-MM-dd") + " " + t.EndTime.ToString("HH:mm:59"));
                    List<StockOrderDetail> details = stockOrderDetails.Where(p =>
                          p.WarehouseId == d.WarehouseId
                       && p.ResourceId == d.WipResourceId
                       && p.StockType == d.StockModel
                       && DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + p.DemandTime.ToString("HH:mm:00")) >= start
                       && DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + p.DemandTime.ToString("HH:mm:00")) < end
                       ).ToList();
                    if (details.Count > 0)
                    {
                        details.GroupBy(p => new { p.ItemId, p.ItemExtProp, p.ItemExtPropName, p.IsManualRec }).ForEach(ri =>
                             {
                                 if (ri.Count() > 1)
                                 {
                                     MergeCreateStockPlan(ri.ToList(), planParams);
                                     merDtlIds.AddRange(ri.Select(a => a.Id).ToList());
                                 }
                             });
                    }
                });
            });
            //非合并下发
            stockOrderDetails.Where(p => !merDtlIds.Contains(p.Id)).ForEach(d =>
            {
                planParams.Add(new ShipPlanParam()
                {
                    OrderType = (int)OrderType.WorkFeed,
                    ItemCode = d.ItemCode,
                    EnterpriseCode = d.EnterpriseCode,
                    DeliveryDate = DateTime.Now,
                    OrderNo = d.StockOrderNo,
                    OrderLineNo = d.LineNo,
                    PlanOrderNo = d.StockOrderNo,
                    PlanDtlLineNo = int.Parse(d.LineNo),
                    ResourceId = d.ResourceId,
                    ItemExtProp = d.ItemExtProp,
                    ItemExtPropName = d.ItemExtPropName,
                    Qty = d.Qty,
                    IsMergeIssued = false
                });
                d.StockState = StockState.Issued;
            });
            RT.Service.Resolve<DeliveryPlanController>().CreateDeliveryPlanData(planParams);
            RF.Save(stockOrders);

        }


        /// <summary>
        /// 删除接收记录更新备料单状态
        /// </summary>
        /// <param name="stockIds">备料单ID</param>
        /// <param name="stockDtls">备料单明细ID</param>
        /// <param name="stockSns">删除的备料单接收记录</param>
        public virtual void UpDateStockOrderByCancelShip(List<double> stockIds, List<double> stockDtls, EntityList<StockOrderSn> stockSns)
        {
            StockOrderService stockService = RT.Service.Resolve<StockOrderService>();
            EntityList<StockOrder> stocks = stockService.GetStockOrderByIds(stockIds);
            stocks.ForEach(p =>
            {
                p.StockOrderDetailList.Where(f => stockDtls.Contains(f.Id)).ForEach(a =>
                {
                    decimal qty = stockSns.Where(x => x.StockOrderDetailId == a.Id).Sum(x => x.ShipQty);
                    a.ShipQty -= qty;
                    RT.Service.Resolve<StockOrderDetailService>().UpdateStockDetailState(a);
                });
                RT.Service.Resolve<StockOrderService>().UpdateStockOrderState(p, p.StockOrderDetailList);
            });
            RF.Save(stocks);
        }


        /// <summary>
        /// 获取接收方式
        /// </summary>
        /// <returns></returns>
        public virtual StockReceiveType GetReceiveType()
        {
            StockReceiveTypeConfigValue config = ConfigService.GetConfig(new StockReceiveTypeConfig(), typeof(StockOrder));
            return config == null ? throw new ValidationException("未找到,请检查规则配置".L10N()) : config.ReceiveType;
        }

        /// <summary>
        /// 推式备料是否限制最高存量
        /// </summary>
        /// <returns></returns>
        public virtual bool GetLimitedMaximumStock()
        {
            LimitedMaximumStockConfigValue config = ConfigService.GetConfig(new LimitedMaximumStockConfig(), typeof(StockOrder));
            return config == null ? throw new ValidationException("未找到,请检查规则配置".L10N()) : config.IsLimited;
        }

        /// <summary>
        /// 获取拉式物料最高库量
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="whId"></param>
        /// <param name="itemExtPro"></param>
        /// <returns></returns>
        public virtual decimal GetPrepareItemPullMaximumStock(double itemId, double? whId, string itemExtPro)
        {
            PrepareItemPull prepareItemPull = DB.Query<PrepareItemPull>().Where(p => p.WarehouseId == whId && p.ItemId == itemId && p.ItemExtProp == itemExtPro).FirstOrDefault();
            return prepareItemPull != null ? prepareItemPull.MaxStock.HasValue ? prepareItemPull.MaxStock.Value : -1 : -1;
        }

        /// <summary>
        /// 获取推式物料最高库量
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="whId"></param>
        /// <param name="itemExtProp"></param>
        /// <returns></returns>
        public virtual decimal GetBaseItemIoLimit(double itemId, double? whId, string itemExtProp)
        {
            BaseItemIoLimit baseItemIoLimitPush = DB.Query<BaseItemIoLimit>().Where(m => m.WarehouseId == whId && m.ItemId == itemId && m.ItemExtProp == itemExtProp).FirstOrDefault();
            return baseItemIoLimitPush != null ? baseItemIoLimitPush.MaxStockQty.HasValue ? baseItemIoLimitPush.MaxStockQty.Value : -1 : -1;
        }

        /// <summary>
        /// 查询工单
        /// </summary>
        /// <param name="criteira"></param>
        /// <returns></returns>
        public virtual EntityList<StockOrderWoViewModel> GetStockOrderWoViewModels(StockOrderWoViewModelCriteria criteira)
        {
            if (criteira == null)
            {
                return new EntityList<StockOrderWoViewModel>();
            }
            EntityList<StockOrderWoViewModel> viewmodel = new EntityList<StockOrderWoViewModel>();
            var workOrderInfoWithCount = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderList(criteira.WoNo, criteira.FactoryId, criteira.WorkshopId, criteira.WipResourceId, criteira.ProductCode, criteira.ProductName, false, false, null, null, criteira.PagingInfo);
            var workInfoList = workOrderInfoWithCount.WorkOrderInfos;
            foreach (var workInfo in workInfoList)
            {
                StockOrderWoViewModel view = new StockOrderWoViewModel
                {
                    Id = workInfo.WorkOrderId,
                    WoNo = workInfo.WorkOrderNo,
                    FactoryId = workInfo.FactoryId,
                    Factory = workInfo.FactoryCode,
                    WorkshopId = workInfo.WorkShopId ?? 0,
                    Workshop = workInfo.WorkShopCode,
                    WipResourceId = workInfo.ResourceId ?? 0,
                    WipResource = workInfo.ResourceCode,
                    ProductId = workInfo.ProductId,
                    ProductCode = workInfo.ProductCode,
                    ProductName = workInfo.ProductName,
                    PlanQty = workInfo.PlanQty,
                    WoState = workInfo.State,
                    PlanBeginDate = workInfo.PlanBeginDate,
                    PlanEndDate = workInfo.PlanEndDate,
                };
                viewmodel.Add(view);
            }
            viewmodel.SetTotalCount(workOrderInfoWithCount.TotalCount);
            return viewmodel;
        }
        #endregion
    }
}