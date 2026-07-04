using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.AssetScraps.Configs;
using SIE.EMS.Checks.Plans;
using SIE.EMS.FixedAssets.Accounts;
using SIE.EMS.Lubrications;
using SIE.EMS.Maintains.Plans;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipmentCards;
using SIE.Equipments.WorkFlows;
using SIE.EventMessages.EMS.MeteringEquipments;
using SIE.EventMessages.EMS.Repairs;
using SIE.EventMessages.EMS.SpecialEquipments;
using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using SIE.EMS.Maintains.Controller;

namespace SIE.EMS.AssetScraps
{
    /// <summary>
    /// 资产报废单控制器
    /// </summary>
    public class AssetScrapController : DomainController
    {
        #region 资产报废单号生成规则
        /// <summary>
        /// 获取自动生成资产报废单号No
        /// </summary>
        /// <returns>编号</returns>
        public virtual string GetNo()
        {
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(AssetScrap));

            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到资产报废单号生成规则,请检查规则配置".L10N());
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
            var configValue = ConfigService.GetConfig<ApprovalConfigValue>(new ApprovalConfig(), typeof(AssetScrap));

            if (configValue == null)
                throw new ValidationException("未找到审批流配置规则,请检查规则配置".L10N());

            return configValue;
        }
        #endregion

        #region 获取是否启用资产处置配置信息
        /// <summary>
        /// 获取是否启用资产处置配置信息
        /// </summary>
        /// <returns>启用资产处置配置项信息</returns>
        public virtual EnableAssetDisposalConfigValue GetEnableAssetDisposalConfigValue()
        {
            var configValue = ConfigService.GetConfig<EnableAssetDisposalConfigValue>(new EnableAssetDisposalConfig(), typeof(AssetScrap));

            if (configValue == null)
                throw new ValidationException("未找到是否启用资产处置配置规则,请检查规则配置".L10N());

            return configValue;
        }
        #endregion

        /// <summary>
        /// 查询资产报废单
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns>资产报废列表</returns>
        public virtual EntityList<AssetScrap> GetAssetScrapList(AssetScrapCriteria criteria)
        {
            var q = Query<AssetScrap>();

            if (criteria.No.IsNotEmpty())
            {
                q.Where(p => p.No.Contains(criteria.No));
            }
            if (criteria.QureyFactoryId != null && criteria.QureyFactoryId != 0)
            {
                q.Where(p => p.FactoryId == criteria.QureyFactoryId);
            }
            if (criteria.AssetObject != null)
            {
                q.Where(p => p.AssetObject == criteria.AssetObject);
            }
            if (criteria.ManageDeptId != null && criteria.ManageDeptId != 0)
            {
                q.Where(p => p.ManageDeptId == criteria.ManageDeptId);
            }
            if (criteria.UseDeptId != null && criteria.UseDeptId != 0)
            {
                q.Where(p => p.UseDeptId == criteria.UseDeptId);
            }
            if (criteria.WarehouseId != null && criteria.WarehouseId != 0)
            {
                q.Where(p => p.WarehouseId == criteria.WarehouseId);
            }
            if (criteria.ApprovalStatus != null)
            {
                q.Where(p => p.ApprovalStatus == criteria.ApprovalStatus);
            }
            if (criteria.ApplicantId != null && criteria.ApplicantId != 0)
            {
                q.Where(p => p.ApplicantId == criteria.ApplicantId);
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
        /// 获取设备清单
        /// </summary>
        /// <param name="assetScrapId">报废单Id</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>设备清单列表</returns>
        public virtual EntityList<AssetScrapEquipment> GetAssetScrapEquipmentList(double assetScrapId, IList<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var q = Query<AssetScrapEquipment>().Where(p => p.AssetScrapId == assetScrapId);

            var list = q.OrderBy(orderInfoList).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            var equipAccountIds = list.Select(p => p.EquipAccountId).ToList();

            //获取设备维修单的工时和成本
            var repairInfoList = RT.Service.Resolve<IEquipRepairBill>().GetEquipRepairWorkHourAndCost(equipAccountIds);

            //获取设备保养单的工时和成本
            var mainPlanInfoList = RT.Service.Resolve<MaintainController>().GetMaintainWorkHourAndCost(equipAccountIds);

            foreach (var item in list)
            {
                var repairInfo = repairInfoList.FirstOrDefault(p => p.EquipAccountId == item.EquipAccountId);
                var mainPlanInfo = mainPlanInfoList.FirstOrDefault(p => p.EquipAccountId == item.EquipAccountId);

                if (repairInfo != null) 
                {
                    item.RepairHours += repairInfo.RepairHours;
                    item.SparePartCost += repairInfo.SparePartCost;
                    item.OutRepairCost += repairInfo.OutRepairCost;
                    item.TotalRepairHours += repairInfo.TotalRepairHours;
                    item.TotalSparePartCost += repairInfo.TotalSparePartCost;
                }

                if (mainPlanInfo != null)
                {
                    item.MaintenanceHours += mainPlanInfo.MaintenanceHours;
                    item.SparePartCost += mainPlanInfo.SparePartCost;
                }
            }

            return list;
        }

        /// <summary>
        /// 根据设备Id获取维修、保养工时和成本
        /// </summary>
        /// <param name="equipAccountId">设备台账Id</param>
        /// <returns>设备清单列表</returns>
        public virtual EntityList<AssetScrapEquipment> GetWorkHourAndCostInfoById(double equipAccountId)
        {
            var list = new EntityList<AssetScrapEquipment>();
            list.Add(new AssetScrapEquipment() { EquipAccountId = equipAccountId });

            var equipAccountIds = new List<double>() { equipAccountId };

            //获取设备维修单的工时和成本
            var repairInfoList = RT.Service.Resolve<IEquipRepairBill>().GetEquipRepairWorkHourAndCost(equipAccountIds);

            //获取设备保养单的工时和成本
            var mainPlanInfoList = RT.Service.Resolve<MaintainController>().GetMaintainWorkHourAndCost(equipAccountIds);

            foreach (var item in list)
            {
                var repairInfo = repairInfoList.FirstOrDefault(p => p.EquipAccountId == item.EquipAccountId);
                var mainPlanInfo = mainPlanInfoList.FirstOrDefault(p => p.EquipAccountId == item.EquipAccountId);

                if (repairInfo != null)
                {
                    item.RepairHours += repairInfo.RepairHours;
                    item.SparePartCost += repairInfo.SparePartCost;
                    item.OutRepairCost += repairInfo.OutRepairCost;
                    item.TotalRepairHours += repairInfo.TotalRepairHours;
                    item.TotalSparePartCost += repairInfo.TotalSparePartCost;
                }

                if (mainPlanInfo != null)
                {
                    item.MaintenanceHours += mainPlanInfo.MaintenanceHours;
                    item.SparePartCost += mainPlanInfo.SparePartCost;
                }
            }

            return list;
        }

        /// <summary>
        /// 根据报废工治具清单明细行获取工治具编码信息
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="scrapFixture">报废工治具清单明细</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>工治具编码列表</returns>
        public virtual EntityList<FixtureEncode> GetAssetScrapFixtureEncodes(PagingInfo pagingInfo, AssetScrapFixture scrapFixture, string keyword)
        {
            var fixtureAccountList = Query<FixtureAccount>().Join<FixtureAccountStock>((a, b) => a.Id == b.FixtureAccountId)
                                           .Where<FixtureAccountStock>((a, b) => b.FixtureWarehouseId == scrapFixture.WarehouseId)
                                           .WhereIf(scrapFixture.IsFixAsset, a => a.FixedAssetsAccountId != null).ToList();

            var fixtureEncodeList = fixtureAccountList.Select(p => p.FixtureEncodeId).Distinct().SplitContains(tempIds =>
            {
                return Query<FixtureEncode>().Where(p => tempIds.Contains(p.Id))
                                             .WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword))
                                             .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            });

            return fixtureEncodeList;
        }

        /// <summary>
        /// 根据报废工治具清单明细行获取工治具ID台账信息
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="scrapFixture">报废工治具清单明细</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>工治具ID台账列表</returns>
        public virtual EntityList<FixtureIDAccount> GetAssetScrapFixtureIDAccounts(PagingInfo pagingInfo, AssetScrapFixture scrapFixture, string keyword)
        {
            var q = Query<FixtureIDAccount>().Join<FixtureAccountStock>((a, b) => a.Id == b.FixtureAccountId)
                                             .WhereIf(scrapFixture.IsFixAsset, p => p.FixedAssetsAccountId != null)
                                             .Where<FixtureAccountStock>((a, b) => a.AccountState == FixtureAccountState.InStorage
                                                                                && a.FixtureEncode.Code == scrapFixture.FixtureEncode.Code
                                                                                && b.FixtureWarehouseId == scrapFixture.WarehouseId);
            if (keyword.IsNotEmpty())
            {
                q.Where(p => p.Code.Contains(keyword));
            }

            var list = q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty().LoadWith(FixtureIDAccount.StockListProperty));

            foreach (var item in list)
            {
                item.LocationId = item.StockList[0].FixtureStorageLocationId;
                item.LocationCode = item.StockList[0].StorageLocation.Code;
                item.ScrapQty = 1;
            }
            return list;
        }

        /// <summary>
        /// 校验保存的设备编码是否存在于未完结的单据中
        /// </summary>
        /// <param name="assetEquips">资产报废设备清单</param>
        /// <returns>bool</returns>
        public virtual bool VerifyIsExistNotAuditAssetScrapEquip(EntityList<AssetScrapEquipment> assetEquips)
        {
            var assetEquipIds = assetEquips.Select(p => p.Id).ToList();
            var assetEquipAccountIds = assetEquips.Select(p => p.EquipAccountId).ToList();

            var list = assetEquipAccountIds.SplitContains(tempIds =>
            {

                return Query<AssetScrapEquipment>()
                    .Where(p => tempIds.Contains(p.EquipAccountId)
                        && !assetEquipIds.Contains(p.Id)
                        && p.AssetScrap.ApprovalStatus != ApprovalStatus.Audited)
                    .ToList();
            });

            return list.Any();
        }

        /// <summary>
        /// 校验保存的工治具序列号是否存在于未完结的单据中
        /// </summary>
        /// <param name="assetFixtures">资产报废工治具清单</param>
        /// <returns>bool</returns>
        public virtual bool VerifyIsExistNotAuditAssetScrapFixture(List<AssetScrapFixture> assetFixtures)
        {
            var assetFixtureIds = assetFixtures.Select(p => p.Id).ToList();
            var assetFixtureAccountIds = assetFixtures.Select(p => p.FixtureAccountId).ToList();

            var list = assetFixtureAccountIds.SplitContains(tempIds =>
            {
                return Query<AssetScrapFixture>()
                .Where(p => tempIds.Contains(p.FixtureAccountId)
                    && !assetFixtureIds.Contains(p.Id)
                    && p.AssetScrap.ApprovalStatus != ApprovalStatus.Audited)
                .ToList();
            });

            return list.Any();
        }

        /// <summary>
        /// 获取资产报废单集合
        /// </summary>
        /// <param name="idList">报废单Id集合</param>
        /// <returns>资产报废单集合</returns>
        public virtual EntityList<AssetScrap> GetAssetScrapListByIds(IList<double> idList)
        {
            return idList.SplitContains((ids) =>
            {
                return Query<AssetScrap>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(AssetScrap.AssetScrapEquipmentListProperty).LoadWith(AssetScrap.AssetScrapFixtureListProperty));
            });
        }

        /// <summary>
        /// 提交资产报废单
        /// </summary>
        /// <param name="selectedIds">报废单Id集合</param>
        public virtual void SumbitAssetScraps(List<double> selectedIds)
        {
            var configValue = GetApprovalFlowConfigValue();
            var assetScraps = GetAssetScrapListByIds(selectedIds);
            if (assetScraps.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有状态为【待提交】、【驳回】的数据才能提交".L10N());
            }
            var nowDate = RF.Find<AssetScrap>().GetDbTime();
            var recordIds = new List<double>();
            foreach (var item in assetScraps)
            {
                item.ApprovalStatus = ApprovalStatus.PendingReview;
                recordIds.Add(item.Id);
            }
            //保存
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(assetScraps);
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(recordIds, typeof(AssetScrap).FullName, ApprovalResult.Submit, nowDate, "");
                if (!configValue.EnableAudit)
                {
                    ApprovalAssetScrapsInner(selectedIds, ApprovalResult.Pass, "通过".L10N(), assetScraps);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 报废单审批通过后更新设备台账的相关信息
        /// </summary>
        /// <param name="assetScrapList">报废单集合</param>
        public virtual void UpdateAssetScrapEquipments(List<AssetScrap> assetScrapList)
        {

            var configValue = GetEnableAssetDisposalConfigValue();
            var equipAccountIds = assetScrapList.SelectMany(p => p.AssetScrapEquipmentList).Select(p => p.EquipAccountId).ToList();

            var fixedAssetEquipList = configValue.IsEnableFixedAssetScrap ? equipAccountIds.SplitContains(tempIds =>
              {
                  return Query<FixedAssetDeviceBill>().Where(p => tempIds.Contains(p.EquipAccountId) && p.IsMajor).ToList();
              }) : new EntityList<FixedAssetDeviceBill>();

            foreach (var assetScrap in assetScrapList)
            {
                foreach (var assetEquip in assetScrap.AssetScrapEquipmentList)
                {
                    //更新设备台账
                    DB.Update<EquipAccount>()
                        .Set(p => p.UseState, Core.Enums.AccountUseState.Scrap)
                        .Where(p => p.Id == assetEquip.EquipAccountId)
                        .Execute();

                    //更新设备立卡
                    DB.Update<EquipmentCard>()
                        .Set(p => p.AccountUseState, Core.Enums.AccountUseState.Scrap)
                        .Where(p => p.Code == assetEquip.EquipAccount.Code)
                        .Execute();

                    //更新设备履历
                    EquipAccountResume resume = new EquipAccountResume();
                    resume.EquipAccountId = assetEquip.EquipAccountId;
                    resume.State = assetEquip.EquipAccount.State;
                    resume.ResumeType = ResumeType.AssetScrap;
                    resume.No = assetScrap.No;
                    RF.Save(resume);

                    //设备关联了固定资产且是主设备时，则更新固定资产的管理状态为【报废】
                    var fixedAssetEquip = fixedAssetEquipList.FirstOrDefault(p => p.EquipAccountId == assetEquip.EquipAccountId);

                    if (fixedAssetEquip != null)
                    {
                        DB.Update<FixedAssetsAccount>()
                          .Set(p => p.ManageStatus, ManageState.Scrap)
                          .Where(p => p.Id == fixedAssetEquip.FixedAssetsAccountId).Execute();
                    }
                }
            }

            //更新未完结的设备维修单状态为关闭
            RT.Service.Resolve<IEquipRepairBill>().CloseEquipRepairBillByEquipAccountIds(equipAccountIds);

            //更新未完结的设备点检单状态为关闭
            var checkPlans = equipAccountIds.SplitContains(tempIds =>
            {
                return Query<CheckPlan>().Where(p => tempIds.Contains(p.EquipAccountId) &&
                          (p.ExeState == Enums.CheckExeState.NotPerformed || p.ExeState == Enums.CheckExeState.Overdue ||
                           p.ExeState == Enums.CheckExeState.Performing)).ToList();
            });
            checkPlans.ForEach(plan =>
            {
                plan.ExeState = Enums.CheckExeState.Closed;
            });
            RF.Save(checkPlans);

            //更新未完结的设备保养单状态为关闭
            var maintainPlans = equipAccountIds.SplitContains(tempIds =>
            {
                return Query<MaintainPlan>().Where(p => tempIds.Contains(p.EquipAccountId) &&
                          (p.ExeState == Enums.MaintExeState.NotPerformed || p.ExeState == Enums.MaintExeState.Overdue ||
                           p.ExeState == Enums.MaintExeState.Performing)).ToList();
            });

            maintainPlans.ForEach(plan =>
            {
                plan.ExeState = Enums.MaintExeState.Closed;
            });
            RF.Save(maintainPlans);

            //更新未完结的设备润滑任务状态为关闭
            var lubrications = equipAccountIds.SplitContains(tempIds =>
            {
                return Query<Lubrication>().Where(p => tempIds.Contains(p.EquipAccountId) &&
                          (p.LubricationStatus == Enums.LubricationStatus.Pending || p.LubricationStatus == Enums.LubricationStatus.Doing)).ToList();
            });

            lubrications.ForEach(task =>
            {
                task.LubricationStatus = Enums.LubricationStatus.Closed;
            });
            RF.Save(lubrications);

            //更新未完结的设备特种定检任务状态为关闭
            RT.Service.Resolve<IRegularInspection>().CloseRegularInspectionByEquipAccountIds(equipAccountIds);

            //更新未完结的设备计量定检任务状态为关闭
            RT.Service.Resolve<ICalibration>().CloseCalibrationByEquipAccountIds(equipAccountIds);
        }

        /// <summary>
        /// 报废单审批通过后更新工治具的相关信息
        /// </summary>
        /// <param name="assetScrapList">报废单集合</param>
        public virtual void UpdateAssetScrapFixtures(List<AssetScrap> assetScrapList)
        {

            var configValue = GetEnableAssetDisposalConfigValue();

            //批量查询编码类工治具台账
            var fixtureAccountList = assetScrapList.SelectMany(p => p.AssetScrapFixtureList).Where(p => p.FixtureAccountId == null).Select(p => p.FixtureEncodeId).SplitContains(tempIds =>
              {
                  return Query<FixtureAccount>().Where(p => tempIds.Contains(p.FixtureEncodeId)).ToList(null, new EagerLoadOptions().LoadWith(FixtureCodeAccount.StockListProperty));
              });

            //批量查询ID类工治具台账
            fixtureAccountList.AddRange(assetScrapList.SelectMany(p => p.AssetScrapFixtureList).Where(p => p.FixtureAccountId != null).Select(p => p.FixtureAccountId).SplitContains(tempIds =>
            {
                return Query<FixtureAccount>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWith(FixtureCodeAccount.StockListProperty));
            }));

            //批量查询报废单明细
            var assetScrapFixtureList = assetScrapList.Select(p => p.Id).SplitContains(tempIds =>
            {
                return Query<AssetScrapFixture>().Where(p => tempIds.Contains(p.AssetScrapId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            //批量查询固定资产工治具明细
            var fixedAssetFixtureList = assetScrapList.SelectMany(p => p.AssetScrapFixtureList).Where(p => p.FixtureAccountId != null).Select(p => p.FixtureAccountId).SplitContains(tempIds =>
            {
                return Query<FixedAssetFixtureBill>().Where(p => tempIds.Contains(p.FixtureIDAccountId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            foreach (var assetScrap in assetScrapList)
            {
                foreach (var assetFixture in assetScrap.AssetScrapFixtureList)
                {
                    var assetScrapFixture = assetScrapFixtureList.First(p => p.Id == assetFixture.Id);

                    var fixtureAccount = assetFixture.FixtureAccountId == null ?
                        fixtureAccountList.First(p => p.FixtureEncodeId == assetFixture.FixtureEncodeId) :
                        fixtureAccountList.First(p => p.Id == assetFixture.FixtureAccountId);

                    var fixtureStock = fixtureAccount.StockList.FirstOrDefault(p => p.FixtureWarehouseId == assetScrap.WarehouseId
                                                                                 && p.FixtureStorageLocationId == assetFixture.StorageLocationId
                                                                                 && (assetFixture.FixtureQualityState == FixtureQualityState.Pass ? p.PassQty : p.NgQty) >= assetFixture.Qty);
                    if (fixtureStock != null)
                    {
                        //扣减工治具台账和库存详情的合格数/不合格数，增加台账的报废数
                        fixtureAccount.PassQty -= assetScrapFixture.FixtureQualityState == FixtureQualityState.Pass ? assetScrapFixture.Qty : 0;
                        fixtureAccount.NgQty -= assetScrapFixture.FixtureQualityState == FixtureQualityState.Ng ? assetScrapFixture.Qty : 0;
                        fixtureAccount.ScrapQty += assetScrapFixture.Qty;
                        fixtureAccount.AccountState = assetFixture.FixtureAccountId != null ? FixtureAccountState.Scrap : fixtureAccount.AccountState;

                        fixtureStock.PassQty -= assetScrapFixture.FixtureQualityState == FixtureQualityState.Pass ? assetScrapFixture.Qty : 0;
                        fixtureStock.NgQty -= assetScrapFixture.FixtureQualityState == FixtureQualityState.Ng ? assetScrapFixture.Qty : 0;
                        fixtureStock.TotalQty -= assetScrapFixture.Qty;
                        fixtureStock.PersistenceStatus = fixtureStock.TotalQty == 0 ? PersistenceStatus.Deleted : PersistenceStatus.Modified;

                        RF.Save(fixtureAccount);
                        RF.Save(fixtureStock);

                        //增加库存详情的报废数
                        FixtureAccountStock newFixtureStock = null;
                        newFixtureStock = fixtureAccount.StockList.FirstOrDefault(p => p.FixtureWarehouseId == assetScrapFixture.ScrapWarehouseId && p.FixtureStorageLocationId == assetScrapFixture.ScrapLocationId);
                        if (newFixtureStock != null)
                        {
                            newFixtureStock.ScrapQty += assetScrapFixture.Qty;
                            newFixtureStock.TotalQty += assetScrapFixture.Qty;
                        }
                        else
                        {
                            newFixtureStock = new FixtureAccountStock();
                            newFixtureStock.FixtureAccountId = fixtureAccount.Id;
                            newFixtureStock.FixtureWarehouseId = assetScrapFixture.ScrapWarehouseId;
                            newFixtureStock.FixtureStorageLocationId = assetScrapFixture.ScrapLocationId;
                            newFixtureStock.PassQty = 0;
                            newFixtureStock.NgQty = 0;
                            newFixtureStock.ScrapQty = assetScrapFixture.Qty;
                            newFixtureStock.TotalQty = assetScrapFixture.Qty;
                        }
                        RF.Save(newFixtureStock);
                    }
                    else
                    {
                        throw new ValidationException("工治具编码【{0}】,仓库编码【{1}】,库位编码【{2}】,质量状态【{3}】的库存不足以扣减报废数量!".L10nFormat(assetScrapFixture.FixtureEncodeCode, assetScrapFixture.WarehouseCode, assetScrapFixture.StorageLocationCode, assetScrapFixture.FixtureQualityState.ToLabel()));
                    }

                    //配置项【是否启用资产处置】未勾选时，且工治具ID关联了固定资产，
                    //获取关联同一个固定资产的工治具ID的状态是否都为【报废、处置】，是则更新固定资产的管理状态为【报废】
                    if (assetFixture.FixtureAccountId != null && !configValue.IsEnableAssetDisposal)
                    {
                        UpdateFixedAssetFixtureStatus(fixedAssetFixtureList, assetFixture);
                    }
                }
            }
        }

        /// <summary>
        /// 根据工治具明细报废情况更新固定资产状态为报废
        /// </summary>
        /// <param name="fixedAssetFixtureList">固定资产工治具清单</param>
        /// <param name="assetFixture">工治具报废明细</param>
        public virtual void UpdateFixedAssetFixtureStatus(IList<FixedAssetFixtureBill> fixedAssetFixtureList, AssetScrapFixture assetFixture)
        {
            var fixedAssetFixture = fixedAssetFixtureList.FirstOrDefault(p => p.FixtureIDAccountId == assetFixture.FixtureAccountId);

            if (fixedAssetFixture != null)
            {
                fixedAssetFixture.AccountState = FixtureAccountState.Scrap;
                var associateFixedAssetFixtureList = fixedAssetFixtureList.Where(p => p.FixedAssetsAccountId == fixedAssetFixture.FixedAssetsAccountId);

                if (associateFixedAssetFixtureList.Count(p => p.AccountState == FixtureAccountState.Scrap) == associateFixedAssetFixtureList.Count())
                {
                    DB.Update<FixedAssetsAccount>().Set(p => p.ManageStatus, ManageState.Scrap).Where(p => p.Id == fixedAssetFixture.FixedAssetsAccountId).Execute();
                }
            }
        }

        /// <summary>
        /// 撤回资产报废单
        /// </summary>
        /// <param name="selectedIds">报废单Id集合</param>
        public virtual void CancelAssetScraps(List<double> selectedIds)
        {
            var assetScraps = GetAssetScrapListByIds(selectedIds);
            if (assetScraps.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能操作".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                List<double> assetScrapsIds = new List<double>();
                assetScraps.ForEach(p =>
                {
                    p.ApprovalStatus = ApprovalStatus.Draft;
                    assetScrapsIds.Add(p.Id);
                });
                RF.Save(assetScraps);
                var nowDate = RF.Find<AssetScrap>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(assetScrapsIds, typeof(AssetScrap).FullName, ApprovalResult.Retract, nowDate, "");
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核资产报废单
        /// </summary>
        /// <param name="selectedIds">报废单Id集合</param>
        /// <param name="value">审核结果</param>
        /// <param name="remark">审核意见</param>
        public virtual void ApprovalAssetScraps(List<double> selectedIds, ApprovalResult value, string remark)
        {
            if (!selectedIds.Any())
            {
                throw new ValidationException("传参异常".L10N());
            }

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                ApprovalAssetScrapsInner(selectedIds, value, remark);
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核资产报废单
        /// </summary>
        /// <param name="selectedIds">报废单Id集合</param>
        /// <param name="value">审核结果</param>
        /// <param name="remark">审核意见</param>
        /// <param name="assetScrapList">数据组</param>
        public virtual void ApprovalAssetScrapsInner(List<double> selectedIds, ApprovalResult value, string remark, EntityList<AssetScrap> assetScrapList = null)
        {
            if (assetScrapList == null)
            {
                assetScrapList = GetAssetScrapListByIds(selectedIds);
                if (!assetScrapList.Any())
                {
                    throw new ValidationException("审核数据异常".L10N());
                }
            }
            //验证只有待审核的数据才能审核
            if (assetScrapList.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            }

            var status = value == ApprovalResult.Pass ? ApprovalStatus.Audited : ApprovalStatus.Reject;
            var nowDate = RF.Find<AssetScrap>().GetDbTime();
            var ids = new List<double>();
            assetScrapList.ForEach(item =>
            {
                item.ApprovalStatus = status;
                ids.Add(item.Id);
            });

            //保存成功之后添加审核记录
            RF.Save(assetScrapList);
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(ids, typeof(AssetScrap).FullName, value, nowDate, remark);

            //执行报废单审批后的逻辑
            if (status == ApprovalStatus.Audited)
            {
                var scrapEquipList = assetScrapList.Where(p => p.AssetObject == Enums.AssetObject.Equipment).ToList();
                var scrapFixtureList = assetScrapList.Where(p => p.AssetObject == Enums.AssetObject.Fixture).ToList();

                if (scrapEquipList.Any())
                {
                    //更新设备台账的相关信息
                    UpdateAssetScrapEquipments(scrapEquipList);
                }

                if (scrapFixtureList.Any())
                {
                    //更新工治具的相关信息
                    UpdateAssetScrapFixtures(scrapFixtureList);
                }
            }
        }

        /// <summary>
        /// 获取工治具清单
        /// </summary>
        /// <param name="assetScrapId">报废单Id</param>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>工治具清单列表</returns>
        public virtual EntityList<AssetScrapFixture> GetAssetScrapFixtureList(double assetScrapId, double warehouseId, IList<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var q = Query<AssetScrapFixture>().Where(p => p.AssetScrapId == assetScrapId);

            var list = q.OrderBy(orderInfoList).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            //查询工治具清单的库位库存数
            var encodeIds = list.Select(p => p.FixtureEncodeId).Distinct().ToList();
            var locationIds = list.Select(p => p.StorageLocationId).Distinct().ToList();
            var stocklist = RT.Service.Resolve<CoreFixtureController>().GetCanUseNumByWarehouseId(warehouseId, encodeIds, locationIds);

            foreach (var item in list)
            {
                item.StoreUsableQty = 0;
                FixtureAccountStock stock = stocklist.FirstOrDefault(p => p.EncodeId == item.FixtureEncodeId && p.FixtureStorageLocationId == item.StorageLocationId);

                if (stock != null)
                {
                    if (item.FixtureQualityState == FixtureQualityState.Pass)
                    {
                        item.StoreUsableQty = stock.PassQty;
                    }
                    else if (item.FixtureQualityState == FixtureQualityState.Ng)
                    {
                        item.StoreUsableQty = stock.NgQty;
                    }
                    else
                    {
                        item.StoreUsableQty = stock.TotalQty;
                    }
                }
            }

            return list;
        }
    }
}
