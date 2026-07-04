using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.WorkOrders;
using SIE.EventMessages.MES.WorkOrders.Models;
using SIE.Items;
using SIE.Items.Items;
using SIE.Items.ProductModels;
using SIE.LES.Commons;
using SIE.LES.Interfaces;
using SIE.LES.LinesideWarehouses;
using SIE.LES.PrepareItems.Models;
using SIE.LES.StockOrders;
using SIE.LES.StockOrders.Configs;
using SIE.LES.StockOrders.Dao;
using SIE.LES.StockOrders.Models;
using SIE.LES.StockOrders.Service;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SIE.LES
{
    /// <summary>
    /// 推式物料备料
    /// </summary>
    public class PrepareItemPushController : DomainController
    {
        #region 控制器和服务
        /// <summary>
        /// 备料模式控制器
        /// </summary>
        private readonly PrepareItemController PrepareItemCtrl
            = RT.Service.Resolve<PrepareItemController>();
        /// <summary>
        /// 工单模式控制器
        /// </summary>
        private readonly WoInfoForLesController WoInfoForLesController
            = RT.Service.Resolve<WoInfoForLesController>();
        /// <summary>
        /// 产品机型控制器
        /// </summary>
        private readonly ProductModelController productModelCtrl
            = RT.Service.Resolve<ProductModelController>();
        /// <summary>
        /// 物料控制器
        /// </summary>
        private readonly ItemController itemController
            = RT.Service.Resolve<ItemController>();
        /// <summary>
        /// 生产资源控制器
        /// </summary>
        private readonly WipResourceController WipResourceCtrl
            = RT.Service.Resolve<WipResourceController>();
        /// <summary>
        /// 获取产线线边仓控制器
        /// </summary>
        private readonly LinesideWarehouseController linesideWarehouseCtrl
            = RT.Service.Resolve<LinesideWarehouseController>();

        /// <summary>
        /// 物料扩展控制器
        /// </summary>
        private readonly BaseItemExtController baseItemExtCtrl
            = RT.Service.Resolve<BaseItemExtController>();

        /// <summary>
        /// 备料单控制器
        /// </summary>
        private readonly StockOrderService stockOrderService
            = RT.Service.Resolve<StockOrderService>();
        private Dictionary<double, ProductModel> productModelDic;
        private Dictionary<double, List<ProductModelLineCapacity>> modelLineCapacitiesDic;
        private Dictionary<double, LinesideWarehouse> linesideWarehousesDic;

        #endregion

        /// <summary>
        /// 生成备料单（推式）业务逻辑
        /// </summary>
        public virtual PrepareItemPushResult CreateStockOrder()
        {
            PrepareItemPushResult prepareItemPushResult = new PrepareItemPushResult();

            //加载数据
            //获取备料模式推式列表
            var prepareItemPushs = PrepareItemCtrl.GetPrepareItemPushs();
            if (!prepareItemPushs.Any())
            {
                return prepareItemPushResult;
            }

            //转换拉式备料模式数据为运行所需要的格式
            var prepareItemDatas = GetPrepareItemDatas(prepareItemPushs);

            // 累计匹配（XXX）（资源+物料+触发方式）的数据
            prepareItemPushResult.DataCount = prepareItemDatas.Count;

            //生产资源列表
            List<double?> wipResourceNullIds = prepareItemPushs.Select(x => x.WipResourceId).Distinct().ToList();

            //获取工单信息列表
            var woInfoForLesList = WoInfoForLesController.GetWoInfoForLes(wipResourceNullIds);

            //无工单返回 false，即无数据要计算
            if (!woInfoForLesList.Any())
            {
                return prepareItemPushResult;
            }

            //共有（XXX）个发放的工单
            prepareItemPushResult.WoCount = woInfoForLesList.Count;

            var woIds = woInfoForLesList.Select(x => x.Id).Distinct().ToList();

            //获取所有备料需求明细
            var stockOrderDetails = RT.Service.Resolve<StockOrderDetailDao>()
               .GetPushStockOrderDetails(woIds);

            GetCapacities(woInfoForLesList);

            //产线线边仓维护列表
            var linesideWarehouses = linesideWarehouseCtrl.GetLinesideWarehouses();
            linesideWarehousesDic = linesideWarehouses.Where(p => p.WipResouceId != null)
              .GroupBy(p => (double)p.WipResouceId)
              .ToDictionary(p => p.Key, p => p.FirstOrDefault());

            //统一以数据库时间为准
            var dbDateTime = RF.Find<StockOrder>().GetDbTime();

            //获取调度触发的备料单状态的配置项值
            var configValue = stockOrderService.GetSchedulingTriggeredStatusConfigValue();

            var stockState = StockState.Created;
            if (configValue != null && configValue.TriggeredStatus == TriggeredStatus.Submitted)
            {
                stockState = StockState.Submitted;
            }

            //业务逻辑
            List<WoInfoForLes> woInfoForLes = ComputeCanPrepareWo(woInfoForLesList, stockOrderDetails, prepareItemDatas, dbDateTime);
            if (!woInfoForLes.Any())
            {
                //没有符合备料的工单和工单BOM
                return prepareItemPushResult;
            }

            //共有（XXX）个（工单+物料+触发方式）满足触发条件
            prepareItemPushResult.FitDataCount = woInfoForLes.SelectMany(x => x.WoBomInfos).Count();

            //生成备料单
            CreateStockOrderData(woInfoForLes, prepareItemPushResult, prepareItemDatas, stockState, stockOrderDetails);

            return prepareItemPushResult;
        }


        /// <summary>
        /// 获取产能数量
        /// </summary>
        private void GetCapacities(List<WoInfoForLes> woInfoForLesList)
        {
            List<double> productIds = woInfoForLesList.Select(x => x.ProductId).Distinct().ToList();

            //物料与产品机型字典
            productModelDic = itemController.GetItemIdToModels(productIds);

            //产线产能列表
            List<double> productModelIds = new List<double>();
            var productModelLines = productModelDic.Values.Distinct();
            foreach (var i in productModelLines)
            {
                if (i != null)
                {
                    productModelIds.Add(i.Id);
                }
            }

            var modelLineCapacities = productModelCtrl.GetProductModelLineCapacities(productModelIds);
            modelLineCapacitiesDic = modelLineCapacities
              .GroupBy(p => p.ProductModelId)
              .ToDictionary(p => p.Key, p => p.ToList());
        }

        /// <summary>
        /// 转换备料模式数据为运行所需要的格式
        /// </summary>        
        /// <param name="prepareItemPushs">备料模式</param>
        private List<PrepareItemPushData> GetPrepareItemDatas(EntityList<PrepareItemPush> prepareItemPushs)
        {
            List<PrepareItemPushData> prepareItemDatas = new List<PrepareItemPushData>();
            foreach (var itemPush in prepareItemPushs.Where(x => x.ItemId.HasValue && x.WipResourceId.HasValue))
            {
                //仓库加物料及物料扩展属性已经有数据，则跳过
                if (prepareItemDatas.Any(x => x.WipResourceId == itemPush.WipResourceId
                    && x.ItemId == itemPush.ItemId.Value && x.ItemExtProp == itemPush.ItemExtProp))
                {
                    continue;
                }
                PrepareItemPushData prepareItemData = new PrepareItemPushData();
                prepareItemData.WipResourceId = itemPush.WipResourceId.HasValue ? itemPush.WipResourceId.Value : 0;
                prepareItemData.FixedQuantity = itemPush.FixedQuantity;
                prepareItemData.DemandType = itemPush.DemandType.HasValue ? itemPush.DemandType.Value : DemandMode.ManualFillIn;
                prepareItemData.TriggerType = itemPush.TriggerType.HasValue ? itemPush.TriggerType.Value : TriggerMode.BelowSafe;
                prepareItemData.ItemId = itemPush.ItemId.Value;
                prepareItemData.ItemExtProp = itemPush.ItemExtProp;
                prepareItemData.ItemExtPropName = itemPush.ItemExtPropName;
                prepareItemData.AdvanceHours = itemPush.AdvanceHours;
                prepareItemData.SatisfactionTime = itemPush.SatisfactionTime;
                prepareItemData.PreparationTime = itemPush.PreparationTime;
                prepareItemDatas.Add(prepareItemData);
            }

            //按分类维护的拉式备料模式
            var itemCategoryIds = prepareItemPushs
                .Where(x => x.ItemId == null && x.ItemCategoryId != null)
                .Select(x => x.ItemCategoryId)
                .Distinct()
                .ToList();

            //获取分类下面所有物料信息
            var itemCategoryRelations = itemController
                .GetItemCategoryRelations(itemCategoryIds, consumeMode: ConsumeMode.Push);

            foreach (var itemPush in prepareItemPushs
                .Where(x => x.ItemId == null && x.ItemCategoryId != null && x.WipResourceId != null))
            {
                foreach (var itemId in itemCategoryRelations
                    .Where(x => x.ItemCategoryId == itemPush.ItemCategoryId)
                    .Select(x => x.ItemId))
                {
                    //仓库加物料及物料扩展属性已经有数据，则跳过
                    if (prepareItemDatas.Any(x => x.WipResourceId == itemPush.WipResourceId
                        && x.ItemId == itemId))
                    {
                        continue;
                    }

                    PrepareItemPushData prepareItemData = new PrepareItemPushData();
                    prepareItemData.WipResourceId = itemPush.WipResourceId.Value;
                    prepareItemData.FixedQuantity = itemPush.FixedQuantity;
                    prepareItemData.DemandType = itemPush.DemandType.HasValue ? itemPush.DemandType.Value : DemandMode.ManualFillIn;
                    prepareItemData.TriggerType = itemPush.TriggerType.HasValue ? itemPush.TriggerType.Value : TriggerMode.BelowSafe;
                    prepareItemData.ItemId = itemId;
                    prepareItemData.ItemExtProp = itemPush.ItemExtProp;
                    prepareItemData.ItemExtPropName = itemPush.ItemExtPropName;
                    prepareItemData.AdvanceHours = itemPush.AdvanceHours;
                    prepareItemData.SatisfactionTime = itemPush.SatisfactionTime;
                    prepareItemData.PreparationTime = itemPush.PreparationTime;
                    prepareItemDatas.Add(prepareItemData);
                }
            }

            return prepareItemDatas;
        }

        /// <summary>
        /// 计算需要备料的工单和工单BOM
        /// </summary>
        public virtual List<WoInfoForLes> ComputeCanPrepareWo(List<WoInfoForLes> woInfoForLesList,
            EntityList<StockOrderDetail> stockOrderDetails,
            List<PrepareItemPushData> prepareItemDatas,
            DateTime dbDateTime)
        {
            try
            {
                //保存工单信息
                var saveWoInfoForLesList = new List<WoInfoForLes>();

                foreach (var woInfoForLes in woInfoForLesList)
                {
                    //生产节拍
                    var meter = GetMeter(woInfoForLes);

                    List<WoBomInfoForLes> saveWoBomInfoForList = new List<WoBomInfoForLes>();

                    //获取工单BOM （物料消耗类型为推式物料）
                    var woBomInfos = woInfoForLes.WoBomInfos.Where(x => x.ConsumeMode == 1).ToList();

                    foreach (WoBomInfoForLes woBomInfo in woBomInfos)
                    {
                        //按（工单+物料）校验，是否存在未接收的备料单的数据，存在则该物料不生成推式备料
                        if (stockOrderDetails.Any(x => x.WorkOrderId == woInfoForLes.Id
                                && woBomInfo.ItemId == x.ItemId
                                && woBomInfo.ItemExtProp == x.ItemExtProp
                                && (x.StockState == StockState.Audit
                                    || x.StockState == StockState.Submitted
                                    || x.StockState == StockState.PickStocking
                                    || x.StockState == StockState.TobeReceive
                                    || x.StockState == StockState.Issued
                                    )))
                        {
                            continue;
                        }

                        //获取备料模式-推式 (工单）
                        var prepareItemPush = prepareItemDatas
                            .FirstOrDefault(x => x.WipResourceId == woInfoForLes.ResourceId
                                && x.ItemId == woBomInfo.ItemId && x.ItemExtProp == woBomInfo.ItemExtProp);

                        if (prepareItemPush == null)
                        {
                            continue; //跳出当前循环
                        }

                        //判断当前时间是否在计划开始前几个小时内
                        if (!IsPlanStartTime(prepareItemPush, woInfoForLes, dbDateTime))
                        {
                            continue;
                        }


                        if (!IsStockSatisfy(prepareItemPush, woInfoForLes, woBomInfo, meter))
                        {
                            continue; //跳出当前循环
                        }

                        saveWoBomInfoForList.Add(woBomInfo);
                    }

                    if (saveWoBomInfoForList.Any())
                    {
                        woInfoForLes.WoBomInfos = saveWoBomInfoForList;

                        saveWoInfoForLesList.Add(woInfoForLes);
                    }
                }

                return saveWoInfoForLesList;
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.StackTrace);
            }
        }

        /// <summary>
        /// 生成备料清单
        /// </summary>
        public virtual void CreateStockOrderData(List<WoInfoForLes> woInfoForLesList, PrepareItemPushResult prepareItemPushResult,
            List<PrepareItemPushData> prepareItemDatas, StockState stockState, EntityList<StockOrderDetail> stockOrderDetails)
        {
            List<double> wipResourceIds = prepareItemDatas
                  .Select(x => x.WipResourceId)
                  .Distinct()
                  .ToList();

            //生产资源列表
            var wipResourceList = WipResourceCtrl.GetResourceList(wipResourceIds).ToList();
            var wipResourceDic = wipResourceList.GroupBy(p => p.Id).ToDictionary(p => p.Key, p => p.FirstOrDefault());

            EntityList<StockOrder> stockOrders = new EntityList<StockOrder>();

            foreach (var woInfoForLes in woInfoForLesList)
            {
                var meter = GetMeter(woInfoForLes);

                var itemIdsOfWo = woInfoForLes.WoBomInfos.Select(x => x.ItemId).Distinct().ToList();

                var prepareItemPushDatas = prepareItemDatas
                 .Where(x => x.WipResourceId == woInfoForLes.ResourceId
                     && itemIdsOfWo.Contains(x.ItemId));

                int line = 0;
                StockOrder stockOrder = null;
                PrepareItemPushData prepareItemPush = null;

                foreach (var prepareItemPushNew in prepareItemPushDatas
                    .OrderBy(x => x.TriggerType)
                    .ThenBy(x => x.DemandType))
                {
                    //工单BOM中有与备料模式匹配的物料和扩展属性的，才继续执行，否则跳过后面的逻辑
                    if (!woInfoForLes.WoBomInfos.Any(x => x.ItemId == prepareItemPushNew.ItemId
                            && x.ItemExtProp == prepareItemPushNew.ItemExtProp))
                    {
                        continue;
                    }

                    //第一次或与原来备料模式的触发方式、或与原来的需求量计算方式不同，则创建新备料单
                    if (prepareItemPush == null
                        || prepareItemPush.TriggerType != prepareItemPushNew.TriggerType
                        || prepareItemPush.DemandType != prepareItemPushNew.DemandType)
                    {
                        //切换时，判断备料单是否有明细，有明细则添加进待保存备料单列表中
                        if (stockOrder != null && stockOrder.StockOrderDetailList.Any())
                        {
                            stockOrders.Add(stockOrder);
                        }

                        prepareItemPush = prepareItemPushNew;
                        stockOrder = CreateOneStockOrder(woInfoForLes, prepareItemPushNew, wipResourceDic, stockState);

                        line = 0;
                    }

                    foreach (var woBom in woInfoForLes.WoBomInfos
                        .Where(x => x.ItemId == prepareItemPushNew.ItemId && x.ItemExtProp == prepareItemPushNew.ItemExtProp))
                    {
                        StockOrderDetail stockOrderDetail = CreateOneStockOrderDetail(woInfoForLes,
                            prepareItemPush, prepareItemPushNew, woBom, meter, stockState, stockOrderDetails);

                        if (stockOrderDetail.Qty <= 0)
                        {
                            continue;
                        }

                        //合计生成（XXX）个备料需求
                        prepareItemPushResult.PrepareDataCount += 1;

                        line++;
                        stockOrderDetail.LineNo = line.ToString(); //自动生成 
                        stockOrder.StockOrderDetailList.Add(stockOrderDetail);
                    }
                }

                //单个工单遍历完成时，判断备料单是否有明细，有明细则添加进待保存备料单列表中
                if (stockOrder != null && stockOrder.StockOrderDetailList.Any())
                {
                    stockOrders.Add(stockOrder);
                }
            }

            //如果配置项【推式备料是否限制最高存量】= 是，则最终的本次需求量取（本次需求量、最高存量）两者的最小值。
            LimitMaxStocks(stockOrders);

            if (stockOrders.Any())
            {
                //批量获取单号
                var noList = stockOrderService.GetStockOrderNoList(stockOrders.Count);
                for (int i = 0; i < stockOrders.Count; i++)
                {
                    stockOrders[i].No = noList[i];
                }

                using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
                {
                    if (stockState == StockState.Submitted)
                    {
                        //不创建备料计划；将状态改为已提交
                        stockOrders.ForEach(p =>
                        {
                            p.StockState = StockState.Submitted;
                            p.StockOrderDetailList.ForEach(q => { q.StockState = StockState.Submitted; });
                        });
                    }
                    RF.Save(stockOrders);
                    tran.Complete();
                }
            }
            // 生成（XXX）个备料单        
            prepareItemPushResult.StockOrderCount = stockOrders.Count;
        }

        /// <summary>
        /// 如果配置项【推式备料是否限制最高存量】= 是，则最终的本次需求量取（本次需求量、最高存量）两者的最小值。
        /// </summary>
        /// <param name="stockOrders"></param>
        private void LimitMaxStocks(EntityList<StockOrder> stockOrders)
        {
            //推式备料是否限制最高存量的配置项
            var limitedMaximumStock = RT.Service.Resolve<StockOrderService>().GetLimitedMaximumStock();

            //如果配置项【推式备料是否限制最高存量】= 否，返回
            if (!limitedMaximumStock)
            {
                return;
            }
            var whIds = stockOrders.SelectMany(x => x.StockOrderDetailList)
                .Where(x => x.WarehouseId.HasValue)
                .Select(x => x.WarehouseId.Value)
                .Distinct()
                .ToList();

            var itemIds = stockOrders.SelectMany(x => x.StockOrderDetailList)
                .Select(x => x.ItemId)
                .Distinct()
                .ToList();

            //获取收发控制
            var baseItemIoLimits = baseItemExtCtrl.GetBaseItemIoLimit(whIds, itemIds);

            stockOrders.ForEach(stockOrder =>
            {
                stockOrder.StockOrderDetailList
                    .Where(x => x.WarehouseId.HasValue && x.WarehouseId.Value > 0)
                    .ForEach(stockOrderDetail =>
                    {
                        //如果配置项【推式备料是否限制最高存量】= 是，则最终的本次需求量取（本次需求量、最高存量）两者的最小值。
                        //最高存量，先获取工单的资源，再根据资源取【产线线边仓维护】中资源对于的仓库。取【物料】-收发控制-对应仓库的最高存量。
                        var baseItemIoLimit = baseItemIoLimits
                            .FirstOrDefault(x => x.WarehouseId == stockOrderDetail.WarehouseId.Value
                                && x.ItemId == stockOrderDetail.ItemId
                                && x.ItemExtProp == stockOrderDetail.ItemExtProp);

                        if (baseItemIoLimit != null
                            && baseItemIoLimit.MaxStockQty.HasValue
                            && baseItemIoLimit.MaxStockQty.Value > 0)
                        {
                            var maxStockQty = baseItemIoLimit.MaxStockQty.Value;
                            if (stockOrderDetail.Qty > maxStockQty)
                            {
                                stockOrderDetail.Qty = maxStockQty;
                            }
                        }
                    });
            });
        }

        private StockOrderDetail CreateOneStockOrderDetail(WoInfoForLes woInfoForLes, PrepareItemPushData prepareItemPush,
            PrepareItemPushData prepareItemPushNew, WoBomInfoForLes woBom, decimal meter, StockState stockState,
            EntityList<StockOrderDetail> stockOrderDetails)
        {
            StockOrderDetail stockOrderDetail = new StockOrderDetail();

            //工单计划开始时间-(备料)提前时间
            double advanceHours = (double)(prepareItemPushNew.AdvanceHours ?? 0);
            stockOrderDetail.DemandTime = woInfoForLes.PlanBeginDate.AddHours(-advanceHours); //需求时间
            stockOrderDetail.ItemId = woBom.ItemId; //物料
            stockOrderDetail.ItemExtProp = woBom.ItemExtProp; //物料扩展属性
            stockOrderDetail.ItemExtPropName = woBom.ItemExtPropName; //物料扩展属性名称

            LinesideWarehouse linesideWarehouse = GetLineWarehouseId(woInfoForLes.ResourceId);
            if (linesideWarehouse != null)
            {
                stockOrderDetail.WarehouseId = linesideWarehouse.WarehouseId; // 备料接收仓库Id 
                stockOrderDetail.StorageLocationId = linesideWarehouse.StorageLocationId;// 备料接收库位Id
            }
            var woBomSurplusQty = ComputeWorkOrderSurplus(woInfoForLes, woBom, stockOrderDetails);
            stockOrderDetail.Qty = ComputeRequirement(woBom, prepareItemPushNew, meter, stockOrderDetails, woBomSurplusQty);
            stockOrderDetail.StockState = stockState;
            stockOrderDetail.IsManualRec = prepareItemPush.IsManualReception;//是否启用手工物料接收
            stockOrderDetail.WoTotalQty = woBom.RequestQty;//工单总需求
            return stockOrderDetail;
        }

        private StockOrder CreateOneStockOrder(WoInfoForLes woInfoForLes, PrepareItemPushData prepareItemPush,
            Dictionary<double, WipResource> wipResourceDic, StockState stockState)
        {
            StockOrder stockOrder = new StockOrder();
            stockOrder.WorkOrderId = woInfoForLes.Id;
            stockOrder.BillSource = BillSource.Automatic;
            stockOrder.StockType = PrepareItemType.Push;
            stockOrder.StockState = stockState;
            stockOrder.TriggerMode = prepareItemPush.TriggerType;  //触发方式
            stockOrder.DemandMode = prepareItemPush.DemandType;  //需求计算方式
            stockOrder.ResourceId = prepareItemPush.WipResourceId;//生产资源

            if (!wipResourceDic.ContainsKey(prepareItemPush.WipResourceId))
            {
                throw new ValidationException("生产资源【Id:{0}】找不到资料"
                    .L10nFormat(prepareItemPush.WipResourceId));
            }

            var wipResource = wipResourceDic[prepareItemPush.WipResourceId];

            if (!wipResource.FactoryId.HasValue)
            {
                throw new ValidationException("生产资源【{0}】的工厂为空"
                    .L10nFormat(wipResource.Code));
            }

            if (!wipResource.WorkShopId.HasValue)
            {
                throw new ValidationException("生产资源【{0}】的车间为空"
                   .L10nFormat(wipResource.Code));
            }

            stockOrder.WorkShopId = wipResource.WorkShopId.Value;
            stockOrder.FactoryId = wipResource.FactoryId.Value;
            return stockOrder;
        }

        /// <summary>
        /// 获取节拍
        /// </summary>
        public virtual decimal GetMeter(WoInfoForLes woInfoForLes)
        {
            //节拍，取自【产品机型】-产线产能或工时，优先取产线产能，没有的话直接取【产品机型】维护的工时
            if (!productModelDic.ContainsKey(woInfoForLes.ProductId))
            {
                return 0;
            }

            ProductModel productModel = productModelDic[woInfoForLes.ProductId];

            if (productModel == null)
            {
                return 0;
            }

            if (!modelLineCapacitiesDic.Any())
            {
                return 0;
            }

            if (!modelLineCapacitiesDic.ContainsKey(productModel.Id))
            {
                return 0;
            }

            List<ProductModelLineCapacity> lineCapacitys = modelLineCapacitiesDic[productModel.Id];
            ProductModelLineCapacity lineCapacity = lineCapacitys
                .FirstOrDefault(x => x.ResourceId == woInfoForLes.ResourceId);
            if (lineCapacity != null)
            {
                return lineCapacity.WorkingHours ?? 0;
            }

            return productModel.WorkingHours ?? 0;
        }

        #region 获取生成备料清单所生成的数据        
        /// <summary>
        /// 备料接收仓库Id
        /// </summary>
        /// <returns></returns>
        private LinesideWarehouse GetLineWarehouseId(double? resourceId)
        {
            //根据【产线线边仓维护】基础物料获取
            LinesideWarehouse linesideWarehouse = null;
            if (resourceId.HasValue
                && linesideWarehousesDic.TryGetValue(resourceId.Value, out linesideWarehouse))
            {
                return linesideWarehouse;
            }
            return null;
        }

        #endregion

        #region 需求量
        /// <summary>
        /// 计算出需求量
        /// </summary>        
        /// <param name="woBomInfoFor">工单 BOM 信息</param>        
        /// <param name="prepareItemPush"></param>
        /// <param name="meter">生产节拍</param>
        /// <param name="stockOrderDetails"></param>
        /// <param name="woBomSurplusQty">缺料数量</param>        
        /// <returns></returns>
        private decimal ComputeRequirement(WoBomInfoForLes woBomInfoFor,
            PrepareItemPushData prepareItemPush, decimal meter,
            EntityList<StockOrderDetail> stockOrderDetails, decimal woBomSurplusQty)
        {
            if (stockOrderDetails is null)
            {
                throw new ArgumentNullException(nameof(stockOrderDetails));
            }

            decimal requirement = 0;

            switch (prepareItemPush.DemandType)
            {
                case DemandMode.FixedQuantity:
                    requirement = ComputeFixedQuantity(prepareItemPush, woBomSurplusQty);
                    break;
                case DemandMode.WoSurplusQty:
                    requirement = woBomSurplusQty;
                    break;
                case DemandMode.StockToSafeLevelForBeat:
                    requirement = ComputeStockToSafeLevelForBeat(woBomInfoFor, prepareItemPush, woBomSurplusQty, meter);
                    break;
                case DemandMode.StockIsSafeLevelForBeat:
                    requirement = ComputeStockIsSafeLevelForBeat(woBomInfoFor, prepareItemPush, woBomSurplusQty, meter);
                    break;
            }

            return requirement;
        }
        /// <summary>
        /// 需求量(取最小固定量)
        /// </summary>
        /// <returns></returns>
        private decimal ComputeFixedQuantity(PrepareItemPushData prepareItemPush, decimal woBomSurplusQty)
        {
            //工单剩余数量 = 总需求-已备料
            //固定量
            //min(固定量,工单剩余需求）
            decimal fixedQuantity = prepareItemPush.FixedQuantity ?? 0;
            if (woBomSurplusQty > fixedQuantity)
            {
                return fixedQuantity;
            }

            return woBomSurplusQty;
        }

        /// <summary>
        /// 需求量(备料到安全水位)
        /// </summary>
        /// <param name="woBomInfoFor">工单BOM</param>        
        /// <param name="prepareItemPush">备料模式</param>
        /// <param name="woBomSurplusQty">工单物料剩余需求数量</param>
        /// <param name="meter">生产节拍</param>
        /// <returns>需求数量</returns>
        private decimal ComputeStockToSafeLevelForBeat(WoBomInfoForLes woBomInfoFor,
            PrepareItemPushData prepareItemPush, decimal woBomSurplusQty, decimal meter)
        {
            //工单剩余数量 = 总需求-已备料
            //最小备料时间 × 节拍(取自【产品机型】-产线产能或工时，优先取产能)*单机用量 -当前库存（当前库存= 已绑定该工单的标签量）
            // MIN（最小备料时间 × 节拍 - 当前库存,工单剩余需求）
            decimal preparationTime = prepareItemPush.PreparationTime ?? 0;

            if (preparationTime > 0 && meter > 0)
            {
                //到安全水位还差多少数量
                //最小备料时间 × 节拍(取自【产品机型】-产线产能或工时，优先取产能)*单机用量 -当前库存（当前库存= 已绑定该工单的标签量）
                var stockIsSafeNumber = (preparationTime * meter) * woBomInfoFor.SingleQty - woBomInfoFor.OnhandQty;

                //【到安全水位还差多少数量】比工单剩余量还小，则取这个为备料数量
                if (stockIsSafeNumber > 0 && woBomSurplusQty > stockIsSafeNumber)
                {
                    return stockIsSafeNumber;
                }
            }

            return woBomSurplusQty;
        }

        /// <summary>
        /// 需求量(备料量为安全水位量)
        /// </summary>
        /// <param name="woBomInfoFor">工单BOM</param>        
        /// <param name="prepareItemPush">备料模式</param>
        /// <param name="woBomSurplusQty">工单物料剩余需求数量</param>
        /// <param name="meter">生产节拍</param>
        /// <returns>需求数量</returns>
        private decimal ComputeStockIsSafeLevelForBeat(WoBomInfoForLes woBomInfoFor,
            PrepareItemPushData prepareItemPush, decimal woBomSurplusQty, decimal meter)
        {
            //工单剩余数量 = 总需求-已备料
            //最小备料时间 × 节拍(取自【产品机型】-产线产能或工时，优先取产能)*单机用量
            //min（最小备料时间 × 节拍，工单剩余需求）

            //最小备料时间（小时)
            decimal preparationTime = prepareItemPush.PreparationTime ?? 0;

            if (preparationTime > 0 && meter > 0)
            {
                //安全水位
                //最小备料时间 × 节拍(取自【产品机型】-产线产能或工时，优先取产能)*单机用量
                var stockIsSafeNumber = preparationTime * meter * woBomInfoFor.SingleQty;

                //min（最小备料时间 × 节拍，工单剩余需求）
                //工单剩余数量大于安全水位，则取安全水位
                if (stockIsSafeNumber > 0 && woBomSurplusQty > stockIsSafeNumber)
                {
                    return stockIsSafeNumber;
                }
            }

            //否则还取工单物料剩余数量
            return woBomSurplusQty;
        }

        #endregion

        /// <summary>
        /// 工单物料剩余数量
        /// </summary>
        /// <returns></returns>
        private decimal ComputeWorkOrderSurplus(WoInfoForLes woInfoForLes, WoBomInfoForLes woBomInfoFor,
            EntityList<StockOrderDetail> stockOrderDetails)
        {
            //已备料数量
            var prepareQty = stockOrderDetails.Where(x => x.WorkOrderId == woInfoForLes.Id
                    && x.ItemId == woBomInfoFor.ItemId
                    && x.ItemExtProp == woBomInfoFor.ItemExtProp
                    && x.StockState != StockState.Created
                    && x.StockState != StockState.ReCall
                    && x.StockState != StockState.Closed)
                .Sum(p => p.Qty - p.CancelQty);

            //总需求-已备料
            return woBomInfoFor.RequestQty - prepareQty;
        }

        #region 是否生成备料清单
        /// <summary>
        /// 判断当前时间是否在计划开始前几个小时内
        /// </summary>
        /// <param name="prepareItemPush">备料模式推式</param>
        /// <param name="woInfoForLes">工单信息</param>
        /// <returns></returns>
        public virtual bool IsPlanStartTime(PrepareItemPushData prepareItemPush, WoInfoForLes woInfoForLes, DateTime dbDateTime)
        {
            if (!prepareItemPush.AdvanceHours.HasValue)
            {
                return false;
            }

            double advanceHours = decimal.ToDouble(prepareItemPush.AdvanceHours.Value);
            DateTime woInfoDateTime = woInfoForLes.PlanBeginDate.AddHours(-advanceHours);

            //当前时间晚于物料要求到位时间
            if (dbDateTime > woInfoDateTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 库存是否低于安全水位
        /// </summary>
        /// <param name="prepareItemPush">备料模式推式</param>
        /// <param name="woInfoForLes">工单信息</param>
        /// <param name="woBomInfoForLes">工单BOM信息</param>
        /// <param name="meter">生产节拍</param>
        /// <returns></returns>
        public virtual bool IsStockSatisfy(PrepareItemPushData prepareItemPush, WoInfoForLes woInfoForLes,
            WoBomInfoForLes woBomInfoForLes, decimal meter)
        {
            //备料触发方式为【警戒水位】            
            //检查已有量是否低于安全水位

            //备料触发方式不是【警戒水位】，不校验现有量是否低于安全水位
            if (prepareItemPush.TriggerType != TriggerMode.InvBelowSafeLevelToBeat)
            {
                return true;
            }

            //最短满足时间（小时）
            decimal satisfactionTime = prepareItemPush.SatisfactionTime ?? 0;

            //生产节拍 和 最短满足时间 没有配置时，不校验
            if (meter <= 0 && satisfactionTime <= 0)
            {
                return true;
            }

            //安全水位：满足生产多少个产品 乘以 单机定额
            decimal safetyLevel = meter * satisfactionTime * woBomInfoForLes.SingleQty;

            //现有量大于或等于安全水位
            if (woBomInfoForLes.OnhandQty >= safetyLevel)
            {
                return false;
            }

            return true;
        }
        #endregion

        /// <summary>
        /// 获取推式备料需求数据
        /// </summary>
        /// <param name="bill">备料单</param>
        /// <param name="linesideWarehouse">产线线边仓</param>
        /// <returns>推式备料需求数据</returns>
        public virtual List<ItemRequireData> SetPushRequireData(StockOrder bill, LinesideWarehouse linesideWarehouse)
        {
            List<ItemRequireData> itemRequireDatas = new List<ItemRequireData>();
            if (!bill.WorkOrderId.HasValue)
            {
                return itemRequireDatas;
            }

            if (!bill.ResourceId.HasValue)
            {
                return itemRequireDatas;
            }

            //获取工单信息列表
            var woInfoForLesList = WoInfoForLesController.GetWoInfoForLesByWorkOrderId(bill.WorkOrderId.Value);

            //单个备料单只会有一个工单
            var woInfoForLes = woInfoForLesList.FirstOrDefault();
            if (woInfoForLes == null)
            {
                throw new ValidationException("当前工单为非【发放】、非【生产中】状态，请检查！".L10N());
            }

            //获取所有备料需求明细
            var stockOrderDetails = RT.Service.Resolve<StockOrderDetailDao>()
             .GetPushStockOrderDetails(new List<double> { bill.WorkOrderId.Value });
            if (bill.DemandMode == DemandMode.ManualSetQuantity)//按套数备料
            {
                GetManualSetQuantityList(bill, linesideWarehouse, itemRequireDatas, woInfoForLes, stockOrderDetails);
                return itemRequireDatas;
            }
            //获取产能
            GetCapacities(woInfoForLesList);

            //生产节拍
            var meter = GetMeter(woInfoForLes);

            //备料模式数据
            var pushItems = RT.Service.Resolve<PrepareItemController>()
                .GetPushItems(bill.DemandMode, new List<double?> { bill.ResourceId.Value });

            if (!pushItems.Any())
            {
                return itemRequireDatas;
            }

            //转换备料模式数据
            var prepareItemDatas = GetPrepareItemDatas(pushItems);

            foreach (var bom in woInfoForLes.WoBomInfos)
            {
                var prepareItemPushData = prepareItemDatas
                     .FirstOrDefault(x => x.ItemId == bom.ItemId && x.ItemExtProp == bom.ItemExtProp);

                if (prepareItemPushData == null)
                {
                    continue;
                }

                var woSurplusQty = ComputeWorkOrderSurplus(woInfoForLes, bom, stockOrderDetails);

                itemRequireDatas.Add(new ItemRequireData
                {
                    ItemId = bom.ItemId,
                    ItemCode = bom.ItemCode,
                    ItemName = bom.ItemName,
                    WoTotalQty = bom.RequestQty,
                    WoSurplusQty = woSurplusQty,
                    RequireQty = ComputeRequirement(bom, prepareItemPushData, meter, stockOrderDetails, woSurplusQty),
                    WarehouseId = linesideWarehouse?.WarehouseId,
                    WarehouseCode = linesideWarehouse?.WarehouseCode,
                    LocId = linesideWarehouse?.StorageLocationId,
                    LocCode = linesideWarehouse?.LocaltionCode,
                    IsEnabelManualRec = prepareItemPushData.IsManualReception,
                    //后台生成的，不允许编辑扩展属性
                    IsAllowEdit = false,
                    ItemExtProp = bom.ItemExtProp,
                    ItemExtPropName = bom.ItemExtPropName,
                    RequireDate = DateTime.Now,
                });
            }

            //推式备料是否限制最高存量的配置项
            LimitMaxStock(itemRequireDatas);

            return itemRequireDatas;
        }

        /// <summary>
        /// 获取手动-按套备料数据
        /// </summary>
        /// <param name="bill"></param>
        /// <param name="linesideWarehouse"></param>
        /// <param name="itemRequireDatas"></param>
        /// <param name="woInfoForLes"></param>
        /// <param name="stockOrderDetails"></param>
        private void GetManualSetQuantityList(StockOrder bill, LinesideWarehouse linesideWarehouse, List<ItemRequireData> itemRequireDatas, WoInfoForLes woInfoForLes, EntityList<StockOrderDetail> stockOrderDetails)
        {
            var itemIds = woInfoForLes.WoBomInfos.Select(m => m.ItemId).Distinct().ToList();
            var itemList = itemIds.SplitContains(ids => { return Query<Item>().Where(m => ids.Contains(m.Id)).ToList(); });
            foreach (var bom in woInfoForLes.WoBomInfos)
            {

                var consumeMode = itemList.FirstOrDefault(m => m.Id == bom.ItemId).ConsumeMode;
                if (consumeMode != ConsumeMode.Push)
                {
                    continue;
                }
                var woSurplusQty = ComputeWorkOrderSurplus(woInfoForLes, bom, stockOrderDetails);
                decimal numberOfSetsQuantity = bom.SingleQty * bill.NumberOfSets;
                if (numberOfSetsQuantity > 0)
                {
                    itemRequireDatas.Add(new ItemRequireData
                    {
                        ItemId = bom.ItemId,
                        ItemCode = bom.ItemCode,
                        ItemName = bom.ItemName,
                        WoTotalQty = bom.RequestQty,
                        WoSurplusQty = woSurplusQty,
                        RequireQty = woSurplusQty > numberOfSetsQuantity ? numberOfSetsQuantity : woSurplusQty,//取剩余数量和套数两者之间小的数
                        WarehouseId = linesideWarehouse?.WarehouseId,
                        WarehouseCode = linesideWarehouse?.WarehouseCode,
                        LocId = linesideWarehouse?.StorageLocationId,
                        LocCode = linesideWarehouse?.LocaltionCode,
                        IsEnabelManualRec = true,
                        //后台生成的，不允许编辑扩展属性
                        IsAllowEdit = false,
                        ItemExtProp = bom.ItemExtProp,
                        ItemExtPropName = bom.ItemExtPropName,
                        RequireDate = DateTime.Now,
                    });
                }
            }
        }

        /// <summary>
        /// 限制备料数量不能大于收发控制配置的最大存量
        /// </summary>
        /// <param name="itemRequireDatas"></param>
        private void LimitMaxStock(List<ItemRequireData> itemRequireDatas)
        {
            //推式备料是否限制最高存量的配置项
            var limitedMaximumStock = RT.Service.Resolve<StockOrderService>().GetLimitedMaximumStock();

            //如果配置项【推式备料是否限制最高存量】= 否，返回
            if (!limitedMaximumStock)
            {
                return;
            }

            var whIds = itemRequireDatas
                .Where(x => x.WarehouseId.HasValue)
                .Select(x => x.WarehouseId.Value)
                .Distinct()
                .ToList();

            var itemIds = itemRequireDatas
                .Select(x => x.ItemId)
                .Distinct()
                .ToList();

            //获取收发控制
            var baseItemIoLimits = baseItemExtCtrl.GetBaseItemIoLimit(whIds, itemIds);

            itemRequireDatas.Where(x => x.WarehouseId.HasValue && x.WarehouseId.Value > 0)
                .ForEach(itemRequireData =>
                {
                    //如果配置项【推式备料是否限制最高存量】= 是，则最终的本次需求量取（本次需求量、最高存量）两者的最小值。
                    //最高存量，先获取工单的资源，再根据资源取【产线线边仓维护】中资源对于的仓库。取【物料】-收发控制-对应仓库的最高存量。
                    var baseItemIoLimit = baseItemIoLimits
                        .FirstOrDefault(x => x.WarehouseId == itemRequireData.WarehouseId.Value
                            && x.ItemId == itemRequireData.ItemId
                            && x.ItemExtProp == itemRequireData.ItemExtProp);

                    if (baseItemIoLimit != null
                        && baseItemIoLimit.MaxStockQty.HasValue
                        && baseItemIoLimit.MaxStockQty.Value > 0)
                    {
                        var maxStockQty = baseItemIoLimit.MaxStockQty.Value;
                        if (itemRequireData.RequireQty > maxStockQty)
                        {
                            itemRequireData.RequireQty = maxStockQty;
                        }
                    }
                });
        }
    }
}
