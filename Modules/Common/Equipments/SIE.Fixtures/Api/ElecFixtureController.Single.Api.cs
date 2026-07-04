using SIE.Api;
using SIE.Core.ApiModels;
using SIE.Core.Enums;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.EMS.Fixtures;
using SIE.EventMessages.MES.Models;
using SIE.EventMessages.MES.WorkOrders;
using SIE.Fixtures.ApiModels;
using SIE.Fixtures.FixtureDemands;
using SIE.Fixtures.FixtureRecords;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.FixtureTypes;
using SIE.Fixtures.InboundOrders;
using SIE.Fixtures.MaintainTasks;
using SIE.Fixtures.Models;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Fixtures
{
    /// <summary>
    /// 单工治具下架
    /// </summary>
    public partial class ElecFixtureController : CoreFixtureController
    {
        #region 单工治具出库接口 
        /// <summary>
        /// 获取工治具类型信息列表
        /// </summary>      
        /// <returns>工治具类型信息列表</returns>
        [ApiService("获取工治具类型信息列表")]
        [return: ApiReturn("工治具类型信息列表。返回值类型：List<FixtureTypeInfo>")]
        public virtual List<FixtureTypeInfo> GetFixtureTypeInfos()
        {
            List<FixtureTypeInfo> infos = new List<FixtureTypeInfo>();
            var fixtureTypes = RF.GetAll<FixtureType>();
            foreach (var fixtureType in fixtureTypes)
            {
                infos.Add(new FixtureTypeInfo { FixtureTypeId = fixtureType.Id, FixtureTypeName = fixtureType.Name });
            }

            return infos;
        }

        /// <summary>
        /// 获取工治具仓库列表
        /// </summary>
        /// <param name="queryInfo">工治具仓库查询信息</param>
        /// <returns>分页工治具仓库信息</returns>
        [ApiService("获取工治具仓库列表")]
        [return: ApiReturn("分页工治具仓库信息 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetPagingSingleWarehouseInfos([ApiParameter("单工治具仓库查询信息")] PagingKeywordQueryInfo queryInfo)
        {
            if (queryInfo == null)
            {
                throw new ValidationException("输入参数异常！".L10N());
            }
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };

            var warehouses = GetFixtureWarehouses(queryInfo.Keyword, pagingInfo);
            PagingBaseDataInfo result = new PagingBaseDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = warehouses.TotalCount
            };

            warehouses.ForEach(w =>
            {
                result.DataInfos.Add(new BaseDataInfo()
                {
                    Id = w.Id,
                    Code = w.Code,
                    Name = w.Name,
                });
            });

            return result;
        }

        /// <summary>
        /// 获取推荐工治具仓库列表
        /// </summary>
        /// <param name="fixtureTypeId">工治具类型Id</param>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="warehouseId">工治具仓库Id</param>
        /// <returns>分页工治具仓库信息</returns>
        [ApiService("获取推荐工治具仓库列表")]
        [return: ApiReturn("分页推荐工治具仓库信息 SingleActWarehouseInfo")]
        public virtual List<SingleActWarehouseInfo> GetSingleActWarehouseInfos([ApiParameter("工治具类型")] double fixtureTypeId, [ApiParameter("工单Id")] double workOrderId, [ApiParameter("工治具仓库Id")] double? warehouseId)
        {
            var wo = RF.GetById<WorkOrder>(workOrderId);
            if (wo == null)
            { throw new ValidationException("输入的工单不存在！".L10N()); }
            var deckId = RT.Service.Resolve<IProcessSurface>().GetProcessSurface(wo.Id);
            var accountStocks = GetFixtureAccountStocksByFixtureDemand(fixtureTypeId, wo.ProductId, deckId, warehouseId);
            var locationIds = accountStocks.Select(p => p.FixtureStorageLocationId).Distinct().ToList();
            var locations = GetFixtureStorageLocations(locationIds);
            var dicLocations = locations.ToDictionary(p => p.Id);

            var actWarehouseInfos = GetSingleWarehouseInfos(accountStocks, dicLocations);
            return actWarehouseInfos;
        }

        /// <summary>
        /// 获取实际工治具仓库数据信息列表
        /// </summary>
        /// <param name="accountStocks">库存台帐列表</param>
        /// <param name="dicLocations">工治具库位字典</param>
        /// <returns>实际工治具仓库数据信息列表</returns>
        private List<SingleActWarehouseInfo> GetSingleWarehouseInfos(EntityList<FixtureAccountStock> accountStocks, Dictionary<double, StorageLocation> dicLocations)
        {
            var actWarehouseInfos = new List<SingleActWarehouseInfo>();
            var dicAccountStocks = accountStocks.GroupBy(p => new { p.FixtureStorageLocationId, p.EncodeCode }).ToDictionary(p => p.Key, p => p.ToList());

            foreach (var dicAccountStock in dicAccountStocks)
            {
                if (dicLocations.TryGetValue(dicAccountStock.Key.FixtureStorageLocationId.Value, out StorageLocation location))
                {
                    var actWarehouseInfo = new SingleActWarehouseInfo()
                    {
                        EncodeCode = dicAccountStock.Key.EncodeCode,
                        WarehouseId = location.WarehouseId,
                        WarehouseCode = location.WarehouseCode,
                        WarehouseName = location.WarehouseName,
                        LocationId = location.Id,
                        LocationCode = location.Code,
                        LocationName = location.Name,
                        Location = codeNameFormant.L10nFormat(location.Name, dicAccountStock.Value.Sum(p => p.PassQty))
                    };

                    actWarehouseInfos.Add(actWarehouseInfo);
                }
            }

            return actWarehouseInfos;
        }

        /// <summary>
        /// 验证工治具出库工单
        /// </summary>
        /// <param name="no">工单编码</param>
        /// <returns>单工治具出库工单信息</returns>
        [ApiService("验证工治具出库工单")]
        [return: ApiReturn("验证工治具出库工单 ValidateUnloadWorkOrder")]
        public virtual UnloadWorkOrderInfo ValidateUnloadWorkOrder([ApiParameter("工单")] string no)
        {
            var unloadInfo = new UnloadWorkOrderInfo();

            if (!no.IsNotEmpty())
            {
                throw new ValidationException("输入/扫描的工单不能为空！".L10N());
            }

            var wo = RT.Service.Resolve<WorkOrderController>().GetWorkOrder(no);
            if (wo == null)
            {
                throw new ValidationException(workOrderNotExists.L10N());
            }

            if (!(wo.State == WorkOrderState.Release || wo.State == WorkOrderState.Producing))
            {
                throw new ValidationException("工单状态不为发放或生产中！".L10N());
            }

            unloadInfo.WorkOrderId = wo.Id;
            return unloadInfo;
        }

        /// <summary>
        /// 获取工单信息列表
        /// </summary>
        /// <param name="queryInfo">工单查询信息</param>
        /// <returns>工单信息列表</returns>
        [ApiService("获取工单信息列表")]
        [return: ApiReturn("获取工单信息列表 GetPagingSingleWorkOrderInfos")]
        public virtual SingleWorkOrderDataInfo GetPagingSingleWorkOrderInfos([ApiParameter("工单查询信息")] SingleWorkOrderQueryInfo queryInfo)
        {
            if (queryInfo == null)
            {
                throw new ValidationException("输入参数异常！".L10N());
            }
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };

            var wos = GetWorkOrders(pagingInfo, queryInfo.ResourceId, queryInfo.Keyword);

            SingleWorkOrderDataInfo result = new SingleWorkOrderDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = wos.TotalCount
            };

            wos.ForEach(wo =>
            {
                var workOrderInfo = new SingleWorkOrderInfo()
                {
                    WorkOrderId = wo.Id,
                    WorkOrderNo = wo.No,
                };

                result.WorkOrderInfos.Add(workOrderInfo);
            });

            return result;
        }

        /// <summary>
        /// 获取工单列表
        /// </summary>
        /// <param name="queryInfo">工单查询信息</param>
        /// <returns>分页工单信息</returns>
        [ApiService("获取工单列表")]
        [return: ApiReturn("分页工单信息 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetPagingWorkOrdertInfos([ApiParameter("工单查询信息")] WorkOrderQueryInfo queryInfo)
        {
            if (queryInfo == null)
            { throw new ValidationException("前端传参有误".L10N()); }
            return RT.Service.Resolve<IWorkOrderQuery>().GetPagingWorkOrdertInfos(queryInfo);
        }


        /// <summary>
        /// 获取单工治具出库库位列表
        /// </summary>
        /// <param name="queryInfo">单工治具出库库位查询信息</param>
        /// <returns>单工治具出库库位列表</returns>
        [ApiService("获取单工治具出库库位列表")]
        [return: ApiReturn("获取单工治具出库库位列表 GetPagingSingleLocationInfos")]
        public virtual SingleLocationDataInfo GetPagingSingleLocationInfos1([ApiParameter("工单查询信息")] SingleLocationQueryInfo queryInfo)
        {
            if (queryInfo == null)
            { throw new ValidationException("前端传参有误".L10N()); }
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };

            if (!queryInfo.Code.IsNotEmpty())
            { throw new ValidationException(fixtureCodeIsNullOrEmpty.L10N()); }
            var account = GetFixtureAccountByCodeOrRFID(queryInfo.Code);
            if (account == null)
            { throw new ValidationException(fixtureAccountNotExists.L10N()); }
            if (queryInfo.WorkOrderId <= 0)
            { throw new ValidationException("工单未选择，请先选择工单！".L10N()); }
            var wo = RF.GetById<WorkOrder>(queryInfo.WorkOrderId);
            if (wo == null)
            { throw new ValidationException(workOrderNotExists.L10N()); }
            var deckId = RT.Service.Resolve<IProcessSurface>().GetProcessSurface(wo.Id);
            var encodeList = GetFixtureEncodeList(null, null, null, wo.ProductId, deckId, string.Empty);
            if (account.BindProduct == YesNo.Yes)
            {
                if (!encodeList.Any(p => p.Id == account.FixtureEncodeId))
                {
                    throw new ValidationException("输入/扫描的绑定产品工治具ID[{0}]的工治具编码下的产品清单中不包含工单[{1}]的产品和工艺面！".L10nFormat(queryInfo.Code, wo.No));
                }
            }
            else
            {
                if (!encodeList.Any(p => p.Id == account.FixtureEncodeId))
                {
                    throw new ValidationException("输入/扫描的非绑定产品工治具ID[{0}]的工治具编码不存在！".L10nFormat(queryInfo.Code));
                }
            }

            var result = GetSingleLocationInfo(queryInfo, pagingInfo, account);

            return result;
        }

        /// <summary>
        /// 获取库位信息列表
        /// </summary>
        /// <param name="queryInfo">查询信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="account">工治具台帐</param>
        /// <returns>库位信息列表</returns>
        private SingleLocationDataInfo GetSingleLocationInfo(SingleLocationQueryInfo queryInfo, PagingInfo pagingInfo, FixtureAccount account)
        {
            SingleLocationDataInfo result = new SingleLocationDataInfo();
            var stocks = GetFixtureAccountStocks(account.Id, queryInfo.LocationCode, pagingInfo);
            stocks.ForEach(stock =>
            {
                var locationInfo = new SingleLocationInfo()
                {
                    LocationId = stock.FixtureStorageLocationId.Value,
                    Location = codeNameFormant.L10nFormat(stock.LocationCode, stock.LocationName),
                    WarehouseId = stock.FixtureWarehouseId,
                    Warehouse = codeNameFormant.L10nFormat(stock.WarehouseCode, stock.WarehouseName),
                    Qty = stock.PassQty,
                };

                result.LocationInfos.Add(locationInfo);
            });
            return result;
        }

        /// <summary>
        /// 提交单工治具出库
        /// </summary>
        /// <param name="singleUnloadInfo">单工治具出库信息</param>
        [ApiService("提交单工治具出库")]
        [return: ApiReturn("提交单工治具出库 SubmitSingleUnloadInfo")]
        public virtual void SubmitSingleUnloadInfo([ApiParameter("单工治具出库信息")] SingleUnloadInfo singleUnloadInfo)
        {
            if (singleUnloadInfo == null)
            {
                throw new ValidationException("前端参数有误！".L10N());
            }
            if (!singleUnloadInfo.Code.IsNotEmpty())
            {
                throw new ValidationException(fixtureCodeIsNullOrEmpty.L10N());
            }
            var account = GetFixtureAccountByCodeOrRFID(singleUnloadInfo.Code);
            if (account == null)
            { 
                throw new ValidationException(fixtureAccountNotExists.L10N()); 
            }
            var wo = RF.GetById<WorkOrder>(singleUnloadInfo.WorkOrderId);
            if (wo == null)
            { 
                throw new ValidationException(workOrderNotExists.L10N()); 
            }
            var deckId = RT.Service.Resolve<IProcessSurface>().GetProcessSurface(wo.Id);
            var encodeList = GetFixtureEncodeList(null, null, null, wo.ProductId, deckId, string.Empty);
            if (account.BindProduct == YesNo.Yes)
            {
                if (!encodeList.Any(p => p.Id == account.FixtureEncodeId))
                {
                    throw new ValidationException("输入/扫描的绑定产品工治具ID[{0}]的工治具编码下的产品清单中不包含工单[{1}]的产品和工艺面！".L10nFormat(singleUnloadInfo.Code, wo.No));
                }
            }
            else
            {
                if (!encodeList.Any(p => p.Id == account.FixtureEncodeId))
                {
                    throw new ValidationException("输入/扫描的非绑定产品工治具ID[{0}]的工治具编码不存在！".L10nFormat(singleUnloadInfo.Code));
                }
            }

            var location = GetStorageLocation(singleUnloadInfo.LocationId);
            if (location == null)
            { 
                throw new ValidationException("工治具仓库没有维护此库位！".L10N());
            }
            if (location.WarehouseId != singleUnloadInfo.WarehouseId)
            { 
                throw new ValidationException("仓库和库位关系异常，请先确认！".L10N());
            }
            var stock = GetPassStock(account.Id, location.Id);
            if (stock == null)
            { 
                throw new ValidationException("工治具台帐[{0}]的库位[{1}]不存在出库的合格库存台帐，请先入库！".L10nFormat(singleUnloadInfo.Code, location.Code));
            }
            if (stock.PassQty < singleUnloadInfo.Qty)
            { 
                throw new ValidationException("工治具台帐[{0}]的库位[{1}]出库的数量不能大于库存的合格数！".L10nFormat(singleUnloadInfo.Code, location.Code)); 
            }

            SaveSingleUnloadInfo(singleUnloadInfo, account, wo, deckId, location, stock);
        }

        /// <summary>
        /// 保存单工治具出库信息
        /// </summary>
        /// <param name="singleUnloadInfo">单工治具出库信息</param>
        /// <param name="account">工治具台帐</param>
        /// <param name="wo">工单</param>
        /// <param name="deckId">工艺面</param>
        /// <param name="location">库位</param>
        /// <param name="stock">库存台帐</param>
        private void SaveSingleUnloadInfo(SingleUnloadInfo singleUnloadInfo, FixtureAccount account, WorkOrder wo, int? deckId, StorageLocation location, FixtureAccountStock stock)
        {
            using (var tran = DB.TransactionScope(KitFixturesEntityDataProvider.ConnectionStringName))
            {
                var demand = CreateFixtureDemand(singleUnloadInfo.WorkOrderId, singleUnloadInfo.Qty, account, wo, deckId);
                var maintainPrjs = GetToStorageMaintainMaintainProjects(account.FixtureEncodeId);

                CreateMaintainTaskAndFixtureUnload(singleUnloadInfo.Qty, account, location, demand, maintainPrjs);
                CreateFixtureRecord(singleUnloadInfo.Qty, account, location, demand);
                UpdateAccountAndStock(singleUnloadInfo.Qty, account, stock, maintainPrjs);

                if (account.ManageMode == ManageMode.Code)
                {
                    CreateSaveAccountUseResume(account.Id, demand.ResourceId, demand.WorkOrderId, UseResumeType.Unload, singleUnloadInfo.Qty);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 创建保养任务和工治具需求清单出库明细
        /// </summary>
        /// <param name="unloadQty">出库数量</param>
        /// <param name="account">工治具台账</param>
        /// <param name="location">库位</param>
        /// <param name="demand">工治具需求清单</param>
        /// <param name="maintainPrjs">保养项目列表</param>
        public virtual void CreateMaintainTaskAndFixtureUnload(int unloadQty, FixtureAccount account, StorageLocation location, FixtureDemand demand, EntityList<FixtureEncodeMaintainProject> maintainPrjs)
        {
            var maintainTask = GetMaintainTask(unloadQty, account, demand, maintainPrjs);
            var unload = CreateFixtureUnload(unloadQty, account, location);
            if (maintainTask != null && maintainPrjs.Any())
                unload.MaintainTaskId = maintainTask.Id;
            demand.UnloadList.Add(unload);
            RF.Save(demand);
        }

        /// <summary>
        /// 创建工治具需求清单和明细
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <param name="unloadQty">出库数量</param>
        /// <param name="account">工治具台账</param>
        /// <param name="wo">工单</param>
        /// <param name="deckId">工艺面</param>
        /// <returns>工治具需求清单</returns>
        public virtual FixtureDemand CreateFixtureDemand(double woId, int unloadQty, FixtureAccount account, WorkOrder wo, int? deckId)
        {
            var demand = new FixtureDemand()
            {
                No = _commonController.GetNo<FixtureDemand>("工治具需求清单单号".L10N()),
                WorkOrderId = wo.Id,
                DemandState = DemandState.Finish,
                ReceiveState = ReceiveState.None,
                Billsource = Enums.BillSource.Auto,
                DemandTime = DateTime.Now
            };

            var workOrderInfo = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderResource(woId);
            demand.ResourceId = workOrderInfo.ResourceId;
            demand.WorkShopId = workOrderInfo.WorkShopId;

            var demandDetail = new FixtureDemandDetail()
            {
                FixtureEncodeId = account.FixtureEncodeId,
                FixtureModelId = account.FixtureEncode.FixtureModelId,
                FixtureTypeId = account.FixtureEncode.FixtureModel.FixtureTypeId,
                DemandQty = unloadQty,
                UnloadQty = unloadQty,

            };

            if (deckId != null)
                demandDetail.ProcessSurface = (Deck)deckId;
            demand.DetailList.Add(demandDetail);
            return demand;
        }

        /// <summary>
        /// 更新工治具台帐和库存台帐
        /// </summary>
        /// <param name="unloadQty">出库数量</param>
        /// <param name="account">工治具台帐</param>
        /// <param name="stock">库存台帐</param>
        /// <param name="maintainPrjs">工治具编码保养项目列表</param>
        public virtual void UpdateAccountAndStock(int unloadQty, FixtureAccount account, FixtureAccountStock stock, EntityList<FixtureEncodeMaintainProject> maintainPrjs)
        {
            if (maintainPrjs.Any())
            {
                UpdateAccountAndStock(true, unloadQty, account, stock);
            }
            else
            {
                UpdateAccountAndStock(false, unloadQty, account, stock);
            }
        }

        /// <summary>
        /// 更新工治具台帐和库存台帐
        /// </summary>
        /// <param name="isNeedMaintain">是否需要保养</param>
        /// <param name="unloadQty">出库数量</param>
        /// <param name="account">工治具台帐</param>
        /// <param name="stock">库存台帐</param>
        private void UpdateAccountAndStock(bool isNeedMaintain, int unloadQty, FixtureAccount account, FixtureAccountStock stock)
        {
            if (account.ManageMode == ManageMode.Number)
            {
                UpdateIDAccountAndStock(isNeedMaintain, account, stock);
            }
            else
            {
                UpdateCodeAccountAndStock(isNeedMaintain, unloadQty, account, stock);
            }
        }

        /// <summary>
        /// 更新编码类工治具台帐和库存台帐
        /// </summary>
        /// <param name="isNeedMaintain">是否需要保养</param>
        /// <param name="unloadQty">出库数量</param>
        /// <param name="account">编码类工治具台帐</param>
        /// <param name="stock">库存台帐</param>
        private void UpdateCodeAccountAndStock(bool isNeedMaintain, int unloadQty, FixtureAccount account, FixtureAccountStock stock)
        {
            account.InStockQty -= unloadQty;
            if (isNeedMaintain)
            {
                account.WaitMaintain += unloadQty;
            }
            else
            {
                account.WaitReceive += unloadQty;
            }
            RF.Save(account);

            stock.TotalQty -= unloadQty;
            stock.PassQty -= unloadQty;
            stock.PersistenceStatus = PersistenceStatus.Modified;
            if (stock.TotalQty == 0)
            {
                stock.PersistenceStatus = PersistenceStatus.Deleted;
            }
            RF.Save(stock);
        }

        /// <summary>
        /// 更新ID类工治具台帐和库存台帐
        /// </summary>
        /// <param name="isNeedMaintain">是否需要保养</param>
        /// <param name="account">ID类工治具台帐</param>
        /// <param name="stock">库存台帐</param>
        private void UpdateIDAccountAndStock(bool isNeedMaintain, FixtureAccount account, FixtureAccountStock stock)
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

            stock.PersistenceStatus = PersistenceStatus.Deleted;
            RF.Save(stock);
        }

        /// <summary>
        /// 创建工治具出入库记录
        /// </summary>
        /// <param name="unloadQty">出库数量</param>
        /// <param name="account">工治具台账</param>
        /// <param name="location">工治具库位</param>
        /// <param name="demand">工治具需求清单</param>
        public virtual void CreateFixtureRecord(int unloadQty, FixtureAccount account, StorageLocation location, FixtureDemand demand)
        {
            var record = new FixtureRecord()
            {
                RecordType = RecordType.Out,
                BusinessType = BusinessType.Demand,
                Code = demand.No,
                FixtureAccountId = account.Id,
                FixtureWarehouseId = location.WarehouseId,
                FixtureStorageLocationId = location.Id,
                Qty = unloadQty,
                ApplyById = demand.CreateBy,
                ApplyDate = demand.CreateDate,
                ComplyById = RT.IdentityId,
                ComplyDate = RF.Find<FixtureRecord>().GetDbTime()
            };
            RF.Save(record);
        }

        /// <summary>
        /// 创建出库明细
        /// </summary>
        /// <param name="unloadQty">出库数量</param>
        /// <param name="account">工治具台账</param>
        /// <param name="location">工治具库位</param>
        /// <returns>出库明细</returns>
        private FixtureUnload CreateFixtureUnload(int unloadQty, FixtureAccount account, StorageLocation location)
        {
            return new FixtureUnload()
            {
                FixtureAccountId = account.Id,
                WarehouseId = location.WarehouseId,
                LocationId = location.Id,
                UnloadQty = unloadQty,
                NgQty = 0,
                UnloadById = RT.IdentityId,
                UnloadDate = RF.Find<FixtureUnload>().GetDbTime(),
                State = ReceiveState.None,
            };
        }

        /// <summary>
        /// 获取保养任务
        /// </summary>
        /// <param name="unloadQty">出库数量</param>
        /// <param name="account">工治具台账</param>
        /// <param name="demand">工治具需求清单</param>
        /// <param name="maintainPrjs">出库保养明细列表</param>
        /// <returns>保养任务</returns>
        private MaintainTask GetMaintainTask(int unloadQty, FixtureAccount account, FixtureDemand demand, EntityList<FixtureEncodeMaintainProject> maintainPrjs)
        {
            MaintainTask maintainTask = null;
            if (maintainPrjs.Any())
            {
                maintainTask = CreateMaintainTask(unloadQty, account, demand);
                this.CreateMaintainTaskDetail(maintainPrjs.ToList(), maintainTask);
                RF.Save(maintainTask);
            }

            return maintainTask;
        }

        /// <summary>
        /// 创建保养任务
        /// </summary>
        /// <param name="unloadQty">出库数量</param>
        /// <param name="account">工治具台账</param>
        /// <param name="demand">工治具需求清单</param>
        /// <returns>保养任务</returns>
        private MaintainTask CreateMaintainTask(int unloadQty, FixtureAccount account, FixtureDemand demand)
        {
            return new MaintainTask()
            {
                No = _commonController.GetNo<MaintainTask>("保养任务编号"),
                RelatedNo = demand.No,
                FixtureAccountId = account.Id,
                MaintainType = MaintainType.ToStorage,
                Qty = unloadQty,
                State = MaintainState.Wait,
                ApplyDate = RF.Find<MaintainTask>().GetDbTime(),
            };
        }

        /// <summary>
        /// 创建保存保养任务和保养执行详情
        /// </summary>
        /// <param name="task">上架任务</param>
        /// <param name="account">工装治具台账</param>
        /// <param name="maintainPrjs">工装治具编码保养项目列表</param>
        private void CreateMaintainTask(InboundOrder task, FixtureAccount account, EntityList<FixtureEncodeMaintainProject> maintainPrjs)
        {
            var qty = 1;
            if (task.FixtureEncode.FixtureModel.ManageMode == ManageMode.Code && task.InboundOrderFixtureCodeAccountList.Any())
            {
                qty = (int)task.InboundOrderFixtureCodeAccountList.Sum(m => m.Qty);
            }
            if (task.FixtureEncode.FixtureModel.ManageMode == ManageMode.Number && task.InboundOrderFixtureIdAccountList.Any())
            {
                qty = (int)task.InboundOrderFixtureIdAccountList.Sum(m => m.Qty);
            }
            var maintainTask = new MaintainTask()
            {
                No = _commonController.GetNo<MaintainTask>("保养任务编号"),
                RelatedNo = task.No,
                MaintainType = MaintainType.InStorage,
                FixtureAccountId = account.Id,
                State = MaintainState.Wait,
                ApplyDate = RF.Find<MaintainTask>().GetDbTime(),
                Qty = qty
            };
            maintainTask.GenerateId();
            task.MaintainTaskId = maintainTask.Id;
            foreach (var maintainPrj in maintainPrjs)
            {
                var detail = new MaintainTaskDetail()
                {
                    MaintainProjectId = maintainPrj.MaintainProjectId,
                    MinValue = maintainPrj.MinValue,
                    MaxValue = maintainPrj.MaxValue
                };

                maintainTask.Details.Add(detail);
            }

            RF.Save(maintainTask);
        }
        #endregion
    }
}
