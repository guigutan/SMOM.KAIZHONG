using DocumentFormat.OpenXml.EMMA;
using SIE.Core.Common;
using SIE.Core.Enums;
using SIE.Data;
using SIE.Data.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.WMS.Inventory;
using SIE.Inventory.Commom;
using SIE.Inventory.TransactionProcessing;
using SIE.Items;
using SIE.MetaModel;
using SIE.Rbac.InvOrgs;
using SIE.Utils;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 库位库存控制器
    /// </summary>
    public partial class InvOnhandController : DomainController
    {
        /// <summary>
        /// 检查是否有库存
        /// </summary>
        /// <param name="lpnList">码盘列表</param>
        /// <returns>bool</returns>
        public virtual bool CheckHasOnhand(List<string> lpnList)
        {
            var exp = lpnList.Distinct().ToList().CreateContainsExpression<LotLpnOnhand>("x", nameof(LotLpnOnhand.Lpn));
            if (exp == null)
            {
                return false;
            }

            return Query<LotLpnOnhand>().Where(p => p.Qty > 0).Where(exp).Count() > 0;
        }

        /// <summary>
        /// 检查是否有库存
        /// </summary>
        /// <param name="lpnList">码盘列表</param>
        /// <returns>bool</returns>
        public virtual List<string> CheckAndReturnHasOnhand(List<string> lpnList)
        {
            var exp = lpnList.Distinct().ToList().CreateContainsExpression<LotLpnOnhand>("x", nameof(LotLpnOnhand.Lpn));
            if (exp == null)
            {
                return new List<string>();
            }

            return Query<LotLpnOnhand>().Where(p => p.Qty > 0).Where(exp).Select(p => p.Lpn).ToList<string>().ToList();
        }

        /// <summary>
        /// 获取LPN库位库存行数
        /// </summary>
        /// <param name="queryAction">查询条件委托</param>
        /// <returns>库位库存行数</returns>
        public virtual int GetLotLpnOnhandCount(Action<IEntityQueryer<LotLpnOnhand>> queryAction)
        {
            var query = Query<LotLpnOnhand>();
            queryAction?.Invoke(query);
            return query.Count();
        }

        /// <summary>
        /// 查询库位的LPN数
        /// </summary>
        /// <param name="locId">库位ID</param>
        /// <returns>LPN数</returns>
        public virtual int GetLotLpnOnhandCount(double locId)
        {
            var onhands = Query<LotLpnOnhand>().Where(p => p.StorageLocationId == locId && p.Qty > 0).Select(p => p.Lpn).Distinct().ToList<string>();
            var count = onhands.Count();

            return count;
        }

        /// <summary>
        /// 根据获取批次和LPN库存行数
        /// </summary>
        /// <param name="lpn">LPN</param>
        /// <param name="isFilterZero">过滤零库存</param>
        /// <returns>批次和LPN库存</returns>
        public virtual int GetLotLpnOnhandCount(string lpn, bool isFilterZero = true)
        {
            var query = Query<LotLpnOnhand>();
            query.Where(p => p.Lpn == lpn);
            if (isFilterZero)
                query.Where(p => p.Qty > 0);
            return query.Count();
        }

        /// <summary>
        /// 获取库存信息
        /// </summary>
        /// <param name="assignParamList">分配参数</param>
        /// <param name="isValidateState">是否验证库存状态</param>
        /// <returns>批次和LPN库存</returns>
        public virtual List<LotLpnOnhand> GetLotLpnOnhand(List<AssignParam> assignParamList, bool isValidateState = true)
        {
            List<double> itemIdList = assignParamList.Select(p => p.ItemId).ToList();
            List<double> warehouseIdList = assignParamList.Select(p => p.WarehouseId).ToList();

            var query = Query<LotLpnOnhand>()
                .Join<StorageLocationOperation>((a, b) => a.StorageLocationId == b.StorageLocationId && b.IsPick)
                .Where(p => itemIdList.Contains(p.ItemId) && warehouseIdList.Contains(p.WarehouseId) && p.AvailableQty > 0);

            query.Where(p => p.State != OnhandState.None);
            if (isValidateState)
                query.Where(p => p.State != OnhandState.Ng);

            EntityList<LotLpnOnhand> lotLpnOnhandEList = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            List<LotLpnOnhand> lotLpnOnhandList = lotLpnOnhandEList.Where(p =>
            assignParamList.Any(q =>
            {
                if (q.ItemId != p.ItemId || q.WarehouseId != p.WarehouseId) return false;

                if (q.AppointLotId.HasValue && p.LotId != q.AppointLotId) return false;

                if (!string.IsNullOrEmpty(q.AppointLpn) && p.Lpn != q.AppointLpn) return false;

                if (q.AppointStorageLocationId.HasValue && q.AppointStorageLocationId.Value != p.StorageLocationId) return false;

                if (q.AppointStorageAreaId.HasValue && q.AppointStorageAreaId.Value != p.StorageAreaId) return false;

                if (q.StorerCode != p.StorerCode) return false;

                if (q.ProjectNo != p.ProjectNo) return false;

                if (q.TaskNo != p.TaskNo) return false;

                return true;
            })
            ).ToList();

            return lotLpnOnhandList.OrderBy(p => p.Lot.LotAtt03).ToList();
        }

        /// <summary>
        /// 根据获取批次和LPN库存
        /// </summary>
        /// <param name="lpn">LPN</param>
        /// <param name="isFilterZero">过滤零库存</param>
        /// <returns>批次和LPN库存</returns>
        public virtual LotLpnOnhand GetLotLpnOnhand(string lpn, bool isFilterZero = true)
        {
            var query = Query<LotLpnOnhand>();
            query.Where(p => p.Lpn == lpn);
            if (isFilterZero)
                query.Where(p => p.Qty > 0);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据库位获取首笔库存
        /// </summary>
        /// <param name="locId">库位Id</param>
        /// <returns>批次和LPN库存</returns>
        public virtual LotLpnOnhand GetLotLpnOnhand(double locId)
        {
            var query = Query<LotLpnOnhand>();
            query.Where(p => p.StorageLocationId == locId);
            query.Where(p => p.Qty > 0);
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 获取LPN库存
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="areaId">库区ID</param>
        /// <param name="locId">库位ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="lotId">批次ID</param>
        /// <param name="storerCode">货主</param>
        /// <param name="lpn">lpn</param>
        /// <param name="projectNo">项目号</param>
        /// <param name="taskNo">任务号</param>
        /// <param name="state">库存状态</param>
        /// <returns>库存现有量总和</returns>
        public virtual decimal GetLotLpnOnhand(double? warehouseId, double? areaId, double? locId, double? itemId, double? lotId,
            string storerCode, string lpn, string projectNo, string taskNo, OnhandState? state)
        {
            var query = Query<LotLpnOnhand>();
            if (warehouseId.HasValue)
                query.Where(p => p.WarehouseId == warehouseId);
            if (areaId.HasValue)
                query.Where(p => p.StorageAreaId == areaId);
            if (locId.HasValue)
                query.Where(p => p.StorageLocationId == locId);
            if (itemId.HasValue)
                query.Where(p => p.ItemId == itemId);
            if (lotId.HasValue)
                query.Where(p => p.LotId == lotId);
            if (!lpn.IsNullOrEmpty())
                query.Where(p => p.Lpn == lpn);
            if (!storerCode.IsNullOrEmpty())
                query.Where(p => p.StorerCode == storerCode);
            if (!projectNo.IsNullOrEmpty())
                query.Where(p => p.ProjectNo == projectNo);
            if (!taskNo.IsNullOrEmpty())
                query.Where(p => p.TaskNo == taskNo);
            if (state.HasValue)
                query.Where(p => p.State == state.Value);
            return query.ToList().Sum(p => p.Qty);
        }

        /// <summary>
        /// 获取批次LPN库存信息
        /// </summary>
        /// <param name="lotLpnOnhandIdList">批次LPN库存Id</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>批次LPN库存信息</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhand(List<double> lotLpnOnhandIdList, EagerLoadOptions elo = null)
        {
            if (lotLpnOnhandIdList == null || lotLpnOnhandIdList.Count == 0)
            {
                return new EntityList<LotLpnOnhand>();
            }
            EntityList<LotLpnOnhand> LotLpnOnhands = new EntityList<LotLpnOnhand>();
            if (elo == null)
                elo = new EagerLoadOptions().LoadWithViewProperty();
            DataProcessEx.SplitDataExecute(lotLpnOnhandIdList, sons =>
            {
                var customersList = Query<LotLpnOnhand>().Where(p => sons.Contains(p.Id)).ToList(null, elo);
                LotLpnOnhands.AddRange(customersList);
            });
            return LotLpnOnhands;
        }

        /// <summary>
        /// 获取批次和LPN库存（包含0库存）
        /// </summary>
        /// <param name="locId">库位</param>
        /// <param name="itemId">物料</param>
        /// <param name="lotCode">批次</param>
        /// <param name="param">库存可选参数，如库存管控，则需要传入</param>
        /// <returns>批次和LPN库存</returns>
        public virtual LotLpnOnhand GetLotLpnOnhand(double locId, double itemId, string lotCode, InvOptionalParam param)
        {
            var query = Query<LotLpnOnhand>();
            query.Where(p => p.StorageLocationId == locId && p.ItemId == itemId);
            query.Where(p => p.LotCode == lotCode);
            query.Where(p => p.Lpn == param.Lpn);
            query.Where(p => p.StorerCode == param.StorerCode);
            query.Where(p => p.ProjectNo == param.ProjectNo);
            query.Where(p => p.TaskNo == param.TaskNo);
            query.Where(p => p.State == param.State);
            if (!param.IsIgnoreItemExtProp && !param.ItemExtProp.IsNullOrEmpty())
                query.Where(p => p.ItemExtProp == param.ItemExtProp);
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 根据库位、物料、批次、lpn获取库存
        /// </summary>
        /// <param name="wareHouseId">仓库Id</param>
        /// <param name="locationId">库位Id</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="lotCode">批次Id</param>
        /// <param name="lpn">Lpn</param>
        /// <returns>返回库存信息</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhandListForFrozen(double wareHouseId, double locationId, double itemId, string lotCode, string lpn)
        {
            var query = Query<LotLpnOnhand>();
            query.Where(p => p.WarehouseId == wareHouseId && p.StorageLocationId == locationId &&
                                  p.ItemId == itemId && p.LotCode == lotCode && p.Lpn.Contains(lpn) && p.AvailableQty > 0);

            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取库存信息
        /// </summary>
        /// <param name="warehouseId">分配参数</param>
        /// <param name="onhandDataList">是否验证库存状态</param>
        /// <returns>批次和LPN库存</returns>
        public virtual List<LotLpnOnhand> GetLotLpnOnhandByRID(double warehouseId, List<OnhandData> onhandDataList)
        {
            List<double> itemIdList = onhandDataList.Select(p => p.ItemId).Distinct().ToList();

            var query = Query<LotLpnOnhand>().Where(p => itemIdList.Contains(p.ItemId) && p.WarehouseId == warehouseId && p.AvailableQty > 0);

            EntityList<LotLpnOnhand> lotLpnOnhandList = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            List<LotLpnOnhand> onhands = lotLpnOnhandList.Where(p =>
            onhandDataList.Any(q =>
            {
                if (q.ItemId != p.ItemId) return false;

                if (q.LocationId != p.StorageLocationId) return false;

                if (q.LotCode != p.LotCode) return false;

                if (q.Param.Lpn != p.Lpn) return false;

                if (q.Param.StorerCode != p.StorerCode) return false;

                if (q.Param.ProjectNo != p.ProjectNo) return false;

                if (q.Param.TaskNo != p.TaskNo) return false;

                if (q.Param.State != p.State) return false;

                if (q.Param.ItemExtProp != p.ItemExtProp) return false;

                return true;
            })
            ).ToList();

            return onhands.ToList();
        }

        /// <summary>
        /// 获取库存信息
        /// </summary>
        /// <param name="assignParamList">分配参数</param>
        /// <param name="isLayIn">是否存储</param>
        /// <param name="isAllowGroup">是否允许人工上架</param>
        /// <param name="matchParam">是否匹配参数</param>
        /// <returns>批次和LPN库存</returns>
        public virtual List<LotLpnOnhand> GetAssignLotLpnOnhand(List<AssignParam> assignParamList, bool? isLayIn = null, bool? isAllowGroup = null, bool matchParam = true)
        {
            if (assignParamList is null)
            {
                throw new ArgumentNullException(nameof(assignParamList));
            }

            List<double> itemIdList = assignParamList.Select(p => p.ItemId).ToList();
            List<double> warehouseIdList = assignParamList.Select(p => p.WarehouseId).ToList();

            var query = Query<LotLpnOnhand>().Where(f => f.State != OnhandState.None);

            if (isLayIn == null)
            {//拣货库位，目前大部分分配的逻辑都是这个
                query.Join<StorageLocation>((x, y) => x.StorageLocationId == y.Id && !y.IsFrozen)
                .Join<StorageLocationOperation>((a, b) => a.StorageLocationId == b.StorageLocationId && b.IsPick)
                .Where(p => itemIdList.Contains(p.ItemId) && warehouseIdList.Contains(p.WarehouseId) && p.AvailableQty > 0);
            }
            else if (isLayIn.Value)
            {//要求存储库位，补货的逻辑用到
                query.Join<StorageLocation>((x, y) => x.StorageLocationId == y.Id && !y.IsFrozen)
                .Join<StorageLocationOperation>((a, b) => a.StorageLocationId == b.StorageLocationId && b.IsLayIn && !b.IsPick)
                .Where(p => itemIdList.Contains(p.ItemId) && warehouseIdList.Contains(p.WarehouseId) && p.AvailableQty > 0);
            }
            else
            {//isLayIn=false，对库位存储拣货没有要求，目前库内加工、不同仓库间补货是这种
                query.Join<StorageLocation>((x, y) => x.StorageLocationId == y.Id && !y.IsFrozen)
               .Join<StorageLocationOperation>((a, b) => a.StorageLocationId == b.StorageLocationId)
               .Where(p => itemIdList.Contains(p.ItemId) && warehouseIdList.Contains(p.WarehouseId) && p.AvailableQty > 0);
            }
            if (isAllowGroup.HasValue)
            {
                query.Join<StorageArea>((x, y) => x.StorageAreaId == y.Id && y.IsAllowManualGrounding == isAllowGroup.Value);
            }
            //立库增加的筛选条件
            query.Where<StorageLocation>((x, y) => !y.IsOutLock && !y.IsCountLock && !y.IsBackup);

            EntityList<LotLpnOnhand> lotLpnOnhandEList = query.Distinct().ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(LotLpnOnhand.LotProperty));
            if (matchParam)
            {
                var paparm = assignParamList.First();
                List<LotLpnOnhand> lotLpnOnhandList = lotLpnOnhandEList.Where(d => d.ItemId == paparm.ItemId && d.WarehouseId == paparm.WarehouseId
              //&& (paparm.OrderType == OrderType.SupplierReturn && d.State == OnhandState.Ng || paparm.OrderType != OrderType.SupplierReturn && d.State == OnhandState.Ok)修改为从分配规则明细获取库存状态
              && paparm.StorerCode == d.StorerCode && (d.TaskNo == "*" || paparm.TaskNo == d.TaskNo) && (d.ProjectNo == "*" || paparm.ProjectNo == d.ProjectNo)
              && (paparm.InvItemExtProp == d.ItemExtProp || paparm.IsIgnoreItemExtProp)).ToList();
                return lotLpnOnhandList.ToList();
            }
            else
                return lotLpnOnhandEList.ToList();
        }

        /// <summary>
        /// 匹配扩展属性参数
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public virtual bool MatchAssignParam(LotLpnOnhand p, AssignParam q)
        {
            if (q.ItemId != p.ItemId || q.WarehouseId != p.WarehouseId)
                return false;

            if (q.OrderType.HasValue && q.OrderType.Value == OrderType.SupplierReturn && p.State == OnhandState.None)
                return false;

            if (q.OrderType.HasValue && q.OrderType.Value != OrderType.SupplierReturn && p.State != OnhandState.Ok)
                return false;
            if (q.StorerCode.IsNotEmpty() && q.StorerCode != p.StorerCode)
                return false;
            if (q.ProjectNo.IsNotEmpty())
            {
                if (q.ProjectNo == "*" && p.ProjectNo != "*")
                    return false;//指定*只能匹配* 
                if (q.ProjectNo != "*" && p.ProjectNo != q.ProjectNo && p.ProjectNo != "*")
                    return false;//没有指定*，可以匹配*+匹配值 by guorui 2023.6.30 补货计划发运单

            }
            if (q.TaskNo.IsNotEmpty())
            {
                if (q.TaskNo == "*" && p.TaskNo != "*")
                    return false;//指定*只能匹配*
                if (q.TaskNo != "*" && p.TaskNo != q.TaskNo && p.TaskNo != "*")
                    return false;//没有指定*，可以匹配*+匹配值
            }
            if (!q.IsIgnoreItemExtProp && q.InvItemExtProp != p.ItemExtProp)
                return false;

            return true;
        }

        /// <summary>
        /// 获取库存数据
        /// </summary>
        /// <param name="idList">Id列表</param>
        /// <param name="type">拣货处理</param>
        /// <returns></returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhandData(List<double> idList, int type)
        {
            //var exp = idList.CreateContainsExpression<LotLpnOnhand>("x", nameof(LotLpnOnhand.Id));
            //if (exp == null)
            //    return new EntityList<LotLpnOnhand>();

            //return Query<LotLpnOnhand>().Join<StorageLocationOperation>((a, b) => a.StorageLocationId == b.StorageLocationId && (int)b.PickProcess == type).
            //                     Where(p => p.Qty > 0).Where(exp).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (idList == null || idList.Count == 0)
            {
                return new EntityList<LotLpnOnhand>();
            }
            EntityList<LotLpnOnhand> LotLpnOnhands = new EntityList<LotLpnOnhand>();
            DataProcessEx.SplitDataExecute(idList, sons =>
            {
                var LotLpnOnhandsList = Query<LotLpnOnhand>().Join<StorageLocationOperation>((a, b) => a.StorageLocationId == b.StorageLocationId && (int)b.PickProcess == type).
                Where(p => p.Qty > 0).Where(i => sons.Contains(i.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                LotLpnOnhands.AddRange(LotLpnOnhandsList);
            });
            return LotLpnOnhands;
        }

        /// <summary>
        /// 获取库存数据
        /// </summary>
        /// <param name="lpn">lpn</param>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>库存数据</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhandData(string lpn, double warehouseId, PagingInfo pagingInfo)
        {
            return Query<LotLpnOnhand>().Where(p => p.AvailableQty > 0 && p.Qty == p.AvailableQty && p.Lpn == lpn && p.WarehouseId == warehouseId && p.State != OnhandState.None).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取库存信息
        /// </summary>
        /// <param name="onhandParams">库存参数</param>      
        /// <param name="isAppoint">指定项目号任务号</param>
        /// <returns>库存信息</returns>
        public virtual List<LotLpnOnhandData> GetLotLpnOnhandData(List<OnhandParam> onhandParams, bool isAppoint = true)
        {
            var ctl = RT.Service.Resolve<InvOnhandController>();
            List<LotLpnOnhandData> results = new List<LotLpnOnhandData>();
            foreach (var param in onhandParams)
            {
                ValidateOnhandParam(param);

                OnhandState? onhandState = null;
                if (param.OnhandState != null)
                {
                    onhandState = (OnhandState)EnumViewModel.LabelToEnum(param.OnhandState, typeof(OnhandState));
                    if (onhandState == OnhandState.None)
                        continue;
                }
                var onhandList = ctl.GetLotLpnOnhands(param.WarehouseId, query =>
                {
                    if (param.LocId > 0)
                        //query.Where(p => p.StorageLocationId == param.LocId);
                        query.Join<StorageLocation>((x, y) => x.StorageLocationId == y.Id && x.StorageLocationId == param.LocId && y.IsPick == true);
                    if (param.ItemId > 0)
                        query.Where(p => p.ItemId == param.ItemId);
                    if (!param.Lot.IsNullOrWhiteSpace())
                        query.Join<Lot>((x, y) => x.LotId == y.Id && y.Code == param.Lot);
                    if (param.Lpn.IsNotEmpty())
                        query.Where(p => p.Lpn == param.Lpn);
                    if (param.StorerCode.IsNotEmpty())
                        query.Where(p => p.StorerCode == param.StorerCode);
                    if (param.ProjectNo.IsNotEmpty())
                    {
                        if (isAppoint)
                            query.Where(p => p.ProjectNo == param.ProjectNo);
                        else
                            query.Where(p => p.ProjectNo == param.ProjectNo || p.ProjectNo == "*");
                    }
                    if (param.TaskNo.IsNotEmpty())
                    {
                        if (isAppoint)
                            query.Where(p => p.TaskNo == param.TaskNo);
                        else
                            query.Where(p => p.TaskNo == param.TaskNo || p.TaskNo == "*");
                    }

                    if (onhandState.HasValue)
                        query.Where(p => p.State == onhandState.Value);
                    if (!param.ItemExtProp.IsNullOrWhiteSpace())
                        query.Where(p => p.ItemExtProp == param.ItemExtProp);
                }, true);
                foreach (LotLpnOnhand onhand in onhandList)
                {
                    LotLpnOnhandData data = new LotLpnOnhandData();
                    data.Id = onhand.Id;
                    data.LocationId = onhand.StorageLocationId;
                    data.LocationCode = onhand.StorageLocation.Code;
                    data.LocationName = onhand.StorageLocation.Name;
                    data.LPN = onhand.Lpn;
                    data.Qty = onhand.Qty;
                    data.AvailableQty = onhand.AvailableQty;
                    data.AllottedQty = onhand.AllottedQty;
                    data.StorerCode = onhand.StorerCode;
                    data.ProjectNo = onhand.ProjectNo;
                    data.TaskNo = onhand.TaskNo;
                    data.OnhandState = onhand.State.ToLabel();
                    data.Lot = onhand.LotCode;
                    data.ItemExtProp = onhand.ItemExtProp;
                    data.ItemExtPropName = onhand.ItemExtPropName;
                    data.AreaId = onhand.StorageAreaId;
                    data.LotId = onhand.LotId;
                    data.ItemId = onhand.ItemId;
                    data.LotAtt01 = onhand.LotAtt01;
                    data.LotAtt02 = onhand.LotAtt02;
                    data.LotAtt03 = onhand.LotAtt03;
                    data.LotAtt04 = onhand.LotAtt04;
                    data.LotAtt05 = onhand.LotAtt05;
                    data.LotAtt06 = onhand.LotAtt06;
                    data.LotAtt07 = onhand.LotAtt07;
                    data.LotAtt08 = onhand.LotAtt08;
                    data.LotAtt09 = onhand.LotAtt09;
                    data.LotAtt10 = onhand.LotAtt10;
                    data.LotAtt11 = onhand.LotAtt11;
                    data.LotAtt12 = onhand.LotAtt12;
                    results.Add(data);
                }
            }

            return results;
        }

        /// <summary>
        /// 根据id集合获取库存
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public virtual EntityList<LotLpnOnhand> GetSortLotLpnOnhand(List<double> idList)
        {
            if (idList == null || idList.Count == 0)
            {
                return new EntityList<LotLpnOnhand>();
            }
            return idList.SplitContains(ids =>
            {
                return Query<LotLpnOnhand>().Join<StorageLocationOperation>((a, b) => a.StorageLocationId == b.StorageLocationId).
                                     Where(p => p.Qty > 0).Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取批次和LPN的库存（可用量）
        /// </summary>
        /// <param name="criteria">查询实体.</param>
        /// <returns>批次和LPN库存</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhands(LotLpnOnhandCriteria criteria)
        {
            var q = Query<LotLpnOnhand>();

            ////增加仓库权限关联查询
            RT.Service.Resolve<WarehouseController>().ExistWarehouseEmplyee(q, LotLpnOnhand.WarehouseIdProperty);

            if (criteria.Item != null)
                q.Where(p => p.ItemId == criteria.ItemId);
            if (criteria.WarehouseId != null)
                q.Where(p => p.WarehouseId == criteria.WarehouseId);
            if (criteria.StorageLocation != null)
                q.Where(p => p.StorageLocationId == criteria.StorageLocationId);
            if (criteria.StorerCode.IsNotEmpty())
                q.Where(p => p.StorerCode.Contains(criteria.StorerCode));
            if (criteria.Lot.IsNotEmpty())
                q.Where(p => p.LotCode.Contains(criteria.Lot));
            if (criteria.Lpn.IsNotEmpty())
                q.Where(p => p.Lpn.Contains(criteria.Lpn));
            if (criteria.ProjectNo.IsNotEmpty())
                q.Where(p => p.ProjectNo.Contains(criteria.ProjectNo));
            if (criteria.TaskNo.IsNotEmpty())
                q.Where(p => p.TaskNo.Contains(criteria.TaskNo));
            if (!criteria.IsZero)
                q.Where(p => p.Qty > 0);
            if (criteria.IsFrozen.HasValue)
                q.Where(p => p.StorageLocation.IsFrozen == criteria.IsFrozen);
            if (criteria.State.HasValue)
                q.Where(p => p.State == criteria.State);
            if (criteria.IsExistsAvailableQty.HasValue)
                q.Where(p => p.AvailableQty > 0);
            if (criteria.IsNotEmptyStorer.HasValue)
                q.Where(p => p.StorerCode != "*");
            q.Where(p => p.State != OnhandState.None);
            if (!string.IsNullOrEmpty(criteria.ItemCode))
            {
                q.Where(p => p.Item.Code.Contains(criteria.ItemCode) || p.Item.Name.Contains(criteria.ItemCode));
            }
            if (!string.IsNullOrEmpty(criteria.StorageAreaCode))
            {
                q.Where(p => p.StorageArea.Code.Contains(criteria.StorageAreaCode) || p.StorageArea.Name.Contains(criteria.StorageAreaCode));
            }
            if (!string.IsNullOrEmpty(criteria.StorageLocationCode))
            {
                q.Where(p => p.StorageLocation.Code.Contains(criteria.StorageLocationCode) || p.StorageLocation.Name.Contains(criteria.StorageLocationCode));
            }

            q.OrderBy(criteria.OrderInfoList);
            EagerLoadOptions elo = new EagerLoadOptions();
            return q.ToList(criteria.PagingInfo, elo.LoadWithViewProperty());
        }

        /// <summary>
        /// 获取现有量不为零的库存记录
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhands(Action<IEntityQueryer<LotLpnOnhand>> queryAction = null)
        {
            var query = Query<LotLpnOnhand>();
            query.Where(p => p.Qty > 0);
            queryAction?.Invoke(query);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取可用量大于0的批次和LPN库存
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="queryAction">查询条件委托</param>
        /// <param name="isQueryQty">是否查询现有数</param>
        /// <param name="info">分页信息</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>批次和LPN库存</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhands(double warehouseId, Action<IEntityQueryer<LotLpnOnhand>> queryAction, bool? isQueryQty, PagingInfo info = null, EagerLoadOptions elo = null)
        {
            var query = Query<LotLpnOnhand>().Where(p => p.WarehouseId == warehouseId);
            if (isQueryQty == true)
                query.Where(p => p.Qty > 0);
            else
                query.Where(p => p.AvailableQty > 0);

            queryAction?.Invoke(query);

            if (elo == null)
            {
                elo = new EagerLoadOptions().LoadWithViewProperty();
            }
            else
            {
                elo.LoadWithViewProperty();
            }

            return query.ToList(info, elo);
        }

        /// <summary>
        /// 库存调整获取库存数据
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="keywords">关键字查询维度</param>
        /// <param name="isQueryQty">是否查询现有数</param>
        /// <returns>库存数据</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhands(double warehouseId, BaseKeywordData keywords, bool? isQueryQty)
        {
            var query = Query<LotLpnOnhand>().Where(p => p.WarehouseId == warehouseId);
            if (!string.IsNullOrEmpty(keywords.LocationCode))
                query.Join<StorageLocation>((a, b) => a.StorageLocationId == b.Id && b.State == State.Enable && !b.IsFrozen &&
                (b.Code.Contains(keywords.LocationCode) || b.Name.Contains(keywords.LocationCode)));

            if (!string.IsNullOrEmpty(keywords.LpnCode))
                query.Where(p => p.Lpn.Contains(keywords.LpnCode));

            if (!string.IsNullOrEmpty(keywords.StorerCode))
                query.Where(p => p.StorerCode.Contains(keywords.StorerCode));

            if (!string.IsNullOrEmpty(keywords.ProjectNo))
                query.Where(p => p.ProjectNo.Contains(keywords.ProjectNo));

            if (!string.IsNullOrEmpty(keywords.TaskNo))
                query.Where(p => p.TaskNo.Contains(keywords.TaskNo));

            if (!string.IsNullOrEmpty(keywords.ItemCode))
                query.Join<Item>((a, i) => a.ItemId == i.Id && (i.Code.Contains(keywords.ItemCode) || i.Name.Contains(keywords.ItemCode)));

            if (!string.IsNullOrEmpty(keywords.LotCode))
                query.Where(p => p.LotCode.Contains(keywords.LotCode));

            query.Where(p => p.ItemExtPropName.Contains(keywords.ItemExtPropName));

            if (isQueryQty.HasValue && isQueryQty.Value)
                query.Where(p => p.Qty > 0);
            else
                query.Where(p => p.AvailableQty > 0);

            query.Where(p => p.State == keywords.State);

            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取LPN库存
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="areaId">库区ID</param>
        /// <param name="locId">库位ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="lotId">批次ID</param>
        /// <param name="storerCode">货主</param>
        /// <param name="lpn">lpn</param>
        /// <param name="projectNo">项目号</param>
        /// <param name="taskNo">任务号</param>
        /// <param name="isZeroQty">是否排除0库存</param>
        /// <param name="lotCode">批次号</param>
        /// <param name="onhandState">库存状态</param>
        /// <param name="itemExtProp">物料扩展属性</param>  
        /// <param name="itemExtPropName">物料扩展属性</param>
        /// <returns>Lpn库存</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhands(double? warehouseId, double? areaId, double? locId, double? itemId, double? lotId,
            string storerCode, string lpn, string projectNo, string taskNo, bool isZeroQty, OnhandState? onhandState, string itemExtProp = "",
            string lotCode = "", string itemExtPropName = "")
        {
            var query = Query<LotLpnOnhand>();
            if (warehouseId.HasValue)
                query.Where(p => p.WarehouseId == warehouseId);
            if (areaId.HasValue)
                query.Where(p => p.StorageAreaId == areaId);
            if (locId.HasValue)
                query.Where(p => p.StorageLocationId == locId);
            if (itemId.HasValue)
                query.Where(p => p.ItemId == itemId);
            if (lotId.HasValue)
                query.Where(p => p.LotId == lotId);
            else if (!lotCode.IsNullOrWhiteSpace())
            {
                query.Join<Lot>((x, y) => x.LotId == y.Id && y.Code == lotCode);
            }
            if (!lpn.IsNullOrEmpty())
                query.Where(p => p.Lpn == lpn);
            if (!storerCode.IsNullOrEmpty())
                query.Where(p => p.StorerCode == storerCode);
            if (!projectNo.IsNullOrEmpty())
                query.Where(p => p.ProjectNo == projectNo);
            if (!taskNo.IsNullOrEmpty())
                query.Where(p => p.TaskNo == taskNo);
            if (isZeroQty)
                query.Where(p => p.Qty > 0);
            if (onhandState.HasValue)
                query.Where(p => p.State == onhandState.Value);
            if (!itemExtProp.IsNullOrWhiteSpace())
                query.Where(p => p.ItemExtProp == itemExtProp);
            if (!itemExtPropName.IsNullOrWhiteSpace())
                query.Where(p => p.ItemExtPropName == itemExtPropName);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据获取批次和LPN库存
        /// </summary>
        /// <param name="lpn">LPN</param>
        /// <param name="isFilterZero">过滤零库存</param>
        /// <returns>批次和LPN库存</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhands(string lpn, bool isFilterZero = true)
        {
            var query = Query<LotLpnOnhand>();
            query.Where(p => p.Lpn == lpn);
            if (isFilterZero)
            {
                query.Where(p => p.Qty > 0);
            }
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据LPN获取批次和LPN库存
        /// </summary>
        /// <param name="lpnList">LPN集合</param>
        /// <returns></returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnHands(List<string> lpnList)
        {
            return lpnList.Distinct().SplitContains(lpns =>
            {
                return Query<LotLpnOnhand>().Where(p => p.Qty > 0).Where(p => lpns.Contains(p.Lpn)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取LPN可用量大于0的库存集合
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="lpnList">lpn集合</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>LPN可用量大于0的库存集合</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnHands(double warehouseId, List<string> lpnList, EagerLoadOptions elo = null)
        {
            return lpnList.Distinct().SplitContains(lpns =>
            {
                var query = Query<LotLpnOnhand>().Where(p => p.WarehouseId == warehouseId && p.AvailableQty > 0);
                return query.Where(p => lpns.Contains(p.Lpn)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 获取库存信息
        /// </summary>
        /// <param name="wareHouseId">仓库Id</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="locationCode">库位编号</param>
        /// <param name="lpn">LPN</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <param name="pagInfo">分页对象</param>
        /// <returns>返回</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhands(double wareHouseId, double itemId, string locationCode, string lpn, string itemExtProp, PagingInfo pagInfo)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(LotLpnOnhand.StorageLocationProperty);
            var query = Query<LotLpnOnhand>().Where(p => p.WarehouseId == wareHouseId && p.ItemId == itemId && p.ItemExtProp == itemExtProp && p.State != OnhandState.None);
            if (!string.IsNullOrEmpty(lpn))
            {
                query = query.Where(p => p.Lpn.Contains(lpn));
            }

            if (!string.IsNullOrEmpty(locationCode))
            {
                query = query.Join<StorageLocation>((a, b) => a.StorageLocationId == b.Id).Where<StorageLocation>((a, b) => b.Code.Contains(locationCode));
            }

            return query.OrderBy(p => p.Lot.LotAtt03).OrderByDescending(p => p.AvailableQty).ToList(pagInfo, elo);
        }

        /// <summary>
        /// 获取库存信息
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="itemIds">物料ID集合</param>
        /// <param name="storerCodes">货主集合</param>
        /// <param name="areaIds">库区ID</param>
        /// <param name="locIds">库位ID集合</param>
        /// <param name="lpns">LPN集合</param>
        /// <param name="lotIds">批次ID集合</param>
        /// <param name="projects">项目号集合</param>
        /// <param name="taskNos">任务号集合 </param>
        /// <param name="itemExtProps">物料扩展属性集合</param>
        /// <param name="lotCodes">批次号</param>
        /// <returns>包装库存</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhands(double warehouseId, List<double> itemIds, List<string> storerCodes,
            List<double> areaIds, List<double> locIds, List<string> lpns, List<double> lotIds, List<string> projects, List<string> taskNos,
            List<string> itemExtProps, List<string> lotCodes = null)
        {
            var query = Query<LotLpnOnhand>().Where(p => p.AvailableQty > 0);
            if (warehouseId > 0)
                query.Where(p => p.WarehouseId == warehouseId);
            if (itemIds.Any())
                query.Where(p => itemIds.Contains(p.ItemId));
            if (storerCodes.Any())
                query.Where(p => storerCodes.Contains(p.StorerCode));
            if (areaIds.Any())
                query.Where(p => areaIds.Contains(p.StorageAreaId));
            if (locIds.Any())
                query.Where(p => locIds.Contains(p.StorageLocationId));
            if (lpns.Any())
                query.Where(p => lpns.Contains(p.Lpn));
            if (lotIds.Any())
                query.Where(p => lotIds.Contains(p.LotId));
            if (projects.Any())
                query.Where(p => projects.Contains(p.ProjectNo));
            if (taskNos.Any())
                query.Where(p => taskNos.Contains(p.TaskNo));
            if (itemExtProps.Any())
                query.Where(p => itemExtProps.Contains(p.ItemExtProp));
            if (lotCodes != null)
                query.Where(p => lotCodes.Contains(p.LotCode));

            return query.ToList();
        }

        /// <summary>
        /// 获取库存信息
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="assignParams">查询参数</param>
        /// <param name="isAllowManualGrounding">是否允许人工上架</param>
        /// <returns>库存信息</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhands(double warehouseId, List<AssignParam> assignParams, bool? isAllowManualGrounding = null)
        {
            var itemIds = assignParams.Select(p => p.ItemId).Distinct().ToList();
            var storerCodes = assignParams.Select(p => p.StorerCode).Distinct().ToList();
            var areaIds = assignParams.Where(p => p.AppointStorageAreaId > 0).Select(p => (double)p.AppointStorageAreaId).Distinct().ToList();
            var locIds = assignParams.Where(p => p.AppointStorageLocationId > 0).Select(p => (double)p.AppointStorageLocationId).Distinct().ToList();
            var lpns = assignParams.Where(p => !p.AppointLpn.IsNullOrEmpty()).Select(p => p.AppointLpn).Distinct().ToList();
            var lotCodes = assignParams.Where(p => p.AppointLotId > 0).Select(p => p.LotCode).Distinct().ToList();
            var lotIds = assignParams.Where(p => p.AppointLotId > 0).Select(p => p.AppointLotId).Distinct().ToList();
            var projects = assignParams.Select(p => p.ProjectNo).Union(new List<string>() { "*" }).Distinct().ToList();
            var taskNos = assignParams.Select(p => p.TaskNo).Union(new List<string>() { "*" }).Distinct().ToList();
            var itemExtProps = assignParams.Select(p => p.InvItemExtProp).Distinct().ToList();
            var lotAtt01s = assignParams.Where(p => p.LotAtt01 != null).Select(p => p.LotAtt01).Distinct().ToList();
            var lotAtt02s = assignParams.Where(p => p.LotAtt02 != null).Select(p => p.LotAtt02).Distinct().ToList();
            var lotAtt03s = assignParams.Where(p => p.LotAtt03 != null).Select(p => p.LotAtt03).Distinct().ToList();
            var lotAtt04s = assignParams.Where(p => !p.LotAtt04.IsNullOrEmpty()).Select(p => p.LotAtt04).Distinct().ToList();
            var lotAtt05s = assignParams.Where(p => p.LotAtt05 > 0).Select(p => p.LotAtt05).Distinct().ToList();
            var lotAtt06s = assignParams.Where(p => p.LotAtt06 > 0).Select(p => p.LotAtt06).Distinct().ToList();
            var lotAtt07s = assignParams.Where(p => p.LotAtt07.HasValue).Select(p => p.LotAtt07).Distinct().ToList();
            var lotAtt08s = assignParams.Where(p => !p.LotAtt08.IsNullOrEmpty()).Select(p => p.LotAtt08).Distinct().ToList();
            var lotAtt09s = assignParams.Where(p => !p.LotAtt09.IsNullOrEmpty()).Select(p => p.LotAtt09).Distinct().ToList();
            var lotAtt10s = assignParams.Where(p => !p.LotAtt10.IsNullOrEmpty()).Select(p => p.LotAtt10).Distinct().ToList();
            var lotAtt11s = assignParams.Where(p => p.LotAtt11 != null).Select(p => p.LotAtt11).Distinct().ToList();
            var lotAtt12s = assignParams.Where(p => p.LotAtt12 != null).Select(p => p.LotAtt12).Distinct().ToList();

            var query = Query<LotLpnOnhand>()
                .Join<Lot>((p, l) => p.LotId == l.Id);
            query.Where(p => p.AvailableQty > 0);
            if (warehouseId > 0)
                query.Where(p => p.WarehouseId == warehouseId);
            if (itemIds.Any())
                query.Where(p => itemIds.Contains(p.ItemId));
            if (storerCodes.Any())
                query.Where(p => storerCodes.Contains(p.StorerCode));
            if (areaIds.Any())
                query.Where(p => areaIds.Contains(p.StorageAreaId));
            if (locIds.Any())
                query.Where(p => locIds.Contains(p.StorageLocationId));
            if (lpns.Any())
                query.Where(p => lpns.Contains(p.Lpn));
            if (lotIds.Any())
                query.Where(p => lotIds.Contains(p.LotId));
            if (projects.Any())
                query.Where(p => projects.Contains(p.ProjectNo));
            if (taskNos.Any())
                query.Where(p => taskNos.Contains(p.TaskNo));
            if (itemExtProps.Any())
            {
                if (itemExtProps.Any(p => p.IsNullOrEmpty()))
                {
                    query.Where(p => itemExtProps.Contains(p.ItemExtProp) || p.ItemExtProp == null);
                }
                else
                    query.Where(p => itemExtProps.Contains(p.ItemExtProp));
            }
            if (lotCodes.Any())
                query.Where(p => lotCodes.Contains(p.LotCode));
            if (lotAtt01s.Any())
                query.Where<Lot>((p, l) => lotAtt01s.Contains(l.LotAtt01));
            if (lotAtt02s.Any())
                query.Where<Lot>((p, l) => lotAtt02s.Contains(l.LotAtt02));
            if (lotAtt03s.Any())
                query.Where<Lot>((p, l) => lotAtt03s.Contains(l.LotAtt03));
            if (lotAtt04s.Any())
                query.Where<Lot>((p, l) => lotAtt04s.Contains(l.LotAtt04));
            if (lotAtt05s.Any())
                query.Where<Lot>((p, l) => lotAtt05s.Contains(l.LotAtt05));
            if (lotAtt06s.Any())
                query.Where<Lot>((p, l) => lotAtt06s.Contains(l.LotAtt06));
            if (lotAtt07s.Any())
                query.Where<Lot>((p, l) => lotAtt07s.Contains(l.LotAtt07));
            if (lotAtt08s.Any())
                query.Where<Lot>((p, l) => lotAtt08s.Contains(l.LotAtt08));
            if (lotAtt09s.Any())
                query.Where<Lot>((p, l) => lotAtt09s.Contains(l.LotAtt09));
            if (lotAtt10s.Any())
                query.Where<Lot>((p, l) => lotAtt10s.Contains(l.LotAtt10));
            if (lotAtt11s.Any())
                query.Where<Lot>((p, l) => lotAtt11s.Contains(l.LotAtt11));
            if (lotAtt12s.Any())
                query.Where<Lot>((p, l) => lotAtt12s.Contains(l.LotAtt12));
            if (isAllowManualGrounding.HasValue)
            {
                query.Where(p => p.StorageArea.IsAllowManualGrounding == isAllowManualGrounding.Value);
            }
            return query.ToList();
        }

        /// <summary>
        /// 根据id集合获取库存
        /// </summary>
        /// <param name="idList">库存ID</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>LPN库存</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhands(List<double> idList, EagerLoadOptions elo = null)
        {
            if (idList == null || idList.Count == 0)
            {
                return new EntityList<LotLpnOnhand>();
            }
            return idList.SplitContains(sons =>
            {
                var query = Query<LotLpnOnhand>().Where(p => p.Qty > 0).Where(i => sons.Contains(i.Id));
                return query.ToList(null, elo);
            });
        }

        /// <summary>
        /// 通过ID获取库存 不过滤库存组织
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<LotLpnOnhand> GetAllInvOrgLotLpnOnhands(List<double> idList, EagerLoadOptions elo = null)
        {
            if (idList == null || idList.Count == 0)
            {
                return new EntityList<LotLpnOnhand>();
            }
            using (SIE.Common.InvOrg.InvOrgs.WithAll())
            {
                return idList.SplitContains(sons =>
                {
                    var query = Query<LotLpnOnhand>().Where(p => p.Qty > 0).Where(i => sons.Contains(i.Id));
                    return query.ToList(null, elo);
                });
            }
        }
        /// <summary>
        /// 根据LPN获取批次和LPN库存(获取分配量)
        /// </summary>
        /// <param name="lpnList">LPN集合</param>
        /// <returns></returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnHandsByAllottedQty(List<string> lpnList)
        {
            return lpnList.Distinct().SplitContains(lpns =>
            {
                var query = Query<LotLpnOnhand>().Where(p => p.AllottedQty > 0);
                return query.Where(p => lpns.Contains(p.Lpn)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }


        /// <summary>
        /// 根据批次ID获取现有量大于0的库存
        /// </summary>
        /// <param name="lotIds">批次ID</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns></returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnHandsByLotIds(List<double> lotIds, EagerLoadOptions elo = null)
        {
            return lotIds.Distinct().SplitContains(ids =>
            {
                var query = Query<LotLpnOnhand>().Where(p => p.Qty > 0);
                return query.Where(p => ids.Contains(p.LotId)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 获取越库LPN库存
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="areaId">库区ID</param>
        /// <param name="locId">库位ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="lotId">批次ID</param>
        /// <param name="storerCode">货主</param>
        /// <param name="lpn">lpn</param>
        /// <param name="projectNo">项目号</param>
        /// <param name="taskNo">任务号</param>
        /// <param name="isZeroQty">是否排除0库存</param>
        /// <param name="lotCode">批次号</param>
        /// <param name="onhandState">库存状态</param>
        /// <param name="itemExtProp">物料扩展属性</param>  
        /// <param name="itemExtPropName">物料扩展属性</param>
        /// <returns>Lpn库存</returns>
        public virtual EntityList<LotLpnOnhand> GetCrossLotLpnOnhands(double? warehouseId, double? areaId, double? locId, double? itemId, double? lotId,
            string storerCode, string lpn, string projectNo, string taskNo, bool isZeroQty, OnhandState? onhandState, string itemExtProp = "",
            string lotCode = "", string itemExtPropName = "")
        {
            var query = Query<LotLpnOnhand>();
            if (warehouseId.HasValue)
                query.Where(p => p.WarehouseId == warehouseId);
            if (areaId.HasValue)
                query.Where(p => p.StorageAreaId == areaId);
            if (locId.HasValue)
                query.Where(p => p.StorageLocationId == locId);
            if (itemId.HasValue)
                query.Where(p => p.ItemId == itemId);
            if (lotId.HasValue)
                query.Where(p => p.LotId == lotId);
            else if (!lotCode.IsNullOrWhiteSpace())
            {
                query.Join<Lot>((x, y) => x.LotId == y.Id && y.Code == lotCode);
            }
            if (!lpn.IsNullOrEmpty() && lpn != "*")
                query.Where(p => p.Lpn == lpn);
            if (!storerCode.IsNullOrEmpty() && storerCode != "*")
                query.Where(p => p.StorerCode == storerCode);
            if (!projectNo.IsNullOrEmpty() && projectNo != "*")
                query.Where(p => p.ProjectNo == projectNo);
            if (!taskNo.IsNullOrEmpty())
                query.Where(p => p.TaskNo == taskNo && taskNo != "*");
            if (isZeroQty)
                query.Where(p => p.Qty > 0);
            if (onhandState.HasValue)
                query.Where(p => p.State == onhandState.Value);
            if (!itemExtProp.IsNullOrWhiteSpace())
                query.Where(p => p.ItemExtProp == itemExtProp);
            if (!itemExtPropName.IsNullOrWhiteSpace())
                query.Where(p => p.ItemExtPropName == itemExtPropName);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 锁定当前库存，防止并发
        /// </summary>
        /// <param name="locId">库位</param>
        /// <param name="itemId">物料</param>
        /// <param name="lotCode">批次</param>
        /// <param name="param">库存可选参数，如库存管控，则需要传入</param>
        public virtual void LockLotLpnOnhand(double locId, double itemId, string lotCode, InvOptionalParam param)
        {
            var query = DB.Update<LotLpnOnhand>();
            query.Where(p => p.StorageLocationId == locId && p.ItemId == itemId);
            query.Where(p => p.LotCode == lotCode);
            query.Where(p => p.Lpn == param.Lpn);
            query.Where(p => p.StorerCode == param.StorerCode);
            query.Where(p => p.ProjectNo == param.ProjectNo);
            query.Where(p => p.TaskNo == param.TaskNo);
            query.Where(p => p.State == param.State);
            if (!param.IsIgnoreItemExtProp && !param.ItemExtProp.IsNullOrEmpty())
                query.Where(p => p.ItemExtProp == param.ItemExtProp);
            query.Execute();
        }

        /// <summary>
        /// 获取批次和LPN库存
        /// </summary>
        /// <param name="locId">库位</param>
        /// <param name="itemId">物料</param>
        /// <param name="lotCode">批次</param>
        /// <param name="param">库存可选参数，如库存管控，则需要传入</param>
        /// <returns>批次和LPN库存</returns>
        public virtual LotLpnOnhand GetLotLpnOnhandByWorkFeed(double locId, double itemId, string lotCode, InvOptionalParam param)
        {
            var query = Query<LotLpnOnhand>();
            query.Where(p => p.StorageLocationId == locId && p.ItemId == itemId);
            query.Where(p => p.LotCode == lotCode);
            query.Where(p => p.Lpn == param.Lpn);
            query.Where(p => p.StorerCode == param.StorerCode || p.StorerCode == "*");
            query.Where(p => p.ProjectNo == param.ProjectNo);
            query.Where(p => p.TaskNo == param.TaskNo);
            query.Where(p => p.State == param.State);
            if (!param.IsIgnoreItemExtProp)
                query.Where(p => p.ItemExtProp == param.ItemExtProp);
            query.Where(p => p.AvailableQty > 0);
            return query.OrderByDescending(p => p.StorerCode).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        ///// <summary>
        ///// 获取批次库存（包含0库存）
        ///// </summary>
        ///// <param name="locId">库位</param>
        ///// <param name="itemId">物料</param>
        ///// <param name="lotCode">批次</param>
        ///// <param name="param">库存可选参数，如库存管控，则需要传入</param>
        ///// <returns>批次库存</returns>
        //public virtual LotOnhand GetLotOnhand(double locId, double itemId, string lotCode, InvOptionalParam param)
        //{
        //    var query = Query<LotOnhand>().Where(p => p.StorageLocationId == locId && p.ItemId == itemId);
        //    query.Where(p => p.LotCode == lotCode);
        //    query.Where(p => p.StorerCode == param.StorerCode);
        //    query.Where(p => p.ProjectNo == param.ProjectNo);
        //    query.Where(p => p.TaskNo == param.TaskNo);
        //    return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        //}

        ///// <summary>
        ///// 库位库存（包含0库存）
        ///// </summary>
        ///// <param name="locId">库位</param>
        ///// <param name="itemId">物料</param>
        ///// <param name="param">库存可选参数，如库存管控，则需要传入</param>
        ///// <returns>库位库存信息</returns>
        //public virtual LocationOnhand GetLocationOnhand(double locId, double itemId, InvOptionalParam param)
        //{
        //    var query = Query<LocationOnhand>().Where(p => p.StorageLocationId == locId && p.ItemId == itemId);
        //    query.Where(p => p.StorerCode == param.StorerCode);
        //    query.Where(p => p.ProjectNo == param.ProjectNo);
        //    query.Where(p => p.TaskNo == param.TaskNo);
        //    return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        //}

        /// <summary>
        /// 库位库存（包含0库存）
        /// </summary>
        /// <param name="locId">库位</param>
        /// <returns>库位库存信息</returns>
        public virtual void HasLocationOnhands(double locId)
        {
            var count = Query<LotLpnOnhand>().Where(p => p.StorageLocationId == locId && p.Qty > 0).Count();
            if (count > 0)
                throw new ValidationException("当前库位库存不为0,不可禁用".L10N());
        }

        ///// <summary>
        ///// 获取拣货库位库存
        ///// </summary>      
        ///// <param name="itemId">物料</param>
        ///// <returns>库位库存</returns>
        //public virtual EntityList<LocationOnhand> GetIsPickLocationOnhands(double itemId)
        //{
        //    return Query<LocationOnhand>().Join<StorageLocation>((x, y) => x.StorageLocationId == y.Id && y.IsPick && y.State == State.Enable)
        //        .Where(p => p.ItemId == itemId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        //}

        /// <summary>
        /// 判断物料是否还有库存
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <returns>返回是否库存</returns>
        public virtual void HasItemStock(double itemId)
        {
            var count = Query<LotLpnOnhand>().Where(p => p.ItemId == itemId && p.Qty > 0).Count();
            if (count > 0)
                throw new ValidationException("当前物料库存不为0,不可禁用".L10N());
        }

        ///// <summary>
        ///// 获取仓库对应物料有库存信息
        ///// </summary>
        ///// <param name="wareHouseId">仓库Id</param>
        ///// <param name="itemId">物料Id</param>
        ///// <returns>库存信息</returns>
        //public virtual LocationOnhand GetItemOnhand(double wareHouseId, double itemId)
        //{
        //    var query = Query<LocationOnhand>();
        //    query.Where(p => p.WarehouseId == wareHouseId && p.ItemId == itemId && p.Qty > 0);
        //    query.Join<StorageLocation>((x, y) => x.StorageLocationId == y.Id && y.State == State.Enable && !y.IsFrozen);
        //    query.Join<StorageLocationOperation>((x, y) => x.StorageLocationId == y.StorageLocationId && y.IsLayIn);
        //    return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        //}

        /// <summary>
        /// 获取库位库存信息
        /// </summary>
        /// <param name="storageLocationIds">库位Id集合</param>        
        /// <returns>库存信息</returns>
        public virtual EntityList<LocationOnhand> GetLocOnhands(List<double> storageLocationIds)
        {

            //var query = Query<LocationOnhand>();
            //var exp = storageLocationIds.CreateContainsExpression<LocationOnhand>("x", nameof(LocationOnhand.StorageLocationId));
            //if (exp == null)
            //    return new EntityList<LocationOnhand>();
            //return query.Where(exp).Where(p => p.Qty > 0).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (storageLocationIds == null || storageLocationIds.Count == 0)
            {
                return new EntityList<LocationOnhand>();
            }
            return storageLocationIds.SplitContains(sons =>
            {
                return Query<LocationOnhand>().Where(p => sons.Contains(p.StorageLocationId)).Where(i => i.Qty > 0).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        ///// <summary>
        ///// 获取库位库存
        ///// </summary>
        ///// <param name="queryAction">查询条件委托</param>
        ///// <param name="elo">贪婪加载项</param>
        ///// <returns>库位库存</returns>
        //public virtual EntityList<LocationOnhand> GetLocOnhands(Action<IEntityQueryer<LocationOnhand>> queryAction, EagerLoadOptions elo)
        //{
        //    var query = Query<LocationOnhand>();
        //    queryAction?.Invoke(query);
        //    return query.ToList(null, elo);
        //}

        /// <summary>
        /// 获取库位库存信息
        /// </summary>
        /// <param name="locIds">库位Id集合</param>        
        /// <returns>库存信息</returns>
        public virtual List<double> GetLocNotOnhands(List<double> locIds)
        {
            if (locIds == null || locIds.Count == 0)
            {
                return new List<double>();
            }

            locIds.SplitDataExecute(sons =>
            {
                locIds.AddRange(Query<LotLpnOnhand>().Where(i => sons.Contains(i.StorageLocationId)).Where(p => p.Qty <= 0).Select(a => a.StorageLocationId).ToList<double>());
            });
            return locIds.Distinct().ToList();
        }

        ///// <summary>
        ///// 获取库位库存行数
        ///// </summary>
        ///// <param name="queryAction">查询条件委托</param>
        ///// <returns>库位库存行数</returns>
        //public virtual int GetLocOnhandCount(Action<IEntityQueryer<LocationOnhand>> queryAction)
        //{
        //    var query = Query<LocationOnhand>();
        //    queryAction?.Invoke(query);
        //    return query.Count();
        //}

        ///// <summary>
        ///// 获取库位批次数
        ///// </summary>
        ///// <param name="queryAction">查询条件委托</param>
        ///// <returns>库位批次数key：库位 value：批次总数</returns>
        //public virtual Dictionary<double, int> GetLocItemOnhandCount(Action<IEntityQueryer<LocationOnhand>> queryAction)
        //{
        //    var query = Query<LocationOnhand>();
        //    query.Select(p => new { p.StorageLocationId, p.ItemId });
        //    queryAction?.Invoke(query);
        //    query.GroupBy(p => new { p.StorageLocationId, p.ItemId });
        //    var list = query.ToList<dynamic>().GroupBy(p => p.StorageLocationId).Select(p => new { p.Key, Count = p.Count() });
        //    return list.ToDictionary(p => (double)p.Key, p => p.Count);
        //}

        /// <summary>
        /// 获取库位批次数
        /// </summary>
        /// <param name="queryAction">查询条件委托</param>
        /// <returns>库位批次数key：库位 value：批次总数</returns>
        public virtual Dictionary<double, int> GetLocLotOnhandCount(Action<IEntityQueryer<LotOnhand>> queryAction)
        {
            var query = Query<LotOnhand>();
            query.Select(p => new { p.StorageLocationId, p.LotId });
            queryAction?.Invoke(query);
            query.GroupBy(p => new { p.StorageLocationId, p.LotId });
            var list1 = query.ToList<dynamic>();
            var list = list1.GroupBy(p => p.STORAGELOCATIONID).Select(p => new { p.Key, Count = p.Count() });
            return list.ToDictionary(p => (double)p.Key, p => p.Count);
        }

        /// <summary>
        /// 获取批次库存
        /// </summary>
        /// <param name="queryAction">查询条件委托</param>
        /// <param name="elo">贪婪加载项</param>
        /// <returns>批次库存</returns>
        public virtual EntityList<LotOnhand> GetLotOnhands(Action<IEntityQueryer<LotOnhand>> queryAction, EagerLoadOptions elo)
        {
            var query = Query<LotOnhand>();
            queryAction?.Invoke(query);
            return query.ToList(null, elo);
        }

        /// <summary>
        /// 获取批次和LPN库存
        /// </summary>
        /// <param name="criteria">查询实体.</param>
        /// <returns>批次和LPN库存</returns>
        public virtual EntityList<LotOnhand> GetLotOnhands(LotOnhandCriteria criteria)
        {
            var q = Query<LotOnhand>().Join<Lot>((x, y) => x.LotId == y.Id);

            ////增加仓库权限关联查询
            RT.Service.Resolve<WarehouseController>().ExistWarehouseEmplyee(q, LotOnhand.WarehouseIdProperty);

            if (criteria.Item != null)
                q.Where(p => p.ItemId == criteria.ItemId);
            if (criteria.Warehouse != null)
                q.Where(p => p.WarehouseId == criteria.WarehouseId);
            if (criteria.StorageArea != null)
                q.Where(p => p.StorageAreaId == criteria.StorageAreaId);
            if (criteria.StorageLocation != null)
                q.Where(p => p.StorageLocationId == criteria.StorageLocationId);
            if (criteria.StorerCode.IsNotEmpty())
                q.Where(p => p.StorerCode.Contains(criteria.StorerCode));
            if (criteria.LotCode.IsNotEmpty())
                q.Where<Lot>((x, y) => y.Code.Contains(criteria.LotCode));
            if (criteria.ProjectNo.IsNotEmpty())
                q.Where(p => p.ProjectNo.Contains(criteria.ProjectNo));
            if (criteria.TaskNo.IsNotEmpty())
                q.Where(p => p.TaskNo.Contains(criteria.TaskNo));
            if (!criteria.IsZero)
                q.Where(p => p.Qty > 0);

            EagerLoadOptions elo = new EagerLoadOptions();
            if (criteria.OrderInfoList.Count == 0)
                q.OrderByDescending(f => f.LotId);
            else
                q.OrderBy(criteria.OrderInfoList);
            return q.ToList(criteria.PagingInfo, elo.LoadWithViewProperty());
        }

        /// <summary>
        /// 获取批次库存行数
        /// </summary>
        /// <param name="queryAction">查询条件委托</param>
        /// <returns>批次库存行数</returns>
        public virtual int GetLotOnhandCount(Action<IEntityQueryer<LotOnhand>> queryAction)
        {
            var query = Query<LotOnhand>();
            queryAction?.Invoke(query);
            return query.Count();
        }

        ///// <summary>
        ///// 获取仓库没有库存库位
        ///// </summary>
        ///// <param name="wareHouseId">仓库ID</param>
        ///// <returns>仓库没有库存库位</returns>
        //public virtual StorageLocation GetWareHouseEmptyLoc(double wareHouseId)
        //{
        //    var query = Query<StorageLocation>();
        //    query.Where(p => p.WarehouseId == wareHouseId && p.State == State.Enable && !p.IsFrozen);
        //    query.Join<StorageLocationOperation>((x, y) => x.Id == y.StorageLocationId && y.IsLayIn);
        //    query.NotExists<LocationOnhand>((x, y) => y.Where(t => t.StorageLocationId == x.Id && t.Qty > 0));
        //    return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        //}

        /// <summary>
        /// 检查返回空库位
        /// </summary>
        /// <param name="locIds">要检查的库位Id</param>
        /// <param name="elo">贪婪</param>
        /// <returns>库位</returns>
        public virtual EntityList<StorageLocation> CheckAndReturnEmptyLocs(List<double> locIds, EagerLoadOptions elo = null)
        {
            if (locIds == null || locIds.Count == 0)
            {
                return new EntityList<StorageLocation>();
            }
            return locIds.SplitContains(sons =>
            {
                var query = Query<StorageLocation>().Where(p => sons.Contains(p.Id));
                query.NotExists<LotLpnOnhand>((x, y) => y.Where(t => t.StorageLocationId == x.Id && t.Qty > 0));
                return query.ToList(null, elo);
            });
        }

        /// <summary>
        /// 获取库区没有库存库位
        /// </summary>
        /// <param name="areaId">库区ID</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pageNumber">页号</param>
        /// <param name="pageSize">页码</param>
        /// <returns>仓库没有库存库位</returns>
        public virtual EntityList<StorageLocation> GetAvailableLocationList(double areaId, string keyword, int pageNumber, int pageSize)
        {
            var query = Query<StorageLocation>().Where(p => p.AreaId == areaId && p.State == State.Enable && !p.IsFrozen);

            ////查询关键字
            if (!string.IsNullOrEmpty(keyword))
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));

            query.Join<StorageArea>((x, y) => x.AreaId == y.Id && y.State == State.Enable && !y.IsFrozen);
            query.Join<StorageLocationOperation>((x, y) => x.Id == y.StorageLocationId && y.IsLayIn);
            query.NotExists<LotLpnOnhand>((x, y) => y.Where(t => t.StorageLocationId == x.Id && t.Qty > 0));

            PagingInfo pageInfo = new PagingInfo();
            pageInfo.PageNumber = pageNumber;
            pageInfo.PageSize = pageSize;
            return query.ToList(pageInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 调整库存
        /// </summary>
        /// <param name="adjustType">调整类型</param>
        /// <param name="loc">库位</param>
        /// <param name="item">物料</param>
        /// <param name="lot">批次</param>
        /// <param name="qty">调整数量：入库为正数，出库为负数</param>
        /// <param name="param">库存可选参数，如库存管控，则需要传入</param>
        /// <param name="isAllotted">是否调整分配数：是则调整库存分配数，否则调整可用数</param>
        /// <param name="isValidateMulLoc">是否验证LPN多库位存储</param>
        /// <param name="isValidateState">是否验证库存状态</param>
        /// <param name="isReceive">是否收货</param>
        /// <param name="isNotCheckNone">是否不验证未质检库存</param>
        public virtual LotLpnOnhand AdjustOnhand(AdjustType adjustType, StorageLocation loc, Item item, Lot lot, decimal qty, InvOptionalParam param,
            bool isAllotted = false, bool isValidateMulLoc = true, bool isValidateState = true, bool isReceive = false, bool isNotCheckNone = false)
        {
            var lotLpnOnhand = AdjustLotLpnOnhand(adjustType, loc, item, lot, qty, param, isAllotted, isValidateMulLoc, isValidateState, isReceive, isNotCheckNone);
            //AdjustLotOnhand(loc, item, lot, qty, param, isAllotted);
            //AdjustLocationOnhand(loc, item, qty, param, isAllotted);

            return lotLpnOnhand;
        }

        /// <summary>
        /// 获取库位库存
        /// </summary>
        /// <param name="criteria">查询实体.</param>
        /// <returns>库位库存</returns>
        public virtual EntityList<LocationOnhand> GetLocationOnhands(LocationOnhandCriteria criteria)
        {
            var q = Query<LocationOnhand>();

            ////增加仓库权限关联查询
            RT.Service.Resolve<WarehouseController>().ExistWarehouseEmplyee(q, LocationOnhand.WarehouseIdProperty);

            if (criteria.Item != null)
                q.Where(p => p.ItemId == criteria.ItemId);
            if (criteria.Warehouse != null)
                q.Where(p => p.WarehouseId == criteria.WarehouseId);
            if (criteria.StorageArea != null)
                q.Where(p => p.StorageAreaId == criteria.StorageAreaId);
            if (criteria.StorageLocation != null)
                q.Where(p => p.StorageLocationId == criteria.StorageLocationId);
            if (criteria.StorerCode.IsNotEmpty())
                q.Where(p => p.StorerCode.Contains(criteria.StorerCode));
            if (criteria.ProjectNo.IsNotEmpty())
                q.Where(p => p.ProjectNo.Contains(criteria.ProjectNo));
            if (criteria.TaskNo.IsNotEmpty())
                q.Where(p => p.TaskNo.Contains(criteria.TaskNo));
            if (!criteria.IsZero)
                q.Where(p => p.Qty > 0);
            if (criteria.ItemCode.IsNotEmpty())
                q.Where(p => p.Item.Code.Contains(criteria.ItemCode));
            if (criteria.WarehouseCode.IsNotEmpty())
                q.Where(p => p.Warehouse.Code.Contains(criteria.WarehouseCode));
            EagerLoadOptions elo = new EagerLoadOptions();
            if (criteria.OrderInfoList.Count == 0)
                q.OrderBy(f => f.WarehouseId).OrderBy(f => f.StorageLocationId);
            else q.OrderBy(criteria.OrderInfoList);
            return q.ToList(criteria.PagingInfo, elo.LoadWithViewProperty());
        }

        /// <summary>
        /// 获取批次和LPN库存
        /// </summary>
        /// <param name="criteria">查询实体.</param>
        /// <returns>批次和LPN库存</returns>
        public virtual EntityList<LotLpnOnhand> GetLpnOnhands(LpnOnhandCriteria criteria)
        {
            var meta = RF.Find<LotLpnOnhand>().EntityMeta;
            var q = DB.Query<LotLpnOnhand>("i1").Join<Lot>("l1", (x, y) => x.LotId == y.Id);

            ////增加仓库权限关联查询
            RT.Service.Resolve<WarehouseController>().ExistWarehouseEmplyee(q, LotLpnOnhand.WarehouseIdProperty);

            if (criteria.Item != null)
                q.Where(p => p.ItemId == criteria.ItemId);
            if (criteria.Warehouse != null)
                q.Where(p => p.WarehouseId == criteria.WarehouseId);
            if (criteria.StorageArea != null)
                q.Where(p => p.StorageAreaId == criteria.StorageAreaId);
            if (criteria.StorageLocation != null)
                q.Where(p => p.StorageLocationId == criteria.StorageLocationId);
            if (criteria.StorerCode.IsNotEmpty())
                q.Where(p => p.StorerCode.Contains(criteria.StorerCode));
            if (criteria.LotCode.IsNotEmpty())
                q.Where(p => p.LotCode.Contains(criteria.LotCode));
            if (criteria.Lpn.IsNotEmpty())
                q.Where(p => p.Lpn.Contains(criteria.Lpn));
            if (criteria.ProjectNo.IsNotEmpty())
                q.Where(p => p.ProjectNo.Contains(criteria.ProjectNo));
            if (criteria.TaskNo.IsNotEmpty())
                q.Where(p => p.TaskNo.Contains(criteria.TaskNo));
            if (!criteria.IsZero)
                q.Where(p => p.Qty > 0);
            if (criteria.IsAllowManualGrounding.HasValue)
            {
                q.Where(p => p.StorageArea.IsAllowManualGrounding == criteria.IsAllowManualGrounding.Value);
            }

            EagerLoadOptions elo = new EagerLoadOptions();
            SetLpnOnhandCriteriaData(q, criteria, meta);

            q.OrderBy(criteria.OrderInfoList);
            return q.ToList(criteria.PagingInfo, elo.LoadWithViewProperty());
        }

        /// <summary>
        /// 设置查询实体条件
        /// </summary>
        /// <param name="q">查询器</param>
        /// <param name="criteria">查询实体</param>
        /// <param name="meta">元数据</param>
        private void SetLpnOnhandCriteriaData(IEntityQueryer<LotLpnOnhand> q, LpnOnhandCriteria criteria, EntityMeta meta)
        {
            if (criteria.OrderType.HasValue)
            {
                if (criteria.OrderType == OrderType.Frozen)
                    q.Where(p => p.AvailableQty > 0);
                if (criteria.OrderType == OrderType.UnFrozen)
                    q.Where(p => p.FreezingQty > 0);
            }
            if (criteria.IsNotNoneState == true)
            {
                q.Where(p => p.State != OnhandState.None);
            }

            if (criteria.State.HasValue)
            {
                //与不查询未质检互斥，这里条件是为了避免定义了不查询未质检，但是状态又是查质检的
                if (criteria.State != OnhandState.None || criteria.IsNotNoneState != true)
                {
                    q.Where(p => p.State == criteria.State);
                }
            }
            if (!string.IsNullOrEmpty(criteria.ItemCode))
                q.Where(p => p.Item.Code.Contains(criteria.ItemCode) || p.Item.Name.Contains(criteria.ItemCode));
            if (criteria.WarehouseId == null && criteria.WarehouseCode.IsNotEmpty())
                q.Where(p => p.Warehouse.Code.Contains(criteria.WarehouseCode) || p.Warehouse.Name.Contains(criteria.WarehouseCode));
            if (!string.IsNullOrEmpty(criteria.StorageAreaCode))
                q.Where(p => p.StorageArea.Code.Contains(criteria.StorageAreaCode) || p.StorageArea.Name.Contains(criteria.StorageAreaCode));

            q.Exists<StorageLocation>((a, b) => b.Where(p => p.Id == a.StorageLocationId)
            .WhereIf(criteria.StorageLocationCode.IsNotEmpty(), p => p.Code.Contains(criteria.StorageLocationCode) || p.Name.Contains(criteria.StorageLocationCode))
             .WhereIf(criteria.IsFrozen.HasValue, p => p.IsFrozen == criteria.IsFrozen)
             .WhereIf(criteria.IsLock.HasValue, p => p.IsOutLock == criteria.IsLock && p.IsCountLock == criteria.IsLock && p.IsBackup == criteria.IsLock)
             .WhereIf(criteria.IsAdjustLock.HasValue, p => p.IsOutLock == criteria.IsAdjustLock && p.IsCountLock == criteria.IsAdjustLock && p.IsInLock == criteria.IsAdjustLock)
             .WhereIf(criteria.IsAutomated.HasValue, p => p.IsAutomatedStorage == criteria.IsAutomated)
             );


            if (criteria.IsExistsAvailableQty.HasValue)
                q.Where(p => p.AvailableQty > 0);
            if (criteria.IsNotEmptyStorer.HasValue)
                q.Where(p => p.StorerCode != "*");
            if (criteria.NoLotDefault)
                q.Where(p => p.LotCode != Lot.LotDefault);
            //数据格式 物料Id^物料扩展属性|物料Id^物料扩展属性
            if (criteria.ItemIdandExtprops.IsNotEmpty())
            {
                var itemandext = criteria.ItemIdandExtprops.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                string sql = "(";
                int i = 1;
                itemandext.ForEach(p =>
                {
                    if (i > 1)
                    {
                        sql += " or ";
                    }
                    var itemf = p.Split('^').ToList();
                    string eprop = string.Empty;
                    if (itemf[1].IsNotEmpty())
                    {
                        eprop = @" and i1.{0}='{1}'".FormatArgs(meta.Property(LotLpnOnhand.ItemExtPropProperty).ColumnMeta.ColumnName, itemf[1]);
                    }//项目号
                    if (itemf.Count > 2)
                    {
                        if (itemf[2].IsNotEmpty())
                        {
                            eprop += @" and i1.{0}='{1}'".FormatArgs(meta.Property(LotLpnOnhand.ProjectNoProperty).ColumnMeta.ColumnName, itemf[2]);
                        }//任务号
                        if (itemf[3].IsNotEmpty())
                        {
                            eprop += @" and i1.{0}='{1}'".FormatArgs(meta.Property(LotLpnOnhand.TaskNoProperty).ColumnMeta.ColumnName, itemf[3]);
                        }//批次
                        if (itemf[4].IsNotEmpty() && itemf[4] != "null")
                        {
                            eprop += @" and i1.{0}={1}".FormatArgs(meta.Property(LotLpnOnhand.LotIdProperty).ColumnMeta.ColumnName, itemf[4]);
                        }//LPN
                        if (itemf[5].IsNotEmpty())
                        {
                            eprop += @" and i1.{0}='{1}'".FormatArgs(meta.Property(LotLpnOnhand.LpnProperty).ColumnMeta.ColumnName, itemf[5]);
                        }
                    }
                    sql += @"i1.{0}={1}{2}".FormatArgs(meta.Property(LotLpnOnhand.ItemIdProperty).ColumnMeta.ColumnName, itemf[0], eprop);

                    i++;
                });
                sql += ")";
                q.Where(f => f.SQL<bool>(new FormattedSql(sql)));
            }

            if (criteria.IsInspection)
            {
                string sql = string.Empty;
                var provider = Domain.ORM.RdbDataProvider.Get(RF.Find<LotLpnOnhand>()).DbSetting.ProviderName;
                if (provider == DbProvider.ODP || provider == DbProvider.Oracle)
                    sql = @" exists(select 1 from ITEM_STOCK_DATA k where k.ITEM_ID = i1.ITEM_ID
                                            and (sysdate + k.Early_Warn_Period >= l1.lot_att02 or k.Early_Warn_Period is null))";
                else if (provider == DbProvider.MicrosoftSqlClient || provider == DbProvider.SqlClient)
                    sql = @" exists(select 1 from ITEM_STOCK_DATA k where k.ITEM_ID = i1.ITEM_ID
                                            and (dateadd(dd,k.Early_Warn_Period,getdate()) >= l1.lot_att02 or k.Early_Warn_Period is null))";
                if (sql.IsNotEmpty())
                    q.Where(f => f.SQL<bool>(new FormattedSql(sql)));
            }
            if (criteria.ExpectIds.IsNotEmpty())
            {
                List<double> noIds = new List<double>();
                criteria.ExpectIds.Split(',').ForEach(p =>
                {
                    if (p.IsNotEmpty())
                        noIds.Add(double.Parse(p));
                });
                q.Where(f => !noIds.Contains(f.Id));
            }
        }

        /// <summary>
        /// 标准移动和标准调拨获取库存数据(可用量）
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="keywords">关键对象</param>
        /// <param name="pageNumber">页号</param>
        /// <param name="pageSize">页码</param>
        /// <returns>库存数据</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhandForMove(double warehouseId, KeywordDatas keywords, int pageNumber, int pageSize)
        {
            var query = Query<LotLpnOnhand>().Where(p => p.WarehouseId == warehouseId);
            if (keywords.QtyType.HasValue)
            {
                switch (keywords.QtyType.Value)
                {
                    case 1:
                        query.Where(p => p.AvailableQty > 0);
                        break;
                    case 2:
                        query.Where(p => p.AllottedQty > 0);
                        break;
                    case 3:
                        query.Where(p => p.AvailableQty + p.AllottedQty > 0);
                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrEmpty(keywords.LocationCode))
            {
                query.Join<StorageLocation>((a, b) => a.StorageLocationId == b.Id && b.State == State.Enable && !b.IsFrozen &&
                 (b.Code.Contains(keywords.LocationCode) || b.Name.Contains(keywords.LocationCode)));
            }

            if (!string.IsNullOrEmpty(keywords.LpnCode))
            {
                query.Where(p => p.Lpn.Contains(keywords.LpnCode));
            }

            if (!string.IsNullOrEmpty(keywords.StorerCode))
            {
                query.Where(p => p.StorerCode.Contains(keywords.StorerCode));
            }

            if (!string.IsNullOrEmpty(keywords.ProjectNo))
            {
                query.Where(p => p.ProjectNo.Contains(keywords.ProjectNo));
            }

            if (!string.IsNullOrEmpty(keywords.ItemCode))
            {
                query.Join<Item>((a, i) => a.ItemId == i.Id && (i.Code.Contains(keywords.ItemCode) || i.Name.Contains(keywords.ItemCode)));
            }
            if (!string.IsNullOrEmpty(keywords.LotCode))
            {
                query.Where(p => p.LotCode.Contains(keywords.LotCode));
            }
            if (keywords.OnhandState.HasValue)
            {
                query.Where(p => (int)p.State == keywords.OnhandState.Value);
            }

            if (!string.IsNullOrEmpty(keywords.ItemExtProp))
            {
                query.Where(p => p.ItemExtProp == keywords.ItemExtProp);
            }

            query.Where(p => p.State != OnhandState.None && p.StorageArea.IsAllowManualGrounding);

            PagingInfo pageInfo = new PagingInfo();
            pageInfo.PageNumber = pageNumber;
            pageInfo.PageSize = pageSize;

            return query.OrderBy(p => p.Item).ToList(pageInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 标准移动和标准调拨获取库存数据(可用量 + 分配量）
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="keywords">关键对象</param>
        /// <param name="pageNumber">页号</param>
        /// <param name="pageSize">页码</param>
        /// <returns>库存数据</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhandForTask(double warehouseId, KeywordDatas keywords, int pageNumber, int pageSize)
        {
            var query = Query<LotLpnOnhand>().Where(p => p.WarehouseId == warehouseId);

            if (!string.IsNullOrEmpty(keywords.LocationCode))
                query.Join<StorageLocation>((a, b) => a.StorageLocationId == b.Id && b.State == State.Enable && !b.IsFrozen &&
                (b.Code.Contains(keywords.LocationCode) || b.Name.Contains(keywords.LocationCode)));

            if (!string.IsNullOrEmpty(keywords.LpnCode))
                query.Where(p => p.Lpn.Contains(keywords.LpnCode));

            if (!string.IsNullOrEmpty(keywords.StorerCode))
                query.Where(p => p.StorerCode.Contains(keywords.StorerCode));

            if (!string.IsNullOrEmpty(keywords.ProjectNo))
                query.Where(p => p.ProjectNo.Contains(keywords.ProjectNo));

            if (!string.IsNullOrEmpty(keywords.ItemCode))
                query.Join<Item>((a, i) => a.ItemId == i.Id && (i.Code.Contains(keywords.ItemCode) || i.Name.Contains(keywords.ItemCode)));

            if (!string.IsNullOrEmpty(keywords.LotCode))
                query.Where(p => p.LotCode.Contains(keywords.LotCode));

            if (keywords.OnhandState.HasValue)
                query.Where(p => (int)p.State == keywords.OnhandState.Value);

            if (!string.IsNullOrEmpty(keywords.ItemExtProp))
                query.Where(p => p.ItemExtProp == keywords.ItemExtProp);
            query.Where(p => p.State != OnhandState.None);

            PagingInfo pageInfo = new PagingInfo();
            pageInfo.PageNumber = pageNumber;
            pageInfo.PageSize = pageSize;

            return query.OrderBy(p => p.Item).ToList(pageInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 标准移动和标准调拨获取库存数据
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="keyword">关键对象</param>
        /// <param name="pageNumber">页号</param>
        /// <param name="pageSize">页码</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>库存数据</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhandDatas(double warehouseId, string keyword, int pageNumber, int pageSize, EagerLoadOptions elo)
        {
            var query = Query<LotLpnOnhand>().Where(p => p.WarehouseId == warehouseId && p.Qty > 0);
            if (!string.IsNullOrEmpty(keyword))
                query.LeftJoin<StorageLocation>((a, s) => a.StorageLocationId == s.Id).LeftJoin<Item>((a, i) => a.ItemId == i.Id).
                    Where<StorageLocation, Item>((x, y, z) => x.Lpn.Contains(keyword) || x.LotCode.Contains(keyword) ||
                    x.StorerCode.Contains(keyword) || x.ProjectNo.Contains(keyword) || y.Code.Contains(keyword) || y.Name.Contains(keyword) ||
                    z.Code.Contains(keyword) || z.Name.Contains(keyword));

            PagingInfo pageInfo = new PagingInfo();
            pageInfo.PageNumber = pageNumber;
            pageInfo.PageSize = pageSize;

            return query.ToList(pageInfo, elo);
        }

        /// <summary>
        /// 标准移动和标准调拨获取库存数据
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="keyword">关键对象</param>
        /// <returns>库存数据</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhandDatas(double warehouseId, string keyword)
        {
            var query = Query<LotLpnOnhand>().Where(p => p.WarehouseId == warehouseId);
            if (!string.IsNullOrEmpty(keyword))
                query.LeftJoin<StorageLocation>((a, s) => a.StorageLocationId == s.Id).LeftJoin<Item>((a, i) => a.ItemId == i.Id).
                    Where<StorageLocation, Item>((x, y, z) => x.Lpn.Contains(keyword) || x.LotCode.Contains(keyword) ||
                    x.StorerCode.Contains(keyword) || x.ProjectNo.Contains(keyword) || y.Code.Contains(keyword) || y.Name.Contains(keyword) ||
                    z.Code.Contains(keyword) || z.Name.Contains(keyword));

            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 库存冻结获取库存数据
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="keywords">关键对象</param>
        /// <param name="type">操作</param>
        /// <param name="pageNumber">页号</param>
        /// <param name="pageSize">页码</param>
        /// <returns>库存数据</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhandForFrozen(double warehouseId, KeywordDatas keywords, int type, int pageNumber, int pageSize)
        {
            var query = type == (int)OrderType.Frozen ? Query<LotLpnOnhand>().Where(p => p.WarehouseId == warehouseId && p.AvailableQty > 0 && p.State != OnhandState.None) : Query<LotLpnOnhand>().Where(p => p.WarehouseId == warehouseId && p.FreezingQty > 0 && p.State != OnhandState.None);
            if (!string.IsNullOrEmpty(keywords.LocationCode))
                query.Join<StorageLocation>((a, b) => a.StorageLocationId == b.Id && b.State == State.Enable && !b.IsFrozen &&
                (b.Code.Contains(keywords.LocationCode) || b.Name.Contains(keywords.LocationCode)));

            if (!string.IsNullOrEmpty(keywords.LpnCode))
                query.Where(p => p.Lpn.Contains(keywords.LpnCode));

            if (!string.IsNullOrEmpty(keywords.StorerCode))
                query.Where(p => p.StorerCode.Contains(keywords.StorerCode));

            if (!string.IsNullOrEmpty(keywords.ProjectNo))
                query.Where(p => p.ProjectNo.Contains(keywords.ProjectNo));

            if (!string.IsNullOrEmpty(keywords.ItemCode))
                query.Join<Item>((a, i) => a.ItemId == i.Id && (i.Code.Contains(keywords.ItemCode) || i.Name.Contains(keywords.ItemCode)));

            if (!string.IsNullOrEmpty(keywords.LotCode))
                query.Where(p => p.LotCode.Contains(keywords.LotCode));

            if (keywords.OnhandState.HasValue)
                query.Where(p => (int)p.State == keywords.OnhandState.Value);

            PagingInfo pageInfo = new PagingInfo();
            pageInfo.PageNumber = pageNumber;
            pageInfo.PageSize = pageSize;

            return query.OrderBy(p => p.Item).ToList(pageInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取批次和LPN的库存（过滤可用量或冻结量为0）
        /// </summary>
        /// <param name="criteria">查询实体.</param>
        /// <returns>批次和LPN库存</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhandForFrozen(LotLpnOnhandCriteria criteria)
        {
            var query = Query<LotLpnOnhand>().Where(p => p.WarehouseId == criteria.WarehouseId);

            ////增加仓库权限关联查询
            RT.Service.Resolve<WarehouseController>().ExistWarehouseEmplyee(query, LotLpnOnhand.WarehouseIdProperty);

            if (criteria.type == OrderType.Frozen)
                query.Where(p => p.AvailableQty > 0);
            else
                query.Where(p => p.FreezingQty > 0);
            if (criteria.Item != null)
                query.Where(p => p.ItemId == criteria.ItemId);
            if (criteria.StorageLocation != null)
                query.Where(p => p.StorageLocationId == criteria.StorageLocationId);
            if (criteria.StorerCode.IsNotEmpty())
                query.Where(p => p.StorerCode.Contains(criteria.StorerCode));
            if (criteria.Lot.IsNotEmpty())
                query.Where(p => p.LotCode.Contains(criteria.Lot));
            if (criteria.Lpn.IsNotEmpty())
                query.Where(p => p.Lpn.Contains(criteria.Lpn));
            if (criteria.ProjectNo.IsNotEmpty())
                query.Where(p => p.ProjectNo.Contains(criteria.ProjectNo));
            if (criteria.TaskNo.IsNotEmpty())
                query.Where(p => p.TaskNo.Contains(criteria.TaskNo));
            if (!criteria.IsZero)
                query.Where(p => p.Qty > 0);
            if (criteria.IsFrozen.HasValue)
                query.Where(p => p.StorageLocation.IsFrozen == criteria.IsFrozen);
            if (criteria.State.HasValue)
                query.Where(p => p.State == criteria.State);
            if (criteria.IsExistsAvailableQty.HasValue)
                query.Where(p => p.AvailableQty > 0);
            if (criteria.IsNotEmptyStorer.HasValue)
                query.Where(p => p.StorerCode != "*");
            query.Where(p => p.State != OnhandState.None);

            query.OrderBy(criteria.OrderInfoList);
            EagerLoadOptions elo = new EagerLoadOptions();
            return query.ToList(criteria.PagingInfo, elo.LoadWithViewProperty());
        }

        /// <summary>
        /// 根据库位、物料、批次、lpn获取库存
        /// </summary>
        /// <param name="wareHouseId">仓库Id</param>
        /// <param name="locationId">库位Id</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="lotCode">批次Id</param>
        /// <param name="lpn">Lpn</param>
        /// <returns>返回库存信息</returns>
        public virtual LotLpnOnhand GetLotLpnOnhandForFrozen(double wareHouseId, double locationId, double itemId, string lotCode, string lpn)
        {
            var query = Query<LotLpnOnhand>();
            query.Where(p => p.WarehouseId == wareHouseId && p.StorageLocationId == locationId &&
                                  p.ItemId == itemId && p.LotCode == lotCode);
            if (!lpn.IsNullOrEmpty())
            {
                query.Where(p => p.Lpn.Contains(lpn));
            }
            query.Where(p => p.AvailableQty > 0);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 库存冻结获取库存行数
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="keywords">关键对象</param>
        /// <param name="type">操作</param>
        /// <returns>库存数据</returns>
        public virtual int GetLotLpnOnhandForFrozenCount(double warehouseId, KeywordDatas keywords, int type)
        {
            var query = type == (int)OrderType.Frozen ? Query<LotLpnOnhand>().Where(p => p.WarehouseId == warehouseId && p.AvailableQty > 0 && p.State != OnhandState.None) : Query<LotLpnOnhand>().Where(p => p.WarehouseId == warehouseId && p.FreezingQty > 0 && p.State != OnhandState.None);
            if (!string.IsNullOrEmpty(keywords.LocationCode))
                query.Join<StorageLocation>((a, b) => a.StorageLocationId == b.Id && b.State == State.Enable && !b.IsFrozen &&
                (b.Code.Contains(keywords.LocationCode) || b.Name.Contains(keywords.LocationCode)));

            if (!string.IsNullOrEmpty(keywords.LpnCode))
                query.Where(p => p.Lpn.Contains(keywords.LpnCode));

            if (!string.IsNullOrEmpty(keywords.StorerCode))
                query.Where(p => p.StorerCode.Contains(keywords.StorerCode));

            if (!string.IsNullOrEmpty(keywords.ProjectNo))
                query.Where(p => p.ProjectNo.Contains(keywords.ProjectNo));

            if (!string.IsNullOrEmpty(keywords.ItemCode))
                query.Join<Item>((a, i) => a.ItemId == i.Id && (i.Code.Contains(keywords.ItemCode) || i.Name.Contains(keywords.ItemCode)));

            if (!string.IsNullOrEmpty(keywords.LotCode))
                query.Where(p => p.LotCode.Contains(keywords.LotCode));

            if (keywords.OnhandState.HasValue)
                query.Where(p => (int)p.State == keywords.OnhandState.Value);

            return query.ToList().Count;
        }

        /// <summary>
        /// 获取批次和LPN的库存（过滤货主为*）
        /// </summary>
        /// <param name="criteria">查询实体.</param>
        /// <returns>批次和LPN库存</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhandsForOnhandAdjust(LotLpnOnhandCriteria criteria)
        {
            var q = Query<LotLpnOnhand>().Where(p => p.StorerCode != "*");

            ////增加仓库权限关联查询
            RT.Service.Resolve<WarehouseController>().ExistWarehouseEmplyee(q, LotLpnOnhand.WarehouseIdProperty);

            if (criteria.Item != null)
                q.Where(p => p.ItemId == criteria.ItemId);
            if (criteria.Warehouse != null)
                q.Join<StorageArea>((a, b) => a.StorageAreaId == b.Id && !b.IsFrozen).Where(p => p.WarehouseId == criteria.WarehouseId);
            if (criteria.StorageLocation != null)
                q.Where(p => p.StorageLocationId == criteria.StorageLocationId);
            if (criteria.StorerCode.IsNotEmpty())
                q.Where(p => p.StorerCode.Contains(criteria.StorerCode));
            if (criteria.Lot.IsNotEmpty())
                q.Where(p => p.LotCode.Contains(criteria.Lot));
            if (criteria.Lpn.IsNotEmpty())
                q.Where(p => p.Lpn.Contains(criteria.Lpn));
            if (criteria.ProjectNo.IsNotEmpty())
                q.Where(p => p.ProjectNo.Contains(criteria.ProjectNo));
            if (criteria.TaskNo.IsNotEmpty())
                q.Where(p => p.TaskNo.Contains(criteria.TaskNo));
            if (!criteria.IsZero)
                q.Where(p => p.Qty > 0);
            if (criteria.IsFrozen.HasValue)
                q.Where(p => p.StorageLocation.IsFrozen == criteria.IsFrozen);
            if (criteria.State.HasValue)
                q.Where(p => p.State == criteria.State);
            if (criteria.IsExistsAvailableQty.HasValue)
                q.Where(p => p.AvailableQty > 0);
            if (criteria.IsNotEmptyStorer.HasValue)
                q.Where(p => p.StorerCode != "*");
            q.Where(p => p.State != OnhandState.None);

            EagerLoadOptions elo = new EagerLoadOptions();
            q.OrderBy(criteria.OrderInfoList);
            return q.ToList(criteria.PagingInfo, elo.LoadWithViewProperty());
        }

        /// <summary>
        /// 标准移动和标准调拨获取库存数据
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="keywords">关键对象</param>
        /// <param name="pageNumber">页号</param>
        /// <param name="pageSize">页码</param>
        /// <param name="type">0-去掉*的货主</param>
        /// <returns>库存数据</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhandsForOnhandAdjust(double warehouseId, KeywordDatas keywords, int pageNumber, int pageSize, int type)
        {
            var query = Query<LotLpnOnhand>().Where(p => p.WarehouseId == warehouseId && p.AvailableQty > 0);
            if (!string.IsNullOrEmpty(keywords.LocationCode))
                query.Join<StorageLocation>((a, b) => a.StorageLocationId == b.Id && b.State == State.Enable && !b.IsFrozen &&
                (b.Code.Contains(keywords.LocationCode) || b.Name.Contains(keywords.LocationCode)));

            if (!string.IsNullOrEmpty(keywords.LpnCode))
                query.Where(p => p.Lpn.Contains(keywords.LpnCode));

            if (!string.IsNullOrEmpty(keywords.StorerCode))
                query.Where(p => p.StorerCode.Contains(keywords.StorerCode));

            if (!string.IsNullOrEmpty(keywords.ProjectNo))
                query.Where(p => p.ProjectNo.Contains(keywords.ProjectNo));

            if (!string.IsNullOrEmpty(keywords.TaskNo))
                query.Where(p => p.TaskNo.Contains(keywords.TaskNo));

            if (!string.IsNullOrEmpty(keywords.ItemCode))
                query.Join<Item>((a, i) => a.ItemId == i.Id && (i.Code.Contains(keywords.ItemCode) || i.Name.Contains(keywords.ItemCode)));

            if (!string.IsNullOrEmpty(keywords.LotCode))
                query.Where(p => p.LotCode.Contains(keywords.LotCode));

            if (keywords.OnhandState.HasValue)
                query.Where(p => (int)p.State == keywords.OnhandState.Value);
            query.Where(p => p.State != OnhandState.None);
            if (type == 0)
            {
                ////query.Where(p => p.StorerCode != "*");
            }

            PagingInfo pageInfo = new PagingInfo();
            pageInfo.PageNumber = pageNumber;
            pageInfo.PageSize = pageSize;

            return query.OrderBy(p => p.Item).ToList(pageInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 波次计划添加分配明细获取库位信息
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <param name="keyworks">关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="storerCode">项目号</param>
        /// <returns>库存信息</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhandsForWavePlan(double warehouseId, double itemId, string itemExtProp, string keyworks, PagingInfo pagingInfo = null, string storerCode = "")
        {
            var query = Query<LotLpnOnhand>().Where(p => p.WarehouseId == warehouseId && p.ItemId == itemId && p.ItemExtProp == itemExtProp && p.AvailableQty > 0);

            if (!string.IsNullOrEmpty(keyworks))
            {
                query.Join<Warehouse>((x, y) => x.WarehouseId == y.Id && (y.Code.Contains(keyworks) || y.Name.Contains(keyworks)));
            }
            if (storerCode.IsNotEmpty())
            {
                query.Where(p => p.StorerCode == storerCode);
            }
            var onhands = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            return onhands;
        }

        /// <summary>
        /// 获取合格库位物料库存
        /// </summary>
        /// <param name="itemId">物料</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <param name="storerCode">货主</param>
        /// <param name="warehouseId">仓库id</param>
        /// <returns>库存</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhandsForReplenish(double itemId, double warehouseId, string itemExtProp, string storerCode)
        {
            return Query<LotLpnOnhand>().Join<StorageLocation>((x, y) => x.StorageLocationId == y.Id && y.IsPick && y.State == State.Enable)
                .Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtProp && p.State == OnhandState.Ok && p.WarehouseId == warehouseId && p.StorerCode == storerCode).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 补货分配明细获取库位信息
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="locationId">库位Id</param>
        /// <param name="lotId">批次Id</param>
        /// <param name="lpn">lpn</param>
        /// <param name="storageAreaId">库区Id</param>
        /// <param name="keyworks">关键字</param>
        /// <param name="stoerCode">货主</param>
        /// <param name="projectNo">项目号</param>
        /// <param name="taskNo">任务号</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <param name="itemExtPropName">物料扩展属性</param>
        /// <param name="exceptId">排除Id</param>
        /// <returns>库存信息</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhandsForReplenishAssign(double warehouseId, double itemId, double? storageAreaId, double? locationId,
            string lpn, double? lotId, string keyworks, string stoerCode, string projectNo, string taskNo, string itemExtProp, string itemExtPropName, PagingInfo pagingInfo = null, double? exceptId = null)
        {
            var query = Query<LotLpnOnhand>().Where(p => p.WarehouseId == warehouseId && p.ItemId == itemId && p.AvailableQty > 0);
            if (!string.IsNullOrEmpty(keyworks))
            {
                query.Join<Warehouse>((x, y) => x.WarehouseId == y.Id && (y.Code.Contains(keyworks) || y.Name.Contains(keyworks)));
            }
            if (storageAreaId.HasValue)
            {
                query.Where(p => p.StorageAreaId == storageAreaId);
            }
            if (locationId.HasValue)
            {
                query.Where(p => p.StorageLocationId == locationId);
            }
            if (!lpn.IsNullOrWhiteSpace() && lpn != "*")
            {
                query.Where(p => p.Lpn == lpn);
            }
            if (lotId.HasValue)
            {
                query.Where(p => p.LotId == lotId);
            }
            if (exceptId.HasValue)
            {
                query.Where(p => p.Id != exceptId);
            }
            if (!stoerCode.IsNullOrEmpty())
            {
                query.Where(p => p.StorerCode == stoerCode);
            }
            if (!projectNo.IsNullOrEmpty() && projectNo != "*")
            {
                query.Where(p => p.ProjectNo == projectNo);
            }
            if (!taskNo.IsNullOrEmpty() && taskNo != "*")
            {
                query.Where(p => p.TaskNo == taskNo);
            }
            if (!itemExtProp.IsNullOrEmpty())
            {
                query.Where(p => p.ItemExtProp == itemExtProp);
            }
            if (!itemExtPropName.IsNullOrEmpty())
            {
                query.Where(p => p.ItemExtPropName == itemExtPropName);
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 扫描识别获取内存
        /// </summary>
        /// <param name="warehouseId">仓库id</param>
        /// <param name="queryAction"></param>
        /// <returns></returns>
        public virtual EntityList<LotLpnOnhand> GetLotOnhandForAutoScan(double warehouseId, Action<IEntityQueryer<LotLpnOnhand>> queryAction)
        {
            var query = Query<LotLpnOnhand>();
            query.Where(p => p.WarehouseId == warehouseId);
            queryAction?.Invoke(query);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取物料属性定义字典
        /// </summary>
        /// <param name="defIdList">物料属性定义Id列表</param>
        /// <returns>物料属性定义字典</returns>
        public virtual Dictionary<double, string> GetItemPropertyDefinitionDict(List<double> defIdList)
        {
            //Dictionary<double, string> itemPropertyDefDict = new Dictionary<double, string>();
            //var exp = defIdList.CreateContainsExpression<ItemPropertyDefinition>("x", nameof(ItemPropertyDefinition.Id));
            //if (exp == null)
            //{
            //    return itemPropertyDefDict;
            //}

            //itemPropertyDefDict = Query<ItemPropertyDefinition>().Where(exp).ToList().ToDictionary(p => p.Id, p => p.SortName);
            Dictionary<double, string> itemPropertyDefDict = new Dictionary<double, string>();
            if (defIdList == null || defIdList.Count == 0)
            {
                return itemPropertyDefDict;
            }
            defIdList.SplitDataExecute(sons =>
            {
                itemPropertyDefDict = Query<ItemPropertyDefinition>().Where(p => sons.Contains(p.Id)).ToList().ToDictionary(p => p.Id, p => p.Name);
            });
            return itemPropertyDefDict;
        }

        /// <summary>
        /// 获取冻结数大于0的库存
        /// </summary>
        /// <returns>返回库存信息</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhandForUnFrozen()
        {
            var query = Query<LotLpnOnhand>();
            query.Where(p => p.FreezingQty > 0);

            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询失效库存行数
        /// </summary>
        /// <param name="onhandIds">库存ID</param>
        /// <returns>失效库存行数</returns>
        public virtual int GetExpireOnhandCount(List<double> onhandIds)
        {
            int count = 0;
            if (onhandIds == null || onhandIds.Count == 0)
            {
                return count;
            }
            onhandIds.SplitDataExecute(sons =>
            {
                var query = Query<LotLpnOnhand>();
                query.Where(p => p.Lot.LotAtt02 != null && p.Lot.LotAtt02 < DateTime.Now);
                query.Where(p => sons.Contains(p.Id));
                count = query.Count();
            });
            return count;
        }

        /// <summary>
        /// 获取库存信息
        /// </summary>
        /// <param name="warehouseIds">仓库ID集合</param>
        /// <param name="onhandDataList">库存参数集合</param>
        /// <returns>批次和LPN库存</returns>
        public virtual List<LotLpnOnhand> GetLotLpnOnhandByOnhandDatas(List<double> warehouseIds, List<OnhandData> onhandDataList)
        {
            List<double> itemIdList = onhandDataList.Select(p => p.ItemId).Distinct().ToList();

            var query = Query<LotLpnOnhand>().Where(p => itemIdList.Contains(p.ItemId) && warehouseIds.Contains(p.WarehouseId) && p.AvailableQty > 0);

            EntityList<LotLpnOnhand> lotLpnOnhandList = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            List<LotLpnOnhand> onhands = lotLpnOnhandList.Where(p =>
            onhandDataList.Any(q =>
            {
                if (q.ItemId != p.ItemId) return false;

                if (q.LocationId != p.StorageLocationId) return false;

                if (q.LotCode != p.LotCode) return false;

                if (q.Param.Lpn != p.Lpn) return false;

                if (q.Param.StorerCode != p.StorerCode) return false;

                if (q.Param.ProjectNo != p.ProjectNo) return false;

                if (q.Param.TaskNo != p.TaskNo) return false;

                if (q.Param.State != p.State) return false;

                if (q.Param.ItemExtProp != p.ItemExtProp) return false;

                return true;
            })
            ).ToList();

            return onhands.ToList();
        }

        /// <summary>
        /// 获取库存中可用的LPN
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="areaId">库区Id</param>
        /// <param name="storageLocationId">库位Id</param>
        /// <param name="pagingInfo">分页对象</param>
        /// <param name="keyword">关键字</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <returns>返回Lpn数据</returns>
        public virtual EntityList<LotLpnOnhand> GetOnHandLpn(double warehouseId, double itemId,
            double? areaId, double? storageLocationId, string itemExtProp, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<LotLpnOnhand>().Where(p => p.WarehouseId == warehouseId && p.ItemId == itemId &&
                            p.AvailableQty > 0 && p.ItemExtProp == itemExtProp);
            if (areaId.HasValue)
            {
                query = query.Join<StorageArea>((a, b) => a.WarehouseId == b.WarehouseId && b.Id == areaId);
            }

            if (storageLocationId.HasValue)
            {
                query = query.Where(p => p.StorageLocationId == storageLocationId.Value);
            }

            if (keyword.IsNotEmpty())
                query = query.Where(p => p.Lpn.Contains(keyword));

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            return query.ToList(pagingInfo, elo);
        }

        /// <summary>
        /// 获取库存信息
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="itemIds">物料ID集合</param>
        /// <param name="storerCodes">货主集合</param>
        /// <param name="locIds">库位ID集合</param>
        /// <param name="lpns">LPN集合</param>
        /// <param name="onhandStates">库存状态集合集合</param>
        /// <param name="projects">项目号集合</param>
        /// <param name="taskNos">任务号集合 </param>
        /// <param name="itemExtProps">物料扩展属性集合</param>
        /// <param name="lotCodes">批次号</param>
        /// <returns>包装库存</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhandByItemIds(double warehouseId, List<double> itemIds, List<string> storerCodes, List<double> locIds, List<string> lpns, List<OnhandState> onhandStates, List<string> projects, List<string> taskNos,
            List<string> itemExtProps, List<string> lotCodes)
        {
            var query = Query<LotLpnOnhand>().Where(p => p.AvailableQty + p.AllottedQty > 0 && p.State != OnhandState.None);
            if (warehouseId > 0)
                query.Where(p => p.WarehouseId == warehouseId);
            if (itemIds.Any())
                query.Where(p => itemIds.Contains(p.ItemId));
            if (storerCodes.Any())
                query.Where(p => storerCodes.Contains(p.StorerCode));
            if (locIds.Any())
                query.Where(p => locIds.Contains(p.StorageLocationId));
            if (lpns.Any())
                query.Where(p => lpns.Contains(p.Lpn));
            if (onhandStates.Any())
                query.Where(p => onhandStates.Contains(p.State));
            if (projects.Any())
                query.Where(p => projects.Contains(p.ProjectNo));
            if (taskNos.Any())
                query.Where(p => taskNos.Contains(p.TaskNo));
            if (itemExtProps.Any())
                query.Where(p => itemExtProps.Contains(p.ItemExtProp));
            if (lotCodes.Any())
                query.Where(p => lotCodes.Contains(p.LotCode));

            return query.ToList();
        }

        /// <summary>
        /// 获取库存信息
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="itemIds">物料ID集合</param>        
        /// <returns>库存列表</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhandByItemIds(double warehouseId, List<double> itemIds, EagerLoadOptions elo = null)
        {
            return itemIds.SplitContains(tempIds =>
            {
                var query = Query<LotLpnOnhand>().Where(p => p.AvailableQty > 0 && p.State != OnhandState.None)
                    .Where(p => p.WarehouseId == warehouseId)
                    .Where(p => tempIds.Contains(p.ItemId));
                return query.ToList(null, elo);
            });
        }

        /// <summary>
        /// 获取库存可用数
        /// </summary>
        /// <param name="warehouseIds"></param>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public virtual List<LotLpnOnhandDataInfo> GetLotLpnOnhandByItemIds(List<double> warehouseIds, List<double> itemIds)
        {
            List<LotLpnOnhandDataInfo> lotLpnOnhandDataInfos = new List<LotLpnOnhandDataInfo>();
            warehouseIds.SplitDataExecute(tempWareIds =>
            {
                itemIds.SplitDataExecute(tempItemIds =>
                {
                    var list = Query<LotLpnOnhand>().Where(p => p.AvailableQty > 0 && p.State != OnhandState.None
                    && tempWareIds.Contains(p.WarehouseId) && tempItemIds.Contains(p.ItemId))
                    .GroupBy(p => new {p.WarehouseId, p.ItemId, p.ItemExtProp})
                    .Select(p => new
                    {
                        WarehouseId = p.WarehouseId,
                        ItemId = p.ItemId,
                        ItemExtProp = p.ItemExtProp,
                        AvailableQty = p.AvailableQty.SUM()
                    }).ToList<LotLpnOnhandDataInfo>();
                    lotLpnOnhandDataInfos.AddRange(list);
                });
            });
            return lotLpnOnhandDataInfos;
        }

        /// <summary>
        /// 验证库存参数
        /// </summary>
        /// <param name="param">库存参数</param>
        private void ValidateOnhandParam(OnhandParam param)
        {
            if (param.ItemId <= 0)
                throw new ValidationException("获取库存失败，物料参数为空".L10N());

            ////if (param.ItemExtProp.IsNullOrEmpty())
            ////    throw new ValidationException("获取库存失败，物料扩展属性参数为空".L10N());

            if (param.Lot.IsNullOrEmpty())
                throw new ValidationException("获取库存失败，批次参数为空".L10N());

            if (param.LocId <= 0)
                throw new ValidationException("获取库存失败，库位参数为空".L10N());
        }

        /// <summary>
        /// 根据库位（排层列深度）获取LPN库存
        /// </summary>
        /// <param name="rowNo">排</param>
        /// <param name="layerNo">层</param>
        /// <param name="columnNo">列</param>
        /// <param name="depth">当前深度</param>
        /// <returns>LPN库存</returns>
        public virtual LotLpnOnhand GetLotLpnOnhandsByLoc(int rowNo, int layerNo, int columnNo, int depth)
        {
            var query = Query<LotLpnOnhand>()
                .Join<StorageLocation>((p, l) => p.StorageLocationId == l.Id && l.RowNo == rowNo &&
                l.LayerNo == layerNo && l.ColumnNo == columnNo && l.Depth == depth);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据巷道ID获取库位库存
        /// </summary>
        /// <param name="routewayId">巷道ID</param>
        /// <param name="depth">深度</param>
        /// <param name="elo">贪懒加载</param>
        /// <returns>库位库存</returns>
        public virtual EntityList<LotLpnOnhand> GetDepthLotLpnOnhands(double routewayId, int depth, EagerLoadOptions elo = null)
        {
            var query = Query<LotLpnOnhand>()
                .Join<StorageLocation>("l", (p, l) => p.StorageLocationId == l.Id && l.RoutewayId == routewayId && l.Depth == depth &&
                /*l.IsInLock && */!l.IsOutLock && !l.IsCountLock);

            if (elo == null)
                elo = new EagerLoadOptions().LoadWithViewProperty();
            query.Where(p => p.Qty > 0);
            return query.ToList(null, elo);
        }

        /// <summary>
        /// 根据库位ID获取库位库存（同排层列，深度不同）
        /// </summary>
        /// <param name="locId">库位ID</param>
        /// <param name="depth">深度</param>
        /// <returns>库位库存</returns>
        public virtual int GetDepthLotLpnOnhandCount(double locId, int depth)
        {
            var query = Query<LotLpnOnhand>()
                .Join<StorageLocation>("l", (p, l) => p.StorageLocationId == l.Id)
                .Join<StorageLocation, StorageLocation>("ll", (l, ll) => ll.AreaId == l.AreaId && ll.Id == locId &&
                ll.RowNo == l.RowNo && ll.ColumnNo == l.ColumnNo && ll.LayerNo == l.LayerNo && l.Depth == depth);
            query.Where(p => p.Qty > 0);
            return query.Count();
        }

        /// <summary>
        /// 根据库位id集合获取库存
        /// </summary>
        /// <param name="locIdList">库位Id</param>
        /// <returns>库存</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhandByLocIds(List<double> locIdList, EagerLoadOptions elo = null)
        {
            if (locIdList == null || locIdList.Count == 0)
            {
                return new EntityList<LotLpnOnhand>();
            }
            return locIdList.SplitContains(sons =>
            {
                return Query<LotLpnOnhand>().Where(p => p.Qty > 0).Where(i => sons.Contains(i.StorageLocationId)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 根据LPN获取批次和LPN库存
        /// </summary>
        /// <param name="lpnList">LPN集合</param>
        /// <returns></returns>
        public virtual List<string> GetLotLpnOnhandByLpns(List<string> lpnList)
        {
            var lpns = lpnList.Distinct();
            return DataProcessEx.SplitContains(lpns, tempLpns =>
            {
                var onhands = Query<LotLpnOnhand>().Where(p => p.Qty > 0).Where(p => tempLpns.Contains(p.Lpn)).ToList();

                return onhands.Select(p => p.Lpn).Distinct().ToList();
            });
        }

        /// <summary>
        /// 根据LPN获取批次和LPN库存
        /// </summary>
        /// <param name="lpnList">LPN集合</param>
        /// <returns></returns>
        public virtual List<LotLpnOnhand> GetLotLpnOnhandDatasByLpns(List<string> lpnList, EagerLoadOptions elo = null)
        {
            var lpns = lpnList.Distinct();
            return DataProcessEx.SplitContains(lpns, tempLpns =>
            {
                var onhands = Query<LotLpnOnhand>().Where(p => p.Qty > 0).Where(p => tempLpns.Contains(p.Lpn)).ToList(null, elo);

                return onhands.ToList();
            });
        }

        /// <summary>
        /// 获取深浅库位库存
        /// </summary>
        /// <param name="locIds">库位ID</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>深浅库位库存</returns>
        public virtual EntityList<LotLpnOnhand> GetDepthShallowLocOnhands(List<double> locIds, EagerLoadOptions elo = null)
        {
            if (locIds == null || locIds.Count == 0)
            {
                return new EntityList<LotLpnOnhand>();
            }
            return locIds.SplitContains(sons =>
            {
                var query = Query<LotLpnOnhand>();
                query.Join<StorageLocation>("l", (p, l) => p.StorageLocationId == l.Id);
                query.Join<StorageLocation, StorageLocation>("ll", (l, ll) => ll.Id != l.Id && ll.RoutewayId == l.RoutewayId && ll.RowNo == l.RowNo && ll.LayerNo == l.LayerNo && ll.ColumnNo == l.ColumnNo);
                query.Where<StorageLocation, StorageLocation>((p, l, ll) => p.Qty > 0 && sons.Contains(ll.Id));
                return query.ToList(null, elo);
            });
        }

        /// <summary>
        /// 获取LPN库存
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="itemCode">物料编码</param>
        /// <param name="itemName">物料名称</param>
        /// <param name="locCode">库位编码</param>
        /// <param name="lotCode">批次号</param>
        /// <param name="storerCode">货主</param>
        /// <param name="lpn">lpn</param>
        /// <param name="projectNo">项目号</param>
        /// <param name="taskNo">任务号</param>
        /// <param name="onhandState">库存状态</param>
        /// <param name="isIgnoreItemExtProp">是否忽略物料扩展属性</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <param name="pagInfo">分页信息</param>
        /// <returns>批次LPN库存</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhandsBySo(double? warehouseId, string itemCode, string itemName, string locCode, string lotCode,
            string storerCode, string lpn, string projectNo, string taskNo, OnhandState? onhandState, bool? isIgnoreItemExtProp, string itemExtProp, PagingInfo pagInfo
            , bool isPick)
        {
            var query = DB.Query<LotLpnOnhand>("i").Where(p => p.AvailableQty > 0 && p.State != OnhandState.None);
            query.Join<Item>((x, i) => x.ItemId == i.Id);
            query.Join<StorageLocation>((x, s) => x.StorageLocationId == s.Id && !s.IsOutLock && !s.IsCountLock);
            if (isPick)
                query.Join<StorageLocationOperation>((a, b) => a.StorageLocationId == b.StorageLocationId && b.IsPick);
            if (warehouseId.HasValue)
                query.Where(p => p.WarehouseId == warehouseId);
            if (itemCode.IsNotEmpty())
                query.Where<Item>((x, i) => i.Code.Contains(itemCode));
            if (itemName.IsNotEmpty())
                query.Where<Item>((x, i) => i.Name.Contains(itemName));
            if (locCode.IsNotEmpty())
                query.Where<StorageLocation>((x, s) => s.Code.Contains(locCode));
            if (lotCode.IsNotEmpty())
                query.Where(p => p.LotCode.Contains(lotCode));
            if (lpn.IsNotEmpty())
                query.Where(p => p.Lpn == lpn);
            if (!storerCode.IsNullOrEmpty())
                query.Where(p => p.StorerCode == storerCode);
            if (!projectNo.IsNullOrEmpty())
                query.Where(p => p.ProjectNo == projectNo);
            if (!taskNo.IsNullOrEmpty())
                query.Where(p => p.TaskNo == taskNo);
            if (onhandState.HasValue)
                query.Where(p => p.State == onhandState.Value);
            if (isIgnoreItemExtProp.HasValue && !isIgnoreItemExtProp.Value)
            {
                query.Where(p => p.ItemExtProp == itemExtProp);
            }
            return query.ToList(pagInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取LPN库存
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="itemCode">物料编码</param>
        /// <param name="itemName">物料名称</param>
        /// <param name="locCode">库位编码</param>
        /// <param name="lotCode">批次号</param>
        /// <param name="storerCode">货主</param>
        /// <param name="lpn">lpn</param>
        /// <param name="projectNo">项目号</param>
        /// <param name="taskNo">任务号</param>
        /// <param name="onhandState">库存状态</param>
        /// <param name="isIgnoreItemExtProp">是否忽略物料扩展属性</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <param name="pagInfo">分页信息</param>
        /// <returns>批次LPN库存</returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhandsBySoAssign(double? warehouseId, string itemCode, string itemName, string locCode, string lotCode,
            string storerCode, string lpn, string projectNo, string taskNo, OnhandState? onhandState, bool? isIgnoreItemExtProp, string itemExtProp, PagingInfo pagInfo)
        {
            var query = Query<LotLpnOnhand>().Where(p => (p.AvailableQty > 0 || (p.AvailableQty == 0 && p.AllottedQty > 0)) && p.State != OnhandState.None);
            query.Join<Item>((x, i) => x.ItemId == i.Id);
            query.Join<StorageLocation>((x, s) => x.StorageLocationId == s.Id && !s.IsOutLock && !s.IsCountLock);
            if (warehouseId.HasValue)
                query.Where(p => p.WarehouseId == warehouseId);
            if (itemCode.IsNotEmpty())
                query.Where<Item>((x, i) => i.Code.Contains(itemCode));
            if (itemName.IsNotEmpty())
                query.Where<Item>((x, i) => i.Name.Contains(itemName));
            if (locCode.IsNotEmpty())
                query.Where<StorageLocation>((x, s) => s.Code.Contains(locCode));
            if (lotCode.IsNotEmpty())
                query.Where(p => p.LotCode.Contains(lotCode));
            if (lpn.IsNotEmpty())
                query.Where(p => p.Lpn == lpn);
            if (!storerCode.IsNullOrEmpty())
                query.Where(p => p.StorerCode == storerCode);
            if (!projectNo.IsNullOrEmpty())
                query.Where(p => p.ProjectNo == projectNo);
            if (!taskNo.IsNullOrEmpty())
                query.Where(p => p.TaskNo == taskNo);
            if (onhandState.HasValue)
                query.Where(p => p.State == onhandState.Value);
            if (isIgnoreItemExtProp.HasValue && !isIgnoreItemExtProp.Value)
            {
                query.Where(p => p.ItemExtProp == itemExtProp);
            }

            return query.ToList(pagInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 补货规则获取仓库下的所有
        /// </summary>
        /// <param name="warehouseIds">仓库ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="ItemExtProp">扩展属性</param>
        /// <param name="storeCode">货主</param>
        /// <returns></returns>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhandsByReplenish(List<double> warehouseIds, double itemId, string ItemExtProp, string storeCode)
        {
            return warehouseIds.SplitContains(sons =>
            {
                var query = Query<LotLpnOnhand>();
                query.Where(p => p.StorerCode == storeCode && p.ItemId == itemId && p.ItemExtProp == ItemExtProp && sons.Contains(p.WarehouseId));
                query.Where(p => p.AvailableQty > 0);
                return query.ToList(null);
            });
        }

        /// <summary>
        /// 获取可用数
        /// </summary>
        /// <param name="param">库存查询参数</param>
        /// <returns>可用数</returns>
        public virtual decimal GetLotLpnOnhandAvaQty(OnhandParam param)
        {
            var query = Query<LotLpnOnhand>();

            if (param.LocId > 0)
                query.Where(p => p.StorageLocationId == param.LocId);
            if (param.ItemId > 0)
                query.Where(p => p.ItemId == param.ItemId);
            if (!param.Lot.IsNullOrWhiteSpace())
                query.Join<Lot>((x, y) => x.LotId == y.Id && y.Code == param.Lot);
            if (param.Lpn.IsNotEmpty())
                query.Where(p => p.Lpn == param.Lpn);
            if (param.StorerCode.IsNotEmpty())
                query.Where(p => p.StorerCode == param.StorerCode);
            if (param.ProjectNo.IsNotEmpty())
                query.Where(p => p.ProjectNo == param.ProjectNo);
            if (param.TaskNo.IsNotEmpty())
                query.Where(p => p.TaskNo == param.TaskNo);
            if (param.WarehouseId > 0)
                query.Where(p => p.WarehouseId == param.WarehouseId);
            if (!param.ItemExtPropName.IsNullOrWhiteSpace())
                query.Where(p => p.ItemExtPropName == param.ItemExtPropName);
            return query.ToList().Sum(p => p.AvailableQty);
        }

        /// <summary>
        /// 检查LPN是否在库位
        /// </summary>
        /// <param name="lpn">lpn</param>
        /// <param name="locId">库位Id</param>
        /// <returns>bool</returns>
        public virtual bool CheckOnhandByLpnLoc(string lpn, double locId)
        {
            return Query<LotLpnOnhand>().Where(p => p.Lpn == lpn && p.StorageLocationId == locId).Count() > 0;
        }

        /// <summary>
        /// 按LPN发货获取库存数据
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="keywords">关键字查询维度</param>
        /// <returns>库存数据</returns>
        public virtual EntityList<LotLpnOnhand> GetLocItemLotLpnOnhands(double warehouseId, BaseKeywordData keywords)
        {
            var query = Query<LotLpnOnhand>().Where(p => p.WarehouseId == warehouseId && p.Qty > 0);
            if (!string.IsNullOrEmpty(keywords.LocationCode))
                query.Join<StorageLocation>((a, b) => a.StorageLocationId == b.Id && b.State == State.Enable && !b.IsFrozen &&
                (b.Code.Contains(keywords.LocationCode) || b.Name.Contains(keywords.LocationCode)));

            if (!string.IsNullOrEmpty(keywords.LpnCode))
                query.Where(p => p.Lpn.Contains(keywords.LpnCode));

            if (!string.IsNullOrEmpty(keywords.StorerCode))
                query.Where(p => p.StorerCode.Contains(keywords.StorerCode));

            if (!string.IsNullOrEmpty(keywords.ProjectNo))
                query.Where(p => p.ProjectNo.Contains(keywords.ProjectNo));

            if (!string.IsNullOrEmpty(keywords.TaskNo))
                query.Where(p => p.TaskNo.Contains(keywords.TaskNo));

            if (!string.IsNullOrEmpty(keywords.ItemCode))
                query.Join<Item>((a, i) => a.ItemId == i.Id && (i.Code.Contains(keywords.ItemCode) || i.Name.Contains(keywords.ItemCode)));

            if (!string.IsNullOrEmpty(keywords.LotCode))
                query.Where(p => p.LotCode.Contains(keywords.LotCode));

            if (!string.IsNullOrEmpty(keywords.ItemExtPropName))
            {
                query.Where(p => p.ItemExtPropName.Contains(keywords.ItemExtPropName));
            }

            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 标签生成（按库存）获取库存数据
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="keywords">关键字查询维度</param>
        /// <returns>库存数据</returns>
        public virtual EntityList<LotLpnOnhand> GetLabelLocItemLotLpnOnhands(double warehouseId, BaseKeywordData keywords)
        {
            var query = Query<LotLpnOnhand>().Where(p => p.WarehouseId == warehouseId && p.Qty > 0);
            if (!string.IsNullOrEmpty(keywords.LocationCode))
                query.Join<StorageLocation>((a, b) => a.StorageLocationId == b.Id && b.State == State.Enable && !b.IsFrozen &&
                (b.Code.Contains(keywords.LocationCode) || b.Name.Contains(keywords.LocationCode)));

            if (!string.IsNullOrEmpty(keywords.LpnCode))
                query.Where(p => p.Lpn.Contains(keywords.LpnCode));

            if (!string.IsNullOrEmpty(keywords.StorerCode) && keywords.StorerCode != "*")
                query.Where(p => p.StorerCode.Contains(keywords.StorerCode));

            if (!string.IsNullOrEmpty(keywords.ProjectNo) && keywords.ProjectNo != "*")
                query.Where(p => p.ProjectNo.Contains(keywords.ProjectNo));

            if (!string.IsNullOrEmpty(keywords.TaskNo) && keywords.TaskNo != "*")
                query.Where(p => p.TaskNo.Contains(keywords.TaskNo));

            if (!string.IsNullOrEmpty(keywords.ItemCode))
                query.Join<Item>((a, i) => a.ItemId == i.Id && (i.Code.Contains(keywords.ItemCode) || i.Name.Contains(keywords.ItemCode)));

            if (!string.IsNullOrEmpty(keywords.LotCode))
                query.Where(p => p.LotCode.Contains(keywords.LotCode));

            if (!string.IsNullOrEmpty(keywords.ItemExtPropName))
            {
                query.Where(p => p.ItemExtPropName.Contains(keywords.ItemExtPropName));
            }

            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 选择库存添加
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>库存添加ViewModel</returns>
        public virtual EntityList<BaseSelectViewModel> GetSelectOnhandViewModels(SelectOnhandCriteria criteria)
        {
            EntityList<BaseSelectViewModel> selectOnhands = new EntityList<BaseSelectViewModel>();
            var onhands = SetLotLpnOnhands(criteria);
            if (!onhands.Any())
            {
                return selectOnhands;
            }
            var itemIds = onhands.Select(a => a.ItemId).Distinct().ToList();
            var itemUnits = RT.Service.Resolve<ItemUnitController>().GetAllItemUnits(itemIds);
            var units = RT.Service.Resolve<ItemController>().GetUnitList(onhands.Select(a => a.ItemUnitId.Value).Distinct().ToList());
            foreach (var onhand in onhands)
            {
                var data = new BaseSelectViewModel
                {
                    AvailableQty = onhand.AvailableQty,
                    ItemId = onhand.ItemId,
                    ItemCode = onhand.ItemCode,
                    ItemName = onhand.ItemName,
                    ItemExtProp = onhand.ItemExtProp,
                    ItemExtPropName = onhand.ItemExtPropName,
                    ItemUnitName = onhand.ItemUnit,
                    StorageLocationId = onhand.StorageLocationId,
                    LocCode = onhand.StorageLocationCode,
                    Lpn = onhand.Lpn,
                    LotId = onhand.LotId,
                    LotCode = onhand.LotCode,
                    OnhandState = onhand.State,
                    StorerCode = onhand.StorerCode,
                    ProjectNo = onhand.ProjectNo,
                    TaskNo = onhand.TaskNo,
                    LotAtt01 = onhand.LotAtt01,
                    LotAtt02 = onhand.LotAtt02,
                    LotAtt03 = onhand.LotAtt03,
                    LotAtt04 = onhand.LotAtt04,
                    LotAtt05 = onhand.LotAtt05,
                    Qty = onhand.Qty,
                    AllottedQty = onhand.AllottedQty,
                    FreezingQty = onhand.FreezingQty,
                    CreateBy = onhand.CreateByName,
                    CreateDate = onhand.CreateDate,
                    UpdateBy = onhand.UpdateByName,
                    UpdateDate = onhand.UpdateDate,
                    OnhandId = onhand.Id,
                    ItemUnitId = onhand.ItemUnitId.Value,
                    MainPrecision = onhand.UnitPrecision ?? 3,
                    SecondPrecision = onhand.SecondUnitPrecision ?? 3,
                    MainTrade = onhand.MainTrade,
                };

                var itemUnit = itemUnits.FirstOrDefault(a => a.ItemId == onhand.ItemId && onhand.SecondUnitId == a.UnitId ||
               a.MainUnitId == onhand.ItemUnitId.Value && a.UnitId == onhand.SecondUnitId && a.IsBaseUnit);
                if (itemUnit != null)
                {
                    data.SecondUnit = itemUnit.Unit;
                    data.ConvertFigre = itemUnit.GetConvertFigre();
                    data.Denominator = itemUnit.Denominator;
                    data.Numerator = itemUnit.Numerator;
                    data.SecondUnitName = itemUnit.UnitName;
                    data.SecondPrecision = itemUnit.SecondUnitPrecision ?? 3;
                }
                else
                {
                    data.SecondUnit = units.FirstOrDefault(a => a.Id == onhand.ItemUnitId);
                    data.ConvertFigre = 1;
                    data.Denominator = 1;
                    data.Numerator = 1;
                    data.SecondUnitName = onhand.SecondUnitName;
                    data.SecondPrecision = data.SecondUnit.Precision ?? 3;
                }
                data.SecondTrade = data.SecondUnit.TradeType;
                if (criteria.AutoExpectQty == true)
                {
                    data.ExpectQty = onhand.AvailableQty;
                }

                selectOnhands.Add(data);
            }

            selectOnhands.SetTotalCount(onhands.TotalCount > 0 ? onhands.TotalCount : onhands.Count);

            return selectOnhands;
        }

        /// <summary>
        /// 获取库存数据
        /// </summary>
        /// <param name="critreia">查询实体</param>
        /// <returns>库存数据</returns>
        private EntityList<LotLpnOnhand> SetLotLpnOnhands(SelectOnhandCriteria critreia)
        {
            return GetLotLpnOnhandsBySo(critreia.WarehouseId, critreia.ItemCode, critreia.ItemName, critreia.LocCode, critreia.LotCode, critreia.ShipperCode, critreia.Lpn, critreia.ProjectNo, critreia.TaskNo, critreia.OnhandState, critreia.IsIgnoreItemExtProp,
                critreia.ItemExtProp, critreia.PagingInfo, false);
        }

        /// <summary>
        /// 获取库存中可用的LPN
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="areaId">库区Id</param>
        /// <param name="storageLocationId">库位Id</param>
        /// <param name="pagingInfo">分页对象</param>
        /// <param name="keyword">关键字</param>
        /// <param name="storerCode">货主</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <param name="isIgnoreItemExtProp">是否忽略物料扩展属性</param>
        /// <returns>返回Lpn数据</returns>
        public virtual EntityList<LotLpnOnhand> GetOnHandLpn(double warehouseId, double itemId,
            double? areaId, double? storageLocationId, PagingInfo pagingInfo, string keyword, string storerCode = "", string itemExtProp = "", bool? isIgnoreItemExtProp = false)
        {
            var query = Query<LotLpnOnhand>().Where(p => p.WarehouseId == warehouseId && p.ItemId == itemId &&
                            p.AvailableQty > 0 && p.Lpn.Contains(keyword));
            if (areaId.HasValue)
            {
                query = query.Join<StorageArea>((a, b) => a.WarehouseId == b.WarehouseId && b.Id == areaId);
            }

            if (storageLocationId.HasValue)
            {
                query = query.Where(p => p.StorageLocationId == storageLocationId.Value);
            }

            if (!string.IsNullOrEmpty(storerCode))
            {
                query = query.Where(p => p.StorerCode == storerCode);
            }

            if (isIgnoreItemExtProp.HasValue && !isIgnoreItemExtProp.Value)
            {
                query.Where(p => p.ItemExtProp == itemExtProp);
            }

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            return query.ToList(pagingInfo, elo);
        }

        /// <summary>
        /// 获取库存中的批次
        /// </summary>
        /// <param name="itemIds">物料Id</param>
        /// <returns>批次</returns>
        public virtual EntityList<Lot> GetOnhandLots(List<double?> itemIds)
        {
            EntityList<Lot> rst = new EntityList<Lot>();
            itemIds.SplitDataExecute(ids =>
            {
                rst.AddRange(Query<Lot>().Where(p => ids.Contains(p.ItemId)).
                Exists<LotLpnOnhand>((x, y) => y.Where(p => p.LotId == x.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
            });
            return rst;
        }

        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhands(List<double> warehouseIds, List<double> itemIds)
        {
            EntityList<LotLpnOnhand> rst = new EntityList<LotLpnOnhand>();

            itemIds.SplitDataExecute(ids =>
             {
                 rst.AddRange(Query<LotLpnOnhand>().Where(p => warehouseIds.Contains(p.WarehouseId) && ids.Contains(p.ItemId)).ToList());
             });
            return rst;
        }

        
    }

}
