using SIE.Core.Common;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages;
using SIE.EventMessages.Shipment;
using SIE.EventMessages.WMS.Receipt;
using SIE.Items;
using SIE.ShipPlan.Datas;
using SIE.ShipPlan.ViewModels;
using SIE.Warehouses;
using SIE.Warehouses.Enums;
using SIE.WMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ShipPlan
{
    /// <summary>
    /// 发货计划控制器
    /// </summary>
    public partial class DeliveryPlanController : DomainController
    {
        /// <summary>
        /// 获取发货计划数据
        /// </summary>
        /// <param name="criteria">发货计划实体</param>
        /// <returns>发货计划数据</returns>
        public virtual EntityList<DeliveryPlan> GetDeliveryPlans(DeliveryPlanCriteria criteria)
        {
            var query = Query<DeliveryPlan>();

            if (criteria.No.IsNotEmpty())
            {
                query.Where(p => p.No.Contains(criteria.No));
            }
            if (!string.IsNullOrEmpty(criteria.State))
            {
                var criteriaState = new List<int>();
                criteria.State.Split(',').ForEach(state =>
                {
                    criteriaState.Add(int.Parse(state));
                });
                query.Where(p => criteriaState.Contains((int)p.State));
            }
            if (criteria.OrderType.HasValue)
            {
                query.Where(p => p.OrderType == criteria.OrderType.Value);
            }
            if (criteria.EnterpriseId.HasValue)
            {
                query.Where(p => p.EnterpriseId == criteria.EnterpriseId.Value);
            }
            if (criteria.CustomerId.HasValue)
            {
                query.Where(p => p.CustomerId == criteria.CustomerId.Value);
            }
            if (criteria.SupplierId.HasValue)
            {
                query.Where(p => p.SupplierId == criteria.SupplierId.Value);
            }
            if (criteria.ItemCode.IsNotEmpty() || criteria.ItemName.IsNotEmpty())
            {
                query.Join<Item>((x, y) => y.Id == x.ItemId)
                     .WhereIf<Item>(criteria.ItemCode.IsNotEmpty(), (x, y) => y.Code.Contains(criteria.ItemCode))
                     .WhereIf<Item>(criteria.ItemName.IsNotEmpty(), (x, y) => y.Name.Contains(criteria.ItemName));
            }
            if (criteria.DeliveryDate.BeginValue.HasValue)
            {
                query.Where(p => p.DeliveryDate >= criteria.DeliveryDate.BeginValue.Value);
            }
            if (criteria.DeliveryDate.EndValue.HasValue)
            {
                query.Where(p => p.DeliveryDate <= criteria.DeliveryDate.EndValue.Value);
            }
            if (criteria.IsFilter)
            {
                query.Where(p => p.State != DeliveryState.Cancel && p.State != DeliveryState.Finished);
            }
            if (criteria.OrderNo.IsNotEmpty())
                query.Where(p => p.OrderNo == criteria.OrderNo);
            if (criteria.WarehouseId.HasValue)
                query.Where(p => p.WarehouseId == criteria.WarehouseId);
            if (criteria.TargetWarehouseId.HasValue)
                query.Where(p => p.TargetWarehouseId == criteria.TargetWarehouseId);
            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询出货计划
        /// </summary>
        /// <param name="productId">物料ID</param>
        /// <param name="itemRevision">物料扩展属性</param>
        /// <param name="customerId">客户ID</param>
        /// <param name="lotNo">批次号</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">过滤关键字</param>
        /// <returns>出货计划</returns>
        public virtual EntityList<DeliveryPlan> GetDeliveryPlans(double productId, string itemRevision,
            double? customerId, string lotNo, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<DeliveryPlan>()
                .Where(x => x.ItemId == productId && x.CustomerId == customerId
                    && (x.State == DeliveryState.Aduited || x.State == DeliveryState.Executing)
                    && x.RequireQty > x.PackagedUnitQty)
                .Where(x => x.ProductBatch == null || x.ProductBatch == lotNo)
                .WhereIf(!keyword.IsNullOrEmpty(), x => x.No.Contains(keyword))
                .WhereIf(!itemRevision.IsNullOrEmpty(), x => x.ItemExtProp == itemRevision);

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询出货计划
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>出货计划</returns>
        public virtual EntityList<DeliveryPlan> GetDeliveryPlans(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<DeliveryPlan>()
                .Where(x => (x.State == DeliveryState.Aduited || x.State == DeliveryState.Executing)
                    && x.RequireQty > x.PackagedUnitQty)
                .WhereIf(!keyword.IsNullOrEmpty(), x => x.No.Contains(keyword));

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取发货计划数据
        /// </summary>
        /// <param name="planIds">发货计划ID集合</param>
        /// <returns>发货计划数据</returns>
        public virtual EntityList<DeliveryPlan> GetDeliveryPlans(List<double> planIds)
        {
            if (planIds == null || planIds.Count == 0)
            {
                return new EntityList<DeliveryPlan>();
            }
            return planIds.SplitContains(sons =>
            {
                return Query<DeliveryPlan>().Where(p => sons.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取发货计划数据
        /// </summary>
        /// <param name="planIds">发货计划ID集合</param>
        /// <returns>发货计划数据</returns>
        public virtual EntityList<DeliveryPlan> GetDeliveryPlans(List<string> planNos)
        {
            if (planNos == null || planNos.Count == 0)
            {
                return new EntityList<DeliveryPlan>();
            }
            return planNos.SplitContains(sons =>
            {
                return Query<DeliveryPlan>().Where(p => sons.Contains(p.No) || sons.Contains(p.OrderNo)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 发货计划审核操作
        /// </summary>
        /// <param name="planIds">发货计划ID集合</param>
        public virtual void AuditDeliveryPlans(List<double> planIds)
        {
            var plans = GetDeliveryPlans(planIds);
            if (plans.Any(p => p.State != DeliveryState.Created))
            {
                throw new ValidationException("发货计划必须是创建状态,才能进行审核操作!".L10N());
            }
            using (var tran = DB.TransactionScope(ShipPlanEntityDataProvider.ConnectionStringName))
            {
                plans.ForEach(p =>
                {
                    p.CreateQty = p.NoCreateQty;
                    p.State = DeliveryState.Aduited;
                });

                RF.Save(plans);

                //自动将未填写仓库的发货计划进行分配仓库操作
                List<double> noPlanIds = plans.Where(p => p.WarehouseId == null).Select(p => p.Id).ToList();
                if (noPlanIds.Any())
                {
                    AssignWarehousePlans(noPlanIds);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 发货计划分配仓库操作
        /// </summary>
        /// <param name="planIds">发货计划ID集合</param>
        public virtual void AssignWarehousePlans(List<double> planIds)
        {
            var plans = GetDeliveryPlans(planIds);
            if (plans.Any(p => p.State != DeliveryState.Created && p.State != DeliveryState.Aduited && p.State != DeliveryState.Executing))
            {
                throw new ValidationException("发货计划必须是创建、审核、执行中的状态,才能进行分配仓库操作!".L10N());
            }
            var itemIds = plans.Select(p => p.ItemId).Distinct().ToList();
            var items = RT.Service.Resolve<ItemController>().GetItemList(itemIds);
            bool successFlag = false;
            using (var tran = DB.TransactionScope(ShipPlanEntityDataProvider.ConnectionStringName))
            {
                plans.ForEach(p =>
                {
                    var item = items.FirstOrDefault(t => t.Id == p.ItemId);
                    var rule = RT.Service.Resolve<AssignWarehouseRuleController>().GetAssignWarehouseRuleData(p.OrderType, item, p.CustomerId, p.SupplierId, p.EnterpriseId, p.ResourceId);
                    if (rule != null)
                    {
                        p.WarehouseId = rule?.WarehouseId;
                        successFlag = true;
                    }
                });
                if (!successFlag)
                    throw new ValidationException("仓库分配失败，请检查分配仓库规则".L10N());
                RF.Save(plans);
                tran.Complete();
            }
        }

        /// <summary>
        /// 发货计划强制完成操作
        /// </summary>
        /// <param name="planIds">发货计划ID集合</param>
        public virtual void ForceCompleteDeliveryPlans(List<double> planIds)
        {
            var plans = GetDeliveryPlans(planIds);
            if (plans.Any(p => p.State != DeliveryState.Aduited && p.State != DeliveryState.Executing))
            {
                throw new ValidationException("发货计划必须是审核、执行中状态,才能进行强制完成操作!".L10N());
            }
            Dictionary<double, decimal> planDic = new Dictionary<double, decimal>();

            using (var tran = DB.TransactionScope(ShipPlanEntityDataProvider.ConnectionStringName))
            {
                plans.ForEach(p =>
                {
                    planDic.Add(p.Id, p.NoCreateQty);
                    p.CancelQty += p.NoCreateQty;
                    p.NoCreateQty = 0;

                    if (p.CancelQty == p.RequireQty)
                    {
                        p.State = DeliveryState.Cancel;
                    }

                    if (p.CancelQty < p.RequireQty)
                    {
                        p.State = DeliveryState.Finished;
                    }
                    p.CreateQty = 0;
                });
                RF.Save(plans);
                tran.Complete();
            }
        }

        /// <summary>
        /// 获取发运订单参数
        /// </summary>
        /// <param name="selPlanDatas">发货计划</param>
        /// <param name="rule">合并创单规则</param>
        /// <param name="soParams">发运订单参数</param>
        private void GetSoParams(List<DeliveryPlan> selPlanDatas, MergeCreateRule rule, List<SoParam> soParams)
        {


            selPlanDatas.ForEach(p => p.DeliveryDate = p.DeliveryDate.Value.Date);
            var planGroupyDatas = selPlanDatas.GroupBy(p => new
            {
                p.OrderType,
                WarehouseId = p.WarehouseId.Value,
                p.StorerCode,
                p.EnterpriseId,
                p.CustomerId,
                p.SupplierId,
                DeliveryDate = rule.IsSameDeliveryDate ? p.DeliveryDate : null,
                p.No,
                OrderNo = rule.IsSameOrderNo ? p.OrderNo : string.Empty,
                OrderLineNo = rule.IsSameOrderNo ? p.OrderLineNo : string.Empty,
                p.AllotModel,
                p.TargetWarehouseId,
            });
            soParams.AddRange(planGroupyDatas.Distinct().Select(p => new SoParam
            {
                OrderType = (int)p.Key.OrderType,
                WarehouseId = p.Key.WarehouseId,
                StorerCode = p.Key.StorerCode,
                CustomerId = p.Key.CustomerId,
                SupplierId = p.Key.SupplierId,
                EnterpriseId = p.Key.EnterpriseId,
                DeliveryDate = p.Key.DeliveryDate,
                SaleOrderNo = p.Key.No,
                OrderNo = p.Key.OrderNo,
                IsSameDeliveryDate = rule.IsSameDeliveryDate,
                IsSameOrderNo = rule.IsSameOrderNo,
                IsSameNo = rule.IsSameNo,
                AllotMode = (int?)p.Key.AllotModel,
                TargetWhId = p.Key.TargetWarehouseId,
                SoRequireDtlParams = selPlanDatas.Where(t => t.WarehouseId == p.Key.WarehouseId && t.StorerCode == p.Key.StorerCode
                && t.CustomerId == p.Key.CustomerId && t.SupplierId == p.Key.SupplierId && t.EnterpriseId == p.Key.EnterpriseId).Select(t => new SoRequireDtlParam
                {
                    ItemId = t.ItemId,
                    RequireQty = t.CreateQty,
                    ProjectNo = t.ProjectNo,
                    TaskNo = t.TaskNo,
                    LotCode = t.LotCode,
                    ProductBatch = t.ProductBatch,
                    DeliveryDate = t.DeliveryDate,
                    SoRequireNo = t.No,
                    SoRequireDtlNo = t.LineNo.ToString(),
                    OrderNo = t.OrderNo,
                    OrderLineNo = t.OrderLineNo,
                    ItemExtProp = t.ItemExtProp,
                    ItemExtPropName = t.ItemExtPropName,
                    ShipPlanId = t.Id,
                    ErpDetailId = t.ErpDetailId,
                    ErpOrderId = t.ErpOrderId,
                    ErpOrganizationName = t.ErpOrganizationName,
                    ErpOrgName = t.ErpOrgName,
                    ErpWoNo = t.ErpWoNo,
                    ScheduleShipDate = t.ScheduleShipDate,
                }).ToList(),
            }).ToList());
        }

        /// <summary>
        /// 获取库存调拨参数
        /// </summary>
        /// <param name="selPlanDatas">发货计划</param>
        /// <param name="rule">合并创单规则</param>
        /// <param name="invAllotParams">库存调拨参数</param>
        /// <exception cref="ValidationException">异常信息</exception>
        private void GetInvAllotParams(List<DeliveryPlan> selPlanDatas, MergeCreateRule rule, List<InvAllotParam> invAllotParams)
        {
            if (selPlanDatas.Any(f => f.WarehouseId == f.TargetWarehouseId))
                throw new ValidationException("发货仓库和收货仓库不能相同".L10N());

            //单据类型为直接调拨或两步调拨时创建库存调拨单
            var planAllotGroupyDatas = selPlanDatas.GroupBy(p => new
            {
                p.OrderType,
                WarehouseId = p.WarehouseId.Value,
                TargetWarehouseId = p.TargetWarehouseId.Value,
                p.StorerCode,
                p.LotCode,
                p.ProductBatch,
                DeliveryDate = rule.IsSameDeliveryDate ? p.DeliveryDate : null,
                No =  p.No,
                OrderNo = rule.IsSameOrderNo ? p.OrderNo : string.Empty
            });
            invAllotParams.AddRange(planAllotGroupyDatas.Distinct().Select(p => new InvAllotParam
            {
                OrderType = (int)p.Key.OrderType,
                WarehouseId = p.Key.WarehouseId,
                TargetWarehouseId = p.Key.TargetWarehouseId,
                StorerCode = p.Key.StorerCode,
                LotCode = p.Key.LotCode,
                ProductBatch = p.Key.ProductBatch,
                DeliveryDate = p.Key.DeliveryDate,
                SaleOrderNo = p.Key.No,
                OrderNo = p.Key.OrderNo,
                IsSameDeliveryDate = rule.IsSameDeliveryDate,
                IsSameOrderNo = rule.IsSameOrderNo,
                IsSameNo = rule.IsSameNo,
                InvAllotReqDtlParams = selPlanDatas.Where(t => t.WarehouseId == p.Key.WarehouseId && t.TargetWarehouseId == p.Key.TargetWarehouseId && t.StorerCode == p.Key.StorerCode && t.LotCode == p.Key.LotCode && t.ProductBatch == p.Key.ProductBatch).Select(t => new InvAllotReqDtlParam
                {
                    ItemId = t.ItemId,
                    RequireQty = t.CreateQty,
                    ProjectNo = t.ProjectNo,
                    TaskNo = t.TaskNo,
                    LotCode = t.LotCode,
                    ProductBatch = t.ProductBatch,
                    DeliveryDate = t.DeliveryDate,
                    SoRequireNo = t.No,
                    SoRequireDtlNo = t.LineNo.ToString(),
                    OrderNo = t.OrderNo,
                    ItemExtProp = t.ItemExtProp,
                    ItemExtPropName = t.ItemExtPropName,
                    ShipPlanId = t.Id,
                }).ToList(),
            }).ToList());
        }

        /// <summary>
        /// 创建发运订单
        /// </summary>
        /// <param name="planIds">备料计划ID</param>
        /// <param name="isStockPlan">是否是备料计划创建的发运订单</param>
        /// <exception cref="ValidationException"></exception>
        public virtual void CreateSoByDeliveryPlans(List<double> planIds, bool isStockPlan = false)
        {
            List<double> billIds = new List<double>();
            List<double> invAllocateIds = new List<double>();
            using (var tran = DB.TransactionScope(ShipPlanEntityDataProvider.ConnectionStringName))
            {
                //防止并发,将数据行锁定.
                DB.Update<DeliveryPlan>().Where(p => planIds.Contains(p.Id)).Execute();
                EntityList<DeliveryPlan> plans = GetDeliveryPlans(planIds);
                if (plans.Any(p => p.State != DeliveryState.Aduited && p.State != DeliveryState.Executing))
                {
                    throw new ValidationException("发货计划必须是审核、执行中状态,才能进行创建发运订单操作!".L10N());
                }
                if (plans.Any(p => p.WarehouseId == null))
                {
                    throw new ValidationException("发货计划的发货仓库不能为空,才能进行创建发运订单操作!".L10N());
                }
                if (plans.Any(p => p.CreateQty <= 0 || p.CreateQty > p.NoCreateQty))
                {
                    throw new ValidationException("发货计划必须需创单数量大于0且小于等于未建单数,才能进行创建发运订单操作!".L10N());
                }
                List<OrderType> orderTypes = plans.Select(p => p.OrderType).Distinct().ToList();
                var mergeRules = GetMergeCreateRules(orderTypes);
                if (!mergeRules.Any())
                {
                    throw new ValidationException("合并创单规则未初始化数据".L10N());
                }
                List<SoParam> soParams = new List<SoParam>();
                List<InvAllotParam> invAllotParams = new List<InvAllotParam>();

                foreach (var rule in mergeRules)
                {
                    var selPlanDatas = plans.Where(p => p.OrderType == rule.OrderType).ToList();
                    if (selPlanDatas.Any())
                    {
                        ////单据类型不是直接调拨和两步调拨则创建发运单
                        if (rule.OrderType != OrderType.DirectAllocate && rule.OrderType != OrderType.TwoAllocate)
                        {
                            ////获取发运订单参数
                            GetSoParams(selPlanDatas, rule, soParams);
                        }
                        else
                        {
                            ////获取库存调拨参数
                            GetInvAllotParams(selPlanDatas, rule, invAllotParams);
                        }
                    }
                }

                if (soParams.Any())
                {
                    List<double> itemIds = new List<double>();
                    soParams.ForEach(p =>
                    {
                        p.IsStockPlan = isStockPlan;
                        itemIds.AddRange(p.SoRequireDtlParams.Select(q => q.ItemId).Distinct().ToList());
                    });

                    billIds.AddRange(RT.Service.Resolve<IShippingOrder>().CreateSoByDeliveryPlan(soParams));
                }
                if (invAllotParams.Any())
                {
                    invAllocateIds = RT.Service.Resolve<IAllocate>().CreateInvAllocateByShipPlan(invAllotParams);
                }
                plans.ForEach(p =>
                {
                    if (p.State == DeliveryState.Aduited)
                    {
                        p.State = DeliveryState.Executing;
                    }
                    //RT.Service.Resolve<StockOrderDetailService>().
                    //订单明细状态
                    p.NoCreateQty -= p.CreateQty;
                    if (p.NoCreateQty < 0)
                        p.NoCreateQty = 0;
                    p.CreateQty = p.NoCreateQty;
                });

                RF.Save(plans);
                //更新备料单状态
                tran.Complete();
            }

            //审核发运单
            if (billIds.Any())
            {
                RT.Service.Resolve<IShippingOrder>().AuditShippingOrderData(billIds);
            }

            //审核调拨单
            if (invAllocateIds.Any())
            {
                RT.Service.Resolve<IAllocate>().AuditInvAllocateData(invAllocateIds);
            }
        }

        /// <summary>
        /// 更新发货计划状态
        /// </summary>
        /// <param name="deliveryPlans">发货计划</param>
        public virtual void UpdateDeliveryPlanState(EntityList<DeliveryPlan> deliveryPlans)
        {
            deliveryPlans.ForEach(p =>
            {
                //1）当需求数=未建单数时，状态=审核
                if (p.RequireQty == p.NoCreateQty)
                {
                    p.State = DeliveryState.Aduited;
                }
                //2）当需求数＞未建单数、未建单数＞0时，状态=执行中
                if (p.RequireQty > p.NoCreateQty && p.NoCreateQty > 0)
                {
                    p.State = DeliveryState.Executing;
                }
                //3）当未建单数<=0，发货数＞0时，状态=已完成
                if (p.NoCreateQty <= 0 && p.DeliveryQty > 0)
                {
                    p.State = DeliveryState.Finished;
                }
                //4）当未建单数 == 0，发货数 == 0时，状态 = 取消
                if (p.NoCreateQty == 0 && p.DeliveryQty == 0)
                {
                    p.State = DeliveryState.Cancel;
                }
            });
        }

        /// <summary>
        /// 获取合并创单规则数据
        /// </summary>
        /// <param name="orderTypes">orderTypes</param>
        /// <returns></returns>
        public virtual EntityList<MergeCreateRule> GetMergeCreateRules(List<OrderType> orderTypes)
        {
            return Query<MergeCreateRule>().Where(p => orderTypes.Contains(p.OrderType)).ToList();
        }

        /// <summary>
        /// 初始化合并创单规则
        /// </summary>
        public virtual void InitMergeCreateRule()
        {
            List<OrderType> orderTypes = new List<OrderType>
            {
                OrderType.SaleOut,
                OrderType.WorkFeed,
                OrderType.OutWorkFeed,
                OrderType.OutWorkFeedUse,
                OrderType.OutAllotReturn,
                OrderType.OtherOut,
                OrderType.SupplierReturn,
                OrderType.DirectAllocate,
                OrderType.TwoAllocate,
                 OrderType.WhTransferOut
            };
            var rule = GetMergeCreateRules(orderTypes);
            if (rule.Any())
            {                
                return;
            }

            using (var tran = DB.TransactionScope(ShipPlanEntityDataProvider.ConnectionStringName))
            {
                string strDesc = "同仓库、同货主、同收货对象、同调拨模式、同目标仓库".L10N();
                string allotDesc = "同仓库、同货主、同收货对象、同物料同指定条件".L10N();
                EntityList<MergeCreateRule> createRules = new EntityList<MergeCreateRule>();
                orderTypes.ForEach(type =>
                {
                    createRules.Add(new MergeCreateRule
                    {
                        OrderType = type,
                        IsSameDeliveryDate = false,
                        IsSameNo = false,
                        IsSameOrderNo = false,
                        DefaultDesc = type != OrderType.DirectAllocate && type != OrderType.TwoAllocate ? strDesc : allotDesc,
                    });
                });

                RF.Save(createRules);

                tran.Complete();
            }
        }

        /// <summary>
        /// 根据出货计划行号获取实体
        /// </summary>
        /// <param name="deliveryPlanLine">出货计划行</param>
        public virtual DeliveryPlan GetDeliveryPlan(string deliveryPlanLine)
        {
            if (deliveryPlanLine == null)
            {
                return null;
            }

            if (deliveryPlanLine.IndexOf('-') < 0)
            {
                return new DeliveryPlan();
            }

            var No = deliveryPlanLine.Split('-')[0];
            var lineNo = Convert.ToInt32(deliveryPlanLine.Split('-')[1]);
            return Query<DeliveryPlan>().Where(n => n.No == No && n.LineNo == lineNo).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取出货计划行列表
        /// </summary>
        /// <param name="deliveryPlanNo">出货计划单号</param>
        /// <returns>出货计划行列表</returns>
        public virtual EntityList<DeliveryPlan> GetDeliveryPlanList(string deliveryPlanNo)
        {
            return Query<DeliveryPlan>().Where(n => n.No == deliveryPlanNo).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        #region 齐套分析
        /// <summary>
        /// 备料计划齐套分析
        /// </summary>
        /// <param name="ids">备料计划id集合</param>
        /// <param name="isBuyOnWay">是否考虑在途</param>
        /// <param name="isMakeOnWay">是否考虑再制</param>
        public virtual StockkittingViewData StockPlanAnalys(List<double> ids, bool isBuyOnWay, bool isMakeOnWay)
        {
            StockkittingViewData stockkittingViewData = new StockkittingViewData();

            var stockPlans = GetDeliveryPlans(ids);
            var itemids = stockPlans.Select(p => p.ItemId).Distinct().ToList();
            var warehouseIds = stockPlans.Select(p => p.WarehouseId.Value).ToList();
            List<string> itemNames = new List<string>();
            CheckPlans(stockPlans, warehouseIds);
            var allOnhandDatas = RT.Service.Resolve<IKitting>().SetOnhandList(warehouseIds, itemids, isBuyOnWay, isMakeOnWay, null);
            stockPlans.ForEach(p =>
            {
                DateTime? lastTime = p.DeliveryDate;
                var storeCode = p.StorerCode.IsNullOrEmpty() ? "*" : p.StorerCode;
                var projectNo = p.ProjectNo.IsNullOrEmpty() ? "*" : p.ProjectNo;
                var taskNo = p.TaskNo.IsNullOrEmpty() ? "*" : p.TaskNo;
                List<double> assignids = new List<double>();
                var onhandDatas = GetOnHandData(allOnhandDatas, storeCode, projectNo, taskNo, lastTime);
                if (onhandDatas.Count == 0)
                {
                    p.KittingType = KittingType.Scarce;
                }
                var newOnhandDatas = SelectOnhandDatasByItemExtPorp(p.ItemExtPropName, onhandDatas);
                var onhandIds = newOnhandDatas.Select(f => f.Id).ToList();
                //由于集合得数据包含了采购和在制，利用把集合里面的数据按顺序分配扣减，达到有序（库存-在途-在制）分配给每个明细的目的
                var itemOnhands = onhandDatas.Where(q => onhandIds.Contains(q.Id) && !assignids.Contains(q.Id) && q.ItemId == p.ItemId && q.AvailableQty > 0 && (q.QtyFrom == 0 || q.FinishDate <= lastTime) && q.WarehouseId == p.WarehouseId).OrderBy(q => q.WarehouseId).ThenBy(q => q.LotCode).ThenBy(q => q.QtyFrom).ToList();
                //当前的可用数汇总，在运行期间，集合可用数会被扣减
                var nowSumQty = itemOnhands.Sum(q => q.AvailableQty);
                if (nowSumQty >= p.CreateQty)
                {//当前可用数足够
                    decimal needQty = p.CreateQty;//需要用掉集合中的数量
                    itemOnhands.ForEach(f =>
                    {
                        if (needQty > 0)
                        {
                            var actualQty = needQty;//本次实际能给到的数量actualQty
                            if (f.AvailableQty >= needQty)
                            {
                                needQty = 0;
                            }
                            else
                            {
                                needQty = needQty - f.AvailableQty;
                                actualQty = f.AvailableQty;
                            }
                            //扣减集合中的可用数，差额留给下一条用，0下一条用不了
                            f.AvailableQty = f.AvailableQty - actualQty;
                            if (needQty == 0)
                            {//给完数量后，判断最后使用的这一条是属于库存/在途/在制
                                if (f.QtyFrom == 0)
                                {
                                    p.KittingType = KittingType.StoreFull;
                                }
                                if (f.QtyFrom == 2)
                                {//遇到1，则代表上面的库存已经不够用了，这里不用考虑是否选了在途，集合中有该数据就是代表选了
                                    p.KittingType = KittingType.PurchareFull;
                                }
                                else if (f.QtyFrom == 3)
                                {//同理
                                    p.KittingType = KittingType.MakingFull;
                                }
                            }
                            if (f.AvailableQty == 0)
                            {
                                //说明该库存已经分配完了过滤掉分配完的库存
                                assignids.Add(f.Id);
                            }
                            var assignItem = new StockPlanAssignViewModel()
                            {
                                StockPlanId = p.Id,
                                Qty = actualQty,
                                WarehouseId = p.WarehouseId,
                                LotCode = f.LotCode,
                                WarehouseCode = p.Warehouse.Code,
                                WarehouseName = p.Warehouse.Name,
                            };
                            stockkittingViewData.stockPlanAssigns.Add(assignItem);
                        }

                    });
                }
                else
                {
                    itemNames.Add(p.ItemCode);
                    p.KittingType = KittingType.Scarce;
                }
            });
            stockkittingViewData.stockPlans.AddRange(stockPlans);
            return stockkittingViewData;
        }

        /// <summary>
        /// 根据物料扩展属性筛选数据
        /// </summary>
        /// <param name="itemPropName">计划的扩展属性</param>
        /// <param name="analysOnhands">库存数据</param>
        private List<AnalysOnhandData> SelectOnhandDatasByItemExtPorp(string itemPropName, List<AnalysOnhandData> analysOnhands)
        {
            //如果扩展属性为空 则为全匹配
            if (itemPropName.IsNullOrEmpty())
                return analysOnhands.ToList();
            List<AnalysOnhandData> newAnalysOnhands = new List<AnalysOnhandData>();
            var propArray = itemPropName.Split(';');
            analysOnhands.Where(f => f.ItemExtPropName.IsNotEmpty()).ForEach(p =>
            {
                var onhandPropArray = p.ItemExtPropName.Split(';');
                //有物料扩展属性的，物料扩展属性匹配需要一模一样，个数、属性值
                if (onhandPropArray.Length == propArray.Length)
                {
                    bool flag = true;
                    propArray.ForEach(f =>
                    {
                        if (!onhandPropArray.Contains(f))
                        {
                            flag = false;
                        }
                    });
                    if (flag)
                    {
                        newAnalysOnhands.Add(p);
                    }

                }
            });
            return newAnalysOnhands;
        }

        /// <summary>
        /// 库存数据
        /// </summary>
        /// <param name="onhandDatas">库存数据</param>
        /// <param name="StoreCode">货主</param>
        /// <param name="ProjectNo">项目号</param>
        /// <param name="TaskNo">任务号</param>
        /// <param name="lastTime">最后时间</param>
        /// <returns></returns>
        private List<AnalysOnhandData> GetOnHandData(List<AnalysOnhandData> onhandDatas, string StoreCode, string ProjectNo, string TaskNo, DateTime? lastTime)
        {
            List<AnalysOnhandData> newAnalysOnhandDatas = new List<AnalysOnhandData>();
            onhandDatas.ForEach(p =>
            {
                switch (p.QtyFrom)
                {
                    //在库 没有时间
                    case 0:
                        if (CheckOnHandByAsnOrHand(p, StoreCode, ProjectNo, TaskNo, null))
                        {
                            newAnalysOnhandDatas.Add(p);
                        }
                        break;
                    //在库
                    case 2:
                        if (CheckOnHandByAsnOrHand(p, StoreCode, ProjectNo, TaskNo, lastTime))
                        {
                            newAnalysOnhandDatas.Add(p);
                        }
                        break;
                    case 3:
                        //在制 暂时没有货主只需要判断时间
                        if (p.FinishDate <= lastTime)
                        {
                            newAnalysOnhandDatas.Add(p);
                        }
                        break;
                }
            });
            return newAnalysOnhandDatas;
        }

        /// <summary>
        /// 判断库存或ASN在途库存是否符合 在途库存要判断时间 在库库存不需要判断时间
        /// </summary>
        /// <param name="hand">库存数据</param>
        /// <param name="StoreCode">货主</param>
        /// <param name="ProjectNo">项目</param>
        /// <param name="TaskNo">任务</param>
        /// <param name="lastTime">最后时间</param>
        /// <returns></returns>
        private bool CheckOnHandByAsnOrHand(AnalysOnhandData hand, string StoreCode, string ProjectNo, string TaskNo, DateTime? lastTime)
        {
            if (lastTime.HasValue)
            {
                //如果有时间
                if (hand.StoreCode == StoreCode && (hand.ProjectNo == ProjectNo || hand.ProjectNo == "*") && (hand.TaskNo == TaskNo && hand.TaskNo == "*") && hand.FinishDate <= lastTime)
                {
                    return true;
                }
            }
            else
            {
                if (hand.StoreCode == StoreCode && (hand.ProjectNo == ProjectNo || hand.ProjectNo == "*") && (hand.TaskNo == TaskNo && hand.TaskNo == "*"))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="stockPlans">备料计划</param>
        /// <param name="whIds">仓库IDlist</param>
        /// <exception cref="ValidationException"></exception>
        private void CheckPlans(EntityList<DeliveryPlan> stockPlans, List<double> whIds)
        {
            if (stockPlans.Any(p => p.CreateQty == 0))
            {
                throw new ValidationException("发货计划需创单数不能为0".L10N());
            }
            if (stockPlans.Any(p => p.State == DeliveryState.Created || p.State == DeliveryState.Cancel || p.State == DeliveryState.Finished))
            {
                throw new ValidationException("单据不能是创建、取消、完成状态".L10N());
            }
            var whs = RT.Service.Resolve<WarehouseController>().GetWarehouses(whIds);
            if (whs.Any(p => p.IsFrozen || p.State == State.Disable))
            {
                throw new ValidationException("所选单据存在冻结或禁用的发货仓库".L10N());
            }
        }

        #endregion
    }
}