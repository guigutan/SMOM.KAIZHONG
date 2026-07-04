using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using NPOI.SS.Formula.Functions;
using NPOI.Util;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Core.ApiModels;
using SIE.Core.Enums;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages;
using SIE.EventMessages.Common.WorkOrders;
using SIE.EventMessages.LES.Datas;
using SIE.EventMessages.MES.LoadItems;
using SIE.EventMessages.MES.WorkOrders;
using SIE.EventMessages.WMS.Inventory;
using SIE.Inventory.Commom;
using SIE.Inventory.Onhands;
using SIE.Items;
using SIE.LES.LinesideWarehouses;
using SIE.LES.LinesideWarehouses.Models;
using SIE.LES.MaterialPreparations;
using SIE.LES.MaterialPreparations.ViewModels;
using SIE.LES.MaterialReceives;
using SIE.LES.MaterialReturnApplys.ApiModels;
using SIE.LES.MaterialReturnApplys.Enums;
using SIE.LES.MaterialReturnApplys.ViewModels;
using SIE.LES.Reports;
using SIE.Packages.ItemLabels;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;

namespace SIE.LES.MaterialReturnApplys
{
    /// <summary>
    /// 退料申请控制器
    /// </summary>
    public partial class MaterialReturnApplyController : DomainController
    {

        #region 查询

        /// <summary>
        /// 查询退料申请
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns></returns>
        public virtual EntityList<MaterialReturnApply> GetMaterialReturnApplies(MaterialReturnApplyCriteria criteria)
        {
            if (criteria == null)
            {
                return new EntityList<MaterialReturnApply>();
            }
            var q = Query<MaterialReturnApply>();
            if (criteria.No.IsNotEmpty())
            {
                q.Where(p => p.No.Contains(criteria.No));
            }
            if (criteria.ReStatus.HasValue)
            {
                q.Where(p => p.ReStatus == criteria.ReStatus.Value);
            }
            if (criteria.WoNo.IsNotEmpty())
            {
                q.Where(p => p.WorkOrder.No.Contains(criteria.WoNo));
            }
            if (criteria.WorkShopId != null && criteria.WorkShopId != 0)
            {
                q.Where(p => p.WorkShopId == criteria.WorkShopId);
            }
            if (criteria.WipResourceId != null && criteria.WipResourceId != 0)
            {
                q.Where(p => p.WipResourceId == criteria.WipResourceId);
            }
            if (criteria.WarehouseId != null && criteria.WarehouseId != 0)
            {
                q.Where(p => p.WarehouseId == criteria.WarehouseId);
            }
            if (criteria.Reason.IsNotEmpty())
            {
                q.Where(p => p.Reason == criteria.Reason);
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                q.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                q.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            }
            return q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询工单
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns></returns>
        public virtual EntityList<MaterialReturnWoViewModel> GetMaterialReturnWoViewModels(MaterialReturnWoCriteria criteria)
        {
            if (criteria == null)
            {
                return new EntityList<MaterialReturnWoViewModel>();
            }
            EntityList<MaterialReturnWoViewModel> viewmodel = new EntityList<MaterialReturnWoViewModel>();
            var workOrderInfoWithCount = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderList(criteria.WoNo, criteria.FactoryId, criteria.WorkshopId, criteria.WipResourceId, criteria.ProductCode, criteria.ProductName, true, false, null, null, criteria.PagingInfo);
            var workInfoList = workOrderInfoWithCount.WorkOrderInfos;
            foreach (var workInfo in workInfoList)
            {
                MaterialReturnWoViewModel view = new MaterialReturnWoViewModel
                {
                    Id = workInfo.WorkOrderId,
                    WoNo = workInfo.WorkOrderNo,
                    FactoryId = workInfo.FactoryId,
                    Factory = workInfo.FactoryCode,
                    WorkshopId = workInfo.WorkShopId,
                    Workshop = workInfo.WorkShopCode,
                    WipResourceId = workInfo.ResourceId,
                    WipResource = workInfo.ResourceCode,
                    ProductId = workInfo.ProductId,
                    ProductCode = workInfo.ProductCode,
                    ProductName = workInfo.ProductName,
                    PlanQty = workInfo.PlanQty,
                    WoState = workInfo.State,
                    PlanBeginDate = workInfo.PlanBeginDate,
                    PlanEndDate = workInfo.PlanEndDate,
                    ProjectId = workInfo.ProjectMaintainId,
                    ProjectCode = workInfo.ProjectMaintainCode,
                };
                viewmodel.Add(view);
            }
            viewmodel.SetTotalCount(workOrderInfoWithCount.TotalCount);
            return viewmodel;
        }

        /// <summary>
        /// 根据车间产线获取产线线边仓
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="wipResourceId">产线Id</param>
        /// <returns></returns>
        public virtual LinesideWareBaseData GetLinesideWarehouse(double? workShopId, double? wipResourceId)
        {
            var query = Query<LinesideWarehouse>()
                .LeftJoin<Warehouse>((lw, w) => lw.WarehouseId == w.Id)
                .LeftJoin<StorageLocation>((lw, s) => lw.StorageLocationId == s.Id)
                .Where(lw => lw.WorkShopId == workShopId && lw.WipResouceId == wipResourceId)
                .Select<Warehouse, StorageLocation>((lw, w, s) => new
                {
                    WarehouseId = w.Id,
                    WarehouseName = w.Name,
                    StorageLocationId = s.Id,
                    StorageLocationName = s.Name,
                }).FirstOrDefault<LinesideWareBaseData>();
            return query ?? new LinesideWareBaseData();
        }

        /// <summary>
        /// 依据仓库id获取退料明细
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="itemCode">物料编码</param>
        /// <param name="lot">批次号</param>
        /// <param name="wareId">仓库</param>
        /// <param name="storageId">库位</param>
        /// <param name="woId">工单</param>
        /// <param name="projectNo">项目号</param>
        /// <returns></returns>
        private EntityList<MaterialReturnApplyDetailSelect> GetDetailWithItemLabel(PagingInfo pagingInfo, string itemCode, string lot, double wareId, double storageId, double? woId, string projectNo)
        {
            EntityList<MaterialReturnApplyDetailSelect> materialReturnApplyDetailSelects = new EntityList<MaterialReturnApplyDetailSelect>();
            projectNo = projectNo.IsNullOrEmpty() ? "*" : projectNo;
            var query = Query<WoDemandReport>().Where(w => w.WarehouseId == wareId &&
                (w.ReceivedQty + w.MovedInQty - w.FeedQty - w.MovedOutQty - w.ReturnQtyInTransit - w.NgReturnQtyInTransit - w.ReturnQty - w.NgReturnQty + w.NgQty) > 0) // 线边可用数 > 0
                .WhereIf(woId != null, w => w.WorkOrderId == woId)
                .WhereIf(woId == null, w => w.WorkOrderId == null)
                .Join<ItemLabel>((w, l) => w.ItemId == l.ItemId && w.ItemExtProp.NVL("*") == l.ItemExtProp.NVL("*") && w.WarehouseId.NVL(0) == l.WarehouseId.NVL(0) 
                && l.Qty + l.NgQty > 0 && l.StorageLocationId == storageId && l.ProjectNo == projectNo)  // 物料标签可用数+不良数 > 0
                .WhereIf<ItemLabel>(lot.IsNotEmpty(), (w, l) => l.Lot == lot)
                .LeftJoin<Item>((w, i) => w.ItemId == i.Id)
                .WhereIf<Item>(itemCode.IsNotEmpty(), (w, i) => i.Code.Contains(itemCode))
                .LeftJoin<Item, ItemStockDataBase>((i, isd) => i.Id == isd.ItemId)
                .LeftJoin<Item, Unit>((i, u) => i.UnitId == u.Id)
                .Select<ItemLabel, Item, ItemStockDataBase, Unit>((w, l, i, isd, u) => new
                {
                    WoDemandReportId = w.Id,
                    ItemId = w.ItemId,
                    ItemCode = i.Code,
                    ItemName = i.Name,
                    UnitName = u.Name,
                    EnableExtendProperty = i.EnableExtendProperty,
                    IsBatch = isd.IsBatch,
                    IsSeri = isd.IsSerialNumber,
                    ItemExtProp = w.ItemExtProp,
                    ItemExtPropName = w.ItemExtPropName,
                    ItemLabelId = l.Id,
                    Label = l.Label,
                    Lot = l.Lot,
                    LabelQty = l.Qty,
                    LabelNgQty = l.NgQty,
                    ReceivedQty = w.ReceivedQty,
                    MovedInQty = w.MovedInQty,
                    FeedQty = w.FeedQty,
                    MovedOutQty = w.MovedOutQty,
                    ReturnQtyInTransit = w.ReturnQtyInTransit,
                    NgReturnQtyInTransit = w.NgReturnQtyInTransit,
                    WoReturnQty = w.ReturnQty,
                    WoNgReturnQty = w.NgReturnQty,
                    NgQty = w.NgQty,
                    CanExtProp = false,
                });
            var totalCount = query.Count();
            var list = query.ToList<MaterialReturnApplyDetailSelect>(pagingInfo);
            foreach (var item in list)
            {
                if (item.LabelQty > 0 && item.LabelNgQty <= 0)
                {
                    item.ReDetailQuality = Enums.ReDetailQuality.Good;
                    item.ReturnQty = item.AvailableQty < item.LabelQty ? item.AvailableQty : item.LabelQty;
                }
                else if (item.LabelQty <= 0 && item.LabelNgQty > 0)
                {
                    item.ReDetailQuality = Enums.ReDetailQuality.NotGood;
                    item.ReturnQty = item.NgQty < item.LabelNgQty ? item.NgQty : item.LabelNgQty;
                }
            }
            materialReturnApplyDetailSelects.AddRange(list);
            materialReturnApplyDetailSelects.SetTotalCount(totalCount);
            return materialReturnApplyDetailSelects;
        }

        /// <summary>
        /// 获取lpn库存
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="itemCode"></param>
        /// <param name="lot"></param>
        /// <param name="wareId"></param>
        /// <param name="storageId"></param>
        /// <param name="projectNo"></param>
        /// <returns></returns>
        private EntityList<MaterialReturnApplyDetailSelect> GetDetailWithLpnItemLabel(PagingInfo pagingInfo, string itemCode, string lot, double wareId, double storageId, string projectNo)
        {
            EntityList<MaterialReturnApplyDetailSelect> materialReturnApplyDetailSelects = new EntityList<MaterialReturnApplyDetailSelect>();
            projectNo = projectNo.IsNullOrEmpty() ? "*" : projectNo;
            var query = Query<LotLpnOnhand>().As("lpn").Where(lpn => lpn.WarehouseId == wareId && lpn.StorageLocationId == storageId && lpn.State != OnhandState.None && lpn.ProjectNo == projectNo)
                .Join<ItemLabel>((lpn, l) => lpn.ItemId == l.ItemId && lpn.ItemExtProp.NVL("*") == l.ItemExtProp.NVL("*") && lpn.LotCode == l.Lot && lpn.WarehouseId == l.WarehouseId && lpn.StorageLocationId == l.StorageLocationId
                && l.Qty + l.NgQty > 0 && l.ProjectNo == projectNo) // 物料标签可用数+不良数 > 0
                .WhereIf<ItemLabel>(lot.IsNotEmpty(), (lpn, l) => l.Lot == lot)
                .LeftJoin<Item>((lpn, i) => lpn.ItemId == i.Id)
                .WhereIf<Item>(itemCode.IsNotEmpty(), (lpn, i) => i.Code.Contains(itemCode))
                .LeftJoin<Item, ItemStockDataBase>((i, isd) => i.Id == isd.ItemId)
                .LeftJoin<Item, Unit>((i, u) => i.UnitId == u.Id)
                .Select<ItemLabel, Item, ItemStockDataBase, Unit>((lpn, l, i, isd, u) => new
                {
                    LpnId = lpn.Id,
                    ItemId = lpn.ItemId,
                    ItemCode = i.Code,
                    ItemName = i.Name,
                    UnitName = u.Name,
                    EnableExtendProperty = i.EnableExtendProperty,
                    IsBatch = isd.IsBatch,
                    IsSeri = isd.IsSerialNumber,
                    ItemExtProp = lpn.ItemExtProp,
                    ItemExtPropName = lpn.ItemExtPropName,
                    ItemLabelId = l.Id,
                    Label = l.Label,
                    Lot = l.Lot,
                    LabelQty = l.Qty,
                    LabelNgQty = l.NgQty,
                    LpnQty = lpn.AvailableQty,
                    LpnState = lpn.State,
                    CanExtProp = false,
                });

            var totalCount = query.Count();
            var list = query.ToList<MaterialReturnApplyDetailSelect>(pagingInfo);
            foreach (var item in list)
            {
                if (item.LabelQty > 0 && item.LabelNgQty <= 0)
                {
                    item.ReDetailQuality = Enums.ReDetailQuality.Good;
                    item.ReturnQty = item.LpnQty < item.LabelQty ? item.LpnQty : item.LabelQty;
                }
                else if (item.LabelQty <= 0 && item.LabelNgQty > 0)
                {
                    item.ReDetailQuality = Enums.ReDetailQuality.NotGood;
                    item.ReturnQty = item.NgLpnQty < item.LabelNgQty ? item.NgLpnQty : item.LabelNgQty;
                }
            }
            materialReturnApplyDetailSelects.AddRange(list);
            materialReturnApplyDetailSelects.SetTotalCount(totalCount);
            return materialReturnApplyDetailSelects;
        }

        /// <summary>
        /// 来源bom
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="itemCode">物料编码</param>
        /// <param name="woId">工单Id</param>
        /// <returns></returns>
        private EntityList<MaterialReturnApplyDetailSelect> GetDetailFromBom(PagingInfo pagingInfo, string itemCode, double? woId)
        {
            EntityList<MaterialReturnApplyDetailSelect> materialReturnApplyDetailSelects = new EntityList<MaterialReturnApplyDetailSelect>();
            var query = Query<WorkOrderBom>().Where(w => w.WorkOrderId == woId)
                .LeftJoin<Item>((w, i) => w.ItemId == i.Id)
                .WhereIf<Item>(itemCode.IsNotEmpty(), (w, i) => i.Code.Contains(itemCode))
                .LeftJoin<Item, ItemStockDataBase>((i, isd) => i.Id == isd.ItemId)
                .LeftJoin<Item, Unit>((i, u) => i.UnitId == u.Id)
                .Select<Item, ItemStockDataBase, Unit>((w, i, isd, u) => new
                {
                    ItemId = w.ItemId,
                    ItemCode = i.Code,
                    ItemName = i.Name,
                    UnitName = u.Name,
                    EnableExtendProperty = i.EnableExtendProperty,
                    IsBatch = isd.IsBatch,
                    IsSeri = isd.IsSerialNumber,
                    ItemExtProp = w.ItemExtProp,
                    ItemExtPropName = w.ItemExtPropName,
                    CanExtProp = false,
                });
            var totalCount = query.Count();
            var list = query.ToList<MaterialReturnApplyDetailSelect>(pagingInfo);
            materialReturnApplyDetailSelects.AddRange(list);
            materialReturnApplyDetailSelects.SetTotalCount(totalCount);
            return materialReturnApplyDetailSelects;
        }

        /// <summary>
        /// 来源物料
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="itemCode">物料编码</param>
        /// <returns></returns>
        private EntityList<MaterialReturnApplyDetailSelect> GetDetailFromItem(PagingInfo pagingInfo, string itemCode)
        {
            EntityList<MaterialReturnApplyDetailSelect> materialReturnApplyDetailSelects = new EntityList<MaterialReturnApplyDetailSelect>();
            var query = Query<Item>()
                .WhereIf(itemCode.IsNotEmpty(), i => i.Code.Contains(itemCode))
                .LeftJoin<ItemStockDataBase>((i, isd) => i.Id == isd.ItemId)
                .LeftJoin<Unit>((i, u) => i.UnitId == u.Id)
                .Select<ItemStockDataBase, Unit>((i, isd, u) => new
                {
                    ItemId = i.Id,
                    ItemCode = i.Code,
                    ItemName = i.Name,
                    UnitName = u.Name,
                    EnableExtendProperty = i.EnableExtendProperty,
                    IsBatch = isd.IsBatch,
                    IsSeri = isd.IsSerialNumber,
                    CanExtProp = true,
                });
            var totalCount = query.Count();
            var list = query.ToList<MaterialReturnApplyDetailSelect>(pagingInfo);
            materialReturnApplyDetailSelects.AddRange(list);
            materialReturnApplyDetailSelects.SetTotalCount(totalCount);
            return materialReturnApplyDetailSelects;
        }

        /// <summary>
        /// 查询退料申请明细
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialReturnApplyDetailSelect> GetMaterialReturnApplyDetailSelects(MaterialReturnApplyDtlSelCriteria criteria)
        {
            if (criteria == null) return new EntityList<MaterialReturnApplyDetailSelect>();
            if (criteria.WareId != null && criteria.WoId != null)
            {
                return GetDetailWithItemLabel(criteria.PagingInfo, criteria.ItemCode, criteria.Lot, criteria.WareId.Value, criteria.StorageId.Value, criteria.WoId, criteria.ProjectNo);
            }
            else if (criteria.WareId != null && criteria.WoId == null && criteria.WorkShopId != null)
            {
                return GetDetailWithLpnItemLabel(criteria.PagingInfo, criteria.ItemCode, criteria.Lot, criteria.WareId.Value, criteria.StorageId.Value, criteria.ProjectNo);
            }
            else if (criteria.WareId == null && criteria.WoId != null)
            {
                return GetDetailFromBom(criteria.PagingInfo, criteria.ItemCode, criteria.WoId.Value);
            }
            else
            {
                return GetDetailFromItem(criteria.PagingInfo, criteria.ItemCode);
            }
        }

        /// <summary>
        /// 获取对应退料申请单下的
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="expectIds"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialReturnApplyDetail> GetMaterialReturnApplyDetails(double parentId, List<double> expectIds = null)
        {
            var query = Query<MaterialReturnApplyDetail>().Where(p => p.MaterialReturnApplyId == parentId && !expectIds.Contains(p.Id))
                .Select(p => new
                {
                    Wo_Demand_Report_Id = p.MaterialReturnApplyId,
                    Re_Detail_Quality = p.ReDetailQuality,
                    Item_Id = p.ItemId,
                    Item_Ext_Prop = p.ItemExtProp,
                    Item_Label_Id = p.ItemLabelId,
                }).ToList();
            return query;
        }

        /// <summary>
        /// 获取同工单下退料申请单下的明细
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="woId"></param>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialReturnApplyDetail> GetSameWoMaterialReturnApplyDetails(double parentId, double? woId, List<double> itemIds)
        {
            var query = Query<MaterialReturnApplyDetail>().LeftJoin<MaterialReturnApply>((p, q) => p.MaterialReturnApplyId == q.Id)
                .Where<MaterialReturnApply>((p, q) => q.ReStatus == ReStatus.Saved && q.WorkOrderId == woId)
                .Where(p => p.MaterialReturnApplyId != parentId && itemIds.Contains(p.ItemId))
                 .Select(p => new
                 {
                     Wo_Demand_Report_Id = p.WoDemandReportId,
                     Re_Detail_Quality = p.ReDetailQuality,
                     Item_Id = p.ItemId,
                     Item_Ext_Prop = p.ItemExtProp,
                     Item_Label_Id = p.ItemLabelId,
                     Lpn_Id = p.LpnId,
                     Ng_Lpn_Id = p.NgLpnId,
                     Return_Qty = p.ReturnQty,
                 });
            var list = query.ToList();
            return list;
        }

        /// <summary>
        /// 根据工单号获取退料申请明细
        /// </summary>
        /// <param name="woNo">工单号</param>
        /// <param name="elo">贪婪</param>
        /// <returns></returns>
        public virtual EntityList<MaterialReturnApplyDetail> GetMaterialReturnApplyDetailsByWoNo(string woNo, EagerLoadOptions elo = null)
        {
            var query = Query<MaterialReturnApplyDetail>().Where(p => p.MaterialReturnApply.WorkOrder.No == woNo);
            return query.ToList(null, elo);
        }

        /// <summary>
        /// 获取退料申请单
        /// </summary>
        /// <param name="applyIds"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialReturnApply> GetMaterialReturnApplyByIds(List<double> applyIds)
        {
            return applyIds.SplitContains(temps =>
            {
                return Query<MaterialReturnApply>().Where(p => temps.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取退料申请单Id
        /// </summary>
        /// <param name="nos">退料单号</param>
        /// <returns></returns>
        public virtual List<double> GetMaterialReturnApplyIdsByNos(List<string> nos)
        {
            List<double> ids = new List<double>();
            nos.SplitDataExecute(temps =>
            {
                var list = Query<MaterialReturnApply>().Where(p => temps.Contains(p.No)).Select(p => new { p.Id }).ToList<double>();
                ids.AddRange(list);
            });
            return ids;
        }

        /// <summary>
        /// 获取退料申请单Id(key:单号, value:Id)
        /// </summary>
        /// <param name="nos">退料单号</param>
        /// <returns></returns>
        public virtual Dictionary<string, double> GetMaterialReturnApplyDicByNos(List<string> nos)
        {
            List<MaterialReturnApply> ids = new List<MaterialReturnApply>();
            nos.SplitDataExecute(temps =>
            {
                var list = Query<MaterialReturnApply>().Where(p => temps.Contains(p.No)).Select(p => new { p.Id, p.No }).ToList();
                ids.AddRange(list);
            });
            return ids.ToDictionary(p => p.No, p => p.Id);
        }

        /// <summary>
        /// 获取退料申请明细
        /// </summary>
        /// <param name="parentIds">退料申请ids</param>
        /// <returns></returns>
        public virtual Dictionary<string, MaterialReturnDtlInfo> GetMaterialReturnDtlDic(List<double> parentIds)
        {
            List<MaterialReturnDtlInfo> materialReturnDtlInfos = new List<MaterialReturnDtlInfo>();
            parentIds.SplitDataExecute(temps =>
            {
                var list = Query<MaterialReturnApplyDetail>()
                .LeftJoin<Item>((p, i) => p.ItemId == i.Id)
                .LeftJoin<MaterialReturnApply>((p, q) => p.MaterialReturnApplyId == q.Id)
                .Where(p => temps.Contains(p.MaterialReturnApplyId))
                .Select<Item, MaterialReturnApply>((p, i, q) => new
                {
                    MaterialReturnApplyId = p.MaterialReturnApplyId,
                    ReturnApplyNo = q.No,
                    Id = p.Id,
                    LineNo = p.LineNo,
                    WoDemandReportId = p.WoDemandReportId,
                    ItemId = p.ItemId,
                    ItemCode = i.Code,
                    ItemExtProp = p.ItemExtProp,
                    ItemLabelId = p.ItemLabelId,
                    ReDetailQuality = p.ReDetailQuality
                }).ToList<MaterialReturnDtlInfo>();
                materialReturnDtlInfos.AddRange(list);
            });
            return materialReturnDtlInfos.ToDictionary(p => p.ReturnApplyNo + "-" + p.LineNo + "-" + p.ItemCode + "-" + p.ItemExtProp, p => p);
        }

        /// <summary>
        /// 查询退料申请明细
        /// </summary>
        /// <param name="parentIds">退料申请Ids</param>
        /// <returns></returns>
        public virtual EntityList<MaterialReturnApplyDetail> GetReturnApplyDetailByPIds(List<double> parentIds)
        {
            EntityList<MaterialReturnApplyDetail> materialReturnApplyDetails = new EntityList<MaterialReturnApplyDetail>();
            parentIds.SplitDataExecute(tempIds =>
            {
                var list = Query<MaterialReturnApplyDetail>().Where(p => tempIds.Contains(p.MaterialReturnApplyId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                materialReturnApplyDetails.AddRange(list);
            });
            return materialReturnApplyDetails;
        }

        /// <summary>
        /// 查询标签
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        private Dictionary<double, ItemLabel> GetItemLabelDic(List<double> ids)
        {
            EntityList<ItemLabel> itemLabels = new EntityList<ItemLabel>();
            ids.SplitDataExecute(temps =>
            {
                var list = Query<ItemLabel>().Where(p => temps.Contains(p.Id)).Select(p => new
                {
                    Id = p.Id,
                    Qty = p.Qty,
                    Ng_Qty = p.NgQty,
                }).ToList();
                itemLabels.AddRange(list);
            });
            return itemLabels.ToDictionary(p => p.Id, p => p);
        }

        /// <summary>
        /// 查询工单需求汇总
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        private Dictionary<double, WoDemandReport> GetWoDemanReportDic(List<double> ids)
        {
            EntityList<WoDemandReport> woDemandReports = new EntityList<WoDemandReport>();
            ids.SplitDataExecute(temps =>
            {
                var list = Query<WoDemandReport>().Where(p => temps.Contains(p.Id)).Select(p => new
                {
                    Id = p.Id,
                    Received_Qty = p.ReceivedQty,
                    Moved_In_Qty = p.MovedInQty,
                    Feed_Qty = p.FeedQty,
                    Moved_Out_Qty = p.MovedOutQty,
                    Return_Qty_In_Transit = p.ReturnQtyInTransit,
                    Ng_Return_Qty_In_Transit = p.NgReturnQtyInTransit,
                    Return_Qty = p.ReturnQty,
                    Ng_Return_Qty = p.NgReturnQty,
                    Ng_Qty = p.NgQty,
                }).ToList();
                woDemandReports.AddRange(list);
            });
            return woDemandReports.ToDictionary(p => p.Id, p => p);
        }

        /// <summary>
        /// 查询lpn库存
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        private Dictionary<double, LotLpnOnhand> GetLotLpnOnhandDic(List<double> ids)
        {
            EntityList<LotLpnOnhand> lotlpnOnhands = new EntityList<LotLpnOnhand>();
            ids.SplitDataExecute(temps =>
            {
                var list = Query<LotLpnOnhand>().Where(p => temps.Contains(p.Id)).Select(p => new
                {
                    Id = p.Id,
                    Available_Qty = p.AvailableQty,
                    State = p.State,
                }).ToList();
                lotlpnOnhands.AddRange(list);
            });
            return lotlpnOnhands.ToDictionary(p => p.Id, p => p);
        }

        /// <summary>
        /// 获取退料申请单状态
        /// </summary>
        /// <param name="mrId"></param>
        /// <returns></returns>
        private ReStatus? GetReStatusById(double mrId)
        {
            var mr = Query<MaterialReturnApply>().Where(p => p.Id == mrId).Select(p => new { Re_Status = p.ReStatus }).FirstOrDefault();
            if (mr != null)
            {
                return mr.ReStatus;
            }
            return null;
        }
        #endregion

        #region 业务逻辑

        /// <summary>
        /// 保存前验证
        /// </summary>
        /// <param name="apply">退料申请</param>
        public virtual void ValidateBeforeSave(MaterialReturnApply apply)
        {
            if (apply == null)
            {
                throw new ValidationException("数据异常请刷新界面".L10N());
            }

            var status = GetReStatusById(apply.Id);
            if (status != null && status != ReStatus.Saved)
            {
                throw new ValidationException("退料申请单状态已变更为[{0}]，请重新打开界面".L10nFormat(status.ToLabel()));
            }

            if (apply.WorkOrderId == null && apply.WorkShopId == null)
            {
                throw new ValidationException("工单与车间不能同时为空".L10N());
            }
            if (apply.WorkShopId == null)
            {
                throw new ValidationException("车间不能为空".L10N());
            }
            if (apply.Reason.IsNullOrEmpty())
            {
                throw new ValidationException("退料原因不能为空".L10N());
            }
            if (apply.DetailList == null || !apply.DetailList.Any())
            {
                throw new ValidationException("退料明细不能为空".L10N());
            }
            if (apply.DetailList.Any(p => p.ReturnQty <= 0))
            {
                throw new ValidationException("退料数不能小于0".L10N());
            }
            if (!apply.DetailList.All(p => p.ReDetailQuality == ReDetailQuality.Good) && !apply.DetailList.All(p => p.ReDetailQuality == ReDetailQuality.NotGood))
            {
                throw new ValidationException("退料明细良品状态不一致".L10N());
            }

            var details = apply.DetailList;
            var ids = details.Select(p => p.Id).ToList();
            var deleteList = apply.DetailList.DeletedList as EntityList<MaterialPreparationDetail>;
            if (deleteList != null)
            {
                ids.AddRange(deleteList.Select(p => p.Id));
            }
            details.AddRange(GetMaterialReturnApplyDetails(apply.Id, ids));
            if (details.GroupBy(p => new { p.ItemId, p.ItemExtProp, p.ReDetailQuality, p.ItemLabelId }).Any(p => p.Count() > 1))
            {
                throw new ValidationException("物料+扩展属性+标签+状态唯一".L10N());
            }


            var sameWoDetails = GetSameWoMaterialReturnApplyDetails(apply.Id, apply.WorkOrderId, apply.DetailList.Select(p => p.ItemId).Distinct().ToList());
            sameWoDetails.AddRange(apply.DetailList);
            var itemLabelDic = GetItemLabelDic(sameWoDetails.Where(p => p.ItemLabelId != null).Select(p => (double)p.ItemLabelId).ToList());
            sameWoDetails.ForEach(item =>
            {
                if (item.ItemLabelId != null)
                {
                    itemLabelDic.TryGetValue(item.ItemLabelId.Value, out var itemLabel);
                    if (itemLabel != null)
                    {
                        item.ItemLabel = itemLabel;
                        item.LabelQty = itemLabel.Qty;
                        item.LabelNgQty = itemLabel.NgQty;
                    }
                }
            });
            if (apply.WarehouseId != null && sameWoDetails.Any(p => p.ItemLabelId != null && p.ReDetailQuality == ReDetailQuality.Good && p.ReturnQty > p.LabelQty))
            {
                throw new ValidationException("退料数不能大于标签可用数".L10N());
            }
            if (apply.WarehouseId != null && sameWoDetails.Any(p => p.ItemLabelId != null && p.ReDetailQuality == ReDetailQuality.NotGood && p.ReturnQty > p.LabelNgQty))
            {
                throw new ValidationException("退料数不能大于标签不良数".L10N());
            }

            // 同工单
            var woRemDic = GetWoDemanReportDic(sameWoDetails.Where(p => p.WoDemandReportId != null).Select(p => (double)p.WoDemandReportId).ToList());
            sameWoDetails.ForEach(item =>
            {
                if (item.WoDemandReportId != null)
                {
                    woRemDic.TryGetValue(item.WoDemandReportId.Value, out var woRem);
                    if (woRem != null)
                    {
                        item.WoDemandReport = woRem;
                        item.ReceivedQty = woRem.ReceivedQty;
                        item.MovedInQty = woRem.MovedInQty;
                        item.FeedQty = woRem.FeedQty;
                        item.MovedOutQty = woRem.MovedOutQty;
                        item.ReturnQtyInTransit = woRem.ReturnQtyInTransit;
                        item.NgReturnQtyInTransit = woRem.NgReturnQtyInTransit;
                        item.WoReturnQty = woRem.ReturnQty;
                        item.WoNgReturnQty = woRem.NgReturnQty;
                        item.NgQty = woRem.NgQty;
                    }
                }
            });
            if (apply.WarehouseId != null)
            {
                var woDemandGroup = sameWoDetails.Where(p => p.WoDemandReportId != null).GroupBy(p => new { p.WoDemandReportId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in woDemandGroup.Keys)
                {
                    var list = woDemandGroup[key];
                    var woDemand = list.First();
                    if (woDemand.ReDetailQuality == ReDetailQuality.Good && list.Sum(p => p.ReturnQty) > woDemand.AvailableQty)
                    {
                        throw new ValidationException("存在同工单明细退料数总和大于工单剩余可用数".L10N());
                    }
                    else if (woDemand.ReDetailQuality == ReDetailQuality.NotGood && list.Sum(p => p.ReturnQty) > woDemand.NgQty)
                    {
                        throw new ValidationException("存在同工单明细退料数总和大于工单剩余不良数".L10N());
                    }
                }
            }

            // 验证lpn库存
            var okLpnIds = sameWoDetails.Where(p => p.LpnId != null).Select(p => (double)p.LpnId).ToList();
            var ngLpnIds = sameWoDetails.Where(p => p.NgLpnId != null).Select(p => (double)p.NgLpnId).ToList();
            okLpnIds.AddRange(ngLpnIds);
            var lpnDic = GetLotLpnOnhandDic(okLpnIds);
            sameWoDetails.ForEach(item =>
            {
                if (item.LpnId != null)
                {
                    lpnDic.TryGetValue(item.LpnId.Value, out var lpn);
                    if (lpn != null)
                    {
                        item.LpnQty = lpn.AvailableQty;
                    }
                }
                if (item.NgLpnId != null)
                {
                    lpnDic.TryGetValue(item.NgLpnId.Value, out var lpn);
                    if (lpn != null)
                    {
                        item.NgLpnQty = lpn.AvailableQty;
                    }
                }
            });
            if (apply.WarehouseId != null)
            {
                var okLpnGroup = sameWoDetails.Where(p => p.LpnId != null).GroupBy(p => new { p.LpnId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in okLpnGroup.Keys)
                {
                    var list = okLpnGroup[key];
                    var lpn = list.First();
                    if (lpn.ReDetailQuality == ReDetailQuality.Good && list.Sum(p => p.ReturnQty) > lpn.LpnQty)
                    {
                        throw new ValidationException("存在同工单明细退料数总和大于合格库存可用数".L10N());
                    }
                }
                var ngLpnGroup = sameWoDetails.Where(p => p.NgLpnId != null).GroupBy(p => new { p.NgLpnId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach(var key in ngLpnGroup.Keys)
                {
                    var list = ngLpnGroup[key];
                    var ngLpn = list.First();
                    if (ngLpn.ReDetailQuality == ReDetailQuality.NotGood && list.Sum(p => p.ReturnQty) > ngLpn.NgLpnQty)
                    {
                        throw new ValidationException("存在同工单明细退料数总和大于不合格库存可用数".L10N());
                    }
                }
            }

        }

        /// <summary>
        /// 获取退料申请单号
        /// </summary>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public virtual List<string> GetMrNoLists(int count)
        {
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(MaterialReturnApply));
            if (config == null || config.NumberRuleId == null)
                throw new ValidationException("未找到退料申请单号生成规则,请检查规则配置".L10N());

            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.NumberRuleId.Value, count).ToList();
        }

        /// <summary>
        /// 创建退料数据
        /// </summary>
        /// <param name="applyDic">申请单</param>
        /// <param name="details">退料申请单明细</param>
        /// <param name="isCancel">是否取消</param>
        /// <returns></returns>
        private List<ReturnMaterialData> CreateReturnMaterialDatas(Dictionary<double, MaterialReturnApply> applyDic, EntityList<MaterialReturnApplyDetail> details, bool isCancel = false)
        {
            List<ReturnMaterialData> returnMaterialData = new List<ReturnMaterialData>();
            var invorg = RT.InvOrg != null ? RT.InvOrg.Value : 1;
            foreach (var detail in details)
            {
                var hasApply = applyDic.TryGetValue(detail.MaterialReturnApplyId, out var apply);
                if (hasApply)
                {
                    ReturnMaterialData data = new ReturnMaterialData
                    {
                        InvOrgId = invorg,
                        BillNo = apply.No,
                        EnterpriseCode = apply.WorkShopCode,
                        CreateDate = apply.CreateDate,
                        ReType = apply.ReType == ReType.WorkOrderReturn ? 0 : 1,
                        LineNo = detail.LineNo,
                        WoNo = apply.WoNo,
                        IsReturnLot = apply.WarehouseId != null && detail.IsBatch,
                        LotCode = detail.Lot,
                        SourceLabel = detail.Label,
                        ItemCode = detail.ItemCode,
                        ItemExtProp = detail.ItemExtProp,
                        ItemExtPropName = detail.ItemExtPropName,
                        SecondQty = isCancel ? 0 : detail.ReturnQty,
                        SecondUnitName = detail.UnitName,
                        SecondUnitId = detail.UniId,
                        State = detail.ReDetailQuality == ReDetailQuality.Good ? 10 : 20,
                        WarehouseId = apply.WarehouseId,
                        LocId = apply.StorageLocationId,
                        ReceiveWarehouseId = apply.ReceiveWarehouseId,
                        ProjectNo = apply.ProjectCode,
                    };
                    returnMaterialData.Add(data);
                }

            }
            return returnMaterialData;
        }

        /// <summary>
        /// 保存提交退料申请单
        /// </summary>
        /// <param name="apply"></param>
        public virtual void SaveSubmitReturnApply(MaterialReturnApply apply)
        {
            try
            {
                using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
                {
                    // 保存
                    RF.Save(apply);

                    // 创建Asn
                    var newApply = RF.GetById<MaterialReturnApply>(apply.Id, new EagerLoadOptions().LoadWithViewProperty());
                    var applyDic = new Dictionary<double, MaterialReturnApply> { { newApply.Id, newApply } };
                    var details = GetReturnApplyDetailByPIds(new List<double> { apply.Id });
                    List<ReturnMaterialData> returnMaterialData = CreateReturnMaterialDatas(applyDic, details, false);
                    RT.Service.Resolve<IMesReturnItem>().ReturnMaterialCreateAsn(returnMaterialData);

                    // 更新单为已提交
                    DB.Update<MaterialReturnApply>().Set(p => p.ReStatus, ReStatus.Submitted).Where(p => apply.Id == p.Id).Execute();
                    var detailIds = apply.DetailList.Select(p => p.Id).ToList();
                    // 更新明细状态，更新在途数
                    DB.Update<MaterialReturnApplyDetail>().Set(p => p.ReDetailStatus, ReDetailStatus.ToReceive).Set(p => p.OnWayQty, p => p.ReturnQty).Where(p => detailIds.Contains(p.Id)).Execute();
                    // 更新物料标签可用和不良数、工单需求汇总的在途数
                    // 标签退料明细
                    var itemlabelDetails = details.Where(p => p.ItemLabelId != null).ToList();
                    foreach (var item in itemlabelDetails)
                    {
                        if (item.ReDetailQuality == ReDetailQuality.Good)
                        {
                            DB.Update<ItemLabel>().Set(p => p.ReturnQtyInTransit, p => p.ReturnQtyInTransit + item.ReturnQty).Set(p => p.Qty, p => p.Qty - item.ReturnQty).Where(p => p.Id == item.ItemLabelId).Execute();
                            DB.Update<WoDemandReport>().Set(p => p.ReturnQtyInTransit, p => p.ReturnQtyInTransit + item.ReturnQty).Where(p => p.Id == item.WoDemandReportId).Execute();
                        }
                        else if (item.ReDetailQuality == ReDetailQuality.NotGood)
                        {
                            DB.Update<ItemLabel>().Set(p => p.NgReturnQtyInTransit, p => p.NgReturnQtyInTransit + item.ReturnQty).Set(p => p.NgQty, p => p.NgQty - item.ReturnQty).Where(p => p.Id == item.ItemLabelId).Execute();
                            DB.Update<WoDemandReport>().Set(p => p.NgReturnQtyInTransit, p => p.NgReturnQtyInTransit + item.ReturnQty).Set(p => p.NgQty, p => p.NgQty - item.ReturnQty).Where(p => p.Id == item.WoDemandReportId).Execute();
                        }
                    }
                    //更新工单BOM
                    UpdateWoBomQty(itemlabelDetails);

                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException("{0}".L10nFormat(ex.Message));
            }
        }

        /// <summary>
        /// 提交退料申请单
        /// </summary>
        /// <param name="applyIds">退料申请单Ids</param>
        public virtual void SubmitReturnApply(List<double> applyIds)
        {
            var applys = GetMaterialReturnApplyByIds(applyIds);
            if (applys.Any(p => p.ReStatus != ReStatus.Saved))
            {
                throw new ValidationException("只有保存状态的退料申请单才能提交".L10N());
            }
            var applyDic = applys.ToDictionary(p => p.Id, p => p);
            // 明细
            var details = GetReturnApplyDetailByPIds(applyIds);
            var detailIds = details.Select(p => p.Id).ToList();
            // 标签退料明细
            var itemlabelDetails = details.Where(p => p.ItemLabelId != null).ToList();

            // 创建ASN
            List<ReturnMaterialData> returnMaterialData = CreateReturnMaterialDatas(applyDic, details, false);

            try
            {
                using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
                {
                    // 创建asn
                    RT.Service.Resolve<IMesReturnItem>().ReturnMaterialCreateAsn(returnMaterialData);
                    // 更新单为已提交
                    DB.Update<MaterialReturnApply>().Set(p => p.ReStatus, ReStatus.Submitted).Where(p => applyIds.Contains(p.Id)).Execute();
                    // 更新明细状态，更新在途数
                    DB.Update<MaterialReturnApplyDetail>().Set(p => p.ReDetailStatus, ReDetailStatus.ToReceive).Set(p => p.OnWayQty, p => p.ReturnQty).Where(p => detailIds.Contains(p.Id)).Execute();
                    // 更新物料标签可用和不良数、工单需求汇总的在途数
                    foreach (var item in itemlabelDetails)
                    {
                        if (item.ReDetailQuality == ReDetailQuality.Good)
                        {
                            DB.Update<ItemLabel>().Set(p => p.ReturnQtyInTransit, p => p.ReturnQtyInTransit + item.ReturnQty).Set(p => p.Qty, p => p.Qty - item.ReturnQty).Where(p => p.Id == item.ItemLabelId).Execute();
                            DB.Update<WoDemandReport>().Set(p => p.ReturnQtyInTransit, p => p.ReturnQtyInTransit + item.ReturnQty).Where(p => p.Id == item.WoDemandReportId).Execute();
                        }
                        else if (item.ReDetailQuality == ReDetailQuality.NotGood)
                        {
                            DB.Update<ItemLabel>().Set(p => p.NgReturnQtyInTransit, p => p.NgReturnQtyInTransit + item.ReturnQty).Set(p => p.NgQty, p => p.NgQty - item.ReturnQty).Where(p => p.Id == item.ItemLabelId).Execute();
                            DB.Update<WoDemandReport>().Set(p => p.NgReturnQtyInTransit, p => p.NgReturnQtyInTransit + item.ReturnQty).Set(p => p.NgQty, p => p.NgQty - item.ReturnQty).Where(p => p.Id == item.WoDemandReportId).Execute();
                        }
                    }
                    //更新工单BOM
                    UpdateWoBomQty(itemlabelDetails);

                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException("{0}".L10nFormat(ex.Message));
            }
        }

        /// <summary>
        /// 取消退料申请
        /// </summary>
        /// <param name="applyId">退料申请Id</param>
        public virtual void CancelReturnApply(double applyId)
        {
            var apply = RF.GetById<MaterialReturnApply>(applyId, new EagerLoadOptions().LoadWithViewProperty());
            if (apply.ReStatus != ReStatus.Submitted)
            {
                throw new ValidationException("退料申请单状态不为已提交".L10N());
            }
            var applyDic = new Dictionary<double, MaterialReturnApply> { { applyId, apply } };
            // 明细
            var details = GetReturnApplyDetailByPIds(new List<double> { applyId });
            if (details.Any(p => p.CollectQty > 0))
            {
                throw new ValidationException("存在明细收货数大于0，无法取消".L10N());
            }
            // 标签退料明细
            var itemlabelDetails = details.Where(p => p.ItemLabelId != null).ToList();

            // 创建ASN
            List<ReturnMaterialData> returnMaterialData = CreateReturnMaterialDatas(applyDic, details, true);

            try
            {
                using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
                {
                    // 更新asn
                    RT.Service.Resolve<IMesReturnItem>().ReturnMaterialCreateAsn(returnMaterialData);
                    // 更新主子表状态为取消
                    DB.Update<MaterialReturnApply>().Set(p => p.ReStatus, ReStatus.Canceled).Where(p => p.Id == applyId).Execute();
                    DB.Update<MaterialReturnApplyDetail>().Set(p => p.ReDetailStatus, ReDetailStatus.Cancel).Where(p => p.MaterialReturnApplyId == applyId).Execute();
                    // 更新物料标签可用和不良数、工单需求汇总的在途数
                    foreach (var item in itemlabelDetails)
                    {
                        if (item.ReDetailQuality == ReDetailQuality.Good)
                        {
                            DB.Update<ItemLabel>().Set(p => p.ReturnQtyInTransit, p => p.ReturnQtyInTransit - item.ReturnQty).Set(p => p.Qty, p => p.Qty + item.ReturnQty).Where(p => p.Id == item.ItemLabelId).Execute();
                            DB.Update<WoDemandReport>().Set(p => p.ReturnQtyInTransit, p => p.ReturnQtyInTransit - item.ReturnQty).Where(p => p.Id == item.WoDemandReportId).Execute();
                        }
                        else if (item.ReDetailQuality == ReDetailQuality.NotGood)
                        {
                            DB.Update<ItemLabel>().Set(p => p.NgReturnQtyInTransit, p => p.NgReturnQtyInTransit - item.ReturnQty).Set(p => p.NgQty, p => p.NgQty + item.ReturnQty).Where(p => p.Id == item.ItemLabelId).Execute();
                            DB.Update<WoDemandReport>().Set(p => p.NgReturnQtyInTransit, p => p.NgReturnQtyInTransit - item.ReturnQty).Set(p => p.NgQty, p => p.NgQty + item.ReturnQty).Where(p => p.Id == item.WoDemandReportId).Execute();
                        }
                    }
                    //更新工单BOM
                    UpdateWoBomQty(itemlabelDetails);

                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException("{0}".L10nFormat(ex.Message));
            }
        }

        /// <summary>
        /// 更新工单BOM
        /// </summary>
        /// <param name="details"></param>
        void UpdateWoBomQty(IList<MaterialReturnApplyDetail> details)
        {
            details.Where(p => p.WorkOrderNo.IsNotEmpty()).GroupBy(p => p.WorkOrderNo).ForEach(p =>
            {
                var woNo = p.Key;
                var itemIds = p.Select(p => p.ItemId).Distinct().ToList();
                RT.Service.Resolve<IWorkOrderUpdate>().UpdateWoBomQty(woNo, itemIds);
            });
        }

        /// <summary>
        /// 获取退料数量
        /// </summary>
        /// <param name="woIds"></param>
        /// <returns></returns>
        public virtual Dictionary<Tuple<double, double, string>, decimal> GetMaterialReturnQtyDic(List<double> woIds)
        {
            List<MaterialReturnQtyInfo> materialReturnQtyInfos = new List<MaterialReturnQtyInfo>();
            woIds.SplitDataExecute(temps =>
            {
                var list = Query<MaterialReturnApplyDetail>().LeftJoin<MaterialReturnApply>((mrd, mr) => mrd.MaterialReturnApplyId == mr.Id)
                .Where<MaterialReturnApply>((mrd, mr) => mr.WorkOrderId != null && temps.Contains((double)mr.WorkOrderId) && mr.ReStatus == ReStatus.Finished)
                .GroupBy<MaterialReturnApply>((mrd, mr) => new { mr.WorkOrderId, mrd.ItemId, mrd.ItemExtProp })
                .Select<MaterialReturnApply>((mrd, mr) => new
                {
                    WoId = mr.WorkOrderId,
                    ItemId = mrd.ItemId,
                    ItemExtProp = mrd.ItemExtProp,
                    ReturnQty = mrd.ReturnQty.SUM(),
                }).ToList<MaterialReturnQtyInfo>();
                materialReturnQtyInfos.AddRange(list);
            });
            return materialReturnQtyInfos.ToDictionary(p => new Tuple<double, double, string>(p.WoId, p.ItemId, p.ItemExtProp), p => p.ReturnQty);
        }

        /// <summary>
        /// 计算明细lpn库存
        /// </summary>
        /// <param name="details">明细</param>
        /// <param name="wareId">仓库Id</param>
        /// <param name="storageId">库位Id</param>
        /// <returns></returns>
        public virtual List<MaterialReturnApplyDetailSelect> CaseLpnOnHand(List<MaterialReturnApplyDetailSelect> details, double? wareId, double? storageId)
        {
            if (wareId == null || storageId == null)
            {
                return details;
            }

            var itemIds = details.Select(p => p.ItemId).Distinct().ToList();
            var lots = details.Select(p => p.Lot).Distinct().ToList();

            // 获取lpn库存
            var lpnList = RT.Service.Resolve<InvOnhandController>().GetLotLpnOnhands(wareId.Value, e =>
            {
                e.Where(p => itemIds.Contains(p.ItemId) && lots.Contains(p.LotCode) && p.StorageLocationId == storageId);
            }, true);
            var lpnDic = lpnList.ToLookup(p => new { ItemId = p.ItemId, ItemExtProp = p.ItemExtProp, Lot = p.LotCode });

            foreach (var detail in details)
            {
                var key = new { ItemId = detail.ItemId, ItemExtProp = detail.ItemExtProp, Lot = detail.Lot };
                var lpns = lpnDic[key];
                var okLpn = lpns.FirstOrDefault(p => p.State == OnhandState.Ok);
                if (okLpn != null)
                {
                    detail.LpnId = okLpn.Id;
                    detail.LpnQty = okLpn.AvailableQty;
                }
                var ngLpn = lpns.FirstOrDefault(p => p.State == OnhandState.Ng);
                if (ngLpn != null)
                {
                    detail.NgLpnId = ngLpn.Id;
                    detail.NgLpnQty = ngLpn.AvailableQty;
                }
            }
            return details;
        }
        #endregion
    }
}
