using NPOI.Util;
using SIE.Api;
using SIE.Common.Catalogs;
using SIE.Core.ApiModels;
using SIE.Core.Common.Controllers;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.Common.WorkOrders;
using SIE.EventMessages.LES;
using SIE.Inventory.Onhands;
using SIE.Items;
using SIE.LES.MaterialMoves.ApiModels;
using SIE.LES.MaterialReturnApplys;
using SIE.LES.Reports;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.LES.MaterialMoves
{
    /// <summary>
    /// 工单挪料
    /// </summary>
    public partial class MaterialMoveRecordController : DomainController
    {
        /// <summary>
        /// 获取工单信息
        /// </summary>
        /// <param name="no">工单号</param>
        /// <param name="workShopId">车间Id</param>
        /// <param name="warehouseId">仓库Id</param>
        /// <returns></returns>
        [ApiService("获取工单信息")]
        [return: ApiReturn("List<MoveWoData> 工单信息")]
        public virtual List<MoveWoData> GetMoveWoDatas([ApiParameter("工单号")] string no, [ApiParameter("车间Id")] double? workShopId, [ApiParameter("仓库Id")] double? warehouseId)
        {
            var list = Query<WorkOrder>().WhereIf(no.IsNotEmpty(), wo => wo.No.Contains(no))
                .Where(wo => wo.State != WorkOrderState.Finish && wo.State != WorkOrderState.Close && wo.State != WorkOrderState.CancelRelease)
                .LeftJoin<WoDemandReport>((wo, wdr) => wo.Id == wdr.WorkOrderId)
                .Where<WoDemandReport>((wo, wdr) => wdr.WorkShopId != null)
                .WhereIf<WoDemandReport>(warehouseId != null, (wo, wdr) => wdr.WarehouseId == warehouseId)
                .WhereIf<WoDemandReport>(workShopId != null, (wo, wdr) => wdr.WorkShopId == workShopId)
                .LeftJoin<WoDemandReport, Warehouse>((wdr, wh) => wdr.WarehouseId == wh.Id)
                .LeftJoin<WoDemandReport, Enterprise>((wdr, e) => wdr.WorkShopId == e.Id)
                .Select<WoDemandReport, Warehouse, Enterprise>((wo, wdr, wh, e) => new
                {
                    WoId = wo.Id,
                    WoNo = wo.No,
                    WorkShopId = wdr.WorkShopId,
                    WorkShopName = e.Name,
                    WarehouseId = wdr.WarehouseId,
                    WarehouseName = wh.Name,
                    CreateDate = wo.CreateDate,
                }).OrderByDescending(wo => wo.CreateDate).Distinct().ToList<MoveWoData>().ToList();
            return list;
        }

        /// <summary>
        /// 查询挪料原因
        /// </summary>
        /// <returns></returns>
        [ApiService("查询挪料原因")]
        [return: ApiReturn("List<BaseDataInfo> 挪料原因")]
        public virtual List<BaseDataInfo> GetMoveReason()
        {
            List<BaseDataInfo> reasons = new List<BaseDataInfo>();
            var list = RT.Service.Resolve<CatalogController>().GetCatalogList(MaterialMoveRecord.MaterialMoveReasonStr);
            foreach (var item in list)
            {
                BaseDataInfo info = new BaseDataInfo
                {
                    Code = item.Code,
                    Name = item.Name,
                };
                reasons.Add(info);
            }
            reasons.Add(new BaseDataInfo
            {
                Code = "无".L10N(),
                Name = "无".L10N(),
            });
            return reasons;
        }

        /// <summary>
        /// 获取来源工单和目标工单Bom交集
        /// </summary>
        /// <param name="sourceWoId">来源工单Id</param>
        /// <param name="targetWoId">目标工单Id</param>
        /// <param name="workShopId">车间Id</param>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="itemCode">物料编码</param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [ApiService("获取来源工单和目标工单Bom交集")]
        [return: ApiReturn("MoveBomInfoWithCount bom数据源")]
        public virtual MoveBomInfoWithCount GetMoveBomInfos([ApiParameter("挪料工单Id")] double? sourceWoId,
            [ApiParameter("目标工单Id")] double? targetWoId, [ApiParameter("车间Id")] double? workShopId, [ApiParameter("挪料仓库Id")] double? warehouseId,
            [ApiParameter("物料编码")] string itemCode, [ApiParameter("页码")] int pageNumber, [ApiParameter("页面大小")] int pageSize)
        {
            if (sourceWoId == null && targetWoId == null)
            {
                throw new ValidationException("挪料工单和目标工单不能同时为空".L10N());
            }
            PagingInfo pagingInfo = new PagingInfo
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                IsNeedCount = true,
            };

            var query = Query<WorkOrderBom>().As("wb")
                .LeftJoin<Item>("i", (wb, i) => wb.ItemId == i.Id)
                .WhereIf<Item>(itemCode.IsNotEmpty(), (wb, i) => i.Code.Contains(itemCode))
                .LeftJoin<Item, Unit>("u", (i, u) => i.UnitId == u.Id);


            bool sourceToPublic = sourceWoId != null && targetWoId == null;// 挪料工单移至公共库存
            bool publicToTarget = sourceWoId == null && targetWoId != null;// 公共库存移至目标工单
            bool sourceToTarget = sourceWoId != null && targetWoId != null;// 挪料工单移至目标工单
            if (sourceToPublic)
            {
                query.Where(wb => wb.WorkOrderId == sourceWoId);
            }
            else if (sourceToTarget)
            {
                query.Where(wb => wb.WorkOrderId == sourceWoId)
                    .Join<WorkOrderBom>("wbo", (wb, wbo) => wb.Id != wbo.Id
                    && wb.ItemId == wbo.ItemId && wb.ItemExtProp.NVL("*") == wbo.ItemExtProp.NVL("*") && wbo.WorkOrderId == targetWoId);

            }
            else if (publicToTarget)
            {
                query.Where(wb => wb.WorkOrderId == targetWoId);
            }

            var count = query.Count();
            if (sourceToPublic && count <= 0)
            {
                throw new ValidationException("挪料工单BOM不存在可挪料的物料".L10N());
            }
            else if (publicToTarget && count <= 0)
            {
                throw new ValidationException("目标工单BOM不存在可挪料的物料".L10N());
            }
            else if (sourceToTarget && count <= 0)
            {
                throw new ValidationException("挪料工单BOM与目标工单BOM不存在相同的物料".L10N());
            }

            List<MoveBomInfo> list = new List<MoveBomInfo>();

            if (sourceToPublic || sourceToTarget)
            {
                list = query.LeftJoin<WoDemandReport>("wr", (wb, wr) => wr.WarehouseId == warehouseId && wr.WorkShopId == workShopId && wb.WorkOrderId == wr.WorkOrderId && wb.ItemId == wr.ItemId && wb.ItemExtProp.NVL("*") == wr.ItemExtProp.NVL("*"))
                    .Where<WoDemandReport>((wb, wr) =>
                    (wr.ReceivedQty + wr.MovedInQty - wr.FeedQty - wr.NgQty - wr.MovedOutQty - wr.ReturnQtyInTransit - wr.NgReturnQtyInTransit - wr.ReturnQty - wr.NgReturnQty) > 0).Select<WoDemandReport, Item, Unit>((wb, wr, i, u) => new
                    {
                        ItemId = wb.ItemId,
                        ItemCode = i.Code,
                        ItemName = i.Name,
                        ItemExtProp = wb.ItemExtProp,
                        ItemExtPropName = wb.ItemExtPropName,
                        UnitId = i.UnitId,
                        UnitName = u.Name,
                        WorkOrderId = wr.WorkOrderId,
                        WorkShopId = wr.WorkShopId,
                        WarehouseId = wr.WarehouseId,
                        ReceivedQty = wr.ReceivedQty,
                        MovedInQty = wr.MovedInQty,
                        FeedQty = wr.FeedQty,
                        NgQty = wr.NgQty,
                        MovedOutQty = wr.MovedOutQty,
                        ReturnQtyInTransit = wr.ReturnQtyInTransit,
                        NgReturnQtyInTransit = wr.NgReturnQtyInTransit,
                        ReturnQty = wr.ReturnQty,
                        NgReturnQty = wr.NgReturnQty
                    }).ToList<MoveBomInfo>(pagingInfo).ToList();
                foreach (var wr in list)
                {
                    wr.AvailableQty = wr.ReceivedQty + wr.MovedInQty - wr.FeedQty - wr.NgQty - wr.MovedOutQty - wr.ReturnQtyInTransit - wr.NgReturnQtyInTransit - wr.ReturnQty - wr.NgReturnQty;
                }
            }
            else
            {
                list.Clear();
                var q = query
                    .LeftJoin<LotLpnOnhand>("lpn", (wb, lpn) => wb.ItemId == lpn.ItemId && wb.ItemExtProp.NVL("*") == lpn.ItemExtProp.NVL("*"))
                    .Where<LotLpnOnhand>((wb, lpn) => lpn.State != OnhandState.None && lpn.AvailableQty > 0 && lpn.WarehouseId == warehouseId)
                    //.GroupBy<Item, Unit>((wb, i, u) => new { wb.ItemId, i.Code, Item_Name = i.Name, wb.ItemExtProp, wb.ItemExtPropName, i.UnitId, Unit_Name = u.Name })
                    .Select<LotLpnOnhand, Item, Unit>((wb, lpn, i, u) => new
                    {
                        ItemId = wb.ItemId,
                        ItemCode = i.Code,
                        ItemName = i.Name,
                        ItemExtProp = wb.ItemExtProp,
                        ItemExtPropName = wb.ItemExtPropName,
                        UnitId = i.UnitId,
                        UnitName = u.Name,
                        AvailableQty = lpn.AvailableQty,
                    }).ToList<MoveBomInfo>(pagingInfo)
                    .GroupBy(p => new {p.ItemId, p.ItemCode, p.ItemName, p.ItemExtProp, p.ItemExtPropName, p.UnitId, p.UnitName})
                    .ToDictionary(p => p.Key, p => p.Sum(x => x.AvailableQty));
                // 数据库适配问题在内存分组聚合，如果数据库适配且有性能问题则把groupby注释取消在数据库中分组聚合统计可用量

                // 扣减占用库存
                var woDemandDic = Query<WoDemandReport>().Where(wr => wr.WarehouseId == warehouseId)
                    .GroupBy(wr => new { wr.ItemId, wr.ItemExtProp })
                    .Select(wr => new
                    {
                        ItemId = wr.ItemId,
                        ItemExtProp = wr.ItemExtProp,
                        ReceivedQty = wr.ReceivedQty.SUM(),
                        MovedInQty = wr.MovedInQty.SUM(),
                        FeedQty = wr.FeedQty.SUM(),
                        NgQty = wr.NgQty.SUM(),
                        MovedOutQty = wr.MovedOutQty.SUM(),
                        ReturnQtyInTransit = wr.ReturnQtyInTransit.SUM(),
                        NgReturnQtyInTransit = wr.NgReturnQtyInTransit.SUM(),
                        ReturnQty = wr.ReturnQty.SUM(),
                        NgReturnQty = wr.NgReturnQty.SUM()
                    }).ToList<MoveBomInfo>().ToDictionary(p => new Tuple<double, string> (p.ItemId, p.ItemExtProp), p => p);
                foreach(var i in q.Keys)
                {
                    MoveBomInfo moveBomInfo = new MoveBomInfo
                    {
                        ItemId = i.ItemId,
                        ItemCode = i.ItemCode,
                        ItemName = i.ItemName,
                        ItemExtProp = i.ItemExtProp,
                        ItemExtPropName = i.ItemExtPropName,
                        UnitId = i.UnitId,
                        UnitName = i.UnitName,
                        AvailableQty = q[i],
                    };
                    if (woDemandDic.TryGetValue(new Tuple<double, string>(i.ItemId, i.ItemExtProp), out MoveBomInfo wr))
                    {
                        var availableQty = wr.ReceivedQty + wr.MovedInQty - wr.FeedQty - wr.NgQty - wr.MovedOutQty - wr.ReturnQtyInTransit - wr.NgReturnQtyInTransit - wr.ReturnQty - wr.NgReturnQty;
                        moveBomInfo.AvailableQty -= availableQty;
                    }
                    list.Add(moveBomInfo);
                }
            }
            MoveBomInfoWithCount moveBomInfoWithCount = new MoveBomInfoWithCount();
            moveBomInfoWithCount.Count = count;
            moveBomInfoWithCount.BomInfos.Clear();
            moveBomInfoWithCount.BomInfos.AddRange(list);
            return moveBomInfoWithCount;
        }

        /// <summary>
        /// 提交挪料
        /// </summary>
        /// <param name="sourceWoId">来源工单</param>
        /// <param name="targetWoId">目标工单</param>
        /// <param name="reason">挪料原因</param>
        /// <param name="warehouseId">挪料仓库</param>
        /// <param name="details">挪料明细</param>
        /// <param name="isHand">手动</param>
        [ApiService("提交挪料")]
        [return: ApiReturn("无")]
        public virtual void SubmitWorkOrderMove([ApiParameter("挪料工单Id")] double? sourceWoId, [ApiParameter("目标工单Id")] double? targetWoId, [ApiParameter("仓库Id")] double? warehouseId, [ApiParameter("原因")] string reason, [ApiParameter("挪料明细")] List<MoveBomInfo> details, [ApiParameter("来源类型")] bool isHand = true)
        {
            bool sourceToPublic = sourceWoId != null && targetWoId == null;// 挪料工单移至公共库存
            bool publicToTarget = sourceWoId == null && targetWoId != null;// 公共库存移至目标工单
            bool sourceToTarget = sourceWoId != null && targetWoId != null;// 挪料工单移至目标工单

            EntityList<MaterialMoveRecord> records = new EntityList<MaterialMoveRecord>();

            using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                foreach (var detail in details)
                {
                    if (sourceToPublic)
                    {
                        DB.Update<WoDemandReport>().Set(p => p.MovedOutQty, p => p.MovedOutQty + detail.MoveQty).Where(p => p.WorkOrderId == sourceWoId && p.ItemId == detail.ItemId && p.ItemExtProp == detail.ItemExtProp && p.WarehouseId == warehouseId).Execute();
                    }
                    else if (sourceToTarget)
                    {
                        DB.Update<WoDemandReport>().Set(p => p.MovedOutQty, p => p.MovedOutQty + detail.MoveQty).Where(p => p.WorkOrderId == sourceWoId && p.ItemId == detail.ItemId && p.ItemExtProp == detail.ItemExtProp && p.WarehouseId == warehouseId).Execute();
                        DB.Update<WoDemandReport>().Set(p => p.MovedInQty, p => p.MovedInQty + detail.MoveQty).Where(p => p.WorkOrderId == targetWoId && p.ItemId == detail.ItemId && p.ItemExtProp == detail.ItemExtProp && p.WarehouseId == warehouseId).Execute();
                    }
                    else if (publicToTarget)
                    {
                        DB.Update<WoDemandReport>().Set(p => p.MovedInQty, p => p.MovedInQty + detail.MoveQty).Where(p => p.WorkOrderId == targetWoId && p.ItemId == detail.ItemId && p.ItemExtProp == detail.ItemExtProp && p.WarehouseId == warehouseId).Execute();
                    }
                    MaterialMoveRecord materialMoveRecord = new MaterialMoveRecord
                    {
                        MoveType = Enums.MoveType.WorkOrder,
                        SourceWoId = sourceWoId,
                        TargetWoId = targetWoId,
                        ItemId = detail.ItemId,
                        ItemExtProp = detail.ItemExtProp,
                        ItemExtPropName = detail.ItemExtPropName,
                        Reason = reason,
                        MoveQty = detail.MoveQty,
                        WarehouseId = warehouseId,
                        MoveSourceType = isHand ? Enums.MoveSourceType.Hand : Enums.MoveSourceType.Auto
                    };
                    records.Add(materialMoveRecord);
                }
                RT.Service.Resolve<CommonController>().BatchInsertSave(records);
                //更新工单BOM
                UpdateWoBomQty(records);
                tran.Complete();
            }

        }

        /// <summary>
        /// 更新工单BOM
        /// </summary>
        /// <param name="records"></param>
        void UpdateWoBomQty(IList<MaterialMoveRecord> records)
        {
            records.Where(p => p.SourceWoId > 0).GroupBy(p => p.SourceWoId).ForEach(p =>
            {
                var woId = p.Key.Value;
                var itemIds = p.Select(p => p.ItemId).Distinct().ToList();
                RT.Service.Resolve<IWorkOrderUpdate>().UpdateWoBomQty(woId, itemIds);
            });

            records.Where(p => p.TargetWoId > 0).GroupBy(p => p.TargetWoId).ForEach(p =>
            {
                var woId = p.Key.Value;
                var itemIds = p.Select(p => p.ItemId).Distinct().ToList();
                RT.Service.Resolve<IWorkOrderUpdate>().UpdateWoBomQty(woId, itemIds);
            });
        }

        /// <summary>
        /// 扫描实物标签并转化
        /// </summary>
        /// <param name="label">实物标签</param>
        /// <returns></returns>
        [ApiService("扫描实物标签并转化")]
        [return: ApiReturn("扫描内容")]
        public virtual ScanLabelData GetScanLabelData([ApiParameter("实物标签")] string label)
        {
            var invorg = RT.InvOrg != null ? RT.InvOrg.Value : 1;
            var labelData = RT.Service.Resolve<ILesShippingOrder>().ScanLabel(invorg, label);
            if (labelData != null && labelData.Count > 0)
            {
                return labelData.First();
            }
            else
            {
                return new ScanLabelData();
            }
        }
    }
}
