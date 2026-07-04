using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Common.Controller;
using SIE.EMS.SpareParts;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipmentCards;
using SIE.Equipments.WorkFlows;
using SIE.Fixtures.Fixtures.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.EMS.FixedAssets.Accounts
{
    /// <summary>
    /// 固定资产台账控制器
    /// </summary>
    public partial class FixedAssetsAccountController : DomainController
    {
        /// <summary>
        /// 保存固定资产台账
        /// </summary>
        /// <param name="assetAccount">固定资产台账</param>
        public virtual void SaveFixedAssetsAccount(FixedAssetsAccount assetAccount)
        {
            StringBuilder sb = new StringBuilder();

            //设备
            if (assetAccount.AssetsType == AssetsType.Equipment)
            {
                var equipIds = assetAccount.DeviceBillList.Select(p => p.EquipAccountId);
                var assetEquipList = Query<FixedAssetDeviceBill>().Where(p => p.FixedAssetsAccountId != assetAccount.Id && equipIds.Contains(p.EquipAccountId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(FixedAssetDeviceBill.FixedAssetsAccountProperty));

                //验证台账是否已关联固定资产
                if (assetEquipList.Any())
                {
                    foreach (var assetEquip in assetEquipList)
                    {
                        sb.AppendLine("设备台账【{0}】已关联固定资产【{1}】!".L10nFormat(assetEquip.Code, assetEquip.FixedAssetsAccount.Code));
                    }
                    throw new ValidationException(sb.ToString());
                }
                //校验主记录
                if (!assetAccount.DeviceBillList.Any(p => p.IsMajor))
                {
                    if (assetAccount.DeviceBillList.Count == 1)
                    {
                        assetAccount.DeviceBillList.First().IsMajor = true;
                    }
                    else
                    {
                        throw new ValidationException("一个设备类型的固定资产必须有且只有一条主设备记录".L10N());
                    }
                }
                //置空备件和工治具列表
                assetAccount.FixedAssetSparePartList.Clear();
                assetAccount.FixedAssetFixtureBillList.Clear();
                assetAccount.FixedAssetSparePartList.DeletedList.Clear();
                assetAccount.FixedAssetFixtureBillList.DeletedList.Clear();

            }
            //备件
            if (assetAccount.AssetsType == AssetsType.SpareParts)
            {
                var detailIds = assetAccount.FixedAssetSparePartList.Select(p => p.StoreSummaryDetailId);
                var assetSpartList = Query<FixedAssetSparePart>().Where(p => p.FixedAssetsAccountId != assetAccount.Id && detailIds.Contains(p.StoreSummaryDetailId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(FixedAssetDeviceBill.FixedAssetsAccountProperty));
                //验证备件是否已关联固定资产
                if (assetSpartList.Any())
                {
                    foreach (var assetSpart in assetSpartList)
                    {
                        sb.AppendLine("备件【{0}】已关联固定资产【{1}】!".L10nFormat(assetSpart.OrderNumberCode, assetSpart.FixedAssetsAccount.Code));
                    }
                    throw new ValidationException(sb.ToString());
                }
                //校验主记录
                if (!assetAccount.FixedAssetSparePartList.Any(p => p.IsMajor))
                {
                    if (assetAccount.FixedAssetSparePartList.Count == 1)
                    {
                        assetAccount.FixedAssetSparePartList.First().IsMajor = true;
                    }
                    else
                    {
                        throw new ValidationException("一个备件类型的固定资产必须有且只有一条主备件记录".L10N());
                    }
                }
                //置空设备和工治具列表
                assetAccount.DeviceBillList.Clear();
                assetAccount.FixedAssetFixtureBillList.Clear();
                assetAccount.DeviceBillList.DeletedList.Clear();
                assetAccount.FixedAssetFixtureBillList.DeletedList.Clear();

            }
            //工治具
            if (assetAccount.AssetsType == AssetsType.ToolsFixtures)
            {
                var fixtureIds = assetAccount.FixedAssetFixtureBillList.Select(p => p.FixtureIDAccountId);
                var assetFixtureList = Query<FixedAssetFixtureBill>().Where(p => p.FixedAssetsAccountId != assetAccount.Id && fixtureIds.Contains(p.FixtureIDAccountId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(FixedAssetDeviceBill.FixedAssetsAccountProperty));
                //验证工治具是否已关联固定资产
                if (assetFixtureList.Any())
                {
                    foreach (var assetFixture in assetFixtureList)
                    {
                        sb.AppendLine("工治具【{0}】已关联固定资产【{1}】!".L10nFormat(assetFixture.Code, assetFixture.FixedAssetsAccount.Code));
                    }
                    throw new ValidationException(sb.ToString());
                }
                //校验主记录
                if (!assetAccount.FixedAssetFixtureBillList.Any(p => p.IsMajor))
                {
                    if (assetAccount.FixedAssetFixtureBillList.Count == 1)
                    {
                        assetAccount.FixedAssetFixtureBillList.First().IsMajor = true;
                    }
                    else
                    {
                        throw new ValidationException("一个工治具类型的固定资产必须有且只有一条主工治具记录".L10N());
                    }
                }
                //置空设备和备件列表
                assetAccount.DeviceBillList.Clear();
                assetAccount.FixedAssetSparePartList.Clear();
                assetAccount.DeviceBillList.DeletedList.Clear();
                assetAccount.FixedAssetSparePartList.DeletedList.Clear();

            }

            RF.Save(assetAccount);
        }

        /// <summary>
        /// 审核固定资产台账
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <param name="remark">审核意见</param>
        /// <param name="isPass">是否通过</param>
        public virtual void Approvel(double id, string remark, bool isPass)
        {
            using (var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                ApprovelInner(id, remark, isPass);
                tran.Complete();
            }
        }

        /// <summary>
        /// 审核固定资产台账
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <param name="remark">审核意见</param>
        /// <param name="isPass">是否通过</param>
        ///  <param name="fixedAssetsAccount">固定资产台账</param>
        public virtual void ApprovelInner(double id, string remark, bool isPass, FixedAssetsAccount fixedAssetsAccount = null)
        {
            if (fixedAssetsAccount == null)
            {
                fixedAssetsAccount = Query<FixedAssetsAccount>().Where(m => m.Id == id).FirstOrDefault();
                if (fixedAssetsAccount == null)
                {
                    throw new ValidationException("审核数据异常".L10N());
                }
            }

            if (fixedAssetsAccount == null)
            {
                throw new ValidationException("固定资产台账数据无法找到!".L10N());
            }

            if (fixedAssetsAccount.ReviewStatus != ApprovalStatus.PendingReview && fixedAssetsAccount.ReviewStatus != ApprovalStatus.UnderReview)
            {
                throw new ValidationException("审核失败,固定资产台账当前状态不是待审核!".L10N());
            }

            fixedAssetsAccount.ReviewStatus = isPass ? ApprovalStatus.Audited : ApprovalStatus.Reject;

            var now = RF.Find<FixedAssetsAccount>().GetDbTime();

            var equipAccountIds = new List<double>();
            IList<string> equipAccountCodes = new List<string>();
            var detailIds = fixedAssetsAccount.FixedAssetSparePartList.Select(p => p.StoreSummaryDetailId).ToList();
            var fixtureIds = fixedAssetsAccount.FixedAssetFixtureBillList.Select(p => p.FixtureIDAccountId).ToList();

            //审核通过且有选择设备清单
            if (isPass && fixedAssetsAccount.DeviceBillList.Any())
            {
                equipAccountIds = fixedAssetsAccount.DeviceBillList.Select(m => m.EquipAccountId).ToList();

                var exp = equipAccountIds.CreateContainsExpression<EquipAccount>("x", EquipAccount.IdProperty.Name);
                if (exp != null)
                {
                    equipAccountCodes = Query<EquipAccount>()
                        .Select(x => x.Code)
                        .Where(exp)
                        .ToList<string>();
                }
            }

            //审核通过时候更新设备台账和设备立卡的资产编码 
            if (isPass && fixedAssetsAccount.AssetsType == AssetsType.Equipment)
            {
                equipAccountIds.SplitDataExecute(tempIds =>
                {
                    DB.Update<EquipAccount>()
                        .Set(p => p.FixedAssetsAccountId, fixedAssetsAccount.Id)
                        .Set(p => p.ResPersonId, fixedAssetsAccount.AssetOwnerId)
                        .Where(m => tempIds.Contains(m.Id))
                        .Execute();
                });

                equipAccountCodes.SplitDataExecute(tempCodes =>
                {
                    DB.Update<EquipmentCard>()
                        .Set(p => p.FixedAssetsAccountId, fixedAssetsAccount.Id)
                        .Set(p => p.AssetUserId, fixedAssetsAccount.AssetOwnerId)
                        .Where(m => tempCodes.Contains(m.Code))
                        .Execute();
                });
            }

            //审核通过时候更新备件序列号明细的资产编码 
            if (isPass && fixedAssetsAccount.AssetsType == AssetsType.SpareParts)
            {
                detailIds.SplitDataExecute(tempIds =>
                {
                    DB.Update<StoreSummaryDetail>()
                        .Set(p => p.FixedAssetsAccountId, fixedAssetsAccount.Id)
                        .Where(m => tempIds.Contains(m.Id))
                        .Execute();
                });
            }

            //审核通过时候更新工治具ID台账的资产编码 
            if (isPass && fixedAssetsAccount.AssetsType == AssetsType.ToolsFixtures)
            {
                fixtureIds.SplitDataExecute(tempIds =>
                {
                    DB.Update<FixtureIDAccount>()
                    .Set(p => p.FixedAssetsAccountId, fixedAssetsAccount.Id)
                    .Where(m => tempIds.Contains(m.Id))
                    .Execute();
                });
            }

            RF.Save(fixedAssetsAccount);
            ApprovalResult approvalResult = isPass ? ApprovalResult.Pass : ApprovalResult.Reject;
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(new List<double> { fixedAssetsAccount.Id }, typeof(FixedAssetsAccount).FullName, approvalResult, now, remark);

        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="id"></param>
        public virtual bool Submit(double id)
        {
            var config = RT.Service.Resolve<EmsApprovalController>().GetApprovalConfigValue(typeof(FixedAssetsAccount));
            var fixedAssetsAccount = Query<FixedAssetsAccount>().Where(m => m.Id == id).FirstOrDefault();
            if (fixedAssetsAccount == null)
            {
                throw new ValidationException("固定资产台账数据无法找到!".L10N());
            }

            if (fixedAssetsAccount.ReviewStatus != ApprovalStatus.Draft
                && fixedAssetsAccount.ReviewStatus != ApprovalStatus.Reject)
            {
                throw new ValidationException("提交失败,固定资产台账当前状态不是待提交或驳回状态!".L10N());
            }

            fixedAssetsAccount.ReviewStatus = ApprovalStatus.PendingReview;

            var now = RF.Find<FixedAssetsAccount>().GetDbTime();

            using (var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(fixedAssetsAccount);

                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(new List<double> { fixedAssetsAccount.Id }, typeof(FixedAssetsAccount).FullName, ApprovalResult.Submit, now, string.Empty);

                //是否启用审批为false时提交后自动审批
                if (!config.EnableAudit)
                {
                    ApprovelInner(id, "通过".L10N(), true, fixedAssetsAccount);
                }
                tran.Complete();
            }
            return true;
        }

        /// <summary>
        /// 撤回
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Withdraw(double id)
        {
            var fixedAssetsAccount = Query<FixedAssetsAccount>().Where(m => m.Id == id).FirstOrDefault();

            if (fixedAssetsAccount == null)
            {
                throw new ValidationException("固定资产台账数据无法找到!".L10N());
            }

            if (fixedAssetsAccount.ReviewStatus != ApprovalStatus.PendingReview)
            {
                throw new ValidationException("撤回失败,固定资产台账当前状态不是待审核!".L10N());
            }

            fixedAssetsAccount.ReviewStatus = ApprovalStatus.Draft;

            var now = RF.Find<FixedAssetsAccount>().GetDbTime();

            using (var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(fixedAssetsAccount);

                RT.Service.Resolve<WorkFlowRecordController>()
                    .CreateWorkFlowRecords(new List<double> { fixedAssetsAccount.Id },
                    typeof(FixedAssetsAccount).FullName, ApprovalResult.Retract, now, string.Empty);

                tran.Complete();
            }
            return true;
        }

        /// <summary>
        /// 获取资产履历
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortInfo"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual EntityList<FixedAssetResume> GetResumes(double id, List<OrderInfo> sortInfo, PagingInfo pagingInfo, ResumeType? state)
        {
            var query = Query<FixedAssetResume>();
            if (id > 0)
                query.Where(p => p.FixdAccountId == id);
            if (state.HasValue)
                query.Where(p => p.ResumeType == state);

            return query.OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<FixedAssetsAccount> Fetch(FixedAssetsAccountCriteria criteria)
        {
            var query = Query<FixedAssetsAccount>();
            if (!criteria.Code.IsNullOrEmpty())
            {
                query.Where(x => x.Code.Contains(criteria.Code));
            }

            if (!criteria.Name.IsNullOrEmpty())
            {
                query.Where(x => x.Name.Contains(criteria.Name));
            }

            if (!criteria.AssetsCategory.IsNullOrEmpty())
            {
                query.Where(x => x.AssetsCategory == criteria.AssetsCategory);
            }
            if (criteria.AssetsSource.HasValue)
            {
                query.Where(x => x.AssetsSource == criteria.AssetsSource.Value);
            }
            if (criteria.ManageStatus.HasValue)
            {
                query.Where(x => x.ManageStatus == criteria.ManageStatus.Value);
            }

            if (criteria.CreationDate != null)
            {
                if (criteria.CreationDate.BeginValue.HasValue)
                {
                    query.Where(x => x.CreateDate >= criteria.CreationDate.BeginValue);
                }
                if (criteria.CreationDate.EndValue.HasValue)
                {
                    query.Where(x => x.CreateDate <= criteria.CreationDate.EndValue);
                }
            }

            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取编码
        /// </summary>
        /// <returns></returns>
        public virtual string GetCode()
        {
            var config = ConfigService.GetConfig(new Config.FixedAssetAccountConfig(), typeof(FixedAssetsAccount));

            if (config == null || config.Number == null)
            {
                return "";
            }

            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.Number.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 获取设备清单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortInfo"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<FixedAssetDeviceBill> GetDeviceBills(double id, List<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            return Query<FixedAssetDeviceBill>().Where(m => m.FixedAssetsAccountId == id).OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

    }
}
