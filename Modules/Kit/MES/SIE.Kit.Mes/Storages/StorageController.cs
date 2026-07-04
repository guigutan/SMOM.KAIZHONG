using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Tech.Stations;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Kit.MES.Storages
{
    /// <summary>
    /// 货区货位控制器
    /// </summary>
    public class StorageController : DomainController
    {
        /// <summary>
        /// 根据工位货区ID获取工位集合
        /// </summary>
        /// <param name="areaId">工位货区ID</param>
        /// <param name="sortInfo">排序集合</param>
        /// <param name="pagingInfo">分页对象</param>
        /// <returns>返回指定工位货区的工位集合</returns>
        public virtual EntityList<StationStorageArea> GetStationAreas(double areaId, List<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            return Query<StationStorageArea>()
                .Where(p => p.StorageAreaId == areaId).OrderBy(sortInfo)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工位货区ID获取物料安全库存配置集合
        /// </summary>
        /// <param name="areaId">工位货区ID</param>
        /// <param name="sortInfo">排序集合</param>
        /// <param name="pagingInfo">分页对象</param>
        /// <returns>返回指定工位货区的物料库存集合</returns>
        public virtual EntityList<StorageSafty> GetStorageSaftys(double areaId, List<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            return Query<StorageSafty>()
                .Where(p => p.StorageAreaId == areaId).OrderBy(sortInfo)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工位货区ID获取货位
        /// </summary>
        /// <param name="areaId">工位货区ID</param>
        /// <param name="sortInfo">排序集合</param>
        /// <param name="pagingInfo">分页对象</param>
        /// <returns>返回指定工位货区的货位集合</returns>
        public virtual EntityList<StorageLocation> GetStorageLocations(double areaId, List<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            return Query<StorageLocation>()
                .Where(p => p.StorageAreaId == areaId).OrderBy(sortInfo)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据货位ID获取产线物料货位集合
        /// </summary>
        /// <param name="locationId">货位ID</param>
        /// <param name="sortInfo">排序集合</param>
        /// <param name="pagingInfo">分页对象</param> 
        /// <returns>返回指定货位的物料集合</returns>
        public virtual EntityList<ItemStorage> GetItemStorages(double locationId, List<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            return Query<ItemStorage>()
                .Where(p => p.StorageLocationId == locationId).OrderBy(sortInfo)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 工位是否已有指定类型的货区
        /// </summary>
        /// <param name="stationId">工位ID</param>
        /// <param name="type">货区类型</param>
        /// <returns>如果存在返回true，否则返回false</returns>
        public virtual bool IsStationAreaExists(double stationId, StorageAreaType type)
        {
            return Query<StationStorageArea>()
                .Where(p => p.StationId == stationId && p.StorageArea.Type == type)
                .Count() > 0;
        }

        /// <summary>
        /// 查询货区物料库存
        /// </summary>
        /// <param name="areaId">货区</param>
        /// <param name="itemId">物料</param>
        /// <returns>库存</returns>
        public virtual decimal GetItemStorageQtyByArea(double areaId, double itemId)
        {
            return Query<ItemStorage>()
                .Where(p => p.ItemId == itemId && p.StorageLocation.StorageAreaId == areaId)
                .Select(p => p.Qty.SUM())
                .FirstOrDefault<decimal>();
        }

        /// <summary>
        /// 通过货区获取产线物料货位数量
        /// </summary>
        /// <param name="wh">仓库编码</param>
        /// <param name="area">货区编码</param>
        /// <param name="itemNo">物料编码</param>
        /// <returns>产线物料货位</returns>
        public virtual decimal GetItemStorageQtyByArea(string wh, string area, string itemNo)
        {
            return Query<ItemStorage>()
                .Where(p => p.Item.Code == itemNo && p.StorageLocation.StorageArea.Code == area && p.StorageLocation.StorageArea.Warehouse.Code == wh)
                .Select(p => p.Qty.SUM())
                .FirstOrDefault<decimal>();
        }

        /// <summary>
        /// 查询货位物料库存
        /// </summary>
        /// <param name="locationId">货位</param>
        /// <param name="itemId">物料</param>
        /// <returns>库存</returns>
        public virtual decimal GetItemStorageQtyByLocation(double locationId, double itemId)
        {
            return Query<ItemStorage>()
                .Where(p => p.ItemId == itemId && p.StorageLocationId == locationId)
                .Select(p => p.Qty)
                .FirstOrDefault<decimal>();
        }

        /// <summary>
        /// 通过货位获取产线物料货位
        /// </summary>
        /// <param name="wh">仓库编码</param>
        /// <param name="area">货区编码</param>
        /// <param name="location">货位编码</param>
        /// <param name="itemNo">物料编码</param>
        /// <returns>产线物料货位</returns>
        public virtual decimal GetItemStorageQtyByLocation(string wh, string area, string location, string itemNo)
        {
            return Query<ItemStorage>()
                .Where(p => p.Item.Code == itemNo && p.StorageLocation.Code == location && p.StorageLocation.StorageArea.Code == area && p.StorageLocation.StorageArea.Warehouse.Code == wh)
                .Select(p => p.Qty)
                .FirstOrDefault<decimal>();
        }

        /// <summary>
        /// 根据工位物料查找货位，如果找到多个货位，会引发<see cref="StorageLocationAmbiguityException"/>异常，如果没找到为返回null
        /// </summary>
        /// <param name="stationId">货区ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="storageLocation">指定货位</param>
        /// <returns>产线货区货位</returns>
        /// <exception cref="StorageLocationAmbiguityException">找到多个货位</exception>
        public virtual StorageLocation FindStorageLocation(double stationId, double itemId, string storageLocation = null)
        {
            var locations = Query<StorageLocation>()
                .Join<ItemStorage>((l, i) => l.Id == i.StorageLocationId)
                .Join<StationStorageArea>((l, a) => l.StorageAreaId == a.StorageAreaId)
                .Where<ItemStorage, StationStorageArea>((l, i, a) => i.ItemId == itemId && a.StationId == stationId)
                .Distinct().ToList();
            if (storageLocation.IsNotEmpty() && locations.Any())
            {
                var location = locations.FirstOrDefault(p => p.Code == storageLocation);
                if (location == null)
                    throw new ValidationException("货位[{0}]不存在或物料[{1}]不能放于此货位".L10nFormat(storageLocation, GetById<Item>(itemId)?.Code));
                return location;
            }

            if (locations.Count > 1)
                throw new StorageLocationAmbiguityException(locations.Select(p => p.Code).Concat(","));

            if (locations.Count == 0)
            {
                var storageArea = GetInputAreaByStationId(stationId);
                if (storageArea == null)
                    throw new ValidationException("工位[{0}]未设置{1}货区".L10nFormat(GetById<Station>(stationId)?.Code, StorageAreaType.Input.ToLabel()));

                var location = Query<StorageLocation>()
                    .Where(p => p.StorageAreaId == storageArea.Id && p.IsCommon)
                    .FirstOrDefault();
                if (location == null)
                {
                    location = new StorageLocation();
                    location.StorageArea = storageArea;
                    location.Code = storageArea.Code;
                    location.Name = storageArea.Name;
                    location.IsCommon = true;
                    RF.Save(location);
                }

                return location;
            }

            return locations.FirstOrDefault();
        }

        /// <summary>
        /// 根据工位ID找投入区
        /// </summary>
        /// <param name="stationId">工位ID</param>
        /// <returns>工位货区</returns>
        public virtual StorageArea GetInputAreaByStationId(double stationId)
        {
            return Query<StorageArea>()
                .Join<StationStorageArea>((a, s) => a.Id == s.StorageAreaId)
                .Where<StationStorageArea>((a, s) => s.StationId == stationId && a.Type == StorageAreaType.Input)
                .FirstOrDefault();
        }

        /// <summary>
        /// 根据工位ID集合找货区
        /// </summary>
        /// <param name="stationIds">工位ID</param>
        /// <returns>工位货区</returns>
        public virtual EntityList<StationStorageArea> GetAreaByStationIds(List<double> stationIds)
        {
            return Query<StationStorageArea>().Where(s => stationIds.Contains(s.StationId)).ToList();
        }

        /// <summary>
        /// 根据工位ID找产出区
        /// </summary>
        /// <param name="stationId">工位ID</param>
        /// <returns>工位货区</returns>
        public virtual StorageArea GetOutputAreaByStationId(double stationId)
        {
            return Query<StorageArea>()
                .Join<StationStorageArea>((a, s) => a.Id == s.StorageAreaId)
                .Where<StationStorageArea>((a, s) => s.StationId == stationId && a.Type == StorageAreaType.Output)
                .FirstOrDefault();
        }

        /// <summary>
        /// 增加物料库存数量，数量可正可负
        /// </summary>
        /// <param name="locationId">货位ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="qty">库存变化数量</param>
        public virtual void AddItemStorageQty(double locationId, double itemId, decimal qty)
        {
            var result = DB.Update<ItemStorage>()
                .Set(p => p.Qty, p => p.Qty + qty)
                .Where(p => p.ItemId == itemId && p.StorageLocationId == locationId)
                .Execute();

            if (result == 0)
            {
                var item = RF.GetById<Item>(itemId);

                if (item.ConsumeMode == ConsumeMode.Pull)
                {
                    throw new ValidationException("拉式物料上料或扣料要提前维护[产线物料货位]资料".L10nFormat());
                }

                //没更新到，插入一条
                var storage = new ItemStorage
                {
                    ItemId = itemId,
                    Qty = qty,
                    StorageLocationId = locationId
                };

                RF.Save(storage);
            }

            OnStorageChanged(locationId, itemId);
            CheckStorageSafty(locationId, itemId);
        }

        /// <summary>
        /// 增加物料库存数量
        /// </summary>
        /// <param name="wh">胀库编码</param>
        /// <param name="area">货区编码</param>
        /// <param name="location">货位编码</param>
        /// <param name="itemNo">物料编码</param>
        /// <param name="qty">数量</param>
        public virtual void AddItemStorageQty(string wh, string area, string location, string itemNo, decimal qty)
        {
            var l = Query<StorageLocation>()
                .Where(p => p.StorageArea.Warehouse.Code == wh && p.StorageArea.Code == area && p.Code == location)
                .FirstOrDefault();
            if (l == null)
                throw new EntityNotFoundException("找不到仓库{0}货区{1}货位{2}".L10nFormat(wh, area, location));
            var item = Query<Item>().Where(p => p.Code == itemNo).FirstOrDefault();
            if (item == null)
                throw new EntityNotFoundException("找不到物料{0}".L10nFormat(itemNo));
            AddItemStorageQty(l.Id, item.Id, qty);
        }

        /// <summary>
        /// 库存变更通知
        /// </summary>
        /// <param name="locationId">货位ID</param>
        /// <param name="itemId">物料ID</param>
        protected virtual void OnStorageChanged(double locationId, double itemId)
        {
            //RT.RemotingEventBus.Publish(new ItemStorageChangedEvent
            //{
            //    ItemId = itemId,
            //    LocationId = locationId,
            //    IsWip = true
            //});
        }

        /// <summary>
        /// 检查产线库存
        /// </summary>
        /// <param name="locationId">货位ID</param>
        /// <param name="itemId">物料ID</param>
        protected virtual void CheckStorageSafty(double locationId, double itemId)
        {
            var safty = Query<StorageSafty>()
                .Join<StorageLocation>((x, y) => x.StorageAreaId == y.StorageAreaId)
                .Where<StorageLocation>((x, y) => x.ItemId == itemId && y.Id == locationId)
                .FirstOrDefault();
            if (safty != null)
            {
                var total = GetItemStorageQtyByArea(safty.StorageAreaId, itemId);
                if (total < safty.SafetyQty)
                    OnStorageStarving(safty, total);
            }
        }

        /// <summary>
        /// 产线库存缺货
        /// </summary>
        /// <param name="safty">产线库存</param>
        /// <param name="qty">缺货数量</param>
        protected virtual void OnStorageStarving(StorageSafty safty, decimal qty)
        {
            //EventBus通知物料配送
            RT.EventBus.Publish(new StorageSaftyEvent { StorageSafty = safty });

            //消息队列通知看板
            var e = new StorageStarvingEvent
            {
                DeliveryQty = safty.DeliveryQty,
                ItemId = safty.ItemId,
                Qty = qty,
                SaftyQty = safty.SafetyQty,
                StorageAreaId = safty.StorageAreaId,
                AreaType = safty.StorageArea.Type
            };
            RT.RemotingEventBus.Publish(e);
        }

        /// <summary>
        /// 增加物料库存数量，数量可正可负
        /// </summary>
        /// <param name="stationId">工位ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="qty">库存变化数量</param>
        /// <param name="isPullItem">拉式物料</param>
        public virtual void AddItemStorageQtyByStation(double stationId, double itemId, decimal qty, bool isPullItem = false)
        {
            var location = Query<StorageLocation>()
                   .Join<ItemStorage>((l, i) => l.Id == i.StorageLocationId)
                   .Join<StationStorageArea>((l, a) => l.StorageAreaId == a.StorageAreaId)
                   .Where<ItemStorage, StationStorageArea>((l, i, a) => i.ItemId == itemId && a.StationId == stationId && !l.IsCommon)
                   .FirstOrDefault();
            if (location == null)
            {
                var storageArea = GetInputAreaByStationId(stationId);
                if (storageArea == null)
                    throw new ValidationException("工位[{0}]未设置{1}货区".L10nFormat(GetById<Station>(stationId)?.Code, StorageAreaType.Input.ToLabel()));

                location = Query<StorageLocation>()
                    .Where(p => p.StorageAreaId == storageArea.Id && p.IsCommon)
                    .FirstOrDefault();
                if (location == null)
                {
                    location = new StorageLocation();
                    location.StorageArea = storageArea;
                    location.Code = storageArea.Code;
                    location.Name = storageArea.Name;
                    location.IsCommon = true;
                    RF.Save(location);
                }
            }

            AddItemStorageQty(location.Id, itemId, qty);

            if (isPullItem)
            {
                //更新物料库存
                DB.Update<StorageSafty>()
                    .Set(p => p.SurplusQty, p => p.SurplusQty + qty)
                    .Where(p => p.ItemId == itemId && p.StorageLocationId == location.Id
                        && p.StorageAreaId == location.StorageAreaId)
                    .Execute();
            }
        }

        /// <summary>
        /// 更新货区物料数量
        /// </summary>
        /// <param name="locationId">货位ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="qty">更新数量</param>
        public virtual void UpdateItemStorageQty(double locationId, double itemId, decimal qty)
        {
            var result = DB.Update<ItemStorage>()
                .Set(p => p.Qty, qty)
                .Where(p => p.StorageLocationId == locationId && p.ItemId == itemId)
                .Execute();
            if (result == 0)
            {
                //没更新到，插入一条
                var storage = new ItemStorage
                {
                    ItemId = itemId,
                    Qty = qty,
                    StorageLocationId = locationId
                };
                RF.Save(storage);
            }

            OnStorageChanged(locationId, itemId);
            CheckStorageSafty(locationId, itemId);
        }

        /// <summary>
        /// 物料校准，更新货位物料库存
        /// </summary>
        /// <param name="locationId">货位ID</param>
        /// <param name="areId">货区ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="qty">物料数量</param>
        /// <exception cref="EntityNotFoundException">找不到产线获取货位</exception>
        public virtual void UpdateItemStorageQty(double locationId, double areId, double itemId, decimal qty)
        {
            var location = Query<StorageLocation>()
                .Where(p => p.Id == locationId && p.StorageAreaId == areId)
                .FirstOrDefault();
            if (location == null)
                throw new EntityNotFoundException("找不到产线获取货位".L10N());
            var result = DB.Update<ItemStorage>()
           .Set(p => p.Qty, p => p.Qty + qty)
           .Where(p => p.StorageLocationId == locationId && p.ItemId == itemId)
           .Execute();
            if (result == 0 && qty > 0)
            {
                var storage = new ItemStorage
                {
                    ItemId = itemId,
                    Qty = qty,
                    StorageLocationId = locationId
                };
                RF.Save(storage);
            }
        }

        /// <summary>
        /// 获取产线获取货位
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns>产线货区货位</returns>
        public virtual StorageLocation GetStorageLocation(string code)
        {
            return Query<StorageLocation>().Where(p => p.Code == code).FirstOrDefault();
        }

        /// <summary>
        /// 保存产线物料货位属性值
        /// </summary>
        /// <param name="detailList">产线物料货位属性值列表</param>
        /// <param name="detailId">产线物料货位Id</param>
        public virtual void SaveItemStoragePropertys(List<ItemStoragePropertyValue> detailList, double detailId)
        {
            EntityList<ItemStoragePropertyValue> propertyValueList = new EntityList<ItemStoragePropertyValue>();
            var orgPropertyValues = Query<ItemStoragePropertyValue>().Where(p => p.ItemStorageId == detailId).ToList();
            var existedPropertyIds = new List<double>();
            foreach (var selectedValue in detailList)
            {
                bool flag = false;
                foreach (var orgValue in orgPropertyValues)
                {
                    if (selectedValue.DefinitionId == orgValue.DefinitionId && selectedValue.Value == orgValue.Value)
                    {
                        flag = true;
                        if (!existedPropertyIds.Contains(orgValue.Id))
                            existedPropertyIds.Add(orgValue.Id);
                        break;
                    }
                }
                if (!flag)
                {
                    var newPropertyValue = new ItemStoragePropertyValue()
                    {
                        ItemStorageId = detailId,
                        DefinitionId = selectedValue.DefinitionId,
                        Value = selectedValue.Value,
                        PersistenceStatus = PersistenceStatus.New
                    };
                    propertyValueList.Add(newPropertyValue);
                }
            }
            var deletedPropertyValues = orgPropertyValues.Where(p => !existedPropertyIds.Contains(p.Id));
            deletedPropertyValues.ForEach(p =>
            {
                p.PersistenceStatus = PersistenceStatus.Deleted;
                propertyValueList.Add(p);
            });
            RF.Save(propertyValueList);
        }

        /// <summary>
        /// 保存产线库存属性值
        /// </summary>
        /// <param name="detailList">产线库存属性值列表</param>
        /// <param name="detailId">物料库存Id</param>
        public virtual void SaveStorageSaftyPropertys(List<StorageSaftyPropertyValue> detailList, double detailId)
        {
            EntityList<StorageSaftyPropertyValue> propertyValueList = new EntityList<StorageSaftyPropertyValue>();
            var orgPropertyValues = Query<StorageSaftyPropertyValue>().Where(p => p.StorageSaftyId == detailId).ToList();
            var existedPropertyIds = new List<double>();
            foreach (var selectedValue in detailList)
            {
                bool flag = false;
                foreach (var orgValue in orgPropertyValues)
                {
                    if (selectedValue.DefinitionId == orgValue.DefinitionId && selectedValue.Value == orgValue.Value)
                    {
                        flag = true;
                        if (!existedPropertyIds.Contains(orgValue.Id))
                            existedPropertyIds.Add(orgValue.Id);
                        break;
                    }
                }
                if (!flag)
                {
                    var newPropertyValue = new StorageSaftyPropertyValue()
                    {
                        StorageSaftyId = detailId,
                        DefinitionId = selectedValue.DefinitionId,
                        Value = selectedValue.Value,
                        PersistenceStatus = PersistenceStatus.New
                    };
                    propertyValueList.Add(newPropertyValue);
                }
            }
            var deletedPropertyValues = orgPropertyValues.Where(p => !existedPropertyIds.Contains(p.Id));
            deletedPropertyValues.ForEach(p =>
            {
                p.PersistenceStatus = PersistenceStatus.Deleted;
                propertyValueList.Add(p);
            });
            RF.Save(propertyValueList);
        }

        /// <summary>
        /// 获取产线物料货位属性值
        /// </summary>
        /// <param name="detailId">产线物料货位Id</param>
        /// <param name="itemId">物料id</param>
        /// <returns>产线物料货位属性值列表</returns>
        public virtual EntityList<ItemStoragePropertyValue> GetItemStoragePropertyList(double detailId, double itemId)
        {
            var selectedPropertyValues = Query<ItemStoragePropertyValue>().Where(p => p.ItemStorageId == detailId).ToList();
            var itemPropValueList = RT.Service.Resolve<ItemController>().GetItemPropertys(itemId);
            var prodBomDetailValues = new EntityList<ItemStoragePropertyValue>();
            itemPropValueList.ForEach(p =>
            {
                var prodBomDetailValue = new ItemStoragePropertyValue
                {
                    DefinitionId = p.DefinitionId,
                    Definition = p.Definition,
                    Value = p.Value
                };
                prodBomDetailValues.Add(prodBomDetailValue);
            });
            prodBomDetailValues.SetTotalCount(itemPropValueList.Count);
            foreach (var selectPropertyValue in prodBomDetailValues)
            {
                foreach (var selectedPropertyValue in selectedPropertyValues)
                {
                    if (selectPropertyValue.DefinitionId == selectedPropertyValue.DefinitionId && selectPropertyValue.Value == selectedPropertyValue.Value)
                    {
                        selectPropertyValue.Id = selectedPropertyValue.Id;
                    }
                }
                selectPropertyValue.ItemStorageId = detailId;
            }
            return prodBomDetailValues;
        }

        /// <summary>
        /// 获取产线库存属性值
        /// </summary>
        /// <param name="detailId">物料库存Id</param>
        /// <param name="itemId">物料id</param>
        /// <returns>产线库存属性值列表</returns>
        public virtual EntityList<StorageSaftyPropertyValue> GetStorageAreaPropertyList(double detailId, double itemId)
        {
            var selectedPropertyValues = Query<StorageSaftyPropertyValue>().Where(p => p.StorageSaftyId == detailId).ToList();
            var itemPropValueList = RT.Service.Resolve<ItemController>().GetItemPropertys(itemId);
            var prodBomDetailValues = new EntityList<StorageSaftyPropertyValue>();
            itemPropValueList.ForEach(p =>
            {
                var prodBomDetailValue = new StorageSaftyPropertyValue
                {
                    DefinitionId = p.DefinitionId,
                    Definition = p.Definition,
                    Value = p.Value
                };
                prodBomDetailValues.Add(prodBomDetailValue);
            });
            prodBomDetailValues.SetTotalCount(itemPropValueList.Count);
            foreach (var selectPropertyValue in prodBomDetailValues)
            {
                foreach (var selectedPropertyValue in selectedPropertyValues)
                {
                    if (selectPropertyValue.DefinitionId == selectedPropertyValue.DefinitionId && selectPropertyValue.Value == selectedPropertyValue.Value)
                    {
                        selectPropertyValue.Id = selectedPropertyValue.Id;
                    }
                }
                selectPropertyValue.StorageSaftyId = detailId;
            }
            return prodBomDetailValues;
        }

        /// <summary>
        /// 对应工位货区是否不能存放多个物料
        /// </summary>
        /// <param name="storageLocationId">货位id</param>
        /// <param name="itemId">物料id</param>
        /// <returns>不能存放返回true，能返回false</returns>
        public virtual bool IsStorageMixItem(double storageLocationId, double itemId)
        {
            var storageLocation = RF.GetById<StorageLocation>(storageLocationId);
            if (storageLocation == null)
                return false;
            if (storageLocation.StorageArea.IsMixItem)
                return false;
            else
            {
                return Query<ItemStorage>()
                    .Join<Item>((x, y) => x.ItemId == y.Id)
                    .Where<Item>((x, y) => x.StorageLocationId == storageLocationId 
                            && x.ItemId != itemId 
                            && y.ConsumeMode == ConsumeMode.Pull)
                    .Count() != 0;
            }
        }

        /// <summary>
        /// 货区下是否有多个物料，不能取消混放
        /// </summary>
        /// <param name="storageAreaId">货区id</param>
        /// <returns>不能取消混放返回true，能返回false</returns>
        public virtual bool IsNoUpdateMixItem(double storageAreaId)
        {
            var storageLocationIds = Query<StorageLocation>().Select(p => p.Id).Where(p => p.StorageAreaId == storageAreaId).ToList<double>();
            foreach (var locationId in storageLocationIds)
            {
                if (Query<ItemStorage>().Where(p => p.StorageLocationId == locationId).Count() > 1)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 获取需要叫料的产线物料货位
        /// </summary>
        /// <returns>产线物料货位列表</returns>
        public virtual EntityList<ItemStorage> GetNeedCallMaterialItemStorage()
        {
            return Query<ItemStorage>().Where(p => p.Qty < p.SafetyQty && p.StorageLocation.StorageArea.State == State.Enable).ToList();
        }

        /// <summary>
        /// 根据货区货位物料获取物料库存
        /// </summary>
        /// <param name="storageAreaId">货区id</param>
        /// <param name="locationId">货位id</param>
        /// <param name="itemId">物料id</param>
        /// <returns>物料库存</returns>
        public virtual StorageSafty GetStorageSaftyByLocationItem(double storageAreaId, double locationId, double itemId)
        {
            return Query<StorageSafty>().Where(p => p.StorageAreaId == storageAreaId && p.StorageLocationId == locationId && p.ItemId == itemId).FirstOrDefault();
        }

        /// <summary>
        /// 获取货区下的一个产线工位
        /// </summary>
        /// <param name="storageAreaId">货区id</param>
        /// <returns>一个产线工位</returns>
        public virtual StationStorageArea GetStationByLocation(double storageAreaId)
        {
            return Query<StationStorageArea>().Where(p => p.StorageAreaId == storageAreaId).FirstOrDefault();
        }
    }
}