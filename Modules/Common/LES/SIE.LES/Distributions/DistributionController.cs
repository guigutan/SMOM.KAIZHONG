using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.LES;
using SIE.Inventory.Onhands;
using SIE.Inventory.Piles;
using SIE.Items;
using SIE.LES.Distributions.Configs;
using SIE.LES.StockOrders.Service;
using SIE.LES.StockPlans;
using SIE.LES.StockPlans.Service;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.LES.Distributions
{
    /// <summary>
    /// 配送管理
    /// </summary>
    public partial class DistributionController : DomainController, IDistribution
    {
        /// <summary>
        /// AGV更新配送单状态
        /// </summary>
        /// <param name="lpn">托盘</param>       
        public virtual void AgvFinishUpdateDistribution(string lpn)
        {
            DB.Update<Distribution>().Where(p => p.IsCallAgv && p.Lpn == lpn).Set(p => p.OrderState, OrderState.Receipt).Execute();
        }

        /// <summary>
        /// 获取集货库位
        /// </summary>
        /// <param name="billNo">备料单号</param>
        /// <param name="lineNo">备料单行号</param>       
        /// <returns>集货库位</returns>       
        public virtual double? GetDisSettingLocation(string billNo, string lineNo)
        {
            var setting = Query<DistributionSetting>().Join<StockPlan>((x, y) => x.WarehouseId == y.WarehouseId && x.ProductLineId == y.ResourceId)
                        .Where<StockPlan>((x, y) => (y.OrderNo == billNo && y.OrderLineNo == lineNo || y.No == billNo && y.IsMergeIssued) && y.State != ShipPlan.DeliveryState.Cancel
                        && y.State != ShipPlan.DeliveryState.Created && y.State != ShipPlan.DeliveryState.Aduited)
                        .Where(p => p.ProductLineId > 0 && p.StorageLocationId > 0 && p.State == State.Enable).FirstOrDefault();
            return setting?.StorageLocationId;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="criteria">查询</param>
        /// <returns></returns>       
        public virtual EntityList GetDistributionSettings(DistributionSettingCriteria criteria)
        {
            var query = DB.Query<DistributionSetting>("A0");
            ////仓库权限关联查询
            RT.Service.Resolve<WarehouseController>().ExistWarehouseTypeEmplyee(query, DistributionSetting.WarehouseIdProperty, LibraryType.Entity);

            if (criteria.ProductLineId.HasValue)
            {
                query.Where(p => p.ProductLineId == criteria.ProductLineId);
            }
            if (criteria.WarehouseId.HasValue)
            {
                query.Where(p => p.WarehouseId == criteria.WarehouseId);
            }
            if (criteria.State.HasValue)
            {
                query.Where(p => p.State == criteria.State);
            }
            if (criteria.CreateById.HasValue)
            {
                query.Where(p => p.CreateBy == criteria.CreateById);
            }
            //创建时间
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            }

            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            }

            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据仓库ID和产品ID获取配置设置信息
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="productLineId">产品ID</param>
        /// <param name="state">状态</param>
        /// <param name="id">Id</param>
        /// <param name="elo">贪婪加载选项</param>
        /// <returns>配置设置信息</returns>
        public virtual int GetDistributionSettings(double id, double? warehouseId, double productLineId, State state)
        {
            return Query<DistributionSetting>().Where(p => p.Id != id && p.WarehouseId == warehouseId && p.ProductLineId == productLineId && p.State == state).Count();
        }

        /// <summary>
        /// 获取配送管理数据
        /// </summary>
        /// <param name="criteria">查询</param>
        /// <returns>配送管理</returns>
        public virtual EntityList GetDistributions(DistributionCriteria criteria)
        {
            var query = DB.Query<Distribution>("A0");
            ////仓库权限关联查询
            RT.Service.Resolve<WarehouseController>().ExistWarehouseTypeEmplyee(query, Distribution.WarehouseIdProperty, LibraryType.Entity);

            if (criteria.No.IsNotEmpty())
            {
                query.Where(p => p.No == criteria.No);
            }
            if (criteria.SourceNo.IsNotEmpty())
            {
                query.Where(p => p.SourceNo == criteria.SourceNo);
            }
            if (criteria.WarehouseId.HasValue)
            {
                query.Where(p => p.WarehouseId == criteria.WarehouseId);
            }
            if (!string.IsNullOrEmpty(criteria.OrderState))
            {
                var criteriaState = new List<int>();
                criteria.OrderState.Split(',').ForEach(state =>
                {
                    criteriaState.Add(int.Parse(state));
                });
                query.Where(p => criteriaState.Contains((int)p.OrderState));
            }
            if (criteria.Lpn.IsNotEmpty())
            {
                query.Where(p => p.Lpn == criteria.Lpn);
            }
            if (criteria.ProductLineId.HasValue)
            {
                query.Where(p => p.ProductLineId == criteria.ProductLineId);
            }
            if (criteria.ItemCode.IsNotEmpty())
            {
                query.Exists<DistributionDetail>((x, y) => y.Where(p => p.DistributionId == x.Id
                && (p.Item.Code.Contains(criteria.ItemCode) || p.Item.Name.Contains(criteria.ItemCode))));
            }
            //创建时间
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            }

            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            }

            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取配送单
        /// </summary>
        /// <param name="ids">Id</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>配送单</returns>
        public virtual EntityList<Distribution> GetDistributions(List<double> ids, EagerLoadOptions elo = null)
        {
            return Query<Distribution>().Where(p => ids.Contains(p.Id)).ToList(null, elo);
        }

        /// <summary>
        /// 取消配送单
        /// </summary>
        /// <param name="billIds">单据Id</param>
        public virtual void CancelDistribution(List<double> billIds)
        {
            using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                UpdateStockOrder(billIds);
                DB.Update<Distribution>().Set(p => p.OrderState, OrderState.Cancel).Set(p => p.UpdateBy, RT.IdentityId).Set(p => p.UpdateDate, DateTime.Now)
                    .Where(p => billIds.Contains(p.Id) && p.OrderState == OrderState.WaitDelivery).Execute();
                tran.Complete();
            }
        }

        /// <summary>
        /// 根据配送单号/LPN/发运单号查询待配送配送单
        /// </summary>
        /// <param name="no">配送单号</param>
        /// <param name="lpn">lpn</param>
        /// <param name="orderNo">发运单号</param>
        /// <param name="type">1-待配送 2-配送中 3-配送中/待配送</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns></returns>
        public virtual EntityList<Distribution> GetDistributionsByNos(string no, string lpn, string orderNo, int type, EagerLoadOptions elo = null)
        {
            var query = Query<Distribution>();
            if (!no.IsNullOrEmpty())
            {
                query.Where(p => p.No == no);
            }
            if (!lpn.IsNullOrEmpty())
            {
                query.Where(p => p.Lpn == lpn);
            }
            if (!orderNo.IsNullOrEmpty())
            {
                query.Where(p => p.SourceNo == orderNo);
            }
            if (type == 1)
            {
                //待配送
                query.Where(p => p.OrderState == OrderState.WaitDelivery);
            }
            else if (type == 2)
            {
                query.Where(p => p.OrderState == OrderState.Delivery);
            }
            else
            {
                query.Where(p => p.OrderState == OrderState.Delivery || p.OrderState == OrderState.WaitDelivery);
            }
            return query.ToList(null, elo);
        }

        /// <summary>
        /// 根据物料编码获取配送单
        /// </summary>
        /// <param name="ItemCode">物料编码</param>
        /// <param name="type">1-待配送 2-配送中 3-配送中/待配送</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns></returns>
        public virtual EntityList<Distribution> GetDistributionsByItemCode(string ItemCode, int type, EagerLoadOptions elo = null)
        {
            var query = Query<Distribution>().Join<DistributionDetail>((x, y) => x.Id == y.DistributionId).Join<DistributionDetail, Item>((x, y) => x.ItemId == y.Id && y.Code == ItemCode);
            if (type == 1)
            {
                //待配送
                query.Where(p => p.OrderState == OrderState.WaitDelivery);
            }
            else if (type == 2)
            {
                query.Where(p => p.OrderState == OrderState.Delivery);
            }
            else
            {
                query.Where(p => p.OrderState == OrderState.Delivery || p.OrderState == OrderState.WaitDelivery);
            }
            return query.ToList(null, elo);
        }


        /// <summary>
        /// 根据标签条码获取配送单
        /// </summary>
        /// <param name="label">标签条码</param>
        /// <param name="type">1-待配送 2-配送中 3-配送中/待配送</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns></returns>
        public virtual EntityList<Distribution> GetDistributionsByLabel(string label, int type, EagerLoadOptions elo = null)
        {
            var query = Query<Distribution>().Join<DistributionLabel>((x, y) => x.Id == y.DistributionId && y.LabelNo == label);
            if (type == 1)
            {
                query.Where(p => p.OrderState == OrderState.WaitDelivery);
            }
            else if (type == 2)
            {
                query.Where(p => p.OrderState == OrderState.Delivery);
            }
            else
            {
                query.Where(p => p.OrderState == OrderState.Delivery || p.OrderState == OrderState.WaitDelivery);
            }
            return query.ToList(null, elo);
        }

        /// <summary>
        /// 根据配送单号/LPN查询待配送配送单
        /// </summary>
        /// <param name="keyWord">单号或LPN</param>
        /// <param name="type">1-待配送 2-配送中 3-配送中/待配送</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns></returns>
        public virtual EntityList<Distribution> GetDistributionsByNosOrLpn(string keyWord, int type, EagerLoadOptions elo = null)
        {
            var query = Query<Distribution>().Where(p => p.No == keyWord || p.Lpn == keyWord);
            if (type == 1)
            {
                query.Where(p => p.OrderState == OrderState.WaitDelivery);
            }
            else if (type == 2)
            {
                query.Where(p => p.OrderState == OrderState.Delivery);
            }
            else
            {
                query.Where(p => p.OrderState == OrderState.Delivery || p.OrderState == OrderState.WaitDelivery);
            }
            return query.ToList(null, elo);
        }

        /// <summary>
        /// 获取配送单号
        /// </summary>
        /// <returns>配送单号</returns>         
        public virtual string GetDistributionNo()
        {
            var config = ConfigService.GetConfig(new DistributionNoConfig(), typeof(Distribution));
            if (config == null || config.DistributionNoRule == null)
                throw new ValidationException("未找到单号生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(config.DistributionNoRuleId.Value, 1).FirstOrDefault();
        }

        /// <summary>
        /// 获取配送单是否跳过扫描配送/配送接收
        /// </summary>
        /// <returns>是否跳过扫描配送/配送接收</returns>         
        public virtual IsNoDistributionType? CheckIsNoDistribution()
        {
            var config = ConfigService.GetConfig(new DistributionNoConfig(), typeof(Distribution));
            return config.IsNoDistribution;

        }

        /// <summary>
        /// 根据ID获取配送单单号
        /// </summary>
        /// <param name="idList">单据ID集合</param>
        /// <param name="elo">贪婪加载选项</param>
        /// <returns></returns>
        public virtual EntityList<Distribution> GetDistributionsByIds(List<double> idList, EagerLoadOptions elo = null)
        {
            return idList.SplitContains(ids =>
            {
                return Query<Distribution>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 创建配送单
        /// </summary>
        /// <param name="distributionDatas">配送单数据</param>         
        public virtual bool CreateDistribution(List<DistributionData> distributionDatas)
        {
            var piles = RT.Service.Resolve<PileController>().GetPiles(distributionDatas.Select(f => f.Lpn).Distinct().ToList());
            var outLpns = piles.Where(f => f.ItemState == ItemState.OutStore).Select(f => f.Code).ToList();
            bool flag = false;
            EntityList<Distribution> distributions = new EntityList<Distribution>();
            var disdatas = distributionDatas.Where(f => outLpns.Contains(f.Lpn) || f.Lpn == "*" || f.Lpn.IsNullOrEmpty());
            if (!disdatas.Any())
                return false;
            disdatas.ForEach(distributionData =>
                   {
                       if (distributionData.SourceNo.IsNullOrEmpty())
                           return;
                       var stockNos = distributionData.DistributionDtls.Select(a => a.OrderNo).Distinct().ToList();

                       var stockPlans = RT.Service.Resolve<StockPlanService>().GetStockPlans(stockNos);
                       if (stockPlans == null || !stockPlans.Any())
                           return;
                       if (stockPlans.Any(f => f.ResourceId == null))
                           return;
                       //var planDic = stockPlans.ToDictionary(a => a.No, a => a.ResourceId.Value);
                       stockPlans.Select(a => a.ResourceId.Value).Distinct().ForEach(r =>
                       {
                           var disSetting = Query<DistributionSetting>().Where(p => p.State == State.Enable && p.ProductLineId == r && (p.WarehouseId == distributionData.SourceWhId || p.WarehouseId == null)).Count();
                           if (disSetting == 0)
                               return;
                           var curStocks = stockPlans.Where(a => a.ResourceId == r).Select(a=>a.No).ToList();

                           Distribution distribution = new Distribution()
                           {
                               Lpn = distributionData.Lpn,
                               No = GetDistributionNo(),
                               OrderState = OrderState.WaitDelivery,
                               ProductLineId = r,
                               SourceNo = distributionData.SourceNo,
                               StorageLocationId = distributionData.SourceLocId,
                               WarehouseId = distributionData.SourceWhId,
                           };
                           distribution.GenerateId();
                           int lineNo = 1;
                           distributionData.DistributionDtls.Where(p => curStocks.Contains(p.OrderNo)).ForEach(p =>
                            {
                                DistributionDetail detail = new DistributionDetail()
                                {
                                    AssignId = p.AssignId,
                                    ItemExtProp = p.ItemExtProp,
                                    ItemExtPropName = p.ItemExtPropName,
                                    ItemId = p.ItemId,
                                    LotCode = p.LotCode,
                                    OnhandState = (OnhandState)p.OnhandState,
                                    Qty = p.Qty,
                                    LineNo = lineNo.ToString(),
                                    SoLineNo = p.SoLineNo,
                                    SoDtlId = p.SoDtlId,
                                    OrderNo = p.OrderNo,
                                    OrderLineNo = p.OrderLineNo,
                                };
                                detail.GenerateId();
                                lineNo++;
                                distribution.DistributionDetailList.Add(detail);

                                distributionData.SoLabels.Where(f => f.SoNo == distributionData.SourceNo
                                && f.AssignId == p.AssignId).ForEach(f =>
                                {
                                    DistributionLabel distributionLabel = new DistributionLabel()
                                    {
                                        DistributionDetailId = detail.Id,
                                        DistributionId = distribution.Id,
                                        IsSerialNumber = f.IsSerialNumber,
                                        ItemId = f.ItemId,
                                        LabelNo = f.No,
                                        HighestNo = f.HighestNo,
                                        Qty = f.Qty,
                                    };
                                    distribution.DistributionLabelList.Add(distributionLabel);
                                });
                            });

                           distributions.Add(distribution);
                           flag = true;

                       });
                   });
            if (flag)
                RF.Save(distributions);
            return flag;
        }

        /// <summary>
        /// 根据产线获取配送单据
        /// </summary>
        /// <param name="keyword">产线名称/编码</param>
        /// <param name="type">1-待配送 2-配送中 3-配送中/待配送</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns></returns>
        public virtual EntityList<Distribution> GetDistributionsByProductLine(string keyword, int type, EagerLoadOptions elo = null)
        {
            var query = Query<Distribution>().Where(p => p.ProductLine.Code == keyword || p.ProductLine.Name == keyword);
            if (type == 1)
            {
                query.Where(p => p.OrderState == OrderState.WaitDelivery);
            }
            else if (type == 2)
            {
                query.Where(p => p.OrderState == OrderState.Delivery);
            }
            else
            {
                query.Where(p => p.OrderState == OrderState.Delivery || p.OrderState == OrderState.WaitDelivery);
            }
            return query.ToList(null, elo);
        }

        /// <summary>
        /// 根据配送单ID 获取扫描记录
        /// </summary>
        /// <param name="idList">配送单ID集合</param>
        /// <param name="elo">贪婪加载选项</param>
        /// <returns></returns>
        public virtual EntityList<DistributionLabel> GetDistributionLabelsByOrderIds(List<double> idList, EagerLoadOptions elo = null)
        {
            return idList.SplitContains(ids =>
            {
                return Query<DistributionLabel>().Where(p => ids.Contains(p.DistributionId)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 根据配送单ID和分配ID获取配送单明细
        /// </summary>
        /// <param name="id">配送单ID</param>
        /// <param name="assignId">分配ID</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns></returns>
        public virtual EntityList<DistributionDetail> GetDistributionDetailByAssignId(double id, string assignId, EagerLoadOptions elo = null)
        {
            return Query<DistributionDetail>().Where(p => p.DistributionId == id && p.AssignId == assignId).ToList(null, elo);
        }

        /// <summary>
        /// 取消发货
        /// </summary>
        /// <param name="soDtlIds">发运单明细</param>    
        public virtual void CancelShipping(List<double> soDtlIds)
        {
            var dis = Query<Distribution>().Exists<DistributionDetail>((x, y) => y.Where(f => f.DistributionId == x.Id && soDtlIds.Contains(f.SoDtlId))).ToList();
            if (dis.Any(f => f.OrderState == OrderState.Delivery || f.OrderState == OrderState.Receipt))
                throw new ValidationException("发运单已经配送，不能取消".L10N());
            var disIds = dis.Select(f => f.Id).ToList();
            var disNo = Query<Distribution>().Where(p => disIds.Contains(p.Id)).Exists<DistributionDetail>((x, y) => y.Where(f => f.DistributionId == x.Id && !soDtlIds.Contains(f.SoDtlId))).Count();
            if (disNo > 0)
                throw new ValidationException("配送单关联多个发运单明细，不能对明细单独取消发货".L10N());
            dis.Where(p => p.OrderState == OrderState.WaitDelivery).ForEach(p => p.OrderState = OrderState.Close);
            RF.Save(dis);
        }
    }
}
