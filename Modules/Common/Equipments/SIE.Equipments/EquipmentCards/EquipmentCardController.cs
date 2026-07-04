using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.Domain;
using SIE.Common.Import;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Common.Controller;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipmentCards.ImportEquipmentCard;
using SIE.Equipments.EquipModels;
using SIE.Equipments.WorkFlows;
using SIE.EventMessages.EMS.EquipAccount;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Equipments.EquipmentCards
{
    /// <summary>
    /// 设备立卡控制器
    /// </summary>
    public class EquipmentCardController : DomainController
    {

        /// <summary>
        /// 获取实体修改日志
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityId">实体ID</param>
        /// <param name="sortInfo">排序参数</param>
        /// <param name="pagingInfo">分页参数</param>
        /// <returns></returns>
        public virtual EntityList<EntityLog> GetList(Type entityType, double entityId, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            return Query<EntityLog>()
                .Where(p => p.EntityId == entityId && p.TypeName == entityType.GetQualifiedName())
                .OrderBy(sortInfo)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据查询实体获取设备立卡集合
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<EquipmentCard> GetEquipmentCardList(EquipmentCardCriteria criteria)
        {
            var query = Query<EquipmentCard>();
            if (criteria.FactoryId.HasValue)
            {
                query.Where(p => p.FactoryId == criteria.FactoryId);
            }
            if (criteria.Code.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(criteria.Code));
            }
            if (criteria.EquipmentCardSource.HasValue)
            {
                query.Where(p => p.EquipmentCardSource == criteria.EquipmentCardSource);
            }
            if (criteria.EquipModelId.HasValue)
            {
                query.Where(p => p.EquipModelId == criteria.EquipModelId);
            }
            if (criteria.ApprovalStatus.HasValue)
            {
                query.Where(p => p.ApprovalStatus == criteria.ApprovalStatus);
            }
            if (criteria.SupplierId.HasValue)
            {
                query.Where(p => p.SupplierId == criteria.SupplierId);
            }
            if (criteria.PurchaseOrderNo.IsNotEmpty())
            {
                query.Where(p => p.PurchaseOrderNo.Contains(criteria.PurchaseOrderNo));
            }

            //创建时间
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            }

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(EquipModel.EquipTypeProperty);
            elo.LoadWithViewProperty();

            //只查询开箱验收值为【是】的数据
            query.Where(p => p.NeedAcceptance);
            query.OrderBy(criteria.OrderInfoList);
            EntityList<EquipmentCard> equipmentCards = query.ToList(criteria.PagingInfo, elo);

            var equipAccountAssetConfigValue = ConfigService.GetConfig<EquipAccountAssetConfigValue>(
                new EquipAccountAssetConfig(), typeof(EquipAccount));
            //【设备台账】的配置项【启用固定资产】为【是】时，不能编辑，关联固定资产带出:
            //  固定资产编码\资产名称\原值\净值
            if (equipAccountAssetConfigValue != null && equipAccountAssetConfigValue.Asset)
            {
                equipmentCards.ForEach(card =>
                {
                    card.AssetCode = card.FixedAssetsAccountCode;
                    card.AssetName = card.FixedAssetsAccountName;
                    card.OriginalValue = card.OriginalAssetsValue;
                });
            }
            return equipmentCards;
        }

        /// <summary>
        /// 根绝Id获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public virtual EquipmentCard GetEquipmentCardById(double id)
        {
            var card = GetById<EquipmentCard>(id);
            var equipAccountAssetConfigValue = ConfigService.GetConfig<EquipAccountAssetConfigValue>(
               new EquipAccountAssetConfig(), typeof(EquipAccount));
            if (equipAccountAssetConfigValue != null && equipAccountAssetConfigValue.Asset)
            {
                card.AssetCode = card.FixedAssetsAccountCode;
                card.AssetName = card.FixedAssetsAccountName;
                card.OriginalValue = card.OriginalAssetsValue;
            }
            return card;
        }

        /// <summary>
        /// 根据id列表获取设备立卡
        /// </summary>
        /// <param name="cardIds">id列表</param>
        /// <returns>设备立卡列表</returns>
        public virtual EntityList<EquipmentCard> GetEquipmentCardByIds(List<double> cardIds)
        {
            return cardIds.SplitContains(ids => Query<EquipmentCard>().Where(p => ids.Contains(p.Id)).ToList());
        }

        /// <summary>
        /// 根据编码获取设备台账
        /// </summary>
        /// <param name="codeList">设备台账编码集合</param>
        /// <returns></returns>
        public virtual EntityList<EquipmentCard> GetEquipmentCardByCode(List<string> codeList)
        {
            return codeList.SplitContains(codes =>
            {
                return Query<EquipmentCard>().Where(m => codes.Contains(m.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }


        /// <summary>
        /// 检查设备台账启用固定资产配置的数据
        /// </summary>
        /// <param name="card"></param>
        public virtual void CheckAssetConfig(EquipmentCard card)
        {
            if (card == null)
            {
                return;
            }
            if (!card.UseDepartmentId.HasValue)
            {
                throw new ValidationException("使用部门不能为空".L10N());
            }
            if (!card.ManagementId.HasValue)
            {
                throw new ValidationException("管理部门不能为空".L10N());
            }
            if (!card.UseLevel.IsNotEmpty())
            {
                throw new ValidationException("ABC分类不能为空".L10N());
            }

            var assetConfig = ConfigService.GetConfig<EquipAccountAssetConfigValue>(
            new EquipAccountAssetConfig(), typeof(EquipAccount));
            if (!assetConfig.Asset)
            {
                if (card.IssAsset == true)
                {
                    if (!card.AssetCode.IsNotEmpty())
                    {
                        throw new ValidationException("【是否固定资产】为【是】时,固定资产编码必填".L10N());
                    }
                    if (!card.AssetName.IsNotEmpty())
                    {
                        throw new ValidationException("【是否固定资产】为【是】时,固定资产名称必填".L10N());
                    }
                    if (card.NetAssetValue > card.OriginalValue)
                    {
                        throw new ValidationException("【是否固定资产】为【是】时,原值需大于等于净值".L10N());
                    }
                }
                else
                {
                    if (card.AssetCode.IsNotEmpty())
                    {
                        throw new ValidationException("【是否固定资产】为【否】时,固定资产编码只能为空".L10N());
                    }
                    if (card.AssetName.IsNotEmpty())
                    {
                        throw new ValidationException("【是否固定资产】为【否】时,固定资产名称只能为空".L10N());
                    }
                    if (card.OriginalValue > 0)
                    {
                        throw new ValidationException("【是否固定资产】为【否】时,原值为0".L10N());
                    }
                    if (card.NetAssetValue > 0)
                    {
                        throw new ValidationException("【是否固定资产】为【否】时,净值为0".L10N());
                    }
                }
            }

            if ((card.WarehouseId.HasValue && !card.StorageLocationId.HasValue) || (!card.WarehouseId.HasValue && card.StorageLocationId.HasValue))
            {
                throw new ValidationException("仓库和库位只能同时为空或不为空".L10N());
            }
        }

        /// <summary>
        /// 新增保存设备立卡
        /// </summary>
        /// <param name="card">设备立卡</param>
        public virtual void SaveEquipmentCard(EquipmentCard card)
        {
            //手动创建开箱验收值为【是】的数据
            card.NeedAcceptance = true;
            CheckAssetConfig(card);
            RF.Save(card);
        }

        /// <summary>
        /// 修改页保存设备立卡
        /// </summary>
        /// <param name="card">设备立卡</param>
        public virtual void EditSaveEquipmentCard(EquipmentCard card)
        {
            CheckAssetConfig(card);
            //  所有字段都没变动时，不能要操作，关闭页面
            //立卡日期为空时，保存修改的字段
            CheckCardState(card);
            var Oldcard = GetById<EquipmentCard>(card.Id);

            using (var tran = DB.TransactionScope(EquipmentEntityDataProvider.ConnectionStringName))
            {
                if (Oldcard.CreateCardDateTime != null)
                {
                    //立卡日期不为空，且审核状态为【通过】时
                    if (Oldcard.ApprovalStatus == ApprovalStatus.Audited)
                    {
                        card.ApprovalStatus = ApprovalStatus.Draft;
                        card.IsChange = true;
                    }
                    EntityLogHelper.CreateEntityLog(typeof(EquipmentCard), card, Oldcard);
                }
                RF.Save(card);
                tran.Complete();
            }
        }

        /// <summary>
        /// 提交设备立卡
        /// </summary>
        /// <param name="cardIds"></param>
        public virtual void SubmitEquipmentCard(List<double> cardIds)
        {
            //设备立卡是否需要审核,是否开启审核流程
            var config = RT.Service.Resolve<ApprovalController>().GetApprovalConfigValue(typeof(EquipmentCard));
            var cards = GetEquipmentCardByIds(cardIds);
            if (cards.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有状态为【待提交】、【驳回】的数据才能提交".L10N());
            }
            foreach (var card in cards)
            {
                if (!card.Name.IsNotEmpty())
                {
                    throw new ValidationException("设备编码:{0}的设备名称必填".L10nFormat(card.Code));
                }
                if (!card.UseLevel.IsNotEmpty())
                {
                    throw new ValidationException("设备编码:{0}的ABC分类必填".L10nFormat(card.Code));
                }
                if (!card.ManagementId.HasValue)
                {
                    throw new ValidationException("设备编码:{0}的管理部门必填".L10nFormat(card.Code));
                }
                card.ApprovalStatus = ApprovalStatus.PendingReview;
            }

            List<double> ids = cards.Select(p => p.Id).ToList();

            var now = RF.Find<EquipmentCard>().GetDbTime();
            using (var tran = DB.TransactionScope(EquipmentEntityDataProvider.ConnectionStringName))
            {
                RF.Save(cards);
                //保存审核记录信息
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(ids, typeof(EquipmentCard).FullName, ApprovalResult.Submit, now, "");
                if (!config.EnableAudit)
                {
                    //【是否启用审批】为否时,提交的同时进行审批
                    AuditEquipCardInner(ids, ApprovalResult.Pass, "通过", cards);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 撤回设备立卡
        /// </summary>
        /// <param name="cardIds"></param>
        public virtual void CancelEquipmentCard(List<double> cardIds)
        {
            var cards = GetEquipmentCardByIds(cardIds);
            if (cards.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能撤回".L10N());
            }
            foreach (var card in cards)
            {
                card.ApprovalStatus = ApprovalStatus.Draft;
            }

            List<double> ids = cards.Select(p => p.Id).ToList();
            var now = RF.Find<EquipmentCard>().GetDbTime();
            using (var tran = DB.TransactionScope(EquipmentEntityDataProvider.ConnectionStringName))
            {
                //保存审核记录信息
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(ids, typeof(EquipmentCard).FullName, ApprovalResult.Retract, now, "");
                RF.Save(cards);
                tran.Complete();
            }
        }

        /// <summary>
        /// 删除设备立卡
        /// </summary>
        /// <param name="cardIds"></param>
        public virtual void DeleteEquipCard(List<double> cardIds)
        {
            var cards = GetEquipmentCardByIds(cardIds);

            if (cards.Any(p => p.EquipmentCardSource != EquipmentCardSource.Manual || p.CreateCardDateTime != null || (p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject)))
            {
                throw new ValidationException("仅【卡片来源】为【手动创建】且立卡日期为空且审核状态为【待提交】、【驳回】的数据才能删除".L10N());
            }

            cards.ForEach(p =>
            {
                p.PersistenceStatus = PersistenceStatus.Deleted;
            });

            RF.Save(cards);
        }

        /// <summary>
        /// 批量审批
        /// </summary>
        /// <param name="cardIds"></param>
        /// <param name="result"></param>
        /// <param name="remark"></param>
        public virtual void AuditEquipCard(List<double> cardIds, ApprovalResult result, string remark)
        {
            using (var tran = DB.TransactionScope(EquipmentEntityDataProvider.ConnectionStringName))
            {
                AuditEquipCardInner(cardIds, result, remark);
                tran.Complete();
            }
        }

        /// <summary>
        /// 批量审批
        /// </summary>
        /// <param name="cardIds"></param>
        /// <param name="result"></param>
        /// <param name="remark"></param>
        /// <param name="equipmentCardList"></param>
        public virtual void AuditEquipCardInner(List<double> cardIds, ApprovalResult result, string remark, EntityList<EquipmentCard> equipmentCardList = null)
        {
            if (equipmentCardList == null)
            {
                equipmentCardList = GetEquipmentCardByIds(cardIds);
                if (!equipmentCardList.Any())
                {
                    throw new ValidationException("审核数据异常".L10N());
                }
            }

            if (equipmentCardList.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            }

            ApprovalStatus statu = (result == ApprovalResult.Pass) ? ApprovalStatus.Audited : ApprovalStatus.Reject;

            //取数据库时间
            var now = RF.Find<EquipmentCard>().GetDbTime();

            foreach (var card in equipmentCardList)
            {
                card.ApprovalStatus = statu;
                card.IsChange = false;
                //审批改变立卡日期，第二次修改提交不变
                if (card.CreateCardDateTime == null)
                {
                    card.CreateCardDateTime = now;
                }
            }

            List<double> ids = equipmentCardList.Select(p => p.Id).ToList();
            List<string> codeList = equipmentCardList.Select(p => p.Code).Distinct().ToList();
            //根据设备立卡编码查询台账表存在的设备台账信息用于修改
            var equipAccounts = RT.Service.Resolve<EquipAccountController>().GetEquipAccountsByCode(codeList);
            //有设备台账的设备立卡数据
            Dictionary<string, EquipAccount> equipAccountDictionary = equipAccounts.ToDictionary(p => p.Code);

            //通过则创建或修改设备台账
            if (statu == ApprovalStatus.Audited)
            {
                SynchronizeEquipAccount(equipmentCardList, equipAccountDictionary);
            }

            EntityList<EquipAccount> EquipAccountList = new EntityList<EquipAccount>();
            if (equipAccountDictionary != null && equipAccountDictionary.Any())
            {
                EquipAccountList = equipAccountDictionary.Values.AsEntityList();
            }
            //根据为每一台设备创建设备立卡的设备履历
            EntityList<EquipAccountResume> resList = GenerateEquipAccountResume(EquipAccountList);

            using (var tran = DB.TransactionScope(EquipmentEntityDataProvider.ConnectionStringName))
            {
                if (statu == ApprovalStatus.Audited)
                {
                    //审批通过
                    RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(ids, typeof(EquipmentCard).FullName, ApprovalResult.Pass, now, remark);
                }
                else
                {
                    //审批驳回
                    RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(ids, typeof(EquipmentCard).FullName, ApprovalResult.Reject, now, remark);
                }
                //保存或更新设备台账
                if (EquipAccountList.Any())
                {
                    RF.Save(EquipAccountList);
                }
                RF.Save(equipmentCardList);

                //保存设备台账类型为设备立卡的检验履历
                if (resList.Any())
                {
                    RF.Save(resList);
                }

                if (EquipAccountList.Any())
                {
                    //抽取所有设备台账的id并使用接口同步
                    var accountIds = EquipAccountList.Select(p => p.Id).ToList();
                    //设备立卡提交之后再同步所有设备台账的点检，保养，润滑项目
                    RT.Service.Resolve<IEquipAccount>().SynModelData(accountIds);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 更新设备台账信息
        /// </summary>
        /// <param name="equipmentCardList">设备卡片列有</param>
        /// <param name="equipAccountDictionary">设备台账字典</param>
        public virtual void SynchronizeEquipAccount(EntityList<EquipmentCard> equipmentCardList,
            Dictionary<string, EquipAccount> equipAccountDictionary)
        {
            if (equipmentCardList == null || equipmentCardList.Count <= 0)
            {
                return;
            }

            if (equipAccountDictionary == null)
            {
                equipAccountDictionary = new Dictionary<string, EquipAccount>();
            }

            EquipAccount account;
            foreach (var card in equipmentCardList)
            {
                if (equipAccountDictionary.ContainsKey(card.Code))
                {
                    account = equipAccountDictionary[card.Code];
                }
                else
                {
                    account = new EquipAccount();
                    account.GenerateId();
                    equipAccountDictionary.Add(card.Code, account);
                }

                account.Code = card.Code;
                account.Name = card.Name;
                account.Alias = card.Alias;
                account.OriginalSerialNumber = card.OriginalSerialNumber;
                account.RFID = card.Rfid;
                account.UseLevel = card.UseLevel;

                account.EquipmentGrading = card.EquipModelGrade;
                account.PurchaseUnit = card.PurchaseUnit;
                account.Manufacturer = card.Manufacturer;
                account.PurchaseOrderNo = card.PurchaseOrderNo;
                account.EnterDate = card.EnterDate;
                account.IsCustomsSupervision = card.IsCustomsSupervision;
                account.UsefulLife = card.UsefulLife;
                account.WarrantyPeriod = card.WarrantyPeriod;
                account.InstallationLocation = card.InstallationLocation;
                account.CardDate = card.CreateCardDateTime;
                account.IssAsset = card.IssAsset;
                account.AssetCode = card.AssetCode;
                account.AssetName = card.AssetName;
                account.OriginalValue = card.OriginalValue;

                account.StorageLocationId = card.StorageLocationId;
                account.Proprietorship = card.Proprietorship;

                //使用责任人
                account.UserId = card.UserId;
                account.State = card.AccountState;

                account.FactoryId = card.FactoryId;
                account.ManageDepartmentId = card.ManagementId;
                account.UseState = card.AccountUseState;
                account.UseDepartmentId = card.UseDepartmentId;
                account.WorkShopId = card.WorkShopId;

                account.EquipModelId = card.EquipModelId;

                account.SupplierId = card.SupplierId;
                account.WarehouseId = card.WarehouseId;
                account.ResourceId = card.ResourceId;
                account.AdministratorId = card.AdministratorId;
                account.EquipModelId = card.EquipModelId;
                account.FixedAssetsAccountId = card.FixedAssetsAccountId;
            }
        }


        /// <summary>
        /// 并行操作校验
        /// </summary>
        /// <param name="card"></param>
        public virtual void CheckCardState(EquipmentCard card)
        {
            if (card != null)
            {
                // 保存之前先确认此单据是否已经由其他人员操作保存或提交，防止并发操作时程序发生错误
                var result = RF.GetById<EquipmentCard>(card.Id);
                if (result != null && card.ApprovalStatus != result.ApprovalStatus)
                {
                    throw new ValidationException("此单据已由其他人员操作，请退出当前界面重新操作".L10N());
                }
            }
        }


        #region 生成设备台账的设备立卡类型的设备履历

        /// <summary>
        /// 生成设备台账的设备立卡类型的设备履历
        /// </summary>
        /// <param name="EquipAccountList"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountResume> GenerateEquipAccountResume(EntityList<EquipAccount> EquipAccountList)
        {
            EntityList<EquipAccountResume> resList = new EntityList<EquipAccountResume>();
            EquipAccountResume res = null;
            foreach (var item in EquipAccountList)
            {
                res = new EquipAccountResume();
                res.No = item.Code;
                res.EquipAccountId = item.Id;
                res.ResumeType = ResumeType.EquipmentCard;
                res.State = item.State;
                resList.Add(res);
            }
            return resList;
        }

        #endregion

        /// <summary>
        /// 验证是否存在
        /// </summary>
        /// <param name="Osn"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>

        public virtual bool CheckOriginalSerialNumber(double cardId, string Osn)
        {
            return Query<EquipmentCard>().Where(p => p.OriginalSerialNumber == Osn && p.Id != cardId).Count() > 0;
        }


        /// <summary>
        /// 验证是否存在
        /// </summary>
        /// <param name="rfid"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>

        public virtual bool CheckRfid(double cardId, string rfid)
        {
            return Query<EquipmentCard>().Where(p => p.Rfid == rfid && p.Id != cardId).Count() > 0;
        }



        #region 导入保存设备台账相关方法


        /// <summary>
        /// 导入保存设备台账
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
            ImportEquipmentCardHandle handle = new ImportEquipmentCardHandle();

            List<ImportMessageResult> ImpMesResultList = handle.ImportEquipmentCard(data);
            //返回数据集
            return ImpMesResultList;
        }
        #endregion;
    }
}
