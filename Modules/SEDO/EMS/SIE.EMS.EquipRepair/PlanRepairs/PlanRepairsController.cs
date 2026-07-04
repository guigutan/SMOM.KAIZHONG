using SIE.Common.Configs;
using SIE.Core.Common.Controllers;
using SIE.DataAuth;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepair.EquipRepairs.Enums;
using SIE.EMS.EquipRepairs.Enums;
using SIE.EMS.RunStandards;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.WorkFlows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.EquipRepair.PlanRepairs
{
    /// <summary>
    /// 计划维修控制器
    /// </summary>
    public class PlanRepairsController : DomainController
    {
        /// <summary>
        /// 查询计划维修
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<PlanRepair> CriteriaPlanRepairss(PlanRepairCriteria criteria)
        {
            var query = Query<PlanRepair>();
            if (!criteria.No.IsNullOrEmpty())
            {
                query.Where(p => p.No.Contains(criteria.No));
            }

            if (criteria.EquipAccountId.HasValue)
            {
                query.Where(p => p.EquipAccountId == criteria.EquipAccountId.Value);
            }

            if (criteria.StandardType.HasValue)
            {
                query.Where(p => p.StandardType == criteria.StandardType);
            }
            if (criteria.ApprovalStatus.HasValue)
            {
                query.Where(p => p.ApprovalStatus == criteria.ApprovalStatus.Value);
            }
            if (criteria.CreateId.HasValue)
            {
                query.Where(p => p.CreateBy == criteria.CreateId.Value);
            }
            if (criteria.PlanDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.PlanDate.BeginValue.Value);
            }
            if (criteria.PlanDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.PlanDate.EndValue.Value);
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 强制关单
        /// </summary>
        /// <param name="selectedIds"></param>
        public virtual void ForcedShutdown(List<double> selectedIds)
        {
            var planRepairs = GetListByIds(selectedIds);
            if (planRepairs.Any(p => p.ApprovalStatus == ApprovalStatus.Audited))
            {
                throw new ValidationException("已审批计划维修不允许关闭".L10N());
            }
            planRepairs.ForEach(item =>
            {
                item.Close = YesNo.Yes;
            });
            RF.Save(planRepairs);
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="selectedIds"></param>
        public virtual void Sumbit(List<double> selectedIds)
        {
            var planRepairs = GetListByIds(selectedIds);
            if (planRepairs.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有状态为【待提交】、【驳回】的数据才能提交".L10N());
            }
            var config = ConfigService.GetConfig(new ApprovalConfig(), typeof(PlanRepair));
            if (config == null)
            {
                throw new ValidationException("未配置审批流程配置，请配置！".L10nFormat());
            }

            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                var now = RF.Find<PlanRepair>().GetDbTime();
                var recordIds = new List<double>();
                foreach (var item in planRepairs)
                {
                    //【是】则更新审核状态为【待审核】，【否】则更新审核状态为【通过】
                    item.ApprovalStatus = config.EnableAudit ? ApprovalStatus.PendingReview : ApprovalStatus.Audited;
                    if (item.ApprovalStatus == ApprovalStatus.Audited)
                    {
                        item.Remark = "通过!".L10N();
                    }
                    recordIds.Add(item.Id);
                }
                RF.Save(planRepairs);
                if (!config.EnableAudit)//不启用流程审批则直接生成维修工单
                {
                    var resultRepairs = GenerateRepairOrder(planRepairs);
                    if (resultRepairs.Any())
                    {
                        RF.Save(resultRepairs);
                    }
                }
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(recordIds, typeof(PlanRepair).FullName, ApprovalResult.Submit, now, "");
                trans.Complete();
            }

        }

        /// <summary>
        /// 根据ID集合获取计划维修列表
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        public virtual EntityList<PlanRepair> GetListByIds(List<double> selectedIds)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(PlanRepair.EquipAccountProperty);
            elo.LoadWith(PlanRepair.PlanRepairProjectListProperty);
            elo.LoadWithViewProperty();
            return selectedIds.SplitContains(ids =>
            {
                return Query<PlanRepair>().Where(m => ids.Contains(m.Id)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 获取计划维修
        /// </summary>
        /// <returns></returns>
        public virtual PlanRepair GetPlanRepair()
        {
            var entity = new PlanRepair();
            entity.No = RT.Service.Resolve<CommonController>().GetNo<PlanRepair>("计划维修");
            entity.CreateDate = DateTime.Now;
            entity.ApprovalStatus = ApprovalStatus.Draft;
            entity.CreateBy = RT.IdentityId;
            entity.BillSourceType = BillSourceType.Manual;

            return entity;
        }

        /// <summary>
        /// 保存计划维修
        /// </summary>
        /// <param name="model"></param>
        public virtual void SavePlanRepair(PlanRepair model)
        {
            if (model.Name.IsNullOrEmpty())
            {
                throw new ValidationException("名称必填".L10N());
            }
            RF.Save(model);
        }

        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="selectedIds"></param>
        public virtual void Cancel(List<double> selectedIds)
        {
            var runStandards = GetListByIds(selectedIds);
            if (runStandards.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能操作".L10N());
            }
            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                List<double> transfersIds = new List<double>();
                runStandards.ForEach(p =>
                {
                    p.ApprovalStatus = ApprovalStatus.Draft;
                    transfersIds.Add(p.Id);

                });
                RF.Save(runStandards);
                var now = RF.Find<PlanRepair>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(transfersIds, typeof(PlanRepair).FullName, ApprovalResult.Retract, now, "");
                trans.Complete();
            }
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
            var planRepairList = GetListByIds(selectedIds);
            //验证只有执行中的数据才能审核
            if (planRepairList.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            }

            var status = value == ApprovalResult.Pass ? ApprovalStatus.Audited : ApprovalStatus.Reject;
            var now = RF.Find<PlanRepair>().GetDbTime();
            var ids = new List<double>();
            var resultRepairs = new EntityList<EquipRepairBill>();
            planRepairList.ForEach(item =>
            {
                item.ApprovalStatus = status;
                ids.Add(item.Id);


            });
            //审核通过 生成维修工单
            if (value == ApprovalResult.Pass)
            {
                resultRepairs = GenerateRepairOrder(planRepairList);
            }

            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                if (resultRepairs.Any())
                {
                    RF.Save(resultRepairs);
                }
                RF.Save(planRepairList);
                //保存成功之后添加审核记录
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(ids, typeof(PlanRepair).FullName, value, now, remark);

                trans.Complete();
            }
        }

        /// <summary>
        /// 生成计划维修工单
        /// </summary>
        private EntityList<EquipRepairBill> GenerateRepairOrder(EntityList<PlanRepair> planRepairList)
        {
            var results = new EntityList<EquipRepairBill>();
            var now = RF.Find<EquipRepairBill>().GetDbTime();
            var ctr = RT.Service.Resolve<RepairController>();
            foreach (var item in planRepairList)
            {
                var newItem = new EquipRepairBill()
                {
                    ApplyRepairDate = now,
                    ApplyRepairEmployeeId = item.CreateBy,
                    EquipAccountId = item.EquipAccountId,
                    ProjectId = item.ProjectId,
                    ProjectKeyItemId = item.ProjectKeyItemId,
                    RepairCategory = RepairCategory.Plan,
                    RepairNo = ctr.GenerateRepairNo(),
                    RepairType = EquipRepairType.EquipRepair,
                    SourceType = RepairSourceType.PlanRepair,
                    RepairState = EquipRepairState.ApplyRepair,
                    ProduceState = item.EquipAccountState == Core.Enums.AccountState.Running ? ProduceState.Produce : ProduceState.StopWork,
                    SourceNo = item.No,
                    UrgentDegree = UrgentDegree.Common

                };
                newItem.GenerateId();
                item.EquipRepairBillId = newItem.Id;
                item.PlanRepairProjectList.ForEach(child =>
                {

                    newItem.EquipRepairBillProjectList.Add(new EquipRepairBillProject()
                    {
                        EquipRepairBillId = newItem.Id,
                        Consumable = child.Consumable,
                        Method = child.Method,
                        MaxValue = child.MaxValue,
                        MinValue = child.MinValue,
                        Part = child.Part,
                        ProjectDetailId = child.ProjectDetailId,
                        Standard = child.Standard,
                        Unit = child.Unit,
                        UseTime = child.UseTime
                    });

                });
                newItem.EquipRepairOperationRecList.Add(new EquipRepairOperationRec()
                {
                    EquipRepairBillId = newItem.Id,
                    OperationType = RepairOperationType.ApplyRepair,
                    OperationDate = now,
                    OperationerId = RT.IdentityId,
                });
                results.Add(newItem);
            }
            return results;

        }

        /// <summary>
        /// 调度自动生成计划维修
        /// </summary>
        public virtual EntityList<PlanRepair> SyncSchedulingAutoGeneratePlanRepairs()
        {
            EntityList<EquipAccount> equipments;
            //授予调度获取所有设备的权限
            using (DataAuths.LoadAll())
            {
                //缓存所有的设备+设备维修定标
                equipments = Query<EquipAccount>().ToList();
            }
            var planRepairList = new EntityList<PlanRepair>();
            if (equipments.Any())
            {
                var allEquipmentIds = equipments.Select(m => m.Id).ToList();
                //根据所有的设备台账Id集合获取它们所绑定的所有的维修定标数据
                var allEquipAccountRepairStandards = allEquipmentIds.SplitContains(ids =>
                {
                    return Query<EquipAccountRepairStandard>().Where(m => ids.Contains(m.EquipAccountId)).ToList();
                });
                var now = RF.Find<EquipAccountRepairStandard>().GetDbTime();
                var needUpdateStandard = new EntityList<EquipAccountRepairStandard>();
                //循环每一个设备 
                foreach (var equipment in equipments)
                {
                    //找到该设备是否存在维修定标数据
                    var equipmentRepairStandards = allEquipAccountRepairStandards.Where(m => m.EquipAccountId == equipment.Id).ToList();
                    //当天只会生成一次
                    foreach (var equipmentRepairStandard in equipmentRepairStandards)
                    {
                        // 时间周期类/ 且维护了下一个生成日期
                        if (equipmentRepairStandard.StandardType == Enums.StandardType.Period && equipmentRepairStandard.NextExecuteDate.HasValue)
                        {
                            //下次执行日期 - 预警期小于等于当前日期  循环这里考虑调度失败，后期重启调度补救
                            while (equipmentRepairStandard.NextExecuteDate.Value.AddDays(-equipmentRepairStandard.LeadTime).Date <= now.Date)
                            {
                                var executeDate = equipmentRepairStandard.NextExecuteDate.Value.AddDays(-equipmentRepairStandard.LeadTime).Date;
                                //根据设备维护的定标量找到设备运行定标单号
                                RunStandard runStandard = GetRunStandard(equipmentRepairStandard);
                                //跟新设备绑定的维修定标数据
                                equipmentRepairStandard.LastExecuteDate = executeDate;
                                equipmentRepairStandard.NextExecuteDate = executeDate.AddDays(equipmentRepairStandard.Amount);//下一次执行时间未当前日期加周期量
                                GeneratePlanRepair(planRepairList, equipmentRepairStandard, runStandard); 
                                //生成计划维修后更新下一次时间
                                var periodItems = equipmentRepairStandards.FindAll(m => m.StandardType != Enums.StandardType.Period);
                                //定标数据只要一条满足执行 其它非时间周期的均重置为0
                                foreach (var periodItem in periodItems)
                                {
                                    //非时间周期类的更新累计数为0 避免重复生成
                                    periodItem.RoundAmount = 0;
                                    needUpdateStandard.Add(periodItem);//缓存列表用于统一提交数据库
                                }

                                needUpdateStandard.Add(equipmentRepairStandard);
                            }
                            break;//一种满足后不再执行第二种类型生成计划维修
                        }
                        if (equipmentRepairStandard.StandardType != Enums.StandardType.Period && equipmentRepairStandard.RoundAmount >= equipmentRepairStandard.Amount - equipmentRepairStandard.LeadTime)//累加数大于等于（周期量-预警期）
                        {
                            RunStandard runStandard = GetRunStandard(equipmentRepairStandard);
                            GeneratePlanRepair(planRepairList, equipmentRepairStandard, runStandard);//生成计划维修后更新时间周期类型的执行时间
                            equipmentRepairStandard.RoundAmount = 0;
                            var periodItem = equipmentRepairStandards.FirstOrDefault(m => m.StandardType == Enums.StandardType.Period);
                            if (periodItem != null)//时间周期类的更新上次执行时间和下次执行时间
                            {
                                periodItem.LastExecuteDate = now;
                                periodItem.NextExecuteDate = now.AddDays(periodItem.Amount);
                                needUpdateStandard.Add(periodItem);
                            }
                            needUpdateStandard.Add(equipmentRepairStandard);
                            break;//一种满足后不再执行第二种类型生成计划维修
                        }
                    }
                }

                using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
                {
                    if (needUpdateStandard.Any())
                    {
                        RF.Save(needUpdateStandard);
                    }
                    if (planRepairList.Any())
                    {
                        RF.Save(planRepairList);
                    }
                    trans.Complete();
                }
            }
            return planRepairList;
        }

        /// <summary>
        /// 获取维修定标
        /// </summary>
        /// <param name="equipmentRepairStandard"></param>
        /// <returns></returns>
        private RunStandard GetRunStandard(EquipAccountRepairStandard equipmentRepairStandard)
        {
            return Query<RunStandard>().Join<RunStandardValue>((n, x) => x.RunStandardId == n.Id && x.Id == equipmentRepairStandard.RunStandardValueId).FirstOrDefault(new EagerLoadOptions().LoadWith(RunStandard.RunStandardProjectListProperty));
        }


        /// <summary>
        /// 生成计划维修
        /// </summary>
        /// <param name="planRepairList"></param>
        /// <param name="item"></param>
        /// <param name="runStandard"></param>
        private void GeneratePlanRepair(EntityList<PlanRepair> planRepairList, EquipAccountRepairStandard item, RunStandard runStandard)
        {
            if (runStandard != null)
            {
                //生成主表信息
                PlanRepair planRepair = new PlanRepair()
                {
                    RoundAmount = item.RoundAmount,
                    LeadTime = item.LeadTime,
                    LastExecuteDate = item.LastExecuteDate,
                    EquipAccRepairStandardId = item.Id,
                    Name = runStandard.Name,//定标名称
                    No = RT.Service.Resolve<CommonController>().GetNo<PlanRepair>("计划维修"),
                    RunStandardNo = runStandard.No,
                    EquipAccountId = item.EquipAccountId,
                    ApprovalStatus = ApprovalStatus.Draft,
                    BillSourceType = BillSourceType.Automatically,
                    StandardType = item.StandardType,
                    StandardUnit = item.StandardUnit,
                    Amount = item.Amount,
                };

                planRepair.GenerateId();

                //生成维修规则
                runStandard.RunStandardProjectList.ForEach(runStandardProject =>
                {
                    planRepair.PlanRepairProjectList.Add(new PlanRepairProject()
                    {
                        Consumable = runStandardProject.Consumable,
                        DepartmentId = runStandardProject.DepartmentId,
                        Method = runStandardProject.Method,
                        Part = runStandardProject.Part,
                        MinValue = runStandardProject.MinValue,
                        MaxValue = runStandardProject.MaxValue,
                        PlanRepairId = planRepair.Id,
                        ProjectDetailId = runStandardProject.ProjectDetailId,
                        Standard = runStandardProject.Standard,
                        Unit = runStandardProject.Unit,
                        UseTime = runStandardProject.UseTime,                        
                    });
                });

                planRepairList.Add(planRepair);
            }
        }

        /// <summary>
        /// 获取审批流程配置
        /// </summary>
        /// <returns>审批流程配置</returns>
        public virtual ApprovalConfigValue GetApprovalConfigValue(Type type)
        {
            var config = ConfigService.GetConfig(new ApprovalConfig(), type);
            if (config == null)
            {
                throw new ValidationException("未找到审批流程配置,请检查规则配置".L10N());
            }
            return config;
        }
    }
}
