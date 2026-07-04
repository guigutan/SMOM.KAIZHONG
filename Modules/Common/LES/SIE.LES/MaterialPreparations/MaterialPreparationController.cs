using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Core.Common.Controllers;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.LES;
using SIE.EventMessages.MES.WorkOrders;
using SIE.Items;
using SIE.LES.LinesideWarehouses;
using SIE.LES.LinesideWarehouses.Models;
using SIE.LES.MaterialPreparations.ApiModels;
using SIE.LES.MaterialPreparations.Enums;
using SIE.LES.MaterialPreparations.Helpers;
using SIE.LES.MaterialPreparations.ViewModels;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.LES.MaterialPreparations
{
    /// <summary>
    /// 备料需求单控制器
    /// </summary>
    public partial class MaterialPreparationController : DomainController
    {
        #region 查询

        /// <summary>
        /// 查询命令
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns></returns>
        public virtual EntityList<MaterialPreparation> MaterialPreparationQuery(MaterialPreparationCriteria criteria)
        {
            if (criteria == null)
            {
                return new EntityList<MaterialPreparation>();
            }
            var query = Query<MaterialPreparation>();
            if (criteria.No.IsNotEmpty())
            {
                query.Where(p => p.No.Contains(criteria.No));
            }
            if (criteria.Wo.IsNotEmpty())
            {
                query.Where(p => p.WorkOrder.No.Contains(criteria.Wo));
            }
            if (criteria.WorkShopId != null && criteria.WorkShopId != 0)
            {
                query.Where(p => p.WorkShopId == criteria.WorkShopId);
            }
            if (criteria.WipResourceId != null && criteria.WipResourceId != 0)
            {
                query.Where(p => p.ResourceId == criteria.WipResourceId);
            }
            if (criteria.PrepareStatus.HasValue)
            {
                query.Where(p => p.PrepareStatus == criteria.PrepareStatus.Value);
            }
            if (criteria.PrepareType.HasValue)
            {
                query.Where(p => p.PrepareType == criteria.PrepareType.Value);
            }
            if (criteria.PrepareReason.IsNotEmpty())
            {
                query.Where(p => p.Reason == criteria.PrepareReason);
            }
            if (criteria.SaleOrderNo.IsNotEmpty())
            {
                query.Where(p => p.WorkOrder.SaleOrderNo.Contains(criteria.SaleOrderNo));
            }
            if (criteria.CustomerOrderNo.IsNotEmpty())
            {
                query.Where(p => p.WorkOrder.CustomerOrderNo.Contains(criteria.CustomerOrderNo));
            }
            if (criteria.ShippingOrderNo.IsNotEmpty())
            {
                query.Where(p => p.ShippingOrderNo.Contains(criteria.ShippingOrderNo));
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取当前工单下的bom并计算
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialPreparationDetailSelect> GetMaterialPreparationDetailSelects(MaterialPreparationDtlSelCriteria criteria)
        {
            EntityList<MaterialPreparationDetailSelect> details = new EntityList<MaterialPreparationDetailSelect>();
            var helper = new CalculateQtyHelper();
            helper.InitSelectDataBase(criteria.WoId, criteria.ItemCode, criteria.PagingInfo);
            
            // 当前工单bom数据
            var bomList = helper.GetBomList(criteria.WoId);

            if (!bomList.Any())
            {
                return details;
            }

            foreach (var bom in bomList)
            {
                var canPrepareQty = helper.CalculateCanQty(criteria.WoId, bom);
                MaterialPreparationDetailSelect detail = new MaterialPreparationDetailSelect
                {
                    LineNo = bom.LineNo,
                    MpWo = bom.WoNo,
                    MpWoId = bom.WoId,
                    ItemId = bom.ItemId,
                    ItemCode = bom.ItemCode,
                    ItemName = bom.ItemName,
                    ItemConsumeMode = bom.ConsumeMode,
                    ItemExtProp = bom.ItemExtProp,
                    ItemExtPropName = bom.ItemExtPropName,
                    UnitName = bom.UnitName,
                    BomNeedQty = bom.BomNeedQty,
                    CanPrepareQty = canPrepareQty > 0 ? canPrepareQty : 0,
                    Qty = canPrepareQty,
                };
                details.Add(detail);
            }
            details.SetTotalCount(helper.SingleWoBomTotalCount);
            return details;
        }

        /// <summary>
        /// 车间备料选择物料
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialPrepareItemViewModel> WorkShopSelectItemQuery(MaterialPrepareItemCriteria criteria)
        {
            if (criteria == null)
            {
                return new EntityList<MaterialPrepareItemViewModel>();
            }
            var query = Query<Item>().Where(p => p.State == State.Enable);
            if (criteria.ItemCode.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(criteria.ItemCode));
            }
            if (criteria.ItemName.IsNotEmpty())
            {
                query.Where(p => p.Name.Contains(criteria.ItemName));
            }
            if (criteria.ConsumeMode.HasValue)
            {
                query.Where(p => p.ConsumeMode == criteria.ConsumeMode.Value);
            }
            var q = query.LeftJoin<Unit>((i, u) => i.UnitId == u.Id)
                .Select<Unit>((i, u) => new
                {
                    ItemId = i.Id,
                    ItemCode = i.Code,
                    ItemName = i.Name,
                    ConsumeMode = i.ConsumeMode,
                    UnitName = u.Name,
                    EnableExtendProperty = i.EnableExtendProperty
                });
            var totalCount = q.Count();
            var list = q.ToList<MaterialPrepareItemViewModel>(criteria.PagingInfo);
            EntityList<MaterialPrepareItemViewModel> materialPrepareItemViewModels = new EntityList<MaterialPrepareItemViewModel>();
            materialPrepareItemViewModels.AddRange(list);
            materialPrepareItemViewModels.SetTotalCount(totalCount);
            return materialPrepareItemViewModels;
        }

        /// <summary>
        /// 备料需求的查询明细
        /// </summary>
        /// <param name="parentId">备料需求单Id</param>
        /// <returns></returns>
        public virtual EntityList<MaterialPreparationDetail> GetMaterialPreDetail(double parentId)
        {
            return Query<MaterialPreparationDetail>().Where(p => p.MaterialPreparationId == parentId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 备料需求明细数量
        /// </summary>
        /// <param name="parentId">备料需求单Id</param>
        /// <returns></returns>
        public virtual int GetMaterialPreDetailCount(double parentId)
        {
            return Query<MaterialPreparationDetail>().Where(p => p.MaterialPreparationId == parentId).Count();
        }

        /// <summary>
        /// 备料需求的查询明细(排除修改数据)
        /// </summary>
        /// <param name="parentId">备料需求单Id</param>
        /// <param name="exceptIds">修改数据Id</param>
        /// <returns></returns>
        public virtual EntityList<MaterialPreparationDetail> GetMaterialPreDetail(double parentId, List<double> exceptIds)
        {
            return Query<MaterialPreparationDetail>().Where(p => !exceptIds.Contains(p.Id) && p.MaterialPreparationId == parentId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 备料需求的查询明细
        /// </summary>
        /// <param name="parentIds">备料需求单Ids</param>
        /// <returns></returns>
        public virtual EntityList<MaterialPreparationDetail> GetMaterialPreDetail(List<double> parentIds)
        {
            return parentIds.SplitContains(tempIds =>
            {
                return Query<MaterialPreparationDetail>().Where(p => tempIds.Contains(p.MaterialPreparationId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            });
        }

        /// <summary>
        /// 备料需求的查询明细
        /// </summary>
        /// <param name="detailIds">备料需求明细Ids</param>
        /// <returns></returns>
        public virtual EntityList<MaterialPreparationDetail> GetMaterialPreDetailByIds(List<double> detailIds)
        {
            return detailIds.SplitContains(tempIds =>
            {
                return Query<MaterialPreparationDetail>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            });
        }

        /// <summary>
        /// 非待发运备料需求的查询明细
        /// </summary>
        /// <param name="parentId">备料需求单Id</param>
        /// <returns></returns>
        public virtual int CountNotShippingMaterialPreDetail(double parentId)
        {
            return Query<MaterialPreparationDetail>().Where(p => p.MaterialPreparationId == parentId && p.PreDetailStatus != PrepareDetailStatus.ToShipping).Count();
        }

        /// <summary>
        /// 非待发运备料需求的查询明细
        /// </summary>
        /// <param name="detailIds">备料需求明细Ids</param>
        /// <returns></returns>
        public virtual int CountNotShippingMaterialPreDetail(List<double> detailIds)
        {
            return Query<MaterialPreparationDetail>().Where(p => detailIds.Contains(p.Id) && p.PreDetailStatus != PrepareDetailStatus.ToShipping).Count();
        }

        /// <summary>
        /// 非取消运备料需求的查询明细
        /// </summary>
        /// <param name="preId">备料单Id</param>
        /// <returns></returns>
        public virtual int CountNotCancelMaterialPreDetail(double preId)
        {
            return Query<MaterialPreparationDetail>().Where(p => preId == p.MaterialPreparationId && p.PreDetailStatus != PrepareDetailStatus.Canceled).Count();
        }

        /// <summary>
        /// 获取备料需求单状态
        /// </summary>
        /// <param name="preIds">需求的Ids</param>
        /// <returns></returns>
        public virtual EntityList<MaterialPreparation> GetMaterialPreparation(List<double> preIds)
        {
            return preIds.SplitContains(tempIds =>
            {
                return Query<MaterialPreparation>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取备料需求单状态
        /// </summary>
        /// <param name="preIds">需求的Ids</param>
        /// <returns></returns>
        public virtual EntityList<MaterialPreparation> GetMaterialPreparationStatus(List<double> preIds)
        {
            return preIds.SplitContains(tempIds =>
            {
                return Query<MaterialPreparation>().Where(p => tempIds.Contains(p.Id)).Select(p => new { p.PrepareStatus }).ToList();
            });
        }

        /// <summary>
        /// 查询工单
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialPrepareWoViewModel> GetMaterialPrepareWoViewModels(MaterialPrepareWoCriteria criteria)
        {
            if (criteria == null)
            {
                return new EntityList<MaterialPrepareWoViewModel>();
            }
            EntityList<MaterialPrepareWoViewModel> viewmodel = new EntityList<MaterialPrepareWoViewModel>();
            var workOrderInfoWithCount = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderList(criteria.WoNo, criteria.FactoryId, criteria.WorkshopId, criteria.WipResourceId, criteria.ProductCode, criteria.ProductName, true, true, null, null, criteria.PagingInfo);
            var workInfoList = workOrderInfoWithCount.WorkOrderInfos;
            foreach (var workInfo in workInfoList)
            {
                MaterialPrepareWoViewModel view = new MaterialPrepareWoViewModel
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
                    ProjectMaintainId = workInfo.ProjectMaintainId,
                    ProjectMaintainCode = workInfo.ProjectMaintainCode,
                };
                viewmodel.Add(view);
            }
            viewmodel.SetTotalCount(workOrderInfoWithCount.TotalCount);
            return viewmodel;
        }

        /// <summary>
        /// 查询工单备料汇总
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrderMpViewModel> GetWorkOrderMpViewModels(WorkOrderMpViewModelCriteria criteria)
        {
            if (criteria == null)
            {
                return new EntityList<WorkOrderMpViewModel>();
            }
            EntityList<WorkOrderMpViewModel> viewmodel = new EntityList<WorkOrderMpViewModel>();
            var workOrderInfoWithCount = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderList(criteria.WoNo, criteria.FactoryId, criteria.WorkshopId, criteria.WipResourceId, criteria.ProductCode, criteria.ProductName, true, true, criteria.PlanBeginDateRange.BeginValue, criteria.PlanBeginDateRange.EndValue, criteria.PagingInfo);
            var workInfoList = workOrderInfoWithCount.WorkOrderInfos;
            foreach (var workInfo in workInfoList)
            {
                WorkOrderMpViewModel view = new WorkOrderMpViewModel
                {
                    Id = workInfo.WorkOrderId,
                    No = workInfo.WorkOrderNo,
                    FactoryName = workInfo.FactoryCode,
                    WorkShopName = workInfo.WorkShopCode,
                    ResourceName = workInfo.ResourceCode,
                    ProductCode = workInfo.ProductCode,
                    ProductName = workInfo.ProductName,
                    PlanQty = workInfo.PlanQty,
                    FinishQty = workInfo.FinishQty,
                    WoType = workInfo.Type,
                    WoState = workInfo.State,
                    PlanBeginDate = workInfo.PlanBeginDate,
                    PlanEndDate = workInfo.PlanEndDate,
                    ActuStartDate = workInfo.ActuStartDate,
                    ActuFinishDate = workInfo.ActuFinishDate,
                    SaleOrderNo = workInfo.SaleOrderNo,
                    CustomerOrderNo = workInfo.CustomerOrderNo,
                };
                viewmodel.Add(view);
            }
            viewmodel.SetTotalCount(workOrderInfoWithCount.TotalCount);
            return viewmodel;
        }

        /// <summary>
        /// 获取工单bom信息
        /// </summary>
        /// <param name="woId">工单id</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        public virtual Tuple<int, List<WoBomInfo>> GetBomList(double woId, PagingInfo pagingInfo)
        {
            var query = Query<WorkOrderBom>().As("wb").Where(wb => woId == wb.WorkOrderId)
                .LeftJoin<Item>((wb, i) => wb.ItemId == i.Id).Where<Item>((wb, i) => i.ConsumeMode == ConsumeMode.Push)
                .LeftJoin<Item, Unit>((i, u) => i.UnitId == u.Id)
                .Select<Item, Unit>((wb, i, u) => new
                {
                    WorkOrderId = wb.WorkOrderId,
                    LineNo = wb.LineNo,
                    ItemId = i.Id,
                    ItemCode = i.Code,
                    ItemName = i.Name,
                    UnitName = u.Name,
                    ConsumeMode = i.ConsumeMode,
                    ItemExtProp = wb.ItemExtProp,
                    ItemExtPropName = wb.ItemExtPropName,
                    BomNeedQty = wb.RequireQty,
                });
            var totalCount = query.Count();
            var list = query.ToList<WoBomInfo>(pagingInfo).ToList();
            return new Tuple<int, List<WoBomInfo>>(totalCount, list);
        }

        /// <summary>
        /// 获取工单bom数据
        /// </summary>
        /// <param name="woIds"></param>
        public virtual Dictionary<double, List<WoBomInfo>> GetBomDic(List<double> woIds)
        {
            List<WoBomInfo> boms = new List<WoBomInfo>();
            woIds.SplitDataExecute(tempIds =>
            {
                var list = Query<WorkOrderBom>().As("wb").Where(wb => tempIds.Contains(wb.WorkOrderId))
                .LeftJoin<WorkOrder>((wb, wo) => wb.WorkOrderId == wo.Id)
                .LeftJoin<Item>((wb, i) => wb.ItemId == i.Id).Where<Item>((wb, i) => i.ConsumeMode == ConsumeMode.Push)
                .LeftJoin<Item, Unit>((i, u) => i.UnitId == u.Id)
                .Select<Item, Unit, WorkOrder>((wb, i, u, wo) => new
                {
                    WorkOrderId = wb.WorkOrderId,
                    LineNo = wb.LineNo,
                    ItemId = i.Id,
                    ItemCode = i.Code,
                    ItemName = i.Name,
                    UnitName = u.Name,
                    ConsumeMode = i.ConsumeMode,
                    ItemExtProp = wb.ItemExtProp,
                    ItemExtPropName = wb.ItemExtPropName,
                    BomNeedQty = wb.RequireQty,
                    WoNo = wo.No,
                    WoId = wo.Id,
                }).ToList<WoBomInfo>();
                boms.AddRange(list);
            });
            var dic = boms.GroupBy(p => p.WorkOrderId).ToDictionary(p => p.Key, p => p.ToList());
            return dic;
        }

        /// <summary>
        /// 获取工单bom数据
        /// </summary>
        /// <param name="woId"></param>
        /// <param name="itemCode"></param>
        /// <param name="pagingInfo"></param>
        public virtual (Dictionary<double, List<WoBomInfo>>, int) GetBomDic(double woId, string itemCode, PagingInfo pagingInfo)
        {
            List<WoBomInfo> boms = new List<WoBomInfo>();
            var query = Query<WorkOrderBom>().As("wb").Where(wb => woId == wb.WorkOrderId)
                .LeftJoin<Item>((wb, i) => wb.ItemId == i.Id).Where<Item>((wb, i) => i.ConsumeMode == ConsumeMode.Push)
                .WhereIf<Item>(itemCode.IsNotEmpty(), (wb, i) => i.Code.Contains(itemCode))
                .LeftJoin<Item, Unit>((i, u) => i.UnitId == u.Id)
                .Select<Item, Unit>((wb, i, u) => new
                {
                    WorkOrderId = wb.WorkOrderId,
                    LineNo = wb.LineNo,
                    ItemId = i.Id,
                    ItemCode = i.Code,
                    ItemName = i.Name,
                    UnitName = u.Name,
                    ConsumeMode = i.ConsumeMode,
                    ItemExtProp = wb.ItemExtProp,
                    ItemExtPropName = wb.ItemExtPropName,
                    BomNeedQty = wb.RequireQty,
                });
            var totalCount = query.Count();
            var list = query.ToList<WoBomInfo>(pagingInfo);
            boms.AddRange(list);
            var dic = boms.GroupBy(p => p.WorkOrderId).ToDictionary(p => p.Key, p => p.ToList());
            return (dic, totalCount);
        }

        /// <summary>
        /// 获取同工单备料明细
        /// </summary>
        /// <param name="woIds">工单Ids</param>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public virtual Dictionary<Tuple<double, double, string, string>, MaterialPreDetailInfo> GetMaterialPreDetailDic(List<double> woIds, string itemCode = null)
        {
            List<MaterialPreDetailInfo> details = new List<MaterialPreDetailInfo>();
            woIds.SplitDataExecute(tempIds =>
            {
                var list = Query<MaterialPreparationDetail>().As("mpd")
                .LeftJoin<MaterialPreparation>((mpd, mp) => mpd.MaterialPreparationId == mp.Id)
                .Where<MaterialPreparation>((mpd, mp) => tempIds.Contains((double)mp.WorkOrderId))
                .GroupBy<MaterialPreparation>((mpd, mp) => new { mpd.LineNo, mp.WorkOrderId, mpd.ItemId, mpd.ItemExtProp })
                .Select<MaterialPreparation>((mpd, mp) => new
                {
                    LineNo = mpd.LineNo,
                    WorkOrderId = mp.WorkOrderId,
                    ItemId = mpd.ItemId,
                    ItemExtProp = mpd.ItemExtProp,
                    Qty = mpd.Qty.SUM(),
                    CancelQty = mpd.CancelQty.SUM(),
                    ShippingQty = mpd.ShippingQty.SUM(),
                }).ToList<MaterialPreDetailInfo>();
                details.AddRange(list);
            });
            return details.ToDictionary(p => new Tuple<double, double, string, string>(p.WorkOrderId, p.ItemId, p.ItemExtProp, p.LineNo), p => new MaterialPreDetailInfo { Qty = p.Qty, CancelQty = p.CancelQty, ShippingQty = p.ShippingQty });
        }

        /// <summary>
        /// 获取同工单备料明细(除去删除数据)
        /// </summary>
        /// <param name="woIds"></param>
        /// <param name="exceptIds"></param>
        /// <returns></returns>
        public virtual Dictionary<Tuple<double, double, string, string>, MaterialPreDetailInfo> GetMaterialPreDetailDic(List<double> woIds, List<double> exceptIds)
        {
            List<MaterialPreDetailInfo> details = new List<MaterialPreDetailInfo>();
            woIds.SplitDataExecute(tempIds =>
            {
                var list = Query<MaterialPreparationDetail>().As("mpd")
                .WhereIf(exceptIds.Count > 0, mpd => !exceptIds.Contains(mpd.Id))
                .LeftJoin<MaterialPreparation>((mpd, mp) => mpd.MaterialPreparationId == mp.Id)
                .Where<MaterialPreparation>((mpd, mp) => tempIds.Contains((double)mp.WorkOrderId))
                .GroupBy<MaterialPreparation>((mpd, mp) => new { mpd.LineNo, mp.WorkOrderId, mpd.ItemId, mpd.ItemExtProp })
                .Select<MaterialPreparation>((mpd, mp) => new
                {
                    LineNo = mpd.LineNo,
                    WorkOrderId = mp.WorkOrderId,
                    ItemId = mpd.ItemId,
                    ItemExtProp = mpd.ItemExtProp,
                    Qty = mpd.Qty.SUM(),
                    CancelQty = mpd.CancelQty.SUM(),
                    ShippingQty = mpd.ShippingQty.SUM(),
                }).ToList<MaterialPreDetailInfo>();
                details.AddRange(list);
            });
            return details.ToDictionary(p => new Tuple<double, double, string, string>(p.WorkOrderId, p.ItemId, p.ItemExtProp, p.LineNo), p => new MaterialPreDetailInfo { Qty = p.Qty, CancelQty = p.CancelQty, ShippingQty = p.ShippingQty });
        }

        /// <summary>
        /// 获取工单bom信息,计算可备料数()
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <param name="preType">领料类型0-生产领料 1-生产超领 2-车间领料</param>
        /// <returns></returns>
        public virtual EntityList<MaterialPreparationDetail> GetWorkOrderBomPrepration(double woId, int preType)
        {
            EntityList<MaterialPreparationDetail> details = new EntityList<MaterialPreparationDetail>();
            var helper = new CalculateQtyHelper(woId);
            helper.InitDataBase();
            // 当前工单bom数据
            var bomList = helper.GetBomList(woId);
            if (!bomList.Any())
            {
                return details;
            }

            foreach (var bom in bomList)
            {
                var canPrepareQty = helper.CalculateCanQty(woId, bom);
                // 生产领料过滤
                if (preType == 0 && canPrepareQty <= 0) continue;
                MaterialPreparationDetail detail = new MaterialPreparationDetail
                {
                    LineNo = bom.LineNo,
                    MpWo = bom.WoNo,
                    MpWoId = bom.WoId,
                    ItemId = bom.ItemId,
                    ItemCode = bom.ItemCode,
                    ItemName = bom.ItemName,
                    ItemConsumeMode = bom.ConsumeMode,
                    ItemExtProp = bom.ItemExtProp,
                    ItemExtPropName = bom.ItemExtPropName,
                    UnitName = bom.UnitName,
                    BomNeedQty = bom.BomNeedQty,
                    CanPrepareQty = canPrepareQty > 0 ? canPrepareQty : 0,
                    Qty = canPrepareQty,
                };
                details.Add(detail);
            }

            return details;
        }

        /// <summary>
        /// 获取工单的备料需求明细统计
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        public virtual EntityList<WorkOrderMpDetailViewModel> GetWorkOrderMpDetailViewModels(double woId, PagingInfo pagingInfo)
        {
            EntityList<WorkOrderMpDetailViewModel> workOrderMpDetailViewModels = new EntityList<WorkOrderMpDetailViewModel>();
            var helper = new CalculateQtyHelper(woId);
            helper.GetPreDetailDic();
            (int totalCount, List<WoBomInfo> bomList) = GetBomList(woId, pagingInfo);
            if (!bomList.Any())
            {
                return workOrderMpDetailViewModels;
            }
            foreach (var bom in bomList)
            {
                var canPrepareQty = helper.CalculateCanQty(woId, bom); // 可备料数
                var hasQty = helper.CalculateHasQty(woId, bom); // 已建备料数
                var hasShippingQty = helper.CalculateShippingQty(woId, bom); // 发料数
                var cancelQty = helper.CalculateCancelQty(woId, bom); // 取消数
                var returnQty = helper.CalculateReturnQty(woId, bom); // 退料数
                var receivedQty = helper.CalculateReceivedQty(woId, bom); // 接收数
                var toReceiveQty = helper.CalculateToReceiveQty(woId, bom); // 待收数
                var moveInQty = helper.CalculateMoveInQty(woId, bom); // 挪入数
                var moveOutQty = helper.CalculateMoveOutQty(woId, bom);// 挪出数
                WorkOrderMpDetailViewModel workOrderMpDetailViewModel = new WorkOrderMpDetailViewModel
                {
                    LineNo = bom.LineNo,
                    ItemCode = bom.ItemCode,
                    ItemName = bom.ItemName,
                    UnitName = bom.UnitName,
                    ConsumeMode = bom.ConsumeMode,
                    ItemExtProp = bom.ItemExtProp,
                    ItemExtPropName = bom.ItemExtPropName,
                    BomNeedQty = bom.BomNeedQty,
                    HasQty = hasQty,
                    CanPrepareQty = canPrepareQty,
                    HasShippingQty = hasShippingQty,
                    CancelQty = cancelQty,
                    ReturnQty = returnQty,
                    HasReceiveQty = receivedQty,
                    ToReceiveQty = toReceiveQty,
                    MoveInQty = moveInQty,
                    MoveOutQty = moveOutQty,
                };
                workOrderMpDetailViewModels.Add(workOrderMpDetailViewModel);
            }
            workOrderMpDetailViewModels.SetTotalCount(totalCount);
            return workOrderMpDetailViewModels;
        }

        /// <summary>
        /// 获取工单的备料需求明细统计(导出用)
        /// </summary>
        /// <param name="woIds">工单Ids</param>
        /// <returns></returns>
        public virtual EntityList<WorkOrderMpDetailViewModel> GetWorkOrderMpDetailViewModels(List<double> woIds)
        {
            EntityList<WorkOrderMpDetailViewModel> workOrderMpDetailViewModels = new EntityList<WorkOrderMpDetailViewModel>();
            var helper = new CalculateQtyHelper(woIds);
            helper.ExportGetPreDetailDic();
            // 查询bom
            foreach (var woId in woIds)
            {
                var bomList = helper.GetBomList(woId);
                if (!bomList.Any())
                {
                    continue;
                }
                foreach (var bom in bomList)
                {
                    var canPrepareQty = helper.CalculateCanQty(woId, bom);
                    var hasQty = helper.CalculateHasQty(woId, bom); // 已建备料数
                    var hasShippingQty = helper.CalculateShippingQty(woId, bom); // 发料数
                    var cancelQty = helper.CalculateCancelQty(woId, bom); // 取消数
                    var returnQty = helper.CalculateReturnQty(woId, bom); // 退料数
                    var receivedQty = helper.CalculateReceivedQty(woId, bom); // 接收数
                    var toReceiveQty = helper.CalculateToReceiveQty(woId, bom); // 待收数
                    var moveInQty = helper.CalculateMoveInQty(woId, bom); // 挪入数
                    var moveOutQty = helper.CalculateMoveOutQty(woId, bom);// 挪出数
                    WorkOrderMpDetailViewModel workOrderMpDetailViewModel = new WorkOrderMpDetailViewModel
                    {
                        LineNo = bom.LineNo,
                        ItemCode = bom.ItemCode,
                        ItemName = bom.ItemName,
                        UnitName = bom.UnitName,
                        ItemExtProp = bom.ItemExtProp,
                        ItemExtPropName = bom.ItemExtPropName,
                        BomNeedQty = bom.BomNeedQty,
                        HasQty = hasQty,
                        CanPrepareQty = canPrepareQty,
                        HasShippingQty = hasShippingQty,
                        CancelQty = cancelQty,
                        ReturnQty = returnQty,
                        HasReceiveQty = receivedQty,
                        ToReceiveQty = toReceiveQty,
                        MoveInQty = moveInQty,
                        MoveOutQty = moveOutQty,
                    };
                    workOrderMpDetailViewModels.Add(workOrderMpDetailViewModel);
                }
            }
            return workOrderMpDetailViewModels;
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
        /// 保存前获取备料需求单状态
        /// </summary>
        /// <param name="preId"></param>
        /// <returns></returns>
        private PrepareStatus? GetPrepareStatusById(double preId)
        {
            var pre = Query<MaterialPreparation>().Where(p => p.Id == preId).Select(p => new { Prepare_Status = p.PrepareStatus}).FirstOrDefault();
            if (pre != null)
            {
                return pre.PrepareStatus;
            }
            return null;
        }

        #endregion

        #region 保存校验

        /// <summary>
        /// 车间备料保存校验
        /// </summary>
        /// <param name="pre">备料需求单</param>
        public virtual void ValidateBeforeSave(MaterialPreparation pre)
        {
            if (pre == null)
            {
                throw new ValidationException("数据异常请刷新界面".L10N());
            }

            var status = GetPrepareStatusById(pre.Id);
            if (status != null && status != PrepareStatus.Saved)
            {
                throw new ValidationException("备料需求单状态已变更为[{0}]，请重新打开界面".L10nFormat(status.ToLabel()));
            }

            // 表头验证
            if (pre.PrepareType == Enums.PrepareType.Sfmr)
            {
                if (pre.FactoryId == null)
                {
                    throw new ValidationException("工厂不能为空".L10N());
                }
                if (pre.WorkShopId == null)
                {
                    throw new ValidationException("车间不能为空".L10N());
                }
                if (pre.Reason.IsNullOrEmpty())
                {
                    throw new ValidationException("备料原因不能为空".L10N());
                }
            }
            else
            {
                if (pre.WorkOrderId == null)
                {
                    throw new ValidationException("工单不能为空".L10N());
                }
                if (!pre.DetailList.Any())
                {
                    throw new ValidationException("不存在本次备料数大于0的明细".L10N());
                }
            }


            // 需求单明细验证
            var dbCount = GetMaterialPreDetailCount(pre.Id);
            if (dbCount + pre.DetailList.Count - pre.DetailList.DeletedList.Count <= 0)
            {
                throw new ValidationException("最少要存在一笔物料需求明细数据".L10N());
            }
            if (pre.DetailList.Any(p => p.Qty <= 0))
            {
                throw new ValidationException("存在本次备料数小于等于0的明细".L10N());
            }
            // 生产领料不允许大于可备料数
            var editIds = pre.DetailList.Select(p => p.Id).ToList();
            var deleteList = pre.DetailList.DeletedList as EntityList<MaterialPreparationDetail>;
            if (deleteList != null)
            {
                editIds.AddRange(deleteList.Select(p => p.Id));
            }

            // 重新计算可备料数
            if (pre.WorkOrderId != null)
            {
                var helper = new CalculateQtyHelper(pre.WorkOrderId.Value);
                helper.GetPreDetailDicForSave(editIds);
                foreach (var detail in pre.DetailList)
                {
                    WoBomInfo bom = new WoBomInfo
                    {
                        LineNo = detail.LineNo,
                        ItemId = detail.ItemId,
                        ItemExtProp = detail.ItemExtProp,
                        BomNeedQty = detail.BomNeedQty,
                    };
                    var canPrepareQty = helper.CalculateCanQty(pre.WorkOrderId.Value, bom);
                    if (detail.Qty > canPrepareQty && pre.PrepareType != Enums.PrepareType.Emc)
                    {
                        throw new ValidationException("本次备料数不能大于可备料数".L10N());
                    }
                }

            }

            var dbDetailList = GetMaterialPreDetail(pre.Id, editIds);
            dbDetailList.AddRange(pre.DetailList);
            if (pre.PrepareType != PrepareType.Sfmr && dbDetailList.GroupBy(p => new { p.LineNo, p.ItemId, p.ItemExtProp }).Any(p => p.Count() > 1))
            {
                throw new ValidationException("非车间备料明细行号+物料+扩展属性唯一".L10N());
            }
            else if (pre.PrepareType == PrepareType.Sfmr && dbDetailList.GroupBy(p => new { p.ItemId, p.ItemExtProp }).Any(p => p.Count() > 1))
            {
                throw new ValidationException("车间备料物料+扩展属性唯一".L10N());
            }
        }
        #endregion

        #region 业务逻辑
        /// <summary>
        /// 生成备料需求单单号
        /// </summary>
        /// <param name="count">生成数量</param>
        /// <returns></returns>
        public virtual List<string> GetMaterialPreprationNo(int count = 1)
        {
            if (count <= 0)
            {
                return new List<string>();
            }
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(MaterialPreparation));
            if (config == null || config.NumberRuleId == null)
                throw new ValidationException("未找到备料需求单号生成规则,请检查规则配置".L10N());

            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.NumberRuleId.Value, count).ToList();
        }

        /// <summary>
        /// 备料需求类型转成单据小类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private int ChangeTransactionType(PrepareType type)
        {
            switch (type)
            {
                case PrepareType.Pmw:
                    return 1;
                case PrepareType.Emc:
                    return 2;
                case PrepareType.Sfmr:
                    return 3;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 创建发运订单数据
        /// </summary>
        /// <param name="details">备料需求明细</param>
        /// <param name="createShippingOrderDatas">发运订单数据</param>
        /// <param name="upDateWoIds">更新工单</param>
        private void CreateShippingOrderDatas(EntityList<MaterialPreparationDetail> details, List<CreateShippingOrderData> createShippingOrderDatas, List<double> upDateWoIds)
        {
            var invorg = RT.InvOrg != null ? RT.InvOrg.Value : 1;
            foreach (var detail in details)
            {
                var enterpriseCode = detail.WipResourceCode.IsNotEmpty() ? detail.WipResourceCode : detail.WorkShopCode;
                CreateShippingOrderData createShippingOrderData = new CreateShippingOrderData
                {
                    InvOrgId = invorg,
                    SourceType = 1,
                    SourceKey = string.Empty, // Erp握手关键字
                    EnterpriseCode = enterpriseCode,
                    IsWoShipMore = detail.MpType == PrepareType.Emc,
                    TransactionType = ChangeTransactionType(detail.MpType),
                    RequireNo = detail.MpNo,
                    RequireLineNo = detail.LineNo,
                    OrderNo = detail.MpWo.IsNotEmpty() ? detail.MpWo : string.Empty,
                    OrderLineNo = detail.MpWo.IsNotEmpty() ? detail.LineNo : string.Empty,
                    ItemCode = detail.ItemCode,
                    SecondExceptQty = detail.Qty,
                    SecondUnitName = detail.UnitName,
                    ItemExtPropName = detail.ItemExtPropName,
                    DeliveryDate = detail.MpCreateDate,
                    ShippingWarehouseCode = detail.WarehouseCode,
                    AppointProjectNo = detail.ProjectMaintainCode.IsNullOrEmpty() ? "*" : detail.ProjectMaintainCode,
                };
                createShippingOrderDatas.Add(createShippingOrderData);
                if (detail.MpWoId != null)
                {
                    upDateWoIds.Add(detail.MpWoId.Value);
                }
            }
        }

        /// <summary>
        /// 更新工单备料类型1
        /// </summary>
        /// <param name="prepareType">备料类型</param>
        /// <param name="workOrderId">工单Id</param>
        public virtual void UpDateWorkOrderMpType(PrepareType prepareType, double? workOrderId)
        {
            if (prepareType == PrepareType.Sfmr)
            {
                return;
            }
            else
            {
                DB.Update<WorkOrder>().Set(p => p.WorkOrderMpType, WorkOrderMpType.ByOrder).Where(p => p.Id == workOrderId).Execute();
            }
        }

        /// <summary>
        /// 保存备料需求单
        /// </summary>
        /// <param name="preparation"></param>
        public virtual void SavePreparationOrder(MaterialPreparation preparation)
        {
            using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                RF.Save(preparation);
                UpDateWorkOrderMpType(preparation.PrepareType, preparation.WorkOrderId);
                tran.Complete();
            }
        }

        /// <summary>
        /// 保存提交备料需求单
        /// </summary>
        /// <param name="preparation"></param>
        public virtual void SaveSubmitPreparationOrder(MaterialPreparation preparation)
        {
            try
            {
                using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
                {
                    RF.Save(preparation);
                    //创建发运订单
                    var preIds = new List<double> { preparation.Id };
                    // 更新工单
                    List<double> upDateWoIds = new List<double>();
                    // 创建发运订单
                    var detailList = GetMaterialPreDetail(preIds);
                    List<CreateShippingOrderData> createShippingOrderDatas = new List<CreateShippingOrderData>();
                    CreateShippingOrderDatas(detailList, createShippingOrderDatas, upDateWoIds);
                    // 调用WMS接口创建发运订单
                    RT.Service.Resolve<ILesShippingOrder>().CreateShippingOrder(createShippingOrderDatas);

                    //更新状态为已提交
                    DB.Update<MaterialPreparation>().Set(p => p.PrepareStatus, Enums.PrepareStatus.Submitted).Where(p => preIds.Contains(p.Id)).Execute();
                    //更新明细状态为待发运
                    DB.Update<MaterialPreparationDetail>().Set(p => p.PreDetailStatus, Enums.PrepareDetailStatus.ToShipping).Where(p => preIds.Contains(p.MaterialPreparationId)).Execute();
                    // 更新工单备料单类型
                    DB.Update<WorkOrder>().Set(p => p.WorkOrderMpType, WorkOrderMpType.ByOrder).Where(p => upDateWoIds.Contains(p.Id)).Execute();

                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException("{0}".L10nFormat(ex.Message));
            }
        }

        /// <summary>
        /// 提交备料需求单
        /// </summary>
        /// <param name="preIds">备料需求单Ids</param>
        public virtual void SubmitPreparationOrder(List<double> preIds)
        {
            var statusList = GetMaterialPreparationStatus(preIds);
            if (statusList.Any(p => p.PrepareStatus != Enums.PrepareStatus.Saved))
            {
                throw new ValidationException("只有保存状态的备料需求单才能提交".L10N());
            }
            // 更新工单
            List<double> upDateWoIds = new List<double>();
            // 创建发运订单
            var detailList = GetMaterialPreDetail(preIds);
            List<CreateShippingOrderData> createShippingOrderDatas = new List<CreateShippingOrderData>();
            CreateShippingOrderDatas(detailList, createShippingOrderDatas, upDateWoIds);

            try
            {
                using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
                {
                    // 调用WMS接口创建发运订单
                    RT.Service.Resolve<ILesShippingOrder>().CreateShippingOrder(createShippingOrderDatas);
                    //更新状态为已提交
                    DB.Update<MaterialPreparation>().Set(p => p.PrepareStatus, Enums.PrepareStatus.Submitted).Where(p => preIds.Contains(p.Id)).Execute();
                    //更新明细状态为待发运
                    DB.Update<MaterialPreparationDetail>().Set(p => p.PreDetailStatus, Enums.PrepareDetailStatus.ToShipping).Where(p => preIds.Contains(p.MaterialPreparationId)).Execute();
                    // 更新工单备料单类型
                    DB.Update<WorkOrder>().Set(p => p.WorkOrderMpType, WorkOrderMpType.ByOrder).Where(p => upDateWoIds.Contains(p.Id)).Execute();

                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException("{0}".L10nFormat(ex.Message));
            }
        }

        /// <summary>
        /// 创建更新发运订单明细(取消信息)
        /// </summary>
        /// <param name="detailList">备料需求明细</param>
        /// <param name="updateShippingOrderDatas">发运订单更新明细</param>
        /// <param name="materialPreCancelRecords">备料需求单取消记录</param>
        /// <returns></returns>
        private void CreateUpdateShippingInfoForCancel(EntityList<MaterialPreparationDetail> detailList, List<UpdateShippingOrderData> updateShippingOrderDatas, EntityList<MaterialPreCancelRecord> materialPreCancelRecords)
        {
            foreach (var detail in detailList)
            {
                // 更新明细
                UpdateShippingOrderData updateShippingOrderData = new UpdateShippingOrderData
                {
                    InvOrgId = RT.InvOrg.Value,
                    BillNo = detail.ShippingDetailNo,
                    LineNo = detail.LineNo,
                    SecondExceptQty = 0,
                };
                updateShippingOrderDatas.Add(updateShippingOrderData);
                // 创建取消记录
                MaterialPreCancelRecord record = new MaterialPreCancelRecord
                {
                    MaterialPreparationId = detail.MaterialPreparationId,
                    ItemCode = detail.ItemCode,
                    ItemExtPropName = detail.ItemExtPropName,
                    CancelQty = detail.Qty,
                };
                materialPreCancelRecords.Add(record);
            }
        }

        /// <summary>
        /// 取消整单备料单
        /// </summary>
        /// <param name="preId">备料需求单</param>
        public virtual void WithDrawPreparation(double preId)
        {
            if (CountNotShippingMaterialPreDetail(preId) > 0)
            {
                throw new ValidationException("存在状态不为待发运的备料需求明细".L10N());
            }

            // 创建更新备料单明细
            var detailList = GetMaterialPreDetail(preId);
            List<UpdateShippingOrderData> updateShippingOrderDatas = new List<UpdateShippingOrderData>();
            // 创建取消记录
            EntityList<MaterialPreCancelRecord> materialPreCancelRecords = new EntityList<MaterialPreCancelRecord>();
            CreateUpdateShippingInfoForCancel(detailList, updateShippingOrderDatas, materialPreCancelRecords);

            try
            {
                using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
                {
                    // 更新备料需求的状态为取消
                    DB.Update<MaterialPreparation>().Set(p => p.PrepareStatus, PrepareStatus.Canceled).Where(p => p.Id == preId).Execute();
                    // 更新明细状态为取消
                    DB.Update<MaterialPreparationDetail>().Set(p => p.PreDetailStatus, PrepareDetailStatus.Canceled).Set(p => p.CancelQty, p => p.Qty).Where(p => p.MaterialPreparationId == preId).Execute();
                    // 保存取消记录
                    if (materialPreCancelRecords.Any())
                    {
                        RT.Service.Resolve<CommonController>().BatchInsertSave(materialPreCancelRecords);
                    }
                    // 调用WMS接口取消备料需求单
                    if (updateShippingOrderDatas.Any())
                    {
                        RT.Service.Resolve<ILesShippingOrder>().UpdateShippingOrder(updateShippingOrderDatas);
                    }
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException("{0}".L10nFormat(ex.Message));
            }
        }

        /// <summary>
        /// 更新备料明细行状态
        /// 0、取消：取消数=备料数，这个判断应该放到最前面
        /// 1、待发运：累计发料数=0
        /// 2、待接收：累计发料数-累计拒收数>0，且接收数=0
        /// 3、部分接收：累计发料数-累计拒收数-接收数>0，且接收数>0
        /// 3、部分接收：备料需求数-取消数-累计发料数>0，且接收数>0
        /// 4、已接收：累计发料数-累计拒收数-接收数=0，且累计发料数≥备料需求数-取消数
        /// </summary>
        /// <param name="detail"></param>
        public virtual void SetPreparationDetailStatus(MaterialPreparationDetail detail)
        {
            detail.PreDetailStatus = PrepareDetailStatus.Created;
            if (detail.CancelQty == detail.Qty)
                detail.PreDetailStatus = PrepareDetailStatus.Canceled;
            else if (detail.ShippingQty == 0)
                detail.PreDetailStatus = PrepareDetailStatus.ToShipping;
            else if ((detail.ShippingQty - detail.RefuseQty) > 0 && detail.ReceiveQty == 0)
                detail.PreDetailStatus = PrepareDetailStatus.ToReceive;
            else if ((detail.ShippingQty - detail.RefuseQty - detail.ReceiveQty) > 0 && detail.ReceiveQty > 0)
                detail.PreDetailStatus = PrepareDetailStatus.PartReceive;
            else if ((detail.Qty - detail.CancelQty - detail.ShippingQty) > 0 && detail.ReceiveQty > 0)
                detail.PreDetailStatus = PrepareDetailStatus.PartReceive;
            if ((detail.ShippingQty - detail.RefuseQty - detail.ReceiveQty) == 0 && detail.ShippingQty >= detail.Qty)
                detail.PreDetailStatus = PrepareDetailStatus.HasReceived;
        }

        /// <summary>
        /// 取消备料需求明细
        /// </summary>
        /// <param name="detailIds"></param>
        public virtual void WithDrawPreDetail(List<double> detailIds)
        {
            if (!detailIds.Any())
            {
                throw new ValidationException("请选择数据行".L10N());
            }
            if (CountNotShippingMaterialPreDetail(detailIds) > 0)
            {
                throw new ValidationException("存在状态不为待发运的备料需求明细".L10N());
            }
            // 创建更新备料单明细
            var detailList = GetMaterialPreDetailByIds(detailIds);
            List<UpdateShippingOrderData> updateShippingOrderDatas = new List<UpdateShippingOrderData>();
            // 创建取消记录
            EntityList<MaterialPreCancelRecord> materialPreCancelRecords = new EntityList<MaterialPreCancelRecord>();
            CreateUpdateShippingInfoForCancel(detailList, updateShippingOrderDatas, materialPreCancelRecords);

            // 备料单Id
            var preId = detailList.FirstOrDefault()?.MaterialPreparationId;

            try
            {
                using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
                {
                    // 更新明细状态为取消
                    DB.Update<MaterialPreparationDetail>().Set(p => p.PreDetailStatus, PrepareDetailStatus.Canceled).Set(p => p.CancelQty, p => p.Qty).Where(p => detailIds.Contains(p.Id)).Execute();
                    // 如果全部明细行都为取消则更新主表状态为取消
                    if (preId != null && CountNotCancelMaterialPreDetail(preId.Value) == 0)
                    {
                        DB.Update<MaterialPreparation>().Set(p => p.PrepareStatus, PrepareStatus.Canceled).Where(p => p.Id == preId.Value).Execute();
                    }
                    // 保存取消记录
                    if (materialPreCancelRecords.Any())
                    {
                        RT.Service.Resolve<CommonController>().BatchInsertSave(materialPreCancelRecords);
                    }
                    // 调用WMS接口取消备料需求单
                    if (updateShippingOrderDatas.Any())
                    {
                        RT.Service.Resolve<ILesShippingOrder>().UpdateShippingOrder(updateShippingOrderDatas);
                    }
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException("{0}".L10nFormat(ex.Message));
            }
        }

        /// <summary>
        /// 校验工单状态和备料单状态
        /// </summary>
        /// <param name="woIds">工单Ids</param>
        /// <param name="type">10:创建发运单 20:创建备料单</param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void ValidateOneKeyWoInfo(List<double> woIds, int type)
        {
            var wo = woIds.SplitContains(tempIds =>
            {
                return Query<WorkOrder>().Where(p => tempIds.Contains(p.Id)).Select(p => new
                {
                    Id = p.Id,
                    Work_Order_Mp_Type = p.WorkOrderMpType,
                    State = p.State,
                }).ToList();
            });
            if (wo.Any(p => p.State != WorkOrderState.Release && p.State != WorkOrderState.Producing))
            {
                throw new ValidationException("存在工单状态不为生产、发放".L10N());
            }

            if (type == 10 && wo.Any(p => p.WorkOrderMpType != null))
            {
                throw new ValidationException("存在工单已创建发运订单".L10N());
            }
            else if (type == 20 && wo.Any(p => p.WorkOrderMpType == WorkOrderMpType.Direct))
            {
                throw new ValidationException("存在工单备料类型为发运订单直发".L10N());
            }

        }

        /// <summary>
        /// 一键备料创建发运订单
        /// </summary>
        /// <param name="woDic">工单</param>
        public virtual void OneKeyCreateShippingOrder(Dictionary<double, string> woDic)
        {
            var woIds = woDic.Keys.ToList();
            var woinfoDic = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderList(woIds);
            var helper = new CalculateQtyHelper(woIds);
            helper.InitDataBase();
            // 获取工单bom
            var bomDic = helper.GetBomDic();
            // 更新工单数量
            List<double> updateWoIds = new List<double>();
            List<CreateShippingOrderData> createShippingOrderDatas = new List<CreateShippingOrderData>();

            foreach (var woId in woIds)
            {
                var successCount = 0;
                var haswo = woinfoDic.TryGetValue(woId, out var wo);
                var hasbom = bomDic.TryGetValue(woId, out var boms);
                if (!haswo)
                {
                    throw new ValidationException("工单[{0}]未维护车间资源".L10nFormat(woDic[woId]));
                }
                if (!hasbom)
                {
                    throw new ValidationException("工单[{0}]不存在推式物料Bom".L10nFormat(woDic[woId]));
                }
                foreach (var bom in boms)
                {
                    decimal canPrepareQty = helper.CalculateCanQty(woId, bom);
                    if (canPrepareQty > 0)
                    {
                        var enterpriseCode = wo.ResourceCode.IsNotEmpty() ? wo.ResourceCode : wo.WorkShopCode;
                        CreateShippingOrderData createShippingOrderData = new CreateShippingOrderData
                        {
                            InvOrgId = RT.InvOrg.Value,
                            SourceType = 0,
                            SourceKey = string.Empty,
                            EnterpriseCode = enterpriseCode,
                            IsWoShipMore = false,
                            TransactionType = 1,
                            OrderNo = wo.WorkOrderNo,
                            OrderLineNo = bom.LineNo,
                            ItemCode = bom.ItemCode,
                            SecondExceptQty = canPrepareQty,
                            SecondUnitName = bom.UnitName,
                            ItemExtPropName = bom.ItemExtPropName,
                            DeliveryDate = wo.CreateTime,
                            ShippingWarehouseCode = string.Empty,
                            AppointProjectNo = wo.ProjectMaintainCode.IsNullOrEmpty() ? "*" : wo.ProjectMaintainCode,
                        };
                        
                        createShippingOrderDatas.Add(createShippingOrderData);
                        successCount++;
                    }
                }

                if (successCount <= 0)
                {
                    throw new ValidationException("工单[{0}]不存在可备料明细".L10nFormat(wo.WorkOrderNo));
                }
                else
                {
                    updateWoIds.Add(woId);
                }
            }

            if (updateWoIds.Any())
            {   
                using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
                {
                    // 调用WMS接口创建发运订单
                    RT.Service.Resolve<ILesShippingOrder>().CreateShippingOrder(createShippingOrderDatas);
                    DB.Update<WorkOrder>().Set(p => p.WorkOrderMpType, WorkOrderMpType.Direct).Where(p => updateWoIds.Contains(p.Id)).Execute();
                    tran.Complete();
                }
            }
        }

        /// <summary>
        /// 一键备料创建备料单
        /// </summary>
        /// <param name="woDic">工单</param>
        public virtual void OneKeyCreateMpOrders(Dictionary<double, string> woDic)
        {
            var woIds = woDic.Keys.ToList();
            var woinfoDic = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderList(woIds);
            EntityList<MaterialPreparation> materialPreparations = new EntityList<MaterialPreparation>();
            EntityList<MaterialPreparationDetail> materialPreparationDetails = new EntityList<MaterialPreparationDetail>();
            List<CreateShippingOrderData> createShippingOrderDatas = new List<CreateShippingOrderData>();
            // 获取产线线边仓
            var resourceIds = woinfoDic.Values.Select(p => p.ResourceId).ToList();
            var lineSideList = RT.Service.Resolve<LinesideWarehouseController>().GetLinesideWarehousesByIds(resourceIds);
            var lineSideDic = lineSideList.ToDictionary(p =>p.WipResouceId + "-" + p.WorkShopId + "-" + p.FactoryId);

            List<double> upDateWoIds = new List<double>();
            var helper = new CalculateQtyHelper(woIds);
            helper.InitDataBase();
            // 获取工单bom
            var bomDic = helper.GetBomDic();

            // 获取备料单号
            var nos = GetMaterialPreprationNo(woinfoDic.Count);
            int index = 0;
            foreach (var woId in woIds)
            {
                var hasInfo = woinfoDic.TryGetValue(woId, out var info);
                var hasBoms = bomDic.TryGetValue(woId, out var boms);
                if (!hasInfo)
                {
                    throw new ValidationException("工单[{0}]未维护车间资源".L10nFormat(woDic[woId]));
                }
                if (!hasBoms)
                {
                    throw new ValidationException("工单[{0}]不存在推式物料Bom".L10nFormat(woDic[woId]));
                }
                var hasLine = lineSideDic.TryGetValue(info.ResourceId + "-" + info.WorkShopId + "-" + info.FactoryId, out var linesideWarehouse);
                if (!hasLine)
                {
                    throw new ValidationException("工单[{0}]未维护对应的产线线边仓".L10nFormat(woDic[woId]));
                }
                MaterialPreparation material = new MaterialPreparation
                {
                    No = nos[index++],
                    WorkOrderId = woId,
                    WorkShopId = info.WorkShopId,
                    ResourceId = info.ResourceId,
                    FactoryId = info.FactoryId,
                    PrepareType = PrepareType.Pmw,
                    PrepareStatus = PrepareStatus.Submitted, // 直接视作提交
                    LineSideWarehouseId = linesideWarehouse.WarehouseId,
                    ProjectMaintainId = info.ProjectMaintainId,
                };

                // 当前工单备料需求单明细
                EntityList<MaterialPreparationDetail> detailList = new EntityList<MaterialPreparationDetail>();
                // 当前工单发运订单数据
                List<CreateShippingOrderData> shippingOrderDatas = new List<CreateShippingOrderData>();
                foreach (var bom in boms)
                {
                    decimal canPrepareQty = helper.CalculateCanQty(woId, bom);
                    if (canPrepareQty <= 0)
                    {
                        continue;
                    }
                    MaterialPreparationDetail detail = new MaterialPreparationDetail
                    {
                        MaterialPreparation = material,
                        PreDetailStatus = PrepareDetailStatus.ToShipping, // 直接视作提交
                        LineNo = bom.LineNo,
                        ItemId = bom.ItemId,
                        ItemCode = bom.ItemCode,
                        ItemName = bom.ItemName,
                        ItemConsumeMode = bom.ConsumeMode,
                        ItemExtProp = bom.ItemExtProp,
                        ItemExtPropName = bom.ItemExtPropName,
                        UnitName = bom.UnitName,
                        BomNeedQty = bom.BomNeedQty,
                        Qty = canPrepareQty,
                        MpWoId = woId,
                        MpWo = info.WorkOrderNo
                    };
                    detailList.Add(detail);

                    var enterpriseCode = info.ResourceCode.IsNotEmpty() ? info.ResourceCode : info.WorkShopCode;
                    // 创建发运订单
                    CreateShippingOrderData createShippingOrderData = new CreateShippingOrderData
                    {
                        InvOrgId = RT.InvOrg.Value,
                        SourceType = 1,
                        SourceKey = string.Empty,
                        EnterpriseCode = enterpriseCode,
                        IsWoShipMore = false,
                        TransactionType = 1,
                        RequireNo = material.No,
                        RequireLineNo = bom.LineNo,
                        OrderNo = info.WorkOrderNo,
                        OrderLineNo = bom.LineNo,
                        ItemCode = bom.ItemCode,
                        SecondExceptQty = canPrepareQty,
                        SecondUnitName = bom.UnitName,
                        ItemExtPropName = bom.ItemExtPropName,
                        DeliveryDate = info.CreateTime,
                        ShippingWarehouseCode = string.Empty,
                        AppointProjectNo = info.ProjectMaintainCode.IsNullOrEmpty() ? "*" : info.ProjectMaintainCode,
                    };
                    shippingOrderDatas.Add(createShippingOrderData);
                }
                if (!detailList.Any())
                {
                    throw new ValidationException("工单[{0}]不存在可备料明细".L10nFormat(info.WorkOrderNo));
                }
                else
                {
                    upDateWoIds.Add(woId);
                    materialPreparations.Add(material);
                    materialPreparationDetails.AddRange(detailList);
                    createShippingOrderDatas.AddRange(shippingOrderDatas);
                }
            }

            if (materialPreparations.Any())
            {
                using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
                {

                    RT.Service.Resolve<CommonController>().BatchInsertSave(materialPreparations);
                    materialPreparationDetails.ForEach(detail =>
                    {
                        detail.MaterialPreparationId = detail.MaterialPreparation.Id;
                    });
                    RT.Service.Resolve<CommonController>().BatchInsertSave(materialPreparationDetails);

                    // 调用WMS接口创建发运订单
                    RT.Service.Resolve<ILesShippingOrder>().CreateShippingOrder(createShippingOrderDatas);

                    if (upDateWoIds.Any())
                    {
                        DB.Update<WorkOrder>().Set(p => p.WorkOrderMpType, WorkOrderMpType.ByOrder).Where(p => upDateWoIds.Contains(p.Id)).Execute();
                    }
                    tran.Complete();
                }
            }
        }

        #endregion

        /// <summary>
        /// 根据单号列表查询数据
        /// </summary>
        /// <param name="noList"></param>
        /// <param name="elo"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialPreparation> GetMaterialPreparationList(List<string> noList, EagerLoadOptions elo = null)
        {

            var list = noList.SplitContains(nos =>
            {
                return Query<MaterialPreparation>().Where(p => nos.Contains(p.No)).ToList(null, elo);
            });

            return list;
        }

        /// <summary>
        /// 根据工单号获取备料申请明细
        /// </summary>
        /// <param name="woNo">工单号</param>
        /// <param name="elo">贪婪</param>
        /// <returns></returns>
        public virtual EntityList<MaterialPreparationDetail> GetMaterialPreparationDetailsByWoNo(string woNo, EagerLoadOptions elo = null)
        {
            var query = Query<MaterialPreparationDetail>().Where(p => p.MpWo == woNo && p.PreDetailStatus < PrepareDetailStatus.Canceled);
            return query.ToList(null, elo);
        }
    }
}
