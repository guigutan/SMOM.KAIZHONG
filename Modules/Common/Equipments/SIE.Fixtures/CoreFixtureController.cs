using SIE.Common.Catalogs;
using SIE.Common.Configs;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Fixtures.Fixtures.Abnormals;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.FixtureTypes;
using SIE.Fixtures.Models;
using SIE.Fixtures.Models.Config;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Fixtures
{
    /// <summary>
    /// 工治具台账控制器
    /// </summary>
    public partial class CoreFixtureController : DomainController
    {
        /// <summary>
        /// 通用控制器
        /// </summary>
        private static CommonController _commonController = RT.Service.Resolve<CommonController>();

        /// <summary>
        /// 获取工治具异常类型列表
        /// </summary>
        /// <param name="criteria">工治具异常类型查询体</param>
        /// <returns>工治具异常类型列表</returns>
        public virtual EntityList<FixtureAbnormal> GetFixtureAbnormalsByCriteria(FixtureAbnormalCriteria criteria)
        {
            var query = Query<FixtureAbnormal>();
            if (criteria.Code.IsNotEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (criteria.Description.IsNotEmpty())
                query.Where(p => p.Description.Contains(criteria.Description));
            if (criteria.FixtureType!=null)
                query.Where(p => p.FixtureTypeId == criteria.FixtureTypeId);
            if (criteria.AbnormalType.HasValue)
                query.Where(p => p.AbnormalType == criteria.AbnormalType);
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据仓库编码获取工治具仓库
        /// </summary>
        /// <param name="code">仓库编码</param>
        /// <returns>工治具仓库</returns>
        public virtual Warehouse GetFixtureWarehouse(string code)
        {
            return Query<Warehouse>().Where(p => p.Code == code).FirstOrDefault();
        }

        /// <summary>
        /// 判断工治具台帐的工治具编码下是否维护存储位置
        /// </summary>
        /// <param name="code">工治具ID</param>
        /// <returns>true/false</returns>
        public virtual bool IsExistEncodeStorageLocation(string code)
        {
            return Query<FixtureEncodeStorageLocation>().Exists<FixtureEncode>(
                    (x, y) => y.Join<FixtureAccount>((c, d) => c.Id == d.FixtureEncodeId && (d.Code == code|| c.Code == code))
                        .Where(p => p.Id == x.FixtureEncodeId)).Count() > 0;
        }

        /// <summary>
        /// 根据库位编码获取工治具库位
        /// </summary>
        /// <param name="locationCode">库位编码</param>
        /// <param name="warehouseId"></param>
        /// <returns>工治具库位</returns>
        public virtual StorageLocation GetStorageLocation(string locationCode,double warehouseId)
        {
            return Query<StorageLocation>().Where(p => p.Code == locationCode&&p.WarehouseId== warehouseId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据库位Id获取工治具库位
        /// </summary>
        /// <param name="id">库位Id</param>
        /// <returns>工治具库位</returns>
        public virtual StorageLocation GetStorageLocation(double id)
        {
            return Query<StorageLocation>().Where(p => p.Id == id).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具ID获取工治具仓库列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="code">工治具ID</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>工治具仓库列表</returns>
        public virtual EntityList<Warehouse> GetFixtureWarehouses(string keyword, string code, PagingInfo pagingInfo)
        {
            var query = Query<Warehouse>().Exists<FixtureEncodeStorageLocation>(
                    (x, y) => y.Join<FixtureEncode>((c, d) => c.FixtureEncodeId == d.Id)
                        .Join<FixtureEncode, FixtureAccount>((c, d) => c.Id == d.FixtureEncodeId && d.Code == code)
                        .Where(p => p.WarehouseId == x.Id));
            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取所有仓库列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>所有仓库列表</returns>
        public virtual EntityList<Warehouse> GetFixtureWarehouses(string keyword, PagingInfo pagingInfo)
        {
            var query = Query<Warehouse>();
            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工治具台账中库存详情列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>所有仓库列表</returns>
        public virtual EntityList<FixtureAccountStock> GetFixtureAccountStocks(string keyword, PagingInfo pagingInfo)
        {
            var query = Query<FixtureAccountStock>();
            if (keyword.IsNotEmpty())
                query.Where(p => p.WarehouseCode.Contains(keyword) || p.WarehouseName.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具库位Id列表获取工治具库位列表
        /// </summary>
        /// <param name="ids">工治具库位Id列表</param>
        /// <returns>工治具库位列表</returns>
        public virtual EntityList<StorageLocation> GetFixtureStorageLocations(List<double?> ids)
        {
            return Query<StorageLocation>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具ID和仓库编码获取工治具库位列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="code">工治具ID</param>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>工治具库位列表</returns>
        public virtual EntityList<StorageLocation> GetEncodeStorageLocations(string keyword, string code, double warehouseId, PagingInfo pagingInfo)
        {
            var query = Query<StorageLocation>().Exists<FixtureEncodeStorageLocation>(
                    (x, y) => y.Join<FixtureEncode>((c, d) => c.FixtureEncodeId == d.Id)
                        .Join<FixtureEncode, FixtureAccount>((c, d) => c.Id == d.FixtureEncodeId && (d.Code == code||c.Code == code))
                        .Where(p => p.StorageLocationId == x.Id && x.WarehouseId == warehouseId));
            if (keyword.IsNotEmpty())
                query.Where(p => p.Name.Contains(keyword) || p.Code.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据仓库Id获取库位列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>库位列表</returns>
        public virtual EntityList<StorageLocation> GetFixtureStorageLocationsByWarehouseId(string keyword, double warehouseId, PagingInfo pagingInfo)
        {
            var query = Query<StorageLocation>().Where(p => p.WarehouseId == warehouseId);
            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具台帐Id获取在库的工治具库位列表
        /// </summary>
        /// <param name="accountId">工治具台帐Id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>工治具库位列表</returns>
        public virtual EntityList<StorageLocation> GetStorageLocations(double accountId, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<StorageLocation>().Exists<FixtureAccountStock>((a, b) => b.Where(f => f.FixtureStorageLocationId == a.Id && f.FixtureAccountId == accountId && f.TotalQty > 0));
            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 根据工治具台帐Id和仓库Id获取在库的工治具库位列表
        /// </summary>
        /// <param name="accountId">工治具台帐Id</param>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>工治具库位列表</returns>
        public virtual EntityList<StorageLocation> GetStorageLocations(double accountId, double warehouseId, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<StorageLocation>().Exists<FixtureAccountStock>((a, b) => b.Where(f => f.FixtureStorageLocationId == a.Id && f.FixtureAccountId == accountId && f.FixtureWarehouseId == warehouseId && f.TotalQty > 0));
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具台帐Id获取在库的工治具仓库列表
        /// </summary>
        /// <param name="accountId">工治具台帐Id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>工治具库位列表</returns>
        public virtual EntityList<Warehouse> GetWarehouses(double accountId, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Warehouse>().Exists<FixtureAccountStock>((a, b) => b.Where(f => f.FixtureWarehouseId == a.Id && f.FixtureAccountId == accountId));
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工治具编码配置项中配置的工治具类型所属的仓库
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>工治具库位列表</returns>
        public virtual EntityList<Warehouse> GetConfigWarehouses(PagingInfo pagingInfo, string keyword)
        {
            List<Catalog> CatalogList = RT.Service.Resolve<CatalogController>().GetCatalogList(Warehouse.CatalogCategory).ToList();
            var config = ConfigService.GetConfig(new SIE.Fixtures.Models.Config.FixtureEncodeConfig(), typeof(FixtureEncode));
            var query = Query<Warehouse>();
            if (config != null && config.WareHouseTypeIds.IsNotEmpty())
            {
                var Ids = config.WareHouseTypeIds.Split(',').ToList();
                List<string> arraylist = CatalogList.Where(p => Ids.Contains(p.Id.ToString())).Select(p => p.Code).ToList();
                query.Where(p => arraylist.Contains(p.Category));
            }
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 获取工治具异常类型编码
        /// </summary>
        /// <returns>工治具异常类型编码</returns>
        public virtual string GetAbnormalCode()
        {
            return _commonController.GetNo<FixtureAbnormal>("工治具异常类型");
        }

        /// <summary>
        /// 获取异常类型-异常现象
        /// </summary>
        /// <param name="fixtureType"><see cref="FixtureType"/>工治具类型</param>
        /// <param name="abnormalType"><see cref="AbnormalType"/>异常类型</param>
        /// <param name="pageinfo">分页</param>
        /// <param name="ordeinfos">排序</param>
        /// <returns></returns>
        public virtual EntityList<FixtureAbnormal> GetFixtureAbnormals(AbnormalType abnormalType, string fixtureType, PagingInfo pageinfo, List<OrderInfo> ordeinfos)
        {
            if (fixtureType.IsNullOrEmpty()) return new EntityList<FixtureAbnormal>();
            var fixtureTypeEntity = Query<FixtureType>().Where(n => n.Code == fixtureType).FirstOrDefault();
            var query = Query<FixtureAbnormal>().Where(c => c.AbnormalType == abnormalType && c.FixtureTypeId == fixtureTypeEntity.Id);
            if (ordeinfos != null && ordeinfos.Any())
                query.OrderBy(ordeinfos);
            return query.ToList(pageinfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        /// <summary>
        /// 查询工治具异常类型列表
        /// </summary>
        /// <param name="pageinfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<FixtureAbnormal> GetFixtureAbnormalList(PagingInfo pageinfo, string keyword)
        {
            var query = Query<FixtureAbnormal>();
            if (keyword.IsNotEmpty()) {
                query.Where(p => p.Code.Contains(keyword));
            }
            return query.ToList(pageinfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具类型获取异常现象列表
        /// </summary>
        /// <param name="fixtureType">工治具类型</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>异常现象列表</returns>
        public virtual EntityList<FixtureAbnormal> GetFixtureAbnormals(FixtureType fixtureType, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<FixtureAbnormal>().Where(p => p.FixtureTypeId == fixtureType.Id && p.AbnormalType == AbnormalType.Unusual);
            if (keyword.IsNotEmpty())
            { query.Where(p => p.Description.Contains(keyword) || p.Code.Contains(keyword)); }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工治具编码配置项的工治具类型的仓库
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<Warehouse> GetFixtureEncodeConfigWarehouses(string keyword,PagingInfo pagingInfo)
        {
            var configValue = ConfigService.GetConfig<FixtureEncodeConfigValue>(new Models.Config.FixtureEncodeConfig(), typeof(FixtureEncode));
            if (configValue != null)
            {
                var typeIds = configValue.WareHouseTypeCode.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                return Query<Warehouse>().WhereIf(typeIds.Any(), m => typeIds.Contains(m.Category))
                    .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.Code.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
            return new EntityList<Warehouse>();
        }
}
}
