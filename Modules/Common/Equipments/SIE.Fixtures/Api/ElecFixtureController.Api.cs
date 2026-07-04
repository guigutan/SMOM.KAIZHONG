using SIE.Api;
using SIE.Core.ApiModels;
using SIE.Core.Common.Controllers;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.EMS.Fixtures;
using SIE.Fixtures.ApiModels;
using SIE.Fixtures.FixtureDemands;
using SIE.Fixtures.FixtureRecords;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.FixtureTypes;
using SIE.Fixtures.MaintainTasks;
using SIE.Fixtures.Models;
using SIE.TurnoverTools.TurnoverTools;
using SIE.Utils;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Fixtures
{
    /// <summary>
    /// API控制器
    /// </summary>
    public partial class ElecFixtureController : CoreFixtureController
    {
        private const string codeNameFormant = "{0}({1})";
        private const string pleaseSelectWarehouse = "请先选择仓库！";
        private const string workOrderNotExists = "输入/扫描的工单不存在！";
        private const string fixtureCodeIsNullOrEmpty = "输入/扫描的工治具ID不能为空！";
        private const string fixtureAccountNotExists = "输入/扫描的工治具ID不存在！";

        /// <summary>
        /// 通用控制器
        /// </summary>
        private static CommonController _commonController = RT.Service.Resolve<CommonController>();

        #region 工治具治具出库接口

        /// <summary>
        /// 获取工治具治具需求清单列表
        /// </summary>
        /// <param name="queryInfo">工治具治具需求清单查询信息</param>
        /// <returns>工治具治具需求清单信息</returns>
        [ApiService("获取工治具治具需求清单列表")]
        [return: ApiReturn("工治具治具需求清单信息  FixtureDemandDataInfo")]
        public virtual FixtureDemandDataInfo GetPagingFixtureDemandInfos([ApiParameter("工治具治具需求清单查询信息")] FixtureDemandQueryInfo queryInfo)
        {
            var pagingInfo = GetPagingInfo(queryInfo.PageNumber, int.MaxValue);
            var fixtureDemands = GetUnloadFixtureDemands(pagingInfo, queryInfo.No);
            var fixtureDemandIds = fixtureDemands.Select(p => p.Id).ToList();
            var details = GetDetailsByDemandIds(fixtureDemandIds);
            var fixtureDemandDataInfo = new FixtureDemandDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = fixtureDemands.Count
            };
            foreach (var fixtureDemand in fixtureDemands)
            {
                var qty = 0;
                var typeNums = new List<FixtureType>();
                var demandDetails = details.Where(p => p.FixtureDemandId == fixtureDemand.Id).ToList();
                foreach (var detail in demandDetails)
                {
                    qty += detail.DemandQty - detail.UnloadQty;//统计治具明细未出库的数量和
                    typeNums.Add(detail.FixtureType);
                }
                var fixtureDemandInfo = new FixtureDemandInfo();
                fixtureDemandInfo.No = fixtureDemand.No;
                fixtureDemandInfo.ResourceName = fixtureDemand.ResourceName;
                fixtureDemandInfo.WorkOrderNo = fixtureDemand.WorkOrderNo;
                fixtureDemandInfo.TypeNum = typeNums.Distinct().Count();
                fixtureDemandInfo.Qty = qty;
                fixtureDemandInfo.ProcessSegmentCode = fixtureDemand.ProcessSegmentId.HasValue ? fixtureDemand.ProcessStegmentCode : "";
                fixtureDemandInfo.DemandTime = fixtureDemand.DemandTime.ToString("yyyy/MM/dd HH:MM:ss");
                fixtureDemandInfo.CreateDate = fixtureDemand.CreateDate;
                fixtureDemandDataInfo.FixtureDemandInfos.Add(fixtureDemandInfo);
            }
            return fixtureDemandDataInfo;
        }

        /// <summary>
        /// 获取【工治具治具需求清单】中【出库状态】不为【出库完成】的数据；
        /// 并且【工治具治具需求清单】对应的【保养任务】的【保养状态】为【保养完成】的治具；(去掉)
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="no"></param>
        /// <returns>工治具治具需求清单列表</returns>
        private EntityList<FixtureDemand> GetUnloadFixtureDemands(PagingInfo pagingInfo, string no)
        {
            return GetDemandsByStateAndNo(pagingInfo, no);
        }

        /// <summary>
        /// 获取当前用户存在权限的仓库
        /// </summary>
        /// <param name="pagingKeywordQueryInfo"></param>
        /// <returns></returns>
        [ApiService("获取当前用户存在权限的仓库")]
        [return: ApiReturn("分页需求明细信息  PagingKeywordQueryInfo")]
        public virtual PagingBaseDataInfo GetPagingWarehouseDataInfos(PagingKeywordQueryInfo pagingKeywordQueryInfo)
        {

            PagingBaseDataInfo warehouseDataInfo = new PagingBaseDataInfo();
            var warehouses = RT.Service.Resolve<WarehouseController>().GetUserWarehouses();
            if (warehouses.Any())
            {
                warehouses.ForEach(item =>
                {
                    warehouseDataInfo.DataInfos.Add(new BaseDataInfo
                    {

                        Code = string.Format(codeNameFormant, item.Name, item.Code),
                        Id = item.Id,
                        Name = item.Name
                    });

                });
            }
            return warehouseDataInfo;
        }

        /// <summary>
        /// 获取需求明细信息列表
        /// </summary>
        /// <param name="demandDetailQueryInfo">需求明细查询信息</param>
        /// <returns>分页需求明细信息</returns>
        [ApiService("获取需求明细信息列表")]
        [return: ApiReturn("分页需求明细信息  DemandDetailDataInfo")]
        public virtual DemandDetailDataInfo GetPagingDemandDetailInfos([ApiParameter("需求明细查询信息")] DemandDetailQueryInfo demandDetailQueryInfo)
        {
            var pagingInfo = GetPagingInfo(demandDetailQueryInfo.PageNumber, demandDetailQueryInfo.PageSize);
            if (demandDetailQueryInfo.No.IsNullOrWhiteSpace())
                throw new ValidationException("查询的需求单号不能为空！".L10N());
            var fixDemand = GetFixtureDemand(demandDetailQueryInfo.No);
            if (fixDemand == null)
                throw new ValidationException("查询的需求单号不存在！".L10N());
            if (!demandDetailQueryInfo.WareHouseId.HasValue)
                throw new ValidationException(pleaseSelectWarehouse.L10N());
            var wh = RF.GetById<Warehouse>(demandDetailQueryInfo.WareHouseId);
            if (wh == null)
                throw new ValidationException(pleaseSelectWarehouse.L10N());
            var details = GetFixtureDemandDetails(fixDemand.Id, pagingInfo);
            var demandDetailDataInfo = new DemandDetailDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = details.Count()
            };
            foreach (var detail in details)
            {
                if (detail.DemandQty <= detail.UnloadQty)
                    continue;
                //获取台账的库存详情
                var accounts = GetAccountsByEncodeId(detail.FixtureEncodeId);
                var accountIds = accounts.Select(p => p.Id).ToList();
                var fixtureStocks = GetFixtureAccountStocksByAccountIds(accountIds, pagingInfo);

                var demandDetailInfo = new DemandDetailInfo();
                demandDetailInfo.No = fixDemand.No;
                demandDetailInfo.EncodeCodeId = detail.FixtureEncodeId;
                demandDetailInfo.Id = detail.Id;
                demandDetailInfo.EncodeCode = detail.EncodeCode;
                demandDetailInfo.ModelName = detail.ModelName;
                demandDetailInfo.ManageMode = detail.FixtureEncode.FixtureModel.ManageMode.ToLabel();
                demandDetailInfo.DemandQty = detail.DemandQty.ToString();
                demandDetailInfo.UnloadQty = detail.UnloadQty.ToString();
                demandDetailInfo.Warehouse = wh.Code;
                demandDetailInfo.WarehouseId = wh.Id;
                if (fixtureStocks.Any())
                {
                    var wareHousesStocks = fixtureStocks.Where(m => m.Warehouse.Id == demandDetailQueryInfo.WareHouseId.Value && m.PassQty > 0).ToList();

                    var dicLocation = new Dictionary<string, int>();

                    wareHousesStocks.ForEach(fixtureStock =>//合并显示库位
                    {
                        if (dicLocation.ContainsKey(fixtureStock.LocationCode))
                        {
                            dicLocation[fixtureStock.LocationCode] += fixtureStock.PassQty;
                        }
                        else
                        {
                            dicLocation.Add(fixtureStock.LocationCode, fixtureStock.PassQty);
                        }
                    });

                    foreach (var key in dicLocation.Keys)
                    {
                        demandDetailInfo.LocationId = string.Join(",", demandDetailInfo.LocationId);
                        demandDetailInfo.Location = string.Join(",", demandDetailInfo.Location, codeNameFormant.L10nFormat(key, dicLocation[key]));
                    }

                    if (!demandDetailInfo.Location.IsNullOrEmpty())
                        demandDetailInfo.Location = demandDetailInfo.Location.TrimStart(',').TrimStart(',');
                }
                demandDetailDataInfo.DemandDetailInfos.Add(demandDetailInfo);

            }
            return demandDetailDataInfo;
        }

        /// <summary>
        /// 获取推荐位置信息列表
        /// </summary>
        /// <param name="fixtureStockQueryInfo">推荐位置查询信息</param>
        /// <returns>分页推荐位置信息</returns>
        [ApiService("获取推荐位置信息列表")]
        [return: ApiReturn("分页推荐位置信息  FixtureStockDataInfo")]
        public virtual FixtureStockDataInfo GetPagingFixtureStockInfos([ApiParameter("推荐位置查询信息")] FixtureStockQueryInfo fixtureStockQueryInfo)
        {
            var pagingInfo = GetPagingInfo(fixtureStockQueryInfo.PageNumber, fixtureStockQueryInfo.PageSize);
            var accounts = GetAccountsByEncodeId(fixtureStockQueryInfo.EncodeCodeId);
            var accountIds = accounts.Select(p => p.Id).ToList();
            var fixtureStocks = GetFixtureAccountStocksByAccountIds(accountIds, pagingInfo);//获取台账的库存详情
            var fixtureStockDataInfo = new FixtureStockDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = fixtureStocks.Count()
            };
            foreach (var fixtureStock in fixtureStocks)
            {
                if (fixtureStock.PassQty <= 0)
                    continue;
                var fixtureStockInfo = new FixtureStockInfo();
                fixtureStockInfo.Code = fixtureStock.AccountCode;
                fixtureStockInfo.WarehouseId = fixtureStock.FixtureWarehouseId;
                fixtureStockInfo.Warehouse = codeNameFormant.L10nFormat(fixtureStock.WarehouseCode, fixtureStock.WarehouseName);
                fixtureStockInfo.LocationId = fixtureStock.FixtureStorageLocationId.HasValue ? fixtureStock.FixtureStorageLocationId.Value : 0;
                fixtureStockInfo.Location = codeNameFormant.L10nFormat(fixtureStock.LocationCode, fixtureStock.LocationName);
                fixtureStockInfo.Qty = fixtureStock.PassQty;
                fixtureStockDataInfo.FixtureStockInfos.Add(fixtureStockInfo);
            }
            return fixtureStockDataInfo;
        }

        /// <summary>
        /// 获取出库明细列表
        /// </summary>
        /// <param name="unloadQueryInfo">出库查询信息</param>
        /// <returns>分页出库明细信息</returns>
        [ApiService("获取出库明细列表")]
        [return: ApiReturn("分页出库明细信息  UnloadDataInfo")]
        public virtual UnloadDataInfo GetPagingFixtureUnloadInfos([ApiParameter("出库查询信息")] UnloadQueryInfo unloadQueryInfo)
        {
            var pagingInfo = GetPagingInfo(unloadQueryInfo.PageNumber, unloadQueryInfo.PageSize);
            if (unloadQueryInfo.No.IsNullOrWhiteSpace())
                throw new ValidationException("查询的需求单号不能为空！".L10N());
            var fixDemand = GetFixtureDemand(unloadQueryInfo.No);
            if (fixDemand == null)
                throw new ValidationException("查询的需求单号不存在！".L10N());
            var fixtureUnloads = GetFixtureUnloadBydemandId(fixDemand.Id, pagingInfo);
            ////GetFixtureUnloadByEncodeCodeId(fixDemand.Id, unloadQueryInfo.EncodeCodeId, pagingInfo);
            var unloadDataInfo = new UnloadDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = fixtureUnloads.Count()
            };
            foreach (var fixtureUnload in fixtureUnloads)
            {
                var unloadInfo = new UnloadInfo();
                unloadInfo.Code = fixtureUnload.AccountCode;
                unloadInfo.Warehouse = codeNameFormant.L10nFormat(fixtureUnload.WarehouseCode, fixtureUnload.WarehouseName);
                unloadInfo.Location = codeNameFormant.L10nFormat(fixtureUnload.LocationCode, fixtureUnload.LocationName);
                unloadInfo.TurnoverToolCode = fixtureUnload.TurnoverToolCode;
                unloadInfo.UnloadQty = fixtureUnload.UnloadQty;
                unloadDataInfo.UnloadInfos.Add(unloadInfo);
            }
            return unloadDataInfo;
        }

        /// <summary>
        /// 验证工治具出库的工治具ID
        /// </summary>
        /// <param name="code">工治具ID</param>
        /// <param name="no">工单号</param>
        /// <returns>出库ID编码信息</returns>
        [ApiService("验证工治具出库ID编码")]
        [return: ApiReturn("出库ID编码信息  UnloadIDCodeInfo")]
        public virtual UnloadIDCodeInfo ValidateUnloadIDCode([ApiParameter("ID编码")] string code, [ApiParameter("工单号")] string no)
        {
            var unloadInfo = new UnloadIDCodeInfo();
            if (!no.IsNotEmpty())
                throw new ValidationException("输入/扫描的工单不能为空！".L10N());
            var wo = RT.Service.Resolve<WorkOrderController>().GetWorkOrder(no);
            if (wo == null)
            {
                throw new ValidationException(workOrderNotExists.L10N());
            }

            var deckId = RT.Service.Resolve<IProcessSurface>().GetProcessSurface(wo.Id);
            var encodeList = GetFixtureEncodeList(null, null, null, wo.ProductId, deckId, string.Empty);
            if (!code.IsNotEmpty())
                throw new ValidationException(fixtureCodeIsNullOrEmpty.L10N());
            var account = GetFixtureAccountByCodeOrRFID(code);
            if (account == null)
                throw new ValidationException(fixtureAccountNotExists.L10N());

            IsExistFixtureEncode(code, wo, encodeList, account);

            unloadInfo.ManageMode = EnumViewModel.EnumToLabel(account.ManageMode).L10N();
            unloadInfo.EncodeCode = account.EncodeCode;
            unloadInfo.ModelName = account.ModelName;
            unloadInfo.FixtureType = account.FixtureType != null ? "" : account.Code;

            ValidataStock(code, unloadInfo, account);

            return unloadInfo;
        }

        /// <summary>
        /// 验证工治具出库的工治具ID
        /// </summary>
        /// <param name="code">工治具ID</param>
        /// <param name="fixtureDemandNo"></param>
        /// <param name="Id">编码Id</param>
        /// <param name="whId"></param>
        /// <returns>出库ID编码信息</returns>
        [ApiService("验证工治具出库ID编码")]
        [return: ApiReturn("出库ID编码信息  UnloadIDCodeInfo")]
        public virtual UnloadIDCodeInfo ValidateUnloadIDCodeVsDemandDetailId([ApiParameter("ID编码")] string code, string fixtureDemandNo, double? Id, double whId)
        {
            var unloadInfo = new UnloadIDCodeInfo();

            var demand = GetFixtureDemand(fixtureDemandNo);
            if (demand == null)
                throw new ValidationException("请先选择出库单！".L10N());
            if (!code.IsNotEmpty())
                throw new ValidationException(fixtureCodeIsNullOrEmpty.L10N());
            var account = GetFixtureAccountByCodeOrRFID(code);
            if (account == null)
                throw new ValidationException(fixtureAccountNotExists.L10N());
            if (account.AccountState != FixtureAccountState.InStorage)
                throw new ValidationException("输入/扫描的工治具ID非在库状态,请更换工治具ID！".L10N());

            var demandDetail = demand.DetailList.FirstOrDefault(m => m.FixtureEncodeId == account.FixtureEncodeId);
            //判断该工治具ID对应的工治具编码是否在待出库明细页签中存在有
            if (Id.HasValue && Id != 0 && demandDetail == null)
            {
                demandDetail = demand.DetailList.FirstOrDefault(m => m.FixtureEncodeId == Id);
                if (demandDetail == null)
                    throw new ValidationException("选择的出库明细不存在！".L10N());
            }
            if (demandDetail == null)
            {
                throw new ValidationException("所扫描的工治具ID在此单据无出库任务".L10N());
            }
            if (demandDetail.DemandQty - demandDetail.UnloadQty < 1)
                throw new ValidationException("该工治具需求明细已扫描完成，请重新扫描！".L10N());
            var ids = account.StockList.Select(m => m.FixtureWarehouseId).ToList();
            if (ids.Any() && ids.FindIndex(m => m == whId) >= 0)//库位属于所选仓库的库位
            {
                unloadInfo.ManageMode = EnumViewModel.EnumToLabel(account.ManageMode).L10N();
                unloadInfo.EncodeCode = account.EncodeCode;
                unloadInfo.ModelName = account.ModelName;
                unloadInfo.Qty = 1;
                unloadInfo.IdCode = account.Code;
                unloadInfo.DetailId = demandDetail.Id;
                ValidataStock(code, unloadInfo, account);
                unloadInfo.CanSumit = true;
                return unloadInfo;
            }
            else
                throw new ValidationException("该工治具ID/编码在所选仓库中无库存！".L10N());
        }

        /// <summary>
        /// 是否存在符合条件的工治具编码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="wo"></param>
        /// <param name="encodeList"></param>
        /// <param name="account"></param>
        private void IsExistFixtureEncode(string code, WorkOrder wo, EntityList<FixtureEncode> encodeList, FixtureAccount account)
        {
            if (account.BindProduct == YesNo.Yes)
            {
                if (!encodeList.Any(p => p.Id == account.FixtureEncodeId))
                    throw new ValidationException("输入/扫描的绑定产品工治具ID[{0}]的工治具编码下的产品清单中不包含工单[{1}]的产品和工艺面！".L10nFormat(code, wo.No));
            }
            else
            {
                if (!encodeList.Any(p => p.Id == account.FixtureEncodeId))
                    throw new ValidationException("输入/扫描的非绑定产品工治具ID[{0}]的工治具编码不存在！".L10nFormat(code));
            }
        }

        /// <summary>
        /// 验证库存台账是否存在
        /// </summary>
        /// <param name="code">工治具ID</param>
        /// <param name="unloadInfo">出库ID编码信息</param>
        /// <param name="account">工治具台帐</param>
        private void ValidataStock(string code, UnloadIDCodeInfo unloadInfo, FixtureAccount account)
        {
            if (account.ManageMode != ManageMode.Number)
                return;
            if (account.FixedStorage == YesNo.Yes)
            {
                if (!account.WarehouseId.HasValue)
                    throw new ValidationException("固定储位的ID类台帐[{0}]的仓库[{1}]不存在，工治具台帐数据异常，请先确认！".L10nFormat(code, account.WarehouseCode));
                if (!account.LocationId.HasValue)
                    throw new ValidationException("固定储位的ID类台帐[{0}]的库位[{1}]不存在，工治具台帐数据异常，请先确认！".L10nFormat(code, account.LocationCode));
                var isExist = GetPassAccountStock(account.Id, account.LocationId.Value);
                if (isExist)
                    throw new ValidationException("固定储位的ID类台帐[{0}]的库位[{1}]不存在出库的合格库存台帐，请先入库！".L10nFormat(code, account.LocationCode));
                unloadInfo.Qty = 1;
                unloadInfo.WarehouseId = account.WarehouseId;
                unloadInfo.LocationId = account.LocationId;
                unloadInfo.Warehouse = codeNameFormant.L10nFormat(account.WarehouseCode, account.WarehouseName);
                unloadInfo.Location = codeNameFormant.L10nFormat(account.LocationCode, account.LocationName);
            }
            else
            {
                var stock = GetAccountStock(account.Id);
                if (stock == null)
                    throw new ValidationException("非固定储位的ID类台帐[{0}]不存在出库的合格库存台帐，请先入库！".L10nFormat(code));
                unloadInfo.Qty = 1;
                unloadInfo.WarehouseId = stock.FixtureWarehouseId;
                unloadInfo.LocationId = stock.FixtureStorageLocationId;
                unloadInfo.Warehouse = codeNameFormant.L10nFormat(stock.WarehouseCode, stock.WarehouseName);
                unloadInfo.Location = stock.LocationCode;
            }
        }

        /// <summary>
        /// 验证工治具出库库位
        /// </summary>
        /// <param name="whId"></param>
        /// <param name="locationCode"></param>
        /// <param name="encodeCode"></param>
        /// <returns></returns>
        [ApiService("验证库位")]
        [return: ApiReturn("出库库位信息  UnloadLocationInfo")]
        public virtual UnloadLocationInfo ValidateFixtureLocation([ApiParameter("仓库Id")] double whId,
            [ApiParameter("库位编码")] string locationCode, [ApiParameter("工治具编码")] string encodeCode)
        {

            var wh = RF.GetById<Warehouse>(whId);
            if (wh == null)
                throw new ValidationException(pleaseSelectWarehouse.L10N());
            if (locationCode.IsNullOrEmpty())
                throw new ValidationException("请扫描库位！".L10N());
            var localtion = Query<StorageLocation>().Where(m => m.Code == locationCode && m.WarehouseId == whId).FirstOrDefault();
            if (localtion == null)
                throw new ValidationException("库位不存在,请重新扫描！".L10N());
            if (encodeCode.IsNullOrEmpty())
                throw new ValidationException("请扫描工治具编码！".L10N());
            var encode = GetFixtureEncodeByCode(encodeCode);
            if (encode == null)
                throw new ValidationException("请扫描工治具编码！".L10N());
            var stock = Query<FixtureAccountStock>().Where(m => m.FixtureWarehouseId == whId && m.FixtureAccount.Code == encode.Code
            && m.PassQty > 0 && m.FixtureStorageLocationId == localtion.Id).FirstOrDefault();
            if (stock == null)
                throw new ValidationException("该库位无库存！".L10N());
            var unloadInfo = new UnloadLocationInfo();
            unloadInfo.Warehouse = stock.WarehouseCode;
            unloadInfo.LocationId = stock.FixtureStorageLocationId;
            unloadInfo.Location = localtion.Code;
            unloadInfo.Qty = stock.PassQty;
            return unloadInfo;

        }

        /// <summary>
        /// 验证工治具出库库位
        /// </summary>
        /// <param name="code">ID编码信息</param>
        /// <param name="locationCode">库位编码</param>
        /// <returns>出库库位信息</returns>
        [ApiService("验证工治具出库库位")]
        [return: ApiReturn("出库库位信息  UnloadLocationInfo")]
        public virtual UnloadLocationInfo ValidateUnloadLocation([ApiParameter("ID编码")] string code,
            [ApiParameter("仓库Id")] double warehouseId,
            [ApiParameter("库位编码")] string locationCode)
        {
            var unloadInfo = new UnloadLocationInfo();
            if (!code.IsNotEmpty())
                throw new ValidationException(fixtureCodeIsNullOrEmpty.L10N());
            if (!locationCode.IsNotEmpty())
                throw new ValidationException("输入/扫描的库位编码不能为空！".L10N());
            var account = GetFixtureAccountByCodeOrRFID(code);
            if (account == null)
                throw new ValidationException(fixtureAccountNotExists.L10N());

            var location = IsExistLocation(code, locationCode, warehouseId);
            var stock = GetPassStock(account.Id, location.Id);
            if (stock == null)
                throw new ValidationException("工治具台帐[{0}]的库位[{1}]不存在出库的合格库存台帐，请先入库！".L10nFormat(code, account.LocationCode));
            if (account.ManageMode == ManageMode.Number && account.FixedStorage == YesNo.Yes)
            {
                if (!account.LocationId.HasValue)
                    throw new ValidationException("固定储位的ID类台帐的库位为空，工治具台帐数据异常，请先确认！".L10nFormat(locationCode, account.LocationCode));
                if (account.LocationId != location.Id)
                    throw new ValidationException("输入/扫描的库位[{0}]与固定储位的ID类台帐的库位[{1}]不一致！".L10nFormat(locationCode, account.LocationCode));
                unloadInfo.Qty = 1;
            }

            unloadInfo.WarehouseId = location.WarehouseId;
            unloadInfo.Warehouse = codeNameFormant.L10nFormat(location.WarehouseCode, location.WarehouseName);
            unloadInfo.LocationId = location.Id;
            unloadInfo.Location = codeNameFormant.L10nFormat(location.Code, location.Name);
            unloadInfo.Qty = stock.PassQty;

            return unloadInfo;
        }

        /// <summary>
        /// 是否存在符合条件的工治具库位
        /// </summary>
        /// <param name="code">工治具ID</param>
        /// <param name="locationCode">库位编码</param>
        /// <param name="warehouseId"></param>
        /// <returns>工治具库位</returns>
        private StorageLocation IsExistLocation(string code, string locationCode, double warehouseId)
        {
            var isExist = IsExistEncodeStorageLocation(code);
            if (isExist)
            {
                var location = GetStorageLocation(code, locationCode, warehouseId);
                if (location == null)
                    throw new ValidationException("输入/扫描的库位在工治具台帐的工治具编码的存储位置下未维护，请先维护！".L10N());
                return location;
            }
            else
            {
                var location = GetStorageLocation(locationCode, warehouseId);
                if (location == null)
                    throw new ValidationException("工治具仓库没有维护库位:{0}，请先维护！".L10nFormat(locationCode));
                return location;
            }
        }

        /// <summary>
        /// 验证载具
        /// </summary>
        /// <param name="toolCode">载具编码</param>
        /// <returns>结果信息</returns>
        [ApiService("验证载具")]
        [return: ApiReturn("结果信息  ResultDataInfo")]
        public virtual ResultDataInfo ValidateTurnoverTool([ApiParameter("载具编码")] string toolCode)
        {
            var resultDataInfo = new ResultDataInfo();
            var turnoverTool = RT.Service.Resolve<KitTurnoverToolController>().GetTurnoverTool(toolCode);
            if (turnoverTool == null)
                resultDataInfo.Message = "该周转工具{0}不存在".L10nFormat(toolCode);
            if (turnoverTool != null)
            {
                resultDataInfo.IsRelax = (turnoverTool.State == TurnoverToolState.Unused);
                resultDataInfo.IsSuccess = true;
            }
            return resultDataInfo;
        }

        /// <summary>
        /// 提交工治具治具出库信息
        /// </summary>
        /// <param name="demandUnloadInfo">工治具治具需求出库信息</param>
        [ApiService("提交工治具治具出库信息")]
        public virtual ScanRecord SubmitDemandUnloadInfo([ApiParameter("工治具治具需求出库信息")] DemandUnloadInfo demandUnloadInfo)
        {
            if (demandUnloadInfo == null)
            {
                throw new ValidationException("前端传值错误！".L10N());
            }
            if (demandUnloadInfo.Qty <= 0)
            {
                throw new ValidationException("出库数量不能小于0！".L10N());
            }
            var now = RF.Find<FixtureDemand>().GetDbTime();
            var fixDemand = GetFixtureDemand(demandUnloadInfo.No);
            if (fixDemand == null)
            {
                throw new ValidationException("需求单号不存在！".L10N());
            }
            if (fixDemand.DemandState == DemandState.Finish)
            {
                throw new ValidationException("此需求单号【{0}】已出库完成！".L10nFormat(demandUnloadInfo.No));
            }
            var details = GetFixtureDemandDetails(fixDemand.Id);
            if (!details.Any())
            {
                throw new ValidationException("找不到需求明细！".L10N());
            }
            var demandQty = details.Sum(p => p.DemandQty);
            var unloadQty = details.Sum(p => p.UnloadQty);
            if (demandQty - unloadQty < demandUnloadInfo.Qty)
            {
                throw new ValidationException("出库失败！此需求单号【{0}】待出库数量为{1},不足{2}！"
                      .L10nFormat(demandUnloadInfo.No, demandQty - unloadQty, demandUnloadInfo.Qty));
            }
            var account = GetFixtureAccountByCodeOrRFID(demandUnloadInfo.Code);
            if (account == null)
            {
                throw new ValidationException("工治具治具台账{0}不存在！".L10nFormat(demandUnloadInfo.Code));
            }
            using (var tran = DB.TransactionScope(KitFixturesEntityDataProvider.ConnectionStringName))
            {
                //创建出库明细
                var fixtureUnload = SubmitCreateFixtureUnload(demandUnloadInfo, now, account.Id, fixDemand.Id);
                //创建治具出入库记录
                UnloadCreateFixtureRecord(demandUnloadInfo.No, fixtureUnload);
                //更新需求明细
                var updateCount = SubmitUpdateDemandDetail(account.FixtureEncodeId, fixDemand.Id, demandUnloadInfo.Qty);
                if (updateCount == 0)
                {
                    throw new ValidationException("更新需求明细失败，请重新确认需求明细是否有此工治具编码的明细！".L10N());
                }
                //更新出库状态
                fixDemand.DemandState = SubmitUpdateDemandState(fixDemand.Id);
                //创建保养任务
                var isMaintain = UnloadCreateMaintainTask(account.FixtureEncodeId, fixDemand.No, demandUnloadInfo.Qty, account.Id, fixtureUnload);
                //更新治具台账
                SubmitUpdateAccount(account, isMaintain, demandUnloadInfo, fixDemand);
                RF.Save(fixDemand);
                tran.Complete();
            }
            if (!demandUnloadInfo.ToolCode.IsNullOrEmpty())//更新周转工具
            {
                var kitTurnoverToolController = RT.Service.Resolve<KitTurnoverToolController>();
                var turnoverTool = kitTurnoverToolController.GetTurnoverTool(demandUnloadInfo.ToolCode);
                if (turnoverTool == null)
                {
                    throw new ValidationException("载具【{0}】不存在".L10nFormat(demandUnloadInfo.ToolCode));
                }
                turnoverTool.State = TurnoverToolState.Inuse;
            }

            var loc = RF.GetById<StorageLocation>(demandUnloadInfo.LocationId);
            ScanRecord scanRecord = new ScanRecord();
            scanRecord.Code = demandUnloadInfo.No;
            scanRecord.RFID = account.Rfid;
            scanRecord.Location = string.Format(codeNameFormant, loc.Code, loc.Name);
            scanRecord.Qty = demandUnloadInfo.Qty.ToString();
            scanRecord.TurnoverToolCode = demandUnloadInfo.ToolCode;
            return scanRecord;
        }

        /// <summary>
        /// 提交工治具治具出库信息-创建出库明细
        /// </summary>
        /// <param name="demandUnloadInfo">工治具治具需求出库信息</param>
        /// <param name="now">出库执行时间</param>
        /// <param name="fixtureAccountId">台账ID</param>
        /// <param name="fixDemandId">工治具治具需求iD</param>
        /// <returns>出库明细</returns>
        private FixtureUnload SubmitCreateFixtureUnload(DemandUnloadInfo demandUnloadInfo, DateTime now, double fixtureAccountId, double fixDemandId)
        {
            var fixtureUnload = new FixtureUnload();
            fixtureUnload.UnloadQty = demandUnloadInfo.Qty;
            fixtureUnload.TurnoverToolCode = demandUnloadInfo.ToolCode;
            fixtureUnload.UnloadDate = now;
            fixtureUnload.State = ReceiveState.None;
            fixtureUnload.FixtureAccountId = fixtureAccountId;
            fixtureUnload.UnloadById = RT.IdentityId;
            fixtureUnload.LocationId = demandUnloadInfo.LocationId;
            fixtureUnload.WarehouseId = demandUnloadInfo.WarehouseId;
            fixtureUnload.FixtureDemandId = fixDemandId;
            RF.Save(fixtureUnload);
            return fixtureUnload;
        }

        /// <summary>
        /// 提交工治具治具出库信息-创建治具出库入记录
        /// </summary>
        /// <param name="no">需求单号</param>
        /// <param name="fixtureUnload">出库明细</param>
        private void UnloadCreateFixtureRecord(string no, FixtureUnload fixtureUnload)
        {
            var record = new FixtureRecord();
            record.Code = no;
            record.Qty = fixtureUnload.UnloadQty;
            record.ApplyById = fixtureUnload.CreateBy;
            record.ApplyDate = fixtureUnload.CreateDate;
            record.ComplyById = fixtureUnload.UnloadById;
            record.ComplyDate = fixtureUnload.UnloadDate;
            record.RecordType = RecordType.Out;
            record.FixtureAccountId = fixtureUnload.FixtureAccountId;
            record.BusinessType = BusinessType.Demand;
            record.FixtureWarehouseId = fixtureUnload.WarehouseId;
            record.FixtureStorageLocationId = fixtureUnload.LocationId;
            RF.Save(record);
        }

        /// <summary>
        /// 找到对应的工治具治具编码所在行更新（增加）【出库数量】
        /// </summary>
        /// <param name="fixtureEncodeId">工治具治具编码ID</param>
        /// <param name="fixDemandId">工治具治具需求id</param>
        /// <param name="qty">出库数量</param>
        private int SubmitUpdateDemandDetail(double fixtureEncodeId, double fixDemandId, int qty)
        {
            return DB.Update<FixtureDemandDetail>()
                .Set(p => p.UnloadQty, p => p.UnloadQty + qty)
                .Where(p => p.FixtureEncodeId == fixtureEncodeId && p.FixtureDemandId == fixDemandId)
                .Execute();
        }

        /// <summary>
        /// 获取出库状态
        /// </summary>
        /// <param name="fixDemandId">需求Id</param>
        /// <returns>出库状态</returns>
        private DemandState SubmitUpdateDemandState(double fixDemandId)
        {
            var details = GetFixtureDemandDetails(fixDemandId);
            if (details.All(p => p.UnloadQty >= p.DemandQty))
                return DemandState.Finish;
            else if (details.All(p => p.UnloadQty == 0))
                return DemandState.None;
            return DemandState.Part;
        }

        /// <summary>
        /// 提交工治具治具出库信息-创建保养任务
        /// </summary>
        /// <param name="fixtureEncodeId">治具编码id</param>
        /// <param name="no">需求单号</param>
        /// <param name="qty">出库数量</param>
        /// <param name="accountId">台账ID</param>
        /// <param name="fixtureUnload">出库明细</param>
        /// <returns>是否有保养任务</returns>
        private bool UnloadCreateMaintainTask(double fixtureEncodeId, string no, int qty, double accountId, FixtureUnload fixtureUnload)
        {
            var maintainPrjs = GetToStorageMaintainMaintainProjects(fixtureEncodeId);
            if (!maintainPrjs.Any())
                return false;
            var maintainTask = new MaintainTask()
            {
                No = _commonController.GetNo<MaintainTask>("保养任务编号".L10N()),
                RelatedNo = no,
                MaintainType = MaintainType.ToStorage,
                FixtureAccountId = accountId,
                State = MaintainState.Wait,
                ApplyDate = RF.Find<MaintainTask>().GetDbTime(),
                Qty = qty
            };
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
            fixtureUnload.MaintainTaskId = maintainTask.Id;
            RF.Save(fixtureUnload);
            return true;
        }

        /// <summary>
        /// 提交工治具治具出库信息-更新库存台账
        /// </summary>
        /// <param name="account">台账</param>
        /// <param name="isMaintain">是否保养</param>
        /// <param name="demandUnloadInfo">工治具治具需求出库信息</param>
        /// <param name="fixtureDemand">工治具治具需求</param>
        private void SubmitUpdateAccount(FixtureAccount account, bool isMaintain, DemandUnloadInfo demandUnloadInfo, FixtureDemand fixtureDemand)
        {
            if (account.ManageMode == ManageMode.Number)
            {
                if (!account.StockList.Any())
                {
                    throw new ValidationException("此工治具【{0}】没有库存数据，不能出库！".L10nFormat(account.Code));
                }
                account.AccountState = isMaintain ? FixtureAccountState.WaitMaintain : FixtureAccountState.WaitReceive;
                account.StockList.Clear();
            }
            else
            {
                if (account.InStockQty < demandUnloadInfo.Qty)
                {
                    throw new ValidationException("该【工治具】在库数量为{0}，不足{1}！".L10nFormat(account.InStockQty, demandUnloadInfo.Qty));
                }
                if (account.PassQty < demandUnloadInfo.Qty)
                {
                    throw new ValidationException("该【工治具】合格数量为{0}，不足{1}！".L10nFormat(account.PassQty, demandUnloadInfo.Qty));
                }
                account.InStockQty -= demandUnloadInfo.Qty;
                //account.PassQty -= demandUnloadInfo.Qty;
                if (isMaintain)
                {
                    account.WaitMaintain += demandUnloadInfo.Qty;
                }
                else
                {
                    account.WaitReceive += demandUnloadInfo.Qty;
                }
                //更新库存
                var fixtureStock = GetStockByIdCodeAndLocation(account.Id, demandUnloadInfo.LocationId);
                if (fixtureStock == null)
                {
                    throw new ValidationException("该【库位】不存在已扫描的【ID编码】库存台账信息！".L10N());
                }
                if (fixtureStock.PassQty < demandUnloadInfo.Qty)
                {
                    throw new ValidationException("该【库位】合格数量为{0}，不足{1}！".L10nFormat(fixtureStock.PassQty, demandUnloadInfo.Qty));
                }
                fixtureStock.TotalQty -= demandUnloadInfo.Qty;
                fixtureStock.PassQty -= demandUnloadInfo.Qty;
                RF.Save(fixtureStock);
                //创建使用履历
                CreateSaveAccountUseResume(account.Id, fixtureDemand.ResourceId, fixtureDemand.WorkOrderId, UseResumeType.Unload, demandUnloadInfo.Qty);
            }
            RF.Save(account);
        }

        #endregion

        /// <summary>
        /// 获取扫描的是什么类型
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        [ApiService("获取扫描的是什么类型")]
        public virtual Tuple<int, string> GetScanCodeIsWhat([ApiParameter("扫描条码")] string Code)
        {
            const int encodeType = 4;
            var turnoverTool = RT.Service.Resolve<KitTurnoverToolController>().GetTurnoverTool(Code);
            if (turnoverTool != null)
            { return new Tuple<int, string>(3, turnoverTool.Code); }
            var account = RT.Service.Resolve<CoreFixtureController>().GetFixtureAccountByCodeOrRFID(Code);
            if (account == null)
            {
                return new Tuple<int, string>(-1, "");
            }
            var model = account.FixtureEncode.FixtureModel.ManageMode;
            var code = model == ManageMode.Code ? account.EncodeCode : account.Code;
            var type = model == ManageMode.Code ? encodeType : 0;
            return new Tuple<int, string>(type, code);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="no"></param>
        /// <param name="whId"></param>
        /// <returns></returns>
        [ApiService("校验工治具编码")]
        public virtual UnloadIDCodeInfo ValidateEncodeCode([ApiParameter("扫描条码")] string code, [ApiParameter("单据号")] string no, [ApiParameter("仓库Id")] double whId)
        {
            var unloadInfo = new UnloadIDCodeInfo();
            var wh = RF.GetById<Warehouse>(whId);
            if (wh == null)
            { throw new ValidationException("请先选择仓库！".L10N()); }
            var demand = GetFixtureDemand(no);
            if (demand == null)
            { throw new ValidationException("出库需求单不存在！".L10N()); }
            var encode = RT.Service.Resolve<CoreFixtureController>().GetFixtureEncodeByCode(code);
            if (encode == null)
            {
                throw new ValidationException("工治具编码不存在{0}！".L10nFormat(code));
            }
            var demandDetail = demand.DetailList.FirstOrDefault(m => m.FixtureEncodeId == encode.Id);
            if (demandDetail == null)
            {
                throw new ValidationException("工治具编码{0}在该单据无出库任务！".L10nFormat(code));
            }
            if (demandDetail.UnloadQty >= demandDetail.DemandQty)
            {
                throw new ValidationException("该工治具编码出库任务已完成,无需再扫描！".L10N());
            }
            var fixtureAccountStock = Query<FixtureAccountStock>().Where(m => m.FixtureAccount.FixtureEncodeId == encode.Id && m.PassQty > 0).FirstOrDefault();
            if (fixtureAccountStock == null)
            {
                throw new ValidationException("该工治具编码在所选仓库无库存！".L10nFormat(code, no));
            }

            unloadInfo.ManageMode = EnumViewModel.EnumToLabel(encode.FixtureModel.ManageMode).L10N();
            unloadInfo.EncodeCode = encode.Code;
            unloadInfo.Qty = demandDetail.UnloadQty;
            unloadInfo.DemanQty = demandDetail.DemandQty;
            unloadInfo.DetailId = demandDetail.Id;
            return unloadInfo;
        }

        #region 数据处理 data
        /// <summary>
        /// 生成分页
        /// </summary>
        /// <param name="pageNumber">页码</param>
        /// <param name="pageSize">据数量</param>
        /// <returns></returns>
        protected virtual PagingInfo GetPagingInfo(int? pageNumber, int? pageSize)
        {
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };
            return pagingInfo;
        }
        #endregion

    }
}
