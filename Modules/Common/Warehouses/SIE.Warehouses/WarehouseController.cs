using Castle.Core.Internal;
using NPOI.SS.Formula.PTG;
using Org.BouncyCastle.Asn1.Mozilla;
using SIE.Api;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.InvOrg;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Core.Common;
using SIE.Core.Equipments;
using SIE.Core.Labels;
using SIE.Data;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.Rbac.InvOrgs;
using SIE.Rbac.Roles;
using SIE.Rbac.Users;
using SIE.Resources.Employees;
using SIE.Warehouses.Configs;
using SIE.Warehouses.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Warehouses
{
    /// <summary>
    /// 仓库控制器
    /// </summary>
    public partial class WarehouseController : DomainController
    {
        #region 库位
        /// <summary>
        /// 验证并返回库位
        /// </summary>
        /// <param name="code">库位编码</param>
        /// <returns>库位</returns>
        /// <exception cref="ValidationException"></exception>
        public virtual StorageLocation ValidateAndGetLocation(string code)
        {
            var location = GetStorageLocation(code);
            if (location == null)
            {
                throw new ValidationException("库位[{0}]不存在".L10nFormat(code));
            }

            if (location.State == State.Disable)
            {
                throw new ValidationException("库位[{0}]已禁用".L10nFormat(code));
            }

            return location;
        }

        /// <summary>
        /// 获取库位
        /// </summary>
        /// <param name="areaId">库位ID</param>
        /// <returns>货位列表</returns>
        public virtual EntityList<StorageLocation> GetStorageLocations(double areaId)
        {
            if (areaId < 0)
            {
                throw new ValidationException("货区ID小于等于零".L10N());
            }

            var q = Query<StorageLocation>();
            q.Where(p => p.AreaId == areaId);
            return q.ToList();
        }

        /// <summary>
        /// 获取库位数据
        /// </summary>
        /// <param name="criteria">库位查询条件</param>
        /// <returns>返回库位数据</returns>
        public virtual EntityList<StorageLocation> GetStorageLocations(MultiQueryLocCriteria criteria)
        {
            var query = Query<StorageLocation>();
            query.Where(p => p.State == State.Enable);
            if (criteria != null)
            {
                if (!string.IsNullOrEmpty(criteria.Code))
                    query.Where(p => p.Code.Contains(criteria.Code));
                if (!string.IsNullOrEmpty(criteria.Name))
                    query.Where(p => p.Name.Contains(criteria.Name));
                if (criteria.LibraryType.HasValue)
                    query.Where(p => p.LibraryType == criteria.LibraryType.Value);
                if (!string.IsNullOrEmpty(criteria.Warehouses))
                    query.Join<Warehouse>("b", (a, b) => a.WarehouseId == b.Id && criteria.Warehouses.Split(';', StringSplitOptions.None).ToList().Contains(b.Code));
                if (!string.IsNullOrEmpty(criteria.Warehouse))
                    query.Where(p => p.Warehouse.Code.Contains(criteria.Warehouse));
                if (!string.IsNullOrEmpty(criteria.Area))
                    query.Where(p => p.Area.Code.Contains(criteria.Area));
                if (!string.IsNullOrEmpty(criteria.AreaIds))
                {
                    var areaIdList = criteria.AreaIds.Split(';').Select(p => Convert.ToDouble(p)).ToList();
                    query.Where(p => areaIdList.Contains(p.AreaId));
                }

                if (criteria.IsFrozen.HasValue)
                    query.Where(p => p.IsFrozen == criteria.IsFrozen.Value);
                if (criteria.FilterId != null && criteria.FilterId.Count > 0)
                {
                    query.Where(p => !criteria.FilterId.ToList().Contains(p.Id));
                }
            }

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(StorageLocation.WarehouseProperty);
            elo.LoadWith(StorageLocation.AreaProperty);
            elo.LoadWithViewProperty();
            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, elo);
        }

        /// <summary>
        /// 获取库位数据
        /// </summary>
        /// <param name="warehouseId">仓库</param>
        /// <param name="locationCodeList">库位编号集合</param>
        /// <param name="elo">贪婪参数</param>
        /// <returns>返回库位数据</returns>
        public virtual EntityList<StorageLocation> GetStorageLocations(double warehouseId, List<string> locationCodeList, EagerLoadOptions elo = null)
        {
            if (elo == null)
                elo = new EagerLoadOptions().LoadWithViewProperty();
            return locationCodeList.SplitContains(codes =>
            {
                return Query<StorageLocation>().Where(p => codes.Contains(p.Code) && p.WarehouseId == warehouseId).ToList(null, elo);
            });
        }

        /// <summary>
        /// 获取库位数据
        /// </summary>
        /// <param name="locationCodeList">库位编号集合</param>
        /// <param name="elo">贪婪参数</param>
        /// <returns>返回库位数据</returns>
        public virtual EntityList<StorageLocation> GetStorageLocations(List<string> locationCodeList, EagerLoadOptions elo = null)
        {
            if (elo == null)
                elo = new EagerLoadOptions().LoadWithViewProperty();
            return locationCodeList.SplitContains(codes =>
            {
                return Query<StorageLocation>().Where(p => codes.Contains(p.Code)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 根据库位Id集合获取库位集合数据
        /// </summary>
        /// <param name="storageLocationIdList">库位Id集合</param>
        /// <param name="elo">贪婪加载对象</param>
        /// <returns>返回库位集合数据</returns>
        public virtual EntityList<StorageLocation> GetStorageLocations(List<double> storageLocationIdList, EagerLoadOptions elo)
        {
            return storageLocationIdList.SplitContains(ids =>
            {
                return Query<StorageLocation>().Where(p => ids.Contains(p.Id)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 根据仓库获取库位
        /// </summary>
        /// <param name="warehouses">仓库集合，已“;”分隔</param>
        /// <param name="areaIds">库区ID集合，已“;”分隔</param>
        /// <param name="locIds">库位ID集合，已“;”分隔</param>
        /// <param name="isEnable">是否筛选可用</param>
        /// <returns>库位</returns>
        public virtual EntityList<StorageLocation> GetStorageLocations(string warehouses, string areaIds, string locIds, bool isEnable = false)
        {
            var q = Query<StorageLocation>();

            if (!warehouses.IsNullOrEmpty())
                q.Where(p => warehouses.Split(';', StringSplitOptions.None).ToList().Contains(p.Warehouse.Code));

            if (!areaIds.IsNullOrEmpty())
            {
                var areaIdList = areaIds.Split(';').Select(t => Convert.ToDouble(t));
                q.Where(p => areaIdList.Contains(p.Area.Id));
            }

            if (!locIds.IsNullOrEmpty())
            {
                var locIdList = locIds.Split(';').Select(t => Convert.ToDouble(t));
                q.Where(p => locIdList.Contains(p.Id));
            }

            if (isEnable)
                q.Where(p => p.State == State.Enable);

            return q.ToList();
        }

        /// <summary>
        /// 获取库位
        /// </summary>
        /// <param name="queryAction">委托查询条件</param>
        /// <param name="elo">贪婪加载项</param>
        /// <returns>库位</returns>
        public virtual EntityList<StorageLocation> GetStorageLocations(Action<IEntityQueryer<StorageLocation>> queryAction, EagerLoadOptions elo)
        {
            var query = Query<StorageLocation>();
            queryAction?.Invoke(query);
            if (elo == null)
                elo = new EagerLoadOptions().LoadWithViewProperty();
            return query.ToList(null, elo);
        }

        /// <summary>
        /// 获取同排层列的数据
        /// </summary>
        /// <param name="storageIds"></param>
        /// <returns></returns>
        public virtual EntityList<StorageLocation> GetStorageLocationsForAuto(List<double> storageIds)
        {
            return storageIds.SplitContains(ids =>
            {
                var query = DB.Query<StorageLocation>("s1").Join<StorageLocation>("s2", (s1, s2) => s1.AreaId == s2.AreaId && s1.RowNo == s2.RowNo && s1.LayerNo == s2.LayerNo && s1.ColumnNo == s2.ColumnNo);
                return query.Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取库位
        /// </summary>
        /// <param name="areaId">库位ID</param>
        /// <returns>货位列表</returns>
        [ApiService("获取库区下库位")]
        [return: ApiReturn("返回货主集合：List<StorageLocation>")]
        public virtual List<LocationData> GetStorageLocationByArea(double areaId)
        {
            if (areaId < 0)
            {
                throw new ValidationException("货区ID小于等于零".L10N());
            }
            List<LocationData> results = new List<LocationData>();
            var q = Query<StorageLocation>().Where(p => p.AreaId == areaId && p.State == State.Enable).ToList();

            q.ForEach((Action<StorageLocation>)(e =>
            {
                var data = new LocationData();
                data.Id = e.Id;
                data.Code = e.Code;
                data.Name = e.Name;
                data.AreaId = e.AreaId;
                data.AreaCode = e.Area.Code;
                data.AreaName = e.Area.Name;

                results.Add(data);
            }));
            return results;
        }

        /// <summary>
        /// 获取库位数据
        /// </summary>
        /// <param name="criteria">库位查询条件</param>
        /// <returns>返回库位数据</returns>
        public virtual EntityList<StorageLocation> GetStorageLocation(StorageLocationCriteria criteria)
        {
            var q = Query<StorageLocation>();

            ////仓库权限关联查询
            RT.Service.Resolve<WarehouseController>().ExistWarehouseEmplyee(q, StorageLocation.WarehouseIdProperty);

            q.Join<Warehouse>("b", (a, b) => a.WarehouseId == b.Id);
            q.Join<StorageArea>("c", (a, c) => a.AreaId == c.Id);
            if (criteria != null)
            {
                if (!string.IsNullOrEmpty(criteria.Code))
                    q.Where(p => p.Code.Contains(criteria.Code));
                if (!string.IsNullOrEmpty(criteria.Name))
                    q.Where(p => p.Name.Contains(criteria.Name));
                if (criteria.LibraryType.HasValue)
                    q.Where(p => p.LibraryType == criteria.LibraryType.Value);
                if (criteria.WarehouseId.HasValue)
                    q.Where(p => p.WarehouseId == criteria.WarehouseId);
                if (!string.IsNullOrEmpty(criteria.Area))
                    q.Where<StorageArea>((a, c) => c.Code.Contains(criteria.Area) || c.Name.Contains(criteria.Area));
                if (criteria.IsFrozen.HasValue)
                    q.Where(p => p.IsFrozen == criteria.IsFrozen.Value);
                if (!string.IsNullOrEmpty(criteria.ErpInvOrg))
                    q.Where(p => p.ErpInvOrg.Contains(criteria.ErpInvOrg));
                if (!string.IsNullOrEmpty(criteria.ErpLocation))
                    q.Where(p => p.ErpLocation.Contains(criteria.ErpLocation));
                if (!string.IsNullOrEmpty(criteria.ErpSubLibrary))
                    q.Where(p => p.ErpSubLibrary.Contains(criteria.ErpSubLibrary));
                if (!string.IsNullOrEmpty(criteria.Warehouses))
                    q.Where<Warehouse>((a, b) => criteria.Warehouses.Split(';', StringSplitOptions.None).ToList().Contains(b.Code));
                if (!string.IsNullOrEmpty(criteria.AreaIds))
                {
                    var areaIdList = criteria.AreaIds.Split(';').Select(p => Convert.ToDouble(p)).ToList();
                    q.Where(p => areaIdList.Contains(p.AreaId));
                }
                if (criteria.IsAutomatedArea.HasValue)
                {
                    if (criteria.IsAutomatedArea == YesNo.Yes)
                    {
                        q.Where(p => !p.IsOutLock && !p.IsCountLock);
                    }

                    q.Where<StorageArea>((a, c) => c.IsAutomatedArea == (criteria.IsAutomatedArea == YesNo.Yes));
                }

                if (criteria.RowNo.HasValue)
                    q.Where(p => p.RowNo == criteria.RowNo.Value);
                if (criteria.ColumnNo.HasValue)
                    q.Where(p => p.ColumnNo == criteria.ColumnNo.Value);
                if (criteria.LayerNo.HasValue)
                    q.Where(p => p.LayerNo == criteria.LayerNo.Value);
                if (criteria.RoutewayId.HasValue)
                    q.Where(p => p.RoutewayId == criteria.RoutewayId.Value);
                if (criteria.LogicAreaId.HasValue)
                    q.Exists<LogicAreaLocation>((p, la) => la.Where(t => t.StorageLocationId == p.Id && t.LogicAreaId == criteria.LogicAreaId));
                if (criteria.State.HasValue)
                    q.Where(p => p.State == criteria.State);

            }

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(StorageLocation.WarehouseProperty);
            elo.LoadWith(StorageLocation.AreaProperty);
            elo.LoadWithViewProperty();
            q.OrderBy(criteria.OrderInfoList);
            return q.ToList(criteria.PagingInfo, elo);
        }

        /// <summary>
        /// 获取库位ByCode和仓库
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns>库位</returns>
        public virtual StorageLocation GetStorageLocation(string code)
        {
            return Query<StorageLocation>().Where(p => p.Code == code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取库位ByCode和仓库
        /// </summary>
        /// <param name="warehouseId">仓库</param>
        /// <param name="code">编码</param>
        /// <returns>库位</returns>
        public virtual StorageLocation GetStorageLocation(double warehouseId, string code)
        {
            return Query<StorageLocation>().Where(p => p.WarehouseId == warehouseId && p.Code == code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据库位Code查询所有的库位数据
        /// </summary>
        /// <param name="codeList">库位编码</param>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="elo"></param>
        /// <returns></returns>
        public virtual EntityList<StorageLocation> GetStorageLocationByCodes(List<string> codeList, double? warehouseId = null, EagerLoadOptions elo = null)
        {
            return codeList.SplitContains(codes =>
            {
                var query = Query<StorageLocation>();
                if (warehouseId.HasValue)
                {
                    query.Where(p => p.WarehouseId == warehouseId);
                }
                query.Where(p => codes.Contains(p.Code));

                return query.ToList(null, elo);
            });
        }

        /// <summary>
        /// 根据仓库Id集合获取库位集合数据
        /// </summary>
        /// <param name="whIds">仓库Id集合</param>
        /// <returns>返回库位集合数据</returns>
        public virtual EntityList<StorageLocation> GetStorageLocationByWhIds(List<double> whIds)
        {
            return whIds.Distinct().SplitContains(pWhIds =>
            {
                return Query<StorageLocation>().Where(p => pWhIds.Contains(p.WarehouseId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 验证库位是否冻结，是否启用，入库锁，出库锁，盘点锁未勾选
        /// </summary>
        /// <param name="locIds">库位ID集合</param>
        /// <exception cref="ValidationException">异常信息</exception>
        public virtual void ValidLocationIsLock(List<double> locIds)
        {
            EntityList<StorageLocation> locs = GetStorageLocations(locIds, new EagerLoadOptions().LoadWithViewProperty());
            if (locs.Any(p => p.State == State.Disable || p.IsInLock || p.IsOutLock || p.IsCountLock || p.IsFrozen))
            {
                string errMsg = "库位必须是启用的并且入库锁、出库锁、盘点锁、是否冻结未勾选".L10N();
                throw new ValidationException(errMsg);
            }
        }

        /// <summary>
        /// 获取库位是否允许人工上架
        /// </summary>
        /// <param name="locIds">库位ID</param>
        /// <returns>是否允许人工上架</returns>
        public virtual bool GetLocationsIsAllowManualGrounding(List<double> locIds)
        {
            bool isAllow = true;
            EntityList<StorageLocation> locs = GetStorageLocations(locIds, new EagerLoadOptions().LoadWithViewProperty());
            if (locs.Any(p => !p.IsAllowManualGrounding))
            {
                isAllow = false;
            }

            return isAllow;
        }

        /// <summary>
        /// 获取库位是否允许人工上架
        /// </summary>
        /// <param name="locIds">库位ID</param>
        /// <returns>是否允许人工上架</returns>
        public virtual List<double> GetIsAllowManualGroundingLocs(List<double> locIds)
        {
            return DataProcessEx.SplitContains(locIds, pLocIds =>
            {
                var locs = Query<StorageLocation>().Where(p => pLocIds.Contains(p.Id)).Exists<StorageArea>((x, y) => y.Where(p => p.Id == x.AreaId && p.IsAllowManualGrounding)).ToList();
                return locs.Select(p => p.Id).ToList();
            });
        }

        /// <summary>
        /// 获取允许人工上架的库位
        /// </summary>
        /// <param name="wareId"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<StorageLocation> GetIsAllowManualGroundingLocList(double wareId, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<StorageLocation>()
                .Where(p => p.WarehouseId == wareId && !p.IsFrozen && p.State == State.Enable)
                .WhereIf(keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .Exists<StorageArea>((x, y) => y.Where(p => p.Id == x.AreaId && p.IsAllowManualGrounding)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return query;
        }

        /// <summary>
        /// 获取库位基本资料
        /// </summary>
        /// <param name="storageLocationId">库位Id</param>
        /// <returns>返回库位基本资料</returns>
        public virtual StorageLocationInfo GetStorageLocationInfo(double storageLocationId)
        {
            return Query<StorageLocationInfo>().Where(p => p.StorageLocationId == storageLocationId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取库位基本资料
        /// </summary>
        /// <param name="locIdList">库位ID集合</param>
        /// <returns>库位基本资料</returns>
        public virtual EntityList<StorageLocationInfo> GetStorageLocationInfos(List<double> locIdList)
        {
            return locIdList.Distinct().SplitContains(locIds =>
            {
                var query = Query<StorageLocationInfo>();
                query.Where(p => locIds.Contains(p.StorageLocationId));
                return query.ToList();
            });
        }

        /// <summary>
        /// 获取仓储资料
        /// </summary>
        /// <param name="storageLocationId">库位Id</param>
        /// <returns>返回库位仓储资料</returns>
        public virtual StorageLocationLayinInfo GetStorageLocationLayinInfo(double storageLocationId)
        {
            return Query<StorageLocationLayinInfo>().Where(p => p.StorageLocationId == storageLocationId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取仓储资料
        /// </summary>
        /// <param name="locIdList">库位ID集合</param>
        /// <returns>仓储资料</returns>
        public virtual EntityList<StorageLocationLayinInfo> GetStorageLocationLayinInfos(List<double> locIdList)
        {
            return locIdList.Distinct().SplitContains(locIds =>
            {
                var query = Query<StorageLocationLayinInfo>().Where(p => locIds.Contains(p.StorageLocationId));
                return query.ToList();
            });
        }

        /// <summary>
        /// 获取库位专储物料清单行数
        /// </summary>
        /// <param name="locId">库位</param>
        /// <param name="itemId">物料ID</param>
        /// <returns>库位专储物料清单行数</returns>
        public virtual int GetStorageLocationItemList(double locId, double? itemId)
        {
            var query = Query<StorageLocationItemList>();
            query.Where(p => p.StorageLocationId == locId);
            if (itemId.HasValue)
                query.Where(p => p.ItemId == itemId.Value);
            return query.Count();
        }

        /// <summary>
        /// 获取操作管理
        /// </summary>
        /// <param name="storageLocationId">库位Id</param>
        /// <returns>返回库位仓储资料</returns>
        public virtual StorageLocationOperation GetStorageLocationOperation(double storageLocationId)
        {
            return Query<StorageLocationOperation>().Where(p => p.StorageLocationId == storageLocationId).FirstOrDefault();
        }

        /// <summary>
        /// 获取库位操作管理
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="code">库位编码</param>
        /// <returns>操作管理</returns>
        public virtual StorageLocationOperation GetStorageLocationOperation(double warehouseId, string code)
        {

            var query = Query<StorageLocationOperation>().Join<StorageLocation>((x, y) => x.StorageLocationId == y.Id && y.Code.Contains(code) && y.WarehouseId == warehouseId);
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 获取库位操作管理
        /// </summary>
        /// <param name="locIdList">库位ID集合</param>
        /// <returns>库位操作管理</returns>
        public virtual EntityList<StorageLocationOperation> GetStorageLocationOperations(List<double> locIdList)
        {
            return locIdList.SplitContains(locIds =>
            {
                var query = Query<StorageLocationOperation>();
                query.Where(p => locIds.Contains(p.StorageLocationId));
                return query.ToList();
            });
        }

        /// <summary>
        /// 获取简单库位集合
        /// </summary>
        /// <param name="locIds">库位Id集合</param>
        /// <returns></returns>
        public virtual List<SimpleStorageLocData> GetSimpleStorageLocData(List<double> locIds)
        {
            return DataProcessEx.SplitContains(locIds, ids =>
            {
                var query = Query<StorageLocation>();
                query.Select(p => new
                {
                    p.Id,
                    p.Code,
                    p.Name,
                });
                return query.ToList<SimpleStorageLocData>().ToList();
            });
        }

        /// <summary>
        /// 获取简单仓库集合
        /// </summary>
        /// <param name="warehouseIds">仓库Id集合</param>
        /// <returns></returns>
        public virtual List<SimpleWarehouseData> GetSimpleWarehouseDatas(List<double> warehouseIds)
        {
            return DataProcessEx.SplitContains(warehouseIds, ids =>
            {
                var query = Query<Warehouse>();
                query.Select(p => new
                {
                    p.Id,
                    p.Code,
                    p.Name,
                });
                return query.ToList<SimpleWarehouseData>().ToList();
            });
        }
        /// <summary>
        /// 冻结解冻库位
        /// </summary>
        /// <param name="storageLocationIdList">库位Id集合</param>
        /// <returns>返回错误信息</returns>
        public virtual string FrozenOrThawLocation(List<double> storageLocationIdList)
        {
            string msg = string.Empty;
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(StorageLocation.AreaProperty);
            EntityList<StorageLocation> storageLocationData = GetStorageLocations(storageLocationIdList, elo);
            List<string> areaCodeList = storageLocationData.Where(p => p.Area.IsFrozen).Select(p => p.Area.Code).ToList();
            if (areaCodeList.Count != 0)
            {
                msg = string.Format("库区为:{0}已经被冻结，库位不能再次冻结或解冻。".L10nFormat(string.Join(",", areaCodeList)));
                return msg;
            }

            List<string> warehouseCodeList = storageLocationData.Where(p => p.Warehouse.IsFrozen).Select(p => p.Warehouse.Code).ToList();
            if (warehouseCodeList.Count != 0)
            {
                msg = string.Format("仓库为:{0}已经被冻结，库位不能再次冻结或解冻。".L10nFormat(string.Join(",", warehouseCodeList)));
                return msg;
            }

            if (storageLocationData.Select(p => p.IsFrozen).Distinct().Count() != 1)
            {
                msg = string.Format("选择的库位必须具有相同的冻结状态。".L10N());
                return msg;
            }

            using (var tran = DB.TransactionScope(WareHouseEntityDataProvider.ConnectionStringName))
            {
                //冻结库位ID集合
                List<double> frozenLocIds = storageLocationData.Where(p => p.IsFrozen).Select(p => p.Id).ToList();
                //冻结库位ID集合存在则标识要进行解冻操作，解冻时，将库位下的冻结原因数据删除
                if (frozenLocIds.Any())
                {
                    BatchDeleteLocFrozenReason(frozenLocIds, FrozenReason.LocFrozen);
                }

                //非冻结库位ID集合
                List<double> unFrozenLocIds = storageLocationData.Where(p => !p.IsFrozen).Select(p => p.Id).ToList();
                //非冻结ID集合存在则标识要进行冻结操作,冻结时，在冻结原因页签插入“库位冻结”数据
                if (unFrozenLocIds.Any())
                {
                    BatchInsertLocFrozenReason(unFrozenLocIds, FrozenReason.LocFrozen, string.Empty);
                }

                //批量更新库位是否冻结栏位
                BatchUpdateLocIsFrozen(storageLocationData, storageLocationIdList);

                tran.Complete();
            }

            return msg;
        }

        /// <summary>
        /// 启用库位
        /// </summary>
        /// <param name="storageLocationIdList">库位Id集合</param>
        /// <returns>返回错误信息</returns>
        public virtual string EnableStorageLocation(List<double> storageLocationIdList)
        {
            string msg = string.Empty;
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(StorageLocation.AreaProperty);
            EntityList<StorageLocation> storageLocationData = GetStorageLocations(storageLocationIdList, elo);
            List<string> areaCodeList = storageLocationData.Where(p => p.Area.State == State.Disable).Select(p => p.Code).ToList();
            if (areaCodeList.Count != 0)
            {
                msg = string.Format("库区为:{0}已经被禁用，库位不能启用".L10nFormat(string.Join(",", areaCodeList)));
                return msg;
            }

            if (storageLocationData.Any(p => p.State == State.Enable))
            {
                msg = string.Format("选择的库位必须为禁用状态。".L10N());
                return msg;
            }

            for (int i = 0; i < storageLocationData.Count; i++)
            {
                storageLocationData[i].State = State.Enable;
            }

            RF.Save(storageLocationData);

            return msg;
        }

        /// <summary>
        /// 禁用库位
        /// </summary>
        /// <param name="storageLocationIdList">库位Id集合</param>
        /// <returns>返回错误信息</returns>
        public virtual string DisableStorageLocation(List<double> storageLocationIdList)
        {
            string msg = string.Empty;
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(StorageLocation.AreaProperty);

            foreach (var locationId in storageLocationIdList)
            {
                EntityList<StorageAreaOperation> storageAreaOperationData = GetStorageAreaOperations(locationId, elo);
                if (storageAreaOperationData.Count != 0)
                {
                    List<string> areaOperationaCode1List = storageAreaOperationData.Select(c => c.UpTransitLocation?.Code).ToList();
                    List<string> areaOperationaCode2List = storageAreaOperationData.Select(c => c.DownTransitLocation?.Code).ToList();
                    if (areaOperationaCode1List.Count != 0)
                    {
                        msg = string.Format("库位为:{0}已经被库区操作引用，库位不能禁用".L10nFormat(string.Join(",", areaOperationaCode1List)));
                    }

                    if (areaOperationaCode2List.Count != 0)
                    {
                        msg = string.Format("库位为:{0}已经被库区操作引用，库位不能禁用".L10nFormat(string.Join(",", areaOperationaCode2List)));
                    }

                    return msg;
                }
            }

            EntityList<StorageLocation> storageLocationData = GetStorageLocations(storageLocationIdList, elo);

            if (storageLocationData.Any(p => p.State == State.Disable))
            {
                msg = string.Format("选择的库位必须为启用状态。".L10N());
                return msg;
            }

            for (int i = 0; i < storageLocationData.Count; i++)
            {
                storageLocationData[i].State = State.Disable;
            }

            RF.Save(storageLocationData);

            return msg;
        }

        /// <summary>
        /// 根据编码规则获取打印模板
        /// </summary>
        /// <param name="numberRuleId">编码规则Id</param>
        /// <param name="printEntityType">实体类型</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        public virtual EntityList<PrintTemplate> GetPrintTemplates(string printEntityType, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<PrintTemplate>().Join<NumberRuleInTemplate>((a, b) => a.Id == b.TemplateId && a.State == State.Enable &&
                        a.EntityType == printEntityType);
            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.FileName.Contains(keyword));
            }
            var printData = query.ToList(pagingInfo);
            if (!printData.Any())
            {
                query = Query<PrintTemplate>().Where(a => a.State == State.Enable &&
                         a.EntityType == printEntityType);
            }
            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 根据实体类型获取打印模板
        /// </summary>
        /// <param name="printEntityType">打印条码类型</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键词</param>
        /// <param name="printType">模板类型</param>
        /// <returns>打印模板列表</returns>
        public virtual EntityList<PrintTemplate> GetPrintTemplatesByType(string printEntityType, string keyword, PagingInfo pagingInfo, PrintType? printType = null)
        {
            var query = Query<PrintTemplate>().Where(p => p.State == State.Enable);
            if (!printEntityType.IsNullOrEmpty())
            {
                query.Where(p => p.EntityType.Contains(printEntityType));
            }
            if (keyword.IsNotEmpty())
                query.Where(p => p.FileName.Contains(keyword));

            if (printType.HasValue)
            {
                query.Where(p => p.PrintType == printType.Value);
            }

            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取库位打印规则
        /// </summary>
        /// <returns>库位打印规则</returns>
        public virtual PrintTemplate GetStorageLocationPrintTemplate()
        {
            var config = ConfigService.GetConfig(new StorageLocationCodeConfig(), typeof(StorageLocation));
            if (config == null || config.StorageLocationPrintRule == null)
                throw new ValidationException("[未配置库位打印模板]".L10N());

            return config.StorageLocationPrintRule;
        }

        /// <summary>
        /// 获取库位编码规则
        /// </summary>
        /// <returns>库位编码规则</returns>
        public virtual NumberRule GetStorageLocationNumberRule()
        {
            var config = ConfigService.GetConfig(new StorageLocationCodeConfig(), typeof(StorageLocation));
            if (config == null || config.StorageLocationCodeRule == null)
                throw new ValidationException("未找到库位编码规则,请检查规则配置".L10N());

            return config.StorageLocationCodeRule;
        }

        /// <summary>
        /// 验证库位编码是否符合编码规则
        /// </summary>
        /// <param name="storageLocationList">库位集合</param>
        public virtual void ValidateStorageLocationCodes(List<StorageLocation> storageLocationList)
        {
            NumberRule storageLocationCodeRule = RT.Service.Resolve<WarehouseController>().GetStorageLocationNumberRule();
            foreach (StorageLocation stroageLocation in storageLocationList)
            {
                if (!RT.Service.Resolve<NumberRuleController>().ValidateSegment(storageLocationCodeRule, stroageLocation.Code, stroageLocation))
                {
                    throw new ValidationException("库位编码不符合配置的编码规则".L10N());
                }
            }
        }

        /// <summary>
        /// 获取库位编号
        /// </summary>
        /// <returns>库位编号</returns>
        public virtual string GetStorageLocationCode()
        {
            var config = ConfigService.GetConfig(new StorageLocationCodeConfig(), typeof(StorageLocation));
            if (config.StorageLocationCodeRule != null)
            {
                return RT.Service.Resolve<NumberRuleController>()
                    .GenerateSegment(config.StorageLocationCodeRule.Id, 1)
                    .FirstOrDefault();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取仓库中的STAGE库位
        /// </summary>
        /// <param name="wareHouseId">仓库Id</param>
        /// <returns>返回Stage库位</returns>
        public virtual StorageLocation GetStageStorageLocation(double wareHouseId)
        {
            using (SIE.Common.InvOrg.InvOrgs.WithAll(true))
            {
                return Query<StorageLocation>().Where(p => p.WarehouseId == wareHouseId && p.Code == Warehouse.STAGE).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取仓库中的STAGE库位
        /// </summary>
        /// <param name="wareHouseIds">仓库Id集合</param>
        /// <returns>返回Stage库位</returns>
        public virtual EntityList<StorageLocation> GetStageStorageLocations(List<double> wareHouseIds)
        {
            return wareHouseIds.Distinct().SplitContains(whIds =>
            {
                var query = Query<StorageLocation>().Where(p => p.Code == Warehouse.STAGE);
                return query.Where(p => whIds.Contains(p.WarehouseId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取仓库中的PICKTO库位
        /// </summary>
        /// <param name="wareHouseIdList">仓库Id集合</param>
        /// <returns>返回PICKTO库位集合</returns>
        public virtual EntityList<StorageLocation> GetPickToStorageLocation(List<double> wareHouseIdList)
        {
            return Query<StorageLocation>().Where(p => wareHouseIdList.Contains(p.WarehouseId) && p.Code == Warehouse.PICKTO).ToList();
        }

        /// <summary>
        /// 获取仓库中的PICKTO库位
        /// </summary>
        /// <param name="wareHouseId">仓库Id</param>
        /// <returns>返回PICKTO库位</returns>
        public virtual StorageLocation GetPickToStorageLocation(double wareHouseId)
        {
            return Query<StorageLocation>().Where(p => p.WarehouseId == wareHouseId && p.Code == Warehouse.PICKTO).FirstOrDefault();
        }

        /// <summary>
        /// 获取仓库下的所有可用库位（可用、未冻结）
        /// </summary>
        /// <param name="wareHouseId">仓库Id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页信息</param>
        /// <param name="isPick">是否拣货</param>
        /// <param name="isAllowManualGrounding">是否允许人工上架</param>
        /// <param name="isLock">库位锁</param>
        /// <returns>库位列表数据</returns>
        public virtual EntityList<StorageLocation> GetEnableStorageLocations(double wareHouseId, string keyword, PagingInfo info, bool? isPick = null, bool? isAllowManualGrounding = null, bool? isLock = null)
        {
            var query = Query<StorageLocation>();
            query.Join<StorageArea>((a, b) => a.AreaId == b.Id && !b.IsFrozen)
                 .Join<Warehouse>((a, b) => a.WarehouseId == b.Id && !b.IsFrozen);
            query.Where(p => p.WarehouseId == wareHouseId && p.State == State.Enable && !p.IsFrozen);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            if (isPick.HasValue)
                query.Where(p => p.IsPick == isPick);
            if (isAllowManualGrounding.HasValue)
                query.Where<StorageArea>((a, b) => b.IsAllowManualGrounding == isAllowManualGrounding);

            if (isLock.HasValue)
            {
                query.Where(p => p.IsOutLock == isLock.Value && p.IsCountLock == isLock.Value && p.IsInLock == isLock.Value);
            }

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            return query.ToList(info, elo);
        }

        /// <summary>
        /// 获取所有可用非冻结的库位
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页信息</param>
        /// <param name="isPick">是否拣货</param>
        /// <param name="isAllowManualGrounding">是否允许人工上架</param>
        /// <param name="isLock">库位锁</param>
        /// <returns></returns>
        public virtual EntityList<StorageLocation> GetAllEnableStorageLocations(string keyword, PagingInfo info, bool? isPick = null, bool? isAllowManualGrounding = null, bool? isLock = null)
        {
            var query = Query<StorageLocation>();
            query.Join<StorageArea>((a, b) => a.AreaId == b.Id)
                 .Join<Warehouse>((a, b) => a.WarehouseId == b.Id);
            query.Where(p => p.State == State.Enable);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            if (isPick.HasValue)
                query.Where(p => p.IsPick == isPick);
            if (isAllowManualGrounding.HasValue)
                query.Where<StorageArea>((a, b) => b.IsAllowManualGrounding == isAllowManualGrounding);

            if (isLock.HasValue)
            {
                query.Where(p => p.IsOutLock == isLock.Value && p.IsCountLock == isLock.Value && p.IsInLock == isLock.Value);
            }

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            return query.ToList(info, elo);
        }

        /// <summary>
        /// 获取仓库下的所有可用库位（可用、未冻结）
        /// </summary>
        /// <param name="wareHouseId">仓库Id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页信息</param>
        /// <param name="isPick">是否拣货</param>
        /// <param name="isAllowManualGrounding">是否允许人工上架</param>
        /// <param name="isLock">库位锁</param>
        /// <returns>库位列表数据</returns>
        public virtual EntityList<StorageLocation> GetEnableStorageLocationsWithAllOrg(double wareHouseId, string keyword, PagingInfo info, bool? isPick = null, bool? isAllowManualGrounding = null, bool? isLock = null)
        {
            using (SIE.Common.InvOrg.InvOrgs.WithAll(true))
            {
                var query = Query<StorageLocation>();
                query.Join<StorageArea>((a, b) => a.AreaId == b.Id && !b.IsFrozen)
                     .Join<Warehouse>((a, b) => a.WarehouseId == b.Id && !b.IsFrozen);
                query.Where(p => p.WarehouseId == wareHouseId && p.State == State.Enable && !p.IsFrozen);
                if (!keyword.IsNullOrEmpty())
                    query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
                if (isPick.HasValue)
                    query.Where(p => p.IsPick == isPick);
                if (isAllowManualGrounding.HasValue)
                    query.Where<StorageArea>((a, b) => b.IsAllowManualGrounding == isAllowManualGrounding);

                if (isLock.HasValue)
                {
                    query.Where(p => p.IsOutLock == isLock.Value && p.IsCountLock == isLock.Value && p.IsInLock == isLock.Value);
                }

                EagerLoadOptions elo = new EagerLoadOptions();
                elo.LoadWithViewProperty();
                return query.ToList(info, elo);
            }
        }


        /// <summary>
        /// 获取仓库下的所有可用库位(用做查询条件过滤)
        /// </summary>
        /// <param name="storageAreaId">库区Id</param>
        /// <param name="wareHouseId">仓库库Id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页信息</param>
        /// <returns>库位列表数据</returns>
        public virtual EntityList<StorageLocation> GetEnableStorageLocations(double? storageAreaId, double? wareHouseId, string keyword, PagingInfo info)
        {
            var query = Query<StorageLocation>().Where(p => p.State == State.Enable && !p.IsFrozen);
            if (wareHouseId != null)
                query.Where(p => p.WarehouseId == wareHouseId);
            if (storageAreaId != null)
                query.Where(p => p.AreaId == storageAreaId);
            if (!keyword.IsNullOrEmpty())
            {
                //if (!keyword.Contains("%"))
                //    keyword = "%" + keyword + "%";
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取仓库下的所有可用库位(用做查询条件过滤)
        /// </summary>       
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页信息</param>
        /// <returns>库位列表数据</returns>
        public virtual EntityList<StorageLocation> GetFictitiousEnableStorageLocations(string keyword, PagingInfo info)
        {
            var query = Query<StorageLocation>().Where(p => p.State == State.Enable && !p.IsFrozen);
            query.Where(p => p.Warehouse.LibraryType == LibraryType.Fictitious);
            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
            //return query.ToList(info, null);
        }


        /// <summary>
        /// 获取仓库下的所有可用库位(用做查询条件过滤)
        /// </summary>
        /// <param name="storageAreaId">库区Id</param>
        /// <param name="warehouseCodes">仓库库Id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页信息</param>
        /// <returns>库位列表数据</returns>
        public virtual EntityList<StorageLocation> GetEnableStorageLocationsByWhCode(double? storageAreaId, string warehouseCodes, string keyword, PagingInfo info)
        {
            var query = Query<StorageLocation>().Where(p => p.State == State.Enable && !p.IsFrozen);
            if (warehouseCodes.IsNotEmpty())
                query.Where(p => p.Warehouse.Code.Contains(warehouseCodes));
            if (storageAreaId != null)
                query.Where(p => p.AreaId == storageAreaId);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取仓库下的所有可用库位
        /// </summary>
        /// <param name="wareHouseId">仓库Id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页信息</param>
        /// <param name="isFilterVirtual">是否过滤虚拟库位 默认否</param>
        /// <returns>库位列表数据</returns>
        public virtual EntityList<StorageLocation> GetEnableStorageLocationDatas(double wareHouseId, string keyword, PagingInfo info, bool? isFilterVirtual = false)
        {
            var query = Query<StorageLocation>();
            query.Where(p => p.WarehouseId == wareHouseId && p.State == State.Enable);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            if (isFilterVirtual.HasValue && isFilterVirtual.Value)
                query.Where(p => p.LibraryType == LibraryType.Entity);
            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 波次计划分配明细获取目标库位
        /// </summary>
        /// <param name="wareHouseId">仓库Id</param>
        /// <param name="keywork">关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>库位信息</returns>
        public virtual EntityList<StorageLocation> GetStorageLocationsForWavePlan(double wareHouseId, string keywork, PagingInfo pagingInfo)
        {
            var query = Query<StorageLocation>().Where(p => p.WarehouseId == wareHouseId && !p.IsFrozen
             && p.State == State.Enable && p.IsFocus);
            if (!string.IsNullOrWhiteSpace(keywork))
            {
                query.Where(p => p.Code.Contains(keywork) || p.Name.Contains(keywork));
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 补货分配明细获取目标库位
        /// </summary>
        /// <param name="wareHouseId">仓库Id</param>
        /// <param name="IsPick">是否拣货</param>    
        /// <param name="IsFocus">发货暂存</param>     
        /// <param name="keywork">关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>库位信息</returns>
        public virtual EntityList<StorageLocation> GetStorageLocationsForReplenish(double wareHouseId, bool? IsPick, bool? IsFocus, string keywork, PagingInfo pagingInfo)
        {
            var query = Query<StorageLocation>().Where(p => p.WarehouseId == wareHouseId && !p.IsFrozen
             && p.State == State.Enable);
            if (IsFocus.HasValue)
            {
                query.Where(p => p.IsFocus == IsFocus.Value);
            }
            if (IsPick.HasValue)
            {
                query.Where(p => p.IsPick == IsPick.Value);
            }

            if (!string.IsNullOrWhiteSpace(keywork))
            {
                query.Where(p => p.Code.Contains(keywork) || p.Name.Contains(keywork));
            }

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取仓库下的所有未冻结库位
        /// </summary>
        /// <param name="warehouseId">库区ID</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页</param>
        /// <returns>库位列表数据</returns>
        public virtual EntityList<StorageLocation> GetUnFrozenStorageLocations(double warehouseId, string keyword, PagingInfo info)
        {
            var query = Query<StorageLocation>();
            query.Join<StorageArea>((a, b) => a.AreaId == b.Id && !b.IsFrozen)
                 .Join<Warehouse>((a, b) => a.WarehouseId == b.Id && !b.IsFrozen);
            query.Where(p => p.WarehouseId == warehouseId && !p.IsFrozen);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取仓库下的所有未冻结库位
        /// </summary>
        /// <param name="warehouseId">库区ID</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页</param>
        /// <returns>库位列表数据</returns>
        public virtual EntityList<StorageLocation> GetUnFrozenStorageLocations(double? warehouseId, string keyword, PagingInfo info)
        {
            var query = Query<StorageLocation>();
            query.Join<StorageArea>((a, b) => a.AreaId == b.Id && !b.IsFrozen)
                 .Join<Warehouse>((a, b) => a.WarehouseId == b.Id && !b.IsFrozen);
            query.Where(p => p.WarehouseId == warehouseId.Value && !p.IsFrozen);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取库区下的所有可用库位
        /// </summary>
        /// <param name="areaId">库区ID</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页</param>
        /// <returns>库位列表数据</returns>
        public virtual EntityList<StorageLocation> GetEnableAreaStorageLocations(double areaId, string keyword, PagingInfo info)
        {
            var query = Query<StorageLocation>();
            query.Join<StorageArea>((a, b) => a.AreaId == b.Id && !b.IsFrozen)
                 .Join<Warehouse>((a, b) => a.WarehouseId == b.Id && !b.IsFrozen);
            query.Where(p => p.AreaId == areaId && p.State == State.Enable && !p.IsFrozen);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取库区下的所有可用库位
        /// </summary>
        /// <param name="areaId">库区ID</param>
        /// <param name="codeList">编码列表</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页</param>
        /// <returns>库位列表数据</returns>
        public virtual EntityList<StorageLocation> GetStorageLocationLists(double areaId, List<string> codeList, string keyword, PagingInfo info)
        {
            var query = Query<StorageLocation>().Where(p => codeList.Contains(p.Code));

            query.Join<StorageArea>((a, b) => a.AreaId == b.Id && !b.IsFrozen)
                 .Join<Warehouse>((a, b) => a.WarehouseId == b.Id && !b.IsFrozen);
            query.Where(p => p.AreaId == areaId && p.State == State.Enable && !p.IsFrozen);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(info);
        }

        /// <summary>
        /// 获取仓库下的所有可用库位（可用、拣货为True）
        /// </summary>
        /// <param name="wareHouseId">仓库Id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页信息</param>
        /// <returns>库位列表数据</returns>
        public virtual EntityList<StorageLocation> GetEnableStorageLocationDatasNoPick(double wareHouseId, string keyword, PagingInfo info)
        {
            var query = Query<StorageLocation>().Join<StorageLocationOperation>((x, y) => x.Id == y.StorageLocationId && y.IsPick);
            query.Where(p => p.WarehouseId == wareHouseId && p.State == State.Enable);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取可用库位（可用）
        /// </summary>
        /// <param name="storageareaId">仓库Id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页信息</param>
        /// <returns>库位列表数据</returns>
        public virtual EntityList<StorageLocation> GetStorageLocationDataList(double storageareaId, string keyword, PagingInfo info)
        {
            return Query<StorageLocation>().Where(p => p.AreaId == storageareaId && p.State == State.Enable)
                .WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(info);
        }

        /// <summary>
        /// 获取可用库位（可用）
        /// </summary>
        /// <param name="storageareaId">仓库Id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页信息</param>
        /// <returns>库位列表数据</returns>
        public virtual EntityList<StorageLocation> GetStorageLocationDataListIsFocus(double storageareaId, string keyword, PagingInfo info)
        {
            return Query<StorageLocation>().Where(p => p.IsFocus && p.AreaId == storageareaId && p.State == State.Enable)
                .WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(info);
        }

        /// <summary>
        /// 根据库区Id集合获取库位集合
        /// </summary>
        /// <param name="areaIds">仓区Id集合</param>
        /// <returns>库区</returns>
        public virtual EntityList<StorageLocation> GetStorageLocationsByAreaIds(List<double> areaIds)
        {
            if (areaIds.IsNullOrEmpty())
                return new EntityList<StorageLocation>();

            return areaIds.SplitContains(items =>
            {
                return Query<StorageLocation>().Where(c => items.Contains(c.AreaId)).ToList();
            });
        }

        /// <summary>
        /// 根据库位编码获取仓库有效（非禁用，非冻结）库位信息
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="code">库位编码</param>
        /// <param name="eager">贪婪加载</param>
        /// <returns>库位信息</returns>
        public virtual StorageLocation GetEffectiveLocation(double warehouseId, string code, bool eager = true)
        {
            var query = Query<StorageLocation>()
                .Join<Warehouse>((a, b) => a.WarehouseId == b.Id && !b.IsFrozen)
                .Where(p => p.State == State.Enable && !p.IsFrozen && p.WarehouseId == warehouseId && p.Code == code);
            var elo = new EagerLoadOptions().LoadWithViewProperty();
            if (!eager)
                elo = null;
            return query.FirstOrDefault(elo);
        }

        /// <summary>
        /// 根据库位编码获取仓库有效（非禁用，非冻结）库位信息
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="code">库位编码</param>      
        /// <returns>库位Id</returns>
        public virtual double? GetEffectiveLocationId(double warehouseId, string code)
        {
            var query = Query<StorageLocation>()
                .Join<Warehouse>((a, b) => a.WarehouseId == b.Id && !b.IsFrozen)
                .Where(p => p.State == State.Enable && !p.IsFrozen && p.WarehouseId == warehouseId && p.Code == code);
            var id = query.Select(e => new { e.Id }).FirstOrDefault<double>();
            if (id == 0)
                return null;
            return id;
        }

        /// <summary>
        /// 根据库位编码获取仓库有效（非禁用，非冻结）库位信息
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="code">库位编码</param>      
        /// <returns>库位</returns>
        public virtual StorageLocationData GetEffectiveStorageLocationData(double warehouseId, string code)
        {
            var query = Query<StorageLocation>()
               .Join<Warehouse>((a, b) => a.WarehouseId == b.Id && !b.IsFrozen)
               .Where(p => p.State == State.Enable && !p.IsFrozen && p.WarehouseId == warehouseId && p.Code == code);
            return query.Select(e => new { e.Id, e.Code, e.IsTemporary, e.State }).FirstOrDefault<StorageLocationData>();
        }

        /// <summary>
        /// 根据库位编码获取仓库有效（非禁用，非冻结）库位信息
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="codes">库位编码</param>
        /// <returns>库位</returns>
        public virtual List<StorageLocationData> GetEffectiveStorageLocationDatas(double warehouseId, List<string> codes)
        {
            var query = Query<StorageLocation>()
               .Join<Warehouse>((a, b) => a.WarehouseId == b.Id && !b.IsFrozen)
               .Where(p => p.State == State.Enable && !p.IsFrozen && p.WarehouseId == warehouseId && codes.Contains(p.Code));
            return query.Select(e => new { e.Id, e.Code, e.IsTemporary, e.State }).ToList<StorageLocationData>().ToList();
        }

        /// <summary>
        /// 根据库位编码获取库位信息
        /// </summary>       
        /// <param name="code">库位编码</param>
        /// <param name="warehouseId">仓库Id</param>
        /// <returns>库位信息</returns>
        public virtual StorageLocation GetLocation(string code, double warehouseId)
        {
            var query = Query<StorageLocation>().Where(p => p.Code == code && p.WarehouseId == warehouseId);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据库位编码集合获取仓库有效（非禁用，非冻结,是拣货）库位信息
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="codeList">库位编码集合</param>
        /// <returns>库位信息</returns>
        public virtual EntityList<StorageLocation> GetEffectivePickLocations(double warehouseId, List<string> codeList)
        {
            var query = Query<StorageLocation>()
                .Join<StorageArea>((a, b) => a.AreaId == b.Id && !b.IsFrozen)
                .Join<Warehouse>((a, b) => a.WarehouseId == b.Id && !b.IsFrozen);
            query.Where(p => p.State == State.Enable && !p.IsFrozen && p.WarehouseId == warehouseId && p.IsPick);

            if (codeList.Count > 0)
                query.Where(p => codeList.Contains(p.Code));

            return query.ToList();
        }

        /// <summary>
        /// 根据库位编码集合获取仓库有效（非禁用，非冻结）库位信息
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="codeList">库位编码集合</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>库位信息</returns>
        public virtual EntityList<StorageLocation> GetEffectiveLocations(double warehouseId, List<string> codeList, EagerLoadOptions elo = null)
        {
            var query = Query<StorageLocation>()
                .Join<StorageArea>((a, b) => a.AreaId == b.Id && !b.IsFrozen)
                .Join<Warehouse>((a, b) => a.WarehouseId == b.Id && !b.IsFrozen);
            query.Where(p => p.State == State.Enable && !p.IsFrozen && p.WarehouseId == warehouseId);

            if (codeList.Count > 0)
                query.Where(p => codeList.Contains(p.Code));

            return query.ToList(null, elo);
        }

        /// <summary>
        /// 根据库位编码集合获取仓库有效（非禁用，非冻结,是否拣货）库位信息
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="code">库位编码</param>
        /// <returns>库位信息</returns>
        public virtual StorageLocation GetEffectiveLocationsForReplensh(double warehouseId, string code)
        {
            var query = Query<StorageLocation>()
                .Join<StorageArea>((a, b) => a.AreaId == b.Id && !b.IsFrozen)
                .Join<Warehouse>((a, b) => a.WarehouseId == b.Id && !b.IsFrozen);
            query.Where(p => p.State == State.Enable && !p.IsFrozen && p.WarehouseId == warehouseId && p.Code == code && p.IsPick);
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 获取暂存库位
        /// </summary>
        /// <param name="wareHouseId">仓库ID</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="info">分页信息</param>
        /// <returns>暂存库位</returns>
        public virtual EntityList<StorageLocation> GetTemporaryLocations(double wareHouseId, string keyword, PagingInfo info)
        {
            var query = Query<StorageLocation>();
            query.Join<StorageLocationOperation>((p, o) => p.Id == o.StorageLocationId && o.IsTemporary);
            query.Where(p => p.WarehouseId == wareHouseId && p.State == State.Enable);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(info);
        }

        /// <summary>
        /// 获取暂存库位
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="info">分页信息</param>
        /// <returns>暂存库位</returns>
        public virtual EntityList<StorageLocation> GetEffectLocations(double warehouseId, string keyword, PagingInfo info)
        {
            var query = Query<StorageLocation>();

            query.Where(p => p.WarehouseId == warehouseId && p.State == State.Enable);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取仓库首个按托上架库位
        /// </summary>
        /// <param name="wareHouseId">仓库id</param>
        /// <param name="upProcessType">上架处理类型</param>
        /// <returns>仓库首个按托上架库位</returns>
        public virtual StorageLocation GetTrayUpLocation(double wareHouseId, UpProcessType upProcessType)
        {
            var query = Query<StorageLocation>();
            query.Join<StorageLocationOperation>((p, o) => p.Id == o.StorageLocationId && o.UpProcess == upProcessType);
            query.Where(p => p.WarehouseId == wareHouseId && p.State == State.Enable && !p.IsFrozen);
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 获取仓库下的所有可用库位（可用、未冻结、拣货为True）
        /// </summary>
        /// <param name="wareHouseId">仓库Id</param>
        /// <param name="pickProcessType">拣货类型</param>
        /// <returns>库位列表</returns>
        public virtual EntityList<StorageLocation> GetEnableNotPickStorageLocations(double wareHouseId, PickProcessType pickProcessType)
        {
            var query = Query<StorageLocation>();
            query.Join<StorageArea>((a, b) => a.AreaId == b.Id && !b.IsFrozen)
                 .Join<Warehouse>((a, b) => a.WarehouseId == b.Id && !b.IsFrozen)
                 .Join<StorageLocationOperation>((x, y) => x.Id == y.StorageLocationId && y.IsPick && pickProcessType == y.PickProcess);
            query.Where(p => p.WarehouseId == wareHouseId && p.State == State.Enable && !p.IsFrozen);
            return query.ToList();
        }

        /// <summary>
        /// 获取仓库下的所有可用库位（可用、未冻结、拣货为true）
        /// </summary>
        /// <param name="wareHouseId">仓库Id</param>
        /// <returns>库位列表</returns>
        public virtual EntityList<StorageLocation> GetEnableNotPickStorageLocations(double wareHouseId)
        {
            var query = Query<StorageLocation>();
            query.Join<StorageArea>((a, b) => a.AreaId == b.Id && !b.IsFrozen)
                 .Join<Warehouse>((a, b) => a.WarehouseId == b.Id && !b.IsFrozen)
                 .Join<StorageLocationOperation>((x, y) => x.Id == y.StorageLocationId && !y.IsPick);
            query.Where(p => p.WarehouseId == wareHouseId && p.State == State.Enable && !p.IsFrozen);
            return query.ToList();
        }

        /// <summary>
        /// 获取仓库下的所有可用库位（可用、未冻结、拣货为true）
        /// </summary>
        /// <param name="storageLocationId">库位Id</param>
        /// <returns>库位列表</returns>
        public virtual StorageLocation GetEnableNotPickStorageLocation(double storageLocationId)
        {
            var query = Query<StorageLocation>();
            query.Join<StorageArea>((a, b) => a.AreaId == b.Id && !b.IsFrozen)
                 .Join<Warehouse>((a, b) => a.WarehouseId == b.Id && !b.IsFrozen)
                 .Join<StorageLocationOperation>((x, y) => x.Id == y.StorageLocationId && y.IsPick);
            query.Where(p => p.Id == storageLocationId && p.State == State.Enable && !p.IsFrozen);
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 获取所有引用库区的库位
        /// </summary> 
        /// <param name="areaIdList">库区Id列表</param>
        /// <returns>返回库位列表数据</returns>
        public virtual EntityList<StorageLocation> GetStorageLocationList(List<double> areaIdList)
        {
            return areaIdList.Distinct().SplitContains(areaIds =>
            {
                return Query<StorageLocation>().Where(p => areaIds.Contains(p.AreaId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 检查库位是否已经存在
        /// </summary>
        /// <param name="whCode">仓库</param>
        /// <param name="loc">库位</param>
        /// <returns>bool</returns>
        public virtual bool CheckIsExistLoction(string whCode, string loc)
        {
            return Query<StorageLocation>().Where(p => p.Warehouse.Code == whCode && p.Code == loc).Count() > 0;
        }

        /// <summary>
        /// 获取库位By巷道Id
        /// </summary>
        /// <param name="routewayId">巷道Id</param>
        /// <param name="info">分页</param>
        /// <returns>库位</returns>
        public virtual EntityList<StorageLocation> GetStorageLocationByRouteway(double routewayId, PagingInfo info)
        {
            var query = Query<StorageLocation>().Where(p => p.RoutewayId == routewayId);

            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取所有立库库位
        /// </summary>
        /// <returns>库位</returns>
        public virtual EntityList<StorageLocation> GetIsAutoMatacLoc()
        {
            return Query<StorageLocation>().Where(p => p.IsAutomatedStorage).ToList();
        }

        /// <summary>
        /// 获取立库库位数据
        /// </summary>
        /// <param name="locIds">库位ID集合</param>
        /// <param name="isAutoMated">是否立库</param>
        /// <returns>库位数据</returns>
        public virtual EntityList<StorageLocation> GetIsAutoMatedStorageLocations(List<double> locIds, bool? isAutoMated = null)
        {
            return locIds.SplitContains(ids =>
            {
                var query = Query<StorageLocation>().Where(p => ids.Contains(p.Id));

                if (isAutoMated.HasValue)
                {
                    query.Where(p => p.IsAutomatedStorage == isAutoMated.HasValue);
                }

                return query.ToList();
            });
        }

        /// <summary>
        /// 获取仓库下的所有可用立库库位（可用、未冻结、不锁）
        /// </summary>
        /// <param name="wareHouseId">仓库Id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页信息</param>
        /// <param name="isNoLock">不锁</param>
        /// <returns>库位列表数据</returns>
        public virtual EntityList<StorageLocation> GetIsAutoMatedEnableStorageLocationDatas(double wareHouseId, string keyword, PagingInfo info, bool? isNoLock = true)
        {
            var query = Query<StorageLocation>();
            query.Where(p => p.WarehouseId == wareHouseId && p.State == State.Enable && !p.IsFrozen && p.IsAutomatedStorage);
            if (isNoLock == true)
                query.Where(p => !p.IsInLock && !p.IsOutLock && !p.IsCountLock);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword) || p.Area.Code.Contains(keyword));
            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取库区下的所有立库库位
        /// </summary>
        /// <param name="areaId">库区</param>
        /// <param name="layerNo">层</param>        
        /// <returns>库位列表数据</returns>
        public virtual EntityList<StorageLocation> GetIsAutoMatedStorageLocationsForMap(double areaId, double layerNo)
        {
            var query = Query<StorageLocation>();
            query.Where(p => p.AreaId == areaId && p.LayerNo == layerNo && p.LibraryType == LibraryType.Entity && p.RoutewayId > 0 && p.RowNo > 0 && p.ColumnNo > 0);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取库位简单字段集合
        /// </summary>
        /// <param name="locIds">库位ID</param>
        /// <returns></returns>
        public virtual List<EntityBaseData> GetEntityBaseDatas(List<double> locIds)
        {
            return DataProcessEx.SplitContains(locIds, ids =>
            {
                var query = Query<StorageLocation>().Select(p => new { p.Id, p.Code, p.State }).Where(p => ids.Contains(p.Id));
                return query.ToList<EntityBaseData>().ToList();
            });
        }

        /// <summary>
        /// 获取库位简单字段字典
        /// </summary>
        /// <param name="locIds">库位ID</param>
        /// <returns></returns>
        public virtual Dictionary<double, EntityBaseData> GetLocationBaseDataDic(List<double> locIds)
        {
            return GetEntityBaseDatas(locIds).ToDictionary(p => p.Id);
        }

        /// <summary>
        /// 获取库位简单字段
        /// </summary>
        /// <param name="locIds">库位ID</param>
        /// <returns></returns>
        public virtual List<StorageLocationData> GetStorageLocationDatas(List<double> locIds)
        {
            return DataProcessEx.SplitContains(locIds, ids =>
            {
                var query = Query<StorageLocation>().Select(p => new { p.Id, p.Code, p.State, p.IsTemporary, p.IsLayIn });
                return query.Where(p => ids.Contains(p.Id)).ToList<StorageLocationData>().ToList();
            });
        }

        /// <summary>
        /// 获取库位简单字段字典
        /// </summary>
        /// <param name="locIds">库位ID</param>
        /// <returns></returns>
        public virtual Dictionary<double, StorageLocationData> GetStorageLocationDataDic(List<double> locIds)
        {
            return GetStorageLocationDatas(locIds).ToDictionary(p => p.Id);
        }

        /// <summary>
        /// 获取最大的层数
        /// </summary>
        /// <param name="areaId">库区</param>
        /// <returns>层</returns>
        public virtual int GetMaxLayerNo(double areaId)
        {
            var query = Query<StorageLocation>();
            return query.Where(p => p.AreaId == areaId).Select(p => p.LayerNo.MAX()).FirstOrDefault<int>();
        }

        /// <summary>
        /// 更新库位锁
        /// </summary>
        /// <param name="locIds">库位ID</param>
        /// <param name="isInLock">入库锁</param>
        /// <param name="isOutLock">出库锁</param>
        /// <param name="isCountLock">盘点锁</param>
        public virtual void BatchUpdateLocationLock(List<double> locIds, bool? isInLock, bool? isOutLock, bool? isCountLock)
        {
            locIds.SplitDataExecute(ids =>
            {
                var query = DB.Update<StorageLocation>();
                if (isInLock.HasValue)
                    query.Set(p => p.IsInLock, isInLock);
                if (isOutLock.HasValue)
                    query.Set(p => p.IsOutLock, isOutLock);
                if (isCountLock.HasValue)
                    query.Set(p => p.IsCountLock, isCountLock);

                query.Where(p => ids.Contains(p.Id)).Execute();
            });
        }

        /// <summary>
        /// 更新库位锁
        /// </summary>
        /// <param name="locId">库位ID</param>
        /// <param name="isInLock">入库锁</param>
        /// <param name="isOutLock">出库锁</param>
        /// <param name="isCountLock">盘点锁</param>
        public virtual void UpdateLocationLock(double locId, bool? isInLock, bool? isOutLock, bool? isCountLock)
        {
            var query = DB.Update<StorageLocation>();
            if (isInLock.HasValue)
                query.Set(p => p.IsInLock, isInLock);
            if (isOutLock.HasValue)
                query.Set(p => p.IsOutLock, isOutLock);
            if (isCountLock.HasValue)
                query.Set(p => p.IsCountLock, isCountLock);
            query.Where(p => p.Id == locId).Execute();
        }

        /// <summary>
        /// 更新库位锁（根据同巷道、排、层、列，不同深度）
        /// </summary>
        /// <param name="routewayId">巷道ID</param>
        /// <param name="rowNo">排</param>
        /// <param name="layerNo">层</param>
        /// <param name="columnNo">列</param>
        /// <param name="depth">深度</param>
        /// <param name="isInLock">入库锁</param>
        /// <param name="isOutLock">出库锁</param>
        /// <param name="isCountLock">盘点锁</param>
        public virtual void UpdateLocationLock(double? routewayId, int rowNo, int layerNo, int columnNo, int depth, bool? isInLock, bool? isOutLock, bool? isCountLock)
        {
            var query = DB.Update<StorageLocation>();
            if (isInLock.HasValue)
            {
                query.Set(p => p.IsInLock, isInLock);
            }

            if (isOutLock.HasValue)
            {
                query.Set(p => p.IsOutLock, isOutLock);
            }

            if (isCountLock.HasValue)
            {
                query.Set(p => p.IsCountLock, isCountLock);
            }

            query.Where(p => p.RoutewayId == routewayId);
            query.Where(p => p.RowNo == rowNo && p.LayerNo == layerNo && p.ColumnNo == columnNo && p.Depth == depth).Execute();
        }

        /// <summary>
        /// 获取库位是否允许人工上架
        /// </summary>
        /// <param name="locId">库位ID</param>
        /// <returns>是否允许人工上架</returns>
        public virtual bool GetLocIsAllowManualGrounding(double locId)
        {
            bool isAllowManualGrounding = false;
            var query = Query<StorageArea>().Exists<StorageLocation>((x, y) => y.Where(p => p.AreaId == x.Id && p.Id == locId));
            StorageArea area = query.FirstOrDefault();
            if (area != null)
                isAllowManualGrounding = area.IsAllowManualGrounding;

            return isAllowManualGrounding;
        }

        /// <summary>
        /// 更新立库库位的库位锁
        /// </summary>
        /// <param name="locIds">库位</param>
        /// <param name="isOutLock">出库锁</param>
        public virtual void UpdateIsAutoMatedLocationOutLock(List<double> locIds, bool isOutLock)
        {
            EntityList<StorageLocation> isAutoMatedLocs = GetIsAutoMatedStorageLocations(locIds, true);
            isAutoMatedLocs.ForEach(loc =>
            {
                //更新库位的出库锁
                UpdateLocationLock(loc.Id, null, isOutLock, null);
                if (loc.IsMaxDepth && loc.Depth > 0)
                {
                    //更新对应的浅库位的入库锁
                    UpdateLocationLock(loc.RoutewayId, loc.RowNo, loc.LayerNo, loc.ColumnNo, 0, isOutLock, null, null);
                }
            });
        }

        /// <summary>
        /// 验证库位编码不能为空
        /// </summary>
        /// <param name="locCode">库位编码</param>
        /// <exception cref="ValidationException">目标库位不能为空</exception>
        public virtual void ValidateLocationNotNull(string locCode)
        {
            if (locCode.IsNullOrEmpty())
            {
                throw new ValidationException("库位不能为空".L10N());
            }
        }

        /// <summary>
        /// 验证目标库位编码不能为空
        /// </summary>
        /// <param name="targetLocCode">目标库位编码</param>
        /// <exception cref="ValidationException">目标库位不能为空</exception>
        public virtual void ValidateTargetLocationNotNull(string targetLocCode)
        {
            if (targetLocCode.IsNullOrEmpty())
            {
                throw new ValidationException("目标库位不能为空".L10N());
            }
        }

        /// <summary>
        /// 验证目标库位不存在或不可用
        /// </summary>
        /// <param name="targetLoc">目标库位</param>
        /// <param name="targetLocCode">目标库位编码</param>
        /// <exception cref="ValidationException">目标库位[{0}]不存在或者不可用</exception>
        public virtual void ValidateTargetLocationNotNull(StorageLocation targetLoc, string targetLocCode)
        {
            if (targetLoc == null)
            {
                throw new ValidationException("目标库位[{0}]不存在或者不可用".L10nFormat(targetLocCode));
            }

            if (targetLoc.State == State.Disable || targetLoc.IsFrozen)
            {
                throw new ValidationException("目标库位[{0}]禁用或者冻结".L10nFormat(targetLocCode));
            }
        }

        /// <summary>
        /// 验证来源仓库下的PICKTO库位不存在或不可用
        /// </summary>
        /// <param name="packToLoc">PICKTO库位</param>
        /// <exception cref="ValidationException">来源仓库下的PICKTO不存在或者不可用</exception>
        public virtual void ValidatePickToLocatopmNotNull(StorageLocation packToLoc)
        {
            if (packToLoc == null)
            {
                throw new ValidationException("来源仓库下的PICKTO不存在或者不可用".L10N());
            }
        }

        /// <summary>
        /// 验证库位是否为收货暂存库位
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="code">库位编码</param>
        /// <exception cref="ValidationException">库位[{0}]不是发货暂存库位</exception>
        public virtual void ValidateIsFocusLocation(double warehouseId, string code)
        {
            var query = Query<StorageLocationOperation>();
            query.Join<StorageLocation>((x, y) => x.StorageLocationId == y.Id && y.Code.Contains(code) && y.WarehouseId == warehouseId);
            query.Where(p => p.IsFocus);
            var count = query.Count();
            if (count <= 0)
            {
                throw new ValidationException("库位[{0}]不是发货暂存库位!".L10nFormat(code));
            }
        }

        #region 专储物料清单
        /// <summary>
        /// 删除专储物料清单
        /// </summary>
        public virtual void StorageLocationItemListDeleteData(List<double> idList)
        {
            var itemList = Query<StorageLocationItemList>().Where(p => idList.Contains(p.Id)).ToList();

            itemList.ForEach(p =>
            {
                p.PersistenceStatus = PersistenceStatus.Deleted;
            });

            RF.Save(itemList);
        }
        #endregion

        #region 包装分配获取有序库位        
        /// <summary>
        /// 包装分配获取立库排序的库位
        /// </summary>
        /// <param name="locations">库位</param>
        /// <param name="routList">巷道均分值</param>
        /// <returns></returns>
        public virtual List<StorageLocationSort> PackassignGetStorageLocation(List<StorageLocation> locations, int[] routList = null)
        {
            if (routList == null)
            {
                var key = "Routeway_" + locations.First().WarehouseCode + "_" + locations.First().AreaCode;
                routList = GetRouteList(key);
            }
            int k = 0;
            List<StorageLocationSort> rst = new List<StorageLocationSort>();
            routList.ForEach(b =>
            {
                locations.Where(a => a.RouteNo == b).OrderByDescending(a => a.ColumnNo).ThenBy(a => a.LayerNo).ThenBy(a => a.RowNo).ThenBy(a => a.Depth).ForEach(f =>
                    {
                        rst.Add(new StorageLocationSort() { StorageLocationCode = f.Code, StorageLocationId = f.Id, SortNo = k, IsMaxDeep = f.IsMaxDepth, RouteNo = f.RouteNo });
                        k++;
                    });
            });
            var zList = locations.Where(a => a.RouteNo == 0).ToList();
            zList.ForEach(f =>
            {
                rst.Add(new StorageLocationSort() { StorageLocationCode = f.Code, StorageLocationId = f.Id, SortNo = k, IsMaxDeep = f.IsMaxDepth, RouteNo = f.RouteNo });
                k++;
            });
            return rst;
        }

        /// <summary>
        /// 获取仓库巷道号
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <returns>仓库巷道号></returns>
        public virtual int[] GetWarehouseRoute(double warehouseId)
        {
            var routeNo = Query<Routeway>().Where(p => p.WarehouseId == warehouseId).Select(p => p.RoutewayNumber).Distinct().OrderBy(p => p.RoutewayNumber).ToList<int>();
            return routeNo.ToArray();
        }

        /// <summary>
        /// 获取巷道均分值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public virtual int[] GetRouteList(string key)
        {
            var routList = RT.Redis.LRange<int>(key, 0, -1);
            if (routList.Length == 0)
            {
                routList = new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            }

            return routList;
        }

        /// <summary>
        /// 获取下一个巷道均分值排序
        /// </summary>
        /// <param name="routList">排序</param>
        /// <param name="actualRoutyNo">实际巷道号</param>      
        /// <returns></returns>
        public virtual int[] GetNewRoutList(int[] routList, int actualRoutyNo)
        {
            int[] newList = new int[routList.Length];
            for (int a = 0; a < routList.Length; a++)
            {
                if (routList[a] <= actualRoutyNo)
                    routList[a] = routList[a] + 100000;
            }
            int i = 0;
            routList.OrderBy(p => p).ForEach(p =>
            {
                if (p > 100000)
                {
                    p -= 100000;
                }
                newList[i] = p;
                i++;
            });
            return newList;
        }

        #endregion

        /// <summary>
        /// 获取库位冻结原因
        /// </summary>
        /// <param name="locId">库位ID</param>
        /// <returns>冻结原因列表</returns>
        public virtual EntityList<StorageLocationFrozenReason> GetStorageLocationFrozenReasons(double locId)
        {
            return Query<StorageLocationFrozenReason>().Where(p => p.StorageLocationId == locId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取库位冻结原因
        /// </summary>
        /// <param name="locIds">库位ID集合</param>
        /// <returns>冻结原因列表</returns>
        public virtual EntityList<StorageLocationFrozenReason> GetLocFrozenReasons(List<double> locIds)
        {
            return locIds.Distinct().SplitContains(pLocIds =>
            {
                return Query<StorageLocationFrozenReason>().Where(p => pLocIds.Contains(p.StorageLocationId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 批量插入冻结原因页签数据
        /// </summary>
        /// <param name="locIds">库位ID</param>
        /// <param name="frozenReason">冻结原因</param>
        /// <param name="reasonDesc">冻结原因描述</param>
        public virtual void BatchInsertLocFrozenReason(List<double> locIds, FrozenReason frozenReason, string reasonDesc)
        {
            EntityList<StorageLocationFrozenReason> locFrozenReasons = new EntityList<StorageLocationFrozenReason>();
            locIds.ForEach(locId =>
            {
                locFrozenReasons.Add(new StorageLocationFrozenReason
                {
                    StorageLocationId = locId,
                    FrozenReason = frozenReason,
                    ReasonDesc = reasonDesc
                });
            });

            if (locFrozenReasons.Any())
            {
                RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchInsertSave(locFrozenReasons);
            }
        }

        /// <summary>
        /// 批量删除冻结原因页签数据
        /// </summary>
        /// <param name="locIds">库位ID</param>
        /// <param name="frozenReason">冻结原因</param>
        public virtual void BatchDeleteLocFrozenReason(List<double> locIds, FrozenReason? frozenReason)
        {
            locIds.Distinct().SplitDataExecute(pLocIds =>
            {
                var query = DB.Delete<StorageLocationFrozenReason>();
                if (frozenReason.HasValue)
                {
                    if (frozenReason != FrozenReason.LocFrozen)
                    {
                        query.Where(p => p.FrozenReason == frozenReason.Value);
                    }
                    else
                    {
                        query.Where(p => p.FrozenReason != FrozenReason.WarehouseFrozen && p.FrozenReason != FrozenReason.AreaFrozen);
                    }
                }

                query.Where(p => pLocIds.Contains(p.StorageLocationId)).Execute();
            });
        }

        /// <summary>
        /// 更新库位是否冻结
        /// </summary>
        /// <param name="locId">库位ID</param>
        public virtual void UpdateLocIsFrozen(double locId)
        {
            var locFrozenReasons = GetStorageLocationFrozenReasons(locId);
            var query = DB.Update<StorageLocation>();
            query.Set(p => p.IsFrozen, locFrozenReasons.Any()).Where(p => p.Id == locId).Execute();
        }

        /// <summary>
        /// 批次更新库位是否冻结
        /// </summary>
        /// <param name="locations">库位数据</param>
        /// <param name="locIds">库位ID集合</param>
        public virtual void BatchUpdateLocIsFrozen(EntityList<StorageLocation> locations, List<double> locIds)
        {
            //根据库位ID集合获取库位冻结原因数据
            var locFrozenReasonList = GetLocFrozenReasons(locIds);
            locations.Where(p => locIds.Contains(p.Id)).ForEach(p =>
            {
                var forzenReasons = locFrozenReasonList.Where(t => t.StorageLocationId == p.Id).ToList();
                p.IsFrozen = forzenReasons.Any();
            });

            RF.Save(locations);
        }
        #endregion

        #region 库区
        /// <summary>
        /// 获取库区
        /// </summary>
        /// <param name="libraryType">库类型</param>
        /// <param name="warehousesId">仓库Id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页信息</param>
        /// <returns>库区列表</returns>
        public virtual EntityList<StorageArea> GetStorageArea(LibraryType? libraryType, double warehousesId, string keyword, PagingInfo info)
        {
            var query = Query<StorageArea>().Where(p => p.WarehouseId == warehousesId && p.State == State.Enable);

            if (!string.IsNullOrEmpty(keyword))
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));

            if (libraryType == LibraryType.Entity)
                query.Where(p => p.LibraryType == libraryType);

            return query.ToList(info);
        }

        /// <summary>
        /// 查询货区（可用、未冻结）
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="isAllowManualGrounding">是否允许人工上架</param>
        /// <returns>货区列表</returns>
        public virtual EntityList<StorageArea> GetStorageAreas(double warehouseId, string keyword, PagingInfo pagingInfo, bool? isAllowManualGrounding = null)
        {
            var q = Query<StorageArea>();
            q.Where(p => p.WarehouseId == warehouseId && p.State == State.Enable && !p.IsFrozen);

            if (!keyword.IsNullOrEmpty())
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            if (isAllowManualGrounding.HasValue)
                q.Where(p => p.IsAllowManualGrounding == isAllowManualGrounding.Value);

            return q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据仓库获取库区
        /// </summary>
        /// <param name="warehouses">仓库集合，已“;”分隔</param>
        /// <param name="areaIds">库区ID集合，已“;”分隔</param>
        /// <returns>库区</returns>
        public virtual EntityList<StorageArea> GetStorageAreas(string warehouses, string areaIds)
        {
            var q = Query<StorageArea>();

            if (!warehouses.IsNullOrEmpty())
                q.Where(p => warehouses.Split(';', StringSplitOptions.None).ToList().Contains(p.Warehouse.Code));

            if (!areaIds.IsNullOrEmpty())
            {
                var list = areaIds.Split(';').Select(t => Convert.ToDouble(t)).ToList();
                q.Where(p => list.Contains(p.Id));
            }
            return q.ToList();
        }

        /// <summary>
        /// 根据仓库Id集合获取库区集合
        /// </summary>
        /// <param name="warehouseIds">仓库Id集合</param>
        /// <returns>库区</returns>
        public virtual EntityList<StorageArea> GetStorageAreasByWarehouseIds(List<double> warehouseIds)
        {
            if (warehouseIds.IsNullOrEmpty())
                return new EntityList<StorageArea>();

            return warehouseIds.SplitContains(items =>
            {
                return Query<StorageArea>().Where(c => items.Contains(c.WarehouseId)).ToList();
            });
        }

        /// <summary>
        /// 获取库区（可用，非冻结）
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="codeList">仓库编号集合</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页对象</param>
        /// <returns>库区集合</returns>
        public virtual EntityList<StorageArea> GetStorageAreasList(double? warehouseId, List<string> codeList, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<StorageArea>().Where(p => p.State == State.Enable);

            if (warehouseId.HasValue)
                query.Where(p => p.WarehouseId == warehouseId);
            query.Where(p => codeList.Contains(p.Code));

            if (!string.IsNullOrEmpty(keyword))
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 根据库区编码集合获取仓库有效（非禁用，非冻结）库区信息
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="code">库区编码</param>
        /// <returns>库位信息</returns>
        public virtual StorageArea GetEffectiveAreaForReplensh(double warehouseId, string code)
        {
            var query = Query<StorageArea>()
                .Join<Warehouse>((a, b) => a.WarehouseId == b.Id && !b.IsFrozen);
            query.Where(p => p.State == State.Enable && !p.IsFrozen && p.WarehouseId == warehouseId && p.Code == code);
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 根据库区编码集合获取仓库有效（非禁用，非冻结）库区信息
        /// </summary>
        /// <param name="warehouseCode">仓库编码</param>
        /// <param name="code">库区编码</param>
        /// <returns>库位信息</returns>
        public virtual StorageArea GetEffectiveArea(string warehouseCode, string code)
        {
            var query = Query<StorageArea>()
                .Join<Warehouse>((a, b) => a.WarehouseId == b.Id && !b.IsFrozen);
            query.Where(p => p.State == State.Enable && !p.IsFrozen && p.Code == code).Where<Warehouse>((x, y) => y.Code == warehouseCode);
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 获取库区基本资料
        /// </summary>
        /// <param name="storageareaId">库区ID</param>
        /// <returns>返回库区基本资料</returns>
        public virtual StorageAreaInfo GetStorageAreaInfoDetail(double storageareaId)
        {
            return Query<StorageAreaInfo>().Where(p => p.StorageAreaId == storageareaId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取库区操作管理资料
        /// </summary>
        /// <param name="storageareaId">库区ID</param>
        /// <returns>返回库区操作管理资料</returns>
        public virtual StorageAreaOperation GetStorageAreaOperationDetail(double storageareaId)
        {
            return Query<StorageAreaOperation>().Where(p => p.StorageAreaId == storageareaId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取库区操作管理资料
        /// </summary>
        /// <param name="areaIds">库区ID集合</param>
        /// <returns>返回库区操作管理资料</returns>
        public virtual EntityList<StorageAreaOperation> GetStorageAreaOperations(List<double> areaIds)
        {
            return areaIds.Distinct().SplitContains(pAreaIds =>
            {
                var query = Query<StorageAreaOperation>().Where(p => pAreaIds.Contains(p.StorageAreaId));
                return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 根据库位集合获取库区操作集合数据
        /// </summary>
        /// <param name="storageLocationId">库位Id</param>
        /// <param name="elo">贪婪加载对象</param>
        /// <returns>返回库区操作集合数据</returns>
        public virtual EntityList<StorageAreaOperation> GetStorageAreaOperations(double storageLocationId, EagerLoadOptions elo)
        {
            return Query<StorageAreaOperation>().Where(p => p.UpTransitLocationId == storageLocationId || p.DownTransitLocationId == storageLocationId).ToList(null, elo);
        }

        /// <summary>
        /// 获取出库操作管理资料
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <returns>返回库区操作管理资料</returns>
        public virtual EntityList<StorageAreaOperation> GetStorageAreaOperationsByWhId(double warehouseId)
        {
            var query = Query<StorageAreaOperation>().Where(p => p.StorageArea.WarehouseId == warehouseId);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取库区下架过渡库位
        /// </summary>
        /// <param name="areaId">库区ID</param>
        /// <param name="elo">贪懒加载</param>
        /// <returns>库区下架过渡库位</returns>
        public virtual StorageLocation GetDownTransitLocation(double areaId, EagerLoadOptions elo = null)
        {
            var query = Query<StorageLocation>()
                .Join<StorageAreaOperation>((l, o) => l.Id == o.DownTransitLocationId && o.StorageAreaId == areaId);
            return query.FirstOrDefault(elo);
        }

        /// <summary>
        /// 获取库区上架过渡库位
        /// </summary>
        /// <param name="areaId">库区ID</param>
        /// <param name="elo">贪懒加载</param>
        /// <returns>库区下架过渡库位</returns>
        public virtual StorageLocation GetUpTransitLocation(double areaId, EagerLoadOptions elo = null)
        {
            var query = Query<StorageLocation>()
                .Join<StorageAreaOperation>((l, o) => l.Id == o.UpTransitLocationId && o.StorageAreaId == areaId);
            return query.FirstOrDefault(elo);
        }

        /// <summary>
        /// 获取最深库位
        /// </summary>
        /// <param name="location">库位</param>
        /// <param name="elo">贪懒加载</param>
        /// <returns>最深库位</returns>
        public virtual StorageLocation GetDepthLocationByLocation(StorageLocation location, EagerLoadOptions elo = null)
        {
            return Query<StorageLocation>().Where(p => p.AreaId == location.AreaId && p.IsMaxDepth).Where(p => p.RowNo == location.RowNo
            && p.ColumnNo == location.ColumnNo && p.LayerNo == location.LayerNo).FirstOrDefault(elo);
        }

        /// <summary>
        /// 添加立库库位检查深度大于0的库位，有没有建立深度比他小的库位
        /// </summary>
        /// <param name="location">库位</param>
        /// <returns>bool</returns>
        public virtual bool CheckFrontDeepLocation(StorageLocation location)
        {
            return Query<StorageLocation>().Where(p => p.AreaId == location.AreaId && p.RowNo == location.RowNo && p.ColumnNo == location.ColumnNo && p.LayerNo == location.LayerNo && p.Depth == (location.Depth - 1)).Count() > 0;
        }

        /// <summary>
        /// 获取库区立库配置资料
        /// </summary>
        /// <param name="storageareaId">库区ID</param>
        /// <returns>返回库区立库配置资料</returns>
        public virtual StorageAreaWcs GetStorageAreaWcs(double storageareaId)
        {
            return Query<StorageAreaWcs>().Where(p => p.StorageAreaId == storageareaId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取库区立库配置资料
        /// </summary>
        /// <param name="storageareaIds">库区ID</param>
        /// <returns>返回库区立库配置资料</returns>
        public virtual EntityList<StorageAreaWcs> GetStorageAreaWcss(List<double> storageareaIds)
        {
            return storageareaIds.Distinct().SplitContains(areaIds =>
            {
                var query = Query<StorageAreaWcs>().Where(p => areaIds.Contains(p.StorageAreaId));
                return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取库区查询数据
        /// </summary>
        /// <param name="criteria">库区查询条件</param>
        /// <returns>返回库区数据</returns>
        public virtual EntityList<StorageArea> GetStorageAreaData(StorageAreaCriteria criteria)
        {
            var q = Query<StorageArea>();
            ////仓库权限关联查询
            RT.Service.Resolve<WarehouseController>().ExistWarehouseEmplyee(q, StorageArea.WarehouseIdProperty);
            q.Join<Warehouse>("b", (a, b) => a.WarehouseId == b.Id);
            if (criteria != null)
            {
                if (!string.IsNullOrEmpty(criteria.Code))
                    q.Where(p => p.Code.Contains(criteria.Code));
                if (!string.IsNullOrEmpty(criteria.Name))
                    q.Where(p => p.Name.Contains(criteria.Name));
                if (criteria.LibraryType.HasValue)
                    q.Where(p => p.LibraryType == criteria.LibraryType.Value);
                if (!string.IsNullOrEmpty(criteria.Warehouse))
                    q.Where<Warehouse>((a, b) => b.Code.Contains(criteria.Warehouse) || b.Name.Contains(criteria.Warehouse));
                if (!criteria.Warehouses.IsNullOrEmpty())
                    q.Where<Warehouse>((a, b) => criteria.Warehouses.Split(';', StringSplitOptions.None).ToList().Contains(b.Code));
                if (criteria.IsFrozen.HasValue)
                    q.Where(p => p.IsFrozen == criteria.IsFrozen.Value);
                if (criteria.State.HasValue)
                    q.Where(p => p.State == criteria.State);
                if (criteria.IsNotAutomated)
                    q.Where(p => !p.IsAutomatedArea);
            }

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(StorageArea.WarehouseProperty);
            elo.LoadWithViewProperty();
            q.OrderBy(criteria.OrderInfoList);
            return q.ToList(criteria.PagingInfo, elo);
        }

        /// <summary>
        /// 获取库区查询数据
        /// </summary>
        /// <param name="criteria">库区查询条件</param>
        /// <returns>返回库区数据</returns>
        public virtual EntityList<StorageArea> GetStorageAreaData(MultiQueryAreaCriteria criteria)
        {
            var query = Query<StorageArea>();
            query.Where(p => p.State == State.Enable);
            if (criteria != null)
            {
                if (!string.IsNullOrEmpty(criteria.Code))
                    query.Where(p => p.Code.Contains(criteria.Code));
                if (!string.IsNullOrEmpty(criteria.Name))
                    query.Where(p => p.Name.Contains(criteria.Name));
                if (criteria.LibraryType.HasValue)
                    query.Where(p => p.LibraryType == criteria.LibraryType.Value);
                if (!criteria.Warehouse.IsNullOrEmpty())
                    query.Where(p => p.Warehouse.Code.Contains(criteria.Warehouse));
                if (!criteria.Warehouses.IsNullOrEmpty())
                    query.Join<Warehouse>("b", (a, b) => a.WarehouseId == b.Id && (criteria.Warehouses.Split(';', StringSplitOptions.None).ToList().Contains(b.Code)));
                if (criteria.IsFrozen.HasValue)
                    query.Where(p => p.IsFrozen == criteria.IsFrozen.Value);
                if (criteria.FilterId != null && criteria.FilterId.Count > 0)
                {
                    query.Where(p => !criteria.FilterId.ToList().Contains(p.Id));
                }
            }

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(StorageArea.WarehouseProperty);
            elo.LoadWithViewProperty();
            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, elo);
        }

        /// <summary>
        /// 获取仓库数据列表
        /// </summary> 
        /// <param name="type">基本分类</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pagingInfo">分页对象</param>
        /// <returns>仓库数据列表</returns>
        public virtual EntityList<Warehouse> GetWarehouseDataList(LibraryType type, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<Warehouse>().Where(p => p.State == State.Enable);
            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Name.Contains(keyword) || p.Code.Contains(keyword));
            }
            if (type == LibraryType.Entity)
                query.Where(p => p.LibraryType == type);

            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取所有引用仓库的库区
        /// </summary> 
        /// <param name="whIdlist">仓库Id列表</param>
        /// <returns>返回库区列表数据</returns>
        public virtual EntityList<StorageArea> GetStorageAreaForWHList(List<double> whIdlist)
        {
            return whIdlist.Distinct().SplitContains(whIds =>
            {
                var query = Query<StorageArea>();
                return query.Where(p => whIds.Contains(p.WarehouseId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取库区数据
        /// </summary> 
        /// <param name="idlist">库区Id列表</param>
        /// <returns>返回库区列表数据</returns>
        public virtual EntityList<StorageArea> GetStorageAreaList(List<double> idlist)
        {
            return idlist.SplitContains(ids =>
            {
                var query = Query<StorageArea>();
                return query.Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 库区禁用
        /// </summary>
        /// <param name="areaIdList">库区Id列表</param>
        public virtual void DisableStorageAreas(List<double> areaIdList)
        {
            var areaList = GetStorageAreaList(areaIdList);
            var locationList = GetStorageLocationList(areaIdList);

            if (locationList.Any(p => p.State != State.Disable))
                throw new ValidationException("库位[{0}]没有被禁用，库区不能被禁用".L10nFormat(string.Join(",", locationList.Where(p => p.State != State.Disable).Select(p => p.Code).ToList())));

            if (areaList.Any(p => p.State != State.Enable))
            {
                throw new ValidationException("库区[{0}]不是启用状态，不能禁用".L10nFormat(string.Join(",", areaList.Where(p => p.State != State.Enable).Select(p => p.Code).ToList())));
            }

            areaList.ForEach(p => p.State = State.Disable);
            RF.Save(areaList);
        }

        /// <summary>
        /// 库区启用
        /// </summary>
        /// <param name="areaIdList">库区Id列表</param>
        public virtual void EnabelStorageAreas(List<double> areaIdList)
        {
            var areaList = GetStorageAreaList(areaIdList);
            var whIdList = areaList.Select(p => p.WarehouseId).Distinct().ToList();
            var warehouseList = GetWarehouses(whIdList);
            if (warehouseList.Any(p => p.State == State.Disable))
            {
                throw new ValidationException("库区[{0}]所属的仓库被禁用，库区不能启用!".L10nFormat(string.Join(",", warehouseList.Where(p => p.State == State.Disable).Select(p => p.Code).ToList())));
            }

            areaList.ForEach(p => p.State = State.Enable);
            RF.Save(areaList);
        }

        /// <summary>
        /// 库区冻结/解冻
        /// </summary>
        /// <param name="areaIdList">库区Id列表</param>
        public virtual void FrozenStorageAreas(List<double> areaIdList)
        {
            var areaList = GetStorageAreaList(areaIdList);
            var whIdList = areaList.Select(p => p.WarehouseId).Distinct().ToList();
            var warehouseList = GetWarehouses(whIdList);
            if (warehouseList.Any(p => p.IsFrozen))
                throw new ValidationException("仓库被冻结，库区[{0}]不能冻结解冻操作".L10nFormat(string.Join(",", warehouseList.Where(p => p.IsFrozen).Select(p => p.Code).ToList())));

            using (var tran = DB.TransactionScope(WareHouseEntityDataProvider.ConnectionStringName))
            {
                //更新冻结库位是否冻结ID集合
                List<double> isFrozenLocIds = new List<double>();

                //库区冻结ID集合
                List<double> frozenAreaIds = areaList.Where(p => p.IsFrozen).Select(p => p.Id).ToList();
                if (frozenAreaIds.Any())
                {
                    //库区冻结ID集合存在，则表示为解冻操作,当库区是解冻时，将库区下的所有库位的“库区冻结”数据删除
                    var frozenLocs = GetStorageLocationList(frozenAreaIds);
                    List<double> frozenLocIds = frozenLocs.Select(p => p.Id).ToList();
                    BatchDeleteLocFrozenReason(frozenLocIds, FrozenReason.AreaFrozen);
                    isFrozenLocIds.AddRange(frozenLocIds);
                }

                //未冻结库区ID集合
                List<double> unFrozenAreaIds = areaList.Where(p => !p.IsFrozen).Select(p => p.Id).ToList();
                if (unFrozenAreaIds.Any())
                {
                    //未冻结库区ID集合存在,则表示为冻结操作,当库区是冻结时，将库区下的所有库位的冻结原因插入“库区冻结”数据；
                    var unfrozenLocs = GetStorageLocationList(unFrozenAreaIds);
                    List<double> unfrozenLocIds = unfrozenLocs.Select(p => p.Id).ToList();
                    BatchInsertLocFrozenReason(unfrozenLocIds, FrozenReason.AreaFrozen, string.Empty);
                    isFrozenLocIds.AddRange(unfrozenLocIds);
                }

                //对库区下的每个库位执行“库位冻结状态更新通用方法
                EntityList<StorageLocation> locs = GetStorageLocations(isFrozenLocIds.Distinct().ToList(), new EagerLoadOptions().LoadWithViewProperty());
                BatchUpdateLocIsFrozen(locs, isFrozenLocIds.Distinct().ToList());

                areaList.ForEach(p => p.IsFrozen = !p.IsFrozen);
                RF.Save(areaList);

                tran.Complete();
            }
        }

        /// <summary>
        /// 获取库区规则
        /// </summary>
        /// <returns>库区规则编码</returns>
        public virtual NumberRule GetStorageAreaNumberRule()
        {
            var config = ConfigService.GetConfig(new StorageAreaCodeConfig(), typeof(StorageArea));
            if (config == null || config.StorageAreaCodeRule == null)
                throw new ValidationException("未找到库区编码验证规则,请检查规则配置".L10N());
            return config.StorageAreaCodeRule;
        }

        /// <summary>
        /// 保存前验证库区编码规则
        /// </summary>
        /// <param name="storageareaList">选择库区列表</param>
        public virtual void StorageAreaValidateSegment(List<StorageArea> storageareaList)
        {
            var codeRule = GetStorageAreaNumberRule();
            foreach (StorageArea storagearea in storageareaList)
            {
                if (!RT.Service.Resolve<NumberRuleController>().ValidateSegment(codeRule, storagearea.Code, storagearea))
                    throw new ValidationException("库区编码不符合配置的编码规则".L10N());
            }
        }

        /// <summary>
        /// 获取库区编号
        /// </summary>
        /// <returns>库区编号</returns>
        public virtual string GetStorageAreaCode()
        {
            var config = ConfigService.GetConfig(new StorageAreaCodeConfig(), typeof(StorageArea));
            if (config.StorageAreaCodeRule != null)
            {
                return RT.Service.Resolve<NumberRuleController>()
                    .GenerateSegment(config.StorageAreaCodeRule.Id, 1)
                    .FirstOrDefault();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取仓库下的所有可用库区
        /// </summary>
        /// <param name="wareHouseId">仓库Id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页信息</param>
        /// <param name="isAutomatedArea">是否立库</param>
        /// <returns>库区列表数据</returns>
        public virtual EntityList<StorageArea> GetEnableStorageAreas(double? wareHouseId, string keyword, PagingInfo info, bool? isAutomatedArea = null)
        {
            var query = Query<StorageArea>().Where(p => p.State == State.Enable);
            if (wareHouseId != null)
            {
                query.Where(p => p.WarehouseId == wareHouseId);
            }

            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            if (isAutomatedArea.HasValue)
            {
                query.Where(p => p.IsAutomatedArea == isAutomatedArea.Value);
            }

            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 获取仓库下的所有可用库区
        /// </summary>
        /// <param name="wareHouseCodes">仓库编码</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页信息</param>
        /// <param name="isAutomatedArea">是否立库</param>
        /// <returns>库区列表数据</returns>
        public virtual EntityList<StorageArea> GetEnableStorageAreasByWhCode(string wareHouseCodes, string keyword, PagingInfo info, bool? isAutomatedArea = null)
        {
            var query = Query<StorageArea>().Where(p => p.State == State.Enable);
            if (wareHouseCodes.IsNotEmpty())
            {
                query.Where(p => wareHouseCodes.Contains(p.Warehouse.Code));
            }

            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            if (isAutomatedArea.HasValue)
            {
                query.Where(p => p.IsAutomatedArea == isAutomatedArea.Value);
            }

            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过库区code列表获取库区列表
        /// </summary>
        /// <param name="codeList">库区code列表</param>
        /// <returns>库区列表</returns>
        public virtual EntityList<StorageArea> GetAreaList(List<string> codeList)
        {
            return codeList.SplitContains(codes =>
            {
                return Query<StorageArea>().Where(p => codes.Contains(p.Code)).ToList();
            });
        }
        #endregion

        #region 仓库
        /// <summary>
        /// 获取仓库编码规则
        /// </summary>
        /// <returns>仓库编码规则</returns>
        public virtual NumberRule GetWarehouseNumberRule()
        {
            var config = ConfigService.GetConfig(new WarehousesCodeConfig(), typeof(Warehouse));
            if (config == null || config.WarehousesCodeRule == null)
                throw new ValidationException("未找到仓库编码验证规则,请检查规则配置".L10N());
            return config.WarehousesCodeRule;
        }

        /// <summary>
        /// 获取仓库
        /// </summary>
        /// <param name="criteria">仓库查询实体</param>
        /// <returns>仓库集合</returns>
        public virtual EntityList<Warehouse> GetWarehouseData(WarehouseCriteria criteria)
        {
            var query = Query<Warehouse>();
            if (criteria != null)
            {
                if (!string.IsNullOrEmpty(criteria.Code))
                    query.Where(p => p.Code.Contains(criteria.Code));
                if (!string.IsNullOrEmpty(criteria.Name))
                    query.Where(p => p.Name.Contains(criteria.Name));
                if (criteria.LibraryType.HasValue)
                    query.Where(p => p.LibraryType == criteria.LibraryType.Value);
                if (criteria.Category.IsNotEmpty())
                    query.Where(p => p.Category.Contains(criteria.Category));
                if (criteria.IsFrozen.HasValue)
                    query.Where(p => p.IsFrozen == criteria.IsFrozen.Value);
                if (criteria.State.HasValue)
                    query.Where(p => p.State == criteria.State.Value);
                if (criteria.IsLine.HasValue)
                {
                    query.Where(p => p.IsLineWarehouse == criteria.IsLine.Value);
                }
                if (criteria.IsEmployeeWarehouse)
                    query.Exists<WarehouseEmployee>((p, e) => e.Where(t => t.WarehouseId == p.Id && t.EmployeeId == RT.IdentityId));

                if (criteria.IsAutomated.HasValue)
                {
                    query.Exists<StorageArea>((p, s) => s.Where(t => t.WarehouseId == p.Id && t.IsAutomatedArea == criteria.IsAutomated.Value));
                }
            }

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, elo);
        }

        /// <summary>
        /// 获取仓库
        /// </summary>
        /// <param name="criteria">仓库查询实体</param>
        /// <returns>仓库集合</returns>
        public virtual EntityList<Warehouse> GetAllWarehouseData(InWarehouseEmployeeSelectCriteria criteria)
        {
            var result = new EntityList<Warehouse>();
            var allOrg = RF.GetAll<InvOrg>();
            string invOrgId = RF.Find<Warehouse>().EntityMeta.Property(InvOrgIdExtension.INV_ORG_IDProperty).ColumnMeta.ColumnName;
            using (SIE.Common.InvOrg.InvOrgs.WithAll())
            {
                var query = DB.Query<Warehouse>("T00");
                if (criteria != null)
                {
                    if (!string.IsNullOrEmpty(criteria.Code))
                        query.Where(p => p.Code.Contains(criteria.Code));
                    if (!string.IsNullOrEmpty(criteria.Name))
                        query.Where(p => p.Name.Contains(criteria.Name));
                    if (criteria.LibraryType.HasValue)
                        query.Where(p => p.LibraryType == criteria.LibraryType.Value);
                    if (criteria.Category.IsNotEmpty())
                        query.Where(p => p.Category.Contains(criteria.Category));
                    if (criteria.IsFrozen.HasValue)
                        query.Where(p => p.IsFrozen == criteria.IsFrozen.Value);
                    if (criteria.State.HasValue)
                        query.Where(p => p.State == criteria.State.Value);
                    if (criteria.IsLine.HasValue)
                    {
                        query.Where(p => p.IsLineWarehouse == criteria.IsLine.Value);
                    }
                    //if (!criteria.InvOrgCode.IsNullOrEmpty())
                    //{
                    //    query.Where(p => p.SQL<bool>(new FormattedSql(" T00.{0} = {1}".FormatArgs(invOrgId, criteria.InvOrgCode))));
                    //}
                    if (criteria.InvOrgId.HasValue)
                    {
                        query.Where(p => p.SQL<bool>(new FormattedSql(" T00.{0} = {1}".FormatArgs(invOrgId, criteria.InvOrgId))));
                    }
                    if (criteria.IsEmployeeWarehouse)
                        query.Exists<WarehouseEmployee>((p, e) => e.Where(t => t.WarehouseId == p.Id && t.EmployeeId == RT.IdentityId));

                    if (criteria.IsAutomated.HasValue)
                    {
                        query.Exists<StorageArea>((p, s) => s.Where(t => t.WarehouseId == p.Id && t.IsAutomatedArea == criteria.IsAutomated.Value));
                    }
                }
                EagerLoadOptions elo = new EagerLoadOptions();
                elo.LoadWithViewProperty();
                //query.OrderBy(criteria.OrderInfoList).OrderBy(p=>p.Code).OrderBy(p=>p.Name);
                result = query.ToList(criteria.PagingInfo, null);
                result.ForEach(p =>
                {
                    var wareInvOrgId = p.GetInvOrgId();
                    var org = allOrg.FirstOrDefault(a => a.Code == wareInvOrgId);
                    p.InvOrgCode = org.Code;
                    p.InvOrgName = org.Name;
                });
                return result;
                //return query.ToList(criteria.PagingInfo, elo);
            }

        }

        /// <summary>
        /// 获取ERP子库
        /// </summary>
        /// <param name="criteria">ERP子库查询实体</param>
        /// <returns>ERP子库集合</returns>
        public virtual EntityList<ErpWarehouse> GetErpWarehouseData(ErpWarehouseCriteria criteria)
        {
            using (SIE.Common.InvOrg.InvOrgs.WithAll())
            {
                var query = Query<ErpWarehouse>();
                if (!string.IsNullOrEmpty(criteria.Code))
                    query.Where(p => p.Code.Contains(criteria.Code));
                if (!string.IsNullOrEmpty(criteria.Name))
                    query.Where(p => p.Name.Contains(criteria.Name));
                if (!string.IsNullOrEmpty(criteria.WmsInvOrg))
                    query.Where(p => p.WmsInvOrg.Contains(criteria.WmsInvOrg));
                if (!string.IsNullOrEmpty(criteria.WarehouseCode) || !string.IsNullOrEmpty(criteria.StorageAreaCode) || !string.IsNullOrEmpty(criteria.StorageLocationCode))
                {
                    var dtlList = Query<ErpWarehouseDetail>()
                        .WhereIf(!string.IsNullOrEmpty(criteria.WarehouseCode), p => p.Warehouse.Code.Contains(criteria.WarehouseCode) || p.Warehouse.Name.Contains(criteria.WarehouseCode))
                        .WhereIf(!string.IsNullOrEmpty(criteria.StorageAreaCode), p => p.Area.Code.Contains(criteria.StorageAreaCode) || p.Area.Name.Contains(criteria.StorageAreaCode))
                        .WhereIf(!string.IsNullOrEmpty(criteria.StorageLocationCode), p => p.StorageLocation.Code.Contains(criteria.StorageLocationCode) || p.StorageLocation.Name.Contains(criteria.StorageLocationCode))
                        .Select(p => p.ErpWarehouseId).Distinct().ToList<double>();

                    query.Where(p => dtlList.Contains(p.Id));
                }

                EagerLoadOptions elo = new EagerLoadOptions();
                elo.LoadWithViewProperty();
                query.OrderBy(criteria.OrderInfoList);
                return query.ToList(criteria.PagingInfo, elo);
            }
        }

        /// <summary>
        /// 获取仓库
        /// </summary>
        /// <param name="criteria">多选仓库查询实体</param>
        /// <returns>仓库集合</returns>
        public virtual EntityList<Warehouse> GetMultiWarehouses(MultiQueryWhCriteria criteria)
        {
            var query = Query<Warehouse>();
            query.Where(p => p.State == State.Enable);
            if (criteria != null)
            {
                if (!string.IsNullOrEmpty(criteria.Code))
                    query.Where(p => p.Code.Contains(criteria.Code));
                if (!string.IsNullOrEmpty(criteria.Name))
                    query.Where(p => p.Name.Contains(criteria.Name));
                if (criteria.LibraryType.HasValue)
                    query.Where(p => p.LibraryType == criteria.LibraryType.Value);
                if (criteria.IsFrozen.HasValue)
                    query.Where(p => p.IsFrozen == criteria.IsFrozen.Value);
                if (criteria.FilterId != null && criteria.FilterId.Count > 0)
                {
                    query.Where(p => !criteria.FilterId.ToList().Contains(p.Id));
                }
            }

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            return query.ToList(criteria.PagingInfo, elo);
        }

        /// <summary>
        /// 根据仓库Id获取仓库数据
        /// </summary>
        /// <param name="warehouseIdList">仓库Id</param>
        /// <returns>返回仓库数据</returns>
        public virtual EntityList<Warehouse> GetWarehouses(List<double> warehouseIdList)
        {
            if (!warehouseIdList.Any())
                return new EntityList<Warehouse>();
            return warehouseIdList.SplitContains(ids =>
            {
                var query = Query<Warehouse>();
                return query.Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 查询仓库列表
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<Warehouse> GetWarehouses(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Warehouse>();
            if (keyword.IsNotEmpty())
                query.Where(p => p.Name.Contains(keyword) || p.Code.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取仓库
        /// </summary>
        /// <param name="codes">编码集合，已“;”分隔</param>
        /// <returns>仓库列表</returns>
        public virtual EntityList<Warehouse> GetWarehouses(string codes)
        {
            var query = Query<Warehouse>();
            if (!codes.IsNullOrEmpty())
            {
                query.Where(p => codes.Split(';', StringSplitOptions.None).ToList().Contains(p.Code));
            }

            return query.ToList();
        }

        /// <summary>
        /// 根据仓库Id获取仓库数据
        /// </summary>
        /// <param name="warehouseIdList">仓库Id</param>
        /// <returns>返回仓库数据</returns>
        public virtual List<double> GetWarehousesIngoreOnhand(List<double> warehouseIdList)
        {
            List<double> rst = new List<double>();
            if (!warehouseIdList.Any())
                return rst;
            warehouseIdList.SplitDataExecute(ids =>
           {
               var query = Query<Warehouse>();
               rst.AddRange(query.Where(p => ids.Contains(p.Id) && p.IngoreOnhand).Select(p => p.Id).ToList<double>());
           });
            return rst;
        }

        /// <summary>
        /// 根据仓库Id获取可用仓库数据
        /// </summary>
        /// <param name="warehouseIdList">仓库Id</param>
        /// <returns>返回仓库数据</returns>
        public virtual EntityList<Warehouse> GetEnableWarehouses(List<double> warehouseIdList)
        {
            return warehouseIdList.SplitContains(ids =>
            {
                var query = Query<Warehouse>();
                return query.Where(p => ids.Contains(p.Id) && p.State == State.Enable).ToList(null);
            });
        }

        /// <summary>
        /// 保存前验证仓库编码规则
        /// </summary>
        /// <param name="warehouseList">仓库</param>
        public virtual void WarehouseValidateSegment(List<Warehouse> warehouseList)
        {
            var codeRule = GetWarehouseNumberRule();
            foreach (Warehouse warehouse in warehouseList)
            {
                if (!RT.Service.Resolve<NumberRuleController>().ValidateSegment(codeRule, warehouse.Code, warehouse))
                    throw new ValidationException("仓库编码不符合配置的编码规则".L10N());
            }
        }

        /// <summary>
        /// 获取仓库编号
        /// </summary>
        /// <returns>仓库编号</returns>
        public virtual string GetWarehouseCode()
        {
            var config = ConfigService.GetConfig(new WarehousesCodeConfig(), typeof(Warehouse));
            if (config.WarehousesCodeRule != null)
            {
                return RT.Service.Resolve<NumberRuleController>()
                    .GenerateSegment(config.WarehousesCodeRule.Id, 1)
                    .FirstOrDefault();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取可用仓库（可用、未冻结）
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="isFilterVirtual">是否过滤虚拟仓库</param>
        /// <param name="whIds">仓库ID集合</param>
        /// <param name="isContainerLine">是否过滤线边仓</param>
        /// <returns>可用仓库列表</returns>
        public virtual EntityList<Warehouse> GetAvailableWarehouses(PagingInfo pagingInfo, string keyword, bool? isFilterVirtual = false, List<double> whIds = null, bool isContainerLine = false)
        {
            var q = Query<Warehouse>();
            q.Where(p => p.State == State.Enable);
            q.Where(p => !p.IsFrozen);
            if (isContainerLine)
            {
                q.Where(p => !p.IsLineWarehouse);
            }
            if (!keyword.IsNullOrEmpty())
            {
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            if (isFilterVirtual.HasValue && isFilterVirtual.Value)
                q.Where(p => p.LibraryType == LibraryType.Entity);
            if (whIds != null && whIds.Any())
            {
                q.Where(p => whIds.Contains(p.Id));
            }
            return q.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取可用仓库（可用）
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="type">仓库类型</param>
        /// <param name="isLineWareHouse">是否过滤线边仓-默认不过滤</param>
        /// <returns>可用仓库列表</returns>
        public virtual EntityList<Warehouse> GetEnableWarehouses(PagingInfo pagingInfo, string keyword, LibraryType? type = null, bool isLineWareHouse = false)
        {
            var q = Query<Warehouse>();
            q.Where(p => p.State == State.Enable);
            if (!keyword.IsNullOrEmpty())
            {
                if (!keyword.Contains("%"))
                    keyword = "%" + keyword + "%";
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            if (type.HasValue)
                q.Where(p => p.LibraryType == type.Value);
            if (isLineWareHouse)
            {
                q.Where(p => !p.IsLineWarehouse);
            }
            return q.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取除线边仓的可用仓库（可用）
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="type">仓库类型</param>
        /// <returns>可用仓库列表</returns>
        public virtual EntityList<Warehouse> GetEnableWarehousesWithOutLine(PagingInfo pagingInfo, string keyword, LibraryType? type = null)
        {
            var q = Query<Warehouse>();
            q.Where(p => p.State == State.Enable && !p.IsLineWarehouse);
            if (!keyword.IsNullOrEmpty())
            {
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            if (type.HasValue)
                q.Where(p => p.LibraryType == type.Value);
            return q.ToList(pagingInfo);
        }


        /// <summary>
        ///  获取登录用户有权限的所有仓库（可用，非冻结）
        /// </summary>
        /// <param name="IsContainerLine">是否过滤线边仓</param>
        /// <returns></returns>
        public virtual EntityList<Warehouse> GetUserWarehouses(bool IsContainerLine = false)
        {
            var query = Query<Warehouse>();
            query.Where(p => p.State == State.Enable && p.LibraryType == LibraryType.Entity);
            if (IsContainerLine)
            {
                query.Where(p => !p.IsLineWarehouse);
            }
            query.Exists<WarehouseEmployee>((a, b) => b.Join<Employee>((c, d) => c.EmployeeId == d.Id && d.Id == RT.IdentityId)
            .Where(p => p.WarehouseId == a.Id));
            return query.ToList();
        }

        /// <summary>
        /// 获取员工的可调拨仓库
        /// </summary>
        /// <param name="warehouseIds"></param>
        /// <returns></returns>
        public virtual EntityList<Warehouse> GetUserAllocateWarehouses(List<double> warehouseIds)
        {
            if (warehouseIds.Count > 0)
            {
                return warehouseIds.SplitContains(ids =>
                {
                    var query = Query<Warehouse>().Where(p => p.State == State.Enable && !p.IsFrozen);
                    return query.Where(p => ids.Contains(p.Id)).ToList();
                });
            }
            else
            {
                return Query<Warehouse>().Where(p => p.State == State.Enable && !p.IsFrozen).ToList();
            }
        }
        /// <summary>
        /// 获取登录用户有权限的所有仓库（可用，非冻结）
        /// </summary>
        /// <param name="codeList">仓库编号集合</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页对象</param>
        /// <returns>仓库集合</returns>
        public virtual EntityList<Warehouse> GetUserWarehouses(List<string> codeList, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<Warehouse>().Where(p => codeList.Contains(p.Code));
            if (!string.IsNullOrEmpty(keyword))
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 仓库禁用
        /// </summary>
        /// <param name="whIdList">仓库Id列表</param>
        public virtual void DisableWarehouses(List<double> whIdList)
        {
            var warehouseList = GetWarehouses(whIdList);
            var areaList = GetStorageAreaForWHList(whIdList);

            if (areaList.Any(p => p.State != State.Disable))
                throw new ValidationException("库区[{0}]没有被禁用，仓库不能被禁用".L10nFormat(string.Join(",", areaList.Where(p => p.State != State.Disable).Select(p => p.Code).ToList())));

            if (warehouseList.Any(p => p.State != State.Enable))
            {
                throw new ValidationException("仓库[{0}]不是启用状态，不能禁用".L10nFormat(string.Join(",", warehouseList.Where(p => p.State != State.Enable).Select(p => p.Code).ToList())));
            }

            warehouseList.ForEach(p => p.State = State.Disable);
            RF.Save(warehouseList);
        }

        /// <summary>
        /// 仓库启用
        /// </summary>
        /// <param name="whIdList">仓库Id列表</param>
        public virtual void EnabelWarehouses(List<double> whIdList)
        {
            var warehouseList = GetWarehouses(whIdList);
            if (warehouseList.Any(p => p.State != State.Disable))
            {
                throw new ValidationException("仓库[{0}]不是禁用状态，不能启用".L10nFormat(string.Join(",", warehouseList.Where(p => p.State != State.Disable).Select(p => p.Code).ToList())));
            }

            warehouseList.ForEach(p => p.State = State.Enable);
            RF.Save(warehouseList);
        }

        /// <summary>
        /// 仓库冻结/解冻
        /// </summary>
        /// <param name="whIdList">仓库Id列表</param>
        public virtual void FrozenWarehouses(List<double> whIdList)
        {
            var warehouseList = GetWarehouses(whIdList);
            using (var tran = DB.TransactionScope(WareHouseEntityDataProvider.ConnectionStringName))
            {
                //更新冻结库位是否冻结ID集合
                List<double> isFrozenLocIds = new List<double>();

                //仓库冻结ID集合
                List<double> frozenWhIds = warehouseList.Where(p => p.IsFrozen).Select(p => p.Id).ToList();
                if (frozenWhIds.Any())
                {
                    //仓库冻结ID集合存在，则表示为解冻操作,当仓库是解冻时，将仓库下的所有库位的“仓库冻结”选项删除
                    var frozenLocs = GetStorageLocationByWhIds(frozenWhIds);
                    List<double> frozenLocIds = frozenLocs.Select(p => p.Id).ToList();
                    BatchDeleteLocFrozenReason(frozenLocIds, FrozenReason.WarehouseFrozen);
                    isFrozenLocIds.AddRange(frozenLocIds);
                }

                //未冻结仓库ID集合
                List<double> unFrozenWhIds = warehouseList.Where(p => !p.IsFrozen).Select(p => p.Id).ToList();
                if (unFrozenWhIds.Any())
                {
                    //未冻结仓库ID集合存在,则表示为冻结操作,当仓库是冻结时，将仓库下的所有库位的冻结原因增加“仓库冻结”选项
                    var unfrozenLocs = GetStorageLocationByWhIds(unFrozenWhIds);
                    List<double> unfrozenLocIds = unfrozenLocs.Select(p => p.Id).ToList();
                    BatchInsertLocFrozenReason(unfrozenLocIds, FrozenReason.WarehouseFrozen, string.Empty);
                    isFrozenLocIds.AddRange(unfrozenLocIds);
                }

                //对仓库下的每个库位执行“库位冻结状态更新通用方法
                EntityList<StorageLocation> locs = GetStorageLocations(isFrozenLocIds.Distinct().ToList(), new EagerLoadOptions().LoadWithViewProperty());
                BatchUpdateLocIsFrozen(locs, isFrozenLocIds.Distinct().ToList());

                warehouseList.ForEach(p => p.IsFrozen = !p.IsFrozen);
                RF.Save(warehouseList);

                tran.Complete();
            }
        }

        /// <summary>
        /// 保存调拨入仓库与员工的关系
        /// </summary>
        public virtual void SaveInWareHouseEmployee(List<InWarehouseEmployee> employeeList)
        {
            EntityList<InWarehouseEmployee> savedData = new EntityList<InWarehouseEmployee>();
            var warehouseIds = employeeList.Select(p => p.WarehouseId).Distinct().ToList();
            var warehouses = GetTargetWarehouses(warehouseIds);
            var invOrgs = RF.GetAll<InvOrg>();
            foreach (var item in employeeList)
            {
                var warehouse = warehouses.FirstOrDefault(x => x.Id == item.WarehouseId);
                var warehouseOrgCode = warehouse.GetInvOrgId();
                var org = invOrgs.FirstOrDefault(x => x.Code == warehouseOrgCode);
                var employee = new InWarehouseEmployee();
                employee.EmployeeId = item.EmployeeId;
                employee.WarehouseId = item.WarehouseId;
                employee.InvOrgCode = org.Code;
                employee.InvOrgName = org.Name;
                if (warehouseOrgCode == RT.InvOrg)
                {
                    employee.IsDirectAllocate = true;
                    employee.IsTwoAllocate = true;

                }
                else
                {
                    employee.IsCrossOrgTransferIn = true;
                }
                savedData.Add(employee);
            }
            RF.Save(savedData);
        }

        /// <summary>
        /// 获取仓库地址
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="addressId">地址Id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>仓库地址</returns>
        public virtual EntityList<WarehouseAddress> GetWarehouseAddressData(double warehouseId, double? addressId, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<WarehouseAddress>().Where(p => p.WarehouseId == warehouseId && p.State == State.Enable);

            if (addressId.HasValue)
                query.Where(p => p.Id == addressId);

            if (!string.IsNullOrEmpty(keyword))
            {
                query.Where(p => p.Address.Contains(keyword) || p.Name.Contains(keyword) || p.AddressType.Contains(keyword) ||
                            p.Area.Contains(keyword) || p.City.Contains(keyword) || p.Country.Contains(keyword));
            }

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据名称获取仓库
        /// </summary>
        /// <param name="name">仓库名称</param>
        /// <returns>仓库列表</returns>
        public virtual Warehouse GetWarehousedatas(string name)
        {
            var query = Query<Warehouse>();
            if (!name.IsNullOrEmpty())
            {
                query.Where(p => p.Name.Contains(name) && p.State == State.Enable);
            }

            return query.FirstOrDefault();
        }

        /// <summary>
        /// 根据编码获取仓库
        /// </summary>
        /// <param name="code">仓库编码</param>
        /// <returns>仓库数据</returns>
        public virtual Warehouse GetWarehouseByCode(string code)
        {
            var query = Query<Warehouse>();
            if (!code.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(code) && p.State == State.Enable);
            }

            return query.FirstOrDefault();
        }

        /// <summary>
        /// 根据名称获取仓库
        /// </summary>
        /// <param name="name">仓库编码</param>
        /// <returns>仓库数据</returns>
        public virtual Warehouse GetWarehouseByName(string name)
        {
            if (name.IsNullOrEmpty())
                return null;
            var query = Query<Warehouse>();
            query.Where(p => p.Name.Contains(name));
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 通过仓库名称列表获取仓库列表
        /// </summary>
        /// <param name="nameList">仓库name列表</param>
        /// <returns>仓库列表</returns>
        public virtual EntityList<Warehouse> GetWarehouseListByNames(List<string> nameList)
        {
            if (nameList.IsNullOrEmpty())
                return new EntityList<Warehouse>();

            return nameList.SplitContains(codes =>
            {
                return Query<Warehouse>().Where(p => codes.Contains(p.Name)).ToList();
            });
        }

        /// <summary>
        /// 通过仓库code列表获取仓库列表
        /// </summary>
        /// <param name="codeList">仓库code列表</param>
        /// <returns>仓库列表</returns>
        public virtual EntityList<Warehouse> GetWarehouseList(List<string> codeList)
        {
            if (codeList.IsNullOrEmpty())
                return new EntityList<Warehouse>();

            return codeList.SplitContains(codes =>
            {
                return Query<Warehouse>().Where(p => codes.Contains(p.Code)).ToList();
            });
        }

        /// <summary>
        /// 获取仓库基本资料
        /// </summary>
        /// <returns>返回仓库基本资料</returns>
        public virtual EntityList<Warehouse> GetWarehouse()
        {
            return Query<Warehouse>().ToList();
        }

        /// <summary>
        /// 获取仓库基本资料
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <returns>返回仓库基本资料</returns>
        public virtual WarehouseInfo GetWarehouseInfoDetail(double warehouseId)
        {
            return Query<WarehouseInfo>().Where(p => p.WarehouseId == warehouseId).FirstOrDefault();
        }

        /// <summary>
        /// 验证登录员工是否具有仓库权限
        /// </summary>
        /// <typeparam name="T">主体单据实体泛型</typeparam>
        /// <param name="query">查询</param>
        /// <param name="warehouseIdProperty">主体仓库托管属性</param>
        public virtual void ExistWarehouseEmplyee<T>(IEntityQueryer<T> query, IManagedProperty warehouseIdProperty) where T : DataEntity
        {
            query.Exists<WarehouseEmployee>((p, e) => e.Where(t => t.WarehouseId == (double)p.GetProperty(warehouseIdProperty)).Where(t => t.EmployeeId == RT.IdentityId));
        }

        /// <summary>
        /// 验证登录员工是否具有仓库权限
        /// </summary>
        /// <typeparam name="T">主体单据实体泛型</typeparam>
        /// <param name="query">查询</param>
        /// <param name="warehouseIdProperty">主体仓库托管属性</param>
        /// <param name="type">仓库类型</param>
        public virtual void ExistWarehouseTypeEmplyee<T>(IEntityQueryer<T> query, IManagedProperty warehouseIdProperty, LibraryType? type = null) where T : DataEntity
        {
            query.Exists<WarehouseEmployee>((p, e) => e.Where(t => t.WarehouseId == (double)p.GetProperty(warehouseIdProperty) || p.GetProperty(warehouseIdProperty) == null)
                 .Join<Warehouse>((d, w) => d.WarehouseId == w.Id)
                 .WhereIf<Warehouse>(type.HasValue, (d, w) => w.LibraryType == type)
                 .Where(t => t.EmployeeId == RT.IdentityId));
        }

        /// <summary>
        /// 查询员工对应仓库数据
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="type">仓库类型</param>
        /// <param name="contianFrozen">包含冻结</param>
        /// <param name="containLineWareHouse">包含线边仓</param>
        /// <returns></returns>
        public virtual EntityList<Warehouse> GetWarehouseByEmployee(PagingInfo pagingInfo, string keyword, LibraryType? type = null, bool contianFrozen = false)
        {
            var q = Query<Warehouse>();
            q.Where(p => p.State == State.Enable);
            if (!contianFrozen)
                q.Where(p => !p.IsFrozen);

            q.Exists<WarehouseEmployee>((a, b) => b.Where(t => t.WarehouseId == a.Id && t.EmployeeId == RT.IdentityId));
            if (keyword.IsNotEmpty())
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));

            if (type.HasValue)
                q.Where(p => p.LibraryType == type.Value);

            return q.ToList(pagingInfo);
        }



        /// <summary>
        /// 根据用户获取线边仓
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual EntityList<Warehouse> GetLineWareHouseByEmployee(PagingInfo pagingInfo, string keyword, LibraryType? type = null)
        {
            var q = Query<Warehouse>();
            q.Where(p => p.State == State.Enable);
            q.Where(p => p.IsLineWarehouse);
            q.Exists<WarehouseEmployee>((a, b) => b.Where(t => t.WarehouseId == a.Id && t.EmployeeId == RT.IdentityId));
            if (keyword.IsNotEmpty())
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));

            if (type.HasValue)
                q.Where(p => p.LibraryType == type.Value);

            return q.ToList(pagingInfo);
        }

        /// <summary>
        /// 根据用户获取线边仓
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual EntityList<Warehouse> GetLineWareHouse(PagingInfo pagingInfo, string keyword, LibraryType? type = null)
        {
            var q = Query<Warehouse>();
            q.Where(p => p.State == State.Enable);
            q.Where(p => p.IsLineWarehouse);
            if (keyword.IsNotEmpty())
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));

            if (type.HasValue)
                q.Where(p => p.LibraryType == type.Value);

            return q.ToList(pagingInfo);
        }

        /// <summary>
        /// 查询员工对应仓库数据
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="state">状态</param>
        /// <param name="type">仓库类型</param>
        /// <param name="contianFrozen">包含冻结</param>
        /// <param name="containLineWareHouse">包含线边仓</param>
        /// <param name="isDisableInvOrg">查询当前库存组织</param>
        /// <returns></returns>
        public virtual EntityList<Warehouse> GetAllWarehouses(PagingInfo pagingInfo, string keyword, State? state = null, LibraryType? type = null, bool contianFrozen = false, bool containLineWareHouse = false, bool isDisableInvOrg = true)
        {
            using (InvOrgs.WithAll())
            {
                EntityList<Warehouse> list = new EntityList<Warehouse>();
                var q = Query<Warehouse>();
                if (!contianFrozen)
                    q.Where(p => !p.IsFrozen);
                if (containLineWareHouse)
                {
                    q.Where(p => !p.IsLineWarehouse);
                }
                if (state.HasValue)
                {
                    q.Where(p => p.State == state);
                }
                if (keyword.IsNotEmpty())
                {
                    q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
                }
                if (type.HasValue)
                {
                    q.Where(p => p.LibraryType == type.Value);
                }
                var result = q.ToList(pagingInfo).ToList();
                if (isDisableInvOrg)
                {
                    result = result.FindAll(p => p.GetInvOrgId() == RT.InvOrg).ToList();
                }
                else
                {
                    result = result.FindAll(p => p.GetInvOrgId() != RT.InvOrg).ToList();
                }
                list.AddRange(result);
                return list;
            }
        }

        /// <summary>
        /// 查询员工对应仓库数据
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public virtual EntityList<Warehouse> GetWarehouseByAllInvOrg(PagingInfo pagingInfo, string keyword, State? state = null)
        {
            EntityList<Warehouse> warehouseList;
            using (SIE.Common.InvOrg.InvOrgs.WithAll())
            {
                var q = Query<Warehouse>();
                if (state.HasValue)
                    q.Where(p => p.State == state.Value);

                if (keyword.IsNotEmpty())
                    q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));

                warehouseList = q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }

            return warehouseList;
        }

        /// <summary>
        /// 查询员工对应仓库数据
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="isIngoreOnhand">仓库是否管库存</param>
        /// <param name="isAllot">发运单调拨选收货仓库</param>
        /// <returns></returns>
        public virtual EntityList<Warehouse> GetWarehouseByOtherInvOrg(PagingInfo pagingInfo, string keyword, bool? isIngoreOnhand = null, int? allotModel = null)
        {
            EntityList<Warehouse> warehouseList;
            List<double> whIds = new List<double>();
            string invOrgId = RF.Find<Warehouse>().EntityMeta.Property(InvOrgIdExtension.INV_ORG_IDProperty).ColumnMeta.ColumnName;
            if (allotModel.HasValue)
            {
                var warehouses = Query<InWarehouseEmployee>().Where(p => p.EmployeeId == RT.IdentityId).ToList();
                if (warehouses.Any())
                {
                    whIds = warehouses.Where(p => p.IsCrossOrgTransferIn).Select(p => p.WarehouseId).ToList();
                    if (!whIds.Any())
                        return new EntityList<Warehouse>();
                }
            }
            using (SIE.Common.InvOrg.InvOrgs.WithAll())
            {
                var q = DB.Query<Warehouse>("T00");
                q.Where(p => p.State == State.Enable);
                q.Where(p => !p.IsFrozen);
                if (isIngoreOnhand.HasValue)
                {
                    q.Where(p => p.IngoreOnhand == isIngoreOnhand.Value);
                }

                if (whIds.Any())
                    q.Where(p => whIds.Contains(p.Id));

                q.Where(p => p.SQL<bool>(new FormattedSql(" T00.{0} != {1}".FormatArgs(invOrgId, RT.InvOrg))));
                warehouseList = q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                var allOrg = RF.GetAll<InvOrg>();

                warehouseList.ForEach(p =>
                {
                    var org = allOrg.FirstOrDefault(a => a.Code == p.GetInvOrgId());
                    p.InvOrgName = org.Name;
                });

            }

            if (keyword.IsNotEmpty())
            {
                keyword = keyword.Replace("%", "");
                return warehouseList.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword) || p.InvOrgName.Contains(keyword)).AsEntityList();
            }
            return warehouseList;
        }

        /// <summary>
        /// 查询员工对应仓库数据
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="isIngoreOnhand">仓库是否管库存</param>
        /// <returns></returns>
        public virtual EntityList<Warehouse> GetSourceWarehouseByOtherInvOrg(PagingInfo pagingInfo, string keyword, bool? isIngoreOnhand = null)
        {
            EntityList<Warehouse> warehouseList;
            string invOrgId = RF.Find<Warehouse>().EntityMeta.Property(InvOrgIdExtension.INV_ORG_IDProperty).ColumnMeta.ColumnName;
            using (SIE.Common.InvOrg.InvOrgs.WithAll())
            {
                var q = DB.Query<Warehouse>("T00");
                q.Where(p => p.State == State.Enable);
                q.Where(p => !p.IsFrozen);
                if (!keyword.IsNullOrEmpty())
                {
                    q.Where(p => keyword.Contains(p.Code) || keyword.Contains(p.Name));
                }
                q.Where(p => !p.IsFrozen);
                if (isIngoreOnhand.HasValue)
                {
                    q.Where(p => p.IngoreOnhand == isIngoreOnhand.Value);
                }
                q.Where(p => p.SQL<bool>(new FormattedSql(" T00.{0} != {1}".FormatArgs(invOrgId, RT.InvOrg))));
                warehouseList = q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                var invOrg = RF.GetAll<InvOrg>();
                warehouseList.ForEach(a =>
                {
                    a.InvOrgName = invOrg.FirstOrDefault(f => f.Code == a.GetInvOrgId().Value).Name;
                });
            }
            return warehouseList;
        }

        /// <summary>
        /// 获取可用仓库（可用）
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="isIngoreOnHand">仓库是否管库存</param>
        /// <param name="type">仓库类型</param>
        /// <param name="isLineWareHouse">是否过滤线边仓-默认不过滤</param>
        /// <param name="isAllot">发运单调拨选收货仓库</param>
        /// <returns>可用仓库列表</returns>
        public virtual EntityList<Warehouse> GetEnableWarehousesWithOrgName(PagingInfo pagingInfo, string keyword, bool? isIngoreOnHand = null, LibraryType? type = null, bool isLineWareHouse = false, int? allotModel = null)
        {
            var q = Query<Warehouse>();
            q.Where(p => p.State == State.Enable);
            if (!keyword.IsNullOrEmpty())
            {
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            if (type.HasValue)
                q.Where(p => p.LibraryType == type.Value);
            if (isLineWareHouse)
            {
                q.Where(p => !p.IsLineWarehouse);
            }
            if (isIngoreOnHand.HasValue)
            {
                q.Where(p => p.IngoreOnhand == isIngoreOnHand.Value);
            }
            if (allotModel >= 0 && allotModel <= 2)
            {
                var warehouses = Query<InWarehouseEmployee>().Where(p => p.EmployeeId == RT.IdentityId).ToList();
                if (warehouses.Any())
                {

                    List<double> whIds = new List<double>();
                    if (allotModel == 0)
                        whIds = warehouses.Where(p => p.IsDirectAllocate).Select(p => p.WarehouseId).ToList();
                    if (allotModel == 1)
                        whIds = warehouses.Where(p => p.IsTwoAllocate).Select(p => p.WarehouseId).ToList();
                    if (allotModel == 2)
                        whIds = warehouses.Where(p => p.IsCrossOrgTransferIn).Select(p => p.WarehouseId).ToList();
                    if (whIds.Any())
                        q.Where(p => whIds.Contains(p.Id));
                    else
                        return new EntityList<Warehouse>();
                }
            }
            var list = q.ToList(pagingInfo);
            var invOrg = Query<InvOrg>().Where(f => f.Code == RT.InvOrg.Value).FirstOrDefault();
            list.ForEach(f =>
            {
                f.InvOrgName = invOrg.Name;
            });
            return list;
        }

        /// <summary>
        /// 根据仓库Id获取仓库数据
        /// </summary>
        /// <param name="warehouseIdList">仓库Id</param>
        /// <returns>返回仓库数据</returns>
        public virtual EntityList<Warehouse> GetTargetWarehouses(List<double> warehouseIdList)
        {
            EntityList<Warehouse> warehouseList;
            using (SIE.Common.InvOrg.InvOrgs.WithAll())
            {
                var query = Query<Warehouse>().Where(p => warehouseIdList.Distinct().ToList().Contains(p.Id));
                warehouseList = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            }

            return warehouseList;
        }

        /// <summary>
        /// 查询员工对应仓库数据
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="type">仓库类型</param>
        /// <param name="containLineWh">是否过滤线边仓-默认不过滤</param>
        /// <returns></returns>
        public virtual EntityList<Warehouse> GetAllWarehouseByEmployee(PagingInfo pagingInfo, string keyword, LibraryType? type = null, bool containLineWh = true)
        {
            var q = Query<Warehouse>().Where(p => p.State == State.Enable && !p.IsFrozen);
            q.Exists<WarehouseEmployee>((a, b) => b.Where(t => t.WarehouseId == a.Id && t.EmployeeId == RT.IdentityId));
            if (keyword.IsNotEmpty())
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));

            if (type.HasValue)
                q.Where(p => p.LibraryType == type.Value);
            if (!containLineWh)
            {
                q.Where(p => !p.IsLineWarehouse);
            }
            return q.ToList(pagingInfo);
        }

        /// <summary>
        /// 查询员工对应仓库数据
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns></returns>
        public virtual EntityList<Warehouse> GetAllWarehouseByEmp(PagingInfo pagingInfo, string keyword)
        {
            var q = Query<Warehouse>().Where(p => p.State == State.Enable && !p.IsFrozen && p.IsLineWarehouse);
            q.Exists<WarehouseEmployee>((a, b) => b.Where(t => t.WarehouseId == a.Id && t.EmployeeId == RT.IdentityId));
            if (keyword.IsNotEmpty())
            {
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return q.ToList(pagingInfo);
        }

        /// <summary>
        ///获取EDO为仓库
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>

        public virtual EntityList<Warehouse> GetAllWarehouseByEmployeeForEdo(PagingInfo pagingInfo, string keyword)
        {
            var q = Query<Warehouse>().Where(p => p.State == State.Enable && !p.IsFrozen);
            q.Exists<WarehouseEmployee>((a, b) => b.Where(t => t.WarehouseId == a.Id && t.EmployeeId == RT.IdentityId));
            if (keyword.IsNotEmpty())
            {
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return q.ToList(pagingInfo);
        }

        /// <summary>
        /// 查询员工对应仓库数据
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public virtual EntityList<Warehouse> GetEmployeeWarehouseByAllInvOrg(PagingInfo pagingInfo, string keyword, State? state = null)
        {
            EntityList<Warehouse> warehouseList;
            using (SIE.Common.InvOrg.InvOrgs.WithAll())
            {
                var q = Query<Warehouse>();
                q.Exists<WarehouseEmployee>((a, b) => b.Where(t => t.WarehouseId == a.Id && t.EmployeeId == RT.IdentityId));
                if (state.HasValue)
                    q.Where(p => p.State == state.Value);

                if (keyword.IsNotEmpty())
                    q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));

                warehouseList = q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }

            return warehouseList;
        }

        /// <summary>
        /// 获取登录员工有权限的仓库ID集合
        /// </summary>
        /// <returns>登录员工有权限的仓库ID集合</returns>
        public virtual IList<double> GetEmployeeWarehouseIds()
        {
            var query = Query<WarehouseEmployee>().Where(p => p.EmployeeId == RT.IdentityId);
            query.Select(p => p.WarehouseId);
            return query.ToList<double>();
        }

        /// <summary>
        /// 根据员工ID查找仓库与员工关系
        /// </summary>
        /// <param name="empId">员工ID</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>仓库与员工关系</returns>
        public virtual EntityList<WarehouseEmployee> GetWarehouseByEmpId(double empId, PagingInfo pagingInfo)
        {
            return Query<WarehouseEmployee>()
                .Where(r => r.EmployeeId == empId)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据员工ID查找仓库与员工关系
        /// </summary>
        /// <param name="empId">员工ID</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>仓库与员工关系</returns>
        public virtual EntityList<InWarehouseEmployee> GetInWarehouseByEmpId(double empId, PagingInfo pagingInfo)
        {
            return Query<InWarehouseEmployee>()
                .Where(r => r.EmployeeId == empId)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 员工是否存在仓库
        /// </summary>
        /// <param name="employeeId">用户ID</param>
        /// <param name="warehouseId">仓库ID</param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentNullException">参数空引用</exception>
        public virtual bool EmployeeHasWarehouse(double employeeId, double warehouseId)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }

            if (warehouseId <= 0)
            {
                throw new ArgumentNullException(nameof(warehouseId));
            }

            var q = Query<WarehouseEmployee>();
            q.Where(p => p.WarehouseId == warehouseId && p.EmployeeId == employeeId);
            return q.Count() > 0;
        }


        /// <summary>
        /// 获取登录用户有权限的仓库Id集合
        /// </summary>
        /// <returns>返回登录用户有权限的仓库Id集合</returns>
        public virtual List<double> GetAuthorityWarehouseId()
        {
            return Query<WarehouseEmployee>().Where(y => y.EmployeeId == RT.IdentityId).Select(p => p.WarehouseId).Distinct().ToList<double>().ToList();
        }


        /// <summary>
        /// 获取当前用户仓库行数
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <returns>当前用户仓库行数</returns>
        public virtual int CheckEmployeeWarehouse(double warehouseId)
        {
            var query = Query<WarehouseEmployee>().Where(p => p.WarehouseId == warehouseId && p.EmployeeId == RT.IdentityId);
            return query.Count();
        }

        /// <summary>
        /// 根据仓库查询关联的员工
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="eagerLoadOptions"></param>
        /// <returns></returns>
        public virtual EntityList<Employee> GetEmployeesByWarehouse(double? warehouseId, string keyword = null, PagingInfo pagingInfo = null, EagerLoadOptions eagerLoadOptions = null)
        {
            var q = Query<Employee>();
            if (keyword != null)
            {
                q.Where(p => p.Name.Contains(keyword));
            }
            if (warehouseId.HasValue)
            {
                q.Join<WarehouseEmployee>((emp, wh) => emp.Id == wh.EmployeeId && wh.WarehouseId == warehouseId);
            }
            return q.ToList(pagingInfo, eagerLoadOptions);
        }

        /// <summary>
        /// 获取仓库与员工关系列表
        /// </summary>
        /// <param name="warehouseIds">仓库ID列表</param>
        /// <returns></returns>
        public virtual EntityList<WarehouseEmployee> GetWarehouseEmployeesByWarehouseIds(List<double> warehouseIds)
        {
            return warehouseIds.SplitContains(tempIds =>
            {
                return Query<WarehouseEmployee>().Where(x => tempIds.Contains(x.WarehouseId)).ToList();
            });
        }

        /// <summary>
        /// 获取除线边仓的仓库
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual EntityList<Warehouse> GetWarehouseWithOutLine(PagingInfo pagingInfo, string keyword, State? state = null)
        {
            EntityList<Warehouse> warehouseList;
            using (SIE.Common.InvOrg.InvOrgs.WithAll())
            {
                var q = Query<Warehouse>().Where(p => !p.IsLineWarehouse);
                if (state.HasValue)
                    q.Where(p => p.State == state.Value);

                if (keyword.IsNotEmpty())
                    q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));

                warehouseList = q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }

            return warehouseList;
        }

        /// <summary>
        /// 新增仓库自动添加仓库权限
        /// </summary>
        /// <param name="whId">仓库</param>
        public virtual void InsertWarehouseAdminUser(double whId)
        {
            var userEmpIds = Query<User>().Join<UserInRole>((x, y) => x.Id == y.UserId)
                .Join<UserInRole, RoleExtEntity>((y, r) => y.RoleId == r.Id)
                .Where(f => f.EmployeeId > 0).Where<RoleExtEntity>((x, y) => y.IsAllWarehouse == true).Select(f => f.EmployeeId).ToList<double>();
            if (!userEmpIds.Any())
                return;
            EntityList<WarehouseEmployee> warehouseEmployees = new EntityList<WarehouseEmployee>();
            userEmpIds.ForEach(a =>
            {
                WarehouseEmployee warehouseEmployee = new WarehouseEmployee()
                {
                    EmployeeId = a,
                    WarehouseId = whId
                };
                warehouseEmployees.Add(warehouseEmployee);
            });
            RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchInsertSave(warehouseEmployees);
        }

        /// <summary>
        /// 角色更改全仓库权限自动加入用户
        /// </summary>
        /// <param name="roleId">角色</param>
        /// <param name="userId">用户</param>
        /// <param name="empId">员工</param>
        public virtual void InsertWarehouseRoleUser(double roleId, double? userId = null, double? empId = null)
        {
            List<double> userEmpIds = new List<double>();
            if (empId.HasValue) { userEmpIds.Add(empId.Value); }
            else if (userId > 0)
            {
                var user = RF.GetById<User>(userId);
                if (user.EmployeeId > 0)
                    userEmpIds.Add(user.EmployeeId.Value);
            }

            else
                userEmpIds = Query<User>()
                    .Join<UserInRole>((x, y) => x.Id == y.UserId)
                    .Join<UserInRole, RoleExtEntity>((y, r) => y.RoleId == r.Id)
                    .Where(f => f.EmployeeId > 0)
                    .Where<UserInRole>((x, y) => y.RoleId == roleId)
                    .Select(f => f.EmployeeId).Distinct().ToList<double>().ToList();
            if (!userEmpIds.Any())
                return;
            var whs = RF.GetAll<Warehouse>(null, new EagerLoadOptions().LoadWith(Warehouse.EmployeeListProperty));
            EntityList<WarehouseEmployee> warehouseEmployees = new EntityList<WarehouseEmployee>();
            var whIds = whs.Select(a => a.Id).ToList();
            using (var tran = DB.TransactionScope(WareHouseEntityDataProvider.ConnectionStringName))
            {
                DB.Update<WarehouseEmployee>().Where(f => whIds.Contains(f.Id)).Execute();
                whs.ForEach(f =>
               {
                   var existEmpIds = f.EmployeeList.Select(a => a.EmployeeId).ToList();
                   userEmpIds.Where(a => !existEmpIds.Contains(a)).ForEach(a =>
                   {
                       WarehouseEmployee warehouseEmployee = new WarehouseEmployee()
                       {
                           EmployeeId = a,
                           WarehouseId = f.Id
                       };
                       warehouseEmployees.Add(warehouseEmployee);
                   });
               });
                RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchInsertSave(warehouseEmployees);
                tran.Complete();
            }
        }

        /// <summary>
        /// 插入仓库用户
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="empId">员工</param>
        public virtual void InserWhEmp(double userId, double empId)
        {
            //用户所在的角色是全仓库权限才绑定
            if (Query<UserInRole>().Join<RoleExtEntity>((x, y) => x.RoleId == y.Id)
                .Where(f => f.UserId == userId)
                .Where<RoleExtEntity>((x, y) => y.IsAllWarehouse == true)
                .Count() > 0)
                InsertWarehouseRoleUser(0, userId, empId);
        }
        #endregion

        #region 工作区
        /// <summary>
        /// 工作区查询数据
        /// </summary>
        /// <param name="criteria">查询实体数据</param>
        /// <returns>工作区数据</returns>
        public virtual EntityList<WorkArea> GetWorkAreaData(WorkAreaCriteria criteria)
        {
            var query = Query<WorkArea>();
            if (criteria != null)
            {
                if (!string.IsNullOrEmpty(criteria.Code))
                    query.Where(p => p.Code.Contains(criteria.Code));
                if (!string.IsNullOrEmpty(criteria.Name))
                    query.Where(p => p.Name.Contains(criteria.Name));
                if (criteria.WarehouseId.HasValue)
                    query.Where(p => p.WarehouseId == criteria.WarehouseId.Value);
                if (criteria.StorageLocationId.HasValue)
                    query.Join<WorkAreaLocation>("b", (a, b) => a.Id == b.WorkAreaId && b.StorageLocationId == criteria.StorageLocationId.Value);
                if (criteria.EmployeeId.HasValue)
                    query.Join<WorkAreaEmployee>("c", (a, c) => a.Id == c.WorkAreaId && c.EmployeeId == criteria.EmployeeId.Value);

                if (criteria.CreateDate.BeginValue.HasValue)
                    query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);

                if (criteria.CreateDate.EndValue.HasValue)
                    query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            }

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, elo);
        }

        /// <summary>
        /// 获取工作区编号
        /// </summary>
        /// <returns>工作区编号</returns>
        public virtual string GetWorkAreaCode()
        {
            var config = ConfigService.GetConfig(new NoConfig(), typeof(WorkArea));
            if (config == null || config.BacodeRule == null)
                return "";
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 获取库位关系数据
        /// </summary>
        /// <param name="workAreaId">工作区Id</param>
        /// <returns>库位关系数据</returns>
        public virtual EntityList<WorkAreaLocation> GetWorkAreaLocations(double? workAreaId)
        {
            if (workAreaId.HasValue)
            {
                return Query<WorkAreaLocation>().Where(p => p.WorkAreaId == workAreaId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            }
            else
            {
                return new EntityList<WorkAreaLocation>();
            }
        }

        /// <summary>
        /// 获取工作区库位关系数据
        /// </summary>
        /// <param name="locIdList">库位Id列表</param>
        /// <returns>库位关系数据</returns>
        public virtual EntityList<WorkAreaLocation> GetWorkAreaLocationList(List<double> locIdList)
        {
            return Query<WorkAreaLocation>().Where(p => locIdList.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工作区员工关系数据
        /// </summary>
        /// <param name="empIdList">员工Id列表</param>
        /// <returns>工作区员工关系数据</returns>
        public virtual EntityList<WorkAreaEmployee> GetWorkAreaEmployeeList(List<double> empIdList)
        {
            return Query<WorkAreaEmployee>().Where(p => empIdList.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 更新员工在岗情况
        /// </summary>
        /// <param name="empIdList">员工关系Id</param>
        /// <param name="situation">在岗情况</param>
        public virtual void WorkAreaEmployeeWorkSituation(List<double> empIdList, WorkSituation situation)
        {
            EntityList<WorkAreaEmployee> workAreaEmployees = GetWorkAreaEmployeeList(empIdList);

            if (workAreaEmployees.Any(p => p.WorkSituation == situation))
                throw new ValidationException("员工在岗情况与前端操作一致,无需再操作!".L10N());

            using (var tran = DB.TransactionScope(WareHouseEntityDataProvider.ConnectionStringName))
            {
                workAreaEmployees.ForEach(p =>
                {
                    p.WorkSituation = situation;
                });

                RF.Save(workAreaEmployees);

                tran.Complete();
            }
        }

        /// <summary>
        /// 获取工作区域
        /// </summary>
        /// <param name="stayWarehouseId">仓库Id</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>工作区域</returns>
        public virtual EntityList<WorkArea> GetWorkAreas(double stayWarehouseId, string keyWord, PagingInfo pagingInfo)
        {
            var query = Query<WorkArea>().Where(p => p.WarehouseId == stayWarehouseId);
            if (!keyWord.IsNullOrWhiteSpace())
            {
                query.Where(p => p.Code.Contains(keyWord) || p.Name.Contains(keyWord));
            }
            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 根据工作区编码集合获取仓库有效（非禁用，非冻结）库区信息
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="code">工作区编码</param>
        /// <returns>库位信息</returns>
        public virtual WorkArea GetEffectiveWorkAreaForReplensh(double warehouseId, string code)
        {
            var query = Query<WorkArea>().Where(p => p.State == State.Enable && p.WarehouseId == warehouseId && p.Code == code);
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 获取可用工作区域
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>工作区域</returns>
        public virtual EntityList<WorkArea> GetWorkAreasForReplenish(double warehouseId, string keyWord, PagingInfo pagingInfo)
        {
            var query = Query<WorkArea>().Where(p => p.WarehouseId == warehouseId && p.State == State.Enable);
            if (!keyWord.IsNullOrWhiteSpace())
            {
                query.Where(p => p.Code.Contains(keyWord) || p.Name.Contains(keyWord));
            }
            return query.ToList(pagingInfo);
        }
        #endregion

        #region 巷道
        /// <summary>
        /// 获取巷道查询数据
        /// </summary>
        /// <param name="criteria">巷道查询条件</param>
        /// <returns>返回巷道数据</returns>
        public virtual EntityList<Routeway> GetRoutewayData(RoutewayCriteria criteria)
        {
            var q = Query<Routeway>();
            ////仓库权限关联查询
            RT.Service.Resolve<WarehouseController>().ExistWarehouseEmplyee(q, Routeway.WarehouseIdProperty);

            if (criteria != null)
            {
                if (!string.IsNullOrEmpty(criteria.Code))
                    q.Where(p => p.Code.Contains(criteria.Code));
                if (!string.IsNullOrEmpty(criteria.Name))
                    q.Where(p => p.Name.Contains(criteria.Name));
                if (criteria.WarehouseId.HasValue)
                    q.Where(p => p.WarehouseId == criteria.WarehouseId);
                if (criteria.AreaId.HasValue)
                    q.Where(p => p.StorageAreaId == criteria.AreaId.Value);
            }

            q.OrderBy(criteria.OrderInfoList);
            return q.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 获取巷道
        /// </summary>
        /// <param name="whId">仓库Id</param>
        /// <param name="areaId">区域</param>
        /// <param name="info">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>巷道</returns>
        public virtual EntityList<Routeway> GetRouteways(double? whId, double? areaId, string keyword, PagingInfo info)
        {
            var query = Query<Routeway>();
            if (whId.HasValue)
            {
                query.Where(p => p.WarehouseId == whId);
            }
            if (areaId.HasValue)
            {
                query.Where(p => p.StorageAreaId == areaId);
            }
            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取巷道
        /// </summary>
        /// <param name="whId">仓库Id</param>
        /// <param name="areaId">库区Id</param>
        /// <param name="elo">贪懒加载</param>
        /// <returns>巷道</returns>
        public virtual EntityList<Routeway> GetRouteways(double whId, double areaId, EagerLoadOptions elo = null)
        {
            if (elo == null)
                elo = new EagerLoadOptions().LoadWithViewProperty();
            var query = Query<Routeway>().Where(p => p.WarehouseId == whId && p.StorageAreaId == areaId);

            return query.ToList(null, elo);
        }

        /// <summary>
        /// 获取库区巷道ID
        /// </summary>
        /// <param name="whId">仓库Id</param>
        /// <param name="areaId">库区Id</param>
        /// <returns>库区巷道ID</returns>
        public virtual IList<double> GetRoutewayIds(double whId, double areaId)
        {
            var query = Query<Routeway>().Where(p => p.WarehouseId == whId && p.StorageAreaId == areaId);
            return query.Select(p => p.Id).ToList<double>();
        }

        /// <summary>
        /// 获取巷道
        /// </summary>
        /// <param name="whId">仓库Id</param>
        /// <param name="routewayId">巷道ID</param>  
        /// <returns>巷道</returns>
        public virtual Routeway GetRouteway(double whId, double routewayId)
        {
            var query = Query<Routeway>().Where(p => p.WarehouseId == whId && p.Id == routewayId);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取巷道
        /// </summary>
        /// <param name="whId">仓库Id</param>
        /// <param name="code">编码</param>  
        /// <returns>巷道</returns>
        public virtual Routeway GetRouteway(double whId, string code)
        {
            var query = Query<Routeway>().Where(p => p.WarehouseId == whId && p.Code == code);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取巷道信息
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="name">名称</param>
        /// <returns>巷道</returns>
        public virtual Routeway GetRouteway(string code, string name)
        {
            var query = Query<Routeway>();
            if (code.IsNotEmpty())
                query.Where(p => p.Code == code);
            if (name.IsNotEmpty())
                query.Where(p => p.Name == name);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取编号
        /// </summary>
        /// <returns>返回编号</returns>
        public virtual string GetRoutewayCode()
        {
            var config = ConfigService.GetConfig(new NoConfig(), typeof(Routeway));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到编码生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();
        }

        #endregion

        #region 逻辑分区
        /// <summary>
        /// 获取编号
        /// </summary>
        /// <returns>返回编号</returns>
        public virtual string GetLogicAreaCode()
        {
            var config = ConfigService.GetConfig(new NoConfig(), typeof(LogicArea));
            if (config != null && config.BacodeRule != null)
                return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();
            return string.Empty;
        }

        /// <summary>
        /// 获取逻辑分区信息
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="name">名称</param>
        /// <returns>逻辑分区</returns>
        public virtual LogicArea GetLogicArea(string code, string name)
        {
            var query = Query<LogicArea>();
            if (code.IsNotEmpty())
                query.Where(p => p.Code == code);
            if (name.IsNotEmpty())
                query.Where(p => p.Name == name);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取逻辑分区查询数据
        /// </summary>
        /// <param name="criteria">逻辑分区查询条件</param>
        /// <returns>返回逻辑分区数据</returns>
        public virtual EntityList<LogicArea> GetLogicAreaData(LogicAreaCriteria criteria)
        {
            var q = Query<LogicArea>();
            ////仓库权限关联查询
            RT.Service.Resolve<WarehouseController>().ExistWarehouseEmplyee(q, LogicArea.WarehouseIdProperty);

            if (!string.IsNullOrEmpty(criteria.Code))
                q.Where(p => p.Code.Contains(criteria.Code));
            if (!string.IsNullOrEmpty(criteria.Name))
                q.Where(p => p.Name.Contains(criteria.Name));
            if (criteria.WarehouseId.HasValue)
                q.Where(p => p.WarehouseId == criteria.WarehouseId);
            if (criteria.StorageLocationId.HasValue)
                q.Exists<LogicAreaLocation>((p, l) => l.Where(t => t.LogicAreaId == p.Id));

            q.OrderBy(criteria.OrderInfoList);
            return q.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取仓库逻辑分区
        /// </summary>
        /// <param name="wareHouseId">仓库ID</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="info">分页信息</param>
        /// <returns>仓库逻辑分区</returns>
        public virtual EntityList<LogicArea> GetLogicAreas(double wareHouseId, string keyword, PagingInfo info)
        {
            var query = Query<LogicArea>();
            query.Where(p => p.WarehouseId == wareHouseId && p.State == State.Enable);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取逻辑分区与库位的关系行数
        /// </summary>
        /// <param name="logicAreaId">逻辑分区ID</param>
        /// <param name="locId">库位ID</param>
        /// <returns>逻辑分区与库位的关系行数</returns>
        public virtual int GetLogicAreaLocationCout(double logicAreaId, double locId)
        {
            var query = Query<LogicAreaLocation>().Where(p => p.LogicAreaId == logicAreaId && p.StorageLocationId == locId);
            return query.Count();
        }

        /// <summary>
        /// 获取逻辑分区库位关系
        /// </summary>
        /// <param name="logicAreaId">逻辑分区Id</param>
        /// <param name="info">分页</param>
        /// <returns>库位</returns>
        public virtual EntityList<LogicAreaLocation> GetLogicAreaLocations(double logicAreaId, PagingInfo info)
        {
            var query = Query<LogicAreaLocation>().Where(p => p.LogicAreaId == logicAreaId);

            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取逻辑分区库位关系数据
        /// </summary>
        /// <param name="locIdList">库位Id列表</param>
        /// <returns>库位关系数据</returns>
        public virtual EntityList<LogicAreaLocation> GetLogicAreaLocationList(List<double> locIdList)
        {
            var query = Query<LogicAreaLocation>().Where(p => locIdList.Contains(p.Id));
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取逻辑分区库位关系数据
        /// </summary>
        /// <param name="locIds">locIds</param>
        /// <returns>库位关系数据</returns>
        public virtual EntityList<LogicAreaLocation> GetLogicAreaLocByLocIds(List<double> locIds)
        {
            return locIds.SplitContains(tmpIds =>
            {
                return Query<LogicAreaLocation>().Where(p => tmpIds.Contains(p.StorageLocationId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 保存库位
        /// </summary>
        /// <param name="locationList"></param>
        /// <param name="isAuto"></param>
        public virtual void SaveLoc(List<LogicAreaLocation> locationList, bool isAuto)
        {
            using (var tran = DB.TransactionScope(WareHouseEntityDataProvider.ConnectionStringName))
            {
                var logicId = locationList.FirstOrDefault().LogicAreaId;
                EntityList<LogicAreaLocation> savedData = new EntityList<LogicAreaLocation>();
                foreach (var item in locationList)
                {
                    LogicAreaLocation logicAreaLocation = new LogicAreaLocation();
                    logicAreaLocation.LogicAreaId = item.LogicAreaId;
                    logicAreaLocation.StorageLocationId = item.StorageLocationId;
                    savedData.Add(logicAreaLocation);
                }
                RF.Save(savedData);
                var logic = RF.GetById<LogicArea>(logicId);
                if (logic.IsAutomatedArea != isAuto)
                {
                    logic.IsAutomatedArea = isAuto;
                    RF.Save(logic);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 获取逻辑库区库位
        /// </summary>
        /// <param name="logicAreaIds">逻辑区</param>
        /// <param name="locIds">库位ID集合</param>
        /// <param name="elo">贪婪加载</param>
        public virtual EntityList<LogicArea> GetLogicAreaByIds(List<double> logicAreaIds, List<double> locIds, EagerLoadOptions elo = null)
        {
            var query = DB.Query<LogicArea>("p").Where(p => logicAreaIds.Contains(p.Id));
            query.Join<LogicAreaLocation>((l, la) => l.Id == la.LogicAreaId && locIds.Contains(la.StorageLocationId));
            if (elo == null)
            {
                elo = new EagerLoadOptions().LoadWithViewProperty();
            }
            return query.ToList(null, elo);
        }

        /// <summary>
        /// 获取逻辑库区库位
        /// </summary>
        /// <param name="logicAreaId">逻辑区</param>
        /// <param name="queryAction">泛型查询委托</param>
        /// <param name="elo">贪婪加载</param>
        public virtual EntityList<StorageLocation> GetLogicAreaLocs(double logicAreaId, Action<IEntityQueryer<StorageLocation>> queryAction, EagerLoadOptions elo = null)
        {
            var query = DB.Query<StorageLocation>("p");
            query.Join<LogicAreaLocation>((l, la) => la.StorageLocationId == l.Id && la.LogicAreaId == logicAreaId);
            queryAction?.Invoke(query);
            if (elo == null)
            {
                elo = new EagerLoadOptions().LoadWithViewProperty();
            }
            return query.ToList(null, elo);
        }
        #endregion

        #region ERP子库

        /// <summary>
        /// 获取ERP子库
        /// </summary>
        /// <param name="warehouseId">仓库</param>
        /// <param name="areaId">库区</param>
        /// <param name="locId">库位</param>
        /// <param name="keyWrod">关键字</param>
        /// <param name="p">分页</param>
        /// <returns>子库实体</returns>
        public virtual EntityList<ErpWarehouse> GetErpWarehouses(double warehouseId, double? areaId, double? locId, string keyWrod, PagingInfo p)
        {
            var query = Query<ErpWarehouseDetail>()
                .Where(x => x.ErpWarehouse.State == State.Enable && x.ErpWarehouse.WmsInvOrg == RT.InvOrg.ToString() && x.WarehouseId == warehouseId);
            if (areaId > 0)
                query.Where(f => f.AreaId == areaId || f.AreaId == null);
            if (locId > 0)
                query.Where(f => locId == f.StorageLocationId || f.StorageLocationId == null);
            if (keyWrod.IsNotEmpty())
                query.Where(f => f.ErpWarehouse.Code.Contains(keyWrod) || f.ErpWarehouse.Name.Contains(keyWrod));
            var rst = query.ToList(null, new EagerLoadOptions().LoadWith(ErpWarehouseDetail.ErpWarehouseProperty));
            var erpWh = new EntityList<ErpWarehouse>();
            if (rst.Any())
            {
                if (locId > 0)
                {
                    var item = rst.FirstOrDefault(a => a.StorageLocationId == locId);
                    if (item != null)//当前库位已经配置子库
                    {
                        return new EntityList<ErpWarehouse>() { item.ErpWarehouse };
                    }
                }
                if (areaId > 0)
                {
                    var items = rst.Where(a => a.AreaId == areaId).AsEntityList();
                    //当前库区已经配置子库
                    items.ForEach(a =>
                    {
                        if (!erpWh.Any(b => b.Id == a.ErpWarehouseId))
                            erpWh.Add(a.ErpWarehouse);
                    });
                    if (erpWh.Any())
                        return erpWh;
                }
                //当前仓区已经配置子库
                rst.ForEach(a =>
                {
                    if (!erpWh.Any(b => b.Id == a.ErpWarehouseId))
                        erpWh.Add(a.ErpWarehouse);
                });
            }
            return erpWh;


        }

        /// <summary>
        /// 根据仓库获取子库
        /// </summary>
        /// <param name="whIds">仓库</param>
        /// <returns>子库</returns>
        public virtual EntityList<ErpWarehouseDetail> GetErpWarehouses(List<double> whIds)
        {
            var query = Query<ErpWarehouseDetail>().Where(f => whIds.Contains(f.WarehouseId) && f.ErpWarehouse.WmsInvOrg == RT.InvOrg.ToString());
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过当前库存组织获取对应的ERP子库
        /// </summary>
        /// <param name="orgId">ERP库存组织</param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<ErpWarehouse> GetErpWarehouses(int orgId, PagingInfo pagingInfo, string keyword)
        {
            return Query<ErpWarehouse>().Where(p => p.ErpOrgId == orgId.ToString())
                .WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 匹配ERP子库
        /// </summary>
        /// <param name="erpWarehouses">erp子库</param>
        /// <param name="warehouseId">仓库</param>
        /// <param name="areaId">库区</param>
        /// <param name="locId">库位</param>
        /// <returns>ERP子库</returns>
        public virtual ErpWarehouseDetail MatchErpWarehouse(EntityList<ErpWarehouseDetail> erpWarehouses, double warehouseId, double? areaId, double? locId)
        {
            if (locId.HasValue)
            {
                var item = erpWarehouses.FirstOrDefault(a => a.StorageLocationId == locId);
                if (item != null)//当前库位已经配置子库
                    return item;
            }
            if (areaId.HasValue)
            {
                var items = erpWarehouses.Where(a => a.AreaId == areaId).AsEntityList();
                if (items.Any())//当前库区已经配置子库
                    return items.FirstOrDefault();
            }
            return erpWarehouses.FirstOrDefault(a => a.WarehouseId == warehouseId);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public virtual bool ChecLocHasErpWarehouse(double erpWhId, double locId)
        {
            return Query<ErpWarehouseDetail>().Where(f => f.ErpWarehouseId == erpWhId && f.StorageLocationId == locId && f.ErpWarehouse.WmsInvOrg == RT.InvOrg.ToString()).Count() > 0;
        }

        /// <summary>
        /// 通过子库ID获取数据
        /// </summary>
        /// <param name="erpWhId"></param>
        /// <returns></returns>
        public virtual EntityList<ErpWarehouse> GetErpWareHouseByCode(List<double> erpWhId)
        {
            return erpWhId.SplitContains(ids =>
            {
                return Query<ErpWarehouse>().Where(x => ids.Contains(x.Id)).ToList();
            });
        }

        /// <summary>
        /// 通过当前库存组织获取对应的ERP子库
        /// </summary>
        /// <param name="orgId">ERP库存组织</param>
        /// <returns></returns>
        public virtual EntityList<ErpWarehouse> GetErpWareHouseByOrg(int orgId)
        {
            return Query<ErpWarehouse>().Where(p => p.ErpOrgId == orgId.ToString()).ToList();
        }

        /// <summary>
        /// 通过当前库存组织获取对应的ERP子库
        /// </summary>
        /// <param name="orgIds">库存组织ID集合</param>
        /// <returns></returns>
        public virtual EntityList<ErpWarehouse> GetErpWareHouseByOrgs(List<string> orgIds)
        {
            return orgIds.SplitContains(ids =>
            {
                return Query<ErpWarehouse>().Where(x => ids.Contains(x.WmsInvOrg)).ToList();
            });
        }
        #endregion

        #region 调拨至仓库
        /// <summary>
        /// 同步所选员工的调拨至仓库到弹窗所选的员工上
        /// </summary>
        /// <param name="EmployeeId">所选员工</param>
        /// <param name="EmployeeIds">需要同步的员工</param>
        /// <param name="type">1-覆盖同步 2-追加同步</param>
        public virtual void SynchronizeToEmployees(double EmployeeId, List<double> EmployeeIds, int type)
        {
            var allEmployeeIds = new List<double>();
            allEmployeeIds.AddRange(EmployeeIds);
            allEmployeeIds.Add(EmployeeId);
            allEmployeeIds = allEmployeeIds.Distinct().ToList();
            var AllInWarehouseData = GetInWarehouseDataByEmployeeIds(allEmployeeIds).ToList();
            var sourceInWarehouseData = AllInWarehouseData.Where(p => p.EmployeeId == EmployeeId).ToList();
            var toInWarehouseData = AllInWarehouseData.Where(p => EmployeeIds.Contains(p.EmployeeId));
            EntityList<InWarehouseEmployee> saveData = new EntityList<InWarehouseEmployee>();
            if (type == 1)
            {
                //将所选择员工的“可调拨至仓库”页签的数据清空，然后将当前员工的“可调拨至仓库”页签的数据插入到所选择员工“可调拨至仓库”页签
                EmployeeIds.ForEach(p =>
                {
                    sourceInWarehouseData.ForEach(x =>
                    {
                        var newInWarehouseData = new InWarehouseEmployee();
                        newInWarehouseData.Clone(x);
                        newInWarehouseData.PersistenceStatus = PersistenceStatus.New;
                        newInWarehouseData.EmployeeId = p;
                        saveData.Add(newInWarehouseData);
                    });
                });
            }
            if (type == 2)
            {
                //追加同步 将当前员工的“可调拨至仓库”页签的数据和每个所选择员工“可调拨至仓库”页签的数据进行分析，找出所选择员工“可调拨至仓库”页签的数据中不存在的数据，然后插入到所选择员工“可调拨至仓库”页签。
                EmployeeIds.ForEach(p =>
                {
                    var employeeToInWarehouseData = toInWarehouseData.Where(x => x.EmployeeId == p).ToList();
                    var toWarehouseId = employeeToInWarehouseData.Select(x => x.WarehouseId).ToList();
                    sourceInWarehouseData.Where(x => !toWarehouseId.Contains(x.WarehouseId)).ForEach(a =>
                    {
                        var newInWarehouseData = new InWarehouseEmployee();
                        newInWarehouseData.Clone(a);
                        newInWarehouseData.PersistenceStatus = PersistenceStatus.New;
                        newInWarehouseData.EmployeeId = p;
                        saveData.Add(newInWarehouseData);
                    });
                });
            }
            using (var tran = DB.TransactionScope(WareHouseEntityDataProvider.ConnectionStringName))
            {
                if (type == 1)
                {
                    //覆盖追加 先将需要同步的员工的仓库数据删除
                    DB.Delete<InWarehouseEmployee>().Where(p => EmployeeIds.Contains(p.EmployeeId)).Execute();
                }
                RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchInsertSave(saveData);
                tran.Complete();
            }
        }

        /// <summary>
        /// 获取员工对应的调拨至仓库数据
        /// </summary>
        /// <param name="employeeIds"></param>
        /// <returns></returns>
        private EntityList<InWarehouseEmployee> GetInWarehouseDataByEmployeeIds(List<double> employeeIds)
        {
            return employeeIds.SplitContains(tempIds =>
            {
                return Query<InWarehouseEmployee>().Where(x => tempIds.Contains(x.EmployeeId)).ToList();
            });
        }



        #endregion
    }
}
