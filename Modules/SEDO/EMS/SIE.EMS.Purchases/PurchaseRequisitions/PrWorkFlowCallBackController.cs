using SIE.Api;
using SIE.DataAuth;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;

namespace SIE.EMS.Purchases.PurchaseRequisitions
{
    /// <summary>
    /// 资产采购申请回调控制器
    /// </summary>
    public class PrWorkFlowCallBackController : DomainController
    {
        /// <summary>
        /// 流程实例ID
        /// </summary>
        private const string FlowInstanceIdKey = "FlowInstanceId";

        /// <summary>
        /// 核准回调方法
        /// </summary>
        /// <param name="data">流程上下文</param>
        /// <param name="input">输入对象</param>
        [ApiService("采购申请审批通过的回调方法")]
        public virtual void Approved(IDictionary<string, object> data, object input)
        {
            if (!data.ContainsKey(FlowInstanceIdKey))
            {
                throw new ValidationException("流程上下文中不包含[流程实例ID(FlowInstanceId)]".L10N());
            }

            var flowInstanceId = data[FlowInstanceIdKey].ToString();

            Approved(flowInstanceId);
        }


        /// <summary>
        /// 驳回回调方法
        /// </summary>
        /// <param name="data">流程上下文</param>
        /// <param name="input">输入对象</param>
        [ApiService("采购申请驳回的回调方法")]
        public virtual void Reject(IDictionary<string, object> data, object input)
        {
            if (!data.ContainsKey(FlowInstanceIdKey))
            {
                throw new ValidationException("流程上下文中不包含[流程实例ID(FlowInstanceId)]".L10N());
            }

            var flowInstanceId = data[FlowInstanceIdKey].ToString();

            Reject(flowInstanceId);
        }

        /// <summary>
        /// 驳回单据
        /// </summary>
        /// <param name="flowInstanceId">流程实例ID</param>
        protected virtual void Reject(string flowInstanceId)
        {
            using (DataAuths.LoadAll())
            {
                var purchaseRequisitions = Query<PurchaseRequisition>()
                   .Where(x => x.WorkflowInstanceId == flowInstanceId)
                   .ToList();

                 new PurchaseRequisitionInternalWorkFlowService().Reject(purchaseRequisitions, "");
            }
        }

        /// <summary>
        /// 核准单据
        /// </summary>
        /// <param name="flowInstanceId">流程实例ID</param>
        protected virtual void Approved(string flowInstanceId)
        {
            using (DataAuths.LoadAll())
            {
                var purchaseRequisitions = Query<PurchaseRequisition>()
                   .Where(x => x.WorkflowInstanceId == flowInstanceId)
                   .ToList();

                new PurchaseRequisitionInternalWorkFlowService()
                    .Approved(purchaseRequisitions, "");
            }
        }
    }
}
