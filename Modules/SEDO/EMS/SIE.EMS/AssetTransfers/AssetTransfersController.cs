using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Common.Controller;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipmentCards;
using SIE.Equipments.WorkFlows;
using SIE.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.EMS.AssetTransfers
{
    /// <summary>
    /// 资产调拨控制器
    /// </summary>
    public class AssetTransfersController : DomainController
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<AssetTransfer> Fetch(AssetTransferCriteria criteria)
        {
            var query = Query<AssetTransfer>();
            if (!criteria.TransferNo.IsNullOrEmpty())
            {
                query.Where(p => p.TransferNo.Contains(criteria.TransferNo));
            }

            if (criteria.ApprovalStatus.HasValue)
            {
                query.Where(p => p.ApprovalStatus == criteria.ApprovalStatus.Value);
            }

            if (criteria.QureyFactoryId.HasValue)
            {
                query.Where(p => p.SourceFactoryId == criteria.QureyFactoryId.Value || p.TargetFactoryId == criteria.QureyFactoryId.Value);
            }
            if (criteria.ManageDeptId.HasValue)
            {
                query.Where(p => p.ManageDeptId == criteria.ManageDeptId.Value);
            }
            if (criteria.TargetManageDeptId.HasValue)
            {
                query.Where(p => p.TargetManageDeptId == criteria.TargetManageDeptId.Value);
            }
            if (criteria.TransferType.HasValue)
            {
                query.Where(p => p.TransferType == criteria.TransferType.Value);
            }
            if (criteria.ApplicantId.HasValue)
            {
                query.Where(p => p.ApplicantId == criteria.ApplicantId.Value);
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="selectedId"></param>
        public virtual void Send(double selectedId)
        {
            var entity = RF.GetById<AssetTransfer>(selectedId);
            if (entity == null)
            {
                throw new ValidationException("获取提交的数据异常!".L10N());
            }
            if (entity.TransferStatus != TransferStatus.NotShipped)
            {
                throw new ValidationException("发货失败，提交数据不是待发货状态!".L10N());
            }
            if (entity.ApprovalStatus != ApprovalStatus.Audited)
            {
                throw new ValidationException("发货失败，提交数据不是已审核状态!".L10N());
            }
            entity.TransferStatus = TransferStatus.Pending;
            RF.Save(entity);
        }

        /// <summary>
        /// 接收
        /// </summary>
        /// <param name="selectedIds"></param>
        public virtual void Received(List<double> selectedIds)
        {
            EntityList<AssetTransfer> datas = CheckReceivedDatas(selectedIds);
            var assetTransferDetailList = GetAssetTransferDetailsByTransferIds(datas.Select(m => m.Id).ToList());
            if (!assetTransferDetailList.Any())
            {
                throw new ValidationException("至少存在一条明细数据!".L10N());
            }
            var modelEquipAccountsIds = assetTransferDetailList.Select(m => m.EquipAccountId).ToList();
            if (!modelEquipAccountsIds.Any())
            {
                throw new ValidationException("设备清单至少存在一条数据!".L10N());
            }
            //所有待提交的设备台账
            var equipAccounts = RT.Service.Resolve<EquipAccountController>().GetEquipAccountsByIds(modelEquipAccountsIds);
            //所有设备台账对应的立卡
            var equipmentCards = RT.Service.Resolve<EquipmentCardController>().GetEquipmentCardByCode(equipAccounts.Select(m => m.Code).ToList());
            var equipAccountResumeList = new EntityList<EquipAccountResume>();
            datas.ForEach(item =>
            {
                item.TransferStatus = TransferStatus.Received;
                var itemDetailList = assetTransferDetailList.Where(m => m.AssetTransferId == item.Id);
                foreach (var detail in itemDetailList)
                {
                    var account = equipAccounts.FirstOrDefault(m => m.Id == detail.EquipAccountId);
                    if (account != null)
                    {
                        SetEquipAccount(item, account, detail);
                        //创建设备履历
                        CreateEquipAccountResume(item, equipAccountResumeList, account);
                        //更新设备立卡数据
                        var card = equipmentCards.FirstOrDefault(m => m.Code == account.Code);
                        if (card != null)
                        {
                            SetEquipCard(item, card, detail);
                        }

                    }
                }
                item.TransferStatus = TransferStatus.Received;
            });
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(datas);
                if (equipAccounts.Any())
                {
                    RF.Save(equipAccounts);
                }
                if (equipmentCards.Any())
                {
                    RF.Save(equipmentCards);
                }
                if (equipAccountResumeList.Any())
                {
                    RF.Save(equipAccountResumeList);
                }
                trans.Complete();
            }

        }
        /// <summary>
        /// 创建设备履历
        /// </summary>
        /// <param name="item"></param>
        /// <param name="equipAccountResumeList"></param>
        /// <param name="account"></param>
        private void CreateEquipAccountResume(AssetTransfer item, EntityList<EquipAccountResume> equipAccountResumeList, EquipAccount account)
        {
            //产生设备履历
            var equipAccountResume = new EquipAccountResume()
            {
                EquipAccountId = account.Id,
                No = item.TransferNo,
                ResumeType = ResumeType.AssetTranstfers,
                Changed = "",
                State = account.State,
            };
            equipAccountResumeList.Add(equipAccountResume);
        }

        /// <summary>
        /// 检查接收的数据
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        private EntityList<AssetTransfer> CheckReceivedDatas(List<double> selectedIds)
        {
            var datas = GetListAssetTransferByIds(selectedIds);
            if (!datas.Any())
            {
                throw new ValidationException("获取提交的数据异常!".L10N());
            }
            if (datas.Any(m => m.TransferStatus != TransferStatus.Pending))
            {
                throw new ValidationException("接收失败，存在至少一条数据不是待收货状态!".L10N());
            }
            if (datas.Any(m => m.ApprovalStatus != ApprovalStatus.Audited))
            {
                throw new ValidationException("接收失败，存在至少一条数据不是已审核状态!".L10N());
            }
            return datas;
        }

        /// <summary>
        /// 设置设备台账的数据
        /// </summary>
        /// <param name="item"></param>
        /// <param name="account"></param>
        /// <param name="detail"></param>

        private static void SetEquipAccount(AssetTransfer item, EquipAccount account, AssetTransferDetail detail)
        {

            account.ResPersonId = detail.ResponsibleId;
            account.WorkShopId = detail.WorkshopId;
            account.ResourceId = detail.ResourceId;

            account.FactoryId = item.TargetFactoryId;
            account.ManageDepartmentId = item.TargetManageDeptId;
            account.UseDepartmentId = item.TargetUseDepartId;
            account.InstallationLocation = detail.Location;
            account.WarehouseId = detail.WarehouseId;
            account.StorageLocationId = detail.StorageLocationId;
            account.AdministratorId = detail.KeeperId;
        }

        /// <summary>
        /// 设置设备立卡数据
        /// </summary>
        /// <param name="item"></param>
        /// <param name="card"></param>
        /// <param name="detail"></param>

        private static void SetEquipCard(AssetTransfer item, EquipmentCard card, AssetTransferDetail detail)
        {

            card.AssetUserId = detail.ResponsibleId;
            card.WorkShopId = detail.WorkshopId;
            card.ResourceId = detail.ResourceId;

            card.FactoryId = item.TargetFactoryId;
            card.ManagementId = item.TargetManageDeptId;
            card.UseDepartmentId = item.TargetUseDepartId;
            card.InstallationLocation = detail.Location;
            card.WarehouseId = detail.WarehouseId;
            card.StorageLocationId = detail.StorageLocationId;
            card.AdministratorId = detail.KeeperId;
        }

        /// <summary>
        /// 根据ID获取设备台账附加信息
        /// </summary>
        /// <param name="equipAccountId"></param>
        /// <returns></returns>

        public virtual EquipAccountExtInfo GetEquipAccountInfoById(double equipAccountId)
        {
            var res = new EquipAccountExtInfo();
            var account = RT.Service.Resolve<EquipAccountController>().GetEquipAccountById(equipAccountId);
            if (account != null)
            {
                res.OriginalResponsibleId = account.ResPersonId;
                res.OriginalResponsibleId_Display = account.ResPersonId.HasValue ? account.ResPerson.Name : "";
                res.OriginalWorkshopId = account.WorkShopId;
                res.OriginalWorkshopId_Display = account.WorkShopId.HasValue ? account.WorkShopCode : "";

                res.OriginalResourceId = account.ResourceId;
                res.OriginalResourceId_Display = account.ResourceId.HasValue ? account.ResourceName : "";
                res.OriginalLocation = account.InstallationLocation;
                res.OriginalWarehouseId = account.WarehouseId;
                res.OriginalWarehouseId_Display = account.WarehouseId.HasValue ? account.Warehouse.Name : "";
                res.OrinialStorageLocationId = account.StorageLocationId;
                res.OrinialStorageLocationId_Display = account.StorageLocationId.HasValue ? account.StorageLocation.Name : "";
                res.OrignalKeeperId = account.AdministratorId;
                res.OrignalKeeperId_Display = account.Administrator != null ? account.Administrator.Name : "";
            }

            return res;
        }

        /// <summary>
        /// 获取符合条件的设备列表
        /// </summary>
        /// <param name="assetTransferDetail"></param>
        /// <param name="page"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountSelect> GetEquipAccounts(AssetTransferDetail assetTransferDetail, PagingInfo page, string code)
        {
            //选择工厂、管理部门、使用部门、是否固定资产符合主表的原工厂、原管理部门、原使用部门、固定资产的设备（修改主表的这4个字段时，清空设备清单）
            //。过滤管理状态为【报废、待验收、已处置】的设备

            var qurey = Query<EquipAccountSelect>().Where(m =>
                     m.FactoryId == assetTransferDetail.SourceFactoryId
                    && m.ManageDepartmentId == assetTransferDetail.ManageDeptId
                     && m.UseDepartmentId == assetTransferDetail.UseDeptId
                     && m.UseState != Core.Enums.AccountUseState.DisposedOf
                     && m.UseState != Core.Enums.AccountUseState.Scrap
                     && m.UseState != Core.Enums.AccountUseState.ToAccepted
              );
            qurey.WhereIf(assetTransferDetail.IsAsset, p => (p.IssAsset == true || p.FixedAssetsAccountId != null));

            var list = qurey.WhereIf(!code.IsNullOrEmpty(), p => p.Code.Contains(code)||p.Name.Contains(code)).ToList(page, new EagerLoadOptions().LoadWithViewProperty());

            return list;
        }

        /// <summary>
        /// 保存资产调拨
        /// </summary>
        /// <param name="model"></param>
        public virtual void SaveAssetTransfer(AssetTransfer model)
        {
            //设备清单的设备获取关联的设备立卡数据，如能获取到且设备立卡的【修改标识】为【是】则报错：设备XXX（如有多个一次性显示多个）处于立卡修改中，不允许调拨
            var modelEquipAccountsIds = model.AssetTransferDetailList.Select(m => m.EquipAccountId).ToList();
            if (!modelEquipAccountsIds.Any())
            {
                throw new ValidationException("设备清单至少一条数据!".L10N());
            }
            var equips = RT.Service.Resolve<EquipAccountController>().GetEquipAccountsByIds(modelEquipAccountsIds);
            if (!equips.Any())
            {
                throw new ValidationException("获取设备数据异常!".L10N());
            }
            var equipmentCards = RT.Service.Resolve<EquipmentCardController>().GetEquipmentCardByCode(equips.Select(m => m.Code).ToList());
            var eidtingCards = equipmentCards.Where(m => m.IsChange).ToList();
            if (eidtingCards.Any())
            {
                StringBuilder message = new StringBuilder("设备");
                eidtingCards.ForEach(item =>
                {
                    message.AppendFormat("{0},", item.Code);
                });
                throw new ValidationException(message.ToString().Trim(',') + "处于立卡修改中，不允许调拨".L10N());
            }
            var equipIds = equips.Select(m => m.Id).ToList();
            if (ExsitedTransfers(equipIds, model))
            {
                throw new ValidationException("设备存在未完结的资产调拨数据，不允许调拨!".L10N());
            }
            model.AssetTransferDetailList.ForEach(item =>
            {
                var res1 = item.OriginalResourceId == item.ResourceId && item.OriginalResponsibleId == item.ResponsibleId;
                var res2 = item.OriginalWarehouseId == item.WarehouseId && item.OriginalWorkshopId == item.WorkshopId;
                var res3 = item.OrignalKeeperId == item.KeeperId && item.OrinialStorageLocationId == item.StorageLocationId;
                if (res1 && res2 && res3)
                {
                    throw new ValidationException("设备{0}调拨前后字段值不能一致!".L10nFormat(item.EquipAccount.Code));
                }
            });
            if (model.PersistenceStatus == PersistenceStatus.New)
            {
                model.ApplicantId = RT.IdentityId;
                model.ApprovalStatus = ApprovalStatus.Draft;
            }
            if (model.TransferType == TransferType.AcrossFactory && model.SourceFactoryId == model.TargetFactoryId)
            {
                throw new ValidationException("跨工厂调拨，原工厂和目标工厂不能相同!".L10N());
            }

            RF.Save(model);
        }

        /// <summary>
        /// 是否存在待发货的数据
        /// </summary>
        /// <param name="equipIds"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool ExsitedTransfers(IList<double> equipIds, AssetTransfer model)
        {
            if (model == null)
            {
                return false;
            }
            var results = Query<AssetTransfer>().Join<AssetTransferDetail>((x, y) => x.Id == y.AssetTransferId && equipIds.Contains(y.EquipAccountId)
            && x.TransferStatus != TransferStatus.Received).ToList();
            if (results.Any())
            {
                if (model.PersistenceStatus == PersistenceStatus.Modified)//修改时排除自身
                {
                    return results.Any(m => m.Id != model.Id);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 撤回调拨
        /// </summary>
        /// <param name="selectedIds"></param>
        public virtual void CancelAssetTransfers(List<double> selectedIds)
        {
            var transfers = GetListAssetTransferByIds(selectedIds);
            if (transfers.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能操作".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                List<double> transfersIds = new List<double>();
                transfers.ForEach(p =>
                {
                    p.ApprovalStatus = ApprovalStatus.Draft;
                    transfersIds.Add(p.Id);
                });
                RF.Save(transfers);
                var now = RF.Find<AssetTransfer>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(transfersIds, typeof(AssetTransfer).FullName, ApprovalResult.Retract, now, "");
                trans.Complete();
            }
        }

        /// <summary>
        /// 提交资产调拨
        /// </summary>
        /// <param name="selectedIds"></param>
        public virtual void Sumbit(List<double> selectedIds)
        {
            var config = RT.Service.Resolve<ApprovalController>().GetApprovalConfigValue(typeof(AssetTransfer));
            var assetTransfers = GetListAssetTransferByIds(selectedIds);
            if (assetTransfers.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有状态为【待提交】、【驳回】的数据才能提交".L10N());
            }
            var recordIds = new List<double>();
            foreach (var item in assetTransfers)
            {
                item.ApprovalStatus = ApprovalStatus.PendingReview;
                recordIds.Add(item.Id);
            }
            var now = RF.Find<AssetTransfer>().GetDbTime();
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(assetTransfers);
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(recordIds, typeof(AssetTransfer).FullName, ApprovalResult.Submit, now, "");
                if (!config.EnableAudit)
                {
                    //【是否启用审批】为否时,提交的同时进行审批
                    ApprovalInner(selectedIds, ApprovalResult.Pass, "通过".L10N(), assetTransfers);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核资产调拨
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="value"></param>
        /// <param name="remark"></param>
        public virtual void Approval(List<double> selectedIds, ApprovalResult value, string remark)
        {
            if (!selectedIds.Any())
            {
                throw new ValidationException("传参异常".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                ApprovalInner(selectedIds, value, remark);
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核资产调拨
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="value"></param>
        /// <param name="remark"></param>
        /// <param name="assetTransferList"></param>
        public virtual void ApprovalInner(List<double> selectedIds, ApprovalResult value, string remark, EntityList<AssetTransfer> assetTransferList = null)
        {
            if (assetTransferList == null)
            {
                assetTransferList = GetListAssetTransferByIds(selectedIds);
                if (!assetTransferList.Any())
                {
                    throw new ValidationException("审核数据异常".L10N());
                }
            }

            //验证只有执行中的数据才能审核
            if (assetTransferList.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            }

            var status = value == ApprovalResult.Pass ? ApprovalStatus.Audited : ApprovalStatus.Reject;
            var now = RF.Find<AssetTransfer>().GetDbTime();
            var ids = new List<double>();
            assetTransferList.ForEach(item =>
            {
                item.ApprovalStatus = status;
                ids.Add(item.Id);
            });

            //保存成功之后添加审核记录
            RF.Save(assetTransferList);
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(ids, typeof(AssetTransfer).FullName, value, now, remark);
        }

        /// <summary>
        /// 获取调拨集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public virtual EntityList<AssetTransfer> GetListAssetTransferByIds(IList<double> idList)
        {
            return idList.SplitContains((ids) =>
            {
                return Query<AssetTransfer>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

        }

        /// <summary>
        /// 根据调拨Ids获取明细集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public virtual EntityList<AssetTransferDetail> GetAssetTransferDetailsByTransferIds(IList<double> idList)
        {
            return idList.SplitContains((ids) =>
            {
                return Query<AssetTransferDetail>().Where(p => ids.Contains(p.AssetTransferId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

        }

        /// <summary>
        /// 获取新的资产调拨
        /// </summary>
        /// <returns></returns>
        public virtual AssetTransfer GetNewAssetTransfer()
        {
            var entity = new AssetTransfer();
            entity.TransferNo = RT.Service.Resolve<CommonController>().GetNo<AssetTransfer>("资产调拨");
            entity.TransferStatus = TransferStatus.NotShipped;
            entity.ApprovalStatus = ApprovalStatus.Draft;
            entity.ApplyDate = DateTime.Now;
            entity.ApplicantId = RT.IdentityId;
            entity.ApplicantName = RF.GetById<Employee>(RT.IdentityId).Name;
            return entity;
        }
    }
}
