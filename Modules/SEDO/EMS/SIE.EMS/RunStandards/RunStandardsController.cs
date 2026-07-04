using SIE.Common.Configs;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.WorkFlows;
using SIE.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.EMS.RunStandards
{
    /// <summary>
    /// 控制器
    /// </summary>
    public class RunStandardsController : DomainController
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<RunStandard> Fetch(RunStandardCriteria criteria)
        {
            var query = Query<RunStandard>();
            if (!criteria.No.IsNullOrEmpty())
            {
                query.Where(p => p.No.Contains(criteria.No));
            }

            if (criteria.EquipModelId.HasValue)
            {
                query.Where(p => p.EquipModelId == criteria.EquipModelId.Value);
            }

            if (!criteria.Name.IsNullOrEmpty())
            {
                query.Where(p => p.Name == criteria.Name);
            }
            if (criteria.CreateId.HasValue)
            {
                query.Where(p => p.CreateBy == criteria.CreateId.Value);
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
        /// 保存
        /// </summary>
        /// <param name="model"></param>
        public virtual void SaveRunStandard(RunStandard model)
        {
            if (model.Name.IsNullOrEmpty())
            {
                throw new ValidationException("名称必填".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                var record = new RunStandardLog()
                {
                    OperatorId = RT.IdentityId,
                    OperationTypeText = model.PersistenceStatus == PersistenceStatus.New ? "创建" : "录入",
                    RunStandardId = model.Id,
                    OperationDateTime = DateTime.Now
                };
                RF.Save(model);
                RF.Save(record);
                
                trans.Complete();
            }
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="selectedIds"></param>
        public virtual void Sumbit(List<double> selectedIds)
        {
            var idleArchives = GetListByIds(selectedIds);
            if (idleArchives.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有状态为【待提交】、【驳回】的数据才能提交".L10N());
            }
            if (idleArchives.Any(m => !m.RunStandardEquipmentList.Any()))
            {
                throw new ValidationException("提交数据必须存在至少一条设备明细！".L10N());
            }
            var config = ConfigService.GetConfig(new ApprovalConfig(), typeof(RunStandard));
            if (config == null)
            {
                throw new ValidationException("未配置审批流程配置，请配置！".L10nFormat());
            }

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                var now = RF.Find<RunStandard>().GetDbTime();

                //未启用审批时候直接生成
                //if (!config.EnableAudit)
                //{

                //}
                var recordIds = new List<double>();
                var recordOpas = new EntityList<RunStandardLog>();
                foreach (var item in idleArchives)
                {
                    item.ApprovalStatus = ApprovalStatus.PendingReview;
                    recordIds.Add(item.Id);

                    var record = new RunStandardLog()
                    {
                        OperatorId = RT.IdentityId,
                        OperationTypeText = "提交".L10N(),
                        RunStandardId = item.Id,
                        OperationDateTime = DateTime.Now
                    };
                    recordOpas.Add(record);
                }
                RF.Save(idleArchives);
                RF.Save(recordOpas);
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(recordIds, typeof(RunStandard).FullName, ApprovalResult.Submit, now, "");
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
            var runStandardList = GetListByIds(selectedIds);
            //验证只有执行中的数据才能审核
            if (runStandardList.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            }

            var status = value == ApprovalResult.Pass ? ApprovalStatus.Audited : ApprovalStatus.Reject;
            var now = RF.Find<RunStandard>().GetDbTime();
            var ids = new List<double>();
            var recordOpas = new EntityList<RunStandardLog>();
            runStandardList.ForEach(item =>
            {
                item.ApprovalStatus = status;
                ids.Add(item.Id);

                var record = new RunStandardLog()
                {
                    OperatorId = RT.IdentityId,
                    OperationTypeText = "审核".L10N(),
                    RunStandardId = item.Id,
                    OperationDateTime = DateTime.Now
                };
                recordOpas.Add(record);

            });
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(runStandardList);
                RF.Save(recordOpas);
                //保存成功之后添加审核记录
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(ids, typeof(RunStandard).FullName, value, now, remark);
                trans.Complete();
            }
        }

        /// <summary>
        /// 根据ID集合获取列表
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        public virtual EntityList<RunStandard> GetListByIds(List<double> selectedIds)
        {
            return selectedIds.SplitContains(items =>
            {
                return Query<RunStandard>().Where(n => items.Contains(n.Id)).ToList();
            });
        }

        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <returns></returns>
        public virtual RunStandard GetRunStandard()
        {
            var entity = new RunStandard();
            entity.No = RT.Service.Resolve<CommonController>().GetNo<RunStandard>("设备运行定标管理");
            entity.CreateDate = DateTime.Now;
            entity.ApprovalStatus = ApprovalStatus.Draft;
            entity.CreateBy = RT.IdentityId;
            entity.CreateName = RF.GetById<Employee>(RT.IdentityId).Name;
            return entity;
        }

        /// <summary>
        ///撤回
        /// </summary>
        /// <param name="selectedIds"></param>
        public virtual void Cancel(List<double> selectedIds)
        {
            var runStandards = GetListByIds(selectedIds);
            if (runStandards.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能操作".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                List<double> transfersIds = new List<double>();
                var recordOpas = new EntityList<RunStandardLog>();
                runStandards.ForEach(p =>
                {
                    p.ApprovalStatus = ApprovalStatus.Draft;
                    transfersIds.Add(p.Id);
                   
                    var record = new RunStandardLog()
                    {
                        OperatorId = RT.IdentityId,
                        OperationTypeText = "撤销".L10N(),
                        RunStandardId = p.Id,
                        OperationDateTime = DateTime.Now
                    };
                    recordOpas.Add(record);
                });
                RF.Save(runStandards);
                RF.Save(recordOpas);
                var now = RF.Find<RunStandard>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(transfersIds, typeof(RunStandard).FullName, ApprovalResult.Retract, now, "");
                trans.Complete();
            }
        }


        /// <summary>
        /// 根据IDs获取维修定标量
        /// </summary>
        /// <param name="runStandardValueIds"></param>
        /// <returns></returns>
        public virtual EntityList<RunStandardValue> GetRunStandardValueByIds(List<double> runStandardValueIds)
        {
            return runStandardValueIds.SplitContains(ids => {

                return Query<RunStandardValue>().Where(n => ids.Contains(n.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

    }
}
