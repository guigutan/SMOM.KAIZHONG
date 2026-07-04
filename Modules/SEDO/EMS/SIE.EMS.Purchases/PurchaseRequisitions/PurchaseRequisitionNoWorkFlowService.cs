using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Enums;
using SIE.Equipments.WorkFlows;
using System.Linq;
using System;

namespace SIE.EMS.Purchases.PurchaseRequisitions
{
    /// <summary>
    /// 资产采购申请的工作流接口
    /// </summary>    
    public abstract class PurchaseRequisitionWorkFlowService
    {
        /// <summary>
        /// 提交审核
        /// </summary>
        /// <param name="purchaseRequisitions">资产采购申请的列表</param>
        public void Submit(EntityList<PurchaseRequisition> purchaseRequisitions)
        {
            if (purchaseRequisitions.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有状态为【待提交】、【驳回】的数据才能提交".L10N());
            }

            SubmitPurchaseRequisitions(purchaseRequisitions);
        }
        /// <summary>
        /// 提交审核
        /// </summary>
        /// <param name="purchaseRequisitions">资产采购申请的列表</param>
        protected abstract void SubmitPurchaseRequisitions(EntityList<PurchaseRequisition> purchaseRequisitions);

        /// <summary>
        /// 核准
        /// </summary>
        /// <param name="purchaseRequisitions">资产采购申请的列表</param>
        /// <param name="remark">审核意见</param>
        public void Approved(EntityList<PurchaseRequisition> purchaseRequisitions, string remark)
        {
            if (purchaseRequisitions == null || !purchaseRequisitions.Any())
            {
                throw new ValidationException("资产采购申请的列表为空，提交审核失败".L10N());
            }

            //审核前验证审核状态
            CheckApprovalStatusBeforeApproved(purchaseRequisitions);

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                var now = RF.Find<PurchaseRequisition>().GetDbTime();
                foreach (var pr in purchaseRequisitions)
                {
                    pr.ApprovalStatus = ApprovalStatus.Audited;
                    pr.ApprovalTime = now;
                }

                RF.Save(purchaseRequisitions);

                CreateApprovedWorkFlowLog(purchaseRequisitions, remark, now);

                trans.Complete();
            }
        }

        /// <summary>
        /// 核准前验证审核状态
        /// </summary>
        /// <param name="purchaseRequisitions">资产采购申请的列表</param>
        protected abstract void CheckApprovalStatusBeforeApproved(EntityList<PurchaseRequisition> purchaseRequisitions);

        /// <summary>
        /// 驳回
        /// </summary>
        /// <param name="purchaseRequisitions">资产采购申请的列表</param>
        /// <param name="remark">审核意见</param>
        public void Reject(EntityList<PurchaseRequisition> purchaseRequisitions, string remark)
        {
            if (purchaseRequisitions == null || !purchaseRequisitions.Any())
            {
                throw new ValidationException("资产采购申请的列表为空，提交审核失败".L10N());
            }

            CheckApprovalStatusBeforeReject(purchaseRequisitions);

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                var now = RF.Find<PurchaseRequisition>().GetDbTime();
                foreach (var pr in purchaseRequisitions)
                {
                    pr.ApprovalStatus = ApprovalStatus.Reject;
                    pr.ApprovalTime = now;
                }

                RF.Save(purchaseRequisitions);

                CreateRejectWorkFlowLog(purchaseRequisitions, remark, now);

                trans.Complete();
            }
        }

        /// <summary>
        /// 驳回前验证审核状态
        /// </summary>
        /// <param name="purchaseRequisitions">资产采购申请的列表</param>
        protected abstract void CheckApprovalStatusBeforeReject(EntityList<PurchaseRequisition> purchaseRequisitions);

        /// <summary>
        /// 创建驳回的工作流日志
        /// </summary>
        /// <param name="purchaseRequisitions">采购申请单列表</param>
        /// <param name="remark">备注</param>
        /// <param name="now">时间</param>
        protected void CreateRejectWorkFlowLog(EntityList<PurchaseRequisition> purchaseRequisitions, string remark, DateTime now)
        {
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(purchaseRequisitions.Select(x => x.Id).ToList(),
               typeof(PurchaseRequisition).FullName, ApprovalResult.Reject, now, remark);
        }

        /// <summary>
        /// 撤回采购申请
        /// </summary>
        /// <param name="purchaseRequisitions">采购申请单列表</param>        
        public virtual void Withdraw(EntityList<PurchaseRequisition> purchaseRequisitions)
        {
            CheckApprovalStatusBeforeRetract(purchaseRequisitions);

            UpdateApprovalStatusOnWithdraw(purchaseRequisitions);

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(purchaseRequisitions);

                var now = RF.Find<PurchaseRequisition>().GetDbTime();

                CreateRetractWorkFlowLog(purchaseRequisitions, now);

                trans.Complete();
            }
        }

        /// <summary>
        /// 撤回时更新审核状态
        /// </summary>
        /// <param name="purchaseRequisitions">采购申请单列表</param>
        protected abstract void UpdateApprovalStatusOnWithdraw(EntityList<PurchaseRequisition> purchaseRequisitions);

        /// <summary>
        /// 检查单据状态在撤回前
        /// </summary>
        /// <param name="purchaseRequisitions">采购申请单列表</param>
        protected abstract void CheckApprovalStatusBeforeRetract(EntityList<PurchaseRequisition> purchaseRequisitions);

        /// <summary>
        /// 创建驳回的工作流日志
        /// </summary>
        /// <param name="purchaseRequisitions">采购申请单列表</param>        
        /// <param name="now">时间</param>
        protected void CreateRetractWorkFlowLog(EntityList<PurchaseRequisition> purchaseRequisitions, DateTime now)
        {
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(purchaseRequisitions.Select(x => x.Id).ToList(),
                typeof(PurchaseRequisition).FullName, ApprovalResult.Retract, now, "");
        }

        /// <summary>
        /// 创建核准的工作流日志
        /// </summary>
        /// <param name="purchaseRequisitions">采购申请单列表</param>
        /// <param name="remark">备注</param>
        /// <param name="now">时间</param>
        protected void CreateApprovedWorkFlowLog(EntityList<PurchaseRequisition> purchaseRequisitions, string remark, DateTime now)
        {
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(purchaseRequisitions.Select(x => x.Id).ToList(),
                                typeof(PurchaseRequisition).FullName, ApprovalResult.Pass, now, remark);
        }
    }

    /// <summary>
    /// 工作流服务-不使用工作流引擎（即按钮审核）
    /// </summary>
    public class PurchaseRequisitionNoWorkFlowService : PurchaseRequisitionWorkFlowService
    {
        /// <summary>
        /// 提交审核
        /// </summary>
        /// <param name="purchaseRequisitions">资产采购申请的列表</param>
        /// <exception cref="ValidationException">资产采购申请的列表为空，提交审核失败</exception>
        protected override void SubmitPurchaseRequisitions(EntityList<PurchaseRequisition> purchaseRequisitions)
        {
            if (purchaseRequisitions == null || !purchaseRequisitions.Any())
            {
                throw new ValidationException("资产采购申请的列表为空，提交审核失败".L10N());
            }

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var pur in purchaseRequisitions)
                {
                    pur.ApprovalStatus = ApprovalStatus.PendingReview;
                }

                RF.Save(purchaseRequisitions);

                var now = RF.Find<PurchaseRequisition>().GetDbTime();

                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(purchaseRequisitions.Select(x => x.Id).ToList(),
                    typeof(PurchaseRequisition).FullName, ApprovalResult.Submit, now, "");
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核前验证审核状态
        /// </summary>
        /// <param name="purchaseRequisitions">资产采购申请的列表</param>
        protected override void CheckApprovalStatusBeforeApproved(EntityList<PurchaseRequisition> purchaseRequisitions)
        {
            if (purchaseRequisitions.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            }
        }



        /// <summary>
        /// 驳回前验证审核状态
        /// </summary>
        /// <param name="purchaseRequisitions">资产采购申请的列表</param>
        protected override void CheckApprovalStatusBeforeReject(EntityList<PurchaseRequisition> purchaseRequisitions)
        {
            if (purchaseRequisitions.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能驳回".L10N());
            }
        }



        /// <summary>
        /// 检查单据状态在撤回前
        /// </summary>
        /// <param name="purchaseRequisitions">采购申请单列表</param>
        /// <exception cref="ValidationException">只有状态为【待审核】的数据才能操作</exception>
        protected override void CheckApprovalStatusBeforeRetract(EntityList<PurchaseRequisition> purchaseRequisitions)
        {
            if (purchaseRequisitions.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能操作".L10N());
            }
        }

        /// <summary>
        /// 撤回时更新审核状态
        /// </summary>
        /// <param name="purchaseRequisitions">采购申请单列表</param>
        protected override void UpdateApprovalStatusOnWithdraw(EntityList<PurchaseRequisition> purchaseRequisitions)
        {
            purchaseRequisitions.ForEach(p =>
            {
                p.ApprovalStatus = ApprovalStatus.Draft;
                p.WorkflowStartResult = "成功能撤消【资产采购申请】工作流实例".L10N();
            });
        }
    }
}
