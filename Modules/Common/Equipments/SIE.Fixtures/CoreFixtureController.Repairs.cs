using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Core.Enums;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Enums;
using SIE.EventMessages.EMS.Fixtures;
using SIE.Fixtures.FixtureDemands;
using SIE.Fixtures.FixtureRecords;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.InboundOrders;
using SIE.Fixtures.Models;
using SIE.Fixtures.Querys.ViewModels;
using SIE.Fixtures.Repairs;
using SIE.Fixtures.Repairs.ViewModels;
using SIE.TurnoverTools.TurnoverTools;
using SIE.Utils;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using AccountState = SIE.Fixtures.Fixtures.Accounts.FixtureAccountState;
using QualityState = SIE.Fixtures.Fixtures.Accounts.FixtureQualityState;

namespace SIE.Fixtures
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CoreFixtureController
    {
        /// <summary>
        /// 获取报修信息列表-自定义查询
        /// </summary>
        /// <param name="criteria"><see cref="FixtureRepairCriteria"/>查询条件</param>
        /// <returns></returns>
        public virtual EntityList<FixtureRepair> GetFixtureRepairList(FixtureRepairCriteria criteria)
        {
            var q = Query<FixtureRepair>();
            if (criteria.No.IsNotEmpty())
                q.Where(w => w.No.Contains(criteria.No));
            if (criteria.RepairState.HasValue)
                q.Where(w => w.RepairState == criteria.RepairState);
            if (criteria.ApplyDate.BeginValue.HasValue)
                q.Where(w => w.ApplyDate >= criteria.ApplyDate.BeginValue);
            if (criteria.ApplyDate.EndValue.HasValue)
                q.Where(w => w.ApplyDate <= criteria.ApplyDate.EndValue);
            if (criteria.RepairDate.BeginValue.HasValue)
                q.Where(w => w.RepairDate >= criteria.RepairDate.BeginValue);
            if (criteria.RepairDate.EndValue.HasValue)
                q.Where(w => w.RepairDate <= criteria.RepairDate.EndValue);
            return q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        #region 工治具查询与单工治具出库
        /// <summary>
        /// 根据工治具查询查询体获取工治具查询ViewModel列表
        /// </summary>
        /// <param name="criteria">工治具查询查询体</param>
        /// <returns>工治具查询ViewModel列表</returns>
        public virtual EntityList<FixtureQueryViewModel> GetFixtureQueryVMs(FixtureQueryViewModelCriteria criteria)
        {
            if (!criteria.FixtureTypeId.HasValue)
                throw new ValidationException("查询条件工治具类型必选！".L10N());
            if ((criteria.WorkShopId.HasValue || criteria.ResourceId.HasValue) && !criteria.WorkOrderId.HasValue)
                throw new ValidationException("查询条件选择了车间或产线，则工单号必选！".L10N());

            var queryVMList = new EntityList<FixtureQueryViewModel>();
            var satisfiedAccountIds = new List<double>();
            var query = Query<FixtureQueryViewModel>().Exists<FixtureAccount>((x, y) => y.Join<FixtureEncode>((c, d) => c.FixtureEncodeId == d.Id)
                      .Join<FixtureEncode, FixtureModel>((c, d) => c.FixtureModelId == d.Id && d.FixtureTypeId == criteria.FixtureTypeId)
                      .Where(p => p.Id == x.FixtureAccountId)
                      .WhereIf<FixtureEncode>(criteria.FixtureEncodeId.HasValue, (u, e) => e.Id == criteria.FixtureEncodeId)
                      .WhereIf<FixtureModel>(criteria.FixtureModelId.HasValue, (o, t) => t.Id == criteria.FixtureModelId)
                     );

            var fixtureQueryVMs = query.Distinct().OrderBy(criteria.OrderInfoList).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            satisfiedAccountIds.AddRange(fixtureQueryVMs.Select(p => p.FixtureAccountId).Distinct());

            if (criteria.ProductId.HasValue || criteria.Deck.HasValue)
                satisfiedAccountIds = GetSatisfiedFixtureAccountIds(criteria, satisfiedAccountIds, fixtureQueryVMs);

            if (criteria.RepairBeforeState.HasValue)
                queryVMList.AddRange(GetFixtureQueryViewModels(criteria.RepairBeforeState.Value, satisfiedAccountIds));
            else
                queryVMList.AddRange(GetInStockAndOnlineQueryVMs(satisfiedAccountIds));
            queryVMList.MarkSaved();
            return queryVMList;
        }

        /// <summary>
        /// 根据产品和工艺面过滤获取符合条件的工治具台帐Id列表（如果工治具台帐的工治具编码下未维护产品清单，则工治具台帐不受产品和工艺面条件的过滤;如果工治具台帐的工治具编码下至少维护一笔产品清单，则必须匹配产品和工艺面，如不，则过滤掉）
        /// </summary>
        /// <param name="criteria">工治具查询查询体</param>
        /// <param name="accountIds">工治具台帐Id列表</param>
        /// <param name="fixtureQueryVMs">工治具查询ViewModel列表</param>
        /// <returns>满足条件的工治具台帐Id列表</returns>
        private List<double> GetSatisfiedFixtureAccountIds(FixtureQueryViewModelCriteria criteria, List<double> accountIds, EntityList<FixtureQueryViewModel> fixtureQueryVMs)
        {
            var satisfiedAccountIds = new List<double>();
            var validateAccountIds = new List<double>();
            var dicAccountEncodes = GetAccountEncodeDictionary(fixtureQueryVMs);
            var encodes = GetFixtureEncodesByAccountIds(accountIds);
            var dicEncodes = encodes.ToDictionary(p => p.Id);
            foreach (var accountId in accountIds)
            {
                if (!dicAccountEncodes.TryGetValue(accountId, out double encodeId))
                    continue;
                if (!dicEncodes.TryGetValue(encodeId, out FixtureEncode encode))
                    continue;
                if (encode.BindProduct == YesNo.Yes)
                    validateAccountIds.Add(accountId);
                else
                    satisfiedAccountIds.Add(accountId);
            }

            if (validateAccountIds.Any())
                satisfiedAccountIds.AddRange(GetSatisfiedAccountIds(validateAccountIds, criteria.ProductId, criteria.Deck));
            return satisfiedAccountIds;
        }

        /// <summary>
        /// 获取工治具台帐Id和工治具编码Id的字典
        /// </summary>
        /// <param name="fixtureQueryVMs">工治具查询ViewModel列表</param>
        /// <returns>工治具台帐Id和工治具编码Id的字典</returns>
        private Dictionary<double, double> GetAccountEncodeDictionary(EntityList<FixtureQueryViewModel> fixtureQueryVMs)
        {
            var dicAccountEncodes = new Dictionary<double, double>();
            foreach (var queryVM in fixtureQueryVMs)
            {
                if (!dicAccountEncodes.ContainsKey(queryVM.FixtureAccountId))
                    dicAccountEncodes.Add(queryVM.FixtureAccountId, queryVM.FixtureEncodeId);
            }

            return dicAccountEncodes;
        }

        /// <summary>
        /// 根据产品Id和工艺面获取满足条件的工治具台帐Id列表
        /// </summary>
        /// <param name="accountIds">工治具台帐Id列表</param>
        /// <param name="productId">产品Id</param>
        /// <param name="deck">工艺面</param>
        /// <returns>工治具台帐Id列表</returns>
        private List<double> GetSatisfiedAccountIds(List<double> accountIds, double? productId, Deck? deck)
        {
            var satisfiedAccounts = GetSatisfiedAccounts(accountIds, productId, deck);
            return satisfiedAccounts.Select(p => p.Id).Distinct().ToList();
        }

        /// <summary>
        /// 根据工治具台帐Id列表(位置状态包括在库/在线)获取满足在线的工治具查询ViewModel列表工治具台帐Id列表获取满足在线的工治具查询ViewModel列表
        /// </summary>
        /// <param name="accountIds">工治具台帐Id列表</param>
        /// <returns>工治具查询ViewModel列表</returns>
        private List<FixtureQueryViewModel> GetInStockAndOnlineQueryVMs(List<double> accountIds)
        {
            var queryVMList = new List<FixtureQueryViewModel>();
            var satisfiedAccountIds = new List<double>();
            var satisfiedAccounts = new List<FixtureAccount>();
            var accountsOfID = GetFixtureIDAccounts(accountIds);
            satisfiedAccounts.AddRange(accountsOfID);
            var accountIdsOfID = accountsOfID.Select(p => p.Id).Distinct();
            satisfiedAccountIds.AddRange(accountIdsOfID);
            var accountsOfCode = GetFixtureCodeAccounts(accountIds);
            satisfiedAccounts.AddRange(accountsOfCode);
            var accountIdsOfCode = accountsOfCode.Select(p => p.Id).Distinct();
            satisfiedAccountIds.AddRange(accountIdsOfCode);
            var stocks = GetFixtureStocks(satisfiedAccountIds);
            var dicStocks = stocks.GroupBy(p => p.FixtureAccountId).ToDictionary(p => p.Key, p => p.ToList());
            var unloads = GetFixtureUnloadsByAccountIds(satisfiedAccountIds);
            var demandIds = unloads.Select(p => p.FixtureDemandId).Distinct().ToList();
            var dicUnloads = unloads.GroupBy(p => p.FixtureAccountId).ToDictionary(p => p.Key, p => p.ToList());
            var demands = GetFixtureDemands(demandIds);
            var dicDemands = demands.ToDictionary(p => p.Id);

            queryVMList.AddRange(GetFixtureQueryVMList(satisfiedAccounts, dicStocks, dicUnloads, dicDemands));

            return queryVMList;
        }

        /// <summary>
        /// 获取工治具台帐在线/在库的工治具查询ViewModel列表
        /// </summary>
        /// <param name="accounts">工治具台帐列表</param>
        /// <param name="dicStocks">库存台帐字典</param>
        /// <param name="dicUnloads">工治具需求清单的出库明细字典</param>
        /// <param name="dicDemands">工治具需求清单字典</param>
        /// <returns>工治具查询ViewModel列表</returns>
        private List<FixtureQueryViewModel> GetFixtureQueryVMList(List<FixtureAccount> accounts, Dictionary<double, List<FixtureAccountStock>> dicStocks, Dictionary<double, List<FixtureUnload>> dicUnloads, Dictionary<double, FixtureDemand> dicDemands)
        {
            var queryVMList = new List<FixtureQueryViewModel>();
            foreach (var account in accounts)
            {
                if (account.ManageMode == ManageMode.Number)
                {
                    if (account.AccountState == AccountState.InStorage)
                        queryVMList.AddRange(GetFixtureQueryVMs(dicStocks, account));
                    else if (account.AccountState == AccountState.Online || account.AccountState == AccountState.Using)
                        queryVMList.AddRange(GetFixtureQueryVMsByFixtureDemands(dicUnloads, dicDemands, account));
                }
                else
                {
                    if (account.InStockQty > 0)
                        queryVMList.AddRange(GetFixtureQueryVMs(dicStocks, account));
                    if (account.OnlineQty > 0)
                        queryVMList.AddRange(GetFixtureQueryVMsByFixtureDemands(dicUnloads, dicDemands, account));
                }
            }

            return queryVMList;
        }

        /// <summary>
        /// 根据位置状态和工治具台帐Id列表获取满足在线的工治具查询ViewModel列表
        /// </summary>
        /// <param name="repairBeforeState">在库/在线</param>
        /// <param name="accountIds">工治具台帐Id列表</param>
        /// <returns>工治具查询ViewModel列表</returns>
        private List<FixtureQueryViewModel> GetFixtureQueryViewModels(RepairBeforeState repairBeforeState, List<double> accountIds)
        {
            if (repairBeforeState == RepairBeforeState.InStock)
                return GetInStockFixtureQueryVMsByAccountIds(accountIds);
            else
                return GetOnlineFixtureQueryVMsByAccountIds(accountIds);
        }

        /// <summary>
        /// 根据工治具台帐Id列表获取满足在线的工治具查询ViewModel列表
        /// </summary>
        /// <param name="accountIds">工治具台帐Id列表</param>
        /// <returns>工治具查询ViewModel列表</returns>
        private List<FixtureQueryViewModel> GetOnlineFixtureQueryVMsByAccountIds(List<double> accountIds)
        {
            var queryVMList = new List<FixtureQueryViewModel>();
            var satisfiedAccountIds = new List<double>();
            var satisfiedAccounts = new List<FixtureAccount>();
            var accountsOfID = GetOnlineIDAccounts(accountIds);
            satisfiedAccounts.AddRange(accountsOfID);
            var accountIdsOfID = accountsOfID.Select(p => p.Id).Distinct();
            satisfiedAccountIds.AddRange(accountIdsOfID);
            var accountsOfCode = GetOnlineCodeAccounts(accountIds);
            satisfiedAccounts.AddRange(accountsOfCode);
            var accountIdsOfCode = accountsOfCode.Select(p => p.Id).Distinct();
            satisfiedAccountIds.AddRange(accountIdsOfCode);
            var unloads = GetFixtureUnloadsByAccountIds(satisfiedAccountIds);
            var demandIds = unloads.Select(p => p.FixtureDemandId).Distinct().ToList();
            var dicUnloads = unloads.GroupBy(p => p.FixtureAccountId).ToDictionary(p => p.Key, p => p.ToList());
            var demands = GetFixtureDemands(demandIds);
            var dicDemands = demands.ToDictionary(p => p.Id);

            queryVMList.AddRange(GetOnlineFixtureQueryVMs(satisfiedAccounts, dicUnloads, dicDemands));

            return queryVMList;
        }

        /// <summary>
        /// 根据工治具台帐Id列表获取满足在库的工治具查询ViewModel列表
        /// </summary>
        /// <param name="accountIds">工治具台帐Id列表</param>
        /// <returns>工治具查询ViewModel列表</returns>
        private List<FixtureQueryViewModel> GetInStockFixtureQueryVMsByAccountIds(List<double> accountIds)
        {
            var queryVMList = new List<FixtureQueryViewModel>();
            var satisfiedAccountIds = new List<double>();
            var satisfiedAccounts = new List<FixtureAccount>();
            var accountsOfID = GetInStockIDAccounts(accountIds);
            satisfiedAccounts.AddRange(accountsOfID);
            var accountIdsOfID = accountsOfID.Select(p => p.Id).Distinct();
            satisfiedAccountIds.AddRange(accountIdsOfID);
            var accountsOfCode = GetInStockCodeAccounts(accountIds);
            satisfiedAccounts.AddRange(accountsOfCode);
            var accountIdsOfCode = accountsOfCode.Select(p => p.Id).Distinct();
            satisfiedAccountIds.AddRange(accountIdsOfCode);
            var stocks = GetFixtureStocks(satisfiedAccountIds);
            var dicStocks = stocks.GroupBy(p => p.FixtureAccountId).ToDictionary(p => p.Key, p => p.ToList());

            queryVMList.AddRange(GetInStockFixtureQueryVMs(satisfiedAccounts, dicStocks));

            return queryVMList;
        }

        /// <summary>
        /// 获取在线的工治具查询ViewModel列表
        /// </summary>
        /// <param name="accounts">工治具台帐列表</param>
        /// <param name="dicUnloads">工治具需求清单的出库明细字典</param>
        /// <param name="dicDemands">工治具需求清单字典</param>
        /// <returns>工治具查询ViewModel列表</returns>
        private List<FixtureQueryViewModel> GetOnlineFixtureQueryVMs(List<FixtureAccount> accounts, Dictionary<double, List<FixtureUnload>> dicUnloads, Dictionary<double, FixtureDemand> dicDemands)
        {
            var queryVMList = new List<FixtureQueryViewModel>();
            foreach (var account in accounts)
            {
                queryVMList.AddRange(GetFixtureQueryVMsByFixtureDemands(dicUnloads, dicDemands, account));
            }

            return queryVMList;
        }

        /// <summary>
        /// 根据工治具需求清单的出库明细获取在线的工治具查询ViewModel列表
        /// </summary>
        /// <param name="dicUnloads">工治具需求清单的出库明细字典</param>
        /// <param name="dicDemands">工治具需求清单字典</param>
        /// <param name="account">工治具台帐</param>
        /// <returns>工治具查询ViewModel列表</returns>
        private List<FixtureQueryViewModel> GetFixtureQueryVMsByFixtureDemands(Dictionary<double, List<FixtureUnload>> dicUnloads, Dictionary<double, FixtureDemand> dicDemands, FixtureAccount account)
        {
            var queryVMList = new List<FixtureQueryViewModel>();
            if (dicUnloads.TryGetValue(account.Id, out List<FixtureUnload> unloadsOfAccount))
            {
                var dicOnlineUnloads = GetOnlineFixtureUnloads(dicDemands, unloadsOfAccount);

                foreach (var dicOnlineUnload in dicOnlineUnloads)
                {
                    var key = dicOnlineUnload.Key;
                    var unloads = dicOnlineUnload.Value;
                    var fixtureQueryVM = CreateOnlineFixtureQueryVM(account, key, unloads);
                    queryVMList.Add(fixtureQueryVM);
                }
            }

            return queryVMList;
        }

        /// <summary>
        /// 获取在线某工治具台帐相同产线和工单的列表字典
        /// </summary>
        /// <param name="dicDemands">工治具需求清单字典</param>
        /// <param name="unloadsOfAccount">某工治具台帐的出库明细列表</param>
        /// <returns>某工治具台帐相同产线和工单的列表字典</returns>
        private Dictionary<DicOnlineKey, List<FixtureUnload>> GetOnlineFixtureUnloads(Dictionary<double, FixtureDemand> dicDemands, List<FixtureUnload> unloadsOfAccount)
        {
            var dicOnlineUnloads = new Dictionary<DicOnlineKey, List<FixtureUnload>>();
            foreach (var unload in unloadsOfAccount)
            {
                if (dicDemands.TryGetValue(unload.FixtureDemandId, out FixtureDemand demand))
                {
                    var key = new DicOnlineKey() { WorkOrderId = demand.WorkOrderId, ResourceId = demand.ResourceId, WorkOrderNo = demand.WorkOrderNo, ResourceName = demand.ResourceName };
                    if (!dicOnlineUnloads.ContainsKey(key))
                        dicOnlineUnloads.Add(key, new List<FixtureUnload>() { unload });
                    else
                        dicOnlineUnloads[key].Add(unload);
                }
            }

            return dicOnlineUnloads;
        }

        /// <summary>
        /// 创建在线的工治具查询ViewModel
        /// </summary>
        /// <param name="account">工治具台帐</param>
        /// <param name="key">工治具需求清单的出库明细</param>
        /// <param name="unloads">出库明细列表（某一个编码类的工治具台帐存在不同的工治具需求清单中)</param>
        /// <returns>工治具查询ViewModel</returns>
        private FixtureQueryViewModel CreateOnlineFixtureQueryVM(FixtureAccount account, DicOnlineKey key, List<FixtureUnload> unloads)
        {
            return new FixtureQueryViewModel()
            {
                FixtureAccountId = account.Id,
                AccountCode = account.Code,
                EncodeCode = account.EncodeCode,
                ModelCode = account.ModelCode,
                ModelName = account.ModelName,
                FixtureType = account.FixtureEncode.FixtureModel.FixtureType.Code,
                ManageMode = account.ManageMode,
                WorkOrderId = key.WorkOrderId,
                ResourceId = key.ResourceId,
                ResourceName = key.ResourceName,
                WorkOrderNo = key.WorkOrderNo,
                RepairBeforeState = RepairBeforeState.Online,
                Qty = unloads.Sum(p => p.UnloadQty) - unloads.Sum(p => p.ReturnQty),
            };
        }

        /// <summary>
        /// 获取在库的工治具查询ViewModel列表
        /// </summary>
        /// <param name="accounts">工治具台帐列表</param>
        /// <param name="dicStocks">库存台帐字典</param>
        /// <returns>工治具查询ViewModel列表</returns>
        private List<FixtureQueryViewModel> GetInStockFixtureQueryVMs(List<FixtureAccount> accounts, Dictionary<double, List<FixtureAccountStock>> dicStocks)
        {
            var queryVMList = new List<FixtureQueryViewModel>();
            foreach (var account in accounts)
            {
                queryVMList.AddRange(GetFixtureQueryVMs(dicStocks, account));
            }

            return queryVMList;
        }

        /// <summary>
        /// 根据库存台帐获取在库的工治具查询ViewModel列表
        /// </summary>
        /// <param name="dicStocks">库存台帐字典</param>
        /// <param name="account">工治具台帐列表</param>
        /// <returns>工治具查询ViewModel列表</returns>
        private List<FixtureQueryViewModel> GetFixtureQueryVMs(Dictionary<double, List<FixtureAccountStock>> dicStocks, FixtureAccount account)
        {
            var queryVMList = new List<FixtureQueryViewModel>();
            if (dicStocks.TryGetValue(account.Id, out List<FixtureAccountStock> stocksOfAccount))
            {
                foreach (var stockOfAccount in stocksOfAccount)
                {
                    var fixtureQueryVM = CreateInStockFixtureQueryVM(account, stockOfAccount);
                    if (fixtureQueryVM != null)
                    {
                        queryVMList.Add(fixtureQueryVM);
                    }
                }
            }
            return queryVMList;
        }

        /// <summary>
        /// 创建在库的工治具查询ViewModel
        /// </summary>
        /// <param name="account">工治具台帐</param>
        /// <param name="stockOfAccount">某工治具台帐的库存台帐</param>
        /// <returns>工治具查询ViewModel</returns>
        private FixtureQueryViewModel CreateInStockFixtureQueryVM(FixtureAccount account, FixtureAccountStock stockOfAccount)
        {
            if (account == null || stockOfAccount == null)
                return null;
            return new FixtureQueryViewModel()
            {
                FixtureAccountId = account.Id,
                AccountCode = account.Code,
                EncodeCode = account.EncodeCode,
                ModelCode = account.ModelCode,
                ModelName = account.ModelName,
                FixtureType = account.FixtureTypeCode,
                ManageMode = account.ManageMode,
                WarehouseId = stockOfAccount.FixtureWarehouseId,
                WarehouseName = stockOfAccount.WarehouseName,
                LocationId = stockOfAccount.FixtureStorageLocationId,
                LocationName = stockOfAccount.LocationName,
                Qty = stockOfAccount.TotalQty,
                RepairBeforeState = RepairBeforeState.InStock,
            };
        }

        /// <summary>
        /// 执行在库出库
        /// </summary>
        /// <param name="unloadVM">出库ViewModel</param>
        /// <returns>错误信息</returns>
        public virtual string SaveUnload(UnloadViewModel unloadVM)
        {
            string errMsg = string.Empty;

            try
            {
                var account = GetFixtureAccount(unloadVM.FixtureAccountId);
                if (account == null)
                    throw new ValidationException("工治具ID不存在！".L10N());
                var wo = RF.GetById<WorkOrder>(unloadVM.WorkOrderId);
                if (wo == null)
                    throw new ValidationException("工单号不存在！".L10N());
                if (unloadVM.TurnoverToolCode.IsNotEmpty())
                {
                    var isExist = RT.Service.Resolve<KitTurnoverToolController>().IsTurnoverTool(unloadVM.TurnoverToolCode);
                    if (!isExist)
                        throw new ValidationException("载具不存在，请重新填写！".L10N());
                }
                var deckId = RT.Service.Resolve<IProcessSurface>().GetProcessSurface(wo.Id);
                var encodeList = GetFixtureEncodeList(null, null, null, wo.ProductId, deckId, string.Empty);
                if (account.BindProduct == YesNo.Yes)
                {
                    if (!encodeList.Any(p => p.Id == account.FixtureEncodeId))
                        throw new ValidationException("绑定产品工治具ID[{0}]的工治具编码下的产品清单中不包含工单[{1}]的产品和工艺面！".L10nFormat(unloadVM.AccountCode, wo.No));
                }
                else
                {
                    if (!encodeList.Any(p => p.Id == account.FixtureEncodeId))
                        throw new ValidationException("非绑定产品工治具ID[{0}]的工治具编码不存在！".L10nFormat(unloadVM.AccountCode));
                }

                var location = ValidateStorageLocation(unloadVM);
                var stock = ValidateAccountStock(unloadVM, account, location);

                SaveUnloadData(unloadVM, account, wo, deckId, location, stock);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return errMsg;
        }

        /// <summary>
        /// 验证库位台帐
        /// </summary>
        /// <param name="unloadVM">出库ViewModel</param>
        /// <param name="account">工治具台帐</param>
        /// <param name="location">工治具库位</param>
        /// <returns>库位台帐</returns>
        private FixtureAccountStock ValidateAccountStock(UnloadViewModel unloadVM, FixtureAccount account, StorageLocation location)
        {
            var stock = GetPassStock(account.Id, location.Id);
            if (stock == null)
                throw new ValidationException("工治具台帐[{0}]的库位[{1}]不存在出库的合格库存台帐，请先上架！".L10nFormat(unloadVM.AccountCode, location.Code));

            if (stock.PassQty < unloadVM.UnloadQty)
                throw new ValidationException("工治具台帐[{0}]的库位[{1}]出库的数量不能大于库存的合格数！".L10nFormat(unloadVM.AccountCode, location.Code));
            return stock;
        }

        /// <summary>
        /// 验证库位
        /// </summary>
        /// <param name="unloadVM">出库ViewModel</param>
        /// <returns>库位</returns>
        private StorageLocation ValidateStorageLocation(UnloadViewModel unloadVM)
        {
            var location = GetStorageLocation(unloadVM.LocationId);
            if (location == null)
                throw new ValidationException("工治具仓库没有维护此库位，出库失败！".L10N());
            if (location.WarehouseId != unloadVM.WarehouseId)
                throw new ValidationException("仓库和库位关系异常，出库失败！".L10N());
            return location;
        }

        /// <summary>
        /// 保存出库数据
        /// </summary>
        /// <param name="unloadVM">出库ViewModel</param>
        /// <param name="account">工治具台帐</param>
        /// <param name="wo">工单</param>
        /// <param name="deckId">工单工艺面</param>
        /// <param name="location">库位</param>
        /// <param name="stock">库存台帐</param>
        private void SaveUnloadData(UnloadViewModel unloadVM, FixtureAccount account, WorkOrder wo, int? deckId, StorageLocation location, FixtureAccountStock stock)
        {
            var ctr = RT.Service.Resolve<ElecFixtureController>();
            using (var tran = DB.TransactionScope(KitFixturesEntityDataProvider.ConnectionStringName))
            {
                var demand = ctr.CreateFixtureDemand(unloadVM.WorkOrderId, unloadVM.UnloadQty, account, wo, deckId);
                var maintainPrjs = GetToStorageMaintainMaintainProjects(account.FixtureEncodeId);

                ctr.CreateMaintainTaskAndFixtureUnload(unloadVM.UnloadQty, account, location, demand, maintainPrjs);
                ctr.CreateFixtureRecord(unloadVM.UnloadQty, account, location, demand);
                ctr.UpdateAccountAndStock(unloadVM.UnloadQty, account, stock, maintainPrjs);

                if (account.ManageMode == ManageMode.Code)
                {
                    CreateSaveAccountUseResume(account.Id, demand.ResourceId, demand.WorkOrderId, UseResumeType.Unload, unloadVM.UnloadQty);
                }
                tran.Complete();
            }
        }
        #endregion


        #region 获取报修信息 

        /// <summary>
        /// 获取异常/维修详情数据集
        /// </summary>
        /// <param name="fixtureRepairId">报修ID</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="OrderInfos">排序</param>
        /// <returns></returns>
        public virtual EntityList<FixtureRepairDetail> GetFixtureRepairDetailsByRepairId(double fixtureRepairId, PagingInfo pagingInfo = null, IList<OrderInfo> OrderInfos = null)
        {
            var query = Query<FixtureRepairDetail>().Where(c => c.FixtureRepairId == fixtureRepairId);
            if (OrderInfos != null)
                query.OrderBy(OrderInfos);
            var repairDetails = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            repairDetails.ForEach(p => p.Result = p.InspectionResult);
            return repairDetails;
        }

        /// <summary>
        /// 根据工治具台帐ID获取工治具异常详情列表
        /// </summary>
        /// <param name="accountId">工治具台帐ID</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>工治具异常详情列表</returns> 
        public virtual EntityList<FixtureRepairDetail> GetFixtureRepairDetailsByAccountId(double accountId, PagingInfo pagingInfo = null)
        {
            return Query<FixtureRepairDetail>().Where(p => p.FixtureAccountId == accountId).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具异常详情Id列表获取工治具异常详情列表
        /// </summary>
        /// <param name="repairDetailIds">工治具异常详情Id列表</param>
        /// <returns>工治具异常详情列表</returns>
        public virtual EntityList<FixtureRepairDetail> GetFixtureRepairDetailsByDetailIds(List<double> repairDetailIds)
        {
            return Query<FixtureRepairDetail>().Where(p => repairDetailIds.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// 获取维修单号
        /// </summary>
        /// <returns></returns>
        public virtual string GetFixtureRepairNo()
        {
            var config = ConfigService.GetConfig(new NoConfig(), typeof(FixtureRepair));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到维修单号生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(Convert.ToDouble(config.NumberRuleId), 1).FirstOrDefault();
        }

        /// <summary>
        /// 获取所有的附件-图片
        /// </summary>
        /// <param name="fixtureRepairId">工治具报修id</param>
        /// <param name="pagingInfo"></param>
        /// <param name="OrderInfos"></param>
        /// <returns></returns>
        public virtual EntityList<FixtureRepairAttachment> GetFixtureRepairAttachment(double fixtureRepairId, PagingInfo pagingInfo, IList<OrderInfo> OrderInfos)
        {
            return Query<FixtureRepairAttachment>().Exists<FixtureRepairDetail>((a, b) => b.Where(f => f.Id == a.OwnerId && f.FixtureRepairId == fixtureRepairId)).OrderBy(OrderInfos).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取维修记录
        /// </summary>
        /// <param name="fixtureRepairId">工治具报修id</param>
        /// <param name="pagingInfo"></param>
        /// <param name="OrderInfos"></param>
        /// <returns></returns>
        public virtual EntityList<FixtureRepairRecord> GetFixtureRepairRecord(double fixtureRepairId, PagingInfo pagingInfo, IList<OrderInfo> OrderInfos)
        {
            return Query<FixtureRepairRecord>().Exists<FixtureRepairDetail>((a, b) => b.Where(f => f.Id == a.FixtureRepairDetailId && f.FixtureRepairId == fixtureRepairId)).OrderBy(OrderInfos).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具异常详情Id列表获取维修记录列表
        /// </summary>
        /// <param name="repairDetailIds">工治具异常详情Id列表</param>
        /// <returns>维修记录列表</returns>
        public virtual EntityList<FixtureRepairRecord> GetFixtureRepairRecords(List<double> repairDetailIds)
        {
            return Query<FixtureRepairRecord>().Where(p => repairDetailIds.Contains(p.FixtureRepairDetailId)).ToList();
        }

        #endregion

        #region 保存 Save
        /// <summary>
        /// 创建工治具报修信息
        /// </summary>
        /// <returns>工治具报修信息</returns>
        public virtual AddFixtureRepairInfo GetFixtureRepairInfo()
        {
            AddFixtureRepairInfo repairInfo = new AddFixtureRepairInfo();
            repairInfo.errMsg = string.Empty;
            try
            {
                var applyBy = RT.Identity;
                FixtureRepair repair = new FixtureRepair()
                {
                    No = RT.Service.Resolve<ElecFixtureController>().GetFixtureRepairNo(),
                    RepairState = RepairState.Wait,
                    ApplyById = applyBy.Id,
                    ApplyDate = RF.Find<FixtureRepair>().GetDbTime(),
                    ApplyByName = applyBy.Name,
                };

                repairInfo.data = repair;

            }
            catch (Exception ex)
            {
                repairInfo.errMsg = ex.Message;
            }

            return repairInfo;
        }

        /// <summary>
        /// 保存维修工治具报修信息
        /// </summary>
        /// <param name="fixRepairInfo">工治具报修信息</param>
        /// <returns>错误信息</returns>
        public virtual string SaveRepairInfo(FixtureRepairInfo fixRepairInfo)
        {
            string errMsg = string.Empty;

            if (fixRepairInfo.FixtureRepairDetailList.Any(p => p.RepairWhereaboutStatus == Enums.RepairWhereabout.In && p.InWarehouseId == null))
            {
                throw new ValidationException("维修后去向为入库时,入库仓库必填！".L10N());
            }

            try
            {
                using (var trans = DB.TransactionScope(KitFixturesEntityDataProvider.ConnectionStringName))
                {
                    var fixtureRepair = fixRepairInfo.FixtureRepair;
                    var repairDetails = fixRepairInfo.FixtureRepairDetailList;
                    var repairRecords = fixRepairInfo.FixtureRepairRecordList;

                    //防止并发，将数据行锁定
                    DB.Update<FixtureRepair>().Set(p => p.UpdateBy, RT.IdentityId).Where(p => p.Id == fixtureRepair.Id).Execute();
                    var oldRepair = RF.GetById<FixtureRepair>(fixtureRepair.Id) ?? throw new ValidationException("工治具报修不存在！".L10N());
                    if (repairRecords.Any(p => p.Code == string.Empty && p.Name == string.Empty))
                    {
                        throw new ValidationException("备件编码和备件名称必填其一！".L10N());
                    }

                    var repairDetailIds = repairDetails.Select(p => p.Id).Distinct().ToList();
                    var orgRepairDetails = GetFixtureRepairDetailsByDetailIds(repairDetailIds);
                    var dicOrgRepairDetails = orgRepairDetails.ToDictionary(p => p.Id);

                    foreach (var repairDetail in repairDetails)
                    {
                        if (dicOrgRepairDetails.TryGetValue(repairDetail.Id, out FixtureRepairDetail orgRepairDetail))
                        {
                            if (orgRepairDetail.InspectionResult.HasValue)
                            {
                                repairDetail.IsRepair = true;
                            }
                            else
                            {
                                repairDetail.IsRepair = false;
                            }
                        }

                        if (repairDetail.RepairBeforeState == RepairBeforeState.Online && repairDetail.RepairWhereaboutStatus == Enums.RepairWhereabout.In)
                        {
                            var unloads = GetReturnUnloads((double)repairDetail.WorkOrderId, repairDetail.FixtureAccountId);
                            //更新出库明细的归还数量
                            UpdateUnloadReturnQty(repairDetail.Qty, unloads);
                        }
                    }

                    var accountIds = repairDetails.Select(p => p.FixtureAccountId).Distinct().ToList();
                    var accounts = GetFixtureAccounts(accountIds);
                    var dicAccounts = accounts.ToDictionary(p => p.Id);
                    var locationIds = repairDetails.Where(p => p.StorageLocation != null).Select(p => p.FixtureStorageLocationId).Distinct().ToList();
                    RT.Service.Resolve<CoreFixtureController>().GetFixtureStocks(accountIds, locationIds);

                    DealRepairRecords(repairDetails, repairRecords);
                    UpdateFixtureRepair(oldRepair, repairDetails);

                    repairDetails.ForEach(p => p.PersistenceStatus = PersistenceStatus.Modified);
                    RF.Save(repairDetails);

                    //更新工治具台账并创建入库单
                    UpdateAccount(repairDetails.Where(p => !p.IsRepair), dicAccounts);

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
        /// 更新出库明细的归还数量
        /// </summary>
        /// <param name="qty">需归还数量</param>
        /// <param name="unloads">出库明细列表</param>
        private void UpdateUnloadReturnQty(int qty, EntityList<FixtureUnload> unloads)
        {
            var returnQty = qty;
            foreach (var unload in unloads)
            {
                var canReturnQty = unload.UnloadQty - unload.ReturnQty - unload.NgQty;
                if (returnQty <= 0)
                { break; }
                if (returnQty > canReturnQty)
                {
                    unload.ReturnQty = unload.UnloadQty - unload.NgQty;
                    returnQty -= canReturnQty;
                }
                else if (returnQty == canReturnQty)
                {
                    unload.ReturnQty = unload.UnloadQty - unload.NgQty;
                    break;
                }
                else
                {
                    unload.ReturnQty += returnQty;
                    break;
                }
            }
            RF.Save(unloads);
        }



        /// <summary>
        /// 更新工治具报修
        /// </summary>
        /// <param name="oldRepair">数据库工治具报修</param>
        /// <param name="repairDetails">工治具异常详情</param>
        private void UpdateFixtureRepair(FixtureRepair oldRepair, EntityList<FixtureRepairDetail> repairDetails)
        {
            if (repairDetails.All(p => p.InspectionResult != null))
            {
                oldRepair.RepairState = RepairState.Finish;
                oldRepair.RepairById = RT.IdentityId;
                oldRepair.RepairDate = RF.Find<FixtureRepair>().GetDbTime();
            }
            else if (repairDetails.All(p => p.InspectionResult == null))
            {
                oldRepair.RepairById = null;
                oldRepair.RepairState = RepairState.Wait;
            }
            else if (repairDetails.Any(p => p.InspectionResult != null))
            {
                oldRepair.RepairById = RT.IdentityId;
                oldRepair.RepairState = RepairState.Part;
            }

            RF.Save(oldRepair);
        }

        /// <summary>
        /// 处理维修记录列表
        /// </summary>
        /// <param name="repairDetails">工治具异常详情列表</param>
        /// <param name="repairRecords">维修记录列表</param>
        private void DealRepairRecords(EntityList<FixtureRepairDetail> repairDetails, EntityList<FixtureRepairRecord> repairRecords)
        {
            var deleteRecordList = new EntityList<FixtureRepairRecord>();
            var allRecordIds = repairRecords.Select(p => p.Id).Distinct().ToList();
            var repairDetailIds = repairDetails.Select(p => p.Id).Distinct().ToList();
            var records = GetFixtureRepairRecords(repairDetailIds);
            var dicRecords = records.ToDictionary(p => p.Id);
            var deleteRecords = records.Where(p => !allRecordIds.Contains(p.Id));
            if (deleteRecords.Any())
            {
                deleteRecordList.AddRange(deleteRecords);
                deleteRecordList.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
                RF.Save(deleteRecordList);
            }

            foreach (var repairRecord in repairRecords)
            {
                repairRecord.PersistenceStatus = PersistenceStatus.New;
                if (dicRecords.TryGetValue(repairRecord.Id, out FixtureRepairRecord record))
                    repairRecord.PersistenceStatus = PersistenceStatus.Modified;
                RF.Save(repairRecord);
            }
        }

        /// <summary>
        /// 更新工治具台帐
        /// </summary>
        /// <param name="repairDetails">工治具异常详情列表(首次维修）</param>
        /// <param name="dicAccounts">工治具台帐字典</param>
        private void UpdateAccount(IEnumerable<FixtureRepairDetail> repairDetails, Dictionary<double, FixtureAccount> dicAccounts)
        {
            foreach (var repairDetail in repairDetails)
            {
                if (repairDetail.InspectionResult == null)
                    continue;
                if (dicAccounts.TryGetValue(repairDetail.FixtureAccountId, out FixtureAccount account))
                {
                    if (account.ManageMode == ManageMode.Number)
                        UpdateIDAccount(repairDetail, account);
                    else
                        UpdateCodeAccount(repairDetail, account);
                }
            }
        }

        /// <summary>
        /// 更新编码类工治具台帐
        /// </summary>
        /// <param name="repairDetail">工治具异常详情</param>
        /// <param name="account">编码类工治具台帐</param>
        private void UpdateCodeAccount(FixtureRepairDetail repairDetail, FixtureAccount account)
        {
            //if (repairDetail.RepairBeforeState == RepairBeforeState.InStock)
            //{
            //    SaveLaunchTask(repairDetail);
            //    account.WaitRepair -= repairDetail.Qty;
            //    account.WaitShelfQty += repairDetail.Qty;
            //    RF.Save(account);
            //}
            //else
            //{
            //    account.WaitRepair -= repairDetail.Qty;
            //    account.OnlineQty += repairDetail.Qty;
            //    RF.Save(account);
            //}

            if (repairDetail.InspectionResult == Common.InspectionResult.Pass && repairDetail.RepairWhereaboutStatus == Enums.RepairWhereabout.Use)
            {
                //维修合格+继续使用：【在线、合格】增加，【待维修、不合格】减少
                account.OnlineQty += repairDetail.Qty;
                account.PassQty += repairDetail.Qty;
                account.WaitRepair -= repairDetail.Qty;
                account.NgQty -= repairDetail.Qty;
            }
            if (repairDetail.InspectionResult == Common.InspectionResult.Pass && repairDetail.RepairWhereaboutStatus == Enums.RepairWhereabout.In)
            {
                SaveLaunchTask(repairDetail);
                //维修合格+入库：【待入库、合格】增加，【待维修、不合格】减少
                account.WaitShelfQty += repairDetail.Qty;
                account.PassQty += repairDetail.Qty;
                account.WaitRepair -= repairDetail.Qty;
                account.NgQty -= repairDetail.Qty;
            }
            if (repairDetail.InspectionResult == Common.InspectionResult.Fail && repairDetail.RepairWhereaboutStatus == Enums.RepairWhereabout.In)
            {
                SaveLaunchTask(repairDetail);
                //维修不合格+入库：【待入库】增加，【待维修】减少
                account.WaitShelfQty += repairDetail.Qty;
                account.WaitRepair -= repairDetail.Qty;
            }
            RF.Save(account);
        }

        /// <summary>
        /// 更新ID类工治具台帐
        /// </summary>
        /// <param name="repairDetail">工治具异常详情</param>
        /// <param name="account">ID类工治具台帐</param>
        private void UpdateIDAccount(FixtureRepairDetail repairDetail, FixtureAccount account)
        {
            if (repairDetail.RepairBeforeState == RepairBeforeState.InStock)
            {
                //创建入库单
                SaveLaunchTask(repairDetail);
                account.AccountState = AccountState.WaitShelf;
            }
            else
                account.AccountState = AccountState.Online;

            //因维修完成后，无需保养，所以可以直接更改工治具台帐质量状态
            if (repairDetail.InspectionResult == Common.InspectionResult.Pass)
                account.QualityState = QualityState.Pass;
            else
                account.QualityState = QualityState.Ng;
            RF.Save(account);
        }

        /// <summary>
        /// 创建和保存入库单
        /// </summary>
        /// <param name="repairDetail">工治具异常详情</param>
        private void SaveLaunchTask(FixtureRepairDetail repairDetail)
        {
            var account = RF.GetById<FixtureAccount>(repairDetail.FixtureAccountId, new EagerLoadOptions().LoadWithViewProperty());
            var task = new InboundOrders.InboundOrder
            {
                No = _commonController.GetNo<InboundOrder>("工治具入库"),
                FixtureEncodeId = account.FixtureEncodeId,
                Qty = repairDetail.Qty,
                InboundType = FixtureInboundType.RepairIn,
                InboundStatus = InboundStatus.ToBe,
                QualityState = QualityState.Pass,
                WarehouseId = repairDetail.InWarehouseId,
                PersistenceStatus = PersistenceStatus.New
            };
            task.GenerateId();
            if (repairDetail.InspectionResult == Common.InspectionResult.Fail)
                task.QualityState = QualityState.Ng;
            if (account.ManageMode == ManageMode.Code)//编码
            {
                task.InboundOrderFixtureCodeAccountList.Add(new InboundOrderFixtureCodeAccount()
                {
                    InboundOrderId = task.Id,
                    Qty = repairDetail.Qty,
                });
            }
            if (account.ManageMode == ManageMode.Number)
            {
                task.InboundOrderFixtureIdAccountList.Add(new InboundOrderFixtureIdAccount()
                {
                    InboundOrderId = task.Id,
                    Qty = 1,
                    FixtureIDAccountId = repairDetail.FixtureAccountId,
                    Rfid = account.Rfid,

                });
            }
            RF.Save(task);
        }

        /// <summary>
        /// 保存添加工治具报修信息
        /// </summary>InStorage
        /// <param name="fixRepairInfo">工治具报修信息</param>
        /// <returns>错误信息</returns>
        public virtual string SaveAddRepairInfo(FixtureRepairInfo fixRepairInfo)
        {
            string errMsg = string.Empty;
            try
            {
                var fixtureRepair = fixRepairInfo.FixtureRepair;
                var repairDetails = fixRepairInfo.FixtureRepairDetailList;
                if (!repairDetails.Any())
                {
                    throw new ValidationException("工治具异常详情至少维护一笔数据！".L10N());
                }

                //新增验证工治具异常详情
                CheckRepairDetails(repairDetails);

                var orgRepairDetails = GetFixtureRepairDetailsByRepairId(fixtureRepair.Id, null, null);
                var orgRepairDetailIds = orgRepairDetails.Select(p => p.Id).Distinct().ToList();
                var accountIds = repairDetails.Select(p => p.FixtureAccountId).Distinct().ToList();
                var accounts = GetFixtureAccounts(accountIds);
                var dicAccounts = accounts.ToDictionary(p => p.Id);
                var locationIds = repairDetails.Where(p => p.StorageLocation != null).Select(p => p.FixtureStorageLocationId).Distinct().ToList();
                //根据工治具库位Id列表获取工治具库位列表
                var locations = GetFixtureStorageLocations(locationIds);
                var dicLocations = locations.ToDictionary(p => p.Id);
                //根据工治具台帐Id列表和库位Id列表获取库存台帐列表
                var stocks = RT.Service.Resolve<CoreFixtureController>().GetFixtureStocks(accountIds, locationIds);


                using (var trans = DB.TransactionScope(KitFixturesEntityDataProvider.ConnectionStringName))
                {
                    //设置工治具报修
                    SetFixtureRepair(fixtureRepair);
                    //更新验证工治具异常详情列表
                    UpdateValidateRepairDetail(fixtureRepair, repairDetails, orgRepairDetailIds, dicAccounts, dicLocations, stocks);
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
        /// 验证工治具异常详情
        /// </summary>
        private void CheckRepairDetails(EntityList<FixtureRepairDetail> repairDetails)
        {
            foreach (var detail in repairDetails)
            {
                //仓库，库位，工单，报修前质量状态，数量
                if (detail.ManageMode == ManageMode.Code) {
                    if (detail.RepairBeforeState == RepairBeforeState.InStock)
                    {
                        if (!detail.FixtureWarehouseId.HasValue)
                        {
                            throw new ValidationException("报修前状态为【在库】的仓库必输！".L10N());
                        }
                        if (!detail.FixtureStorageLocationId.HasValue) {
                            throw new ValidationException("报修前状态为【在库】的库位必输！".L10N());
                        }
                        if (!detail.RepairBeforeQualityStatus.HasValue)
                        {
                            throw new ValidationException("报修前状态为【在库】时,报修前质量状态必输！".L10N());
                        }
                        //在库报修时，数量不能大于库存数，库存数为：根据【工治具编码 + 仓库 + 库位 + 报修前质量状态】获取当前台账的库存
                        var StockQty = GetFixtureAccountStockQty(detail.FixtureAccountId, (double)detail.FixtureWarehouseId, (double)detail.FixtureStorageLocationId, (QualityState)detail.RepairBeforeQualityStatus);
                        if (detail.Qty > StockQty)
                        {
                            throw new ValidationException("【在库】报修时，数量不能大于库存数！".L10N());
                        }
                    }
                    if (detail.RepairBeforeState == RepairBeforeState.Online)
                    {
                        if (detail.FixtureWarehouseId.HasValue)
                        {
                            throw new ValidationException("报修前状态为【在线】时不能编辑仓库只能为空！".L10N());
                        }
                        if (detail.FixtureStorageLocationId.HasValue)
                        {
                            throw new ValidationException("报修前状态为【在线】时不能编辑库位只能为空！".L10N());
                        }
                        if (!detail.WorkOrderId.HasValue)
                        {
                            throw new ValidationException("报修前状态为【在线】时工单必输！".L10N());
                        }
                        if (detail.RepairBeforeQualityStatus.Value!= QualityState.Pass)
                        {
                            throw new ValidationException("报修前状态为【在线】时报修前质量状态只能合格！".L10N());
                        }

                        var unloads = GetReturnUnloads((double)detail.WorkOrderId, detail.FixtureAccountId);
                        if (!unloads.Any())
                        {
                            throw new ValidationException("此工治具无可报修的出库明细信息！".L10N());
                        }
                        var canReturnQtys = unloads.Sum(p => p.UnloadQty) - unloads.Sum(p => p.ReturnQty) - unloads.Sum(p => p.NgQty);
                        if (canReturnQtys < detail.Qty)
                        {
                            throw new ValidationException("报修数量必须小于未归还数量：{0}！".L10nFormat(canReturnQtys));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 根据【工治具编码+仓库+库位+质量状态】获取当前台账的库存
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="FixtureWarehouseId"></param>
        /// <param name="FixtureStorageLocationId"></param>
        /// <param name="RepairBeforeQualityStatus"></param>
        /// <returns></returns>
        public virtual int GetFixtureAccountStockQty(double accountId, double FixtureWarehouseId, double FixtureStorageLocationId, FixtureQualityState RepairBeforeQualityStatus)
        {
            int Qty = 0;
            var fixtureAccountStock = Query<FixtureAccountStock>().Where(p => p.FixtureAccountId == accountId && p.FixtureWarehouseId == FixtureWarehouseId && p.FixtureStorageLocationId == FixtureStorageLocationId).FirstOrDefault();
            if (fixtureAccountStock != null)
            {
                if (RepairBeforeQualityStatus == QualityState.Pass)
                {
                    Qty = fixtureAccountStock.PassQty;
                }
                if (RepairBeforeQualityStatus == QualityState.Ng)
                {
                    Qty = fixtureAccountStock.NgQty;
                }
            }
            return Qty;
        }

        /// <summary>
        /// 根据【工单、工治具】获取可归还的出库明细列表
        /// </summary>
        /// <param name="workOrderId">工单id</param>
        /// <param name="accountId">工治具id</param>
        /// <returns>可归还的出库明细列表</returns>
        public virtual EntityList<FixtureUnload> GetReturnUnloads(double workOrderId, double accountId)
        {
            var demandIds = Query<FixtureDemand>().Where(p => p.WorkOrderId == workOrderId)
                .Select(p => p.Id).ToList<double>().ToList();
            if (!demandIds.Any())
            {
                throw new ValidationException("找不到此产线和工单的工治具需求清单！".L10N());
            }
            var unloads = Query<FixtureUnload>().Where(p => demandIds.Contains(p.FixtureDemandId) && p.State == ReceiveState.Finish
                          && p.ReturnQty < (p.UnloadQty - p.NgQty) && p.FixtureAccountId == accountId).ToList();
            return unloads;
        }


        /// <summary>
        /// 更新验证工治具异常详情列表
        /// </summary>
        /// <param name="fixtureRepair">工治具报修</param>
        /// <param name="repairDetails">工治具异常详情列表</param>
        /// <param name="orgRepairDetailIds">原工治具异常详情Id列表</param>
        /// <param name="dicAccounts">工治具台帐字典</param>
        /// <param name="dicLocations">工治具库位字典</param>
        /// <param name="stocks">库存台帐列表</param>
        /// <returns>已修改的原工治具异常详情Id列表</returns>
        private void UpdateValidateRepairDetail(FixtureRepair fixtureRepair, EntityList<FixtureRepairDetail> repairDetails, List<double> orgRepairDetailIds, Dictionary<double, FixtureAccount> dicAccounts, Dictionary<double, StorageLocation> dicLocations, EntityList<FixtureAccountStock> stocks)
        {
            var newRepairDetails = repairDetails.Where(p => !orgRepairDetailIds.Contains(p.Id));
            foreach (var repairDetail in newRepairDetails)
            {
                if (repairDetail.Qty <= 0)
                {
                    throw new ValidationException("报修数量必须大于零，不可报修！".L10N());
                }
                UpdateValidateAccountStock(dicAccounts, dicLocations, stocks, repairDetail);
                repairDetail.PersistenceStatus = PersistenceStatus.New;
                repairDetail.FixtureRepairId = fixtureRepair.Id;
                repairDetail.InWarehouseId = repairDetail.FixtureWarehouseId;
                RF.Save(repairDetail);
            }
        }

        /// <summary>
        /// 更新验证工治具台帐及其库存台帐
        /// </summary>
        /// <param name="dicAccounts">工治具台帐字典</param>
        /// <param name="dicLocations">工治具库位字典</param>
        /// <param name="stocks">库存台帐列表</param>
        /// <param name="repairDetail">工治具异常详情</param>
        private void UpdateValidateAccountStock(Dictionary<double, FixtureAccount> dicAccounts, Dictionary<double, StorageLocation> dicLocations, EntityList<FixtureAccountStock> stocks, FixtureRepairDetail repairDetail)
        {
            if (dicAccounts.TryGetValue(repairDetail.FixtureAccountId, out FixtureAccount account))
            {
                if (account.ManageMode == ManageMode.Number)
                {
                    if (!(account.AccountState == AccountState.InStorage || account.AccountState == AccountState.Using || account.AccountState == AccountState.Online))
                        throw new ValidationException("ID类工治具台帐的状态[{0}]不为在线、使用中和在库，不可报修！".L10nFormat(EnumViewModel.EnumToLabel(account.AccountState).L10N()));
                    if (repairDetail.RepairBeforeState == RepairBeforeState.InStock)
                        UpdateInStockIDAccount(dicLocations, stocks, repairDetail, account);
                    else
                        UpdateOnlineIDAccount(repairDetail, account);
                }
                else
                {
                    if (account.InStockQty <= 0 && account.OnlineQty <= 0)
                        throw new ValidationException("编码类工治具台帐的在库和在线数量都小于等于零，不可报修！".L10nFormat(EnumViewModel.EnumToLabel(account.AccountState).L10N()));
                    if (repairDetail.RepairBeforeState == RepairBeforeState.InStock)
                        UpdateInStockCodeAccount(dicLocations, stocks, repairDetail, account);
                    else
                        UpdateOnlineCodeAccount(repairDetail, account);
                }
            }
        }

        /// <summary>
        /// 更新验证报修前状态为在线的编码类工治具台帐
        /// </summary>
        /// <param name="repairDetail">工治具异常详情</param>
        /// <param name="account">编码类工治具台帐</param>
        private void UpdateOnlineCodeAccount(FixtureRepairDetail repairDetail, FixtureAccount account)
        {
            if (repairDetail.FixtureStorageLocationId.HasValue)
                throw new ValidationException("报修前状态为在线的编码类工治具台帐库位不必填！".L10N());
            if (account.OnlineQty <= 0)
                throw new ValidationException("编码类工治具台帐的在线数量小于等于零，不可报修！".L10N());
            if (account.OnlineQty < repairDetail.Qty)
                throw new ValidationException("编码类工治具台帐的报修数量[{0}]大于在线数量，不可报修！".L10nFormat(repairDetail.Qty));

            account.OnlineQty -= repairDetail.Qty;
            account.PassQty -= repairDetail.Qty;
            account.WaitRepair += repairDetail.Qty;
            account.NgQty += repairDetail.Qty;
            RF.Save(account);
        }

        /// <summary>
        /// 更新验证报修前状态为在库的编码类工治具台帐
        /// </summary>
        /// <param name="dicLocations">工治具库位字典</param>
        /// <param name="stocks">库存台帐列表</param>
        /// <param name="repairDetail">工治具异常详情</param>
        /// <param name="account">编码类工治具台帐</param>
        private void UpdateInStockCodeAccount(Dictionary<double, StorageLocation> dicLocations, EntityList<FixtureAccountStock> stocks, FixtureRepairDetail repairDetail, FixtureAccount account)
        {
            if (!repairDetail.FixtureStorageLocationId.HasValue)
                throw new ValidationException("报修前状态为在库的编码类工治具台帐库位必填！".L10N());
            if (!dicLocations.TryGetValue(repairDetail.FixtureStorageLocationId.Value, out StorageLocation locationId))
                throw new ValidationException("报修前状态为在库的编码类工治具台帐库位不存在！".L10N());
            var stock = stocks.FirstOrDefault(p => p.FixtureAccountId == repairDetail.FixtureAccountId && p.FixtureStorageLocationId == repairDetail.FixtureStorageLocationId);
            if (stock == null)
                throw new ValidationException("报修前状态为在库的编码类工治具台帐的库存台帐不存在，不可报修！".L10N());
            if (stock.TotalQty <= 0)
                throw new ValidationException("报修前状态为在库的编码类工治具台帐的在库数量小于等于零，不可报修！".L10N());
            if (stock.TotalQty < repairDetail.Qty)
                throw new ValidationException("报修前状态为在库的编码类工治具台帐的报修数量[{0}]大于在库数量[{1}]，不可报修！".L10nFormat(repairDetail.Qty, stock.TotalQty));
            UpdateInStockCodeAccountStock(repairDetail, account, stock);
        }

        /// <summary>
        /// 更新报修前状态为在库的编码类工治具台帐
        /// </summary>
        /// <param name="repairDetail">工治具异常详情</param>
        /// <param name="account">编码类工治具台帐</param>
        /// <param name="stock">库存详情</param>
        private void UpdateInStockCodeAccountStock(FixtureRepairDetail repairDetail, FixtureAccount account, FixtureAccountStock stock)
        {
            if (repairDetail.RepairBeforeQualityStatus == QualityState.Pass) {
                //报修前质量状态为合格   工治具台账在库,合格减少,  待维修,不合格增加，  库存详情 合格数减少
                account.InStockQty -= repairDetail.Qty;
                account.PassQty -= repairDetail.Qty;
                account.NgQty += repairDetail.Qty;
                account.WaitRepair += repairDetail.Qty;
                stock.PassQty -= repairDetail.Qty;
                stock.TotalQty -= repairDetail.Qty;
            }
            if (repairDetail.RepairBeforeQualityStatus == QualityState.Ng)
            {
                //报修前质量状态为不合格   工治具台账在库减少,待维修增加， 库存详情总数减少 ,不合格数减少
                account.InStockQty -= repairDetail.Qty;
                account.WaitRepair += repairDetail.Qty;
                stock.NgQty -= repairDetail.Qty;
                stock.TotalQty -= repairDetail.Qty;
            }

            stock.PersistenceStatus = PersistenceStatus.Modified;
            if (stock.TotalQty == 0)
            {
                stock.PersistenceStatus = PersistenceStatus.Deleted;
            }
            //保存库存详情和工治具台账
            RF.Save(stock);
            RF.Save(account);
        }

        /// <summary>
        /// 更新验证报修前状态为在线的ID类工治具台帐
        /// </summary>
        /// <param name="repairDetail">工治具异常详情</param>
        /// <param name="account">ID类工治具台帐</param>
        private void UpdateOnlineIDAccount(FixtureRepairDetail repairDetail, FixtureAccount account)
        {
            if (!(account.AccountState == AccountState.Using || account.AccountState == AccountState.Online))
                throw new ValidationException("报修前状态为在线，ID类工治具台帐的状态[{0}]也必须为在线/使用中，不可报修！".L10nFormat(EnumViewModel.EnumToLabel(account.AccountState).L10N()));
            if (repairDetail.FixtureStorageLocationId.HasValue)
                throw new ValidationException("报修前状态为在线的ID类工治具台帐的库位不必填！".L10N());
            if (repairDetail.Qty != 1)
                throw new ValidationException("ID类工治具台帐的报修数量[{0}]不等于1，不可报修！".L10nFormat(repairDetail.Qty));

            account.AccountState = AccountState.WaitRepair;
            account.QualityState = QualityState.Ng;
            RF.Save(account);
        }

        /// <summary>
        /// 更新验证报修前状态为在库的ID类工治具台帐
        /// </summary>
        /// <param name="dicLocations">工治具库位字典</param>
        /// <param name="stocks">库存台帐列表</param>
        /// <param name="repairDetail">工治具异常详情</param>
        /// <param name="account">ID类工治具台帐</param>
        private void UpdateInStockIDAccount(Dictionary<double, StorageLocation> dicLocations, EntityList<FixtureAccountStock> stocks, FixtureRepairDetail repairDetail, FixtureAccount account)
        {
            var stock = ValidateInStockRepairDetail(dicLocations, stocks, repairDetail, account);
            UpdateInStockIDAccountStock(repairDetail, account, stock);
        }

        /// <summary>
        /// 验证报修前状态为在库的ID类工治具台帐的工治具异常详情
        /// </summary>
        /// <param name="dicLocations">工治具库位字典</param>
        /// <param name="stocks">库存台帐列表</param>
        /// <param name="repairDetail">工治具异常详情</param>
        /// <param name="account">ID类工治具台帐</param>
        /// <returns>库存台帐</returns>
        private FixtureAccountStock ValidateInStockRepairDetail(Dictionary<double, StorageLocation> dicLocations, EntityList<FixtureAccountStock> stocks, FixtureRepairDetail repairDetail, FixtureAccount account)
        {
            if (account.AccountState != AccountState.InStorage)
                throw new ValidationException("报修前状态为在库，ID类工治具台帐的状态[{0}]也必须为在库，不可报修！".L10nFormat(EnumViewModel.EnumToLabel(account.AccountState).L10N()));
            if (!repairDetail.FixtureStorageLocationId.HasValue)
                throw new ValidationException("报修前状态为在库的ID类工治具台帐库位必填！".L10N());
            if (!dicLocations.TryGetValue(repairDetail.FixtureStorageLocationId.Value, out StorageLocation locationId))
                throw new ValidationException("报修前状态为在库的ID类工治具台帐库位不存在！".L10N());
            var stock = stocks.FirstOrDefault(p => p.FixtureAccountId == repairDetail.FixtureAccountId && p.FixtureStorageLocationId == repairDetail.FixtureStorageLocationId);
            if (stock == null)
                throw new ValidationException("报修前状态为在库的ID类工治具台帐的库存台帐不存在，不可报修！".L10N());
            if (stock.TotalQty <= 0)
                throw new ValidationException("报修前状态为在库的ID类工治具台帐的在库数量小于等于零，不可报修！".L10N());
            if (stock.TotalQty < repairDetail.Qty)
                throw new ValidationException("报修前状态为在库的ID类工治具台帐的报修数量[{0}]大于在库数量[{1}]，不可报修！".L10nFormat(repairDetail.Qty, stock.TotalQty));
            if (repairDetail.Qty != 1)
                throw new ValidationException("ID类工治具台帐的报修数量[{0}]不等于1，不可报修！".L10nFormat(repairDetail.Qty));
            return stock;
        }

        /// <summary>
        /// 更新报修前状态为在库的ID类工治具台帐
        /// </summary>
        /// <param name="repairDetail">工治具异常详情</param>
        /// <param name="account">ID类工治具台帐</param>
        /// <param name="stock">库存台帐</param>
        private void UpdateInStockIDAccountStock(FixtureRepairDetail repairDetail, FixtureAccount account, FixtureAccountStock stock)
        {
            account.AccountState = AccountState.WaitRepair;
            account.QualityState = QualityState.Ng;
            stock.PersistenceStatus = PersistenceStatus.Deleted;
            RF.Save(stock);
            CreateFixtureRecordByAccountStock(repairDetail.Qty, account, stock);
            RF.Save(account);
        }

        /// <summary>
        /// 设置工治具报修
        /// </summary>
        /// <param name="fixtureRepair">工治具报修</param>
        private void SetFixtureRepair(FixtureRepair fixtureRepair)
        {
            var oldRepair = RF.GetById<FixtureRepair>(fixtureRepair.Id);
            if (oldRepair == null)
                fixtureRepair.PersistenceStatus = PersistenceStatus.New;
            else
                fixtureRepair.PersistenceStatus = PersistenceStatus.Modified;
            RF.Save(fixtureRepair);
        }

        #endregion


        /// <summary>
        /// 创建工治具出入库记录
        /// </summary>
        /// <param name="qty">工治具报修数量</param>
        /// <param name="account">工治具台账</param>
        /// <param name="stock">库存台帐</param>
        private void CreateFixtureRecordByAccountStock(int qty, FixtureAccount account, FixtureAccountStock stock)
        {
            var now = RF.Find<FixtureRecord>().GetDbTime();
            var record = new FixtureRecord()
            {
                RecordType = RecordType.Out,
                BusinessType = BusinessType.RepairOut,
                Code = string.Empty,
                FixtureAccountId = account.Id,
                FixtureWarehouseId = stock.FixtureWarehouseId,
                FixtureStorageLocationId = stock.FixtureStorageLocationId,
                Qty = qty,
                ApplyById = RT.IdentityId,
                ApplyDate = now,
                ComplyById = RT.IdentityId,
                ComplyDate = now
            };
            RF.Save(record);
        }


        /// <summary>
        /// 获取工治具异常详情信息
        /// </summary>
        /// <param name="fixtureAccountId"></param>
        /// <returns></returns>
        public virtual FixtureRepairDetailInfo GetFixtureRepairDetailInfo(double fixtureAccountId)
        {
            FixtureRepairDetailInfo detail = new FixtureRepairDetailInfo();
            //找选择的工治具台账详细信息
            FixtureAccount fixtureAccount = Query<FixtureAccount>().Where(p => p.Id == fixtureAccountId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (fixtureAccount != null)
            {
                //带出保修前状态 在线在库，
                if (fixtureAccount.AccountState == AccountState.Online)
                {
                    detail.RepairBeforeState = RepairBeforeState.Online;
                }
                if (fixtureAccount.AccountState == AccountState.InStorage)
                {
                    detail.RepairBeforeState = RepairBeforeState.InStock;
                }

                if (fixtureAccount.ManageMode == ManageMode.Number)
                {
                    //如果是在线 则带出工单(工治具需求单的工单)，根据工治具ID找工治具需求单的出库所有明细取执行时间最晚的，
                    if (fixtureAccount.AccountState == AccountState.Online)
                    {
                        EagerLoadOptions elo = new EagerLoadOptions();
                        elo.LoadWith(FixtureUnload.FixtureDemandProperty);
                        elo.LoadWith(FixtureDemand.WorkOrderProperty);
                        elo.LoadWithViewProperty();
                        //找工单
                        var fixtureUnload = Query<FixtureUnload>().Where(p => p.FixtureAccountId == fixtureAccount.Id).OrderByDescending(p => p.UnloadDate).FirstOrDefault(elo);
                        if (fixtureUnload != null)
                        {
                            detail.WorkOrderId = fixtureUnload.FixtureDemand.WorkOrderId;
                            detail.WorkOrderId_Display = fixtureUnload.FixtureDemand.WorkOrder?.No;
                        }
                    }

                    //如果是在库，则带出仓库与库位(工治具台账的库存详情)
                    if (fixtureAccount.AccountState == AccountState.InStorage)
                    {
                        var fixtureAccountStock = Query<FixtureAccountStock>().Where(p => p.FixtureAccountId == fixtureAccount.Id).OrderByDescending(p => p.CreateDate).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
                        if (fixtureAccountStock != null)
                        {
                            detail.FixtureWarehouseId = fixtureAccountStock.FixtureWarehouseId;
                            detail.FixtureWarehouseId_Display = fixtureAccountStock.Warehouse?.Code;
                            detail.FixtureStorageLocationId = fixtureAccountStock.FixtureStorageLocationId;
                            detail.FixtureStorageLocationId_Display = fixtureAccountStock.StorageLocation?.Code;
                            detail.FixtureStorageLocationName = fixtureAccountStock.StorageLocation?.Name;
                        }
                    }
                }
            }
            
            return detail;
        }
    }
}
