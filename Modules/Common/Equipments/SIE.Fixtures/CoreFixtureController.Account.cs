using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Core.Common;
using SIE.Core.Common.Controllers;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Enums;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.Fixtures.Accounts.ViewModels;
using SIE.Fixtures.InboundOrders;
using SIE.Fixtures.Models;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SIE.Fixtures
{
    /// <summary>
    /// 工治具台账控制器
    /// </summary>
    public partial class CoreFixtureController : DomainController
    {
        #region 工治具台账
        /// <summary>
        /// 库存台账查询实体方法
        /// </summary>
        /// <param name="criteria">库存台账查询实体</param>
        /// <returns>库存台账列表</returns>
        public virtual EntityList<FixtureAccountStock> GetFixtureAccountStocksByCriteria(FixtureAccountStockCriteria criteria)
        {
            var query = Query<FixtureAccountStock>();
            if (criteria.WarehouseCode.IsNotEmpty() || criteria.WarehouseName.IsNotEmpty())
            {
                query.Exists<Warehouse>((a, b) => b.Where(f => f.Id == a.FixtureWarehouseId)
                .WhereIf(criteria.WarehouseCode.IsNotEmpty(), c => c.Code == criteria.WarehouseCode)
                .WhereIf(criteria.WarehouseName.IsNotEmpty(), c => c.Name == criteria.WarehouseName));
            }
            if (criteria.LocationCode.IsNotEmpty() || criteria.LocationName.IsNotEmpty())
            {
                query.Exists<StorageLocation>((a, b) => b.Where(f => f.Id == a.FixtureStorageLocationId)
                .WhereIf(criteria.LocationCode.IsNotEmpty(), c => c.Code == criteria.LocationCode)
                .WhereIf(criteria.LocationName.IsNotEmpty(), c => c.Name == criteria.LocationName));
            }

            if (criteria.AccountCode.IsNotEmpty() || criteria.EncodeCode.IsNotEmpty() || criteria.ModelCode.IsNotEmpty() || criteria.ModelName.IsNotEmpty()
                || criteria.FixtureTypeId.HasValue)
            {
                query.Exists<FixtureAccount>(
                    (x, y) => y.Join<FixtureEncode>((c, d) => c.FixtureEncodeId == d.Id)
                        .Join<FixtureEncode, FixtureModel>((c, d) => c.FixtureModelId == d.Id)
                        .Where(p => p.Id == x.FixtureAccountId)
                .WhereIf(criteria.AccountCode.IsNotEmpty(), c => c.Code.Contains(criteria.AccountCode))
                .WhereIf<FixtureEncode>(criteria.EncodeCode.IsNotEmpty(), (e, d) => d.Code.Contains(criteria.EncodeCode))
                .WhereIf<FixtureEncode, FixtureModel>(criteria.ModelCode.IsNotEmpty(), (e, d, f) => f.Code.Contains(criteria.ModelCode))
                .WhereIf<FixtureEncode, FixtureModel>(criteria.ModelName.IsNotEmpty(), (e, d, f) => f.Name.Contains(criteria.ModelName))
                .WhereIf<FixtureEncode, FixtureModel>(criteria.FixtureTypeId.HasValue, (e, d, f) => f.FixtureTypeId == criteria.FixtureTypeId));
            }

            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据台账Id列表获取库存台账列表
        /// </summary>
        /// <param name="accountIds">台账Id列表</param>
        /// <returns>库存台账列表</returns>
        public virtual EntityList<FixtureAccountStock> GetFixtureStocks(List<double> accountIds)
        {
            return accountIds.SplitContains(tempIds =>
            {
                return Query<FixtureAccountStock>()
                    .Where(p => tempIds.Contains(p.FixtureAccountId))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 根据工治具台帐Id列表和库位Id列表获取库存台帐列表
        /// </summary>
        /// <param name="accountIds">工治具台帐Id列表</param>
        /// <param name="locationIds">库位Id列表</param>
        /// <returns>库存台帐列表</returns>
        public virtual EntityList<FixtureAccountStock> GetFixtureStocks(List<double> accountIds, List<double?> locationIds)
        {
            return accountIds.SplitContains(tempIds =>
            {
                return Query<FixtureAccountStock>()
                    .Where(p => tempIds.Contains(p.FixtureAccountId)
                        && locationIds.Contains(p.FixtureStorageLocationId) && p.TotalQty > 0)
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 根据工治具台账Id获取库存详情列表
        /// </summary>
        /// <param name="accountIds">工治具台账Id列表</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>库存详情列表</returns>
        public virtual EntityList<FixtureAccountStock> GetFixtureAccountStocksByAccountIds(List<double> accountIds, PagingInfo pagingInfo = null)
        {
            Expression<Func<FixtureAccountStock, bool>> exp = p => accountIds.Contains(p.FixtureAccountId);
            return _commonController.GetDatas(exp, pagingInfo);
        }

        /// <summary>
        /// 根据工治具类型、工单产品和工艺面和工治具仓库获取库存台帐列表
        /// </summary>
        /// <param name="fixtureTypeId">工治具类型</param>
        /// <param name="itemId">工单产品</param>
        /// <param name="deckId">工单工艺面</param>
        /// <param name="warehouseId">仓库Id</param>
        /// <returns>库存台帐列表</returns>
        public virtual EntityList<FixtureAccountStock> GetFixtureAccountStocksByFixtureDemand(double fixtureTypeId, double itemId, int? deckId, double? warehouseId)
        {
            var stockList = new EntityList<FixtureAccountStock>();
            var stocksOfBindProduct = GetAccountStocksOfBindProduct(fixtureTypeId, itemId, deckId, warehouseId);
            if (stocksOfBindProduct.Any())
                stockList.AddRange(stocksOfBindProduct);
            var stocksOfUnBindProduct = GetAccountStocksOfUnBindProduct(fixtureTypeId, warehouseId);
            if (stocksOfUnBindProduct.Any())
                stockList.AddRange(stocksOfUnBindProduct);
            return stockList;
        }

        /// <summary>
        /// 根据工治具类型、工单产品和工艺面和工治具仓库获取绑定产品的工治具台帐的库存台帐列表
        /// </summary>
        /// <param name="fixtureTypeId">工治具类型</param>
        /// <param name="itemId">工单产品</param>
        /// <param name="deckId">工单工艺面</param>
        /// <param name="warehouseId">仓库Id</param>
        /// <returns>库存台帐列表</returns>
        public virtual EntityList<FixtureAccountStock> GetAccountStocksOfBindProduct(double fixtureTypeId, double itemId, int? deckId, double? warehouseId)
        {
            var query = Query<FixtureAccountStock>().Where(p => p.PassQty > 0);
            if (deckId == null)
                query.Exists<FixtureAccount>(
                        (x, y) => y.Join<FixtureEncode>((c, d) => c.FixtureEncodeId == d.Id)
                            .Join<FixtureEncode, FixtureEncodeProductDetail>((c, d) => c.Id == d.FixtureEncodeId && d.ItemId == itemId && d.Deck == null)
                            .Join<FixtureEncode, FixtureModel>((c, d) => c.FixtureModelId == d.Id && d.FixtureTypeId == fixtureTypeId && d.BindProduct == YesNo.Yes)
                            .Where(p => p.Id == x.FixtureAccountId));
            else
                query.Exists<FixtureAccount>(
                        (x, y) => y.Join<FixtureEncode>((c, d) => c.FixtureEncodeId == d.Id)
                            .Join<FixtureEncode, FixtureEncodeProductDetail>((c, d) => c.Id == d.FixtureEncodeId && d.ItemId == itemId && d.Deck == (Deck)deckId)
                            .Join<FixtureEncode, FixtureModel>((c, d) => c.FixtureModelId == d.Id && d.FixtureTypeId == fixtureTypeId && d.BindProduct == YesNo.Yes)
                            .Where(p => p.Id == x.FixtureAccountId));
            if (warehouseId != null)
                query.Where(p => p.FixtureWarehouseId == warehouseId);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具类型和工治具仓库获取非绑定产品的工治具台帐的库存台帐列表
        /// </summary>
        /// <param name="fixtureTypeId">工治具类型</param>
        /// <param name="warehouseId">仓库Id</param>
        /// <returns>库存台帐列表</returns>
        public virtual EntityList<FixtureAccountStock> GetAccountStocksOfUnBindProduct(double fixtureTypeId, double? warehouseId)
        {
            var query = Query<FixtureAccountStock>().Where(p => p.PassQty > 0).Exists<FixtureAccount>(
                        (x, y) => y.Join<FixtureEncode>((c, d) => c.FixtureEncodeId == d.Id)
                            .Join<FixtureEncode, FixtureModel>((c, d) => c.FixtureModelId == d.Id && d.FixtureTypeId == fixtureTypeId && d.BindProduct == YesNo.No)
                            .Where(p => p.Id == x.FixtureAccountId));
            if (warehouseId != null)
                query.Where(p => p.FixtureWarehouseId == warehouseId);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具台账Id和库位Id获取库存详情
        /// </summary>
        /// <param name="fixtureAccountId">工治具台账Id</param>
        /// <param name="locationId">库位Id</param>
        /// <returns>库存详情</returns>
        public virtual FixtureAccountStock GetStockByIdCodeAndLocation(double fixtureAccountId, double locationId)
        {
            return Query<FixtureAccountStock>().Where(p => p.FixtureAccountId == fixtureAccountId && p.FixtureStorageLocationId == locationId)
                .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具台帐Id、仓库Id和库位Id获取库存详情
        /// </summary>
        /// <param name="accountId">工治具台帐I</param>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="locationId">库位Id</param>
        /// <returns>库存详情</returns>
        public virtual FixtureAccountStock GetFixtureAccountStock(double accountId, double warehouseId, double locationId)
        {
            return Query<FixtureAccountStock>().Where(p => p.FixtureAccountId == accountId && p.FixtureWarehouseId == warehouseId && p.FixtureStorageLocationId == locationId).FirstOrDefault();
        }

        /// <summary>
        /// 判断工治具台账Id和库位获取合格数大于0的库存
        /// </summary>
        /// <param name="accountId">工治具台账I</param>
        /// <param name="locationId">库位</param>
        /// <returns>true/false</returns>
        public virtual bool GetPassAccountStock(double accountId, double locationId)
        {
            return Query<FixtureAccountStock>().Where(p => p.FixtureAccountId == accountId && p.PassQty > 0 && p.FixtureStorageLocationId == locationId).Count() <= 0;
        }

        /// <summary>
        /// 根据工治具台账Id和库位获取合格数大于0的库存
        /// </summary>
        /// <param name="accountId">工治具台账I</param>
        /// <param name="locationId">库位</param>
        /// <returns>库存</returns>
        public virtual FixtureAccountStock GetPassStock(double accountId, double locationId)
        {
            return Query<FixtureAccountStock>().Where(p => p.FixtureAccountId == accountId &&
            p.FixtureStorageLocationId == locationId && p.PassQty > 0).FirstOrDefault();
        }

        /// <summary>
        /// 根据工治具台账Id和库位获取总数大于0的库存
        /// </summary>
        /// <param name="accountId">工治具台账I</param>
        /// <param name="locationId">库位</param>
        /// <returns>库存</returns>
        public virtual FixtureAccountStock GetTotalStock(double accountId, double? locationId = null)
        {
            var query = Query<FixtureAccountStock>().Where(p => p.FixtureAccountId == accountId && p.TotalQty > 0);
            if (locationId.HasValue)
                query.Where(p => p.FixtureStorageLocationId == locationId);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具台账Id和库位获取合格数大于0的库存
        /// </summary>
        /// <param name="accountId">工治具台账I</param>
        /// <param name="locationCode">库位编码</param>
        /// <returns>库存</returns>
        public virtual FixtureAccountStock GetTotalStock(double accountId, string locationCode)
        {
            return Query<FixtureAccountStock>().Exists<StorageLocation>((a, b) => b.Where(f => f.Id == a.FixtureStorageLocationId && f.Code == locationCode && a.FixtureAccountId == accountId && a.TotalQty > 0)).FirstOrDefault();
        }

        /// <summary>
        /// 获取ID类工治具台帐的库存台帐
        /// </summary>
        /// <param name="accountId">ID类工治具台帐Id</param>
        /// <returns>库存台帐</returns>
        public virtual FixtureAccountStock GetAccountStock(double accountId)
        {
            return Query<FixtureAccountStock>().Where(p => p.FixtureAccountId == accountId && p.PassQty > 0).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具编码Id和库位获取库存台账列表
        /// </summary>
        /// <param name="accountId">工治具台帐Id</param>
        /// <param name="locationCode">库位编码</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>库存台账列表</returns>
        public virtual EntityList<FixtureAccountStock> GetFixtureAccountStocks(double accountId, string locationCode, PagingInfo pagingInfo)
        {
            var query = Query<FixtureAccountStock>().Where(p => p.FixtureAccountId == accountId);
            if (locationCode.IsNotEmpty())
                query.Exists<StorageLocation>((c, d) => d.Where(m => m.Id == c.FixtureStorageLocationId && m.Code.Contains(locationCode)));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具台账Id获取feeder详情列表
        /// </summary>
        /// <param name="accountId">工治具台账Id</param>
        /// <returns>feeder详情列表</returns>
        public virtual EntityList<FixtureAccountTool> GetFixtureAccountTools(double accountId)
        {
            Expression<Func<FixtureAccountTool, bool>> exp = p => p.FixtureAccountId == accountId;
            return _commonController.GetDatas(exp);
        }

        /// <summary>
        /// 通过feeder详情标签编码获取对应台账的feeder详情列表
        /// </summary>
        /// <param name="labelCodes">feeder详情标签编码列表</param>
        /// <returns>feeder详情列表</returns>
        public virtual EntityList<FixtureAccountTool> GetFixtureAccountTools(List<string> labelCodes)
        {
            var exp = labelCodes.CreateContainsExpression<FixtureAccountTool>("x", FixtureAccountTool.LabelCodeProperty.Name);

            if (exp == null)
            {
                return new EntityList<FixtureAccountTool>();
            }

            return Query<FixtureAccountTool>().Where(exp).ToList();
        }

        /// <summary>
        /// 根据工治具台账Id获取使用履历列表
        /// </summary>
        /// <param name="accountId">工治具台账Id</param>
        /// <returns>使用履历列表</returns>
        public virtual EntityList<FixtureAccountUseResume> GetUseResumes(double accountId)
        {
            Expression<Func<FixtureAccountUseResume, bool>> exp = p => p.FixtureAccountId == accountId;
            return _commonController.GetDatas(exp);
        }

        /// <summary>
        /// 根据工治具台帐Id列表获取工治具台帐使用履历列表
        /// </summary>
        /// <param name="accountIds">工治具台帐Id列表</param>
        /// <param name="woIds">工单Id列表</param>
        /// <returns>工治具台帐使用履历列表</returns>
        public virtual EntityList<FixtureAccountUseResume> GetUseResumes(List<double> accountIds, List<double?> woIds)
        {
            //电子套件MES使用
            return Query<FixtureAccountUseResume>().Where(p => accountIds.Contains(p.FixtureAccountId) && woIds.Contains(p.WorkOrderId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具台账Id获取使用履历列表(带分页和排序)
        /// </summary>
        /// <param name="accountId">工治具台账Id</param>
        /// <param name="pagingInfo"></param>
        /// <param name="sortInfo"></param>
        /// <returns>使用履历列表</returns>
        public virtual EntityList<FixtureAccountUseResume> GetPagingSortUseResumes(double accountId, PagingInfo pagingInfo, List<OrderInfo> sortInfo)
        {
            Expression<Func<FixtureAccountUseResume, bool>> exp = p => p.FixtureAccountId == accountId;
            return _commonController.GetDatas<FixtureAccountUseResume>(exp, pagingInfo, new EagerLoadOptions().LoadWithViewProperty(), sortInfo);
        }

        /// <summary>
        /// 创建保存编码类工治具台账使用履历
        /// </summary>
        /// <param name="accountId">台账id</param>
        /// <param name="resourceId">资源id</param>
        /// <param name="workOrderId">工单id</param>
        /// <param name="type">操作类型</param>
        /// <param name="qty">操作数量</param>
        public virtual void CreateSaveAccountUseResume(double accountId, double? resourceId, double? workOrderId, UseResumeType type, int qty)
        {
            var useResume = new FixtureAccountUseResume();
            useResume.FixtureAccountId = accountId;
            useResume.ResourceId = resourceId;
            useResume.WorkOrderId = workOrderId;
            useResume.OperationById = RT.IdentityId;
            useResume.OperationTime = RF.Find<FixtureAccountUseResume>().GetDbTime();
            useResume.OperationType = type;
            useResume.OperationQty = qty;
            RF.Save(useResume);
        }

        /// <summary>
        /// 根据ID编码获取工治具台账
        /// </summary>
        /// <param name="code">ID编码</param>
        /// <returns>工治具台账</returns>
        public virtual FixtureAccount GetFixtureAccountByCode(string code)
        {
            return Query<FixtureAccount>().Where(p => p.Code == code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据RFID获取工治具台账
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="rfid">RFID</param>
        /// <returns>工治具台账</returns>
        public virtual FixtureAccount GetFixtureAccountByRfid(double id, string rfid)
        {
            return Query<FixtureAccount>().Where(p => p.Id != id && p.Rfid == rfid).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工治具编码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>

        public virtual FixtureEncode GetFixtureEncodeByCode(string code)
        {
            return Query<FixtureEncode>().Where(p => p.Code == code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据ID编码或RFID获取工治具台账
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual FixtureAccount GetFixtureAccountByCodeOrRFID(string code)
        {
            return Query<FixtureAccount>().Where(p => p.Code == code || p.Rfid == code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具台账Id获取工治具台账
        /// </summary>
        /// <param name="id">工治具台账Id</param>
        /// <returns>工治具台账</returns>
        public virtual FixtureAccount GetFixtureAccount(double id)
        {
            return Query<FixtureAccount>().Where(p => p.Id == id).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据ID编码列表获取工治具台账列表
        /// </summary>
        /// <param name="codes">ID编码列表</param>
        /// <returns>工治具台账列表</returns>
        public virtual EntityList<FixtureAccount> GetFixtureAccountsByCodes(List<string> codes)
        {
            return Query<FixtureAccount>().Where(p => codes.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据台账Id列表获取台账列表
        /// </summary>
        /// <param name="accountIds">台账Id列表</param>
        /// <returns>台账列表</returns>
        public virtual EntityList<FixtureAccount> GetFixtureAccounts(List<double> accountIds)
        {
            return accountIds.SplitContains(tempIds =>
            {
                return Query<FixtureAccount>().Where(p => tempIds.Contains(p.Id))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取工治具台帐列表在线/在库的ID类工治具台帐列表
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public virtual EntityList<FixtureAccount> GetFixtureAccounts(PagingInfo pageInfo, string keyWord)
        {
            //找在线在库的Id工治具和所有编码工治具
            var query = Query<FixtureAccount>()
                   .LeftJoin<FixtureEncode>("code", (a, c) => a.FixtureEncodeId == c.Id)
                   .LeftJoin<FixtureEncode, FixtureModel>("model", (c, m) => c.FixtureModelId == m.Id)
                   .Where<FixtureModel>((a, m) => (m.ManageMode == ManageMode.Number && (a.AccountState == FixtureAccountState.InStorage || a.AccountState == FixtureAccountState.Online)) || m.ManageMode == ManageMode.Code);
            if (keyWord.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyWord));
            }
            return query.ToList(pageInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据治具编码Id获取台账列表
        /// </summary>
        /// <param name="encodeCodeId">治具编码Id</param>
        /// <returns>台账列表</returns>
        public virtual EntityList<FixtureAccount> GetAccountsByEncodeId(double encodeCodeId)
        {
            return Query<FixtureAccount>().Where(p => p.FixtureEncodeId == encodeCodeId).ToList();
        }

        /// <summary>
        /// 根据工治具台帐Id列表、工单的产品和工艺面获取满足条件的工治具台帐列表
        /// </summary>
        /// <param name="accountIds">工治具台帐Id列表</param>
        /// <param name="productId">产品Id</param>
        /// <param name="deck">工艺面</param>
        /// <returns>满足条件的工治具台帐列表</returns>
        public virtual EntityList<FixtureAccount> GetSatisfiedAccounts(List<double> accountIds, double? productId, Deck? deck)
        {
            if (productId.HasValue && deck.HasValue)
                return accountIds.SplitContains((tempTaskIds) =>
                {
                    return Query<FixtureAccount>().Exists<FixtureEncode>((x, y) => y.Join<FixtureEncodeProductDetail>((c, d) => c.Id == d.FixtureEncodeId && d.ItemId == productId && d.Deck == deck).Where(p => p.Id == x.FixtureEncodeId && tempTaskIds.Contains(x.Id))).ToList();
                });
            else if (productId.HasValue)
                return accountIds.SplitContains((tempTaskIds) =>
                {
                    return Query<FixtureAccount>().Exists<FixtureEncode>((x, y) => y.Join<FixtureEncodeProductDetail>((c, d) => c.Id == d.FixtureEncodeId && d.ItemId == productId).Where(p => p.Id == x.FixtureEncodeId && tempTaskIds.Contains(x.Id))).ToList();
                });
            else
                return accountIds.SplitContains((tempTaskIds) =>
                {
                    return Query<FixtureAccount>().Exists<FixtureEncode>((x, y) => y.Join<FixtureEncodeProductDetail>((c, d) => c.Id == d.FixtureEncodeId && d.Deck == deck).Where(p => p.Id == x.FixtureEncodeId && tempTaskIds.Contains(x.Id))).ToList();
                });
        }
        #endregion

        #region 工治具台账（编码管理）
        /// <summary>
        /// 获取工治具台账
        /// Expression不支持序列号，前端不允许调用
        /// </summary>
        /// <param name="exp">条件</param>
        /// <returns>工治具台账</returns>
        public virtual FixtureCodeAccount GetFixtureCodeAccount(Expression<Func<FixtureCodeAccount, bool>> exp)
        {
            var query = Query<FixtureCodeAccount>();
            if (exp != null)
            {
                query.Where(exp);
            }
            return query.FirstOrDefault(new EagerLoadOptions().LoadWith(FixtureCodeAccount.CustomerProperty).LoadWith(FixtureCodeAccount.SupplierProperty));
        }

        /// <summary>
        /// 通过查询条件获取工治具型号列表
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns>工治具型号列表</returns>
        public virtual EntityList<FixtureCodeAccount> GetFixtureCodeAccountList(FixtureCodeAccountCriteria criteria)
        {
            var query = Query<FixtureCodeAccount>();
            if (criteria.Code.IsNotEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (criteria.EncodeCode.IsNotEmpty() || criteria.ModelCode.IsNotEmpty() || criteria.ModelName.IsNotEmpty() || criteria.FixtureTypeId.HasValue)
            {
                query.Exists<FixtureEncode>((x, y) =>
                        y.Join<FixtureModel>((c, d) => c.FixtureModelId == d.Id)
                        .Where(p => p.Id == x.FixtureEncodeId)
                .WhereIf(criteria.EncodeCode.IsNotEmpty(), c => c.Code.Contains(criteria.EncodeCode))
                 .WhereIf<FixtureModel>(criteria.ModelCode.IsNotEmpty(), (c, d) => d.Code.Contains(criteria.ModelCode))
                    .WhereIf<FixtureModel>(criteria.ModelName.IsNotEmpty(), (c, d) => d.Name.Contains(criteria.ModelName))
                    .WhereIf<FixtureModel>(criteria.FixtureTypeId.HasValue, (c, d) => d.FixtureTypeId == criteria.FixtureTypeId));
            }

            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具台帐Id列表获取在库/在线数量大于零的编码类工治具台帐列表
        /// </summary>
        /// <param name="ids">工治具台帐Id列表</param>
        /// <returns>编码类工治具台帐列表</returns>
        public virtual EntityList<FixtureCodeAccount> GetFixtureCodeAccounts(List<double> ids)
        {
            return ids.SplitContains((tempTaskIds) =>
            {
                return Query<FixtureCodeAccount>().Where(p => tempTaskIds.Contains(p.Id) && (p.InStockQty > 0 || p.OnlineQty > 0)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 根据工治具台帐Id列表获取在库数量大于零的编码类工治具台帐列表
        /// </summary>
        /// <param name="ids">工治具台帐Id列表</param>
        /// <returns>编码类工治具台帐列表</returns>
        public virtual EntityList<FixtureCodeAccount> GetInStockCodeAccounts(List<double> ids)
        {
            return ids.SplitContains((tempTaskIds) =>
            {
                return Query<FixtureCodeAccount>().Where(p => tempTaskIds.Contains(p.Id) && p.InStockQty > 0).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 根据工治具台帐Id列表获取在线数量大于零的编码类工治具台帐列表
        /// </summary>
        /// <param name="ids">工治具台帐Id列表</param>
        /// <returns>编码类工治具台帐列表</returns>
        public virtual EntityList<FixtureCodeAccount> GetOnlineCodeAccounts(List<double> ids)
        {
            return ids.SplitContains((tempTaskIds) =>
            {
                return Query<FixtureCodeAccount>().Where(p => tempTaskIds.Contains(p.Id) && p.OnlineQty > 0).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 根据工治具编码Id获取工治具类台账信息
        /// </summary>
        /// <param name="encodeId">工治具编码Id</param>
        /// <returns>工治具类台账信息</returns>
        public virtual AddCodeAccInfo GetAddCodeAccInfo(double encodeId)
        {
            var addInfo = new AddCodeAccInfo();
            var encode = RF.GetById<FixtureEncode>(encodeId);
            var isExist = IsExistMaintainProject(encodeId);
            if (isExist)
            {
                addInfo.State = FixtureAccountState.WaitMaintain;
            }
            else
            {
                addInfo.State = FixtureAccountState.WaitShelf;
            }
            var account = GetFixtureCodeAccount(p => p.Code == encode.Code);
            if (account != null)
            {
                addInfo.Account = account;
                addInfo.Account.CustomerCode = account.Customer?.Code;
                addInfo.Account.CustomerName = account.Customer?.Name;
                addInfo.Account.SupplierCode = account.Supplier?.Code;
                addInfo.Account.SupplierName = account.Supplier?.Name;
            }

            return addInfo;
        }

        /// <summary>
        /// 保存编码类工治具台帐
        /// </summary>
        /// <param name="account">界面编辑的工治具台帐</param>
        /// <returns>错误信息</returns>
        public virtual string SaveFixtureCodeAccount(FixtureCodeAccount account)
        {
            string errMsg = string.Empty;
            try
            {
                ValidateDatas(account);
                FixtureAccount modifyAccount = GetFixtureAccountByCode(account.Code);
                //产权归属
                //1、保存后，根据工治具编码获取工治具编码台账，总数和待入库增加；台账没有时，新增这个台账数据
                //2、生成工治具入库单，入库类型为【添加台账入库】要新增这个枚举。同时看看入库功能能不能正常操作
                if (modifyAccount != null)
                {
                    modifyAccount.Proprietorship = account.Proprietorship;
                    //总数,合格数,待入库数增加数量
                    modifyAccount.TotalQty += account.TotalQty;
                    modifyAccount.PassQty += account.TotalQty;
                    modifyAccount.WaitShelfQty += account.TotalQty;
                }
                else
                {
                    //不存在则新建
                    modifyAccount = new FixtureAccount();
                    modifyAccount.GenerateId();
                    modifyAccount.FixtureEncodeId = account.FixtureEncodeId;
                    modifyAccount.Proprietorship = account.Proprietorship;
                    modifyAccount.CustomerId = account.CustomerId;
                    modifyAccount.SupplierId = account.SupplierId;
                    modifyAccount.Code = account.Code;
                    modifyAccount.WarehouseId = account.WarehouseId;
                    modifyAccount.TotalQty = account.TotalQty;
                    modifyAccount.PassQty = account.TotalQty;
                    modifyAccount.WaitShelfQty = account.TotalQty;
                }

                //2、生成工治具入库单，入库类型为【台账入库】要新增这个枚举。同时看看入库功能能不能正常操作
                InboundOrder order = CreateInboundOrder(account);
                using (var tran = DB.TransactionScope(KitFixturesEntityDataProvider.ConnectionStringName))
                {
                    RF.Save(order);
                    RF.Save(modifyAccount);
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return errMsg;
        }

        /// <summary>
        /// 保存编码类工治具台账
        /// </summary>
        /// <param name="fixtureAccVM">编码类工治具台账ViewModel</param>
        /// <returns>错误信息</returns>
        public virtual string SaveFixtureCodeAccount(FixtureAccountViewModel fixtureAccVM)
        {
            string errMsg = string.Empty;

            try
            {
                ValidateDatas(fixtureAccVM);
                var encodeId = fixtureAccVM.FixtureEncodeId;
                var encode = GetFixtureEncode(encodeId);
                var account = GetFixtureCodeAccount(fixtureAccVM, encode);
                var maintainPrjs = GetFixtureEncodeMaintainProjects(encodeId);
                if (maintainPrjs.Any())
                {
                    account.WaitMaintain += fixtureAccVM.Qty;
                }
                else
                {
                    account.WaitShelfQty += fixtureAccVM.Qty;
                }
                account.TotalQty += fixtureAccVM.Qty;

                using (var tran = DB.TransactionScope(KitFixturesEntityDataProvider.ConnectionStringName))
                {
                    SaveRelateTasks(fixtureAccVM, account, encode, maintainPrjs);
                    RF.Save(account);
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return errMsg;
        }

        /// <summary>
        /// 创建工治具入库单
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public virtual InboundOrder CreateInboundOrder(FixtureCodeAccount account)
        {
            var now = RF.Find<FixtureCodeAccount>().GetDbTime();

            InboundOrder order = new InboundOrder();
            order.GenerateId();
            order.No = RT.Service.Resolve<CommonController>().GetNo<InboundOrder>("工治具入库单号");
            order.InboundType = FixtureInboundType.AccountIn;
            order.Qty = account.TotalQty;
            order.InboundStatus = InboundStatus.ToBe;
            order.FixtureEncodeId = account.FixtureEncodeId;
            order.WarehouseId = account.WarehouseId;
            order.SupplierId = account.SupplierId;
            order.CustomerId = account.CustomerId;
            order.InboundDate = now;
            order.Proprietorship = account.Proprietorship;
            order.QualityState = FixtureQualityState.Pass;
            return order;
        }


        /// <summary>
        /// 保存台账相关任务（入库单、保养任务）
        /// </summary>
        /// <param name="fixtureAccVM">工治具台账ViewModel</param>
        /// <param name="account">工治具台账</param>
        /// <param name="encode">工治具编码</param>
        /// <param name="maintainPrjs">入库保养项目列表</param>
        protected virtual void SaveRelateTasks(FixtureAccountViewModel fixtureAccVM, FixtureAccount account, FixtureEncode encode, EntityList<FixtureEncodeMaintainProject> maintainPrjs)
        {
        }

        /// <summary>
        /// 验证工治具领用记录表里，该工治具对应的工单与当前已选工单是否一致
        /// </summary>
        /// <param name="accountId">工治具台账ID</param>
        /// <param name="workOrderId">工单ID</param>
        public virtual void CheckingWorkOrder(double accountId, double workOrderId)
        {
        }

        /// <summary>
        /// 验证数据是否合法
        /// </summary>
        /// <param name="fixtureAccVM">工治具台账ViewModel</param>
        private void ValidateDatas(FixtureAccountViewModel fixtureAccVM)
        {
            if (fixtureAccVM.FixtureEncodeId <= 0)
            {
                throw new ValidationException("工治具编码必填！".L10N());
            }
            if (fixtureAccVM.Code.IsNullOrEmpty())
            {
                throw new ValidationException("ID编码必填！".L10N());
            }
            if (fixtureAccVM.Qty <= 0)
            {
                throw new ValidationException("数量必填！".L10N());
            }
            if (fixtureAccVM.Proprietorship == Proprietorship.Lease && fixtureAccVM.SupplierId == null)
            {
                throw new ValidationException("资产类型为【租凭】时供应商编码不能为空！".L10N());
            }
            if (fixtureAccVM.Proprietorship == Proprietorship.ByCustomer && fixtureAccVM.CustomerId == null)
            {
                throw new ValidationException("资产类型为【客供】时客户编码不能为空！".L10N());
            }
            var now = RF.Find<FixtureAccount>().GetDbTime();
            if (fixtureAccVM.ProductDate.HasValue && fixtureAccVM.ProductDate.Value > now)
            {
                throw new ValidationException("【生产日期】不可以大于当前日期!".L10N());
            }
        }

        /// <summary>
        /// 验证数据是否合法
        /// </summary>
        /// <param name="account">工治具台帐</param>
        private void ValidateDatas(FixtureAccount account)
        {
            if (account.TotalQty < 1)
            {
                throw new ValidationException("数量须大于0！".L10N());
            }
            if (!account.WarehouseId.HasValue)
            {
                throw new ValidationException("仓库不能为空！".L10N());
            }
            if (account.Proprietorship == Proprietorship.Lease && account.SupplierId == null)
            {
                throw new ValidationException("资产类型为【租凭】时供应商编码不能为空！".L10N());
            }
            if (account.Proprietorship == Proprietorship.ByCustomer && account.CustomerId == null)
            {
                throw new ValidationException("资产类型为【客供】时客户编码不能为空！".L10N());
            }
        }
        
        /// <summary>
        /// 获取工治具台账（编码管理）
        /// </summary>
        /// <param name="fixtureAccVM">工治具台账ViewModel</param>
        /// <param name="encode">工治具编码</param>
        /// <returns>工治具台账（编码管理）</returns>
        private FixtureAccount GetFixtureCodeAccount(FixtureAccountViewModel fixtureAccVM, FixtureEncode encode)
        {
            FixtureAccount account = GetFixtureCodeAccount(p => p.Code == encode.Code);
            if (account == null)
            {
                account = new FixtureAccount();
                account.Code = encode.Code;
                account.FixtureEncodeId = encode.Id;
                account.GenerateId();
            }

            account.OriginalSN = fixtureAccVM.OriginalSN;
            account.AssetCode = fixtureAccVM.AssetCode;
            account.Proprietorship = fixtureAccVM.Proprietorship.Value;
            account.Manufacturer = fixtureAccVM.Manufacturer;
            if (fixtureAccVM.SupplierId.HasValue)
            {
                account.SupplierId = fixtureAccVM.SupplierId;
            }
            if (fixtureAccVM.CustomerId.HasValue)
            {
                account.CustomerId = fixtureAccVM.CustomerId;
            }
            return account;
        }

        #endregion

        #region 工治具台账（ID管理）

        /// <summary>
        /// 通过查询条件获取工治具台账（ID管理）列表
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns>工治具台账（ID管理）列表</returns>
        public virtual EntityList<FixtureIDAccount> GetFixtureIDAccounts(FixtureIDAccountCriteria criteria)
        {
            var query = Query<FixtureIDAccount>();
            if (criteria.Code.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(criteria.Code));
            }
            if (criteria.AccountState.HasValue)
            {
                query.Where(p => p.AccountState == criteria.AccountState);
            }
            if (criteria.QualityState.HasValue)
            {
                query.Where(p => p.QualityState == criteria.QualityState);
            }
            if (criteria.FixtureEncodeCode.IsNotEmpty() || criteria.ModelCode.IsNotEmpty() || criteria.ModelName.IsNotEmpty() || criteria.FixtureTypeId.HasValue || criteria.IsExceed.HasValue)
            {
                query.Exists<FixtureEncode>((x, y) => y.Join<FixtureModel>((e, d) => e.FixtureModelId == d.Id)
                .Where(p => p.Id == x.FixtureEncodeId)
                .WhereIf<FixtureModel>(criteria.FixtureEncodeCode.IsNotEmpty(), (e, d) => e.Code.Contains(criteria.FixtureEncodeCode))
                .WhereIf<FixtureModel>(criteria.ModelCode.IsNotEmpty(), (e, d) => d.Code.Contains(criteria.ModelCode))
                .WhereIf<FixtureModel>(criteria.ModelName.IsNotEmpty(), (e, d) => d.Name.Contains(criteria.ModelName))
                .WhereIf<FixtureModel>(criteria.IsExceed == YesNo.Yes, (e, d) => x.TotalUseNum > d.MaxUseNum && x.TotalUseHour > d.MaxUseHour)
                .WhereIf<FixtureModel>(criteria.IsExceed == YesNo.No, (e, d) => x.TotalUseNum <= d.MaxUseNum && x.TotalUseHour <= d.MaxUseHour)
                .WhereIf<FixtureModel>(criteria.FixtureTypeId.HasValue, (e, d) => d.FixtureTypeId == criteria.FixtureTypeId));

            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具台帐Id列表获取在线/在库/使用中的ID类工治具台帐列表
        /// </summary>
        /// <param name="ids">工治具台帐Id列表</param>
        /// <returns>ID类工治具台帐列表</returns>
        public virtual EntityList<FixtureIDAccount> GetFixtureIDAccounts(List<double> ids)
        {
            return ids.SplitContains((tempTaskIds) =>
            {
                return Query<FixtureIDAccount>()
                .Where(p => tempTaskIds.Contains(p.Id)
                && (p.AccountState == FixtureAccountState.Online
                    || p.AccountState == FixtureAccountState.Using
                    || p.AccountState == FixtureAccountState.InStorage))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 根据工治具台帐Id列表获取在库的ID类工治具台帐列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>在库的ID类工治具台帐列表</returns>
        public virtual EntityList<FixtureIDAccount> GetInStockIDAccounts(List<double> ids)
        {
            return ids.SplitContains((tempTaskIds) =>
            {
                return Query<FixtureIDAccount>()
                .Where(p => tempTaskIds.Contains(p.Id) && p.AccountState == FixtureAccountState.InStorage)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 根据工治具台帐Id列表获取在线/使用中的ID类工治具台帐列表
        /// </summary>
        /// <param name="ids">工治具台帐Id列表</param>
        /// <returns>ID类工治具台帐列表</returns>
        public virtual EntityList<FixtureIDAccount> GetOnlineIDAccounts(List<double> ids)
        {
            return ids.SplitContains((tempTaskIds) =>
            {
                return Query<FixtureIDAccount>()
                .Where(p => tempTaskIds.Contains(p.Id) && (p.AccountState == FixtureAccountState.Online || p.AccountState == FixtureAccountState.Using))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 根据工治具编码获取工治具仓库(编码下无维护取所有仓库)
        /// </summary>
        /// <param name="fixtureEncodeId">工治具编码ID</param>
        /// <param name="keyWord">编码或者名称</param>
        /// <param name="pageInfo">分页参数</param>
        /// <returns>工治具仓库列表</returns>
        public virtual EntityList<Warehouse> GetWarehousesByEncodeId(double fixtureEncodeId, string keyWord, PagingInfo pageInfo)
        {
            var warehouseIds = Query<FixtureEncodeStorageLocation>().Where(p => p.FixtureEncodeId == fixtureEncodeId)
                .Select(p => p.WarehouseId).ToList<double>().Distinct();
            var query = Query<Warehouse>();
            if (keyWord.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyWord) || p.Name.Contains(keyWord));
            }
            return query.WhereIf(warehouseIds.Any(), p => warehouseIds.Contains(p.Id)).ToList(pageInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具编码获取工治具库位(编码下无维护取仓库下库位)
        /// </summary>
        /// <param name="encodeId">工治具编码ID</param>
        /// <param name="warehouseId">工治具仓库ID</param>
        /// <param name="keyWord">编码或者名称</param>
        /// <param name="pageInfo">分页参数</param>
        /// <returns>工治具库位列表</returns>
        public virtual EntityList<StorageLocation> GetStorageLocationsByEncodeId(double encodeId, double warehouseId, string keyWord, PagingInfo pageInfo)
        {
            var locationIds = Query<FixtureEncodeStorageLocation>().Where(p => p.FixtureEncodeId == encodeId && p.WarehouseId == warehouseId)
                .Select(p => p.StorageLocationId).ToList<double>();
            var query = Query<StorageLocation>();
            if (keyWord.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyWord) || p.Name.Contains(keyWord));
            }
            query.Where(p => p.WarehouseId == warehouseId);
            return query.WhereIf(locationIds.Any(), p => locationIds.Contains(p.Id)).ToList(pageInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 该库位是否被其他工治具关联占用
        /// </summary>
        /// <param name="locationId">库位ID</param>
        /// <returns>已关联工治具ID编码</returns>
        public virtual string IsExistAccountLocation(double locationId)
        {
            return Query<FixtureAccount>().Where(p => p.LocationId == locationId).Select(p => p.Code).FirstOrDefault()?.Code;
        }

        /// <summary>
        /// 获取工治具库位
        /// </summary>
        /// <param name="locationCode">编码</param>
        /// <returns>工治具库位</returns>
        public virtual StorageLocation GetFixtureStorageLocation(string locationCode)
        {
            return Query<StorageLocation>().Where(p => p.Code == locationCode).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 验证保存ID类工治具台账
        /// </summary>
        /// <param name="model">工治具台账ViewModel</param>
        /// <returns>工治具编码</returns>
        private FixtureEncode ValidationSaveIDAccount(FixtureAccountViewModel model)
        {
            if (model.Code.IsNullOrWhiteSpace())
            {
                throw new ValidationException("【ID编码】不能为空!".L10N());
            }
            var fixtureEncode = GetFixtureEncode(model.FixtureEncodeId);
            if (fixtureEncode == null)
            {
                throw new ValidationException("【工治具编码】不能为空!".L10N());
            }
            if (fixtureEncode.FixedStorage == YesNo.Yes && (model.FixtureWarehouseId == null || model.FixtureStorageLocationId == null))
            {
                throw new ValidationException("当【固定储位】为是时,必须维护【仓库编码】和【库位编码】!".L10N());
            }
            if (model.Proprietorship == null)
            {
                throw new ValidationException("【产权归属】不能为空!".L10N());
            }
            if (model.Proprietorship == Proprietorship.Lease && model.SupplierId == null)
            {
                throw new ValidationException("产权归属为【租凭】时供应商编码不能为空！".L10N());
            }
            if (model.Proprietorship == Proprietorship.ByCustomer && model.CustomerId == null)
            {
                throw new ValidationException("产权归属为【客供】时客户编码不能为空！".L10N());
            }
            var now = RF.Find<FixtureAccount>().GetDbTime();
            if (model.ProductDate.HasValue && model.ProductDate.Value > now)
            {
                throw new ValidationException("【生产日期】不可以大于当前日期!".L10N());
            }
            return fixtureEncode;
        }

        /// <summary>
        /// 验证保存ID类工治具台账
        /// </summary>
        /// <param name="model">工治具台账ViewModel</param>
        /// <returns>工治具编码</returns>
        private void ValidationSaveIDAccount(FixtureIDAccount model)
        {
            if (model.Code.IsNullOrWhiteSpace())
            {
                throw new ValidationException("【ID编码】不能为空!".L10N());
            }
            var fixtureEncode = GetFixtureEncode(model.FixtureEncodeId);
            if (fixtureEncode == null)
            {
                throw new ValidationException("【工治具编码】不能为空!".L10N());
            }
            if (model.WarehouseId == null)
            {
                throw new ValidationException("【仓库编码】不能为空!".L10N());
            }
            if (model.LocationId == null)
            {
                throw new ValidationException("【库位编码】不能为空!".L10N());
            }
            if (model.Proprietorship == Proprietorship.Lease && model.SupplierId == null)
            {
                throw new ValidationException("产权归属为【租凭】时供应商编码不能为空！".L10N());
            }
            if (model.Proprietorship == Proprietorship.ByCustomer && model.CustomerId == null)
            {
                throw new ValidationException("产权归属为【客供】时客户编码不能为空！".L10N());
            }
            var now = RF.Find<FixtureAccount>().GetDbTime();
            if (model.ProductionDate.HasValue && model.ProductionDate.Value > now)
            {
                throw new ValidationException("【生产日期】不可以大于当前日期!".L10N());
            }
        }

        /// <summary>
        /// 创建保存一个ID类工治具台账
        /// </summary>
        /// <param name="model">工治具台账ViewModel</param>
        /// <returns>ID类工治具台账</returns>
        private FixtureAccount CreateFixtureIDAccount(FixtureAccountViewModel model)
        {
            var fixtureIDAccount = new FixtureAccount();
            fixtureIDAccount.GenerateId();
            fixtureIDAccount.Code = model.Code;
            fixtureIDAccount.TotalQty = 1;
            fixtureIDAccount.OriginalSN = model.OriginalSN;
            fixtureIDAccount.AssetCode = model.AssetCode;
            fixtureIDAccount.ProductionDate = model.ProductDate;
            fixtureIDAccount.Manufacturer = model.Manufacturer;
            fixtureIDAccount.Proprietorship = model.Proprietorship.Value;
            fixtureIDAccount.FixtureEncodeId = model.FixtureEncodeId;
            fixtureIDAccount.CustomerId = model.CustomerId;
            fixtureIDAccount.AccountState = model.AccountState;
            fixtureIDAccount.SupplierId = model.SupplierId;
            return fixtureIDAccount;
        }

        /// <summary>
        /// 保存ID类工治具台账
        /// </summary>
        /// <param name="model">工治具台账ViewModel</param>
        public virtual void SaveFixtureIDAccount(FixtureAccountViewModel model)
        {
            var fixtureEncode = ValidationSaveIDAccount(model);
            var fixtureIDAccount = CreateFixtureIDAccount(model);
            var maintainPrjs = GetFixtureEncodeMaintainProjects(model.FixtureEncodeId);
            if (!maintainPrjs.Any())
            {
                fixtureIDAccount.QualityState = FixtureQualityState.Pass;
            }
            if (fixtureEncode.FixedStorage == YesNo.Yes)
            {
                if (model.FixtureWarehouseId.HasValue)
                {
                    fixtureIDAccount.WarehouseId = model.FixtureWarehouseId.Value;
                }
                if (model.FixtureStorageLocationId.HasValue)
                {
                    fixtureIDAccount.LocationId = model.FixtureStorageLocationId.Value;
                }
            }

            using (var trans = DB.TransactionScope(KitFixturesEntityDataProvider.ConnectionStringName))
            {
                SaveRelateTasks(model, fixtureIDAccount, fixtureEncode, maintainPrjs);
                RF.Save(fixtureIDAccount);
                //if (fixtureEncode.FixtureType == FixtureType.Feeder) //Todo 飞达
                //    CreateFeeders(fixtureEncode.SlotType, model.Code, fixtureIDAccount.Id);
                trans.Complete();
            }
        }

        /// <summary>
        /// 修改保存ID类工治具台账
        /// </summary>
        /// <param name="account">工治具台账</param>
        public virtual void EditSaveFixtureIDAccount(FixtureIDAccount account)
        {
            //验证ID类工治具台账数据
            ValidationSaveIDAccount(account);
            //新增工治具台账 
            account.GenerateId();
            account.QualityState = FixtureQualityState.Pass;
            account.AccountState = FixtureAccountState.InStorage;

            FixtureAccountStock stock = new FixtureAccountStock();
            stock.FixtureWarehouseId = account.WarehouseId.Value;
            stock.FixtureStorageLocationId = account.LocationId.Value;
            stock.TotalQty = 1;
            stock.PassQty = 1;
            stock.NgQty = 0;
            stock.ScrapQty = 0;

            account.StockList.Add(stock);
            using (var trans = DB.TransactionScope(KitFixturesEntityDataProvider.ConnectionStringName))
            {
                RF.Save(account);
                trans.Complete();
            }
        }

        /// <summary>
        /// 通过feeder详情标签编码获取对应的ID类工治具台账
        /// </summary>
        /// <param name="labelCode">feeder详情标签编码</param>
        /// <returns>ID类工治具台账</returns>
        public virtual FixtureIDAccount GetFixtureAccountByLabel(string labelCode)
        {
            return Query<FixtureIDAccount>().Exists<FixtureAccountTool>((a, b) => b.Where(f => f.FixtureAccountId == a.Id && f.LabelCode == labelCode)).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过feeder详情标签编码获取对应台账的feeder详情列表
        /// </summary>
        /// <param name="labelCode">feeder详情标签编码</param>
        /// <returns>feeder详情列表</returns>
        public virtual EntityList<FixtureAccountTool> FixtureAccountTools(string labelCode)
        {
            return Query<FixtureAccountTool>().As("t").Exists<FixtureIDAccount>(
                    (t, y) => y.Join<FixtureAccountTool>("tl", (c, tl) => c.Id == tl.FixtureAccountId && tl.LabelCode == labelCode)
                        .Where(p => p.Id == t.FixtureAccountId)).ToList();
        }



        /// <summary>
        /// 更新feeder状态
        /// </summary>
        /// <param name="fixtureAccount">ID类工治具台账</param>
        /// <param name="accountState">状态</param>
        public virtual void UpdateFixtureAccountAccountState(FixtureAccount fixtureAccount, FixtureAccountState accountState)
        {
            fixtureAccount.AccountState = accountState;
            RF.Save(fixtureAccount);
        }

        /// <summary>
        /// 插入feeder履历
        /// </summary>
        /// <param name="resourceId">产线Id</param>
        /// <param name="equipId">设备Id</param>
        /// <param name="woId">工单Id</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="subarea">分区</param>
        /// <param name="stance">站位</param>
        /// <param name="fixtureAccountId">工治具台账Id</param>
        public virtual void InsertUseResume(double resourceId, double equipId, double woId, double itemId, string subarea, string stance, double fixtureAccountId)
        {
            var now = RF.Find<FixtureAccountUseResume>().GetDbTime();
            FixtureAccountUseResume useResume = new FixtureAccountUseResume();
            useResume.FixtureAccountId = fixtureAccountId;
            useResume.Stance = stance;
            useResume.Subarea = subarea;
            useResume.ItemId = itemId;
            useResume.WorkOrderId = woId;
            useResume.EquipAccountId = equipId;
            useResume.ResourceId = resourceId;
            useResume.OnlineDate = now;
            useResume.OperationById = RT.IdentityId;
            useResume.OperationTime = now;
            useResume.OperationType = UseResumeType.OnOffline;
            useResume.OperationQty = 1;
            RF.Save(useResume);
        }

        /// <summary>
        /// 更新feeder履历
        /// </summary>
        /// <param name="resourceId">产线Id</param>
        /// <param name="equipId">设备Id</param>
        /// <param name="woId">工单Id</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="subarea">分区</param>
        /// <param name="stance">站位</param>
        /// <param name="fixtureAccountId">工治具台账Id</param>
        public virtual void UpdateUseResume(double resourceId, double equipId, double woId, double itemId, string subarea, string stance, double fixtureAccountId)
        {
            var now = RF.Find<FixtureAccountUseResume>().GetDbTime();
            DB.Update<FixtureAccountUseResume>().Set(s => s.OfflineDate, now)
                .Where(w => w.FixtureAccountId == fixtureAccountId
                && w.Stance == stance
                && w.Subarea == subarea
                && w.ItemId == itemId
                && w.WorkOrderId == woId
                && w.EquipAccountId == equipId
                && w.ResourceId == resourceId
                && w.OfflineDate == null).Execute();
        }

        /// <summary>
        /// 获取所有工治具类型信息
        /// </summary>
        /// <returns>返回工治具类型信息</returns>
        public virtual List<FixtureIDAccountData> GetAllFixtureIDAccountData()
        {
            return Query<FixtureIDAccount>().Join<FixtureEncode>((a, b) => a.FixtureEncodeId == b.Id)
                .Select<FixtureEncode>((p, q) => new { AccountId = p.Id, AccountCode = p.Code, AccountName = p.Code, FixtureId = q.Id }).ToList<FixtureIDAccountData>().ToList();
        }
        #endregion


        /// <summary>
        /// 获取审批流程配置
        /// </summary>
        /// <returns>审批流程配置</returns>
        public virtual string GetFixtureIDAccountNo()
        {
            var config = ConfigService.GetConfig(new NoConfig(), typeof(FixtureIDAccount));
            if (config == null || config.BacodeRule == null)
            {
                throw new ValidationException("未找到【工治具ID】生成规则,请检查规则配置".L10N());
            }
            return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(Convert.ToDouble(config.NumberRuleId), 1).FirstOrDefault();
        }
    }
}
