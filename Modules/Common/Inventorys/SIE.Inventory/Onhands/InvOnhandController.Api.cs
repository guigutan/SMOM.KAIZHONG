using SIE.Api;
using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.RealTimeInventory;
using SIE.Items;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 库位库存控制器
    /// </summary>
    public partial class InvOnhandController
    {
        /// <summary>
        /// 获取空库位查询
        /// </summary>
        /// <param name="areaId">库区Id</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pageNumber">页号</param>
        /// <param name="pageSize">页码</param>
        /// <returns>空库位数据</returns>
        [ApiService("查询可用的库位")]
        [return: ApiReturn("返回可用库位数据集合：List<LocationData>")]
        public virtual List<LocationData> GetAvailableLocationDatas([ApiParameter("库区Id")] double areaId, [ApiParameter("查询关键字")] string keyword, [ApiParameter("页号")] int pageNumber, [ApiParameter("页码")] int pageSize)
        {
            List<LocationData> datas = new List<LocationData>();
            var whctl = RT.Service.Resolve<WarehouseController>();
            Dictionary<double, StorageLocationLayinInfo> dicLayInfo = new Dictionary<double, StorageLocationLayinInfo>();
            var loclist = GetAvailableLocationList(areaId, keyword, pageNumber, pageSize);

            loclist.ForEach((Action<StorageLocation>)(e =>
            {
                var data = new LocationData();
                data.Id = e.Id;
                data.Code = e.Code;
                data.Name = e.Name;
                data.AreaId = e.AreaId;
                data.AreaCode = e.Area.Code;
                data.AreaName = e.Area.Name;
                data.Type = e.LibraryType.ToLabel();
                if (!dicLayInfo.ContainsKey(e.Id))
                {
                    var layInfo = whctl.GetStorageLocationLayinInfo(e.Id);
                    if (layInfo != null) dicLayInfo.Add(e.Id, layInfo);
                }
                data.WeightLimit = dicLayInfo[e.Id].WeightLimit;
                data.VolumeLimit = dicLayInfo[e.Id].VolumeLimit;

                datas.Add(data);
            }));

            return datas;
        }

        /// <summary>
        /// 获取库存查询数据
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pageNumber">页号</param>
        /// <param name="pageSize">页码</param>
        /// <returns>返回库存查询数据集合</returns>
        [ApiService("获取库存查询数据")]
        [return: ApiReturn("返回库存查询数据集合：List<InventoryData>")]
        public virtual List<InventoryData> GetInventoryDatas([ApiParameter("仓库Id")] double warehouseId, [ApiParameter("查询关键字")] string keyword, [ApiParameter("页号")] int pageNumber, [ApiParameter("页码")] int pageSize)
        {
            List<InventoryData> datas = new List<InventoryData>();
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            var lotLpnOnhandList = GetLotLpnOnhandDatas(warehouseId, keyword, pageNumber, pageSize, elo);

            lotLpnOnhandList.ForEach(p =>
            {
                datas.Add(new InventoryData()
                {
                    LotLpnOnhandId = p.Id,
                    ItemCode = p.Item.Code,
                    ItemName = p.Item.Name,
                    ItemSpecificationModel = p.Item.SpecificationModel,
                    ItemUnitName = p.Item.Unit.Name,
                    LotCode = p.LotCode,
                    LocationId = p.StorageLocationId,
                    LocationCode = p.StorageLocation.Code,
                    Lpn = p.Lpn,
                    StorerCode = p.StorerCode,
                    ProjectNo = p.ProjectNo,
                    Qty = p.Qty,
                    AvailableQty = p.AvailableQty,
                    TaskNo = p.TaskNo,
                    ItemExtProp = p.ItemExtProp,
                    ItemExtPropName = p.ItemExtPropName,
                });
            });

            return datas;
        }

        /// <summary>
        /// 获取不同维度筛选数据
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="type">查询维度</param>
        /// <param name="pageNumber">页号</param>
        /// <param name="pageSize">页码</param>
        /// <returns>返回不同维度筛选数据集合</returns>
        [ApiService("获取不同维度筛选数据")]
        [return: ApiReturn("返回不同维度筛选数据集合：List<InventoryTypeData>")]
        public virtual List<InventoryTypeData> GetInventoryTypeDatas([ApiParameter("仓库Id")] double warehouseId, [ApiParameter("查询关键字")] string keyword, [ApiParameter("查询维度")] int type, [ApiParameter("页号")] int pageNumber, [ApiParameter("页码")] int pageSize)
        {
            var whCtl = RT.Service.Resolve<WarehouseController>();
            var itemCtl = RT.Service.Resolve<ItemController>();

            List<InventoryTypeData> datas = new List<InventoryTypeData>();
            EagerLoadOptions elo = new EagerLoadOptions();
            EntityList<LotLpnOnhand> lotLpnOnhandList = GetLotLpnOnhandDatas(warehouseId, keyword, pageNumber, pageSize, elo);

            switch (type)
            {
                case 0:  ////type为0查询的库位维度数据
                    var locIdList = lotLpnOnhandList.GroupBy(p => p.StorageLocationId).Select(p => p.Key).Distinct().ToList();
                    if (locIdList.Count > 0)
                    {
                        var loclist = whCtl.GetStorageLocations(locIdList, elo);
                        loclist.ForEach(p =>
                        {
                            datas.Add(new InventoryTypeData { Code = p.Code, Name = p.Name, SpecificationModel = string.Empty });
                        });
                    }

                    break;
                case 1:   ////type为1查询的LPN维度数据
                    var lpnList = lotLpnOnhandList.GroupBy(p => p.Lpn).Select(p => p.Key).Distinct().ToList();
                    if (lpnList.Count > 0)
                    {
                        lpnList.ForEach(lpn =>
                        {
                            datas.Add(new InventoryTypeData { Code = lpn, Name = string.Empty, SpecificationModel = string.Empty });
                        });
                    }

                    break;
                case 2:  ////type为2查询的料码维度数据
                    var itemIdList = lotLpnOnhandList.GroupBy(p => p.ItemId).Select(p => p.Key).Distinct().ToList();
                    if (itemIdList.Count > 0)
                    {
                        var itemlist = itemCtl.GetItemList(itemIdList);
                        itemlist.ForEach(p =>
                        {
                            datas.Add(new InventoryTypeData { Code = p.Code, Name = p.Name, SpecificationModel = p.SpecificationModel });
                        });
                    }

                    break;
                case 3:   ////type为3查询的批次维度数据
                    var lotList = lotLpnOnhandList.GroupBy(p => p.LotCode).Select(p => p.Key).Distinct().ToList();
                    if (lotList.Count > 0)
                    {
                        lotList.ForEach(lot =>
                        {
                            datas.Add(new InventoryTypeData { Code = lot, Name = string.Empty, SpecificationModel = string.Empty });
                        });
                    }

                    break;
                case 4:   ////type为4查询的货主维度数据
                    var storerCodeList = lotLpnOnhandList.GroupBy(p => p.StorerCode).Select(p => p.Key).Distinct().ToList();
                    if (storerCodeList.Count > 0)
                    {
                        storerCodeList.ForEach(storerCode =>
                        {
                            datas.Add(new InventoryTypeData { Code = storerCode, Name = string.Empty, SpecificationModel = string.Empty });
                        });
                    }

                    break;
                case 5:   ////type为5查询的项目维度数据
                    var projectNoList = lotLpnOnhandList.GroupBy(p => p.ProjectNo).Select(p => p.Key).Distinct().ToList();
                    if (projectNoList.Count > 0)
                    {
                        projectNoList.ForEach(projectNo =>
                        {
                            datas.Add(new InventoryTypeData { Code = projectNo, Name = string.Empty, SpecificationModel = string.Empty });
                        });
                    }

                    break;
                default:
                    break;
            }

            return datas;
        }

        /// <summary>
        /// 获取筛选不同维度库存数据
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="selectKeyword">选择关键字</param>
        /// <param name="type">查询维度</param>
        /// <param name="pageNumber">页号</param>
        /// <param name="pageSize">页码</param>
        /// <returns>返回筛选不同维度库存数据集合</returns>
        [ApiService("获取筛选不同维度库存数据")]
        [return: ApiReturn("返回筛选不同维度库存数据集合：List<InventoryData>")]
        public virtual List<InventoryData> GetInventorySelectDatas([ApiParameter("仓库Id")] double warehouseId, [ApiParameter("查询关键字")] string keyword,
                                                                   [ApiParameter("选择关键字")] string selectKeyword, [ApiParameter("查询维度")] int type,
                                                                   [ApiParameter("页号")] int pageNumber, [ApiParameter("页码")] int pageSize)
        {
            List<InventoryData> datas = new List<InventoryData>();
            EntityList<LotLpnOnhand> lotLpnOnhandList = new EntityList<LotLpnOnhand>();
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(LotLpnOnhand.StorageLocationProperty);
            elo.LoadWith(LotLpnOnhand.ItemProperty);
            elo.LoadWith(Item.UnitProperty);

            PagingInfo info = new PagingInfo() { PageNumber = pageNumber, PageSize = pageSize };

            if (selectKeyword == null)
            {
                return datas;
            }

            switch (type)
            {
                case 0:  ////type为0查询的库位维度数据
                    lotLpnOnhandList = GetLotLpnOnhands(warehouseId, e =>
                    {
                        GetKeyWords(e, keyword);
                        if (keyword.IsNullOrEmpty())
                        {
                            e.Join<StorageLocation>("b", (a, b) => a.StorageLocationId == b.Id && (b.Code.Contains(selectKeyword) || b.Name.Contains(selectKeyword))).OrderBy(p => p.StorageLocationId);
                        }
                        else
                        {
                            e.Where<StorageLocation>((a, sl) => a.StorageLocationId == sl.Id && (sl.Code.Contains(selectKeyword) || sl.Name.Contains(selectKeyword))).OrderBy(p => p.StorageLocationId);
                        }
                    }, true, info, elo);
                    break;
                case 1:   ////type为1查询的LPN维度数据
                    lotLpnOnhandList = GetLotLpnOnhands(warehouseId, e =>
                    {
                        GetKeyWords(e, keyword);
                        e.Where(p => p.Lpn.Contains(selectKeyword)).OrderBy(p => p.Lpn);
                    }, true, info, elo);

                    break;
                case 2:  ////type为2查询的料码维度数据
                    lotLpnOnhandList = GetLotLpnOnhands(warehouseId, e =>
                    {
                        GetKeyWords(e, keyword);
                        if (keyword.IsNullOrEmpty())
                        {
                            e.Join<Item>("i", (a, i) => a.ItemId == i.Id && (i.Code.Contains(selectKeyword) || i.Name.Contains(selectKeyword))).OrderBy(p => p.ItemId);
                        }
                        else
                        {
                            e.Where<Item>((a, it) => a.ItemId == it.Id && (it.Code.Contains(selectKeyword) || it.Name.Contains(selectKeyword))).OrderBy(p => p.ItemId);
                        }
                    }, true, info, elo);

                    break;
                case 3:   ////type为3查询的批次维度数据
                    lotLpnOnhandList = GetLotLpnOnhands(warehouseId, e =>
                    {
                        GetKeyWords(e, keyword);
                        e.Where(p => p.LotCode.Contains(selectKeyword)).OrderBy(p => p.LotCode);
                    }, true, info, elo);

                    break;
                case 4:   ////type为4查询的货主维度数据
                    lotLpnOnhandList = GetLotLpnOnhands(warehouseId, e =>
                    {
                        GetKeyWords(e, keyword);
                        e.Where(p => p.StorerCode.Contains(selectKeyword)).OrderBy(p => p.StorerCode);
                    }, true, info, elo);

                    break;
                case 5:   ////type为5查询的项目维度数据
                    lotLpnOnhandList = GetLotLpnOnhands(warehouseId, e =>
                    {
                        GetKeyWords(e, keyword);
                        e.Where(p => p.ProjectNo.Contains(selectKeyword)).OrderBy(p => p.ProjectNo);
                    }, true, info, elo);

                    break;
                default:
                    break;
            }

            lotLpnOnhandList.ForEach(p =>
            {
                datas.Add(new InventoryData()
                {
                    LotLpnOnhandId = p.Id,
                    ItemCode = p.Item.Code,
                    ItemName = p.Item.Name,
                    ItemSpecificationModel = p.Item.SpecificationModel,
                    ItemUnitName = p.Item.Unit.Name,
                    LotCode = p.LotCode,
                    LocationId = p.StorageLocationId,
                    LocationCode = p.StorageLocation.Code,
                    Lpn = p.Lpn,
                    StorerCode = p.StorerCode,
                    ProjectNo = p.ProjectNo,
                    Qty = p.Qty,
                    AvailableQty = p.AvailableQty,
                    OnhandState = p.State.ToLabel()
                });
            });

            return datas;
        }

        /// <summary>
        /// 根据关键字获取库位
        /// </summary>
        /// <param name="e"></param>
        /// <param name="keyword">关键字</param>
        private void GetKeyWords(IEntityQueryer<LotLpnOnhand> e, string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                e.LeftJoin<StorageLocation>("sl", (a, sl) => a.StorageLocationId == sl.Id)
             .LeftJoin<Item>("it", (a, it) => a.ItemId == it.Id)
             .Where<StorageLocation, Item>((x, sl, it) => x.Lpn.Contains(keyword) || x.LotCode.Contains(keyword) ||
               x.StorerCode.Contains(keyword) || x.ProjectNo.Contains(keyword) || sl.Code.Contains(keyword) || sl.Name.Contains(keyword) ||
               it.Code.Contains(keyword) || it.Name.Contains(keyword)).Distinct();
            }
        }

        /// <summary>
        /// 获取LPN库存
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="lpn">LPN编码</param>
        /// <returns>LPN库存</returns>
        [ApiService("获取LPN库存")]
        [return: ApiReturn("返回LPN库存：InventoryData")]
        public virtual List<InventoryData> GetLotLpnOnhands([ApiParameter("来源仓库")] double warehouseId, [ApiParameter("LPN编码")] string lpn)
        {
            var results = new List<InventoryData>();

            var onhandlist = GetLotLpnOnhands(warehouseId, e =>
             {
                 e.Where(p => p.Lpn.Contains(lpn));
             }, null, null, null);

            if (onhandlist.Count == 0)
            {
                throw new ValidationException("LPN[{0}]库存不存在或不可用".L10nFormat(lpn));
            }
            else
            {
                if (onhandlist.FirstOrDefault().StorageLocation.IsFrozen)
                    throw new ValidationException("LPN[{0}]所在库位[{1}]已冻结".L10nFormat(lpn, onhandlist.FirstOrDefault().StorageLocation.Name));
                if (onhandlist.Any(e => e.AllottedQty + e.FreezingQty > 0))
                    throw new ValidationException("LPN[{0}]库存已分配或冻结".L10nFormat(lpn));
            }

            onhandlist.ForEach(p =>
            {
                results.Add(new InventoryData()
                {
                    LotLpnOnhandId = p.Id,
                    ItemCode = p.Item.Code,
                    ItemName = p.Item.Name,
                    ItemSpecificationModel = p.Item.SpecificationModel,
                    ItemUnitName = p.Item.Unit.Name,
                    LotCode = p.LotCode,
                    LocationId = p.StorageLocationId,
                    LocationCode = p.StorageLocation.Code,
                    Lpn = p.Lpn,
                    StorerCode = p.StorerCode,
                    ProjectNo = p.ProjectNo,
                    Qty = p.Qty,
                    AvailableQty = p.AvailableQty
                });
            });

            return results;
        }

        /// <summary>
        /// 获取库位库存
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="locCode">库位编码</param>
        /// <returns>LPN库存</returns>
        [ApiService("获取库位库存")]
        [return: ApiReturn("返回库位库存数据集合：List<InventoryData>")]
        public virtual List<InventoryData> GetLocationOnhand([ApiParameter("来源仓库")] double warehouseId, [ApiParameter("库位编码")] string locCode)
        {
            List<InventoryData> results = new List<InventoryData>();
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(LotLpnOnhand.StorageLocationProperty);
            elo.LoadWith(LotLpnOnhand.ItemProperty);
            elo.LoadWith(Item.UnitProperty);
            var onhandlist = GetLotLpnOnhands(warehouseId, e =>
            {
                e.Join<StorageLocation>((a, b) => a.StorageLocationId == b.Id && b.State == State.Enable && !b.IsFrozen && (b.Code.Contains(locCode) || b.Name.Contains(locCode)));
            }, true, null, elo);
            if (onhandlist.Count == 0)
            {
                throw new ValidationException("库位[{0}]库存不存在或不可用".L10nFormat(locCode));
            }
            else
            {
                if (onhandlist.FirstOrDefault().StorageLocation.IsFrozen)
                    throw new ValidationException("库位[{0}]已冻结".L10nFormat(onhandlist.FirstOrDefault().StorageLocation.Name));
                if (onhandlist.Any(e => e.AllottedQty + e.FreezingQty > 0))
                    throw new ValidationException("库位[{0}]的库存已分配或冻结".L10nFormat(locCode));
                if (onhandlist.Any(e => e.State == OnhandState.None))
                    throw new ValidationException("库位[{0}]存在未质检库存".L10nFormat(locCode));
            }

            onhandlist.ForEach(p =>
            {
                results.Add(new InventoryData()
                {
                    LotLpnOnhandId = p.Id,
                    ItemCode = p.Item.Code,
                    ItemName = p.Item.Name,
                    ItemSpecificationModel = p.Item.SpecificationModel,
                    ItemUnitName = p.Item.Unit.Name,
                    LotCode = p.LotCode,
                    LocationId = p.StorageLocationId,
                    LocationCode = p.StorageLocation.Code,
                    Lpn = p.Lpn,
                    StorerCode = p.StorerCode,
                    ProjectNo = p.ProjectNo,
                    Qty = p.Qty,
                    AvailableQty = p.AvailableQty
                });
            });

            return results;
        }

        /// <summary>
        /// APP库存查看界面获取库存（唯一入口）
        /// </summary>
        /// <param name="onhandQueryData">查询参数</param>      
        /// <returns>库存</returns>
        [ApiService("APP库存查看界面获取库存（唯一入口）")]
        [return: ApiReturn("返回库存数据集合：List<LotLpnOnhand>")]
        public virtual EntityList<LotLpnOnhand> AppGetLotLpnOnhands(OnhandQueryDataBase onhandQueryData)
        {
            var query = Query<LotLpnOnhand>().Where(p => p.WarehouseId == onhandQueryData.WarehouseId);
            if (onhandQueryData.StorageLocationCode.IsNotEmpty())
                query.Where(p => p.StorageLocation.Code == onhandQueryData.StorageLocationCode);
            if (onhandQueryData.Lpn.IsNotEmpty())
                query.Where(p => p.Lpn == onhandQueryData.Lpn);
            if (onhandQueryData.ItemCode.IsNotEmpty())
                query.Where(p => p.Item.Code == onhandQueryData.ItemCode);
            if (onhandQueryData.LotCode.IsNotEmpty())
                query.Where(p => p.Lot.Code == onhandQueryData.LotCode);
            if (onhandQueryData.ProjectNo.IsNotEmpty())
                query.Where(p => p.ProjectNo == onhandQueryData.ProjectNo);
            if (onhandQueryData.StorerCode.IsNotEmpty())
                query.Where(p => p.StorerCode == onhandQueryData.StorerCode);
            if (onhandQueryData.ItemExtPropName.IsNotEmpty())
                query.Where(p => p.ItemExtProp == onhandQueryData.ItemExtPropName || p.ItemExtPropName == onhandQueryData.ItemExtPropName);
            if (onhandQueryData.TaskNo.IsNotEmpty())
                query.Where(p => p.TaskNo == onhandQueryData.TaskNo);
            if (onhandQueryData.OnhandState.HasValue && onhandQueryData.OnhandState > 0)
            {
                query.Where(p => p.State == onhandQueryData.OnhandState);
            }
            query.Where(p => p.Qty > 0);

            if (onhandQueryData.Qty > 0)
            {
                query.Where(p => p.AvailableQty >= onhandQueryData.Qty);
            }
            var onhands = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (!onhands.Any() && !onhandQueryData.AllowEmpty)
                throw new ValidationException("当前没有现有量大于0的库存".L10N());

            return onhands;
        }

        /// <summary>
        /// ReturnOnhandData
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="onhands">库存数据</param>
        /// <returns>包装库存数据</returns>
        public virtual List<PackOnhandDataBase> ReturnOnhandData(int? type, EntityList<LotLpnOnhand> onhands)
        {
            List<PackOnhandDataBase> rst = new List<PackOnhandDataBase>();
            onhands.ForEach(p =>
            {
                rst.Add(new PackOnhandDataBase()
                {
                    Id = p.Id,
                    ItemCode = p.ItemCode,
                    ItemName = p.ItemName,
                    ItemSpecificationModel = p.ItemSpecificationModel,
                    ItemExtProp = p.ItemExtProp,
                    ItemExtPropName = p.ItemExtPropName,
                    ItemId = p.ItemId,
                    ItemUnitName = p.UnitName,
                    LocCode = p.StorageLocationCode,
                    LotCode = p.LotCode,
                    Lpn = p.Lpn,
                    OnhandStateName = p.State.ToLabel(),
                    Qty = p.AvailableQty,
                    ProjectNo = p.ProjectNo,
                    TaskNo = p.TaskNo,
                    StorerCode = p.StorerCode,
                    FreezingQty = p.FreezingQty,
                    AllotQty = p.AllottedQty,
                });
            });

            return rst;
        }

        /// <summary>
        /// 获取实时库存信息
        /// </summary>
        /// <param name="warehouseIds">仓库ID列表</param>
        /// <param name="itemIds">物料ID列表</param>
        /// <param name="date">日期</param>
        /// <returns>实时库存信息</returns>
        public virtual List<RealTimeInvInfo> GetRealTimeInvInfos(List<double> warehouseIds, List<double> itemIds, DateTime? date)
        {
            List<RealTimeInvInfo> invInfos = new List<RealTimeInvInfo>();

            var onHandQuery = Query<LotLpnOnhand>();

            //In 太多问题 DataProcessEx.SplitContains  后做修改
            if (warehouseIds != null && warehouseIds.Count > 0)
            {
                var whExp = warehouseIds.CreateContainsExpression<LotLpnOnhand>("x", "WarehouseId");
                onHandQuery.Where(whExp);
            }
            if (itemIds != null && itemIds.Count > 0)
            {
                var itemExp = itemIds.CreateContainsExpression<LotLpnOnhand>("y", "ItemId");
                onHandQuery.Where(itemExp);
            }
            if (date.HasValue)
                onHandQuery.Where(p => p.CreateDate >= date.Value);

            var onhands = onHandQuery.ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            var onhandGroups = onhands.GroupBy(p => new { p.WarehouseId, p.ItemId, p.ItemExtProp, p.ItemExtPropName }).Select(t => new
            {
                t.Key.WarehouseId,
                t.Key.ItemId,
                t.Key.ItemExtProp,
                t.Key.ItemExtPropName,
                Qty = t.Sum(s => s.Qty),
                AvailableQty = t.Sum(s => s.AvailableQty),
                AllottedQty = t.Sum(s => s.AllottedQty),
                FreezingQty = t.Sum(s => s.FreezingQty)
            });

            onhandGroups.ForEach(p =>
            {
                invInfos.Add(new RealTimeInvInfo
                {
                    WarehouseId = p.WarehouseId,
                    ItemId = p.ItemId,
                    ItemExtProp = p.ItemExtProp,
                    ItemExtPropName = p.ItemExtPropName,
                    Qty = p.Qty,
                    AvailableQty = p.AvailableQty,
                    AllottedQty = p.AllottedQty,
                    FreezingQty = p.FreezingQty,
                    RealTimeInvType = 1,
                    WorkOrderNo = string.Empty
                });
            });

            return invInfos;
        }
    }
}