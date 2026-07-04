using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.Import;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.AssetDisposals.Import;
using SIE.EMS.AssetScraps;
using SIE.EMS.FixedAssets.Accounts;
using SIE.EMS.SpareParts;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipmentCards;
using SIE.Equipments.WorkFlows;
using SIE.Fixtures.Fixtures.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.AssetDisposals
{
    /// <summary>
    /// 资产处置单控制器
    /// </summary>
    public class AssetDisposalController : DomainController
    {
        #region 资产处置单号生成规则
        /// <summary>
        /// 获取自动生成资产处置单号No
        /// </summary>
        /// <returns>编号</returns>
        public virtual string GetNo()
        {
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(AssetDisposal));

            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到资产处置单号生成规则,请检查规则配置".L10N());
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
            var configValue = ConfigService.GetConfig<ApprovalConfigValue>(new ApprovalConfig(), typeof(AssetDisposal));

            if (configValue == null)
                throw new ValidationException("未找到审批流配置规则,请检查规则配置".L10N());

            return configValue;
        }
        #endregion

        /// <summary>
        /// 查询资产处置单
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns>资产处置列表</returns>
        public virtual EntityList<AssetDisposal> GetAssetDisposalList(AssetDisposalCriteria criteria)
        {
            var q = Query<AssetDisposal>();

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
        /// 根据处置工治具清单明细行获取工治具ID台账信息
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="disposalFixture">处置工治具清单明细</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>工治具ID台账列表</returns>
        public virtual EntityList<FixtureIDAccount> GetFixtureIDAccounts(PagingInfo pagingInfo, AssetDisposalFixture disposalFixture, string keyword)
        {
            var q = Query<FixtureIDAccount>().Join<FixtureAccountStock>((a, b) => a.Id == b.FixtureAccountId)
                                             .Where<FixtureAccountStock>((a, b) => a.AccountState == FixtureAccountState.Scrap
                                                                                && a.FixtureEncodeId == disposalFixture.FixtureEncodeId
                                                                                && b.FixtureWarehouseId == disposalFixture.WarehouseId);
            if (keyword.IsNotEmpty())
            {
                q.Where(p => p.Code.Contains(keyword));
            }

            var list = q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            var assetScrapFixtureList = list.Select(p => p.Id).SplitContains(tempIds =>
            {
                return Query<AssetScrapFixture>().Where(p => tempIds.Contains((double)p.FixtureAccountId)).ToList();
            });

            var fixedAssetFixtureList = list.Where(p => p.FixedAssetsAccountId != null).Select(p => p.Id).SplitContains(tempIds =>
            {
                return Query<FixedAssetFixtureBill>().Where(p => tempIds.Contains(p.FixtureIDAccountId) && p.IsMajor).ToList();
            });

            list.ForEach(account =>
            {
                var assetScrapFixture = assetScrapFixtureList.FirstOrDefault(p => p.FixtureAccountId == account.Id);
                var fixedAssetFixture = fixedAssetFixtureList.FirstOrDefault(p => p.FixtureIDAccountId == account.Id);

                if (assetScrapFixture != null)
                {
                    account.ScrapType = assetScrapFixture.ScrapType;
                    account.Reason = assetScrapFixture.Reason;
                }

                if (fixedAssetFixture == null)
                {
                    account.OriginalAssetsValue = 0;
                    account.NetAssetValue = 0;
                    account.DepreciationResidualValue = 0;
                }

                account.TreePId = null;
            });

            return list;
        }

        /// <summary>
        /// 获取资产处置单集合
        /// </summary>
        /// <param name="idList">处置单Id集合</param>
        /// <returns>资产处置单集合</returns>
        public virtual EntityList<AssetDisposal> GetAssetDisposalListByIds(IList<double> idList)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(AssetDisposal.AssetDisposalEquipmentListProperty);
            elo.LoadWith(AssetDisposal.AssetDisposalFixtureListProperty);
            elo.LoadWith(AssetDisposal.AssetDisposalSparePartListProperty);
            elo.LoadWithViewProperty();
            return idList.SplitContains((ids) =>
            {
                return Query<AssetDisposal>().Where(p => ids.Contains(p.Id)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 查找备件回收种所有的批次号
        /// </summary>
        /// <returns></returns>
        public virtual List<string> GteAssetDisposalSparePartAllLotNo()
        {
            return Query<AssetDisposalSparePart>().Select(p => p.LotNo).Where(p => p.LotNo != null || p.LotNo != "").Distinct().ToList<string>().ToList();
        }

        /// <summary>
        /// 查找备件回收种所有的序列号
        /// </summary>
        /// <returns></returns>
        public virtual List<string> GteAssetDisposalSparePartAllSn()
        {
            return Query<AssetDisposalSparePart>().Select(p => p.Sn).Where(p=>p.Sn!=null || p.Sn!="").Distinct().ToList<string>().ToList();
        }

        /// <summary>
        /// 获取备件回收记录集合
        /// </summary>
        /// <param name="idList">备件回收记录Id集合</param>
        /// <returns>备件回收记录集合</returns>
        public virtual EntityList<AssetDisposalSparePart> GetAssetDisposalSparePartListByIds(IList<double> idList)
        {
            return idList.SplitContains((ids) =>
            {
                return Query<AssetDisposalSparePart>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

        }

        /// <summary>
        /// 提交资产处置单
        /// </summary>
        /// <param name="selectedIds">处置单Id集合</param>
        public virtual void SumbitAssetDisposals(List<double> selectedIds)
        {
            var configValue = GetApprovalFlowConfigValue();
            //加载数据
            var assetDisposals = GetAssetDisposalListByIds(selectedIds);
            if (assetDisposals.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有状态为【待提交】、【驳回】的数据才能提交".L10N());
            }
            var recordIds = new List<double>();
            //修改状态
            foreach (var item in assetDisposals)
            {
                item.ApprovalStatus = ApprovalStatus.PendingReview;
                recordIds.Add(item.Id);
            }
            var nowDate = RF.Find<AssetDisposal>().GetDbTime();
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //保存集合
                RF.Save(assetDisposals);
                //保存提交记录
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(recordIds, typeof(AssetDisposal).FullName, ApprovalResult.Submit, nowDate, "");
                //是否启用审批为false时提交后自动审批
                if (!configValue.EnableAudit)
                {
                    ApprovalAssetDisposalsInner(selectedIds, ApprovalResult.Pass, "通过".L10N(), assetDisposals);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 处置单审批通过后更新设备台账的相关信息
        /// </summary>
        /// <param name="assetDisposalList">处置单集合</param>
        public virtual void UpdateAssetDisposalEquipments(List<AssetDisposal> assetDisposalList)
        {

            var equipAccountIds = assetDisposalList.SelectMany(p => p.AssetDisposalEquipmentList).Select(p => p.EquipAccountId).ToList();

            var fixedAssetEquipList = equipAccountIds.SplitContains(tempIds =>
            {
                return Query<FixedAssetDeviceBill>().Where(p => tempIds.Contains(p.EquipAccountId) && p.IsMajor).ToList();
            });

            foreach (var assetDisposal in assetDisposalList)
            {
                foreach (var assetEquip in assetDisposal.AssetDisposalEquipmentList)
                {
                    //更新设备台账
                    DB.Update<EquipAccount>()
                        .Set(p => p.UseState, Core.Enums.AccountUseState.DisposedOf)
                        .Where(p => p.Id == assetEquip.EquipAccountId)
                        .Execute();

                    //更新设备立卡
                    DB.Update<EquipmentCard>()
                        .Set(p => p.AccountUseState, Core.Enums.AccountUseState.DisposedOf)
                        .Where(p => p.Code == assetEquip.EquipAccount.Code)
                        .Execute();

                    //更新设备履历
                    EquipAccountResume resume = new EquipAccountResume();
                    resume.EquipAccountId = assetEquip.EquipAccountId;
                    resume.State = assetEquip.EquipAccount.State;
                    resume.ResumeType = ResumeType.AssetDisposal;
                    resume.No = assetDisposal.No;
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

        }

        /// <summary>
        /// 处置单审批通过后更新工治具的相关信息
        /// </summary>
        /// <param name="assetDisposalList">处置单集合</param>
        public virtual void UpdateAssetDisposalFixtures(List<AssetDisposal> assetDisposalList)
        {

            var fixedAssetFixtureList = assetDisposalList.SelectMany(p => p.AssetDisposalFixtureList).Select(p => p.FixtureAccountId)
                                                         .SplitContains(tempIds =>
                                                         {
                                                             return Query<FixedAssetFixtureBill>().Where(p => tempIds.Contains(p.FixtureIDAccountId) && p.IsMajor).ToList();
                                                         });

            foreach (var assetDisposal in assetDisposalList)
            {
                var fixtureAccountIds = assetDisposal.AssetDisposalFixtureList.Select(p => p.FixtureAccountId);

                foreach (var fixtureAccountId in fixtureAccountIds)
                {
                    //更新工治具ID台账
                    DB.Update<FixtureIDAccount>()
                        .Set(p => p.AccountState, FixtureAccountState.Disposal)
                        .Where(p => p.Id == fixtureAccountId)
                        .Execute();

                    //工治具关联了固定资产且是主工治具时，则更新固定资产的管理状态为【报废】
                    var fixedAssetEquip = fixedAssetFixtureList.FirstOrDefault(p => p.FixtureIDAccountId == fixtureAccountId);

                    if (fixedAssetEquip != null)
                    {
                        DB.Update<FixedAssetsAccount>()
                          .Set(p => p.ManageStatus, ManageState.Scrap)
                          .Where(p => p.Id == fixedAssetEquip.FixedAssetsAccountId).Execute();
                    }
                }
            }

        }

        /// <summary>
        /// 处置单审批通过后更新备件的相关信息
        /// </summary>
        /// <param name="assetDisposalList">处置单集合</param>
        public virtual void UpdateAssetDisposalSpareParts(EntityList<AssetDisposal> assetDisposalList)
        {
            var ctl = RT.Service.Resolve<SparePartController>();

            foreach (var assetDisposal in assetDisposalList)
            {
                var disposalSparePartGroupList = assetDisposal.AssetDisposalSparePartList.GroupBy(p => p.WarehouseId);

                foreach (var disposalSparePartGroup in disposalSparePartGroupList)
                {
                    var sparePartStore = new SparePartStore();
                    sparePartStore.StoreCode = ctl.GetStoreCode();
                    sparePartStore.InboundType = SparePartInboundType.Disposal;
                    sparePartStore.DisposalNo = assetDisposal.No;
                    sparePartStore.InboundStatus = InboundStatus.ToBe;
                    sparePartStore.WarehouseId = disposalSparePartGroup.Key;

                    var storeSparePartList = disposalSparePartGroup.ToList();

                    int i = 1;
                    foreach (var item in storeSparePartList)
                    {
                        var detail = new StoreDetail();
                        detail.LineNo = i.ToString();
                        detail.SparePartId = item.SparePartId;
                        detail.IsOldPart = true;
                        detail.QualityStatus = (QualityStatus)item.QualityStatus;
                        detail.UnitPrice = 0;
                        detail.BatchNumber = item.LotNo;
                        detail.Sn = item.Sn;
                        detail.InboundStatus = InboundStatus.ToBe;
                        detail.Number = item.Qty;
                        sparePartStore.StoreDetailList.Add(detail);
                        ++i;
                    }

                    RF.Save(sparePartStore);
                }
            }
        }

        /// <summary>
        /// 撤回资产处置单
        /// </summary>
        /// <param name="selectedIds">处置单Id集合</param>
        public virtual void CancelAssetDisposals(List<double> selectedIds)
        {
            var assetDisposals = GetAssetDisposalListByIds(selectedIds);
            if (assetDisposals.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能操作".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                List<double> assetDisposalsIds = new List<double>();
                assetDisposals.ForEach(p =>
                {
                    p.ApprovalStatus = ApprovalStatus.Draft;
                    assetDisposalsIds.Add(p.Id);
                });
                RF.Save(assetDisposals);
                var nowDate = RF.Find<AssetDisposal>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(assetDisposalsIds, typeof(AssetDisposal).FullName, ApprovalResult.Retract, nowDate, "");
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核资产处置单
        /// </summary>
        /// <param name="selectedIds">处置单Id集合</param>
        /// <param name="value">审核结果</param>
        /// <param name="remark">审核意见</param>
        public virtual void ApprovalAssetDisposals(List<double> selectedIds, ApprovalResult value, string remark)
        {
            if (!selectedIds.Any())
            {
                throw new ValidationException("传参异常".L10N());
            }

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                ApprovalAssetDisposalsInner(selectedIds, value, remark);
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核资产处置单
        /// </summary>
        /// <param name="selectedIds">处置单Id集合</param>
        /// <param name="value">审核结果</param>
        /// <param name="remark">审核意见</param>
        /// <param name="assetDisposalList">数据组</param>
        public virtual void ApprovalAssetDisposalsInner(List<double> selectedIds, ApprovalResult value, string remark, EntityList<AssetDisposal> assetDisposalList = null)
        {
            if (assetDisposalList == null)
            {
                assetDisposalList = GetAssetDisposalListByIds(selectedIds);
                if (!assetDisposalList.Any())
                {
                    throw new ValidationException("审核数据异常".L10N());
                }
            }
            //验证只有待审核的数据才能审核
            if (assetDisposalList.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            }

            var status = value == ApprovalResult.Pass ? ApprovalStatus.Audited : ApprovalStatus.Reject;
            var nowDate = RF.Find<AssetDisposal>().GetDbTime();
            var ids = new List<double>();
            assetDisposalList.ForEach(item =>
            {
                item.ApprovalStatus = status;
                ids.Add(item.Id);
            });

            //保存成功之后添加审核记录
            RF.Save(assetDisposalList);
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(ids, typeof(AssetDisposal).FullName, value, nowDate, remark);

            //执行处置单审批后的逻辑
            if (status == ApprovalStatus.Audited)
            {
                var disposalEquipList = assetDisposalList.Where(p => p.AssetObject == Enums.AssetObject.Equipment).ToList();
                var disposalFixtureList = assetDisposalList.Where(p => p.AssetObject == Enums.AssetObject.Fixture).ToList();

                if (disposalEquipList.Any())
                {
                    //更新设备台账的相关信息
                    UpdateAssetDisposalEquipments(disposalEquipList);
                }

                if (disposalFixtureList.Any())
                {
                    //更新工治具的相关信息
                    UpdateAssetDisposalFixtures(disposalFixtureList);
                }

                //更新备件的相关信息
                UpdateAssetDisposalSpareParts(assetDisposalList);
            }
        }

        /// <summary>
        /// 获取打印模板
        /// </summary>
        /// <param name="entityType">打印条码类型</param>
        /// <param name="info">分页信息</param>
        /// <param name="keyword">关键词</param>
        /// <returns>打印模板列表</returns>
        public virtual EntityList<PrintTemplate> GetPrintTemplatesByType(string entityType, PagingInfo info = null, string keyword = "")
        {
            var query = Query<PrintTemplate>().Where(p => p.EntityType.Contains(entityType) && p.State == State.Enable);
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.FileName.Contains(keyword));
            }
            return query.ToList(info);
        }

        #region 导入保存备件回收相关方法


        /// <summary>
        /// 导入保存备件回收
        /// </summary>
        /// <param name="data">导入数据</param>
        /// <returns>导入返回信息</returns>
        public virtual List<ImportMessageResult> ImportOnSave(IList<RowData> data)
        {
            if (data == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            //调用帮助类
            ImportAssetDisposalSparePartHandle handle = new ImportAssetDisposalSparePartHandle();
            List<ImportMessageResult> ImpMesResultList = handle.ImportEquipmentCard(data);
            //返回数据集
            return ImpMesResultList;
        }
        #endregion;
    }
}

