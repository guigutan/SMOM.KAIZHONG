using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Core.Enums;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Enums;
using SIE.Equipments.WorkFlows;
using SIE.EventMessages.EMS.Fixtures;
using SIE.EventMessages.MES.WorkOrders;
using SIE.Fixtures.FixtureDemands;
using SIE.Fixtures.FixtureDemands.Config;
using SIE.Fixtures.FixtureDemands.Configs;
using SIE.Fixtures.FixtureDemands.ViewModels;
using SIE.Fixtures.FixtureRecords;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.MaintainTasks;
using SIE.Fixtures.Models;
using SIE.Items;
using SIE.Resources.ProcessSegments;
using SIE.TurnoverTools.TurnoverTools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Fixtures
{
    /// <summary>
    /// 工治具任务控制器_工治具需求清单
    /// </summary>
    public partial class CoreFixtureController
    {

        /// <summary>
        /// 根据需求单号获取工治具需求清单
        /// </summary>
        /// <param name="no">需求单号</param>
        /// <returns>工治具需求清单</returns>
        public virtual FixtureDemand GetFixtureDemand(string no)
        {
            return Query<FixtureDemand>().Where(p => p.No == no).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据需求单号列表获取工治具需求清单
        /// </summary>
        /// <param name="nos">需求单号列表</param>
        /// <returns>工治具需求清单</returns>
        public virtual EntityList<FixtureDemand> GetFixtureDemandsByNos(List<string> nos)
        {
            return Query<FixtureDemand>().Where(p => nos.Contains(p.No)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具需求清单Id列表获取工治具需求清单列表
        /// </summary>
        /// <param name="ids">工治具需求清单Id列表</param>
        /// <returns>工治具需求清单列表</returns>
        public virtual EntityList<FixtureDemand> GetFixtureDemands(List<double> ids)
        {
            return Query<FixtureDemand>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工治具需求清单列表
        /// </summary>
        /// <param name="criteria">工治具需求清单查询体</param>
        /// <returns>工治具需求清单列表</returns>
        public virtual EntityList<FixtureDemand> GetFixtureDemands(FixtureDemandCriteria criteria)
        {
            var query = Query<FixtureDemand>();
            if (criteria.No.IsNotEmpty())
                query.Where(p => p.No.Contains(criteria.No));
            if (criteria.WorkOrderNo.IsNotEmpty() || criteria.ProductCode.IsNotEmpty())
            {
                query.Exists<WorkOrder>((x, y) =>
                       y.Join<Item>((c, d) => c.ProductId == d.Id)
                       .Where(p => p.Id == x.WorkOrderId)
               .WhereIf(criteria.WorkOrderNo.IsNotEmpty(), c => c.No.Contains(criteria.WorkOrderNo))
                .WhereIf<Item>(criteria.ProductCode.IsNotEmpty(), (c, d) => d.Code.Contains(criteria.ProductCode)));
            }

            if (criteria.WorkShopId.HasValue)
                query.Where(p => p.WorkShopId == criteria.WorkShopId);
            if (criteria.ResourceId.HasValue)
                query.Where(p => p.ResourceId == criteria.ResourceId);
            if (criteria.EmployeeId.HasValue)
                query.Where(p => p.CreateBy == criteria.EmployeeId);
            if (criteria.DemandState.HasValue)
                query.Where(p => p.DemandState == criteria.DemandState);
            if (criteria.ReceiveState.HasValue)
                query.Where(p => p.ReceiveState == criteria.ReceiveState);
            if (criteria.CreateDate.BeginValue.HasValue)
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            if (criteria.CreateDate.EndValue.HasValue)
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工治具需求明细
        /// </summary>
        /// <param name="woId"></param>
        /// <param name="procesegmentId"></param>
        /// <param name="fixtureDemandId"></param>
        public virtual EntityList<FixtureDemandDetail> GetFixtureDemandDetailList(double? woId, double procesegmentId, double fixtureDemandId)
        {
            var res = Query<FixtureEncodeProductDetail>().Join<FixtureEncode>((y, x) => x.Id == y.FixtureEncodeId)
                .Join<FixtureEncodeProductDetail, WorkOrder>((x, y) => x.ItemId == y.ProductId && y.Id == woId && x.ProcessSegmentId == procesegmentId)
                .Select<FixtureEncode>((z, y) => new
                {
                    FixtureEncodeId = y.Id,
                    DemandQty = z.DemandQuantity,
                    ItemId = z.ItemId,
                    FixtureModelId = y.FixtureModelId,
                    ModelCode = y.FixtureModel.Code,
                    ModelName = y.FixtureModel.Name,
                    FixtureTypeId = y.FixtureModel.FixtureTypeId,
                    FixtureTypeCode = y.FixtureModel.FixtureType.Code,
                    ProcessSegmentId = z.ProcessSegmentId,
                    ProcessSegmentCode = z.ProcessSegment.Code,
                    FixtureDemandId = fixtureDemandId,
                    FixtureEncodeCode = y.Code,
                    ProcessSurface = z.Deck

                }).ToList<FixtureDemandDetail>();
            EntityList<FixtureDemandDetail> result = new EntityList<FixtureDemandDetail>();
            if (res.Any()) result.AddRange(res);
            return result;
        }

        /// <summary>
        /// 获取工治具需求明细
        /// </summary>
        /// <param name="demandId">工治具需求清单Id</param>
        /// <param name="encodeId">工治具编码Id</param>
        /// <returns>工治具需求明细</returns>
        public virtual FixtureDemandDetail GetFixtureDemandDetail(double demandId, double encodeId)
        {
            return Query<FixtureDemandDetail>().Exists<FixtureEncode>((a, b) => b.Where(f => f.Id == encodeId && a.FixtureDemandId == demandId)).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工治具需求清单需求明细列表
        /// </summary>
        /// <param name="id">工治具需求清单Id</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>工治具需求清单需求明细列表</returns>
        public virtual EntityList<FixtureDemandDetail> GetFixtureDemandDetails(double id, PagingInfo pagingInfo = null)
        {
            return Query<FixtureDemandDetail>().Where(p => p.FixtureDemandId == id).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具需求ID和工治具编码ID获取工治具需求明细
        /// </summary>
        /// <param name="fixtureDemandId">工治具需求ID</param>
        /// <param name="fixtureEncodeId">工治具编码ID</param>
        /// <returns>工治具需求明细</returns>
        public virtual FixtureDemandDetail GetDemandDetailByEncode(double fixtureDemandId, double fixtureEncodeId)
        {
            return Query<FixtureDemandDetail>().Where(p => p.FixtureDemandId == fixtureDemandId && p.FixtureEncodeId == fixtureEncodeId).FirstOrDefault();
        }

        /// <summary>
        /// 获取出库明细列表
        /// </summary>
        /// <param name="demandId">工治具需求清单Id</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>出库明细列表</returns>
        public virtual EntityList<FixtureUnload> GetFixtureUnloadsByDemandId(double demandId, PagingInfo pagingInfo)
        {
            return Query<FixtureUnload>().Exists<FixtureDemand>((a, b) => b.Where(f => f.Id == a.FixtureDemandId && f.Id == demandId)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取出库明细ViewModel列表
        /// </summary>
        /// <param name="id">工治具需求清单Id</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>出库明细ViewModel列表</returns>
        public virtual EntityList<FixtureUnloadViewModel> GetFixtureUnloadViewModels(double id, PagingInfo pagingInfo)
        {
            var unloadVMList = new EntityList<FixtureUnloadViewModel>();
            var fixUnloads = GetFixtureUnloadsByDemandId(id, pagingInfo);

            foreach (var fixUnload in fixUnloads)
            {
                var fixUnloadVM = new FixtureUnloadViewModel()
                {
                    Id = fixUnload.Id,
                    WarehouseId = fixUnload.WarehouseId,
                    WarehouseCode = fixUnload.WarehouseCode,
                    WarehouseName = fixUnload.WarehouseName,
                    LocationId = fixUnload.LocationId,
                    LocationCode = fixUnload.LocationCode,
                    LocationName = fixUnload.LocationName,
                    FixtureAccountId = fixUnload.FixtureAccountId,
                    EncodeId = fixUnload.EncodeId,
                    AccountCode = fixUnload.AccountCode,
                    UnloadQty = fixUnload.UnloadQty,
                    NgQty = fixUnload.NgQty,
                    TurnoverToolCode = fixUnload.TurnoverToolCode,
                    UnloadById = fixUnload.UnloadById,
                    UnloadByName = fixUnload.UnloadByName,
                    UnloadDate = fixUnload.UnloadDate,
                    IsOld = true
                };

                unloadVMList.Add(fixUnloadVM);
            }

            return unloadVMList;
        }

        /// <summary>
        /// 根据工治具需求清单Id和工治具编码Id获取需求明细
        /// </summary>
        /// <param name="demandId">工治具需求清单Id</param>
        /// <param name="encodeId">工治具编码Id</param>
        /// <returns>需求明细</returns>
        public virtual FixtureDemandDetail GetDetailsByDemandIdAndEncodeId(double demandId, double encodeId)
        {
            return Query<FixtureDemandDetail>().Where(p => p.FixtureDemandId == demandId && p.FixtureEncodeId == encodeId).FirstOrDefault();
        }

        /// <summary>
        /// 根据工治具需求清单Id列表获取需求明细列表
        /// </summary>
        /// <param name="demandIds">工治具需求清单Id列表</param>
        /// <returns>需求明细列表</returns>
        public virtual EntityList<FixtureDemandDetail> GetDetailsByDemandIds(List<double> demandIds)
        {
            return Query<FixtureDemandDetail>().Where(p => demandIds.Contains(p.FixtureDemandId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具需求Id、工治具Id、保养任务ID获取出库明细列表
        /// </summary>
        /// <param name="demandId">工治具需求Id</param>
        /// <param name="fixtureAccountId">工治具Id</param>
        /// <param name="maintainTaskId">保养任务ID</param>
        /// <returns>出库明细列表</returns>
        public virtual EntityList<FixtureUnload> GetNgFixtureUnloads(double demandId, double fixtureAccountId, double maintainTaskId)
        {
            return Query<FixtureUnload>().Where(p => p.FixtureDemandId == demandId && p.FixtureAccountId == fixtureAccountId && p.MaintainTaskId == maintainTaskId)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具需求清单Id列表获取对应【领用状态】的出库明细列表
        /// </summary>
        /// <param name="demandIds">工治具需求清单Id列表</param>
        /// <param name="receiveState">领用状态</param>
        /// <param name="pagingInfo">分页参数</param>
        /// <returns>出库明细列表</returns>
        public virtual EntityList<FixtureUnload> GetUnloadsByDemandIds(List<double> demandIds, ReceiveState receiveState, PagingInfo pagingInfo = null)
        {
            return Query<FixtureUnload>().Where(p => demandIds.Contains(p.FixtureDemandId) && p.State == receiveState)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具ID获取未领用的出库明细列表
        /// </summary>
        /// <param name="fixtureAccountId">工治具ID</param>
        /// <returns>未领用的出库明细列表</returns>
        public virtual EntityList<FixtureUnload> GetUnloadsByAccountId(double fixtureAccountId)
        {
            return Query<FixtureUnload>().Where(p => p.FixtureAccountId == fixtureAccountId && p.State == ReceiveState.None)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具台帐Id列表获取工治具需求清单的出库明细列表
        /// </summary>
        /// <param name="accountIds">工治具台帐Id列表</param>
        /// <returns>出库明细列表</returns>
        public virtual EntityList<FixtureUnload> GetFixtureUnloadsByAccountIds(List<double> accountIds)
        {
            return accountIds.SplitContains(tempIds =>
            {
                return Query<FixtureUnload>()
                    .Where(p => tempIds.Contains(p.FixtureAccountId) && p.UnloadQty > p.ReturnQty)
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 根据工治具需求清单Id跟治具编码Id获取出库明细列表
        /// </summary>
        /// <param name="demandId">工治具需求清单Id</param>
        /// <param name="encodeCodeId">治具编码ID</param>
        /// <param name="pagingInfo">分页参数</param>
        /// <returns>出库明细列表</returns>
        public virtual EntityList<FixtureUnload> GetFixtureUnloadByEncodeCodeId(double demandId, double encodeCodeId, PagingInfo pagingInfo)
        {
            return Query<FixtureUnload>().Exists<FixtureAccount>((a, b) => b.Where(f => f.Id == a.FixtureAccountId && f.FixtureEncodeId == encodeCodeId))
                .Where(p => p.FixtureDemandId == demandId).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具需求清单获取出库明细列表
        /// </summary>
        /// <param name="demandId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<FixtureUnload> GetFixtureUnloadBydemandId(double demandId, PagingInfo pagingInfo)
        {
            return Query<FixtureUnload>().Exists<FixtureAccount>((a, b) => b.Where(f => f.Id == a.FixtureAccountId))
                .Where(p => p.FixtureDemandId == demandId).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 根据【产线、工单、工治具】获取可归还的出库明细列表
        /// </summary>
        /// <param name="workOrderId">产线id</param>
        /// <param name="resourceId">工单id</param>
        /// <param name="accountId">工治具id</param>
        /// <returns>可归还的出库明细列表</returns>
        public virtual EntityList<FixtureUnload> GetReturnUnloads(double workOrderId, double resourceId, double accountId)
        {
            var demandIds = Query<FixtureDemand>().Where(p => p.WorkOrderId == workOrderId && p.ResourceId == resourceId)
                .Select(p => p.Id).ToList<double>().ToList();
            if (!demandIds.Any())
                throw new ValidationException("找不到此产线和工单的工治具需求清单！".L10N());
            var unloads = Query<FixtureUnload>().Where(p => demandIds.Contains(p.FixtureDemandId) && p.State == ReceiveState.Finish
                          && p.ReturnQty < (p.UnloadQty - p.NgQty) && p.FixtureAccountId == accountId).ToList();
            return unloads;
        }

        /// <summary>
        /// 根据工治具获取可归还的需求单
        /// </summary>
        /// <param name="accountId">工治具id</param>
        /// <returns>可归还的需求单</returns>
        public virtual FixtureDemand GetReturnUnloadsByAccountId(double accountId)
        {
            return Query<FixtureDemand>().Join<FixtureUnload>((a, p) => a.Id == p.FixtureDemandId && p.State == ReceiveState.Finish && p.ReturnQty < p.UnloadQty
            && p.FixtureAccountId == accountId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具编码Id获取库存台账列表
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="encodeId">工治具编码Id</param>
        /// <returns>库存台账列表</returns>
        public virtual EntityList<FixtureAccountStock> GetFixtureStocks(double? warehouseId, double encodeId)
        {
            var query = Query<FixtureAccountStock>().Exists<FixtureAccount>((a, b) => b.Where(f => f.Id == a.FixtureAccountId && f.FixtureEncodeId == encodeId && a.PassQty > 0));
            if (warehouseId.HasValue)
                query.Where(p => p.FixtureWarehouseId == warehouseId);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取库存情况ViewModel列表
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="demandDetail">工治具需求明细</param>
        /// <param name="fixUnloadVMs">出库明细ViewModel列表</param>
        /// <returns>库存情况ViewModel列表</returns>
        public virtual List<UnloadStockViewModel> GetUnloadStockVMs(double? warehouseId, FixtureDemandDetail demandDetail, List<FixtureUnloadViewModel> fixUnloadVMs)
        {
            var unloadStockVMs = new List<UnloadStockViewModel>();
            var stockList = GetFixtureStocks(warehouseId, demandDetail.FixtureEncodeId);
            var accountIds = stockList.Select(p => p.FixtureAccountId).Distinct().ToList();
            var newUnloadVMs = fixUnloadVMs.Where(p => !p.IsOld && accountIds.Contains(p.FixtureAccountId));

            //加载任务相关信息
            var simulation = new DemandSimulation();
            simulation.LoadUnloadInfo(newUnloadVMs);
            GetUnloadStockVMs(demandDetail, unloadStockVMs, stockList, simulation);

            return unloadStockVMs;
        }

        /// <summary>
        /// 获取库存情况ViewModel列表
        /// </summary>
        /// <param name="demandDetail">工治具需求明细</param>
        /// <param name="unloadStockVMs">库存情况ViewModel列表</param>
        /// <param name="stockList">库存列表</param>
        /// <param name="simulation">需求仿真器</param>
        private void GetUnloadStockVMs(FixtureDemandDetail demandDetail, List<UnloadStockViewModel> unloadStockVMs, EntityList<FixtureAccountStock> stockList, DemandSimulation simulation)
        {
            foreach (var stock in stockList)
            {
                var key = new DicUnloadKey()
                {
                    AccountId = stock.FixtureAccountId,
                    WarehouseId = stock.FixtureWarehouseId,
                    LocationId = stock.FixtureStorageLocationId == null ? 0 : stock.FixtureStorageLocationId.Value
                };

                var unloadStockVM = CreateUnloadStockVM(demandDetail, stock);

                if (simulation.DicUnloadVMs.ContainsKey(key))
                {
                    unloadStockVM.Qty -= simulation.DicUnloadVMs[key].Sum(p => p.UnloadQty);
                }

                if (unloadStockVM.Qty == 0)
                {
                    continue;
                }

                unloadStockVMs.Add(unloadStockVM);
            }
        }

        /// <summary>
        /// 创建工治具需求清单信息
        /// </summary>
        /// <returns>工治具需求清单信息</returns>
        public virtual AddDemandInfo CreateDemandInfo()
        {
            AddDemandInfo demandInfo = new AddDemandInfo();
            demandInfo.ErrMsg = string.Empty;
            try
            {
                var now = RF.Find<FixtureDemand>().GetDbTime();
                FixtureDemand fixDemand = new FixtureDemand();
                fixDemand.No = _commonController.GetNo<FixtureDemand>("工治具需求清单单号");
                fixDemand.CreateDate = now;
                fixDemand.UpdateDate = now;
                fixDemand.DemandTime = now;
                fixDemand.ApprovalStatus = ApprovalStatus.Draft;
                fixDemand.DemandState = DemandState.None;
                fixDemand.GenerateId();
                fixDemand.Billsource = Enums.BillSource.Manual;
                demandInfo.Data = fixDemand;
            }
            catch (Exception ex)
            {
                demandInfo.ErrMsg = ex.Message;
            }

            return demandInfo;
        }

        /// <summary>
        /// 保存工治具需求清单信息
        /// </summary>
        /// <param name="fixDemandInfo">工治具需求清单信息</param>
        /// <returns>错误信息</returns>
        public virtual string SaveFixtureDemandInfo(FixtureDemand fixDemandInfo)
        {
            string errMsg = string.Empty;
            EntityList<FixtureDemandDetail> demandDetailList = new EntityList<FixtureDemandDetail>();
            try
            {
                SetFixtureDemand(fixDemandInfo);
                ValidateDemandDetails(fixDemandInfo);
                ValidateFixtureEncode(fixDemandInfo);
                using (var trans = DB.TransactionScope(KitFixturesEntityDataProvider.ConnectionStringName))
                {
                    RF.Save(fixDemandInfo); //更新记录
                    RF.Save(demandDetailList);
                    trans.Complete();
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return errMsg;
        }

        /// <summary>
        /// 验证工治具编码是否合法
        /// </summary>
        /// <param name="fixtureDemand">工治具需求清单</param>
        private void ValidateFixtureEncode(FixtureDemand fixtureDemand)
        {
            var demandDetails = fixtureDemand.DetailList.ToList();
            var wo = RF.GetById<WorkOrder>(fixtureDemand.WorkOrderId);
            if (wo == null)
                throw new ValidationException("当前工单不存在！".L10N());

            ////var deckId = RT.Service.Resolve<IProcessSurface>().GetProcessSurface(fixtureDemand.WorkOrderId);

            var encodeIds = demandDetails.Select(p => p.FixtureEncodeId).Distinct().ToList();
            var encodes = RT.Service.Resolve<CoreFixtureController>().GetFixtureEncodesByIds(encodeIds);
            var dicEncodes = encodes.ToDictionary(p => p.Id);
            var productDetails = RT.Service.Resolve<CoreFixtureController>().GetProductDetailsByEncodeProductId(encodeIds, wo.ProductId);
            var dicProductDetails = productDetails.GroupBy(p => p.FixtureEncodeId).ToDictionary(p => p.Key, p => p.ToList());

            ValidateDeck(demandDetails, dicEncodes, dicProductDetails);
        }

        /// <summary>
        /// 验证工单的工艺面与工治具编码的产品清单工艺面是否一致
        /// </summary>
        /// <param name="demandDetails">工治具需求明细列表</param>
        /// <param name="dicEncodes">工治具编码列表</param>
        /// <param name="dicProductDetails">工治具编码的产品清单列表</param>
        private void ValidateDeck(List<FixtureDemandDetail> demandDetails, Dictionary<double, FixtureEncode> dicEncodes, Dictionary<double, List<FixtureEncodeProductDetail>> dicProductDetails)
        {
            foreach (var demandDetail in demandDetails)
            {
                dicEncodes.TryGetValue(demandDetail.FixtureEncodeId, out FixtureEncode encode);
                if (encode == null)
                    throw new ValidationException("工治具编码不存在，请确认！".L10N());
                if (encode.BindProduct == YesNo.Yes)
                {
                    if (dicProductDetails.TryGetValue(encode.Id, out List<FixtureEncodeProductDetail> productDetailsOfEncode))
                    {
                        ////需求原因 暂且注释
                        ////if (deckId != null)
                        ////{
                        ////    if (!productDetailsOfEncode.Any(p => p.Deck == (SIE.Core.Enums.Deck)deckId))
                        ////        throw new ValidationException("工单的工艺面与工治具编码的产品清单工艺面不一致".L10N());
                        ////}
                        ////else
                        ////{
                        ////    if (!productDetailsOfEncode.Any(p => p.Deck == null))
                        ////        throw new ValidationException("工单的工艺面与工治具编码的产品清单工艺面不一致".L10N());
                        ////}
                    }
                    else
                        throw new ValidationException("工治具编码未维护符合条件的产品清单，请重新操作！".L10N());
                }
            }
        }

        /// <summary>
        /// 设置工治具需求清单信息
        /// </summary>
        /// <param name="fixtureDemand">工治具需求清单</param>
        private void SetFixtureDemand(FixtureDemand fixtureDemand)
        {
            var oldDemand = RF.GetById<FixtureDemand>(fixtureDemand.Id);
            if (oldDemand == null)
            {
                fixtureDemand.PersistenceStatus = PersistenceStatus.New;
                fixtureDemand.ReceiveState = ReceiveState.None;
            }
            else
            {
                fixtureDemand.PersistenceStatus = PersistenceStatus.Modified;
            }
        }

        /// <summary>
        /// 验证需求明细列表信息
        /// </summary>
        /// <param name="fixtureDemand">需求</param>
        private void ValidateDemandDetails(FixtureDemand fixtureDemand)
        {
            List<FixtureDemandDetail> demandDetails = fixtureDemand.DetailList.ToList();
            if (!demandDetails.Any())
                throw new ValidationException("必须维护一笔需求明细！".L10N());
            if (demandDetails.Any(p => !p.FixtureTypeId.HasValue))
                throw new ValidationException("工治具类型必填，请重新操作！".L10N());
            if (demandDetails.Any(p => !p.FixtureModelId.HasValue))
                throw new ValidationException("工治具型号必填，请重新操作！".L10N());
            if (demandDetails.Any(p => p.FixtureEncode == null))
                throw new ValidationException("工治具治编码必填，请重新操作！".L10N());
            if (demandDetails.Any(p => p.DemandQty <= 0))
                throw new ValidationException("需求数量须大于0，请重新操作！".L10N());
            demandDetails.GroupBy(p => p.FixtureEncodeId).ToDictionary(p => p.Key, p => p.Count()).ForEach(p =>
                 {
                     if (p.Value > 1)
                         throw new ValidationException("工治具编码不能重复，请重新操作！".L10N());
                 });
            if (fixtureDemand.ProcessSegmentId.HasValue && demandDetails.Any(m => !m.ProcessSegmentId.HasValue))
            {
                throw new ValidationException("需求清单单据存在工段时，需求清单明细工段必填，请选择工段！".L10N());
            }


        }

        /// <summary>
        /// 获取工治具出库信息
        /// </summary>
        /// <param name="no">需求单号</param>
        /// <returns>工治具出库信息</returns>
        public virtual UnloadDemandInfo GetUnloadDemandInfo(string no)
        {
            var unloadInfo = new UnloadDemandInfo();
            unloadInfo.ErrMsg = string.Empty;
            try
            {
                var fixDemand = GetFixtureDemand(no);
                unloadInfo.fixDemandVM = new FixtureDemandViewModel()
                {
                    No = fixDemand.No,
                    WorkOrderNo = fixDemand.WorkOrderNo,
                    WorkShopName = fixDemand.WorkShopName,
                    ResourceName = fixDemand.ResourceName,
                    ProductCode = fixDemand.WorkOrderProductCode
                };
                unloadInfo.fixDemandVM.Id = fixDemand.Id;
            }
            catch (Exception ex)
            {
                unloadInfo.ErrMsg = ex.Message;
            }

            return unloadInfo;
        }

        /// <summary>
        /// 根据单号获取【工治具需求清单】中【出库状态】不为【出库完成】的数据；
        /// </summary>
        /// <param name="pagingInfo">分页参数</param>
        /// <param name="no">需求单号</param>
        /// <returns>工治具需求清单</returns>
        public virtual EntityList<FixtureDemand> GetDemandsByStateAndNo(PagingInfo pagingInfo, string no)
        {
            //启用审批后列表数据取【工治具需求清单】中需求状态为审批通过且【出库状态】不为【出库完成】的数据
            var config = GetFixtureDemandsConfigValue();

            return Query<FixtureDemand>().Where(p => p.DemandState != DemandState.Finish && (p.Close == false || p.Close == null))
                .WhereIf(config != null && config.SwitchApproval, p => p.ApprovalStatus == ApprovalStatus.Audited)
                .WhereIf(no.IsNotEmpty(), p => p.No.Contains(no)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取【工治具需求清单】中【出库状态】不为【未出库】，【领用状态】不为【领用完成】的数据；
        /// </summary>
        /// <param name="pagingInfo">分页参数</param>
        /// <returns>工治具需求清单</returns>
        public virtual EntityList<FixtureDemand> GetReceiveDemandsByStateAndNo(PagingInfo pagingInfo)
        {
            return Query<FixtureDemand>().Where(p => p.DemandState != DemandState.None && p.ReceiveState != ReceiveState.Finish)
                .NotExists<MaintainTask>((x, y) => y.Where(p => p.RelatedNo == x.No && p.State != MaintainState.Finish))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 保存库存情况出库信息
        /// </summary>
        /// <param name="unloadInfo">库存出库信息</param>
        /// <returns>保存后的库存出库信息</returns>
        public virtual UnloadInfo SaveUnloadStockInfo(UnloadInfo unloadInfo)
        {
            unloadInfo.ErrMsg = String.Empty;
            try
            {
                var unloadStockVM = unloadInfo.UnloadStockVM;

                if (unloadStockVM.TurnoverToolCode.IsNotEmpty())
                {
                    var isExist = RT.Service.Resolve<KitTurnoverToolController>().IsTurnoverTool(unloadStockVM.TurnoverToolCode);
                    if (!isExist)
                        throw new ValidationException("载具不存在，请重新填写！".L10N());
                }

                var unloadedQty = unloadInfo.RestUnloadVMList.Where(p => p.EncodeId == unloadInfo.DemandDetail.FixtureEncodeId).Sum(p => p.UnloadQty);
                var unloadedNgQty = unloadInfo.RestUnloadVMList.Where(p => p.EncodeId == unloadInfo.DemandDetail.FixtureEncodeId).Sum(p => p.NgQty);
                if ((unloadedQty - unloadedNgQty + unloadStockVM.UnloadQty) > unloadInfo.DemandDetail.DemandQty)
                    throw new ValidationException("工治具编码[{0}]的出库数量[{1}]和已出库数量[{2}]不能大于需求数量[{3}]，请重新填写！".L10nFormat(unloadInfo.DemandDetail.EncodeCode, unloadStockVM.UnloadQty, unloadedQty - unloadedNgQty, unloadInfo.DemandDetail.DemandQty));

                var newUnloadVM = unloadInfo.RestUnloadVMList.FirstOrDefault(p => p.WarehouseId == unloadStockVM.WarehouseId && p.LocationId == unloadStockVM.LocationId && p.TurnoverToolCode == unloadStockVM.TurnoverToolCode && p.FixtureAccountId == unloadStockVM.FixtureAccountId && !p.IsOld);

                var now = RF.Find<FixtureUnload>().GetDbTime();
                if (newUnloadVM == null)
                {
                    newUnloadVM = CreateFixtureUnloadVM(unloadStockVM, now);
                    unloadInfo.RestUnloadVMList.Add(newUnloadVM);
                }
                else
                {
                    newUnloadVM.UnloadQty += unloadStockVM.UnloadQty;
                    newUnloadVM.UnloadById = RT.IdentityId;
                    newUnloadVM.UnloadDate = now;
                }

                CreateUnloadStockVMs(unloadInfo);
            }
            catch (Exception ex)
            {
                unloadInfo.ErrMsg = ex.Message;
            }

            return unloadInfo;
        }

        /// <summary>
        /// 创建出库明细ViewModel
        /// </summary>
        /// <param name="unloadStockVM">库存情况ViewModel</param>
        /// <param name="now">当前时间</param>
        /// <returns>出库明细ViewModel</returns>
        private FixtureUnloadViewModel CreateFixtureUnloadVM(UnloadStockViewModel unloadStockVM, DateTime now)
        {
            return new FixtureUnloadViewModel()
            {
                Id = 0,
                IsOld = false,
                FixtureAccountId = unloadStockVM.FixtureAccountId,
                FixtureAccount = unloadStockVM.FixtureAccount,
                EncodeId = unloadStockVM.EncodeId,
                AccountCode = unloadStockVM.AccountCode,
                WarehouseId = unloadStockVM.WarehouseId,
                WarehouseCode = unloadStockVM.WarehouseCode,
                WarehouseName = unloadStockVM.WarehouseName,
                LocationId = unloadStockVM.LocationId,
                LocationCode = unloadStockVM.LocationCode,
                LocationName = unloadStockVM.LocationName,
                FixtureDemandDetailId = unloadStockVM.FixtureDemandDetailId,
                UnloadQty = unloadStockVM.UnloadQty,
                NgQty = 0,
                TurnoverToolCode = unloadStockVM.TurnoverToolCode,
                UnloadById = RT.IdentityId,
                UnloadByName = RT.Identity.Name,
                UnloadDate = now
            };
        }

        /// <summary>
        /// 验证修改后的出库的数量和周转工具是否符合条件
        /// </summary>
        /// <param name="editUnloadInfo">当前编辑的出库明细ViewModel</param>
        /// <returns>错误信息</returns>
        public virtual string ValidateUnloadQty(UnloadInfo editUnloadInfo)
        {
            var errMsg = string.Empty;

            try
            {
                var demandId = editUnloadInfo.DemandId;
                var unloadVM = editUnloadInfo.FixtureUnloadVM;
                var unloadVMs = editUnloadInfo.RestUnloadVMList;
                var demandDetail = GetFixtureDemandDetail(demandId, unloadVM.EncodeId);
                var stock = RT.Service.Resolve<CoreFixtureController>().
                    GetFixtureAccountStock(unloadVM.FixtureAccountId, unloadVM.WarehouseId, unloadVM.LocationId);

                ValidateFixtureUnloadVMs(unloadVM, unloadVMs, demandDetail, stock);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return errMsg;
        }

        /// <summary>
        /// 验证出库明细出库数量和载具是否合法
        /// </summary>
        /// <param name="unloadVM">出库明细ViewModel</param>
        /// <param name="unloadVMs">界面所有出库明细ViewModel列表</param>
        /// <param name="demandDetail">工治具需求明细</param>
        /// <param name="stock">库存台帐</param>
        private void ValidateFixtureUnloadVMs(FixtureUnloadViewModel unloadVM, IEnumerable<FixtureUnloadViewModel> unloadVMs, FixtureDemandDetail demandDetail, FixtureAccountStock stock)
        {
            if (unloadVM.UnloadQty > stock.PassQty)
                throw new ValidationException("工治具台帐[{0}]的仓库库位[{1}-{2}]出库数量[{3}]不能大于库存合格数[{4}]，请重新填写！".L10nFormat(unloadVM.AccountCode, unloadVM.WarehouseName, unloadVM.LocationName, unloadVM.UnloadQty, stock.PassQty));
            if (unloadVM.UnloadQty > demandDetail.DemandQty)
                throw new ValidationException("工治具台帐[{0}]的仓库库位[{1}-{2}]出库数量[{3}]不能大于需求数量[{4}]，请重新填写！".L10nFormat(unloadVM.AccountCode, unloadVM.WarehouseName, unloadVM.LocationName, unloadVM.UnloadQty, demandDetail.DemandQty));
            var totalUnloadQty = unloadVMs.Where(p => p.EncodeId == demandDetail.FixtureEncodeId).Sum(p => p.UnloadQty);
            var totalNgQty = unloadVMs.Where(p => p.EncodeId == demandDetail.FixtureEncodeId).Sum(p => p.NgQty);
            if (totalUnloadQty - totalNgQty > demandDetail.DemandQty)
                throw new ValidationException("工治具编码[{0}]的总出库数量[{1}]不能大于需求数量[{2}]，请重新填写！".L10nFormat(demandDetail.EncodeCode, totalUnloadQty - totalNgQty, demandDetail.DemandQty));
            if (unloadVM.TurnoverToolCode.IsNotEmpty())
            {
                var isExist = RT.Service.Resolve<KitTurnoverToolController>().IsTurnoverTool(unloadVM.TurnoverToolCode);
                if (!isExist)
                    throw new ValidationException("载具不存在，请重新填写！".L10N());
            }
        }

        /// <summary>
        /// 更新库存情况信息
        /// </summary>
        /// <param name="delUnloadInfo">删除出库明细ViewModel信息</param>
        /// <returns>删除出库明细ViewModel信息后库存情况ViewModel列表信息</returns>
        public virtual UnloadInfo UpdateUnloadStockInfo(UnloadInfo delUnloadInfo)
        {
            var unloadInfo = new UnloadInfo();
            unloadInfo.UnloadStockVMList = new List<UnloadStockViewModel>();
            unloadInfo.ErrMsg = string.Empty;

            try
            {
                GetValidFixtureUnloadVMs(delUnloadInfo);
                unloadInfo.RestUnloadVMList = delUnloadInfo.RestUnloadVMList;
                unloadInfo.WarehouseId = delUnloadInfo.WarehouseId;
                unloadInfo.DemandId = delUnloadInfo.DemandId;
                unloadInfo.DemandDetail = delUnloadInfo.DemandDetail;
                unloadInfo.UnloadStockVM = delUnloadInfo.UnloadStockVM;

                CreateUnloadStockVMs(unloadInfo);
            }
            catch (Exception ex)
            {
                unloadInfo.ErrMsg = ex.Message;
            }

            return unloadInfo;
        }

        /// <summary>
        /// 获取有效的出库明细ViewModel列表
        /// </summary>
        /// <param name="delUnloadInfo">出库信息</param>
        private void GetValidFixtureUnloadVMs(UnloadInfo delUnloadInfo)
        {
            var fixUnloadVMs = new List<FixtureUnloadViewModel>();
            foreach (var restUnloadVM in delUnloadInfo.RestUnloadVMList)
            {
                if (!restUnloadVM.IsOld && restUnloadVM.UnloadQty == 0)
                    continue;
                fixUnloadVMs.Add(restUnloadVM);
            }

            delUnloadInfo.RestUnloadVMList = fixUnloadVMs;
        }

        /// <summary>
        /// 创建库存情况ViewModel列表
        /// </summary>
        /// <param name="unloadInfo">出库信息</param>
        private void CreateUnloadStockVMs(UnloadInfo unloadInfo)
        {
            var warehouseId = unloadInfo.WarehouseId;
            var demandDetail = unloadInfo.DemandDetail;
            var restUnloads = unloadInfo.RestUnloadVMList;
            var stockList = GetFixtureStocks(warehouseId, demandDetail.FixtureEncodeId);
            var accountIds = stockList.Select(p => p.FixtureAccountId).Distinct().ToList();
            var newUnloadVMs = restUnloads.Where(p => !p.IsOld && accountIds.Contains(p.FixtureAccountId));

            //加载任务相关信息
            var simulation = new DemandSimulation();
            simulation.LoadUnloadInfo(newUnloadVMs);

            GetUnloadStockVMs(demandDetail, unloadInfo.UnloadStockVMList, stockList, simulation);
        }

        /// <summary>
        /// 创建库存情况ViewModel
        /// </summary>
        /// <param name="demandDetail">工治具需求明细</param>
        /// <param name="stock">库存台账</param>
        /// <returns>库存情况ViewModel</returns>
        private UnloadStockViewModel CreateUnloadStockVM(FixtureDemandDetail demandDetail, FixtureAccountStock stock)
        {
            return new UnloadStockViewModel()
            {
                FixtureAccountId = stock.FixtureAccountId,
                AccountCode = stock.AccountCode,
                EncodeId = stock.EncodeId,
                LocationId = stock.FixtureStorageLocationId.Value,
                LocationCode = stock.LocationCode,
                LocationName = stock.LocationName,
                WarehouseId = stock.FixtureWarehouseId,
                WarehouseCode = stock.WarehouseCode,
                WarehouseName = stock.WarehouseName,
                FixtureDemandDetailId = demandDetail.Id,
                TurnoverToolCode = string.Empty,
                Qty = stock.PassQty
            };
        }

        /// <summary>
        /// 保存出库出库明细信息
        /// </summary>
        /// <param name="unloadInfo">出库信息</param>
        /// <returns>错误信息</returns>
        public virtual string SaveFixtureUnloadList(UnloadInfo unloadInfo)
        {
            string errMsg = string.Empty;
            try
            {
                var demandId = unloadInfo.DemandId;
                var fixUnloadVMs = unloadInfo.RestUnloadVMList;
                var newUnloadVMs = fixUnloadVMs.Where(p => !p.IsOld);

                //加载任务相关信息
                var simulation = new DemandSimulation();
                simulation.LoadDemandRelateInfo(demandId, newUnloadVMs);
                //验证出库明细ViewModel中出库数量和载具是否合法
                ValidateFixtureUnloads(fixUnloadVMs, simulation);

                using (var trans = DB.TransactionScope(KitFixturesEntityDataProvider.ConnectionStringName))
                {
                    //更新工治具台账及其工治具台账下的库存详情
                    UpdateAccountStockAndRecord(simulation);
                    //创建保养任务
                    CreateMaintainTask(simulation);
                    //创建出库明细和治具出入库记录
                    CreateFixtureUnload(newUnloadVMs, simulation);
                    //更新工治具需求清单
                    UpdateDemand(simulation);
                    //创建编码类台账的使用履历
                    CreateUseResum(simulation);
                    trans.Complete();
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return errMsg;
        }

        /// <summary>
        /// 验证出库明细ViewModel中出库数量和载具是否合法
        /// </summary>
        /// <param name="fixUnloadVMs">当前界面所有新增出库明细ViewModel列表</param>
        /// <param name="simulation">需求仿真器</param>
        private void ValidateFixtureUnloads(IEnumerable<FixtureUnloadViewModel> fixUnloadVMs, DemandSimulation simulation)
        {
            foreach (var newUnloadVM in fixUnloadVMs)
            {
                if (newUnloadVM.IsOld)
                    continue;
                var demandDetail = simulation.DemandDetails.FirstOrDefault(p => p.FixtureEncodeId == newUnloadVM.EncodeId);
                var key = new DicUnloadKey() { AccountId = newUnloadVM.FixtureAccountId, WarehouseId = newUnloadVM.WarehouseId, LocationId = newUnloadVM.LocationId };
                var stock = simulation.DicStcoks[key];
                ValidateFixtureUnloadVMs(newUnloadVM, fixUnloadVMs, demandDetail, stock);
            }
        }

        /// <summary>
        /// 创建保养任务
        /// </summary>
        /// <param name="simulation">需求仿真器</param>
        private void CreateMaintainTask(DemandSimulation simulation)
        {
            foreach (var dicUnloadVMOfAccount in simulation.DicUnloadVMsOfAccount)
            {
                if (simulation.DicMaintainPrjs.TryGetValue(dicUnloadVMOfAccount.Key.FixtureEncodeId, out List<FixtureEncodeMaintainProject> maintainPrjsOfEncode))
                    CreateSaveMaintainTask(simulation, dicUnloadVMOfAccount, maintainPrjsOfEncode);
            }
        }

        /// <summary>
        /// 更新工治具台账及其工治具台账下的库存详情
        /// </summary>
        /// <param name="simulation">需求仿真器</param>
        private void UpdateAccountStockAndRecord(DemandSimulation simulation)
        {
            foreach (var dicUnloadVM in simulation.DicUnloadVMs)
            {
                simulation.DicAccounts.TryGetValue(dicUnloadVM.Key.AccountId, out FixtureAccount account);
                //当前工治具台账包含需要保养的保养列表
                if (simulation.DicMaintainPrjs.TryGetValue(account.FixtureEncodeId, out List<FixtureEncodeMaintainProject> maintainPrjsOfEncode))
                {
                    UpdateAccountAndStock(true, simulation, dicUnloadVM, account);
                }
                else
                {
                    UpdateAccountAndStock(false, simulation, dicUnloadVM, account);
                }
                CreateSaveFixtureRecord(simulation.Demand, dicUnloadVM);
            }
        }

        /// <summary>
        /// 更新工治具台账及其工治具台账下的库存详情
        /// </summary>
        /// <param name="isNeedMaintain">是否需要出库保养</param>
        /// <param name="simulation">需求仿真器</param>
        /// <param name="dicUnloadVM">某Id编码、仓库和库存相同的出库明细ViewModel列表</param>
        /// <param name="account">工治具台账</param>
        private void UpdateAccountAndStock(bool isNeedMaintain, DemandSimulation simulation, KeyValuePair<DicUnloadKey, List<FixtureUnloadViewModel>> dicUnloadVM, FixtureAccount account)
        {
           
            if (account.ManageMode == ManageMode.Number)
            {
                //ID类台账更新状态
                SaveIdAccount(isNeedMaintain, account);
                DeleteStock(simulation, dicUnloadVM);
            }
            else
            {
                //编码类台账更新状态
                SaveCodeAccount(isNeedMaintain, dicUnloadVM, account);
                UpdateStock(simulation, dicUnloadVM);
            }
        }

        /// <summary>
        /// 删除Id类台账的库存详情
        /// </summary>
        /// <param name="simulation">需求仿真器</param>
        /// <param name="dicUnloadVM">某Id编码、仓库和库存相同的出库明细ViewModel列表</param>
        private void DeleteStock(DemandSimulation simulation, KeyValuePair<DicUnloadKey, List<FixtureUnloadViewModel>> dicUnloadVM)
        {
            if (simulation.DicStcoks.ContainsKey(dicUnloadVM.Key))
            {
                var fixStock = simulation.DicStcoks[dicUnloadVM.Key];
                fixStock.PersistenceStatus = PersistenceStatus.Deleted;
                RF.Save(fixStock);
            }
        }

        /// <summary>
        /// 保存Id类工治具台账
        /// </summary>
        /// <param name="isNeedMaintain">是否需要出库保养</param>
        /// <param name="account">工治具台账</param>
        private void SaveIdAccount(bool isNeedMaintain, FixtureAccount account)
        {
            if (isNeedMaintain)
            {
                account.AccountState = FixtureAccountState.WaitMaintain;
            }
            else
            {
                account.AccountState = FixtureAccountState.WaitReceive;
            }
            RF.Save(account);
        }

        /// <summary>
        /// 保存编码类工治具台账
        /// </summary>
        /// <param name="isNeedMaintain">是否需要出库保养</param>
        /// <param name="DicUnloadVM">某Id编码、仓库和库存相同的出库明细ViewModel列表</param>
        /// <param name="account">工治具台账</param>
        private void SaveCodeAccount(bool isNeedMaintain, KeyValuePair<DicUnloadKey, List<FixtureUnloadViewModel>> DicUnloadVM, FixtureAccount account)
        {
            //编码类台账更新数量
            var sumQty = DicUnloadVM.Value.Sum(p => p.UnloadQty);
            account.InStockQty -= sumQty;
            if (isNeedMaintain)
            {
                account.WaitMaintain += sumQty;
            }
            else
            {
                account.WaitReceive += sumQty;
            }
            RF.Save(account);
        }

        /// <summary>
        /// 更新某台账下的一笔库存详情
        /// </summary>
        /// <param name="simulation">需求仿真器</param>
        /// <param name="dicUnloadVM">某Id编码、仓库和库存相同的出库明细ViewModel列表</param>
        private void UpdateStock(DemandSimulation simulation, KeyValuePair<DicUnloadKey, List<FixtureUnloadViewModel>> dicUnloadVM)
        {
            if (simulation.DicStcoks.ContainsKey(dicUnloadVM.Key))
            {
                var sumQty = dicUnloadVM.Value.Sum(p => p.UnloadQty);
                var fixStock = simulation.DicStcoks[dicUnloadVM.Key];
                fixStock.TotalQty -= sumQty;
                fixStock.PassQty -= sumQty;
                fixStock.PersistenceStatus = PersistenceStatus.Modified;

                if (fixStock.TotalQty == 0)
                {
                    fixStock.PersistenceStatus = PersistenceStatus.Deleted;
                }
                RF.Save(fixStock);
            }
        }

        /// <summary>
        /// 按照出库明细中相同工治具台账生成一笔保养任务
        /// </summary>
        /// <param name="simulation">需求仿真器</param>
        /// <param name="DicUnloadVM"></param>
        /// <param name="maintainPrjsOfEncode">某工治具编码下的出库保养任务列表</param>
        private void CreateSaveMaintainTask(DemandSimulation simulation, KeyValuePair<FixtureAccount, List<FixtureUnloadViewModel>> DicUnloadVM, List<FixtureEncodeMaintainProject> maintainPrjsOfEncode)
        {
            var maintainTask = new MaintainTask()
            {
                No = _commonController.GetNo<MaintainTask>("保养任务编号"),
                RelatedNo = simulation.Demand.No,
                FixtureAccountId = DicUnloadVM.Key.Id,
                MaintainType = MaintainType.ToStorage,
                Qty = DicUnloadVM.Value.Sum(p => p.UnloadQty),
                State = MaintainState.Wait,
                ApplyDate = RF.Find<MaintainTask>().GetDbTime(),
            };

            CreateMaintainTaskDetail(maintainPrjsOfEncode, maintainTask);
            RF.Save(maintainTask);

            DicUnloadVM.Value.ForEach(p => p.MaintainTaskId = maintainTask.Id);
        }

        /// <summary>
        /// 创建保养任务的保养执行详情
        /// </summary>
        /// <param name="maintainPrjsOfEncode">某工治具编码下的出库保养任务列表</param>
        /// <param name="maintainTask">保养任务</param>
        public virtual void CreateMaintainTaskDetail(List<FixtureEncodeMaintainProject> maintainPrjsOfEncode, MaintainTask maintainTask)
        {
            foreach (var maintainPrjOfEncode in maintainPrjsOfEncode)
            {
                var detail = new MaintainTaskDetail()
                {
                    MaintainProjectId = maintainPrjOfEncode.MaintainProjectId,
                    MinValue = maintainPrjOfEncode.MinValue,
                    MaxValue = maintainPrjOfEncode.MaxValue
                };

                maintainTask.Details.Add(detail);
            }
        }

        /// <summary>
        /// 创建出库明细和治具出入库记录
        /// </summary>
        /// <param name="newUnloadVMs">新增出库明细ViewModel列表</param>
        /// <param name="simulation">需求仿真器</param>
        private void CreateFixtureUnload(IEnumerable<FixtureUnloadViewModel> newUnloadVMs, DemandSimulation simulation)
        {
            var toolCodes = newUnloadVMs.Where(p => p.TurnoverToolCode != string.Empty).Select(p => p.TurnoverToolCode).Distinct().ToList();
            var actToolCodes = RT.Service.Resolve<KitTurnoverToolController>().GetTurnoverToolCodes(toolCodes);
            foreach (var fixUnloadVM in newUnloadVMs)
            {
                CreateSaveFixtureUnload(simulation.Demand, fixUnloadVM, actToolCodes);
            }
        }

        /// <summary>
        /// 更新工治具需求清单
        /// </summary>
        /// <param name="simulation">需求仿真器</param>
        private void UpdateDemand(DemandSimulation simulation)
        {
            var finishCount = 0;
            foreach (var demandDetail in simulation.DemandDetails)
            {
                if (simulation.DicUnloadQtys.ContainsKey(demandDetail.FixtureEncodeId))
                {
                    demandDetail.UnloadQty += simulation.DicUnloadQtys[demandDetail.FixtureEncodeId];
                    demandDetail.PersistenceStatus = PersistenceStatus.Modified;
                }

                if (demandDetail.DemandQty == demandDetail.UnloadQty)
                    finishCount++;
            }

            if (simulation.DemandDetails.Count() == finishCount)
                simulation.Demand.DemandState = DemandState.Finish;
            else if (simulation.DemandDetails.Any(p => p.UnloadQty > 0))
                simulation.Demand.DemandState = DemandState.Part;
            else
                simulation.Demand.DemandState = DemandState.None;

            RF.Save(simulation.DemandDetails);
            RF.Save(simulation.Demand);
        }

        /// <summary>
        /// 创建编码类台账的使用履历
        /// </summary>
        /// <param name="simulation">需求仿真器</param>
        private void CreateUseResum(DemandSimulation simulation)
        {
            foreach (var account in simulation.Accounts)
            {
                if (account.ManageMode == ManageMode.Code)
                {
                    var qty = 0;
                    if (simulation.DicUnloadQtys.ContainsKey(account.FixtureEncodeId))
                    {
                        qty = simulation.DicUnloadQtys[account.FixtureEncodeId];
                    }

                    RT.Service.Resolve<CoreFixtureController>().CreateSaveAccountUseResume(account.Id, simulation.Demand?.ResourceId, simulation.Demand?.WorkOrderId, UseResumeType.Unload, qty);
                }
            }
        }

        /// <summary>
        /// 创建且保存治具出入库记录
        /// </summary>
        /// <param name="demand">工治具需求清单</param>
        /// <param name="dicUnloadVM">某Id编码、仓库和库存相同的出库明细ViewModel列表</param>
        private void CreateSaveFixtureRecord(FixtureDemand demand, KeyValuePair<DicUnloadKey, List<FixtureUnloadViewModel>> dicUnloadVM)
        {
            var record = new FixtureRecord()
            {
                RecordType = RecordType.Out,
                BusinessType = BusinessType.Demand,
                Code = demand.No,
                FixtureAccountId = dicUnloadVM.Key.AccountId,
                FixtureWarehouseId = dicUnloadVM.Key.WarehouseId,
                FixtureStorageLocationId = dicUnloadVM.Key.LocationId,
                Qty = dicUnloadVM.Value.Sum(p => p.UnloadQty),
                ApplyById = demand.CreateBy,
                ApplyDate = demand.CreateDate,
                ComplyById = RT.IdentityId,
                ComplyDate = RF.Find<FixtureRecord>().GetDbTime()
            };

            RF.Save(record);
        }

        /// <summary>
        /// 创建且保存出库明细
        /// </summary>
        /// <param name="demand">工治具需求清单</param>
        /// <param name="fixUnloadVM">出库明细ViewModel</param>
        /// <param name="actToolCodes">载具编码列表</param>
        private void CreateSaveFixtureUnload(FixtureDemand demand, FixtureUnloadViewModel fixUnloadVM, List<string> actToolCodes)
        {
            if (fixUnloadVM.TurnoverToolCode.IsNotEmpty() && !actToolCodes.Contains(fixUnloadVM.TurnoverToolCode))
                throw new ValidationException("载具不存在，请重新填写！".L10nFormat(fixUnloadVM.TurnoverToolCode));
            var fixUnload = new FixtureUnload()
            {
                FixtureDemandId = demand.Id,
                FixtureAccountId = fixUnloadVM.FixtureAccountId,
                WarehouseId = fixUnloadVM.WarehouseId,
                LocationId = fixUnloadVM.LocationId,
                TurnoverToolCode = fixUnloadVM.TurnoverToolCode,
                UnloadQty = fixUnloadVM.UnloadQty,
                NgQty = 0,
                UnloadById = fixUnloadVM.UnloadById,
                UnloadDate = fixUnloadVM.UnloadDate,
                State = ReceiveState.None,
                MaintainTaskId = fixUnloadVM.MaintainTaskId,
                PersistenceStatus = PersistenceStatus.New
            };

            RF.Save(fixUnload);
        }

        /// <summary>
        /// 根据产品Id获取工治具编码列表
        /// </summary>
        /// <param name="woId">产品Id</param>
        /// <param name="fixtureType">工治具类型</param>
        /// <param name="modelId">工治具型号Id</param>
        /// <param name="processStegmentId">工段</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="pageInfo">分页</param>
        /// <returns>工治具编码列表</returns>
        public virtual EntityList<FixtureEncode> GetFixtureEncodeList(double woId, double? fixtureType, double? modelId, double? processStegmentId,
            string keyWord, PagingInfo pageInfo = null)
        {
            var wo = RF.GetById<WorkOrder>(woId);
            var deckId = RT.Service.Resolve<IProcessSurface>().GetProcessSurface(woId);
            return GetFixtureEncodeList(fixtureType, processStegmentId, modelId, wo.ProductId, deckId, keyWord, pageInfo);

        }

        /// <summary>
        /// 获取可选的工段
        /// </summary>
        /// <param name="fixtureEncodeId"></param>
        /// <param name="parentProcessSegmentId"></param>
        /// <param name="deck"></param>
        /// <param name="c"></param>
        /// <param name="r"></param>
        /// <returns></returns>

        public virtual EntityList<ProcessSegment> GetProcessSegment(double fixtureEncodeId, double? parentProcessSegmentId, Deck? deck, PagingInfo c, string r)
        {
            return Query<ProcessSegment>().Join<FixtureEncodeProductDetail>((x, y) => x.Id == y.ProcessSegmentId && y.FixtureEncodeId == fixtureEncodeId)
                .WhereIf<FixtureEncodeProductDetail>(deck.HasValue, (x, u) => u.Deck == deck)
                .WhereIf<FixtureEncodeProductDetail>(r.IsNotEmpty(), (x, u) => x.Code.Contains(r) || x.Name.Contains(r))
                .WhereIf(parentProcessSegmentId.HasValue, m => m.Id == parentProcessSegmentId)
                .Distinct()
                .ToList(c, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工艺面信息
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <param name="encodeId">工治具编码Id</param>
        /// <returns>工艺面信息</returns>
        public virtual DeckInfo GetDeck(double woId, double encodeId)
        {
            var deckInfo = new DeckInfo
            {
                ErrMsg = string.Empty
            };
            try
            {
                var wo = RF.GetById<WorkOrder>(woId);
                var deckId = RT.Service.Resolve<IProcessSurface>().GetProcessSurface(wo.Id);
                var encode = RT.Service.Resolve<CoreFixtureController>().GetFixtureEncodesByIds(new List<double>() { encodeId }).FirstOrDefault();

                deckInfo.FixtureTypeId = encode.FixtureTypeId;
                deckInfo.FixtureTypeCode = encode.FixtureTypeCode;
                deckInfo.FixtureModelId = encode.FixtureModelId;
                deckInfo.ModelCode = encode.ModelCode;
                deckInfo.ModelName = encode.ModelName;
                deckInfo.Deck = deckId;
            }
            catch (Exception ex)
            {
                deckInfo.ErrMsg = ex.Message;
            }

            return deckInfo;
        }

        /// <summary>
        /// 获取单号
        /// </summary>
        /// <returns></returns>
        public virtual string GetFixtureNo<T>()
        {
            var config = ConfigService.GetConfig(new NoConfig(), typeof(T));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到单号,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(Convert.ToDouble(config.NumberRuleId), 1).FirstOrDefault();
        }

        /// <summary>
        /// 根据工单号查询工单列表
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="resouceId">产线Id</param>
        /// <param name="keyword">关键字</param>
        /// <returns>工单列表</returns>
        public virtual EntityList<WorkOrder> GetWorkOrders(PagingInfo pagingInfo, double resouceId, string keyword)
        {
            var query = Query<WorkOrder>().Where(p => (p.State == WorkOrderState.Producing || p.State == WorkOrderState.Release));
            if (keyword.IsNotEmpty())
                query.Where(p => p.No.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        #region 自动创建工单工治具需求单

        /// <summary>
        /// 调度自动创建工单工治具需求
        /// </summary>
        public virtual void SyncSchedulingAutoCreateFixtureDemand()
        {
            var config = ConfigService.GetConfig(new GenerationDemandsConfig(), typeof(FixtureDemand));
            if (config == null)
            {
                throw new ValidationException("未配置工治具需求清单配置项,请配置".L10N());
            }
            if (!config.AutomaticGeneration)//自动生成需求单据
            {
                return;
            }
            var wos = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderList(null, null, null, "", true);//获取工单
            if (wos.Any())
            {
                AutoCreateFixtureDemand(wos, config);
            }
        }

        /// <summary>
        /// 自动生成工单工治具需求
        /// </summary>
        /// <param name="workOrders"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public virtual void AutoCreateFixtureDemand(EntityList<WorkOrder> workOrders, GenerationDemandsConfigValue config)
        {
            if (workOrders == null || config == null || !workOrders.Any())
            {
                return;
            }
            EntityList<FixtureDemand> allResult = new EntityList<FixtureDemand>();
            var leadtimescrond = (double)config.Leadtime * 3600;//转为秒
            var timeNow = RF.Find<WorkOrder>().GetDbTime();

            var workOrderQuery = RT.Service.Resolve<IWorkOrderQuery>();
            var dicProcessSegments = workOrderQuery.GetDicWoProcessSegment(workOrders.Select(m => m.Id).ToList());
            var woIds = workOrders.Select(m => m.Id).ToList();

            //获取所有已经生成过的需求单据
            var fixtureDemandCache = woIds.SplitContains(itemIds => { return Query<FixtureDemand>().Where(m => itemIds.Contains(m.WorkOrderId)).ToList(); });
            foreach (var workOrder in workOrders)
            {
                var lastTime = workOrder.PlanBeginDate.AddSeconds(0 - leadtimescrond);

                //当前工单已生成过工治具需求单
                var isExsitedFixtureDemand = fixtureDemandCache.FirstOrDefault(m => m.WorkOrderId == workOrder.Id) != null;
                if (lastTime <= timeNow && !isExsitedFixtureDemand)
                {
                    var fixtureEncodeList = Query<FixtureEncodeProductDetail>().Join<FixtureEncode>((y, x) => x.Id == y.FixtureEncodeId && y.ItemId == workOrder.ProductId).ToList();
                    if (!fixtureEncodeList.Any())
                    {
                        continue;
                    }
                    var fixtureDeteilEncodes = fixtureEncodeList.ToList();
                    var deckId = RT.Service.Resolve<IProcessSurface>().GetProcessSurface(workOrder.Id);
                    //工艺面已维护，则从根据产品筛选出来的工治具编码再次进行过滤，仅保留维护了该工艺面或者工艺面为空的工治具编码，然后执行第四步；
                    if (deckId.HasValue)
                    {
                        fixtureDeteilEncodes = fixtureEncodeList.Where(m => m.Deck == null || m.Deck == (Deck)deckId).ToList();
                    }
                    List<double> woProcessSegmentIds = dicProcessSegments.ContainsKey(workOrder.Id) ? dicProcessSegments[workOrder.Id] : new List<double>();
                    var info = workOrderQuery.GetWorkOrderResource(workOrder.Id);
                    if (!woProcessSegmentIds.Any())
                    {
                        //没有维护工段，则生成一张工治具需求清单，需求单中带出第二步中找到的工治具编码以及对应的需求数
                        var resNoSegment = CreateFixtureDemand(fixtureDeteilEncodes, workOrder, info.ResourceId, info.WorkShopId);
                        if (resNoSegment != null)
                        {//维护了工段的工单
                            allResult.Add(resNoSegment);
                        }
                    }
                    else
                    {
                        var res = CreateFixtureDemands(woProcessSegmentIds, fixtureDeteilEncodes, workOrder, info.ResourceId, info.WorkShopId);
                        if (res.Any())//维护了工段的工单
                        {
                            allResult.AddRange(res);
                        }
                    }
                }
            }
            if (allResult.Any())
            {
                RF.Save(allResult); //添加记录
            }
        }

        /// <summary>
        ///创建多个工治具需求单
        /// </summary>
        /// <param name="woProcessSegmentIds"></param>
        /// <param name="fixtureEncodeProductDetails"></param>
        /// <param name="workOrder"></param>
        /// <param name="resourceId"></param>
        /// <param name="workShopId"></param>
        /// <returns></returns>
        private List<FixtureDemand> CreateFixtureDemands(List<double> woProcessSegmentIds, List<FixtureEncodeProductDetail> fixtureEncodeProductDetails,
            WorkOrder workOrder, double resourceId, double workShopId)
        {
            List<FixtureDemand> fixtureDemands = new List<FixtureDemand>();
            var woProcessSegments = Query<ProcessSegment>().Where(m => woProcessSegmentIds.Contains(m.Id)).Distinct().ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var woProcessSegment in woProcessSegments)
            {
                var matechFixtureEncodeDels = fixtureEncodeProductDetails.FindAll(m => m.ProcessSegmentId == woProcessSegment.Id);
                if (matechFixtureEncodeDels.Any())//找到存在配置工段的
                {
                    fixtureDemands.Add(CreateFixtureDemand(matechFixtureEncodeDels, workOrder, resourceId, workShopId));
                }
            }
            var noProcessSegmentList = fixtureEncodeProductDetails.FindAll(m => m.ProcessSegment == null);
            if (noProcessSegmentList.Any())
            {
                fixtureDemands.Add(CreateFixtureDemand(noProcessSegmentList, workOrder, resourceId, workShopId));
            }
            return fixtureDemands;
        }
        /// <summary>
        /// 创建单工治具编码
        /// </summary>
        /// <param name="fixtureEncodeProductDetails"></param>
        /// <param name="workOrder"></param>
        /// <param name="resourceId"></param>
        /// <param name="workShopId"></param>
        /// <returns></returns>

        private FixtureDemand CreateFixtureDemand(List<FixtureEncodeProductDetail> fixtureEncodeProductDetails, WorkOrder workOrder, double resourceId, double workShopId)
        {
            var res = new FixtureDemand()
            {
                No = _commonController.GetNo<FixtureDemand>("工治具需求清单单号"),
                ReceiveState = ReceiveState.None,
                WorkOrderId = workOrder.Id,
                ResourceId = resourceId,
                WorkShopId = workShopId,
                DemandTime = workOrder.PlanBeginDate,
                Billsource = Enums.BillSource.Auto
            };
            res.GenerateId();
            foreach (var fixtureEncodeProductDetail in fixtureEncodeProductDetails)
            {
                res.DetailList.Add(new FixtureDemandDetail()
                {
                    DemandQty = fixtureEncodeProductDetail.DemandQuantity,
                    FixtureEncodeId = fixtureEncodeProductDetail.FixtureEncodeId,
                    FixtureModelId = fixtureEncodeProductDetail.FixtureEncode.FixtureModelId,
                    FixtureTypeId = fixtureEncodeProductDetail.FixtureEncode.FixtureModel.FixtureTypeId,
                    FixtureDemandId = res.Id,
                    ProcessSurface = fixtureEncodeProductDetail.Deck,
                    ProcessSegmentId = fixtureEncodeProductDetail.ProcessSegmentId,
                });
            }
            return res;
        }
        #endregion


        /// <summary>
        /// 提交工治具需求
        /// </summary>
        /// <param name="selectedIds"></param>
        public virtual void SubmitDemandsCommand(List<double> selectedIds)
        {
            var items = GetFixtureDemandByIds(selectedIds);
            if (items.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
                throw new ValidationException("只有状态为【待提交】或【驳回】的数据才能审核".L10N());
            items.ForEach(m => m.ApprovalStatus = ApprovalStatus.PendingReview);
            RF.Save(items);
        }

        /// <summary>
        /// 获取工单工艺面
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <returns></returns>
        public virtual Deck? GetWorkOrderProcessSurface(double woId)
        {
            var woInfo = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderResource(woId);
            if (woInfo != null) return woInfo.ProcessSurface;
            return null;

        }

        /// <summary>
        ///需求单审核
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="value"></param>
        /// <param name="remark"></param>

        public virtual void ExamineDemands(List<double> selectedIds, ApprovalResult value, string remark)
        {
            var demands = GetFixtureDemandByIds(selectedIds);
            if (demands.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            var workFlowRecords = new EntityList<WorkFlowRecord>();
            demands.ForEach(item =>
            {
                item.ApprovalStatus = value == ApprovalResult.Pass ? ApprovalStatus.Audited : ApprovalStatus.Reject;
                workFlowRecords.Add(new WorkFlowRecord()
                {
                    ApprovalDatetime = DateTime.Now,
                    ApprovalResult = value,
                    ApproverId = RT.IdentityId,
                    SourceId = item.Id,
                    Remark = remark,
                    SourceType = typeof(FixtureDemand).FullName
                });
            });
            using (var trans = DB.TransactionScope(KitFixturesEntityDataProvider.ConnectionStringName))
            {
                RF.Save(demands);
                RF.Save(workFlowRecords);
                trans.Complete();
            }
        }
        /// <summary>
        /// 强制关闭
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="remark"></param>
        public virtual void ForcedShutdown(List<double> selectedIds, string remark)
        {
            var demands = GetFixtureDemandByIds(selectedIds);
            if (demands.Any(p => (p.DemandState != DemandState.None && (int)p.DemandState != 0)))
                throw new ValidationException("部分出库或已出库需求清单不允许关闭".L10N());
            demands.ForEach(item =>
            {
                item.Close = true;
                item.CloseRemark = remark;
            });
            RF.Save(demands);
        }

        /// <summary>
        ///根据ID获取工治具需求单
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>

        public virtual EntityList<FixtureDemand> GetFixtureDemandByIds(List<double> Ids)
        {
            return Ids.SplitContains(ids => Query<FixtureDemand>().Where(p => ids.Contains(p.Id)).ToList());
        }

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <returns></returns>
        public virtual FixtureDemandsConfigValue GetFixtureDemandsConfigValue()
        {
            var config = ConfigService.GetConfig(new FixtureDemandsConfig(), typeof(FixtureDemand));
            if (config == null)
            {
                throw new ValidationException("未配置工治具需求清单配置项,请配置".L10N());
            }
            return config;
        }

        /// <summary>
        /// 获取绑定工单的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public virtual BindWoInfo GetBindWoInfo(double id)
        {
            var res = new BindWoInfo();
            var wo = Query<WorkOrder>().Where(m => m.Id == id).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (wo != null)
            {
                res.PlanDateTime = wo.PlanBeginDate;
                res.ProductId = wo.ProductId;
                res.ProductCode = wo.WorkOrderProductCode;
                var workOrderInfo = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderResource(id);
                if (workOrderInfo != null)
                {
                    if (workOrderInfo.ProcessSegmentId != 0)
                        res.ProcessSegmentId = workOrderInfo.ProcessSegmentId;
                    if (res.ProcessSegmentId.HasValue)
                        res.ProcessSegment_Display = RF.GetById<ProcessSegment>(res.ProcessSegmentId).Name;
                    res.Desk = workOrderInfo.ProcessSurface;
                }
            }
            return res;
        }


        /// <summary>
        /// 获取工单所有可选工段
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual EntityList<ProcessSegment> GetWoProcessSegment(double workOrderId, PagingInfo pagingInfo, string key)
        {
            var res = RT.Service.Resolve<IWorkOrderQuery>().GetDicWoProcessSegment(new List<double>() { workOrderId });
            if (res.Keys.Count > 0)
            {
                var segmentIds = res[workOrderId];
                return Query<ProcessSegment>().WhereIf(!key.IsNullOrEmpty(), p => p.Code.Contains(key) || p.Name.Contains(key))
                    .Where(m => segmentIds.Contains(m.Id)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
            return new EntityList<ProcessSegment>();
        }
    }
}
