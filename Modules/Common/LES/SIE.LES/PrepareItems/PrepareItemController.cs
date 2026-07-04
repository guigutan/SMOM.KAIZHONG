using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.WMS.Inventory;
using SIE.Items;
using SIE.LES.Commons;
using SIE.LES.LinesideWarehouses;
using SIE.LES.PrepareItems.Models;
using SIE.LES.StockOrders;
using SIE.LES.StockOrders.Configs;
using SIE.LES.StockOrders.Dao;
using SIE.LES.StockOrders.Models;
using SIE.LES.StockOrders.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.LES
{
    /// <summary>
    /// 备料模式控制器
    /// </summary>
    public partial class PrepareItemController : DomainController
    {
        /// <summary>
        /// 获取备料模式-拉式
        /// </summary>
        /// <param name="ids">备料模式-拉式ID集合</param>
        /// <returns>备料模式-拉式列表</returns>
        public virtual EntityList<PrepareItemPull> GetPrepareItemPulls(List<double> ids)
        {
            return Query<PrepareItemPull>().Where(p => !ids.Contains(p.Id) && p.PrepareItemType == PrepareItemType.Pull).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取备料模式-拉式
        /// </summary>
        /// <returns>备料模式-拉式列表</returns>
        private EntityList<PrepareItemPull> GetPrepareItemPulls(EntityList<LinesideWarehouse> linesideWarehouses)
        {
            var warehouseIds = linesideWarehouses.Select(x => (double?)x.WarehouseId).Distinct().ToList();

            return warehouseIds.SplitContains(tempIds =>
            {
                return Query<PrepareItemPull>().Where(p => p.PrepareItemType == PrepareItemType.Pull
                        && p.TriggerType != TriggerMode.ManualModel
                        && p.DemandType != DemandMode.ManualFillIn
                        && tempIds.Contains(p.WarehouseId))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取备料模式-拉式
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>备料模式-拉式列表</returns>
        public virtual EntityList<PrepareItemPull> GetPrepareItemPulls(PrepareItemPullCriteria criteria)
        {
            var query = Query<PrepareItemPull>().Where(p => p.PrepareItemType == PrepareItemType.Pull);

            if (criteria.WarehouseId.HasValue)
            {
                query.Where(p => p.WarehouseId == criteria.WarehouseId.Value);
            }

            if (criteria.ItemCategoryId.HasValue)
            {
                query.Where(p => p.ItemCategoryId == criteria.ItemCategoryId.Value);
            }

            if (criteria.ItemId.HasValue)
            {
                query.Where(p => p.ItemId == criteria.ItemId.Value);
            }

            if (criteria.ItemName.IsNotEmpty())
            {
                query.Where(p => p.Item.Name.Contains(criteria.ItemName));
            }
            if (criteria.TriggerType.HasValue)
            {
                query.Where(p => p.TriggerType == criteria.TriggerType);
            }

            if (criteria.DemandType.HasValue)
            {
                query.Where(p => p.DemandType == criteria.DemandType.Value);
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取备料模式-拉式
        /// </summary>
        public virtual PrepareItemPullResult GetPrepareItemPullList()
        {
            PrepareItemPullResult prepareItemPullResult = new PrepareItemPullResult();


            List<PrepareItemData> prepareItemDatas = new List<PrepareItemData>();

            //产线线边仓有维护才能生成拉式备料需求
            var linesideWarehouses = RT.Service.Resolve<LinesideWarehouseController>().GetLinesideWarehouses();

            //获取所有拉式备料模式
            var prepareItemPulls = GetPrepareItemPulls(linesideWarehouses);

            //转换拉式备料模式数据为运行所需要的格式
            GetPrepareItemDatas(prepareItemDatas, prepareItemPulls);

            //累计匹配（xxx）笔（仓库+物料）的数据
            prepareItemPullResult.DataCount = prepareItemDatas.Count;

            // 过滤掉有未接收的备料单的数据
            List<PrepareItemData> prepareItemList = FilterHaveNotExcuteStockOrders(prepareItemDatas);

            //获取调度触发的备料单状态的配置项值                
            var configValue = RT.Service.Resolve<StockOrderService>().GetSchedulingTriggeredStatusConfigValue();
            StockState stockState = StockState.Created;
            if (configValue != null && configValue.TriggeredStatus == TriggeredStatus.Submitted)
            {
                stockState = StockState.Submitted;
            }

            //按仓库分组
            var dictionaryGroupByWhId = prepareItemList.GroupBy(x => x.WarehouseId).ToDictionary(g => g.Key, g => g.ToList());

            EntityList<StockOrder> stockOrders = new EntityList<StockOrder>();

            foreach (var warehousId in dictionaryGroupByWhId.Keys)
            {
                var linesideWarehouse = linesideWarehouses.FirstOrDefault(x => x.WarehouseId == warehousId);

                var prepareItems = dictionaryGroupByWhId[warehousId];

                var itemIds = prepareItems.Where(x => x.ItemId != null).Select(x => (double)x.ItemId).Distinct().ToList();

                //可用库存
                var onhandDataInfos = RT.Service.Resolve<IGetLotLpnOnhand>()
                    .GetLotLpnOnhandByItemIds(warehousId, itemIds);

                DemandMode? demandMode = null;
                TriggerMode? triggerMode = null;
                StockOrder stockOrder = null;
                int lineNo = 0;

                foreach (var prepareItemData in prepareItems
                    .OrderBy(x => x.DemandType)
                    .ThenBy(x => x.TriggerType))
                {
                    //需求计算方式或触发方式不同时，创建新的一张备料单
                    if (demandMode == null || triggerMode == null
                        || demandMode != prepareItemData.DemandType
                        || triggerMode != prepareItemData.TriggerType)
                    {
                        demandMode = prepareItemData.DemandType;
                        triggerMode = prepareItemData.TriggerType;

                        //切换时，如果明细不为空，则添加进待保存清单中
                        if (stockOrder != null && stockOrder.StockOrderDetailList.Any())
                        {
                            stockOrders.Add(stockOrder);
                        }

                        //创建主表
                        stockOrder = CreateStockOrder(prepareItemData, linesideWarehouse, stockState);
                        lineNo = 0;
                    }

                    //可用量大于或等于最低安全水位，不触发
                    var availableQty = onhandDataInfos
                       .Where(x => x.ItemId == prepareItemData.ItemId && x.ItemExtProp == prepareItemData.ItemExtProp)
                       .Sum(x => x.AvailableQty);
                    if (availableQty >= prepareItemData.LowestStage)
                    {
                        continue;
                    }

                    //满足触发方式的共有（XXX）笔数据
                    prepareItemPullResult.FitDataCount += 1;

                    //需求数量计算方式
                    //6）	当 拉式 且 需求计算方式 = 至最高存量  时，本次需求数量 = 最高存量 - 现有数量。
                    //最高存量，取【备料模式维护 - 拉式】-对应仓库 + 物料的最高存量。
                    //现有数量， 取MES物料标签表中，同仓库同物料同拓展属性的可用数量。（此处需剔除不良数量）
                    decimal qty = 0;
                    if (prepareItemData.DemandType == DemandMode.MaxStock)
                    {
                        qty = (prepareItemData.MaxStock ?? 0) - availableQty;
                    }
                    else if (prepareItemData.DemandType == DemandMode.FixedQuantity)
                    {
                        qty = prepareItemData.FixedQuantity ?? 0;
                    }

                    if (qty <= 0)
                    {
                        continue;
                    }

                    //合计生成（XXX）个备料需求
                    prepareItemPullResult.PrepareDataCount += 1;

                    lineNo++;

                    //创建备料单明细
                    CreateStockOrderDetail(stockOrder, qty, prepareItemData, lineNo, linesideWarehouse);
                }

                //遍历完成时，如果明细不为空，则添加进待保存清单中
                if (stockOrder != null && stockOrder.StockOrderDetailList.Any())
                {
                    stockOrders.Add(stockOrder);
                }
            }

            if (stockOrders.Any())
            {
                //批量获取单号
                SaveStockOrders(stockState, stockOrders);
            }

            // 生成（XXX）个备料单        
            prepareItemPullResult.StockOrderCount = stockOrders.Count;



            return prepareItemPullResult;
        }

        private static void SaveStockOrders(StockState stockState, EntityList<StockOrder> stockOrders)
        {
            var noList = RT.Service.Resolve<StockOrderService>().GetStockOrderNoList(stockOrders.Count);
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

        /// <summary>
        /// 转换拉式备料模式数据为运行所需要的格式
        /// </summary>
        /// <param name="prepareItemDatas"></param>
        /// <param name="prepareItemPulls"></param>
        private void GetPrepareItemDatas(List<PrepareItemData> prepareItemDatas, IList<PrepareItemPull> prepareItemPulls)
        {
            foreach (var itemPull in prepareItemPulls
                .Where(x => x.ItemId.HasValue && x.ItemId.Value != 0 && x.WarehouseId.HasValue))
            {
                //仓库加物料及物料扩展属性已经有数据，则跳过
                if (prepareItemDatas.Any(x => x.WarehouseId == itemPull.WarehouseId
                    && x.ItemId == itemPull.ItemId.Value && x.ItemExtProp == itemPull.ItemExtProp))
                {
                    continue;
                }

                PrepareItemData prepareItemData = new PrepareItemData();
                prepareItemData.WarehouseId = itemPull.WarehouseId.Value;
                prepareItemData.FixedQuantity = itemPull.FixedQuantity;
                prepareItemData.DemandType = itemPull.DemandType.HasValue ? itemPull.DemandType.Value : DemandMode.StockIsSafeLevelForBeat;
                prepareItemData.TriggerType = itemPull.TriggerType.HasValue ? itemPull.TriggerType.Value : TriggerMode.BelowSafe;
                prepareItemData.MaxStock = itemPull.MaxStock;
                prepareItemData.LowestStage = itemPull.LowestStage.HasValue ? itemPull.LowestStage.Value : 0;
                prepareItemData.WarehouseCode = itemPull.WarehouseCode;
                prepareItemData.ItemId = itemPull.ItemId.Value;
                prepareItemData.ItemExtProp = itemPull.ItemExtProp;
                prepareItemData.ItemExtPropName = itemPull.ItemExtPropName;
                prepareItemDatas.Add(prepareItemData);
            }

            //按分类维护的拉式备料模式
            var itemCategoryIds = prepareItemPulls
                .Where(x => x.ItemId == null && x.ItemCategoryId != null)
                .Select(x => x.ItemCategoryId)
                .Distinct()
                .ToList();

            //获取分类下面所有物料信息
            var itemCategoryRelations = RT.Service.Resolve<ItemController>()
                .GetItemCategoryRelations(itemCategoryIds, consumeMode: ConsumeMode.Pull);

            foreach (var itemPull in prepareItemPulls
                .Where(x => x.ItemId == null && x.ItemCategoryId != null && x.WarehouseId != null))
            {
                foreach (var itemId in itemCategoryRelations
                    .Where(x => x.ItemCategoryId == itemPull.ItemCategoryId)
                    .Select(x => x.ItemId))
                {
                    //仓库加物料及物料扩展属性已经有数据，则跳过
                    if (prepareItemDatas.Any(x => x.WarehouseId == itemPull.WarehouseId
                        && x.ItemId == itemId && x.ItemExtProp == itemPull.ItemExtProp))
                    {
                        continue;
                    }

                    PrepareItemData prepareItemData = new PrepareItemData();
                    prepareItemData.WarehouseId = itemPull.WarehouseId.Value;
                    prepareItemData.FixedQuantity = itemPull.FixedQuantity;
                    prepareItemData.DemandType = itemPull.DemandType.HasValue ? itemPull.DemandType.Value : DemandMode.StockIsSafeLevelForBeat;
                    prepareItemData.TriggerType = itemPull.TriggerType.HasValue ? itemPull.TriggerType.Value : TriggerMode.BelowSafe;
                    prepareItemData.MaxStock = itemPull.MaxStock;
                    prepareItemData.LowestStage = itemPull.LowestStage.HasValue ? itemPull.LowestStage.Value : 0;
                    prepareItemData.WarehouseCode = itemPull.WarehouseCode;
                    prepareItemData.ItemId = itemId;
                    prepareItemData.ItemExtProp = itemPull.ItemExtProp;
                    prepareItemData.ItemExtPropName = itemPull.ItemExtPropName;
                    prepareItemDatas.Add(prepareItemData);
                }
            }
        }

        /// <summary>
        /// 过滤掉有未接收的备料单的数据
        /// </summary>
        /// <param name="prepareItemDatas"></param>
        /// <returns></returns>
        private static List<PrepareItemData> FilterHaveNotExcuteStockOrders(List<PrepareItemData> prepareItemDatas)
        {
            var itemIds = prepareItemDatas.Where(x => x.ItemId != null).Select(x => (double)x.ItemId).Distinct().ToList();

            //获取所有未接收的需求明细
            var stockOrderDetails = RT.Service.Resolve<StockOrderDetailDao>()
                .GetStockOrderDetails(itemIds, PrepareItemType.Pull);

            //过滤掉有未接收的备料单的数据
            List<PrepareItemData> prepareItemList = new List<PrepareItemData>();
            foreach (var prep in prepareItemDatas)
            {
                //不存在未接收的备料明细时
                if (!stockOrderDetails.Any(p => p.WarehouseId == prep.WarehouseId && prep.ItemId == p.ItemId
                        && prep.ItemExtProp == p.ItemExtProp))
                {
                    prepareItemList.Add(prep);
                }
            }

            return prepareItemList;
        }

        /// <summary>
        /// 备料单主信息
        /// </summary>
        /// <param name="itemPull">需求数据</param>
        /// <param name="linesideWarehouse">产线线边仓配置</param>
        /// <param name="stockState">备料状态</param>
        /// <returns>备料单</returns>
        private StockOrder CreateStockOrder(PrepareItemData itemPull,
            LinesideWarehouse linesideWarehouse,
            StockState stockState)
        {
            StockOrder stockOrder = new StockOrder();

            stockOrder.BillSource = BillSource.Automatic;
            stockOrder.StockType = PrepareItemType.Pull;
            stockOrder.StockState = stockState;
            stockOrder.TriggerMode = itemPull.TriggerType;  //触发方式
            stockOrder.DemandMode = itemPull.DemandType;  //需求计算方式
            stockOrder.ResourceId = linesideWarehouse.WipResouceId;
            //stockOrder.WorkShopId = linesideWarehouse.WorkShopId;
            stockOrder.FactoryId = linesideWarehouse.FactoryId.Value;

            return stockOrder;
        }

        /// <summary>
        /// 创建备料单明细
        /// </summary>
        /// <param name="sto">备料单</param>
        /// <param name="qty">本次备料数</param>
        /// <param name="itemPull">需求备料数据</param>
        /// <param name="lineNo">行号</param>
        /// <param name="linesideWarehouse">产线线边仓配置</param>
        private void CreateStockOrderDetail(StockOrder sto, decimal qty, PrepareItemData itemPull, int lineNo, LinesideWarehouse linesideWarehouse)
        {
            StockOrderDetail stockOrderDetail = new StockOrderDetail();
            stockOrderDetail.LineNo = lineNo.ToString(); //自动生成 
            stockOrderDetail.Qty = qty;
            stockOrderDetail.DemandTime = DateTime.Now; //需求时间
            stockOrderDetail.ItemId = itemPull.ItemId.Value; //物料
            stockOrderDetail.ItemExtProp = itemPull.ItemExtProp; //物料扩展属性
            stockOrderDetail.ItemExtPropName = itemPull.ItemExtPropName; //物料扩展属性名称
            stockOrderDetail.WarehouseId = itemPull.WarehouseId;
            stockOrderDetail.StorageLocationId = linesideWarehouse.StorageLocationId;
            stockOrderDetail.StockState = sto.StockState;
            stockOrderDetail.IsManualRec = itemPull.IsManualReception;//是否启用手工物料接收
            stockOrderDetail.WoTotalQty = 0;//工单总需求 
            sto.StockOrderDetailList.Add(stockOrderDetail);
        }

        /// <summary>
        /// 获取备料模式-推式
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>备料模式-推式列表</returns>
        public virtual EntityList<PrepareItemPush> GetPrepareItemPushs(PrepareItemPushCriteria criteria)
        {
            var query = Query<PrepareItemPush>().Where(p => p.PrepareItemType == PrepareItemType.Push);

            if (criteria.WipResourceId.HasValue)
            {
                query.Where(p => p.WipResourceId == criteria.WipResourceId.Value);
            }

            if (criteria.ItemCategoryId.HasValue)
            {
                query.Where(p => p.ItemCategoryId == criteria.ItemCategoryId.Value);
            }

            if (criteria.ItemId.HasValue)
            {
                query.Where(p => p.ItemId == criteria.ItemId.Value);
            }

            if (criteria.ItemName.IsNotEmpty())
            {
                query.Where(p => p.Item.Name.Contains(criteria.ItemName));
            }

            if (criteria.TriggerType.HasValue)
            {
                query.Where(p => p.TriggerType == criteria.TriggerType.Value);
            }

            if (criteria.DemandType.HasValue)
            {
                query.Where(p => p.DemandType == criteria.DemandType.Value);
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取备料模式-推式
        /// </summary>
        /// <param name="ids">备料模式-推式ID集合</param>
        /// <returns>备料模式-推式列表</returns>
        public virtual EntityList<PrepareItemPush> GetPrepareItemPushs(List<double> ids)
        {
            return Query<PrepareItemPush>().Where(p => !ids.Contains(p.Id) && p.PrepareItemType == PrepareItemType.Push).ToList();
        }

        /// <summary>
        /// 获取备料模式-推式
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<PrepareItemPush> GetPrepareItemPushs()
        {
            return Query<PrepareItemPush>()
                .Where(x => x.PrepareItemType == PrepareItemType.Push
                    && (x.TriggerType == TriggerMode.XHoursBefore || x.TriggerType == TriggerMode.InvBelowSafeLevelToBeat)).ToList();
        }

        /// <summary>
        /// 获取推式备料维护数据
        /// </summary>
        /// <param name="demandMode">需求计算方式</param>
        /// <param name="whIds">仓库ID集合</param>
        /// <returns>推式备料维护数据</returns>
        public virtual EntityList<PrepareItemPull> GetPullItems(DemandMode demandMode, List<double> whIds)
        {
            // 手工添加备料单：需求计算方式应取触发方式为手工的数据
            var query = Query<PrepareItemPull>().Where(p => p.DemandType == demandMode
                && p.PrepareItemType == PrepareItemType.Pull
                && p.TriggerType == TriggerMode.ManualModel);
            if (whIds.Any())
            {
                query.Where(p => whIds.Contains((double)p.WarehouseId));
            }

            return query.ToList();
        }

        /// <summary>
        /// 获取推式备料维护数据
        /// </summary>
        /// <param name="demandMode">需求计算方式</param>
        /// <param name="resourceIds">资源ID集合</param>
        /// <returns>推式备料维护数据</returns>
        public virtual EntityList<PrepareItemPush> GetPushItems(DemandMode demandMode, List<double?> resourceIds)
        {
            // http://192.168.168.207:30001/bug-view-183.html
            // 手工添加备料单：需求计算方式应取触发方式为手工的数据
            var query = Query<PrepareItemPush>().Where(p => p.DemandType == demandMode
                && p.PrepareItemType == PrepareItemType.Push
                && p.TriggerType == TriggerMode.ManualModel);

            if (resourceIds.Any())
            {
                query.Where(p => resourceIds.Contains(p.WipResourceId));
            }

            return query.ToList();
        }

        /// <summary>
        /// 获取推式备料维护数据
        /// </summary>
        /// <param name="wipResourceId">生产资源ID</param>
        /// <param name="itemCategoryId">物料类型ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="triggerMode">触发方式</param>
        /// <returns>推式备料维护数据</returns>
        public virtual PrepareItemPush GetPrepareItemPush(double? wipResourceId, double? itemCategoryId, double? itemId, TriggerMode? triggerMode, string itemExtPropName)
        {
            var query = Query<PrepareItemPush>();
            if (wipResourceId.HasValue)
                query.Where(p => p.WipResourceId == wipResourceId);
            if (itemCategoryId.HasValue)
                query.Where(p => p.ItemCategoryId == itemCategoryId);
            if (itemId.HasValue)
                query.Where(p => p.ItemId == itemId);
            if (triggerMode.HasValue)
                query.Where(p => p.TriggerType == triggerMode);
            if (!itemExtPropName.IsNullOrEmpty())
                query.Where(p => p.ItemExtPropName == itemExtPropName);
            return query.Where(p => p.PrepareItemType == PrepareItemType.Push).FirstOrDefault();
        }

        /// <summary>
        /// 获取拉式备料维护数据
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="itemCategoryId">物料类型ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="triggerType">触发方式</param>
        /// <param name="itemExtPropName">物料扩展属性</param>
        /// <returns>拉式备料维护数据</returns>
        public virtual PrepareItemPull GetPrepareItemPull(double? warehouseId, double? itemCategoryId, double? itemId, TriggerMode? triggerType, string itemExtPropName)
        {
            var query = Query<PrepareItemPull>();
            if (warehouseId.HasValue)
                query.Where(p => p.WarehouseId == warehouseId);
            if (itemCategoryId.HasValue)
                query.Where(p => p.ItemCategoryId == itemCategoryId);
            if (itemId.HasValue)
                query.Where(p => p.ItemId == itemId);
            if (triggerType.HasValue)
                query.Where(p => p.TriggerType == triggerType);
            if (!itemExtPropName.IsNullOrEmpty())
                query.Where(p => p.ItemExtPropName == itemExtPropName);
            return query.Where(p => p.PrepareItemType == PrepareItemType.Pull).FirstOrDefault();
        }

        /// <summary>
        /// 获取拉式备料需求数据
        /// </summary>
        /// <param name="bill">备料单</param>
        /// <param name="lineWh">线边仓</param>
        /// <param name="warehouseId">仓库ID</param>
        /// <returns>拉式备料需求数据</returns>
        public virtual List<ItemRequireData> SetPullRequireData(StockOrder bill, LinesideWarehouse lineWh, double warehouseId)
        {
            List<ItemRequireData> datas = new List<ItemRequireData>();

            var pullItems = RT.Service.Resolve<PrepareItemController>()
                .GetPullItems(bill.DemandMode, new List<double> { lineWh.WarehouseId });

            List<PrepareItemData> prepareItemDatas = new List<PrepareItemData>();

            //转换拉式备料模式数据为运行所需要的格式
            GetPrepareItemDatas(prepareItemDatas, pullItems);

            var itemIds = prepareItemDatas.Where(x => x.ItemId != null).Select(x => (double)x.ItemId).ToList();

            //可用库存
            var onhandDataInfos = RT.Service.Resolve<IGetLotLpnOnhand>()
                .GetLotLpnOnhandByItemIds(warehouseId, itemIds);

            var items = RT.Service.Resolve<ItemController>().GetItemList(itemIds);

            //获取配置项的【需求计算方式】等于手工填写时，默认值取
            foreach (var prepareItemData in prepareItemDatas)
            {
                //可用量大于或等于最低安全水位，不触发
                var availableQty = onhandDataInfos
                   .Where(x => x.ItemId == prepareItemData.ItemId && x.ItemExtProp == prepareItemData.ItemExtProp)
                   .Sum(x => x.AvailableQty);

                decimal reqQty = 0;
                decimal maxInvQty = prepareItemData.MaxStock ?? 0;
                if (bill.DemandMode == DemandMode.MaxStock)
                {
                    reqQty = maxInvQty - availableQty;
                }
                else
                {
                    if (bill.DemandMode == DemandMode.FixedQuantity)
                    {
                        reqQty = prepareItemData.FixedQuantity ?? 0;
                    }
                }

                if (reqQty <= 0)
                {
                    continue;
                }

                var item = items.FirstOrDefault(x => x.Id == prepareItemData.ItemId);
                if (item == null)
                {
                    throw new ValidationException("物料【Id:{0}】的资料找不到".L10nFormat(prepareItemData.ItemId));
                }

                datas.Add(new ItemRequireData
                {
                    ItemId = prepareItemData.ItemId.Value,
                    ItemCode = item.Code,
                    ItemName = item.Name,
                    WoTotalQty = 0,
                    WoSurplusQty = 0,
                    RequireQty = reqQty,
                    WarehouseId = lineWh?.WarehouseId,
                    WarehouseCode = lineWh?.WarehouseCode,
                    LocId = lineWh?.StorageLocationId,
                    LocCode = lineWh?.LocaltionCode,
                    IsEnabelManualRec = prepareItemData.IsManualReception,
                    ItemExtProp = prepareItemData.ItemExtProp,
                    IsAllowEdit = false,//item.EnableExtendProperty, 后台生成不允许编辑扩展属性
                    ItemExtPropName = prepareItemData.ItemExtPropName,
                    RequireDate = DateTime.Now,
                });
            }

            return datas;
        }
    }
}
