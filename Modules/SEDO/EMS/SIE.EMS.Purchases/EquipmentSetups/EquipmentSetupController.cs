using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Purchases.Common.Controller;
using SIE.EMS.Purchases.Enums;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.WorkFlows;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 安装调试控制器
    /// </summary>
    public partial class EquipmentSetupController : DomainController
    {
        /// <summary>
        /// 查询安装调试
        /// </summary>
        /// <param name="criteria">安装调试查询</param>
        /// <returns>安装调试</returns>
        public virtual EntityList<EquipmentSetup> CriteriaEquipmentSetups(EquipmentSetupCriteria criteria)
        {
            var query = Query<EquipmentSetup>();
            if (criteria.FactoryId.HasValue)
            {
                query.Where(p => p.FactoryId == criteria.FactoryId.Value);
            }
            if (criteria.DepartmentId.HasValue)
            {
                query.Where(p => p.DepartmentId == criteria.DepartmentId.Value);
            }
            if (!criteria.No.IsNullOrWhiteSpace())
            {
                query.Where(p => p.SetupNo.Contains(criteria.No));
            }
            if (criteria.EquipAccountId.HasValue)
            {
                query.Exists<EquipmentDetail>((a, b) => b.Where(p => p.EquipmentSetupId == a.Id && p.EquipAccountId == criteria.EquipAccountId));
            }
            if (criteria.ApprovalStatus.HasValue)
            {
                query.Where(p => p.ApprovalStatus == criteria.ApprovalStatus.Value);
            }
            if (criteria.SetupStatus.HasValue)
            {
                query.Where(p => p.SetupStatus == criteria.SetupStatus.Value);
            }
            if (criteria.PrincipalId.HasValue)
            {
                query.Where(p => p.PrincipalId == criteria.PrincipalId.Value);
            }
            if (criteria.Overtime)
            {
                var now = DateTime.Now;
                query.Where(p => (p.SetupStatus == SetupStatus.ToBe || p.SetupStatus == SetupStatus.Doing) && p.PlanEndDate < now);
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
        /// 根据id列表获取安装调试
        /// </summary>
        /// <param name="ids">id列表</param>
        /// <returns>安装调试</returns>
        public virtual EntityList<EquipmentSetup> GetEquipmentSetupsByIds(List<double> ids)
        {
            return ids.SplitContains(id => Query<EquipmentSetup>().Where(p => id.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
        }

        /// <summary>
        /// 根据安装调试id列表获取安装调试工时登记
        /// </summary>
        /// <param name="setupIds">安装调试id列表</param>
        /// <returns>安装调试工时登记</returns>
        public virtual EntityList<SetupWorkHour> GetWorkHoursBySetupIds(List<double> setupIds)
        {
            return setupIds.SplitContains(ids => Query<SetupWorkHour>().Where(p => ids.Contains(p.EquipmentSetupId)).ToList());
        }

        /// <summary>
        /// 获取安装调试的设备明细
        /// </summary>
        /// <param name="setupId">安装调试id</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>设备明细</returns>
        public virtual EntityList<EquipAccount> GetEquipmentsBySetupId(double setupId, PagingInfo pagingInfo)
        {
            return Query<EquipAccount>().Exists<EquipmentDetail>((a, b) => b.Where(p => p.EquipAccountId == a.Id && p.EquipmentSetupId == setupId))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 创建一个新的安装调试
        /// </summary>
        /// <returns>新的安装调试</returns>
        public virtual EquipmentSetup GetNewEquipmentSetup()
        {
            var entity = new EquipmentSetup();
            entity.SetupNo = RT.Service.Resolve<CommonController>().GetNo<EquipmentSetup>("安装调试");
            entity.SetupStatus = SetupStatus.ToBe;
            entity.ApprovalStatus = ApprovalStatus.Draft;
            entity.PlanStartDate = DateTime.Now;
            entity.PlanEndDate = DateTime.Now;
            return entity;
        }

        /// <summary>
        /// 保存安装调试
        /// </summary>
        /// <param name="setup">安装调试</param>
        public virtual void SaveEquipmentSetup(EquipmentSetup setup)
        {
            if (setup == null)
            {
                throw new ValidationException("保存失败，数据异常".L10N());
            }
            if (setup.PersistenceStatus != PersistenceStatus.New)
            {
                var old = GetById<EquipmentSetup>(setup.Id);
                if (old == null)
                {
                    throw new ValidationException("保存失败，数据异常".L10N());
                }
                if (old.ApprovalStatus != ApprovalStatus.Draft && old.ApprovalStatus != ApprovalStatus.Reject)
                {
                    throw new ValidationException("保存失败，审核状态为【待提交】、【驳回】的数据才能修改".L10N());
                }
            }
            if (setup.PlanStartDate > setup.PlanEndDate)
            {
                throw new ValidationException("计划开始日期不能大于计划结束日期".L10N());
            }

            var workPlanCount = Query<EquipmentSetupPlan>().Where(p => p.EquipmentSetupId == setup.Id).Count();
            var editPlanCount = setup.EquipmentSetupPlanList.Count();
            var deletePlanCount = setup.EquipmentSetupPlanList.DeletedList.Count();
            if (workPlanCount + editPlanCount - deletePlanCount <= 0)
            {
                throw new ValidationException("工作计划至少有一个工作节点".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(setup);
                trans.Complete();
            }
        }

        /// <summary>
        /// 保存安装调试
        /// </summary>
        /// <param name="setupList">安装调试</param>
        public virtual void SaveEquipSetupList(EntityList<EquipmentSetup> setupList)
        {
            if (setupList == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            var logIds = new List<double>();
            var setupIds = setupList.Select(p => p.Id).ToList();
            var oldApplys = GetApplysBySetupIds(setupIds);
            var relation = GetRelationOutDepot();
            var newSetupSparePart = new List<SetupSparePart>();
            foreach (var setup in setupList)
            {
                //验证工时、备件申请
                CheckWorkHourApply(setup, oldApplys, logIds);
                //验证备件使用
                CheckSetupSparePartList(setup.SetupSparePartList, relation, newSetupSparePart);
            }
            var oldSpareParts = GetSparePartsBySetupIds(setupIds);
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //更新修改出库明细的【使用数量】
                UpdateUseCount(setupList, oldSpareParts);

                //保存界面数据
                RF.Save(setupList);

                //更新新增出库明细的【使用数量】为原来的值加上本条数据的【使用数量】
                UpdateAddUseCount(newSetupSparePart);

                //保存操作记录
                if (logIds.Any())
                {
                    var now = RF.Find<EquipmentSetup>().GetDbTime();
                    SaveSetupLog(logIds, "提交工时".L10N(), now);
                }
                //工时页签反写员工数量/耗费工时
                var allPlans = GetPlansBySetupIds(setupIds);
                var allWorkHours = GetWorkHoursBySetupIds(setupIds);
                foreach (var setup in setupList)
                {
                    var plans = allPlans.Where(p => p.EquipmentSetupId == setup.Id).ToList();
                    foreach (var plan in plans)
                    {
                        var workHours = allWorkHours.Where(p => p.EquipmentSetupId == setup.Id && p.EquipmentSetupPlanId == plan.Id).ToList();
                        plan.EmployeeCount = workHours.Select(p => p.EmployeeId).Distinct().Count();
                        plan.ActualWorkHours = workHours.Sum(p => p.Hours);
                    }
                }
                RF.Save(allPlans);
                trans.Complete();
            }
        }

        /// <summary>
        /// 删除前校验最新状态
        /// </summary>
        /// <param name="ids">实体id</param>
        public virtual void DeleteEquipmentSetup(List<double> ids)
        {
            var entity = GetEquipmentSetupsByIds(ids);
            if (entity.Any(p => p.ApprovalStatus != ApprovalStatus.Draft))
            {
                throw new ValidationException("只有状态为【待提交】的数据才能删除".L10N());
            }
            DB.Delete<EquipmentSetup>().Where(p => ids.Contains(p.Id)).Execute();
        }

        /// <summary>
        /// 提交安装调试
        /// </summary>
        /// <param name="ids">选择行id</param>
        public virtual void SubmitEquipmentSetup(List<double> ids)
        {
            var config = RT.Service.Resolve<PurchasesApprovalController>().GetApprovalConfigValue(typeof(EquipmentSetup));
            var now = RF.Find<EquipmentSetup>().GetDbTime();
            var setups = GetEquipmentSetupsByIds(ids);
            //只有审核状态为【待提交】、【驳回】的数据才能点击，点击时还要后台获取最新状态进行校验
            if (setups.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有审核状态为【待提交】、【驳回】的数据才能提交".L10N());
            }
            foreach (var setup in setups)
            {
                //更新状态为【待审核】
                setup.ApprovalStatus = ApprovalStatus.PendingReview;
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(setups);
                //生成审核结果为提交的审核记录数据
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(ids, typeof(EquipmentSetup).FullName, ApprovalResult.Submit, now, "");

                //生成【提交审核】的操作记录
                SaveSetupLog(ids, "提交审核".L10N(), now);
                //是否启用审批为false时提交后自动审批
                if (!config.EnableAudit)
                {
                    ExamineEquipmentSetupInner(ids, ApprovalResult.Pass, "通过".L10N(), setups);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 保存操作记录
        /// </summary>
        /// <param name="setupIds">安装调试id</param>
        /// <param name="text">操作</param>
        /// <param name="now">操作时间</param>
        private void SaveSetupLog(List<double> setupIds, string text, DateTime now)
        {
            var logs = new EntityList<SetupLog>();
            foreach (var id in setupIds)
            {
                var log = new SetupLog();
                log.EquipmentSetupId = id;
                log.OperationText = text;
                log.OperationDateTime = now;
                log.EmployeeId = RT.IdentityId;
                logs.Add(log);
            }
            RF.Save(logs);
        }

        /// <summary>
        /// 撤回安装调试
        /// </summary>
        /// <param name="setupIds">选择行id</param>
        public virtual void CancelEquipmentSetup(List<double> setupIds)
        {
            var setups = GetEquipmentSetupsByIds(setupIds);
            //只有状态为【待审核】的数据才能点击，点击时还要后台获取最新状态进行校验
            if (setups.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能操作".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //更新状态为【待提交】
                setups.ForEach(p => p.ApprovalStatus = ApprovalStatus.Draft);
                RF.Save(setups);

                //生成审核结果为撤回的审核记录数据
                var now = RF.Find<EquipmentSetup>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(setupIds, typeof(EquipmentSetup).FullName, ApprovalResult.Retract, now, "");

                //生成【撤回审核】的操作记录
                SaveSetupLog(setupIds, "撤回审核".L10N(), now);
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核安装调试
        /// </summary>
        /// <param name="setupIds">选择行id</param>
        /// <param name="value">结果</param>
        /// <param name="remark">备注</param>
        public virtual void ExamineEquipmentSetup(List<double> setupIds, ApprovalResult value, string remark)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                ExamineEquipmentSetupInner(setupIds, value, remark);
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核安装调试
        /// </summary>
        /// <param name="setupIds">选择行id</param>
        /// <param name="value">结果</param>
        /// <param name="remark">备注</param>
        /// <param name="setups">数据组</param>
        public virtual void ExamineEquipmentSetupInner(List<double> setupIds, ApprovalResult value, string remark, EntityList<EquipmentSetup> setups = null)
        {
            if (setups == null)
            {
                setups = GetEquipmentSetupsByIds(setupIds);
                if (!setups.Any())
                {
                    throw new ValidationException("审核数据异常".L10N());
                }
            }
            if (setups.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            }

            foreach (var accept in setups)
            {
                //更新审核状态为【通过】或【驳回】
                accept.ApprovalStatus = value == ApprovalResult.Pass ? ApprovalStatus.Audited : ApprovalStatus.Reject;
            }
            RF.Save(setups);

            //往审批记录子表插入一条数据
            var now = RF.Find<EquipmentSetup>().GetDbTime();
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(setupIds, typeof(EquipmentSetup).FullName, value, now, remark);

            //生成【审核通过】或【审核驳回】的操作记录
            var text = value == ApprovalResult.Pass ? "审核通过".L10N() : "审核驳回".L10N();
            SaveSetupLog(setupIds, text, now);
        }

        /// <summary>
        /// 转派
        /// </summary>
        /// <param name="setupIds">选择行id</param>
        /// <param name="principalId">负责人id</param>
        public virtual void Reassignment(List<double> setupIds, double principalId)
        {
            var setups = GetEquipmentSetupsByIds(setupIds);
            //审核状态为【待提交】、【驳回】的数据才能点击
            if (setups.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有审核状态为【待提交】、【驳回】的数据才能转派".L10N());
            }
            var employee = GetById<Employee>(principalId);
            if (employee == null)
            {
                throw new ValidationException("数据异常，员工不存在".L10N());
            }
            var newPrincipal = employee.Name + "(" + employee.Code + ")";
            var now = RF.Find<EquipmentSetup>().GetDbTime();
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var setup in setups)
                {
                    //生成一条操作记录【责任人XXX转派为XXX（姓名+员工号）】
                    var text = "责任人{0}转派为{1}".L10nFormat(setup.PrincipalName + "(" + setup.PrincipalCode + ")", newPrincipal);
                    SaveSetupLog(new List<double> { setup.Id }, text, now);

                    //将责任人更新
                    setup.PrincipalId = employee.Id;
                }
                RF.Save(setups);
                trans.Complete();
            }
        }

        /// <summary>
        /// 交机确认
        /// </summary>
        /// <param name="setupIds">选择行id</param>
        public virtual void HandoverConfirm(List<double> setupIds)
        {
            var setups = GetEquipmentSetupsByIds(setupIds);
            if (setups.Any(p => p.SetupStatus != SetupStatus.Done))
            {
                throw new ValidationException("只有状态为【已完成】的数据才能操作".L10N());
            }
            setups.ForEach(p => p.SetupStatus = SetupStatus.DeliveryConfirm);
            RF.Save(setups);
        }

        /// <summary>
        /// 保存附件
        /// </summary>
        /// <param name="data">附件</param>
        public virtual void SaveSetupAttachment(SetupAttachment data)
        {
            if (data == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            var now = RF.Find<EquipmentSetup>().GetDbTime();
            data.UploaderId = RT.IdentityId;
            data.UploadDate = now;
            data.PersistenceStatus = PersistenceStatus.New;
            if (string.IsNullOrEmpty(data.FilePath) || data.FilePath == " ")
            {
                throw new ValidationException("文件路径不能为空，请上传文件！".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(data);
                //生成【附件上传】的操作记录
                SaveSetupLog(new List<double> { data.EquipmentSetupId }, "附件上传".L10N(), now);
                trans.Complete();
            }
        }

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="setupId">安装调试id</param>
        /// <param name="planId">计划id</param>
        /// <param name="equipAccountId">设备id</param>
        public virtual void DeleteSetupAttachment(double setupId, double? planId, double? equipAccountId)
        {
            if (planId.HasValue)
            {
                DB.Delete<SetupAttachment>().Where(p => p.EquipmentSetupId == setupId && p.EquipmentSetupPlanId == planId.Value).Execute();
            }
            if (equipAccountId.HasValue)
            {
                DB.Delete<SetupAttachment>().Where(p => p.EquipmentSetupId == setupId && p.EquipAccountId == equipAccountId.Value).Execute();
            }
        }
    }
}
