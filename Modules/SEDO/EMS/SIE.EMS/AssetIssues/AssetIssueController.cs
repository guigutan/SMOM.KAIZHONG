using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.AssetRequisitions;
using SIE.EMS.AssetTransfers;
using SIE.EMS.Common.Controller;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipmentCards;
using SIE.Equipments.WorkFlows;
using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.EMS.AssetIssues
{
    /// <summary>
    /// 资产发放单控制器
    /// </summary>
    public class AssetIssueController : DomainController
    {
        #region 资产发放单号生成规则
        /// <summary>
        /// 获取自动生成资产发放单号No
        /// </summary>
        /// <returns>编号</returns>
        public virtual string GetNo()
        {
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(AssetIssue));

            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到资产发放单号生成规则,请检查规则配置".L10N());
            var code = RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();

            return code;
        }
        #endregion

        #region 获取审批流配置信息
        /// <summary>
        /// 获取审批流配置信息
        /// </summary>
        /// <returns>审批流配置信息</returns>
        public virtual ApprovalConfigValue GetApprovalFlowConfigValue()
        {
            var configValue = ConfigService.GetConfig<ApprovalConfigValue>(new ApprovalConfig(), typeof(AssetIssue));

            if (configValue == null)
                throw new ValidationException("未找到审批流配置规则,请检查规则配置".L10N());

            return configValue;
        }
        #endregion

        /// <summary>
        /// 查询资产发放单
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns>资产发放列表</returns>
        public virtual EntityList<AssetIssue> GetAssetIssueList(AssetIssueCriteria criteria)
        {
            var q = Query<AssetIssue>();

            if (criteria.IssueNo.IsNotEmpty())
            {
                q.Where(p => p.IssueNo.Contains(criteria.IssueNo));
            }
            if (criteria.RequisitionNo.IsNotEmpty())
            {
                q.Where(p => p.AssetRequisition.RequisitionNo.Contains(criteria.RequisitionNo));
            }
            if (criteria.QureyFactoryId != null && criteria.QureyFactoryId != 0)
            {
                q.Where(p => p.FactoryId == criteria.QureyFactoryId);
            }
            if (criteria.RequisitionType != null)
            {
                q.Where(p => p.AssetRequisition.RequisitionType == criteria.RequisitionType);
            }
            if (criteria.AssetObject != null)
            {
                q.Where(p => p.AssetRequisition.AssetObject == criteria.AssetObject);
            }
            if (criteria.ApplyDepartmentId != null && criteria.ApplyDepartmentId != 0)
            {
                q.Where(p => p.ApplyDepartmentId == criteria.ApplyDepartmentId);
            }
            if (criteria.LendingDepartmentId != null && criteria.LendingDepartmentId != 0)
            {
                q.Where(p => p.LendingDepartmentId == criteria.LendingDepartmentId);
            }
            if (criteria.WarehouseId != null && criteria.WarehouseId != 0)
            {
                q.Where(p => p.WarehouseId == criteria.WarehouseId);
            }
            if (criteria.ApprovalStatus != null)
            {
                q.Where(p => p.ApprovalStatus == criteria.ApprovalStatus);
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                q.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                q.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            }

            return q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取可发放的设备清单
        /// </summary>
        /// <param name="issueId">发放单Id</param>
        /// <param name="requisitionId">领用单Id</param>
        /// <returns>设备清单集合</returns>
        public virtual EntityList<AssetIssueEquipment> GetAssetIssueEquipmentsById(double issueId, double requisitionId)
        {
            var issueList = Query<AssetIssueEquipment>().Where(p => p.AssetIssueId == issueId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            var reqEquipList = Query<AssetRequisitionEquipment>().Where(p => p.AssetRequisitionId == requisitionId && p.Qty > p.PickedQty).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            foreach (var reqEquip in reqEquipList)
            {
                int count = reqEquip.Qty - reqEquip.PickedQty;
                for (int i = 0; i < count; i++)
                {
                    var issueEquip = new AssetIssueEquipment();
                    issueEquip.IsSelectEquipAccount = reqEquip.EquipAccountId != null;
                    issueEquip.AssetRequisitionEquipmentId = reqEquip.Id;
                    issueEquip.EquipAccountId = reqEquip.EquipAccountId ?? 0;
                    issueEquip.LineNo = reqEquip.LineNo;
                    issueEquip.EquipAccountCode = reqEquip.EquipAccountCode;
                    issueEquip.EquipAccountName = reqEquip.EquipAccountName;
                    issueEquip.UseState = reqEquip.UseState;
                    issueEquip.Alias = reqEquip.Alias;
                    issueEquip.EquipModelCode = reqEquip.EquipModelCode;
                    issueEquip.EquipModelName = reqEquip.EquipModelName;
                    issueEquip.Specifications = reqEquip.Specifications;
                    issueEquip.EquipTypeCode = reqEquip.EquipTypeCode;
                    issueEquip.EquipTypeName = reqEquip.EquipTypeName;
                    issueList.Add(issueEquip);
                }
            }

            var list = new EntityList<AssetIssueEquipment>();
            list.AddRange(issueList.OrderBy(p => p.LineNo));
            return list;
        }

        /// <summary>
        /// 通过设备清单明细Id获取发放设备清单明细
        /// </summary>
        /// <param name="reqEquipId">领用设备清单明细Id</param>
        /// <returns>发放设备清单明细</returns>
        public virtual EntityList<AssetIssueEquipment> GetAssetIssueEquipmentsByReqEquipId(double reqEquipId)
        {
            var issueList = new EntityList<AssetIssueEquipment>();

            var reqEquip = Query<AssetRequisitionEquipment>().Where(p => p.Id == reqEquipId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());

            var issueEquip = new AssetIssueEquipment();
            issueEquip.IsSelectEquipAccount = reqEquip.EquipAccountId != null;
            issueEquip.AssetRequisitionEquipmentId = reqEquip.Id;
            issueEquip.EquipAccountId = reqEquip.EquipAccountId ?? 0;
            issueEquip.LineNo = reqEquip.LineNo;
            issueEquip.EquipAccountCode = reqEquip.EquipAccountCode;
            issueEquip.EquipAccountName = reqEquip.EquipAccountName;
            issueEquip.UseState = reqEquip.UseState;
            issueEquip.Alias = reqEquip.Alias;
            issueEquip.EquipModelCode = reqEquip.EquipModelCode;
            issueEquip.EquipModelName = reqEquip.EquipModelName;
            issueEquip.Specifications = reqEquip.Specifications;
            issueEquip.EquipTypeCode = reqEquip.EquipTypeCode;
            issueEquip.EquipTypeName = reqEquip.EquipTypeName;
            issueList.Add(issueEquip);

            return issueList;
        }

        /// <summary>
        /// 通过设备Id获取发放设备清单明细
        /// </summary>
        /// <param name="equipId">设备Id</param>
        /// <returns>发放设备清单明细</returns>
        public virtual EntityList<AssetIssueEquipment> GetAssetIssueEquipmentsByEquipId(double equipId)
        {
            var issueList = new EntityList<AssetIssueEquipment>();

            var equip = Query<EquipAccount>().Where(p => p.Id == equipId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());

            var issueEquip = new AssetIssueEquipment();
            issueEquip.IsSelectEquipAccount = false;
            issueEquip.EquipAccountId = equip.Id;
            issueEquip.EquipAccountCode = equip.Code;
            issueEquip.EquipAccountName = equip.Name;
            issueEquip.UseState = equip.UseState;
            issueEquip.Alias = equip.Alias;
            issueEquip.EquipModelCode = equip.ModelCode;
            issueEquip.EquipModelName = equip.ModelName;
            issueEquip.Specifications = equip.Specifications;
            issueEquip.EquipTypeCode = equip.EquipTypeCode;
            issueEquip.EquipTypeName = equip.EquipTypeName;
            issueList.Add(issueEquip);

            return issueList;
        }

        /// <summary>
        /// 根据发放工治具清单明细行获取工治具ID台账信息
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="issueFixture">发放工治具清单明细</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>工治具ID台账列表</returns>
        public virtual EntityList<FixtureIDAccount> GetFixtureIDAccounts(PagingInfo pagingInfo, AssetIssueFixture issueFixture, string keyword)
        {
            var q = Query<FixtureIDAccount>().Join<FixtureAccountStock>((a, b) => a.Id == b.FixtureAccountId)
                                             .Where<FixtureAccountStock>((a, b) => a.AccountState == FixtureAccountState.InStorage
                                                                                && a.FixtureEncode.Code == issueFixture.AssetRequisitionFixture.FixtureEncode.Code
                                                                                && b.FixtureWarehouseId == issueFixture.WarehouseId);
            if (keyword.IsNotEmpty())
            {
                q.Where(p => p.Code.Contains(keyword));
            }
            q.NotExists<AssetIssueFixture>((x, y) => y.Where(p => p.AssetIssue.ApprovalStatus != ApprovalStatus.Audited && p.FixtureAccountId == x.Id && p.AssetIssueId != issueFixture.AssetIssueId));

            var list = q.ToList(pagingInfo, new EagerLoadOptions().LoadWith(FixtureIDAccount.StockListProperty));

            foreach (var item in list)
            {
                item.LocationId = item.StockList[0].FixtureStorageLocationId;
                item.LocationCode = item.StockList[0].StorageLocation.Code;
            }
            return list;
        }

        /// <summary>
        /// 获取设备清单集合
        /// </summary>
        /// <param name="issueIds">发放单Id列表</param>        
        /// <returns>设备清单集合</returns>
        private EntityList<AssetIssueEquipment> GetAssetIssueEquipmentsBytIssueIds(List<double> issueIds)
        {
            return issueIds.SplitContains(tempIds =>
            {
                return Query<AssetIssueEquipment>()
                    .Where(p => tempIds.Contains(p.AssetIssueId))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取工治具清单
        /// </summary>
        /// <param name="issueIds">发放单Id列表</param>        
        /// <returns>工治具清单集合</returns>
        private EntityList<AssetIssueFixture> GetAssetIssueFixturesBytIssueIds(List<double> issueIds)
        {
            return issueIds.SplitContains(tempIds =>
            {
                return Query<AssetIssueFixture>()
                    .Where(p => tempIds.Contains(p.AssetIssueId))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            });
        }

        /// <summary>
        /// 获取可发放的工治具清单
        /// </summary>
        /// <param name="issueId">发放单Id</param>
        /// <param name="requisitionId">领用单Id</param>
        /// <returns>工治具清单集合</returns>
        public virtual EntityList<AssetIssueFixture> GetAssetIssueFixturesById(double issueId, double requisitionId)
        {
            var issueList = Query<AssetIssueFixture>().Where(p => p.AssetIssueId == issueId).ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(AssetIssueFixture.AssetRequisitionFixtureProperty));

            issueList.ForEach(issue =>
            {

                issue.NotPickQty = issue.ManageMode == ManageMode.Number ? 1 : issue.AssetRequisitionFixture.Qty - issue.AssetRequisitionFixture.IssuedQty;

            });

            var reqFixtureList = Query<AssetRequisitionFixture>().Where(p => p.AssetRequisitionId == requisitionId && p.Qty > p.PickedQty).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            foreach (var reqFixture in reqFixtureList)
            {
                int count = reqFixture.Qty - reqFixture.PickedQty;
                int notIssueQty = reqFixture.Qty - reqFixture.IssuedQty;
                var issueFixture = new AssetIssueFixture();

                if (reqFixture.ManageMode == Fixtures.ManageMode.Number)
                {
                    for (int i = 0; i < count; i++)
                    {
                        issueFixture = new AssetIssueFixture();
                        issueFixture.AssetRequisitionFixtureId = reqFixture.Id;
                        issueFixture.LineNo = reqFixture.LineNo;
                        issueFixture.FixtureEncodeId = reqFixture.FixtureEncodeId;
                        issueFixture.FixtureEncode = reqFixture.FixtureEncodeCode;
                        issueFixture.ModelCode = reqFixture.ModelCode;
                        issueFixture.ModelName = reqFixture.ModelName;
                        issueFixture.FixtureType = reqFixture.FixtureType;
                        issueFixture.ManageMode = ManageMode.Number;
                        issueFixture.NotPickQty = 1;
                        issueFixture.Qty = 0;
                        issueFixture.UnitName = reqFixture.UnitName;
                        issueFixture.StoreUsableQty = 0;
                        issueList.Add(issueFixture);
                    }
                }
                else
                {
                    issueFixture.AssetRequisitionFixtureId = reqFixture.Id;
                    issueFixture.LineNo = reqFixture.LineNo;
                    issueFixture.FixtureEncodeId = reqFixture.FixtureEncodeId;
                    issueFixture.FixtureEncode = reqFixture.FixtureEncodeCode;
                    issueFixture.ModelCode = reqFixture.ModelCode;
                    issueFixture.ModelName = reqFixture.ModelName;
                    issueFixture.FixtureType = reqFixture.FixtureType;
                    issueFixture.ManageMode = ManageMode.Code;
                    issueFixture.NotPickQty = notIssueQty;
                    issueFixture.Qty = 0;
                    issueFixture.UnitName = reqFixture.UnitName;
                    issueFixture.StoreUsableQty = 0;
                    issueList.Add(issueFixture);
                }
            }

            //查询工治具编码的可用库存
            var warehouseId = (double)RF.GetById<AssetRequisition>(requisitionId).WarehouseId;
            var encodeIds = issueList.Select(p => p.FixtureEncodeId).Distinct().ToList();
            var locationIds = issueList.Where(p => p.StorageLocationId != null).Select(p => (double)p.StorageLocationId).Distinct().ToList();
            var stocklist = RT.Service.Resolve<CoreFixtureController>().GetCanUseNumByWarehouseId(warehouseId, encodeIds, locationIds);

            foreach (var item in issueList)
            {
                item.StoreUsableQty = 0;
                if (item.StorageLocationId != null && item.StorageLocationId != 0)
                {
                    FixtureAccountStock stock = stocklist.FirstOrDefault(p => p.EncodeId == item.FixtureEncodeId && p.FixtureStorageLocationId == item.StorageLocationId);

                    if (stock != null)
                    {
                        if (item.QualityStatus == FixtureQualityState.Pass)
                        {
                            item.StoreUsableQty = stock.PassQty;
                        }
                        else if (item.QualityStatus == FixtureQualityState.Ng)
                        {
                            item.StoreUsableQty = stock.NgQty;
                        }
                        else
                        {
                            item.StoreUsableQty = stock.TotalQty;
                        }
                    }
                }
                else
                {
                    var stockDtl = stocklist.Where(p => p.EncodeId == item.FixtureEncodeId).ToList();

                    if (stockDtl.Any())
                    {
                        if (item.QualityStatus == FixtureQualityState.Pass)
                        {
                            item.StoreUsableQty = stockDtl.Sum(p => p.PassQty);
                        }
                        else if (item.QualityStatus == FixtureQualityState.Ng)
                        {
                            item.StoreUsableQty = stockDtl.Sum(p => p.NgQty);
                        }
                        else
                        {
                            item.StoreUsableQty = stockDtl.Sum(p => p.TotalQty);
                        }
                    }
                }
            }

            var list = new EntityList<AssetIssueFixture>();
            list.AddRange(issueList.OrderBy(p => p.LineNo));
            return list;
        }

        /// <summary>
        /// 保存资产发放单设备明细
        /// </summary>
        /// <param name="assetIssue">含设备明细的资产发放单</param>
        public virtual void SaveAssetIssueEquipment(AssetIssue assetIssue)
        {
            const bool trueValue = true;
            List<double> reqEquipDtlIds = new List<double>();
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                var selEquipList = assetIssue.AssetIssueEquipmentList.Where(p => p.IsSelected).Select(p => p.EquipAccountId).SplitContains(tempIds =>
                {
                    return Query<EquipAccount>().Where(p => tempIds.Contains(p.Id)).ToList();
                });
                var selEquipIds = selEquipList.Select(p => p.Id);

                var existIssueEquipList = Query<AssetIssueEquipment>().Where(p => selEquipIds.Contains((double)p.EquipAccountId) && p.AssetIssue.ApprovalStatus != SIE.Equipments.Enums.ApprovalStatus.Audited && p.AssetIssueId != assetIssue.Id).ToList();
                var existTransEquipList = Query<AssetTransferDetail>().Where(p => selEquipIds.Contains(p.EquipAccountId) && p.AssetTransfer.ApprovalStatus != SIE.Equipments.Enums.ApprovalStatus.Audited).ToList();
                var existIssueEquipIds = existIssueEquipList.Select(p => p.EquipAccountId);
                var existTransEquipIds = existTransEquipList.Select(p => p.EquipAccountId);

                var equipmentCards = RT.Service.Resolve<EquipmentCardController>().GetEquipmentCardByCode(selEquipList.Select(m => m.Code).ToList());
                var eidtingCards = equipmentCards.Where(m => m.IsChange).ToList();

                var assetReqEquipList = assetIssue.AssetIssueEquipmentList.Select(p => p.AssetRequisitionEquipmentId).SplitContains(tempIds =>
                {
                    return Query<AssetRequisitionEquipment>().Where(p => tempIds.Contains(p.Id)).ToList();
                });

                foreach (var issueEquipDtl in assetIssue.AssetIssueEquipmentList)
                {
                    if (issueEquipDtl.IsSelected)
                    {
                        var selEquip = selEquipList.First(p => p.Id == issueEquipDtl.EquipAccountId);

                        if (selEquip.UseState == Core.Enums.AccountUseState.DisposedOf || selEquip.UseState == Core.Enums.AccountUseState.Lease
                            || selEquip.UseState == Core.Enums.AccountUseState.Scrap || selEquip.UseState == Core.Enums.AccountUseState.ToAccepted)
                        {
                            throw new ValidationException("设备编码【{0}】的管理状态为【{1}】，不能发放".L10nFormat(selEquip.Code, selEquip.UseState.ToLabel()));
                        }

                        if (existIssueEquipIds.Contains(issueEquipDtl.EquipAccountId) || existTransEquipIds.Contains((double)issueEquipDtl.EquipAccountId))
                        {
                            throw new ValidationException("设备编码【{0}】存在未完结的调拨单（发放单），不能发放".L10nFormat(selEquip.Code));
                        }

                        if (eidtingCards.Any(p => p.Code == selEquip.Code))
                        {
                            throw new ValidationException("设备编码【{0}】处于立卡修改中，不允许发放".L10nFormat(selEquip.Code));
                        }

                        issueEquipDtl.PersistenceStatus = issueEquipDtl.CreateBy == 0 ? PersistenceStatus.New : PersistenceStatus.Unchanged;
                    }
                    else
                    {
                        issueEquipDtl.PersistenceStatus = issueEquipDtl.CreateBy == 0 ? PersistenceStatus.Unchanged : PersistenceStatus.Deleted;
                    }

                    //更新资产领用单设备明细的拣货数量
                    if (!reqEquipDtlIds.Contains(issueEquipDtl.AssetRequisitionEquipmentId))
                    {
                        var lessQty = assetIssue.AssetIssueEquipmentList.Count(p => p.AssetRequisitionEquipmentId == issueEquipDtl.AssetRequisitionEquipmentId && p.CreateBy != 0 && p.IsSelected != trueValue);
                        var addQty = assetIssue.AssetIssueEquipmentList.Count(p => p.AssetRequisitionEquipmentId == issueEquipDtl.AssetRequisitionEquipmentId && p.CreateBy == 0 && p.IsSelected == trueValue);
                        var subQty = addQty - lessQty;

                        var assetReqEquip = assetReqEquipList.First(p => p.Id == issueEquipDtl.AssetRequisitionEquipmentId);

                        if (assetReqEquip.PickedQty + subQty > assetReqEquip.Qty)
                        {
                            throw new ValidationException("行号【{0}】拣货数量大于申请数量".L10nFormat(assetReqEquip.LineNo));
                        }

                        assetReqEquip.PickedQty += subQty;
                        RF.Save(assetReqEquip);

                        reqEquipDtlIds.Add(issueEquipDtl.AssetRequisitionEquipmentId);
                    }
                }

                RF.Save(assetIssue);

                trans.Complete();
            }
        }

        /// <summary>
        /// 保存资产发放单工治具明细
        /// </summary>
        /// <param name="assetIssue">含工治具明细的资产发放单</param>
        public virtual void SaveAssetIssueFixture(AssetIssue assetIssue)
        {
            const bool trueValue = true;
            List<double> reqFixtureDtlIds = new List<double>();
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                var selFixtureList = assetIssue.AssetIssueFixtureList.Where(p => p.IsSelected && p.FixtureAccountId != null).Select(p => p.FixtureAccountId).SplitContains(tempIds =>
                {
                    return Query<FixtureIDAccount>().Where(p => tempIds.Contains(p.Id)).ToList();
                });
                var selFixtureIds = selFixtureList.Select(p => p.Id);

                var existIssueFixtureList = Query<AssetIssueFixture>().Where(p => selFixtureIds.Contains((double)p.FixtureAccountId) && p.AssetIssue.ApprovalStatus != SIE.Equipments.Enums.ApprovalStatus.Audited && p.AssetIssueId != assetIssue.Id).ToList();
                var existIssueFixtureIds = existIssueFixtureList.Select(p => p.FixtureAccountId);

                var assetReqFixtureList = assetIssue.AssetIssueFixtureList.Select(p => p.AssetRequisitionFixtureId).SplitContains(tempIds =>
                {
                    return Query<AssetRequisitionFixture>().Where(p => tempIds.Contains(p.Id)).ToList();
                });
                var storageLocationList = assetIssue.AssetIssueFixtureList.Select(p => p.StorageLocationId).SplitContains(tempIds =>
                {
                    return Query<StorageLocation>().Where(p => tempIds.Contains(p.Id)).ToList();
                });

                foreach (var issueFixtureDtl in assetIssue.AssetIssueFixtureList)
                {
                    if (issueFixtureDtl.IsSelected)
                    {
                        issueFixtureDtl.FixtureEncode = issueFixtureDtl.AssetRequisitionFixture.FixtureEncode.Code;
                        if (assetIssue.AssetIssueFixtureList.Count(p => p.FixtureAccountId == null
                                    && p.FixtureEncode == issueFixtureDtl.FixtureEncode
                                    && p.QualityStatus == issueFixtureDtl.QualityStatus && p.StorageLocationId == issueFixtureDtl.StorageLocationId) > 1)
                        {
                            throw new ValidationException("行号【{0}】工治具的质量状态和发放库位的数据重复，请确认".L10nFormat(issueFixtureDtl.AssetRequisitionFixture.LineNo));
                        }

                        if (storageLocationList.First(p => p.Id == issueFixtureDtl.StorageLocationId).WarehouseId != assetIssue.WarehouseId)
                        {
                            throw new ValidationException("行号【{0}】工治具的发放库位不属于主表发放仓库，请确认".L10nFormat(issueFixtureDtl.AssetRequisitionFixture.LineNo));
                        }

                        if (issueFixtureDtl.FixtureAccountId != null)
                        {
                            var selFixture = selFixtureList.First(p => p.Id == issueFixtureDtl.FixtureAccountId);

                            if (selFixture.AccountState != FixtureAccountState.InStorage)
                            {
                                throw new ValidationException("工治具【{0}】非在库状态，不能发放".L10nFormat(selFixture.Code));
                            }

                            if (existIssueFixtureIds.Contains(issueFixtureDtl.FixtureAccountId))
                            {
                                throw new ValidationException("工治具【{0}】存在于其它未完结的发放单中，不能发放".L10nFormat(selFixture.Code));
                            }
                        }

                        issueFixtureDtl.PersistenceStatus = issueFixtureDtl.CreateBy == 0 ? PersistenceStatus.New : PersistenceStatus.Modified;
                    }
                    else
                    {
                        issueFixtureDtl.PersistenceStatus = issueFixtureDtl.CreateBy == 0 ? PersistenceStatus.Unchanged : PersistenceStatus.Deleted;
                    }

                    //更新资产领用单ID管控工治具明细的拣货数量
                    if (!reqFixtureDtlIds.Contains(issueFixtureDtl.AssetRequisitionFixtureId) && issueFixtureDtl.FixtureAccountId != null)
                    {
                        int lessQty = 0;
                        int addQty = 0;

                        lessQty = assetIssue.AssetIssueFixtureList.Count(p => p.AssetRequisitionFixtureId == issueFixtureDtl.AssetRequisitionFixtureId && p.CreateBy != 0 && p.IsSelected != trueValue);
                        addQty = assetIssue.AssetIssueFixtureList.Count(p => p.AssetRequisitionFixtureId == issueFixtureDtl.AssetRequisitionFixtureId && p.CreateBy == 0 && p.IsSelected == trueValue);

                        var subQty = addQty - lessQty;

                        var assetReqFixture = assetReqFixtureList.First(p => p.Id == issueFixtureDtl.AssetRequisitionFixtureId);

                        if (assetReqFixture.PickedQty + subQty > assetReqFixture.Qty)
                        {
                            throw new ValidationException("行号【{0}】总拣货数量大于申请数量".L10nFormat(assetReqFixture.LineNo));
                        }

                        assetReqFixture.PickedQty += subQty;
                        RF.Save(assetReqFixture);

                        reqFixtureDtlIds.Add(issueFixtureDtl.AssetRequisitionFixtureId);
                    }
                }

                //保存发放单数据
                RF.Save(assetIssue);

                //更新领用单编码类管控工治具明细的拣货数量
                var issueCodeFixtureReqIds = assetIssue.AssetIssueFixtureList.Where(p => p.FixtureAccountId == null).Select(p => p.AssetRequisitionFixtureId).Distinct();
                var allIssueCodeFixtureList = Query<AssetIssueFixture>().Where(p => issueCodeFixtureReqIds.Contains(p.AssetRequisitionFixtureId)).ToList();

                if (issueCodeFixtureReqIds.Any())
                {
                    foreach (var issueCodeFixtureReqId in issueCodeFixtureReqIds)
                    {
                        int pickQty = allIssueCodeFixtureList.Where(p => p.AssetRequisitionFixtureId == issueCodeFixtureReqId).Sum(p => (int)p.Qty);

                        var assetReqFixture = assetReqFixtureList.First(p => p.Id == issueCodeFixtureReqId);

                        if (pickQty > assetReqFixture.Qty)
                        {
                            throw new ValidationException("行号【{0}】总拣货数量大于申请数量".L10nFormat(assetReqFixture.LineNo));
                        }

                        assetReqFixture.PickedQty = pickQty;
                        RF.Save(assetReqFixture);
                    }
                }

                trans.Complete();
            }
        }

        /// <summary>
        /// 根据发放单创建设备台账位置信息
        /// </summary>
        /// <param name="assetIssue">资产发放单</param>
        /// <param name="locationInfo">领用单位置信息</param>
        /// <returns>设备台账位置信息</returns>
        public virtual string CreateLocationInfoByAssetIssue(AssetIssue assetIssue, string locationInfo)
        {
            StringBuilder sb = new StringBuilder();

            if (assetIssue.External)
            {
                if (assetIssue.ExternalType == Enums.ExternalType.Supply)
                {
                    sb.Append(assetIssue.SupplierCode);
                    sb.Append("-");
                    sb.Append(assetIssue.SupplierName);
                    sb.Append("-");
                }

                if (assetIssue.ExternalType == Enums.ExternalType.Customer)
                {
                    sb.Append(assetIssue.CustomerCode);
                    sb.Append("-");
                    sb.Append(assetIssue.CustomerName);
                    sb.Append("-");
                }

                if (assetIssue.ExternalType == Enums.ExternalType.Other)
                {
                    sb.Append(assetIssue.Destination);
                    sb.Append("-");
                }
            }

            sb.Append(locationInfo);

            return sb.ToString();
        }

        /// <summary>
        /// 获取资产发放单集合
        /// </summary>
        /// <param name="idList">发放单Id集合</param>
        /// <returns>资产发放单集合</returns>
        public virtual EntityList<AssetIssue> GetAssetIssueListByIds(IList<double> idList)
        {
            return idList.SplitContains((ids) =>
            {
                return Query<AssetIssue>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(AssetIssue.AssetIssueEquipmentListProperty).LoadWith(AssetIssue.AssetIssueFixtureListProperty));
            });

        }

        /// <summary>
        /// 删除资产发放单
        /// </summary>
        /// <param name="list">待删除的资产发放单集合</param>
        /// <param name="assetIssueList">待更新的资产发放单集合</param>
        public virtual void DeleteAssetIssues(EntityList list, EntityList<AssetIssue> assetIssueList)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var assetIssue in assetIssueList)
                {
                    if (assetIssue.AssetRequisition.AssetObject == Enums.AssetObject.Equipment)
                    {
                        foreach (var assetIssueEquipment in assetIssue.AssetIssueEquipmentList)
                        {
                            DB.Update<AssetRequisitionEquipment>()
                                .Set(p => p.PickedQty, p => p.PickedQty - 1)
                                .Where(p => p.Id == assetIssueEquipment.AssetRequisitionEquipmentId)
                                .Execute();
                        }
                    }

                    if (assetIssue.AssetRequisition.AssetObject == Enums.AssetObject.Fixture)
                    {
                        foreach (var assetIssueFixture in assetIssue.AssetIssueFixtureList)
                        {
                            DB.Update<AssetRequisitionFixture>()
                                .Set(p => p.PickedQty, p => p.PickedQty - assetIssueFixture.Qty)
                                .Where(p => p.Id == assetIssueFixture.AssetRequisitionFixtureId)
                                .Execute();
                        }
                    }
                }

                RF.Save(list);
                list.MarkSaved();

                trans.Complete();
            }
        }

        /// <summary>
        /// 提交资产发放单
        /// </summary>
        /// <param name="selectedIds">发放单Id集合</param>
        public virtual void SumbitAssetIssues(List<double> selectedIds)
        {
            var configValue = GetApprovalFlowConfigValue();
            var assetIssues = GetAssetIssueListByIds(selectedIds);
            var nowDate = RF.Find<AssetIssue>().GetDbTime();

            if (assetIssues.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有状态为【待提交】、【驳回】的数据才能提交".L10N());
            }
            var recordIds = new List<double>();
            foreach (var item in assetIssues)
            {
                item.ApprovalStatus = ApprovalStatus.PendingReview;
                recordIds.Add(item.Id);
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(assetIssues);
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(recordIds, typeof(AssetIssue).FullName, ApprovalResult.Submit, nowDate, "");
                //因为审批有单独的逻辑需要，所以单独执行
                if (!configValue.EnableAudit)
                {
                    ApprovalAssetIssuesInner(selectedIds, ApprovalResult.Pass, "通过!".L10N(), assetIssues);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 发放单审批通过后更新设备台账的相关信息
        /// </summary>
        /// <param name="assetIssueList">发放单集合</param>
        public virtual void UpdateAssetIssueEquipments(List<AssetIssue> assetIssueList)
        {
            var assetIssueEquipments = GetAssetIssueEquipmentsBytIssueIds(assetIssueList.Select(x => x.Id).Distinct().ToList());

            foreach (var assetIssue in assetIssueList)
            {
                if (!assetIssueEquipments.Any(x => x.AssetIssueId == assetIssue.Id))
                {
                    throw new ValidationException("要提交【资产发放单】，没有明细不能提交。".L10N());
                }
            }


            var requisitionList = assetIssueList.Select(p => p.AssetRequisitionId).SplitContains(tempIds =>
            {
                return Query<AssetRequisition>().Where(p => tempIds.Contains(p.Id))
                    .ToList(null, new EagerLoadOptions().LoadWith(AssetRequisition.AssetRequisitionEquipmentListProperty));
            });

            foreach (var assetIssue in assetIssueList)
            {
                var requisition = requisitionList.First(p => p.Id == assetIssue.AssetRequisitionId);

                foreach (var assetEquip in assetIssueEquipments.Where(x => x.AssetIssueId == assetIssue.Id))
                {
                    var reqEquipment = requisition.AssetRequisitionEquipmentList.First(p => p.Id == assetEquip.AssetRequisitionEquipmentId);

                    //更新设备台账
                    DB.Update<EquipAccount>()
                        .Set(p => p.UseDepartmentId, p => (assetIssue.RequisitionType == Enums.RequisitionType.Consume ? assetIssue.ApplyDepartmentId : p.UseDepartmentId))
                        .Set(p => p.ResPersonId, p => (assetIssue.RequisitionType == Enums.RequisitionType.Consume ? assetIssue.EmployeeId : p.ResPersonId))
                        .Set(p => p.WorkShopId, reqEquipment.WorkShopId)
                        .Set(p => p.ResourceId, reqEquipment.ResourceId)
                        .Set(p => p.AdministratorId, reqEquipment.DepositaryId)
                        .Set(p => p.WarehouseId, p => null)
                        .Set(p => p.StorageLocationId, p => null)
                        .Set(p => p.UseState, assetIssue.External ? Core.Enums.AccountUseState.Lease : Core.Enums.AccountUseState.Using)
                        .Set(p => p.InstallationLocation, CreateLocationInfoByAssetIssue(assetIssue, reqEquipment.Location))
                        .Where(p => p.Id == assetEquip.EquipAccountId)
                        .Execute();

                    //更新设备立卡
                    DB.Update<EquipmentCard>()
                        .Set(p => p.UseDepartmentId, p => (assetIssue.RequisitionType == Enums.RequisitionType.Consume ? assetIssue.ApplyDepartmentId : p.UseDepartmentId))
                        .Set(p => p.UserId, p => (assetIssue.RequisitionType == Enums.RequisitionType.Consume ? assetIssue.EmployeeId : p.UserId))
                        .Set(p => p.WorkShopId, reqEquipment.WorkShopId)
                        .Set(p => p.ResourceId, reqEquipment.ResourceId)
                        .Set(p => p.AdministratorId, reqEquipment.DepositaryId)
                        .Set(p => p.WarehouseId, p => null)
                        .Set(p => p.StorageLocationId, p => null)
                        .Set(p => p.AccountUseState, assetIssue.External ? Core.Enums.AccountUseState.Lease : Core.Enums.AccountUseState.Using)
                        .Set(p => p.InstallationLocation, CreateLocationInfoByAssetIssue(assetIssue, reqEquipment.Location))
                        .Where(p => p.Code == assetEquip.EquipAccount.Code)
                        .Execute();

                    //更新设备履历
                    EquipAccountResume resume = new EquipAccountResume();
                    resume.EquipAccountId = (double)assetEquip.EquipAccountId;
                    resume.State = assetEquip.EquipAccount.State;
                    resume.ResumeType = assetIssue.RequisitionType == Enums.RequisitionType.Consume ? ResumeType.RequisitionIssue : ResumeType.LendingIssue;
                    resume.No = assetIssue.IssueNo;
                    RF.Save(resume);

                    //更新资产领用设备清单的发放数量
                    reqEquipment.IssuedQty += 1;
                    RF.Save(reqEquipment);
                }

                //更新资产领用单的发放状态
                if (requisition.AssetRequisitionEquipmentList.Sum(p => p.Qty) == requisition.AssetRequisitionEquipmentList.Sum(p => p.IssuedQty))
                {
                    requisition.IssueStatus = Enums.IssueStatus.Done;
                }
                else
                {
                    requisition.IssueStatus = Enums.IssueStatus.PartDone;
                }

                RF.Save(requisition);
            }
        }

        /// <summary>
        /// 发放单审批通过后更新工治具的相关信息
        /// </summary>
        /// <param name="assetIssueList">发放单集合</param>
        public virtual void UpdateAssetIssueFixtures(List<AssetIssue> assetIssueList)
        {
            var assetIssueFixtures = GetAssetIssueFixturesBytIssueIds(assetIssueList.Select(x => x.Id).Distinct().ToList());

            foreach (var assetIssue in assetIssueList)
            {
                if (!assetIssueFixtures.Any(x => x.AssetIssueId == assetIssue.Id))
                {
                    throw new ValidationException("要提交【资产发放单】，没有明细不能提交。".L10N());
                }
            }


            var requisitionList = assetIssueList.Select(p => p.AssetRequisitionId).SplitContains(tempIds =>
            {
                return Query<AssetRequisition>().Where(p => tempIds.Contains(p.Id))
                    .ToList(null, new EagerLoadOptions().LoadWith(AssetRequisition.AssetRequisitionFixtureListProperty));
            });

            var fixtureAccountList = assetIssueList.SelectMany(p => p.AssetIssueFixtureList).Select(p => p.AssetRequisitionFixture.FixtureEncodeId).SplitContains(tempIds =>
              {
                  return Query<FixtureCodeAccount>().Where(p => tempIds.Contains(p.FixtureEncodeId) && p.FixtureEncode.FixtureModel.ManageMode == ManageMode.Code).ToList(null, new EagerLoadOptions().LoadWith(FixtureCodeAccount.FixtureEncodeProperty).LoadWith(FixtureCodeAccount.StockListProperty));
              });

            foreach (var assetIssue in assetIssueList)
            {
                var requisition = requisitionList.First(p => p.Id == assetIssue.AssetRequisitionId);

                foreach (var assetFixture in assetIssueFixtures.Where(x => x.AssetIssueId == assetIssue.Id))
                {
                    var reqFixture = requisition.AssetRequisitionFixtureList.First(p => p.Id == assetFixture.AssetRequisitionFixtureId);

                    if (assetFixture.FixtureAccountId != null)
                    {
                        //更新ID类工治具台账
                        DB.Update<FixtureIDAccount>()
                            .Set(p => p.AccountState, requisition.External ? FixtureAccountState.Lending : FixtureAccountState.Requisition)
                            .Where(p => p.Id == assetFixture.FixtureAccountId)
                            .Execute();

                        //删除ID类工治具台账库存
                        DB.Delete<FixtureAccountStock>().Where(p => p.FixtureAccountId == assetFixture.FixtureAccountId).Execute();
                    }
                    else
                    {
                        var fixtureAccount = fixtureAccountList.First(p => p.FixtureEncodeId == reqFixture.FixtureEncodeId);
                        var fixtureAccountStock = fixtureAccount.StockList.FirstOrDefault(p => p.FixtureStorageLocationId == assetFixture.StorageLocationId);

                        if (fixtureAccountStock == null)
                        {
                            throw new ValidationException("工治具编码【{0}】的库存数不足，无法发放".L10nFormat(fixtureAccount.FixtureEncode.Code));
                        }

                        int stockQty = assetFixture.QualityStatus == FixtureQualityState.Pass ? fixtureAccountStock.PassQty : fixtureAccountStock.NgQty;

                        if (stockQty < assetFixture.Qty)
                        {
                            throw new ValidationException("工治具编码【{0}】的库存数不足，无法发放".L10nFormat(fixtureAccount.FixtureEncode.Code));
                        }

                        fixtureAccountStock.PassQty = assetFixture.QualityStatus == FixtureQualityState.Pass ? fixtureAccountStock.PassQty - (int)assetFixture.Qty : fixtureAccountStock.PassQty;
                        fixtureAccountStock.NgQty = assetFixture.QualityStatus == FixtureQualityState.Ng ? fixtureAccountStock.NgQty - (int)assetFixture.Qty : fixtureAccountStock.NgQty;
                        fixtureAccountStock.TotalQty = fixtureAccountStock.TotalQty - (int)assetFixture.Qty;

                        //更新库存详情的合格或不合格数量、总数量
                        RF.Save(fixtureAccountStock);

                        fixtureAccount.TotalQty -= (int)assetFixture.Qty;
                        fixtureAccount.InStockQty -= (int)assetFixture.Qty;
                        fixtureAccount.PassQty = assetFixture.QualityStatus == FixtureQualityState.Pass ? fixtureAccount.PassQty - (int)assetFixture.Qty : fixtureAccount.PassQty;
                        fixtureAccount.NgQty = assetFixture.QualityStatus == FixtureQualityState.Ng ? fixtureAccount.NgQty - (int)assetFixture.Qty : fixtureAccount.NgQty;

                        //更新工治具编码台账的库存数量
                        RF.Save(fixtureAccount);
                    }

                    //更新资产领用设备清单的发放数量
                    reqFixture.IssuedQty += assetFixture.FixtureAccountId != null ? 1 : (int)assetFixture.Qty;
                    RF.Save(reqFixture);
                }

                //更新资产领用单的发放状态
                if (requisition.AssetRequisitionFixtureList.Sum(p => p.Qty) == requisition.AssetRequisitionFixtureList.Sum(p => p.IssuedQty))
                {
                    requisition.IssueStatus = Enums.IssueStatus.Done;
                }
                else
                {
                    requisition.IssueStatus = Enums.IssueStatus.PartDone;
                }
                RF.Save(requisition);
            }

        }

        /// <summary>
        /// 撤回资产发放单
        /// </summary>
        /// <param name="selectedIds">发放单Id集合</param>
        public virtual void CancelAssetIssues(List<double> selectedIds)
        {
            var assetIssues = GetAssetIssueListByIds(selectedIds);
            if (assetIssues.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能操作".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                List<double> assetIssuesIds = new List<double>();
                assetIssues.ForEach(p =>
                {
                    p.ApprovalStatus = ApprovalStatus.Draft;
                    assetIssuesIds.Add(p.Id);
                });
                RF.Save(assetIssues);
                var nowDate = RF.Find<AssetIssue>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(assetIssuesIds, typeof(AssetIssue).FullName, ApprovalResult.Retract, nowDate, "");
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核资产发放单
        /// </summary>
        /// <param name="selectedIds">发放单Id集合</param>
        /// <param name="value">审核结果</param>
        /// <param name="remark">审核意见</param>
        public virtual void ApprovalAssetIssues(List<double> selectedIds, ApprovalResult value, string remark)
        {
            if (!selectedIds.Any())
            {
                throw new ValidationException("传参异常".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                ApprovalAssetIssuesInner(selectedIds, value, remark);
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核资产发放单
        /// </summary>
        /// <param name="selectedIds">发放单Id集合</param>
        /// <param name="value">审核结果</param>
        /// <param name="remark">审核意见</param>
        /// <param name="assetIssueList">数据组</param>
        public virtual void ApprovalAssetIssuesInner(List<double> selectedIds, ApprovalResult value, string remark, EntityList<AssetIssue> assetIssueList = null)
        {
            if (assetIssueList == null)
            {
                assetIssueList = GetAssetIssueListByIds(selectedIds);
                if (!assetIssueList.Any())
                {
                    throw new ValidationException("审核数据异常".L10N());
                }
            }
            //验证只有待审核的数据才能审核
            if (assetIssueList.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            }
            var status = value == ApprovalResult.Pass ? ApprovalStatus.Audited : ApprovalStatus.Reject;
            var nowDate = RF.Find<AssetIssue>().GetDbTime();
            var ids = new List<double>();
            assetIssueList.ForEach(item =>
            {
                item.ApprovalStatus = status;
                ids.Add(item.Id);
            });

            //保存成功之后添加审核记录
            RF.Save(assetIssueList);
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(ids, typeof(AssetIssue).FullName, value, nowDate, remark);

            //执行发放单审批后的逻辑
            if (status == ApprovalStatus.Audited)
            {
                var issueEquipList = assetIssueList.Where(p => p.AssetObject == Enums.AssetObject.Equipment).ToList();
                var issueFixtureList = assetIssueList.Where(p => p.AssetObject == Enums.AssetObject.Fixture).ToList();

                if (issueEquipList.Any())
                {
                    //更新设备台账的相关信息
                    UpdateAssetIssueEquipments(issueEquipList);
                }

                if (issueFixtureList.Any())
                {
                    //更新工治具的相关信息
                    UpdateAssetIssueFixtures(issueFixtureList);
                }
            }
        }
    }
}
