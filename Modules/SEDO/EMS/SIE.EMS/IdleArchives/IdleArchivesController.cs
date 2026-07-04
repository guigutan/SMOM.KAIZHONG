using SIE.Common.Configs;
using SIE.Core.Common.Controllers;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Enums;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.IdleArchives.Configs;
using SIE.EMS.Maintains.Controller;
using SIE.EMS.Maintains.Plans;
using SIE.EMS.Maintains.Projects;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipmentCards;
using SIE.Equipments.WorkFlows;
using SIE.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.IdleArchives
{
    /// <summary>
    /// 闲置封存控制器
    /// </summary>
    public class IdleArchivesController : DomainController
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<IdleArchive> Fetch(IdleArchiveCriteria criteria)
        {
            var query = Query<IdleArchive>();
            if (!criteria.No.IsNullOrEmpty())
            {
                query.Where(p => p.No.Contains(criteria.No));
            }

            //管理部门和使用部门除符合查询外，额外限制两个部门至少有一个是用户有权限的部门的数据才能查询 Todo
            if (criteria.ApprovalStatus.HasValue)
            {
                query.Where(p => p.ApprovalStatus == criteria.ApprovalStatus.Value);
            }

            if (criteria.QureyFactoryId.HasValue)
            {
                query.Where(p => p.FactoryId == criteria.QureyFactoryId.Value);
            }
            if (criteria.ManageDeptId.HasValue)
            {
                query.Where(p => p.DepartmentId == criteria.ManageDeptId.Value);
            }

            if (criteria.UseDeptId.HasValue)
            {
                query.Where(p => p.UseDepartmentId == criteria.UseDeptId.Value);
            }

            if (criteria.IdleArchiveType.HasValue)
            {
                query.Where(p => p.IdleArchiveType == criteria.IdleArchiveType.Value);
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
        /// 获取符合条件的
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="code"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountSelect> GetEquipAccounts(IdleArchiveDetail entity, string code, PagingInfo page)
        {
            var query = Query<EquipAccountSelect>();
            query.WhereIf(entity.IdleArchiveType == IdleArchiveType.Idle, m => m.UseState == AccountUseState.Using);
            query.WhereIf(entity.IdleArchiveType == IdleArchiveType.Archive, m => (m.UseState == AccountUseState.Using || m.UseState == AccountUseState.InIdle));
            query.WhereIf(entity.IdleArchiveType == IdleArchiveType.IdleEnabled, m => m.UseState == AccountUseState.InIdle);
            query.WhereIf(entity.IdleArchiveType == IdleArchiveType.ArchiveEnabled, m => m.UseState == AccountUseState.Archive);

            query.Where(m => m.FactoryId == entity.FactoryId
                   && m.ManageDepartmentId == entity.DepartmentId
                   && m.UseDepartmentId == entity.UseDepartmentId
                   && m.EquipModel.TypeCategory == entity.TypeCategory);
            query.WhereIf(entity.IsAsset, m => m.FixedAssetsAccountId != null);
            query.WhereIf(!code.IsNullOrEmpty(), m => m.Code.Contains(code)||m.Name.Contains(code));
            return query.ToList(page, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 审核
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
                SubmitApproval(selectedIds, value, remark);
                trans.Complete();
            }
        }

        /// <summary>
        /// 提交自动审核逻辑
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="value"></param>
        /// <param name="remark"></param>
        private void SubmitApproval(List<double> selectedIds, ApprovalResult value, string remark)
        {
            var idleArchivesList = GetListIdleArchivesByIds(selectedIds);
            //验证只有执行中的数据才能审核
            if (idleArchivesList.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            }
            var status = value == ApprovalResult.Pass ? ApprovalStatus.Audited : ApprovalStatus.Reject;
            var now = RF.Find<IdleArchive>().GetDbTime();
            var ids = new List<double>();
            idleArchivesList.ForEach(item =>
            {
                item.ApprovalStatus = status;
                ids.Add(item.Id);
            });

            if (value == ApprovalResult.Pass)
            {
                SummitOrApprovalAfterEnvent(selectedIds, idleArchivesList, now);
            }

            RF.Save(idleArchivesList);
            //保存成功之后添加审核记录
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(ids, typeof(IdleArchive).FullName, value, now, remark);

        }

        /// <summary>
        /// 提交或审核之后生成相关单据
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="idleArchivesList"></param>
        /// <param name="now"></param>
        private void SummitOrApprovalAfterEnvent(List<double> selectedIds, EntityList<IdleArchive> idleArchivesList, DateTime now)
        {
            EntityList<IdleArchiveDetail> idleArchiveDetails;
            EntityList<EquipmentCard> equipCards;
            List<double> equipAccountIds;
            EntityList<EquipAccount> equipAccounts;
            GetDetailInfos(selectedIds, out idleArchiveDetails, out equipCards, out equipAccountIds, out equipAccounts);

            var equipTypeIds = new List<double>();
            var config = ConfigService.GetConfig(new MaintainedEquipmentTypeConfig(), typeof(IdleArchive));
            if (config != null && !config.EquipTypeIds.IsNullOrEmpty())
            {
                var equipTypeIdArray = config.EquipTypeIds.Split(',');
                equipTypeIdArray.ForEach(item =>
                {
                    if (!item.IsNullOrEmpty())
                    {
                        equipTypeIds.Add(double.Parse(item));
                    }
                });
            }
            var equipAccountMaintainProjects = RT.Service.Resolve<EquipController>().GetMaintainProjectsOfAccounts(equipAccountIds);
            var equipAccountResumes = new EntityList<EquipAccountResume>();
            foreach (var idleArchiveDetail in idleArchiveDetails)
            {
                //取出主表
                var idleArchive = idleArchivesList.FirstOrDefault(m => m.Id == idleArchiveDetail.IdleArchiveId);
                if (idleArchive != null)
                {
                    //取设备 
                    var equipAccount = equipAccounts.FirstOrDefault(m => m.Id == idleArchiveDetail.EquipAccountId);
                    if (equipAccount != null)
                    {  //更新设备
                        ResumeType resumeType = UpdateEquipAccountInfo(now, equipTypeIds, equipAccountMaintainProjects, idleArchiveDetail, idleArchive, equipAccount);

                        //取设备立卡
                        var equipCard = equipCards.FirstOrDefault(m => m.Code == equipAccount.Code);
                        if (equipCard != null)
                        {//更新设备立卡
                            equipCard.WorkShopId = idleArchiveDetail.WorkshopId;
                            equipCard.ResourceId = idleArchiveDetail.ResourceId;
                            equipCard.InstallationLocation = idleArchiveDetail.Location;
                            equipCard.AdministratorId = idleArchiveDetail.KeeperId;
                        }
                        //生成设备履历，类型取业务类型
                        CreateEquipAccountResume(equipAccountResumes, idleArchive, equipAccount, resumeType);
                    }
                }
            }
            RF.Save(equipCards);
            RF.Save(equipAccounts);
            RF.Save(equipAccountResumes);
        }

        /// <summary>
        /// 创建设备履历
        /// </summary>
        /// <param name="equipAccountResumes"></param>
        /// <param name="idleArchive"></param>
        /// <param name="equipAccount"></param>
        /// <param name="resumeType"></param>
        private void CreateEquipAccountResume(EntityList<EquipAccountResume> equipAccountResumes, IdleArchive idleArchive, EquipAccount equipAccount, ResumeType resumeType)
        {
            EquipAccountResume equipAccountResume = new EquipAccountResume()
            {
                Changed = "",
                EquipAccountId = equipAccount.Id,
                No = idleArchive.No,
                Remark = "",
                ResumeType = resumeType,
                State = equipAccount.State
            };
            equipAccountResumes.Add(equipAccountResume);
        }

        /// <summary>
        /// 更新设备信息
        /// </summary>
        /// <param name="now"></param>
        /// <param name="equipTypeIds"></param>
        /// <param name="equipAccountMaintainProjects"></param>
        /// <param name="idleArchiveDetail"></param>
        /// <param name="idleArchive"></param>
        /// <param name="equipAccount"></param>
        /// <returns></returns>
        private ResumeType UpdateEquipAccountInfo(DateTime now, List<double> equipTypeIds, EntityList<EquipAccountMaintainProject> equipAccountMaintainProjects,
            IdleArchiveDetail idleArchiveDetail, IdleArchive idleArchive, EquipAccount equipAccount)
        {
            equipAccount.WorkShopId = idleArchiveDetail.WorkshopId;
            equipAccount.ResourceId = idleArchiveDetail.ResourceId;
            equipAccount.InstallationLocation = idleArchiveDetail.Location;
            equipAccount.AdministratorId = idleArchiveDetail.KeeperId;
            equipAccount.UserId = idleArchiveDetail.KeeperId;
            ResumeType resumeType = ResumeType.Idle;
            if (idleArchive.IdleArchiveType == IdleArchiveType.Idle)
            {
                equipAccount.UseState = Core.Enums.AccountUseState.InIdle;
                equipAccount.State = Core.Enums.AccountState.Downtime;
            }
            if (idleArchive.IdleArchiveType == IdleArchiveType.Archive)
            {
                equipAccount.UseState = Core.Enums.AccountUseState.Archive;
                equipAccount.State = Core.Enums.AccountState.Downtime;
                resumeType = ResumeType.Archive;
            }
            if (idleArchive.IdleArchiveType == IdleArchiveType.ArchiveEnabled || idleArchive.IdleArchiveType == IdleArchiveType.IdleEnabled)
            {
                equipAccount.UseState = Core.Enums.AccountUseState.Using;
                //闲置启用和封存启用时，设备清单中设备类型在配置项【启用时需要保养的设备类型】中的设备 生成保养单据
                if (equipAccount.EquipModel.EquipTypeId != null && equipTypeIds.Contains(equipAccount.EquipModel.EquipTypeId.Value))
                {
                    CreateMaintainPlan(now, equipAccountMaintainProjects, equipAccount);
                }
                resumeType = idleArchive.IdleArchiveType == IdleArchiveType.ArchiveEnabled ? ResumeType.ArchiveEnabled : ResumeType.IdleEnabled;
            }
            return resumeType;
        }

        /// <summary>
        /// 获取闲置封存单据的相关明细数据
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="idleArchiveDetails"></param>
        /// <param name="equipCards"></param>
        /// <param name="equipAccountIds"></param>
        /// <param name="equipAccounts"></param>
        private void GetDetailInfos(List<double> selectedIds, out EntityList<IdleArchiveDetail> idleArchiveDetails, out EntityList<EquipmentCard> equipCards, out List<double> equipAccountIds, out EntityList<EquipAccount> equipAccounts)
        {
            idleArchiveDetails = GetIdleArchiveDetailsByIdleArchiveIds(selectedIds);
            if (!idleArchiveDetails.Any())
            {
                throw new ValidationException("审核的数据无设备明细，请检查".L10N());
            }
            //获取设备编码集合
            var equipCodes = idleArchiveDetails.Select(m => m.EquipAccountCode).ToList();
            //获取设备立卡集合
            equipCards = RT.Service.Resolve<EquipmentCardController>().GetEquipmentCardByCode(equipCodes);

            //获取设备ID集合
            equipAccountIds = idleArchiveDetails.Select(m => m.EquipAccountId).ToList();
            //获取设备集合
            equipAccounts = RT.Service.Resolve<Equipments.ElecEquipController>().GetEquipAccountsByIds(equipAccountIds);
        }

        /// <summary>
        /// 生成保养计划任务
        /// </summary>
        /// <param name="now"></param>
        /// <param name="equipAccountMaintainProjects"></param>
        /// <param name="equipAccount"></param>
        private void CreateMaintainPlan(DateTime now, EntityList<EquipAccountMaintainProject> equipAccountMaintainProjects, EquipAccount equipAccount)
        {
            //每个设备生成一个设备保养单（保养单号、设备编码、保养状态（未执行）、计划开始结束时间（当前时间）字段赋值，其他字段为空），
            //保养项目子表取这个设备所有的保养项目
            var weekInfo = RT.Service.Resolve<MaintainController>().GetWeekInfoOfDateTime(now);
            var week = weekInfo.Item2;
            var firstDayOfWeek = weekInfo.Item3;
            var lastDayOfWeek = weekInfo.Item4;

            MaintainPlan maintainPlan = new MaintainPlan();
            maintainPlan.YearAndMonth = new DateTime(lastDayOfWeek.Year, lastDayOfWeek.Month, 1);
            maintainPlan.Cycle = week;
            maintainPlan.PlanBeginDate = firstDayOfWeek;
            maintainPlan.PlanEndDate = lastDayOfWeek;
            maintainPlan.EquipAccountId = equipAccount.Id;
            maintainPlan.ExeState = MaintExeState.NotPerformed;
            maintainPlan.MaintainSourceType = Checks.Plans.CheckSourceType.NewCreated;
            maintainPlan.MaintainNo = RT.Service.Resolve<MaintainController>().GetMaintainPlanNo();
            maintainPlan.IsAbnormalInfoPush = false;
            maintainPlan.GenerateId();

            //保养项目子表取这个设备所有的保养项目
            var curequipAccountMaintainProjects = equipAccountMaintainProjects.Where(m => m.EquipAccountId == equipAccount.Id).ToList();
            EntityList<MaintainProject> maintainProjects = new EntityList<MaintainProject>();
            foreach (var curequipAccountMaintainProject in curequipAccountMaintainProjects)
            {
                MaintainProject maintainProject = new MaintainProject()
                {
                    EquipAccountId = equipAccount.Id,
                    EquipMaintainProjectId = curequipAccountMaintainProject.Id,
                    MaxValue = curequipAccountMaintainProject.MaxValue,
                    MinValue = curequipAccountMaintainProject.MinValue,
                    MaintainPlanId = maintainPlan.Id,
                    CycleType = curequipAccountMaintainProject.CycleType,
                    Method = curequipAccountMaintainProject.Method,
                    Part = curequipAccountMaintainProject.Part,
                    UseTime = curequipAccountMaintainProject.UseTime,
                    Unit = curequipAccountMaintainProject.Unit,
                    ProjectType = curequipAccountMaintainProject.ProjectType,
                    Standard = curequipAccountMaintainProject.Standard,
                    ProjectConsumable = curequipAccountMaintainProject.Consumable,
                };
                maintainProjects.Add(maintainProject);
            }
            RF.Save(maintainPlan);
            if (maintainProjects.Any())
            {
                RF.Save(maintainProjects);
            }
        }

        /// <summary>
        /// 获取封存闲置设备明细
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<IdleArchiveDetail> GetIdleArchiveDetailsByIdleArchiveIds(List<double> idleArchiveIds)
        {
            return idleArchiveIds.SplitContains(ids =>
            {
                return Query<IdleArchiveDetail>().Where(m => ids.Contains(m.IdleArchiveId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }


        /// <summary>
        /// 撤回
        /// </summary>
        /// <param name="selectedIds"></param>
        public virtual void CancelIdleArchives(List<double> selectedIds)
        {
            var idleArchives = GetListIdleArchivesByIds(selectedIds);
            if (idleArchives.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能操作".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                List<double> transfersIds = new List<double>();
                idleArchives.ForEach(p =>
                {
                    p.ApprovalStatus = ApprovalStatus.Draft;
                    transfersIds.Add(p.Id);
                });
                RF.Save(idleArchives);
                var now = RF.Find<IdleArchive>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(transfersIds, typeof(IdleArchive).FullName, ApprovalResult.Retract, now, "");
                trans.Complete();
            }
        }

        /// <summary>
        /// 获取闲置封存集合
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        private EntityList<IdleArchive> GetListIdleArchivesByIds(List<double> selectedIds)
        {
            return selectedIds.SplitContains(itemIds =>
            {
                return Query<IdleArchive>().Where(m => itemIds.Contains(m.Id)).ToList();
            });
        }

        /// <summary>
        /// 保存闲置封存
        /// </summary>
        /// <param name="model"></param>
        public virtual void SaveIdleArchive(IdleArchive model)
        {
            if (model.Remark.IsNullOrEmpty())
            {
                throw new ValidationException("业务说明必填".L10N());
            }

            if (model.TypeCategory.IsNullOrEmpty())
            {
                throw new ValidationException("设备类别必填".L10N());
            }
            //保存时校验同一个设备台账只能存在于一个未完结（审核状态不为【通过】）的单据中

            if (model.PersistenceStatus == PersistenceStatus.New && !model.IdleArchiveDetailList.Any())
            {
                throw new ValidationException("设备明细必填！".L10N());
            }

            var equipIds = model.IdleArchiveDetailList.Select(m => m.EquipAccountId).ToList();
            if (GetSameEquipmentBill(model, equipIds))
            {
                throw new ValidationException("同一个设备台账只能存在于一个未完结单据！".L10N());
            }

            if (model.IdleArchiveType == IdleArchiveType.Idle || model.IdleArchiveType == IdleArchiveType.Archive)
            {
                var config = ConfigService.GetConfig(new IdleArchivesMaximumTermConfig(), typeof(IdleArchive));
                model.IdleArchiveDetailList.ForEach(item =>
                {
                    if (!item.Deadline.HasValue)
                    {
                        throw new ValidationException("闲置封存时，闲置封存期限为必填！".L10N());
                    }
                    if (item.Deadline.Value <= model.ApplyDate)
                    {
                        throw new ValidationException("闲置/封存期限须大于申请时间！".L10N());
                    }

                    //申请日期到闲置封存期限的时间长度不能大于配置项【闲置最长期限（月）】维护的月数（未维护时不校验）
                    if (model.IdleArchiveType == IdleArchiveType.Idle && config != null && config.MaximumTerm > 0)
                    {
                        var totalDays = (item.Deadline.Value - model.ApplyDate).TotalDays;
                        if (totalDays > config.MaximumTerm)
                        {
                            throw new ValidationException("申请日期到闲置封存期限的时间长度不能大于配置项【闲置最长期限（天）】维护的天数！".L10N());
                        }
                    }
                });
            }

            RF.Save(model);
        }

        /// <summary>
        /// 获取相同设备未完结的单据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="equipIds"></param>
        private bool GetSameEquipmentBill(IdleArchive model, List<double> equipIds)
        {
            var result = Query<IdleArchive>().Join<IdleArchiveDetail>((x, y) => x.Id == y.IdleArchiveId && equipIds.Contains(y.EquipAccountId))
               .Where(m => m.ApprovalStatus != ApprovalStatus.Audited)
               .WhereIf(model.PersistenceStatus != PersistenceStatus.New, m => m.Id != model.Id).ToList();
            return result.Any();
        }

        /// <summary>
        /// 获取封存闲置
        /// </summary>
        /// <returns></returns>
        public virtual IdleArchive GetIdleArchive()
        {
            var entity = new IdleArchive();
            entity.No = RT.Service.Resolve<CommonController>().GetNo<IdleArchive>("闲置与封存");
            entity.IdleArchiveType = Enums.IdleArchiveType.Idle;
            entity.ApprovalStatus = ApprovalStatus.Draft;
            entity.ApplyDate = DateTime.Now;
            entity.ApplicantId = RT.IdentityId;
            entity.ApplicantName = RF.GetById<Employee>(RT.IdentityId).Name;
            return entity;
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="selectedIds"></param>
        public virtual void Sumbit(List<double> selectedIds)
        {
            var idleArchives = GetListIdleArchivesByIds(selectedIds);
            if (idleArchives.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有状态为【待提交】、【驳回】的数据才能提交".L10N());
            }
            if (idleArchives.Any(m => !m.IdleArchiveDetailList.Any()))
            {
                throw new ValidationException("提交数据必须存在至少一条设备明细！".L10N());
            }
            var config = ConfigService.GetConfig(new ApprovalConfig(), typeof(IdleArchive));
            if (config == null)
            {
                throw new ValidationException("未配置审批流程配置，请配置！".L10nFormat());
            }

            var now = RF.Find<IdleArchive>().GetDbTime();
            var recordIds = new List<double>();

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var item in idleArchives)
                {
                    item.ApprovalStatus = ApprovalStatus.PendingReview;
                    recordIds.Add(item.Id);
                }
                RF.Save(idleArchives);
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(recordIds, typeof(IdleArchive).FullName, ApprovalResult.Submit, now, "");
                //是否启用审批为false时提交后自动审批
                if (!config.EnableAudit)
                {
                    SubmitApproval(selectedIds, ApprovalResult.Pass, "通过!".L10N());
                }
                trans.Complete();
            }
        }
    }
}
