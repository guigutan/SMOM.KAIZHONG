using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using SIE.Common.Configs;
using SIE.Common.Schdules;
using SIE.Core.ApiModels;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.EventMessages.MES.WorkOrders;
using SIE.EventMessages.MES.WorkOrders.Models;
using SIE.EventMessages.WMS.Inventory;
using SIE.Items;
using SIE.LES.Commons;
using SIE.LES.LinesideWarehouses;
using SIE.LES.LinesideWarehouses.Models;
using SIE.LES.MaterialPreparations;
using SIE.LES.MaterialPreparations.ApiModels;
using SIE.LES.MaterialPreparations.Configs;
using SIE.LES.MaterialPreparations.Enums;
using SIE.LES.MaterialPreparations.Helpers;
using SIE.LES.MaterialReturnApplys.Enums;
using SIE.LES.PrepareItems.Models;
using SIE.LES.Reports;
using SIE.LES.Reports.Datas;
using SIE.LES.StockOrders;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using SIE.Warehouses.ItemIoLimits.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES
{
    /// <summary>
    /// 备料模式控制器-调度
    /// </summary>
    public partial class PrepareItemController : DomainController
    {
        #region 拉式调度
        /// <summary>
        /// 获取拉式备料模式数据
        /// </summary>
        /// <returns></returns>
        private List<PrepareItemData> GetPullPrepareItemDatas()
        {
            List<PrepareItemData> prepareItemDatas = new List<PrepareItemData>();
            // 查询备料模式-拉式
            // 物料类型
            var itemList = Query<PrepareItemPull>()
                .Exists<LinesideWarehouse>((pip, lw) => lw.Where(w => w.WarehouseId == pip.WarehouseId))
                .Where(pip => pip.PrepareItemType == PrepareItemType.Pull && pip.TriggerType == Commons.TriggerMode.BelowSafe && pip.DemandType != DemandMode.ManualFillIn
                && pip.ItemId != null)
                .Select(pip => new
                {
                    WarehouseId = pip.WarehouseId,
                    ItemCategoryId = pip.ItemCategoryId,
                    ItemId = pip.ItemId,
                    LowestStage = pip.LowestStage,
                    TriggerType = pip.TriggerType,
                    DemandType = pip.DemandType,
                    MaxStock = pip.MaxStock,
                    FixedQuantity = pip.FixedQuantity,
                    ItemExtProp = pip.ItemExtProp,
                }).ToList<PrepareItemData>();
            prepareItemDatas.AddRange(itemList);

            // 物料类型
            List<PrepareItemData> catelist = Query<PrepareItemPull>().Exists<LinesideWarehouse>((pip, lw) => lw.Where(w => w.WarehouseId == pip.WarehouseId))
                .Where(pip => pip.PrepareItemType == PrepareItemType.Pull && pip.TriggerType == Commons.TriggerMode.BelowSafe && pip.DemandType != DemandMode.ManualFillIn
                && pip.ItemCategoryId != null && pip.ItemId == null)
                .LeftJoin<ItemCategoryRelation>((pip, icr) => pip.ItemCategoryId == icr.ItemCategoryId)
                .LeftJoin<ItemCategoryRelation, Item>((icr, i) => icr.ItemId == i.Id)
                .Where<Item>((pip, i) => i.ConsumeMode == ConsumeMode.Pull)
                .Where<ItemCategoryRelation>((pip, icr) => icr.Type == Items.Items.CategoryType.Item)
                .Select<ItemCategoryRelation>((pip, icr) => new
                {
                    WarehouseId = pip.WarehouseId,
                    ItemCategoryId = pip.ItemCategoryId,
                    ItemId = icr.ItemId,
                    LowestStage = pip.LowestStage,
                    TriggerType = pip.TriggerType,
                    DemandType = pip.DemandType,
                    MaxStock = pip.MaxStock,
                    FixedQuantity = pip.FixedQuantity,
                    ItemExtProp = pip.ItemExtProp,
                }).ToList<PrepareItemData>().ToList();

            // 物料分类去除重复的数据
            var itemListDic = itemList.ToDictionary(p => new { p.WarehouseId, p.ItemId, p.ItemExtProp }, p => p);
            foreach (var cate in catelist)
            {
                var key = new { cate.WarehouseId, cate.ItemId, cate.ItemExtProp };
                if (!itemListDic.ContainsKey(key))
                {
                    prepareItemDatas.Add(cate);
                }
            }
            return prepareItemDatas;
        }

        /// <summary>
        /// 排除已建备料单的物料
        /// </summary>
        /// <param name="prepareItemPullDatas">原备料模式数据</param>
        /// <param name="wareIds">仓库Ids</param>
        /// <returns></returns>
        private List<PrepareItemData> ExceptSameWarehouseDetails(List<PrepareItemData> prepareItemPullDatas, List<double> wareIds)
        {
            List<PrepareItemData> exceptPreItemPullDatas = new List<PrepareItemData>();
            List<MaterialPreDetailInfo> materialPreDetailInfos = new List<MaterialPreDetailInfo>();
            wareIds.SplitDataExecute(temps =>
            {
                var preDetails = Query<MaterialPreparationDetail>().LeftJoin<MaterialPreparation>((mpd, mp) => mpd.MaterialPreparationId == mp.Id)
                .Where<MaterialPreparation>((mpd, mp) => mp.LineSideWarehouseId != null && temps.Contains((double)mp.LineSideWarehouseId))
                .Where<MaterialPreparationDetail>((mpd, mp) => mpd.PreDetailStatus == MaterialPreparations.Enums.PrepareDetailStatus.ToShipping || mpd.PreDetailStatus == MaterialPreparations.Enums.PrepareDetailStatus.ToReceive || mpd.PreDetailStatus == MaterialPreparations.Enums.PrepareDetailStatus.PartReceive)
                .Select(mpd => new
                {
                    ItemId = mpd.ItemId,
                    ItemExtProp = mpd.ItemExtProp,
                }).Distinct().ToList<MaterialPreDetailInfo>();
                materialPreDetailInfos.AddRange(preDetails);
            });
            var detailDic = materialPreDetailInfos.ToDictionary(p => new { ItemId = p.ItemId, ItemExtProp = p.ItemExtProp });
            foreach (var data in prepareItemPullDatas)
            {
                var key = new { ItemId = (double)data.ItemId, ItemExtProp = data.ItemExtProp };
                if (!detailDic.ContainsKey(key))
                {
                    exceptPreItemPullDatas.Add(data);
                }
            }
            return exceptPreItemPullDatas;
        }

        /// <summary>
        /// 获取产线线边仓基础数据
        /// </summary>
        /// <param name="wareIds"></param>
        /// <returns></returns>
        private List<LinesideWareBaseData> GetLinesideWareBaseDatas(List<double> wareIds)
        {
            List<LinesideWareBaseData> linesideWareBaseDatas = new List<LinesideWareBaseData>();
            wareIds.SplitDataExecute(tempIds =>
            {
                var list = Query<LinesideWarehouse>()
                    .Where(p => tempIds.Contains(p.WarehouseId))
                    .Select(p => new
                    {
                        WorkShopId = p.WorkShopId,
                        WarehouseId = p.WarehouseId,
                        FactoryId = p.FactoryId,
                    }).Distinct().ToList<LinesideWareBaseData>();
                linesideWareBaseDatas.AddRange(list);
            });
            return linesideWareBaseDatas;
        }

        /// <summary>
        /// 创建备料需求单
        /// </summary>
        /// <param name="wareId">接收仓库</param>
        /// <param name="factoryId">工厂</param>
        /// <param name="workShopId">车间</param>
        /// <returns></returns>
        private MaterialPreparation CreateMaterialPreparation(double wareId, double factoryId, double workShopId)
        {
            MaterialPreparation materialPreparation = new MaterialPreparation
            {
                PrepareType = MaterialPreparations.Enums.PrepareType.Sfmr,
                FactoryId = factoryId,
                WorkShopId = workShopId,
                LineSideWarehouseId = wareId,
                Reason = "备料模式-拉式调度".L10N(),
            };
            return materialPreparation;
        }

        /// <summary>
        /// 创建备料需求明细
        /// </summary>
        /// <param name="materialPreparation">表头</param>
        /// <param name="lineNo">行号</param>
        /// <param name="itemId">物料id</param>
        /// <param name="itemExtProp">物料扩展属性id</param>
        /// <param name="itemExtPropName">物料扩展属性</param>
        /// <param name="qty">本次备料数</param>
        /// <returns></returns>
        private MaterialPreparationDetail CreateMaterialPreparationDetail(MaterialPreparation materialPreparation, int lineNo, double itemId, string itemExtProp, string itemExtPropName, decimal qty)
        {
            MaterialPreparationDetail materialPreparationDetail = new MaterialPreparationDetail
            {
                MaterialPreparation = materialPreparation,
                LineNo = lineNo.ToString(),
                ItemId = itemId,
                ItemExtProp = itemExtProp,
                ItemExtPropName = itemExtPropName,
                Qty = qty,
            };
            return materialPreparationDetail;
        }

        /// <summary>
        /// 拉式调度生成备料需求单
        /// </summary>
        /// <returns></returns>
        public virtual PrepareItemPullResult PullJobCreatePreparation()
        {
            PrepareItemPullResult prepareItemPullResult = new PrepareItemPullResult();
            //1.获取产线线边仓仓库的备料模式-拉式
            //2.把物料类型转为物料
            List<PrepareItemData> prepareItemPullDatas = GetPullPrepareItemDatas();
            if (!prepareItemPullDatas.Any())
            {
                return prepareItemPullResult;
            }

            //累计匹配（xxx）笔（仓库+物料）的数据
            prepareItemPullResult.DataCount = prepareItemPullDatas.Count;

            //3.排除已建备料需求单明细物料(同接收仓库下状态为待接收、待发运、部分接收的数据)
            List<double> wareIds = prepareItemPullDatas.Select(p => p.WarehouseId).Distinct().ToList();
            var exceptPreItemPullDatas = ExceptSameWarehouseDetails(prepareItemPullDatas, wareIds);
            if (!exceptPreItemPullDatas.Any())
            {
                return prepareItemPullResult;
            }

            List<double> itemIds = exceptPreItemPullDatas.Select(p => (double)p.ItemId).Distinct().ToList();
            List<LinesideWareBaseData> linesideWareBaseDatas = GetLinesideWareBaseDatas(wareIds);
            var linesideBaseLookUp = linesideWareBaseDatas.ToLookup(p => p.WarehouseId);

            //4.根据仓库+触发方式+需求计算方式GroupBy
            //5.校验物料lpn库存是否低于安全水位
            //6.需求计算方式计算备料量(最高存量: 最高存量-lpn  固定量: 固定量)
            List<LotLpnOnhandDataInfo> lpnInfoList = RT.Service.Resolve<IGetLotLpnOnhand>().GetLotLpnOnhandByItemIds(wareIds, itemIds);
            var lpnDic = lpnInfoList.ToDictionary(p => new { WarehouseId = (double)p.WarehouseId, ItemId = p.ItemId, p.ItemExtProp }, p => p.AvailableQty);

            var preByDic = exceptPreItemPullDatas.GroupBy(p => new { WarehouseId = p.WarehouseId, TriggerType = p.TriggerType, DemandType = p.DemandType }).ToDictionary(p => p.Key, p => p.ToList());

            EntityList<MaterialPreparation> mpSaveList = new EntityList<MaterialPreparation>();
            EntityList<MaterialPreparationDetail> mpDetailSaveList = new EntityList<MaterialPreparationDetail>();

            foreach (var key in preByDic.Keys)
            {
                if (preByDic.TryGetValue(key, out var prePullDatas)) // 同仓库+同触发方式+同需求计算方式生成一张备料单
                {
                    MaterialPreparation materialPreparation = new MaterialPreparation();
                    var lineInfo = linesideBaseLookUp[key.WarehouseId].FirstOrDefault(p => p.WorkShopId != null);
                    if (lineInfo != null)
                    {
                        materialPreparation = CreateMaterialPreparation(lineInfo.WarehouseId, lineInfo.FactoryId.Value, lineInfo.WorkShopId.Value);
                    }
                    else
                    {
                        continue;
                    }
                    // 本单明细
                    List<MaterialPreparationDetail> detailList = new List<MaterialPreparationDetail>();
                    int lineNo = 1;
                    foreach (var data in prePullDatas)
                    {
                        lpnDic.TryGetValue(new { key.WarehouseId, data.ItemId, data.ItemExtProp }, out decimal lpnQty);
                        if (data.TriggerType == TriggerMode.BelowSafe && lpnQty >= data.LowestStage) // 安全水位触发
                        {
                            continue;
                        }
                        //满足触发方式的共有（XXX）笔数据
                        prepareItemPullResult.FitDataCount += 1;
                        decimal qty = 0;
                        if (data.DemandType == Commons.DemandMode.MaxStock)
                        {
                            qty = (data.MaxStock ?? 0) - lpnQty;
                        }
                        else if (data.DemandType == Commons.DemandMode.FixedQuantity)
                        {
                            qty = data.FixedQuantity ?? 0;
                        }
                        if (qty <= 0)
                        {
                            continue;
                        }
                        //合计生成（XXX）个备料需求
                        prepareItemPullResult.PrepareDataCount += 1;
                        // 创建备料明细
                        var detail = CreateMaterialPreparationDetail(materialPreparation, lineNo, data.ItemId.Value, data.ItemExtProp, data.ItemExtPropName, qty);
                        lineNo++;
                        detailList.Add(detail);
                    }

                    if (detailList.Count > 0) // 备料单存在备料明细
                    {
                        mpSaveList.Add(materialPreparation);
                        mpDetailSaveList.AddRange(detailList);
                    }
                }
            }

            if (mpSaveList.Any())
            {
                // 生成（XXX）个备料单     
                prepareItemPullResult.StockOrderCount = mpSaveList.Count;
                // 生成备料单号
                var nos = RT.Service.Resolve<MaterialPreparationController>().GetMaterialPreprationNo(mpSaveList.Count);
                for (var i = 0; i < mpSaveList.Count; i++)
                {
                    mpSaveList[i].No = nos[i];
                }

                using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
                {
                    RT.Service.Resolve<CommonController>().BatchInsertSave(mpSaveList);
                    mpDetailSaveList.ForEach(detail =>
                    {
                        detail.MaterialPreparationId = detail.MaterialPreparation.Id;
                    });
                    RT.Service.Resolve<CommonController>().BatchInsertSave(mpDetailSaveList);

                    // 自动做提交
                    var preIds = mpSaveList.Select(p => p.Id).ToList();
                    RT.Service.Resolve<MaterialPreparationController>().SubmitPreparationOrder(preIds);
                    tran.Complete();
                }
            }

            return prepareItemPullResult;
        }
        #endregion

        #region 推式调度

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<PrepareItemData> GetPushPrepareItemDatas()
        {
            List<PrepareItemData> prepareItemDatas = new List<PrepareItemData>();
            // 查询备料模式-推式
            List<TriggerMode?> triggerModes = new List<TriggerMode?> { TriggerMode.XHoursBefore, TriggerMode.InvBelowSafeLevelToBeat };
            // 物料类型
            var itemList = Query<PrepareItemPush>()
                .Where(pip => pip.PrepareItemType == PrepareItemType.Push && triggerModes.Contains(pip.TriggerType) && pip.DemandType != DemandMode.ManualFillIn
                && pip.ItemId != null)
                .Select(pip => new
                {
                    WipResourceId = pip.WipResourceId,
                    ItemCategoryId = pip.ItemCategoryId,
                    ItemId = pip.ItemId,
                    TriggerType = pip.TriggerType,
                    DemandType = pip.DemandType,
                    FixedQuantity = pip.FixedQuantity,
                    ItemExtProp = pip.ItemExtProp,
                    AdvanceHours = pip.AdvanceHours,
                    SatisfactionTime = pip.SatisfactionTime,
                    PreparationTime = pip.PreparationTime,
                }).ToList<PrepareItemData>();
            prepareItemDatas.AddRange(itemList);

            // 分类
            var catelist = Query<PrepareItemPush>()
                .Where(pip => pip.PrepareItemType == PrepareItemType.Push && triggerModes.Contains(pip.TriggerType) && pip.DemandType != DemandMode.ManualFillIn
                && pip.ItemCategoryId != null && pip.ItemId == null)
                .LeftJoin<ItemCategoryRelation>((pip, icr) => pip.ItemCategoryId == icr.ItemCategoryId)
                .LeftJoin<ItemCategoryRelation, Item>((icr, i) => icr.ItemId == i.Id)
                .Where<Item>((pip, i) => i.ConsumeMode == ConsumeMode.Push)
                .Where<ItemCategoryRelation>((pip, icr) => icr.Type == Items.Items.CategoryType.Item)
                .Select<ItemCategoryRelation>((pip, icr) => new
                {
                    WipResourceId = pip.WipResourceId,
                    ItemCategoryId = pip.ItemCategoryId,
                    ItemId = icr.ItemId,
                    TriggerType = pip.TriggerType,
                    DemandType = pip.DemandType,
                    FixedQuantity = pip.FixedQuantity,
                    ItemExtProp = pip.ItemExtProp,
                    AdvanceHours = pip.AdvanceHours,
                    SatisfactionTime = pip.SatisfactionTime,
                    PreparationTime = pip.PreparationTime,
                }).ToList<PrepareItemData>();
            // 物料分类去除重复的数据
            var itemListDic = itemList.ToDictionary(p => new { p.WipResourceId, p.ItemId, p.ItemExtProp }, p => p);
            foreach (var cate in catelist)
            {
                var key = new { cate.WipResourceId, cate.ItemId, cate.ItemExtProp };
                if (!itemListDic.ContainsKey(key))
                {
                    prepareItemDatas.Add(cate);
                }
            }

            return prepareItemDatas;
        }

        /// <summary>
        /// 排除已备料明细物料
        /// </summary>
        /// <param name="woBomInfo">备料模式数据</param>
        /// <param name="woIds">工单Ids</param>
        /// <returns></returns>
        private List<WoBomPushPreInfo> ExceptExistsPreDetails(List<WoBomPushPreInfo> woBomInfo, List<double> woIds)
        {
            List<WoBomPushPreInfo> exceptWoBomInfo = new List<WoBomPushPreInfo>();
            List<MaterialPreDetailInfo> materialPreDetailInfos = new List<MaterialPreDetailInfo>();
            woIds.SplitDataExecute(tempWoIds =>
            {
                var list = Query<MaterialPreparationDetail>().LeftJoin<MaterialPreparation>((mpd, mp) => mpd.MaterialPreparationId == mp.Id)
                    .Where<MaterialPreparation>((mpd, mp) => mp.WorkOrderId != null && tempWoIds.Contains((double)mp.WorkOrderId))
                    .Where<MaterialPreparationDetail>((mpd, mp) => mpd.PreDetailStatus == MaterialPreparations.Enums.PrepareDetailStatus.ToShipping || mpd.PreDetailStatus == MaterialPreparations.Enums.PrepareDetailStatus.ToReceive || mpd.PreDetailStatus == MaterialPreparations.Enums.PrepareDetailStatus.PartReceive)
                    .Select<MaterialPreparation>((mpd, mp) => new
                    {
                        WorkOrderId = mp.WorkOrderId,
                        LineNo = mpd.LineNo,
                        ItemId = mpd.ItemId,
                        ItemExtProp = mpd.ItemExtProp,
                    }).Distinct().ToList<MaterialPreDetailInfo>();
                materialPreDetailInfos.AddRange(list);
            });
            var detailDic = materialPreDetailInfos.GroupBy(p => new { WorkOrderId = p.WorkOrderId, LineNo = p.LineNo, ItemId = p.ItemId, ItemExtProp = p.ItemExtProp }).ToDictionary(p => p.Key, p => p.Count());
            foreach (var data in woBomInfo)
            {
                var key = new { WorkOrderId = data.WorkOrderId, LineNo = data.LineNo, ItemId = (double)data.ItemId, ItemExtProp = data.ItemExtProp };
                if (!detailDic.ContainsKey(key))
                {
                    exceptWoBomInfo.Add(data);
                }
            }
            return exceptWoBomInfo;
        }

        /// <summary>
        /// 返回较小数（限制最高库存）
        /// </summary>
        /// <param name="isLimited">是否限制最高库存</param>
        /// <param name="qty">数量</param>
        /// <param name="maxStockQty">最高库存</param>
        /// <returns></returns>
        private decimal GetMinValue(bool isLimited, decimal qty, decimal maxStockQty)
        {
            return isLimited && qty > maxStockQty ? maxStockQty : qty;
        }

        /// <summary>
        /// 计算本次备料量
        /// </summary>
        /// <param name="canPrepareQty">可备料量</param>
        /// <param name="woBomPushPreInfo">工单bom信息</param>
        /// <param name="prepareItemData">推式备料规则</param>
        /// <param name="isLimited">是否限制最高库存</param>
        /// <returns></returns>
        private decimal CalculateQty(decimal canPrepareQty, WoBomPushPreInfo woBomPushPreInfo, PrepareItemData prepareItemData, bool isLimited)
        {
            decimal qty = 0;
            var time = prepareItemData.PreparationTime ?? 0;
            var workHour = woBomPushPreInfo.ResourceMeter ?? woBomPushPreInfo.ProductModelMeter ?? 0;
            var meter = workHour * woBomPushPreInfo.SingleQty;
            switch (prepareItemData.DemandType)
            {
                case DemandMode.WoSurplusQty:
                    qty = GetMinValue(isLimited, canPrepareQty, woBomPushPreInfo.MaxStockQty);
                    break;
                case DemandMode.StockToSafeLevelForBeat:
                    var toSafeQty = canPrepareQty < time * meter - woBomPushPreInfo.WoReportAvailableQty ? canPrepareQty : time * meter - woBomPushPreInfo.WoReportAvailableQty;
                    qty = GetMinValue(isLimited, toSafeQty, woBomPushPreInfo.MaxStockQty);
                    break;
                case DemandMode.StockIsSafeLevelForBeat:
                    var safeQty = canPrepareQty < time * meter ? canPrepareQty : time * meter;
                    qty = GetMinValue(isLimited, safeQty, woBomPushPreInfo.MaxStockQty);
                    break;
                case DemandMode.FixedQuantity:
                    var fixQty = canPrepareQty < (prepareItemData.FixedQuantity.Value) ? canPrepareQty : prepareItemData.FixedQuantity.Value;
                    qty = GetMinValue(isLimited, fixQty, woBomPushPreInfo.MaxStockQty);
                    break;
            }
            return qty;
        }

        /// <summary>
        /// 生成备料明细
        /// </summary>
        /// <param name="materialPreparation">备料需求单</param>
        /// <param name="woBomPushPreInfo">工单bom明细</param>
        /// <param name="helper">计算类</param>
        /// <param name="prepareItemData">需求方式计算方式</param>
        /// <param name="isLimited">是否限制最高库存</param>
        /// <returns></returns>
        private MaterialPreparationDetail CreateMaterialPreparationDetail(MaterialPreparation materialPreparation, WoBomPushPreInfo woBomPushPreInfo, CalculateQtyHelper helper, PrepareItemData prepareItemData, bool isLimited)
        {
            var detail = new MaterialPreparationDetail
            {
                MaterialPreparation = materialPreparation,
                PreDetailStatus = MaterialPreparations.Enums.PrepareDetailStatus.Created,
                LineNo = woBomPushPreInfo.LineNo,
                ItemId = woBomPushPreInfo.ItemId,
                ItemExtProp = woBomPushPreInfo.ItemExtProp,
                ItemExtPropName = woBomPushPreInfo.ItemExtPropName,
                MpWoId = woBomPushPreInfo.WorkOrderId,
                MpWo = woBomPushPreInfo.WorkOrderNo,
            };
            // 计算可备料量
            WoBomInfo woBom = new WoBomInfo
            {
                LineNo = woBomPushPreInfo.LineNo,
                ItemId = woBomPushPreInfo.ItemId,
                ItemExtProp = woBomPushPreInfo.ItemExtProp,
                BomNeedQty = woBomPushPreInfo.BomNeedQty,
            };
            var canPrepareQty = helper.CalculateCanQty(woBomPushPreInfo.WorkOrderId, woBom);
            // 计算需求方式
            detail.Qty = CalculateQty(canPrepareQty, woBomPushPreInfo, prepareItemData, isLimited);
            return detail;
        }

        /// <summary>
        /// 计算bom的现有量(工单占用库存)以及最大库存限制
        /// </summary>
        /// <param name="workOrderIds">工单</param>
        /// <param name="itemIds">物料</param>
        /// <param name="resourceIds">最高库存</param>
        /// <param name="workOrderBomList">工单bom</param>
        /// <param name="isLimited">是否限制最高库存</param>
        private void CalculateOther(List<double> workOrderIds, List<double> itemIds, List<double> resourceIds, List<WoBomPushPreInfo> workOrderBomList, bool isLimited)
        {
            List<WoDemandInfo> woDemandReports = new List<WoDemandInfo>();
            workOrderIds.SplitDataExecute(tempWIds =>
            {
                itemIds.SplitDataExecute(tempIds =>
                {
                    // 正常情况工单产线对应线边仓不会中途修改
                    var list = Query<WoDemandReport>()
                    .Where(p => p.WorkOrderId != null && tempWIds.Contains((double)p.WorkOrderId) && tempIds.Contains(p.ItemId))
                    .Select(p => new
                    {
                        WorkOrderId = p.WorkOrderId,
                        ItemId = p.ItemId,
                        ItemExtProp = p.ItemExtProp,
                        ReceivedQty = p.ReceivedQty,
                        MovedInQty = p.MovedInQty,
                        FeedQty = p.FeedQty,
                        MovedOutQty = p.MovedOutQty,
                        ReturnQtyInTransit = p.ReturnQtyInTransit,
                        NgReturnQtyInTransit = p.NgReturnQtyInTransit,
                        ReturnQty = p.ReturnQty,
                        NgReturnQty = p.NgReturnQty,
                    })
                    .ToList<WoDemandInfo>();
                    woDemandReports.AddRange(list);
                });
            });
            var woDemandReportDic = woDemandReports.ToDictionary(p => new { WorkOrderId = p.WorkOrderId, ItemId = p.ItemId, ItemExtProp = p.ItemExtProp });

            // 线边仓
            List<LinesideWareBaseData> linesideWareBaseDatas = new List<LinesideWareBaseData>();
            resourceIds.SplitDataExecute(tempRIds =>
            {
                var list = Query<LinesideWarehouse>()
                .Where(p => p.WipResouceId != null && tempRIds.Contains((double)p.WipResouceId))
                .Select(p => new
                {
                    WorkShopId = p.WorkShopId,
                    WipResouceId = p.WipResouceId,
                    FactoryId = p.FactoryId,
                    WarehouseId = p.WarehouseId,
                    StorageLocationId = p.StorageLocationId,
                }).ToList<LinesideWareBaseData>();
                linesideWareBaseDatas.AddRange(list);
            });
            var linesideWareBaseDataDic = linesideWareBaseDatas.ToDictionary(p => new { WorkShopId = p.WorkShopId, ResourceId = p.WipResouceId });

            // 最高库存
            List<ItemIoInfo> itemMaxStockList = new List<ItemIoInfo>();
            if (isLimited)
            {
                itemIds.SplitDataExecute(tempIds =>
                {
                    var list = Query<BaseItemIoLimit>().Where(p => tempIds.Contains(p.ItemId))
                    .Select(p => new
                    {
                        ItemId = p.ItemId,
                        ItemExtPtop = p.ItemExtProp,
                        ItemExrPropName = p.ItemExtPropName,
                        WarehouseId = p.WarehouseId,
                        MaxStockQty = p.MaxStockQty,
                    }).ToList<ItemIoInfo>();
                    itemMaxStockList.AddRange(list);
                });
            }
            var itemMaxStockDic = itemMaxStockList.ToDictionary(p => "{0}@{1}@{2}".FormatArgs(p.ItemId, p.ItemExtProp, p.WarehouseId));

            foreach (var bom in workOrderBomList)
            {
                // 现有量
                var demandKey = new { WorkOrderId = bom.WorkOrderId, ItemId = bom.ItemId, ItemExtProp = bom.ItemExtProp };
                if (woDemandReportDic.TryGetValue(demandKey, out var woDemandReport))
                {
                    bom.WoReportAvailableQty = woDemandReport.ReceivedQty + woDemandReport.MovedInQty - woDemandReport.FeedQty - woDemandReport.MovedOutQty - woDemandReport.ReturnQtyInTransit - woDemandReport.NgReturnQtyInTransit - woDemandReport.ReturnQty - woDemandReport.NgReturnQty;
                }
                // 线边仓库
                var lineKey = new { WorkShopId = bom.WorkShopId, ResourceId = bom.ResourceId };
                if (linesideWareBaseDataDic.TryGetValue(lineKey, out var lineside))
                {
                    bom.WarehouseId = lineside.WarehouseId;
                    // 最高库存
                    var stockKey = "{0}@{1}@{2}".FormatArgs(bom.ItemId, bom.ItemExtProp, bom.WarehouseId);
                    if (isLimited && itemMaxStockDic.TryGetValue(stockKey, out var io))
                    {
                        bom.MaxStockQty = io.MaxStockQty;
                    }
                }
            }
        }

        /// <summary>
        /// 推式调度生成备料需求单
        /// </summary>
        /// <returns></returns>
        public virtual PrepareItemPushResult PushJobCreatePreparation()
        {
            PrepareItemPushResult prepareItemPushResult = new PrepareItemPushResult();
            // 1.根据产线+物料+扩展属性获取推式物料属性
            // 2.把物料类型转为物料
            List<PrepareItemData> prepareItemPushDatas = GetPushPrepareItemDatas();
            if (!prepareItemPushDatas.Any())
            {
                return prepareItemPushResult;
            }
            // 累计匹配（XXX）（资源+物料+触发方式）的数据
            prepareItemPushResult.DataCount = prepareItemPushDatas.Count;

            // 3.按资源获取发放、生产中的非暂停的工单bom推式物料(以及节拍数)
            var itemIds = prepareItemPushDatas.Where(p => p.ItemId != null).Select(p => (double)p.ItemId).Distinct().ToList();
            var resourceIds = prepareItemPushDatas.Select(p => p.WipResourceId).Distinct().ToList();
            var workOrderBomList = RT.Service.Resolve<IWorkOrderQuery>().GetWoBomPushPreInfos(resourceIds, itemIds);
            var woIds = workOrderBomList.Select(p => p.WorkOrderId).Distinct().ToList();
            //共有（XXX）个发放的工单
            prepareItemPushResult.WoCount = woIds.Count;
            // 3.5 计算工单bom在线边库存的现有量以及最高库存
            // 是否限制
            bool isLimited = false;
            var config = ConfigService.GetConfig(new LimitedPrepareMaxConfig(), typeof(MaterialPreparation));
            if (config != null)
            {
                isLimited = config.IsLimited;
            }
            CalculateOther(woIds, itemIds, resourceIds, workOrderBomList, isLimited);
            var wareIds = workOrderBomList.Where(p => p.WarehouseId != null).Select(p => (double)p.WarehouseId).Distinct().ToList();

            // 4.排除已备料的备料需求明细(待发运、待接收、部分接收)
            var exceptworkOrderBomList = ExceptExistsPreDetails(workOrderBomList, woIds);
            //共有（XXX）个（工单+物料+触发方式）满足触发条件
            prepareItemPushResult.FitDataCount = exceptworkOrderBomList.Count();

            // 5.计算出工单剩余需求量
            var helper = new CalculateQtyHelper(woIds);
            helper.GetPreDetailDic();

            // 6.同一工单下触发方式+需求计算方式分组
            var woDic = exceptworkOrderBomList.GroupBy(p => new { p.WorkOrderId }).ToDictionary(p => p.Key, p => p.ToList());
            var time = RF.Find<JobConfig>().GetDbTime();

            EntityList<MaterialPreparation> saveMpList = new EntityList<MaterialPreparation>(); // 保存备料需求单
            EntityList<MaterialPreparationDetail> saveMpDetailList = new EntityList<MaterialPreparationDetail>(); // 保存备料明细
            var preparationModelDic = new Dictionary<Tuple<double, TriggerMode, DemandMode>, MaterialPreparation>(); // 同工单同触发方式同计算方式 创建备料需求单

            var prePushDataDic = prepareItemPushDatas.ToLookup(p => p.WipResourceId + "-" + p.ItemId + "-" + p.ItemExtProp);

            #region new
            foreach (var bom in exceptworkOrderBomList)
            {
                var key1 = bom.ResourceId + "-" + bom.ItemId + "-" + bom.ItemExtProp;
                var preData1List = prePushDataDic[key1];
                var preData = preData1List.FirstOrDefault(p => p.WipResourceId == bom.ResourceId && p.ItemId == bom.ItemId && p.ItemExtProp == bom.ItemExtProp);
                if (preData == null) continue;
                MaterialPreparation materialPreparation = null;
                var key = new Tuple<double, TriggerMode, DemandMode>(bom.WorkOrderId, preData.TriggerType, preData.DemandType);
                preparationModelDic.TryGetValue(key, out materialPreparation);
                if (materialPreparation == null) // 创建新的备料单
                {
                    materialPreparation = new MaterialPreparation
                    {
                        WorkOrderId = bom.WorkOrderId,
                        FactoryId = bom.FactoryId,
                        WorkShopId = bom.WorkShopId,
                        ResourceId = bom.ResourceId,
                        PrepareStatus = PrepareStatus.Saved,
                        PrepareType = PrepareType.Pmw,
                        Reason = "备料模式-推式调度".L10N(),
                        LineSideWarehouseId = bom.WarehouseId,
                    };
                }

                // 计划开始前:当前时间-工单计划开始时间 > 提前小时数 才执行
                if (preData.TriggerType == TriggerMode.XHoursBefore && (decimal)(time - bom.PlanBeginDate).TotalHours <= preData.AdvanceHours.Value)
                {
                    continue;
                }
                // 水位: 现有量 < 最短满足时间 * 节拍(工时*单机定额) 才执行
                var stfTime = preData.SatisfactionTime ?? 0;
                var workHour = bom.ResourceMeter ?? bom.ProductModelMeter ?? 0;
                var meter = workHour * bom.SingleQty;
                if (preData.TriggerType == TriggerMode.InvBelowSafeLevelToBeat && bom.WoReportAvailableQty >= stfTime * meter)
                {
                    continue;
                }
                saveMpDetailList.Add(CreateMaterialPreparationDetail(materialPreparation, bom, helper, preData, isLimited));
                //合计生成（XXX）个备料需求
                prepareItemPushResult.PrepareDataCount += 1;
                if (!preparationModelDic.ContainsKey(key))
                {
                    preparationModelDic.Add(key, materialPreparation);
                    saveMpList.Add(materialPreparation);
                }
            }
            #endregion

            #region old
            //foreach (var woId in woDic.Keys)
            //{
            //    // 工单Bom
            //    woDic.TryGetValue(woId, out var bomList);
            //    var woInfo = bomList.FirstOrDefault();
            //    if (woInfo == null) continue;
            //    var prepareItemPushDataDic = prepareItemPushDatas.Where(p => p.WipResourceId == woInfo.ResourceId).GroupBy(p => new { WipResourceId = p.WipResourceId, TriggerType = p.TriggerType, DemandType = p.DemandType }).ToDictionary(p => p.Key, p => p.ToList());
            //    foreach (var key in prepareItemPushDataDic.Keys) // 同工单同触发方式同需求方式
            //    {
            //        prepareItemPushDataDic.TryGetValue(key, out var prePushDatas);
            //        if (!prePushDatas.Any()) continue;
            //        // 本备料单明细
            //        List<MaterialPreparationDetail> detailList = new List<MaterialPreparationDetail>();
            //        MaterialPreparation materialPreparation = new MaterialPreparation
            //        {
            //            WorkOrderId = woInfo.WorkOrderId,
            //            FactoryId = woInfo.FactoryId,
            //            WorkShopId = woInfo.WorkShopId,
            //            ResourceId = woInfo.ResourceId,
            //            PrepareStatus = PrepareStatus.Saved,
            //            PrepareType = PrepareType.Pmw,
            //            Reason = "备料模式-推式调度".L10N(),
            //            LineSideWarehouseId = woInfo.WarehouseId,
            //        };
            //        var pushItemIds = prePushDatas.Select(p => p.ItemId).ToList();
            //        var pushDataBom = bomList.Where(p => pushItemIds.Contains(p.ItemId)).ToList();
            //        // 匹配工单bom
            //        foreach (var bom in pushDataBom)
            //        {
            //            var preData = prePushDatas.FirstOrDefault(p => p.ItemId == bom.ItemId && p.ItemExtProp == bom.ItemExtProp);
            //            if (preData == null) continue;
            //            // 计划开始前:当前时间-工单计划开始时间 > 提前小时数 才执行
            //            if (preData.TriggerType == TriggerMode.XHoursBefore && (decimal)(time - bom.PlanBeginDate).TotalHours <= preData.AdvanceHours.Value)
            //            {
            //                continue;
            //            }
            //            // 水位: 现有量 < 最短满足时间 * 节拍(工时*单机定额) 才执行
            //            var stfTime = preData.SatisfactionTime ?? 0;
            //            var workHour = bom.ResourceMeter ?? bom.ProductModelMeter ?? 0;
            //            var meter = workHour * bom.SingleQty;
            //            if (preData.TriggerType == TriggerMode.InvBelowSafeLevelToBeat && bom.WoReportAvailableQty >= stfTime * meter)
            //            {
            //                continue;
            //            }
            //            detailList.Add(CreateMaterialPreparationDetail(materialPreparation, bom, helper, preData, isLimited));
            //            //合计生成（XXX）个备料需求
            //            prepareItemPushResult.PrepareDataCount += 1;
            //        }

            //        if (detailList.Any())
            //        {
            //            saveMpList.Add(materialPreparation);
            //            saveMpDetailList.AddRange(detailList);
            //        }
            //    }
            //}
            #endregion

            if (saveMpList.Any())
            {
                // 生成（XXX）个备料单     
                prepareItemPushResult.StockOrderCount = saveMpList.Count;
                // 生成备料单号
                var nos = RT.Service.Resolve<MaterialPreparationController>().GetMaterialPreprationNo(saveMpList.Count);
                for (var i = 0; i < saveMpList.Count; i++)
                {
                    saveMpList[i].No = nos[i];
                }

                using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
                {
                    RT.Service.Resolve<CommonController>().BatchInsertSave(saveMpList);
                    saveMpDetailList.ForEach(detail =>
                    {
                        detail.MaterialPreparationId = detail.MaterialPreparation.Id;
                    });
                    RT.Service.Resolve<CommonController>().BatchInsertSave(saveMpDetailList);

                    // 自动做提交
                    var preIds = saveMpList.Select(p => p.Id).ToList();
                    RT.Service.Resolve<MaterialPreparationController>().SubmitPreparationOrder(preIds);
                    tran.Complete();
                }
            }

            return prepareItemPushResult;
        }


        #endregion
    }
}
