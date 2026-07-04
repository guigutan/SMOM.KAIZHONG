using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Enums;
using SIE.EventMessages.EMS.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.FixtureTypes;
using SIE.Fixtures.InboundOrders;
using SIE.Fixtures.Models;
using SIE.Fixtures.Models.Config;
using SIE.Items;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Fixtures
{
    /// <summary>
    /// 工治具型号控制器
    /// </summary>
    public partial class CoreFixtureController
    {
        #region 工治具型号

        /// <summary>
        /// 获取是否存在相同的设备型号
        /// </summary>
        /// <param name="fixtureModelEquipDetail"></param>
        /// <returns></returns>
        public virtual bool IsExsitedFixtureModelEquipDetail(FixtureModelEquipDetail fixtureModelEquipDetail)
        {
            return Query<FixtureModelEquipDetail>().Where(m => m.EquipModelId == fixtureModelEquipDetail.EquipModelId
             && m.FixtureModelId == fixtureModelEquipDetail.FixtureModelId && m.Id != fixtureModelEquipDetail.Id).FirstOrDefault() != null;
        }



        /// <summary>
        /// 获取是否存在相同的保养项目
        /// </summary>
        /// <param name="fixtureModelMaintainProject"></param>
        /// <returns></returns>
        public virtual bool IsExsitedFixtureModelMaintainProject(FixtureModelMaintainProject fixtureModelMaintainProject)
        {
            return Query<FixtureModelMaintainProject>().Where(m => m.MaintainProjectId == fixtureModelMaintainProject.MaintainProjectId
             && m.FixtureModelId == fixtureModelMaintainProject.FixtureModelId && m.Id != fixtureModelMaintainProject.Id).FirstOrDefault() != null;
        }


        /// <summary>
        /// 获取编码
        /// </summary>
        /// <returns></returns>
        public virtual string GetCode()
        {
            var config = ConfigService.GetConfig(new FixturesModelsConfig(), typeof(FixtureModel));

            if (config == null || config.Number == null)
            {
                return "";
            }

            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.Number.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 通过查询条件获取工治具型号列表
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns>工治具型号列表</returns>
        public virtual EntityList<FixtureModel> GetFixtureModelList(FixtureModelCriteria criteria)
        {
            var query = Query<FixtureModel>();
            if (criteria.Code.IsNotEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (criteria.Name.IsNotEmpty())
                query.Where(p => p.Name.Contains(criteria.Name));
            if (criteria.ManageMode.HasValue)
                query.Where(p => p.ManageMode == criteria.ManageMode);
            if (criteria.FixtureTypeId.HasValue)
                query.Where(p => p.FixtureTypeId == criteria.FixtureTypeId);
            if (!criteria.FixtureTypes.IsNullOrEmpty())
            {

                var replaceStr = criteria.FixtureTypes.Replace(";", ",");
                query.Join<FixtureType>((x, y) => x.FixtureTypeId == y.Id && replaceStr.Contains(y.Code));
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具类型获取工治具型号列表
        /// </summary>
        /// <param name="fixtureType">工治具类型</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>工治具型号列表</returns>
        public virtual EntityList<FixtureModel> GetFixtureModels(SIE.Fixtures.FixtureTypes.FixtureType fixtureType, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<FixtureModel>();
            if (fixtureType != null)
                query.Where(p => p.FixtureTypeId == fixtureType.Id);
            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过工治具型号Id列表获取工治具型号保养项目列表
        /// </summary>
        /// <param name="fixtureModelIds">工治具型号Id列表</param>
        /// <returns>工治具型号保养项目列表</returns>
        public virtual EntityList<FixtureModelMaintainProject> GetFixtureModelProjectsByIds(List<double> fixtureModelIds)
        {
            return Query<FixtureModelMaintainProject>().Where(p => fixtureModelIds.Contains(p.FixtureModelId)).ToList();
        }

        /// <summary>
        /// 通过工治具型号Id获取工治具型号保养项目列表(加载视图属性)
        /// </summary>
        /// <param name="fixtureModelId">工治具型号Id</param>
        /// <returns>工治具型号保养项目列表</returns>
        public virtual EntityList<FixtureModelMaintainProject> GetFixtureModelProjectsByModelId(double fixtureModelId)
        {
            return Query<FixtureModelMaintainProject>().Where(p => p.FixtureModelId == fixtureModelId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具型号Id列表获取工治具型号设备清单列表
        /// </summary>
        /// <param name="modelIds">工治具型号Id列表</param>
        /// <returns>工治具型号设备清单列表</returns>
        public virtual EntityList<FixtureModelEquipDetail> GetFixtureModelEquipDetails(List<double> modelIds)
        {
            return Query<FixtureModelEquipDetail>().Where(p => modelIds.Contains(p.FixtureModelId)).ToList();
        }

        /// <summary>
        /// 是否存在上线定期保养项目
        /// </summary>
        /// <param name="fixtureModelId">工治具型号ID</param>
        /// <param name="modifiedIds">界面修改的ID</param>
        /// <returns>存在返回true，不存在返回false</returns>
        public virtual bool IsExistModelOnlineMaintains(double fixtureModelId, List<double> modifiedIds)
        {
            return Query<FixtureModelMaintainProject>().Where(p => p.FixtureModelId == fixtureModelId && p.OnlineMaintain
            && !modifiedIds.Contains(p.Id)).Count() > 0;
        }
        #endregion

        #region 存储位置
        /// <summary>
        /// 根据仓库Id获取库位列表
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>库位列表</returns>
        public virtual EntityList<StorageLocation> GetFixtureStorageLocations(double warehouseId, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<StorageLocation>().Where(p => p.WarehouseId == warehouseId);
            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 根据工治具Id获取存储位置列表
        /// </summary>
        /// <param name="code">工治具Id</param>
        /// <returns>存储位置列表</returns>
        public virtual EntityList<FixtureEncodeStorageLocation> GetFixtureEncodeStorageLocations(string code)
        {
            return Query<FixtureEncodeStorageLocation>().Exists<FixtureEncode>(
                      (x, y) => y.Join<FixtureAccount>((c, d) => c.Id == d.FixtureEncodeId && (c.Code==code||d.Code==code))
                          .Where(p => p.Id == x.FixtureEncodeId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取存储位置列表
        /// </summary>
        /// <param name="fixtureEncodeId">存储位置列表</param>
        /// <returns></returns>
        public virtual EntityList<FixtureEncodeStorageLocation> GetFixtureEncodeStorageLocationsByEncodeId( double fixtureEncodeId)
        {
            return Query<FixtureEncodeStorageLocation>().Exists<FixtureEncode>(
                      (x, y) => y.Join<FixtureAccount>((c, d) => c.Id == d.FixtureEncodeId && c.Id == fixtureEncodeId)
                          .Where(p => p.Id == x.FixtureEncodeId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        #endregion

        #region 工治具编码

        /// <summary>
        /// 通过查询条件获取工治具编码列表
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns>工治具编码列表</returns>
        public virtual EntityList<FixtureEncode> QueryFixtureEncodeList(FixtureEncodeCriteria criteria)
        {
            var query = Query<FixtureEncode>();
            if (criteria.Code.IsNotEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (criteria.FixtureModelName.IsNotEmpty() || criteria.ManageMode.HasValue || criteria.FixtureTypeId.HasValue)
            {
                query.Exists<FixtureModel>((x, y) => y.Where(w => w.Id == x.FixtureModelId)
                .WhereIf(!string.IsNullOrEmpty(criteria.FixtureModelName), w => w.Name.Contains(criteria.FixtureModelName))
                .WhereIf(criteria.ManageMode.HasValue, w => w.ManageMode == criteria.ManageMode)
                .WhereIf(criteria.FixtureTypeId.HasValue, w => w.FixtureTypeId == criteria.FixtureTypeId));
            }
            if (!criteria.FixtureTypes.IsNullOrEmpty()&& criteria.FixtureModelName.IsNullOrEmpty())
            {
                query.Where(m => criteria.FixtureTypes.Contains(m.FixtureModel.FixtureType.Code));
            }

            var resultList = query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            if (resultList.Any())
            {
                var fixtureEncodeIds = resultList.Select(m => m.Id).ToList();
                //获取当前查询记录的工治具编码对应的工治具台账
                var fixtureAccounts = fixtureEncodeIds.SplitContains(ids =>
                 {
                     return Query<FixtureAccount>().Where(m => ids.Contains(m.FixtureEncodeId)).ToList();
                 });

                var inboundOrders = fixtureEncodeIds.SplitContains(ids =>
                {
                    //获取单据类型为采购入库的工治具待入库单据
                    return Query<InboundOrder>().Where(m => ids.Contains(m.FixtureEncodeId) && m.InboundStatus == InboundStatus.ToBe
                    && (m.InboundType == FixtureInboundType.Po
                    || m.InboundType == FixtureInboundType.Gift
                    || m.InboundType == FixtureInboundType.Guest
                    || m.InboundType == FixtureInboundType.Lease)
                    ).ToList();
                });
                //通过接口获取工治具编码相关待验收数据
                var fixtureAcceptanceInfos = RT.Service.Resolve<IFixtureAcceptance>().GetTobeAcceptanceInfos(fixtureEncodeIds);
                //遍历结果集合的工治具编码统计 
                resultList.ForEach(item =>
                {
                    var itemFixtureAccounts = fixtureAccounts.Where(m => m.FixtureEncodeId == item.Id).ToList();
                    if (item.ManageMode == ManageMode.Code)
                    {
                        //工治具台账同工治具编码的数据总数量汇总
                        item.TotalNum = itemFixtureAccounts.Sum(p => p.TotalQty);
                        //编码类：在库数 = 工治具台账同工治具编码在库数 报废数 = 工治具台账同工治具编码报废数
                        item.ScrapNum = itemFixtureAccounts.Sum(m => m.ScrapQty);
                        item.InWarehouseNum = itemFixtureAccounts.Sum(m => m.InStockQty);
                    }
                    if (item.ManageMode == ManageMode.Number)
                    {
                        item.TotalNum = itemFixtureAccounts.Count;
                        //ID类：工治具台账同工治具编码且状态为在库的数据汇总  ScrapNum:工治具台账同工治具编码且状态为报废的数据汇总
                        item.ScrapNum = itemFixtureAccounts.Count(m => m.AccountState == FixtureAccountState.Scrap || m.AccountState == FixtureAccountState.Disposal);
                        item.InWarehouseNum = itemFixtureAccounts.Count(m => m.AccountState == FixtureAccountState.InStorage);
                    }
                    item.CanUseNum = item.TotalNum - item.ScrapNum;

                    //待验收入库数: 工治具验收功能中同治具编码状态为待验收的数据汇总+工治具入库同治具编码且单据类型为采购入库的待入库单据计划数之和
                    //待入库的采购订单数量
                    var tobeInbounQty = inboundOrders.Where(m => m.FixtureEncodeId == item.Id).Sum(m => m.Qty);
                    //待验收数量
                    var fixtureAcceptanceQty = fixtureAcceptanceInfos.Where(m => m.FixtureEncodeId == item.Id).Sum(m => m.PassQty);
                    item.AcceptedInWHNum = (int)tobeInbounQty + fixtureAcceptanceQty;
                });
            }
            return resultList;
        }

        /// <summary>
        /// 根据管理方式获取工治具编码列表
        /// </summary>
        /// <param name="mode">管理方式</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="pageInfo">分页</param>
        /// <returns>工治具编码列表</returns>
        public virtual EntityList<FixtureEncode> GetFixtureEncodes(ManageMode mode, string keyWord, PagingInfo pageInfo)
        {
            var query = Query<FixtureEncode>().Exists<FixtureModel>((a, b) => b.Where(f => f.Id == a.FixtureModelId && f.ManageMode == mode));
            if (keyWord.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyWord));
            }
            return query.ToList(pageInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 资产处理获取对应仓库的有报废数量的工治具
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="warehouseId"></param>
        /// <param name="keyWord"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public virtual EntityList<FixtureEncode> GetFixtureEncodes(ManageMode mode, double? warehouseId, string keyWord, PagingInfo pageInfo)
        {
            var query = Query<FixtureEncode>()
                .Exists<FixtureModel>((a, b) => b.Where(f => f.Id == a.FixtureModelId && f.ManageMode == mode))
                .Exists<FixtureAccountStock>((a, b) => b.Where(p => p.FixtureAccount.FixtureEncodeId == a.Id && p.ScrapQty > 0).WhereIf(warehouseId != null, p => p.FixtureWarehouseId == warehouseId));
            if (keyWord.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyWord));
            }
            return query.ToList(pageInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具类型和工治具型号Id获取工治具编码列表
        /// </summary>
        /// <param name="fixtureType">工治具类型</param>
        /// <param name="modelId">工治具型号Id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>工治具编码列表</returns>
        public virtual EntityList<FixtureEncode> GetFixtureEncodes(FixtureType fixtureType, double? modelId, PagingInfo pagingInfo, string keyword)
        {
            if (fixtureType != null)
            {

                var query = Query<FixtureEncode>().Exists<FixtureModel>((a, b) => b.Where(f => f.Id == a.FixtureModelId && f.FixtureTypeId == fixtureType.Id));
                if (modelId.HasValue)
                    query.Where(p => p.FixtureModelId == modelId);
                if (keyword.IsNotEmpty())
                    query.Where(p => p.Code.Contains(keyword));
                return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
            else
            {
                return new EntityList<FixtureEncode>();
            }
        }

        /// <summary>
        /// 获取工治具编码列表
        /// </summary>
        /// <param name="keyWord">关键字</param>
        /// <param name="pageInfo">分页</param>
        /// <returns>工治具编码列表</returns>
        public virtual EntityList<FixtureEncode> GetFixtureEncodes(string keyWord, PagingInfo pageInfo)
        {
            var list = Query<FixtureEncode>().WhereIf(keyWord.IsNotEmpty(), p => p.Code.Contains(keyWord) || p.FixtureModel.Name.Contains(keyWord))
                                             .ToList(pageInfo, new EagerLoadOptions().LoadWithViewProperty());

            return list;
        }

        /// <summary>
        /// 根据仓库和工治具编码获取有可用库存数的工治具编码
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="encodeIds">工治具编码Id集合</param>
        /// <returns>有可用库存数的工治具编码列表</returns>
        public virtual IList<FixtureEncode> GetCanUseNumByWarehouseId(double warehouseId, IList<double> encodeIds)
        {
            var encodeList = Query<FixtureAccountStock>()
                                .Where(p => p.FixtureWarehouseId == warehouseId && encodeIds.Contains(p.FixtureAccount.FixtureEncodeId) 
                                        && (p.FixtureAccount.AccountState == FixtureAccountState.InStorage || p.FixtureAccount.AccountState == null))
                                .GroupBy(p => new
                                {
                                    p.FixtureAccount.FixtureEncodeId,
                                })
                                .Select(p => new
                                {
                                    Id = p.FixtureAccount.FixtureEncodeId,
                                    CanUseNum = p.PassQty.SUM() + p.NgQty.SUM()
                                }).ToList<FixtureEncode>();
            return encodeList;
        }

        /// <summary>
        /// 根据仓库和工治具编码获取有可用库存数的库位详情
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="encodeIds">工治具编码Id集合</param>
        /// <param name="locationIds">库位Id集合</param>
        /// <returns>有可用库存数的库位详情列表</returns>
        public virtual IList<FixtureAccountStock> GetCanUseNumByWarehouseId(double warehouseId, IList<double> encodeIds,IList<double> locationIds)
        {
            var stockList = Query<FixtureAccountStock>()
                                .Where(p => p.FixtureWarehouseId == warehouseId && encodeIds.Contains(p.FixtureAccount.FixtureEncodeId)
                                        && (p.FixtureAccount.AccountState == FixtureAccountState.InStorage || p.FixtureAccount.AccountState == null))
                                .WhereIf(locationIds.Any(),p=> locationIds.Contains((double)p.FixtureStorageLocationId))
                                .GroupBy(p => new
                                {
                                    p.FixtureAccount.FixtureEncodeId,
                                    p.FixtureStorageLocationId,
                                })
                                .Select(p => new
                                {
                                    Encode_Id = p.FixtureAccount.FixtureEncodeId,
                                    Fixture_Storage_Location_Id = p.FixtureStorageLocationId,
                                    Total_Qty = p.PassQty.SUM() + p.NgQty.SUM(),
                                    Pass_Qty = p.PassQty.SUM(),
                                    Ng_Qty = p.NgQty.SUM()
                                }).ToList();
            return stockList;
        }

        /// <summary>
        /// 同步工治具编码
        /// </summary>
        /// <param name="isCompatible">是否兼容</param>
        /// <param name="fixtureEncodeIdList">工治具编码Id列表</param>
        public virtual void SyncFixtureEncode(bool isCompatible, List<double> fixtureEncodeIdList)
        {
            var fixtureEncodeList = GetFixtureEncodesByIds(fixtureEncodeIdList);

            //获取所选工治具编码下的所有保养项目
            var allFixtureEncodeMaintainProject = GetFixtureEncodeProjectsByIds(fixtureEncodeIdList);
            //找出当前选择这批工治具编码下的所有工治具型号
            var fixtureModelIds = fixtureEncodeList.Select(p => p.FixtureModelId).ToList();
            //获取上述工治具型号下的所有保养项目
            var allFixtureModelMaintainProject = GetFixtureModelProjectsByIds(fixtureModelIds);
            using (var trans = DB.TransactionScope(KitFixturesEntityDataProvider.ConnectionStringName))
            {
                foreach (var fixtureEncode in fixtureEncodeList)
                {
                    //不兼容 则删除所选工治具编码下对应循环变量中的型号的保养项目 即为覆盖同步
                    if (!isCompatible)
                    {
                        var maintainProjectLists = allFixtureEncodeMaintainProject.Where(p => p.FixtureEncodeId == fixtureEncode.Id).ToList();
                        foreach (var maintainProjectList in maintainProjectLists)
                        {
                            maintainProjectList.PersistenceStatus = PersistenceStatus.Deleted;
                            RF.Save(maintainProjectList);
                        }
                    }
                    var fixtureModelMaintainProjects = allFixtureModelMaintainProject.Where(p => p.FixtureModelId == fixtureEncode.FixtureModelId).ToList();
                    foreach (var fixtureModelMaintainProject in fixtureModelMaintainProjects)
                    {   
                        //兼容同步
                        if (isCompatible)
                        {
                            var isExistProject = allFixtureEncodeMaintainProject.FirstOrDefault(p => p.FixtureEncodeId == fixtureEncode.Id && p.MaintainProjectId == fixtureModelMaintainProject.MaintainProjectId);
                            if (isExistProject != null)
                            {   
                                //当前工治具编码已存在对应的型号的保养项目,部分更新 入库保养 常规保养 上线定期保养 出库保养
                                isExistProject.InStorageMaintain = fixtureModelMaintainProject.InStorageMaintain;
                                isExistProject.CommonMaintain = fixtureModelMaintainProject.CommonMaintain;
                                isExistProject.OnlineMaintain = fixtureModelMaintainProject.OnlineMaintain;
                                isExistProject.ToStorageMaintain = fixtureModelMaintainProject.ToStorageMaintain;
                                RF.Save(isExistProject);
                                continue;
                            }
                        }
                        //当前工治具编码不存在对应的型号的保养项目 即全数新增
                        var fixtureEncodeMaintainProject = new FixtureEncodeMaintainProject();
                        fixtureEncodeMaintainProject.InStorageMaintain = fixtureModelMaintainProject.InStorageMaintain;
                        fixtureEncodeMaintainProject.CommonMaintain = fixtureModelMaintainProject.CommonMaintain;
                        fixtureEncodeMaintainProject.OnlineMaintain = fixtureModelMaintainProject.OnlineMaintain;
                        fixtureEncodeMaintainProject.ToStorageMaintain = fixtureModelMaintainProject.ToStorageMaintain;
                        fixtureEncodeMaintainProject.MaintainProjectId = fixtureModelMaintainProject.MaintainProjectId;
                        fixtureEncodeMaintainProject.FixtureEncodeId = fixtureEncode.Id;
                        RF.Save(fixtureEncodeMaintainProject);
                    }
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 根据工治具编码Id获取工治具编码
        /// </summary>
        /// <param name="id">工治具编码Id</param>
        /// <returns>工治具编码</returns>
        public virtual FixtureEncode GetFixtureEncode(double id)
        {
            return Query<FixtureEncode>().Where(p => p.Id == id).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 通过Id列表获取工治具编码列表
        /// </summary>
        /// <param name="fixtureEncodeIdList">工治具编码Id列表</param>
        /// <returns>工治具编码列表</returns>
        public virtual EntityList<FixtureEncode> GetFixtureEncodesByIds(List<double> fixtureEncodeIdList)
        {
            return Query<FixtureEncode>().Where(p => fixtureEncodeIdList.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具台帐Id列表获取工治具编码列表
        /// </summary>
        /// <param name="accountIds">工治具台帐Id列表</param>
        /// <returns>工治具编码列表</returns>
        public virtual EntityList<FixtureEncode> GetFixtureEncodesByAccountIds(List<double> accountIds)
        {
            return accountIds.SplitContains((tempIds) =>
            {
                return Query<FixtureEncode>()
                .Exists<FixtureAccount>((a, b) => b.Where(f => f.FixtureEncodeId == a.Id 
                    && tempIds.Contains(f.Id)))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取所有满足条件的工治具编码列表
        /// </summary>
        /// <param name="fixtureType">工治具类型</param>
        /// <param name="processStegmentId"></param>
        /// <param name="modelId">工治具型号Id</param>
        /// <param name="itemId">产品Id</param>
        /// <param name="deck">工艺面</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="pageInfo">分页</param>
        /// <returns>工治具编码列表</returns>
        public virtual EntityList<FixtureEncode> GetFixtureEncodeList(double? fixtureType, double? processStegmentId, double? modelId, double itemId, int? deck, string keyWord, PagingInfo pageInfo = null)
        {
            var encodeList = new EntityList<FixtureEncode>();
            var encodesOfBindProduct = GetFixtureEncodesBindProduct(fixtureType, processStegmentId, modelId, itemId, deck, keyWord, pageInfo);
            if (encodesOfBindProduct.Any())
                encodeList.AddRange(encodesOfBindProduct);
            var encodesOfUnBindProduct = GetFixtureEncodesUnBindProduct(processStegmentId, deck, fixtureType, modelId, keyWord, pageInfo);
            if (encodesOfUnBindProduct.Any())
                encodeList.AddRange(encodesOfUnBindProduct);
            return encodeList;
        }

        /// <summary>
        /// 根据工单的产品Id和工艺面获取绑定产品的工治具编码列表
        /// </summary>
        /// <param name="fixtureTypeId">工治具类型</param>
        /// <param name="processStegmentId"></param>
        /// <param name="modelId">工治具型号Id</param>
        /// <param name="itemId">产品Id</param>
        /// <param name="deck">工艺面</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="pageInfo">分页</param>
        /// <returns>工治具编码列表</returns>
        public virtual EntityList<FixtureEncode> GetFixtureEncodesBindProduct(double? fixtureTypeId, double? processStegmentId, double? modelId, double itemId, int? deck, string keyWord, PagingInfo pageInfo = null)
        {
            var query = Query<FixtureEncode>();
            if (fixtureTypeId.HasValue || modelId.HasValue)
            {
                if (fixtureTypeId.HasValue && modelId.HasValue)
                    query.Exists<FixtureModel>((a, b) => b.Where(f => f.Id == a.FixtureModelId && f.BindProduct == YesNo.Yes && f.FixtureTypeId == fixtureTypeId && f.Id == modelId));
                else if (fixtureTypeId.HasValue)
                    query.Exists<FixtureModel>((a, b) => b.Where(f => f.Id == a.FixtureModelId && f.BindProduct == YesNo.Yes && f.FixtureTypeId == fixtureTypeId));
                else if (modelId.HasValue)
                    query.Exists<FixtureModel>((a, b) => b.Where(f => f.Id == a.FixtureModelId && f.BindProduct == YesNo.Yes && f.Id == modelId));
            }
            else
                query.Exists<FixtureModel>((a, b) => b.Where(f => f.Id == a.FixtureModelId && f.BindProduct == YesNo.Yes));
            if (keyWord.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyWord));
            if (deck == null)
            {
                return query.Exists<FixtureEncodeProductDetail>(
                        (x, y) => y.Join<Item>((c, d) => c.ItemId == d.Id && d.Id == itemId)
                                                .Where(p => p.FixtureEncodeId == x.Id && p.ProcessSegmentId == processStegmentId)).ToList(pageInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
            else
            {
                return query.Exists<FixtureEncodeProductDetail>(
                          (x, y) => y.Join<Item>((c, d) => c.ItemId == d.Id && d.Id == itemId && c.Deck == (Deck)deck)
                                                  .Where(p => p.FixtureEncodeId == x.Id && p.ProcessSegmentId == processStegmentId)).ToList(pageInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
        }

        /// <summary>
        /// 获取未绑定产品的工治具编码列表
        /// </summary>
        /// <param name="processStegmentId"></param>
        /// <param name="deck"></param>
        /// <param name="fixtureTypeId">工治具类型</param>
        /// <param name="modelId">工治具型号Id</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="pageInfo">分页</param>
        /// <returns>工治具编码列表</returns>
        public virtual EntityList<FixtureEncode> GetFixtureEncodesUnBindProduct(double? processStegmentId, int? deck,
            double? fixtureTypeId, double? modelId, string keyWord, PagingInfo pageInfo = null)
        {
            var query = Query<FixtureEncode>();
            if (fixtureTypeId.HasValue || modelId.HasValue)
            {
                if (fixtureTypeId.HasValue && modelId.HasValue)
                    query.Exists<FixtureModel>((a, b) => b.Where(f => f.Id == a.FixtureModelId && f.BindProduct == YesNo.No && f.FixtureTypeId == fixtureTypeId && f.Id == modelId));
                else if (fixtureTypeId.HasValue)
                    query.Exists<FixtureModel>((a, b) => b.Where(f => f.Id == a.FixtureModelId && f.BindProduct == YesNo.No && f.FixtureTypeId == fixtureTypeId));
                else if (modelId.HasValue)
                    query.Exists<FixtureModel>((a, b) => b.Where(f => f.Id == a.FixtureModelId && f.BindProduct == YesNo.No && f.Id == modelId));
            }
            else
                query.Exists<FixtureModel>((a, b) => b.Where(f => f.Id == a.FixtureModelId && f.BindProduct == YesNo.No));
            if (processStegmentId.HasValue)
            {
                query.Exists<FixtureEncodeProductDetail>((a, b) => b.Where(f => f.ProcessSegmentId == processStegmentId && f.FixtureEncodeId == a.Id));
            }
            if (deck.HasValue)
            {
                query.Exists<FixtureEncodeProductDetail>((a, b) => b.Where(f => f.Deck == (Deck)deck && f.FixtureEncodeId == a.Id));
            }
            if (keyWord.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyWord));
            return query.ToList(pageInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过工治具编码Id列表获取工治具编码保养项目列表
        /// </summary>
        /// <param name="fixtureEncodeIdList">工治具编码Id列表</param>
        /// <returns>工治具编码保养项目列表</returns>
        public virtual EntityList<FixtureEncodeMaintainProject> GetFixtureEncodeProjectsByIds(List<double> fixtureEncodeIdList)
        {
            return Query<FixtureEncodeMaintainProject>().Where(p => fixtureEncodeIdList.Contains(p.FixtureEncodeId)).ToList();
        }

        /// <summary>
        /// 是否至少存在一笔入库保养项目
        /// </summary>
        /// <param name="encodeId">工治具编码Id</param>
        /// <returns>true/false</returns>
        public virtual bool IsExistMaintainProject(double encodeId)
        {
            return Query<FixtureEncodeMaintainProject>().Where(p => p.FixtureEncodeId == encodeId && p.InStorageMaintain).Count() > 0;
        }

        /// <summary>
        /// 根据治具编码Id获取治具编码保养项目列表
        /// </summary>
        /// <param name="encodeId">治具编码Id</param>
        /// <returns>治具编码保养项目列表</returns>
        public virtual EntityList<FixtureEncodeMaintainProject> GetFixtureEncodeMaintainProjects(double encodeId)
        {
            return Query<FixtureEncodeMaintainProject>().Where(p => p.FixtureEncodeId == encodeId && p.InStorageMaintain).ToList();
        }

        /// <summary>
        /// 根据治具编码Id获取上线保养项目列表
        /// </summary>
        /// <param name="encodeId">治具编码Id</param>
        /// <returns>治具编码保养项目列表</returns>
        public virtual EntityList<FixtureEncodeMaintainProject> GetOnlineMaintainProjects(double encodeId)
        {
            return Query<FixtureEncodeMaintainProject>().Where(p => p.FixtureEncodeId == encodeId && p.OnlineMaintain).ToList();
        }

        /// <summary>
        /// 根据治具编码Id列表获取治具编码出库保养项目列表
        /// </summary>
        /// <param name="encodeIds">治具编码Id列表</param>
        /// <returns>治具编码出库保养项目列表</returns>
        public virtual EntityList<FixtureEncodeMaintainProject> GetToStorageMaintainProjects(List<double> encodeIds)
        {
            return Query<FixtureEncodeMaintainProject>().Where(p => encodeIds.Contains(p.FixtureEncodeId) && p.ToStorageMaintain).ToList();
        }

        /// <summary>
        /// 根据治具编码Id获取出库保养项目列表
        /// </summary>
        /// <param name="encodeId">治具编码Id</param>
        /// <returns>出库保养项目列表</returns>
        public virtual EntityList<FixtureEncodeMaintainProject> GetToStorageMaintainMaintainProjects(double encodeId)
        {
            return Query<FixtureEncodeMaintainProject>().Where(p => p.FixtureEncodeId == encodeId && p.ToStorageMaintain).ToList();
        }

        /// <summary>
        /// 根据工治具编码Id获取治具状态（ID类）
        /// </summary>
        /// <param name="encodeId">工治具编码Id</param>
        /// <returns>治具状态</returns>
        public virtual FixtureAccountState GetIdAccountState(double encodeId)
        {
            var encode = RF.GetById<FixtureEncode>(encodeId);
            var model = RF.GetById<FixtureModel>(encode.FixtureModelId);
            var isExist = IsExistMaintainProject(encodeId);
            if (isExist)
                return FixtureAccountState.WaitMaintain;
            if (model.FixedStorage == YesNo.Yes)
                return FixtureAccountState.InStorage;
            else
                return FixtureAccountState.WaitShelf;
        }

        /// <summary>
        /// 根据工治具编码Id和产品Id获取工治具编码产品清单
        /// </summary>
        /// <param name="encodeIds">工治具编码Id列表</param>
        /// <param name="productId">产品Id</param>
        /// <returns>工治具编码产品清单</returns>
        public virtual EntityList<FixtureEncodeProductDetail> GetProductDetailsByEncodeProductId(List<double> encodeIds, double productId)
        {
            return Query<FixtureEncodeProductDetail>().Where(p => encodeIds.Contains(p.FixtureEncodeId) && p.ItemId == productId).ToList();
        }

        /// <summary>
        /// 根据工治具治具编码Id获取工治具治具编码产品清单
        /// </summary>
        /// <param name="encodeId">工治具治具编码Id</param>
        /// <returns>工治具治具编码产品清单</returns>
        public virtual EntityList<FixtureEncodeProductDetail> GetProductDetailsByEncodeId(double encodeId)
        {
            return Query<FixtureEncodeProductDetail>().Where(p => p.FixtureEncodeId == encodeId).ToList();
        }

        /// <summary>
        /// 根据工治具型号Id获取工治具型号设备清单
        /// </summary>
        /// <param name="modelId">工治具型号Id</param>
        /// <returns>工治具型号设备清单</returns>
        public virtual EntityList<FixtureModelEquipDetail> GetEquipDetails(double modelId)
        {
            return Query<FixtureModelEquipDetail>().Where(p => p.FixtureModelId == modelId).ToList();
        }

        /// <summary>
        /// 根据工治具编码Id列表获取工治具编码产品清单列表
        /// </summary>
        /// <param name="encodeIds">工治具编码Id列表</param>
        /// <param name="excepIds">除去前端ids</param>
        /// <returns>工治具编码产品清单列表</returns>
        public virtual EntityList<FixtureEncodeProductDetail> GetProductDetails(List<double> encodeIds, List<double> excepIds)
        {
            EntityList<FixtureEncodeProductDetail> details = new EntityList<FixtureEncodeProductDetail>();
            excepIds.SplitDataExecute(temps2 =>
            {
                encodeIds.SplitDataExecute(temps1 =>
                {
                    var list = Query<FixtureEncodeProductDetail>().Where(p => temps1.Contains(p.FixtureEncodeId) && !temps2.Contains(p.Id)).ToList();
                    details.AddRange(list);
                });
            });
            
            return details;
        }

        /// <summary>
        /// 获取工治具编码下的存储位置信息
        /// </summary>
        /// <param name="encodeIds">工治具编码Ids</param>
        /// <param name="excepIds">除去前端ids</param>
        /// <returns></returns>
        public virtual EntityList<FixtureEncodeStorageLocation> GetFixtureEncodeStorageLocations(List<double> encodeIds, List<double> excepIds)
        {
            EntityList<FixtureEncodeStorageLocation> locations = new EntityList<FixtureEncodeStorageLocation>();
            excepIds.SplitDataExecute(temps2 =>
            {
                encodeIds.SplitDataExecute(temps1 =>
                {
                    var list = Query<FixtureEncodeStorageLocation>().Where(p => temps1.Contains(p.FixtureEncodeId) && !temps2.Contains(p.Id)).ToList();
                    locations.AddRange(list);
                });
            });
            return locations;
        }

        /// <summary>
        /// 判断某工治具台帐的工治具编码的存储位置下存在某仓库和库位
        /// </summary>
        /// <param name="accountId">工治具台帐Id</param>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="locationId">库位Id</param>
        /// <returns>true/false</returns>
        public virtual bool IsExistWarehouseLocation(double accountId, double warehouseId, double locationId)
        {
            return Query<FixtureEncodeStorageLocation>().Exists<FixtureEncode>(
                   (x, y) => y.Join<FixtureAccount>((c, d) => c.Id == d.FixtureEncodeId && d.Id == accountId)
                       .Where(p => p.Id == x.FixtureEncodeId && x.WarehouseId == warehouseId && x.StorageLocationId == locationId)).Count() <= 0;
        }

        /// <summary>
        /// 判断仓库和库位是否存在
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="locationId">库位Id</param>
        /// <returns>true/false</returns>
        public virtual bool IsExistWarehouseLocation(double warehouseId, double locationId)
        {
            return Query<StorageLocation>().Where(p => p.WarehouseId == warehouseId && p.Id == locationId).Count() <= 0;
        }

        /// <summary>
        /// 根据工治具ID和库位获取库位
        /// </summary>
        /// <param name="code">ID编码</param>
        /// <param name="locationCode">库位编码</param>
        /// <param name="warehouseId"></param>
        /// <returns>库位</returns>
        public virtual StorageLocation GetStorageLocation(string code, string locationCode,double warehouseId)
        {

            return Query<StorageLocation>().Exists<Warehouse>(
                   (x, y) => y.Join<FixtureEncodeStorageLocation>((c, d) => c.Id == d.WarehouseId)
                       .Join<FixtureEncodeStorageLocation, FixtureEncode>((c, d) => c.FixtureEncodeId == d.Id)
                        .Join<FixtureEncode, FixtureAccount>((c, d) => c.Id == d.FixtureEncodeId && d.Code == code)
                       .Where(p => p.Id == x.WarehouseId && x.Code == locationCode && x.WarehouseId == warehouseId)).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 是否存在上线定期保养项目
        /// </summary>
        /// <param name="fixtureEncodeId">工治具编码ID</param>
        /// <param name="modifiedIds">界面修改的ID</param>
        /// <returns>存在返回true，不存在返回false</returns>
        public virtual bool IsExistEncodeOnlineMaintains(double fixtureEncodeId, List<double> modifiedIds)
        {
            return Query<FixtureEncodeMaintainProject>().Where(p => p.FixtureEncodeId == fixtureEncodeId && p.OnlineMaintain
            && !modifiedIds.Contains(p.Id)).Count() > 0;
        }



        /// <summary>
        /// 工治具编码保存前校验
        /// </summary>
        /// <param name="list"></param>
        public virtual void FixtureEncodeBeforeSaveValidate(EntityList<FixtureEncode> list)
        {
            // 校验产品清单重复
            EntityList<FixtureEncodeProductDetail> productListRepeatList = new EntityList<FixtureEncodeProductDetail>();
            // 校验存储位置重复
            EntityList<FixtureEncodeStorageLocation> locationListRepeatList = new EntityList<FixtureEncodeStorageLocation>();

            // 工治具编码
            var encodeIds = list.Select(p => p.Id).ToList();

            // 数据库产品清单
            var dbProductList = GetProductDetails(encodeIds, list.SelectMany(p => p.FixtureEncodeProductDetailList).Select(p => p.Id).ToList());
            // 数据库位置信息
            var dbLocationList = GetFixtureEncodeStorageLocations(encodeIds, list.SelectMany(p => p.FixtureEncodeStorageLocationList).Select(p => p.Id).ToList());

            productListRepeatList.AddRange(list.SelectMany(p => p.FixtureEncodeProductDetailList).ToList());
            productListRepeatList.AddRange(dbProductList);

            locationListRepeatList.AddRange(list.SelectMany(p => p.FixtureEncodeStorageLocationList).ToList());
            locationListRepeatList.AddRange(dbLocationList);

            if (productListRepeatList.GroupBy(p => new { p.FixtureEncodeId, p.ItemId, p.Deck, p.ProcessSegmentId }).Any(p => p.Count() > 1))
            {
                throw new ValidationException("产品清单产品编码+工艺面+工段唯一".L10N());
            }

            if (locationListRepeatList.GroupBy(p => new {p.FixtureEncodeId, p.WarehouseId, p.StorageLocationId}).Any(p => p.Count() > 1))
            {
                throw new ValidationException("仓库编码+库位编码唯一".L10N());
            }
        }
        #endregion
    }
}
