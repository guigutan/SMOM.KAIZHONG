using SIE.Api;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Warehouses
{
    public partial class WarehouseController
    {
        /// <summary>
        /// 获取登录用户仓库信息
        /// </summary>
        /// <returns>仓库信息</returns>
        [ApiService("获取登录用户仓库信息")]
        [return: ApiReturn("返回登录用户仓库信息集合：List<WarehouseData>")]
        public virtual List<WarehouseData> GetWarehouses()
        {
            List<WarehouseData> datas = new List<WarehouseData>();
            var warehouses = RT.Service.Resolve<WarehouseController>().GetUserWarehouses();
            warehouses.ForEach(e =>
            {
                datas.Add(new WarehouseData()
                {
                    UserId = RT.IdentityId,
                    WarehouseId = e.Id,
                    WarehouseCode = e.Code,
                    WarehouseName = e.Name
                });
            });
            return datas;
        }

        /// <summary>
        /// 获取登录用户仓库信息 过滤掉线边仓
        /// </summary>
        /// <returns>仓库信息</returns>
        [ApiService("获取登录用户仓库信息")]
        [return: ApiReturn("返回登录用户仓库信息集合：List<WarehouseData>")]
        public virtual List<WarehouseData> GetWarehousesWithOutLine()
        {
            List<WarehouseData> datas = new List<WarehouseData>();
            var warehouses = RT.Service.Resolve<WarehouseController>().GetUserWarehouses(true);
            warehouses.ForEach(e =>
            {
                datas.Add(new WarehouseData()
                {
                    UserId = RT.IdentityId,
                    WarehouseId = e.Id,
                    WarehouseCode = e.Code,
                    WarehouseName = e.Name
                });
            });
            return datas;
        }


        /// <summary>
        /// 获取员工可调拨至仓库
        /// </summary>
        /// <param name="type">1-两部调拨 2-直接调拨</param>
        /// <returns></returns>
        [ApiService("获取调拨至仓库数据")]
        [return: ApiReturn("返回登录用户仓库信息集合：List<WarehouseData>")]
        public virtual List<WarehouseData> GetAllotWarehouseDatas(int type=1)
        {
            List<WarehouseData> datas = new List<WarehouseData>();
            var userId = RT.IdentityId;
            List<double> InWareHouseId = new List<double>();
            var canInWareHouse = GetInWarehouseDataByEmployeeIds(new List<double>() { userId });
            if (canInWareHouse.Count > 0)
            {
                //说明有调拨至仓库取值
                if (type == 2)
                {
                    InWareHouseId = canInWareHouse.Where(p => p.IsDirectAllocate).Select(p => p.WarehouseId).Distinct().ToList();
                }
                else if(type == 1)
                {
                    InWareHouseId = canInWareHouse.Where(p => p.IsTwoAllocate).Select(p => p.WarehouseId).Distinct().ToList();
                }
            }
            var warehouseDatas = GetUserAllocateWarehouses(InWareHouseId);
            warehouseDatas.ForEach(e =>
            {
                datas.Add(new WarehouseData()
                {
                    UserId = RT.IdentityId,
                    WarehouseId = e.Id,
                    WarehouseCode = e.Code,
                    WarehouseName = e.Name
                });
            });
            return datas;
        }
        /// <summary>
        /// 获取登录用户仓库信息
        /// </summary>
        /// <returns>仓库信息</returns>
        [ApiService("获取可用仓库信息")]
        [return: ApiReturn("返回可用仓库信息集合：List<WarehouseData>")]
        public virtual List<WarehouseData> GetEnableWarehouses()
        {
            List<WarehouseData> datas = new List<WarehouseData>();
            var warehouses = GetEnableWarehouses(null, string.Empty);
            warehouses.ForEach(e =>
            {
                datas.Add(new WarehouseData()
                {
                    UserId = RT.IdentityId,
                    WarehouseId = e.Id,
                    WarehouseCode = e.Code,
                    WarehouseName = e.Name
                });
            });
            return datas;
        }

        /// <summary>
        /// 获取指定仓库下未冻结、未禁用的库位
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <returns>返回ASN未收货对应的物料</returns>
        [ApiService("获取指定仓库下未冻结、未禁用的库位")]
        [return: ApiReturn("返回指定仓库下未冻结、未禁用的库位集合：List<LocationData>")]
        public virtual List<LocationData> GetWarehouseLocations([ApiParameter("仓库Id")] double warehouseId)
        {
            var whCtl = RT.Service.Resolve<WarehouseController>();
            List<LocationData> results = new List<LocationData>();

            var locs = whCtl.GetEnableStorageLocations(warehouseId, string.Empty, null);

            locs.ForEach(e =>
            {
                var data = new LocationData();
                data.Id = e.Id;
                data.Code = e.Code;
                data.Name = e.Name;
                data.AreaId = e.AreaId;
                data.AreaCode = e.Area.Code;
                data.AreaName = e.Area.Name;

                results.Add(data);
            });

            return results;
        }

        /// <summary>
        /// 获取指定仓库下未冻结、未禁用的库位
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <returns>返回ASN未收货对应的物料</returns>
        [ApiService("获取指定仓库下未冻结、未禁用的库位")]
        [return: ApiReturn("返回指定仓库下未冻结、未禁用的库位集合：List<LocationData>")]
        public virtual List<LocationData> GetWarehouseLocationsIsPick([ApiParameter("仓库Id")] double warehouseId)
        {
            var whCtl = RT.Service.Resolve<WarehouseController>();
            List<LocationData> results = new List<LocationData>();

            var locs = whCtl.GetEnableStorageLocations(warehouseId, string.Empty, null, true);

            locs.ForEach(e =>
            {
                var data = new LocationData();
                data.Id = e.Id;
                data.Code = e.Code;
                data.Name = e.Name;
                data.AreaId = e.AreaId;
                data.AreaCode = e.Area.Code;
                data.AreaName = e.Area.Name;

                results.Add(data);
            });

            return results;
        }

        /// <summary>
        /// 验证库位编码有效性
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="code">库位编码</param>
        /// <param name="CheckLiku">校验库位所在库区是否可以人工上架</param>
        /// <param name="isTraget">是否检查目标仓库库位</param>
        /// <param name="isIgnoreFreeze">是否忽略检验冻结库位</param>
        [ApiService("验证库位编码有效性")]
        public virtual StorageLocation CheckLoctaion([ApiParameter("仓库Id")] double warehouseId, [ApiParameter("库位编码")] string code, [ApiParameter("检查立库")] bool CheckLiku, [ApiParameter("是否检查目标仓库库位")] bool isTraget, [ApiParameter("是否忽略冻结库位")] bool isIgnoreFreeze)
        {
            var loc = GetStorageLocation(warehouseId, code);
            if (loc == null)
            {
                if (isTraget)
                {
                    var wh = RF.GetById<Warehouse>(warehouseId);
                    throw new ValidationException("目标仓库[{1}]不存在库位:[{0}]，请输入有效库位".L10nFormat(code,wh?.Code));
                }
                throw new ValidationException("当前仓库不存在库位:[{0}]，请输入有效库位".L10nFormat(code));
            }

            if (loc.IsFrozen && !isIgnoreFreeze)
                throw new ValidationException("库位:[{0}]已冻结，请输入有效库位".L10nFormat(code));

            if (loc.State == State.Disable)
                throw new ValidationException("库位:[{0}]已禁用，请输入有效库位".L10nFormat(code));
            if (CheckLiku)
            {
                if (!loc.IsAllowManualGrounding)
                {
                    throw new ValidationException("库位:[{0}]所在的库区不允许人工上架，此模块只允许扫描可允许人工上架的库位，请输入有效库位".L10nFormat(code));
                }
            }
            return loc;
        }

        /// <summary>
        /// 验证库位编码有效性
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="code">库位编码</param>       
        [ApiService("验证库位编码有效性")]
        public virtual StorageLocation CheckLoctaionLiKu([ApiParameter("仓库Id")] double warehouseId, [ApiParameter("库位编码")] string code)
        {
            var loc = GetStorageLocation(warehouseId, code);
            if (loc == null)
                throw new ValidationException("当前仓库不存在库位:[{0}]，请输入有效库位".L10nFormat(code));

            if (loc.IsFrozen)
                throw new ValidationException("库位:[{0}]已冻结，请输入有效库位".L10nFormat(code));

            if (loc.State == State.Disable)
                throw new ValidationException("库位:[{0}]已禁用，请输入有效库位".L10nFormat(code));
            if (!loc.IsAllowManualGrounding)
            {
                var op = GetStorageAreaOperationDetail(loc.AreaId);
                if (op.UpTransitLocationId != loc.Id)
                    throw new ValidationException("库位:[{0}]所在的库区不允许人工上架，此模块只允许扫描可允许人工上架的库位，请输入有效库位".L10nFormat(code));
            }

            return loc;
        }

        /// <summary>
        /// 立库拣货-验证库位编码有效性
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="code">库位编码</param>       
        [ApiService("验证库位编码有效性")]
        public virtual StorageLocation CheckLoctaionLiKuByPickUp([ApiParameter("仓库Id")] double warehouseId, [ApiParameter("库位编码")] string code)
        {
            var loc = GetStorageLocation(warehouseId, code);
            if (loc == null)
                throw new ValidationException("当前仓库不存在库位:[{0}]，请输入有效库位".L10nFormat(code));

            if (loc.IsFrozen)
                throw new ValidationException("库位:[{0}]已冻结，请输入有效库位".L10nFormat(code));

            if (loc.State == State.Disable)
                throw new ValidationException("库位:[{0}]已禁用，请输入有效库位".L10nFormat(code));
            if (!loc.IsAllowManualGrounding)
            {
                throw new ValidationException("库位:[{0}]所在的库区不允许人工上架，此模块只允许扫描可允许人工上架的库位，请输入有效库位".L10nFormat(code));
            }
            if (loc.IsInLock || loc.IsOutLock || loc.IsCountLock)
            {
                throw new ValidationException("库位:[{0}]入库锁/出库锁/盘点锁已锁".L10nFormat(code));
            }
            if (loc.IsBackup)
            {
                throw new ValidationException("库位:[{0}]是预留库位".L10nFormat(code));
            }
            return loc;
        }

        /// <summary>
        /// 获取当前仓库下的库区信息
        /// </summary>
        /// <returns>库区信息</returns>
        [ApiService("获取当前仓库下的库区信息")]
        [return: ApiReturn("返回库区信息集合：List<StorageAreaData>")]
        public virtual List<StorageAreaData> GetStorageAreaDatas([ApiParameter("仓库Id")] double warehouseId)
        {
            List<StorageAreaData> datas = new List<StorageAreaData>();
            string keyword = string.Empty;
            PagingInfo info = new PagingInfo();
            var areaList = GetStorageAreas(warehouseId, keyword, info);
            areaList.ForEach(e =>
            {
                datas.Add(new StorageAreaData()
                {
                    AreaId = e.Id,
                    AreaCode = e.Code,
                    AreaName = e.Name
                });
            });
            return datas;
        }

        /// <summary>
        /// 获取未禁用的目标仓库
        /// </summary>
        /// <returns>仓库数据</returns>
        [ApiService("获取未禁用的目标仓库")]
        [return: ApiReturn("返回未禁用的目标仓库集合：List<WarehouseData>")]
        public virtual List<WarehouseData> GetWarehouseDatas([ApiParameter("查询关键字")] string keyword, [ApiParameter("页号")] int pageNumber, [ApiParameter("页码")] int pageSize)
        {
            List<WarehouseData> datas = new List<WarehouseData>();
            PagingInfo pageinfo = new PagingInfo();
            pageinfo.PageNumber = pageNumber;
            pageinfo.PageSize = pageSize;
            var warehouseList = GetEnableWarehouses(pageinfo, keyword);
            if (warehouseList.Count == 0) throw new ValidationException("未查找到目标仓库!".L10N());
            warehouseList.ForEach(e =>
            {
                datas.Add(new WarehouseData()
                {
                    UserId = RT.IdentityId,
                    WarehouseId = e.Id,
                    WarehouseCode = e.Code,
                    WarehouseName = e.Name
                });
            });

            return datas;
        }

        /// <summary>
        /// 获取PICKTO库位
        /// </summary>
        /// <param name="warehouseId">目标仓库Id</param>
        /// <returns>PICKTO库位</returns>
        [ApiService("获取PICKTO库位")]
        [return: ApiReturn("返回PICKTO库位：LocationData")]
        public virtual LocationData GetPickToLoction([ApiParameter("目标仓库Id")] double warehouseId)
        {
            var pickTolocation = GetPickToStorageLocation(warehouseId);
            if (pickTolocation == null) throw new ValidationException("未查找到PICKTO库位!".L10N());

            LocationData data = new LocationData();
            data.Id = pickTolocation.Id;
            data.Code = pickTolocation.Code;
            data.Name = pickTolocation.Name;
            data.AreaId = pickTolocation.AreaId;
            data.AreaCode = pickTolocation.Area.Code;
            data.AreaName = pickTolocation.Area.Name;

            return data;
        }
    }
}
