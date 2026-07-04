using SIE.RedCardManagment.RedCardApplyBills;
using SIE.WorkFlow.WorkFlowInstances;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.RedCardManagment.WorkFlow.Events
{

    /// <summary>
    /// 红牌管理事件监听器
    /// </summary>
    public class RedCardManagmentListener
    {
        /// <summary>
        /// 实例
        /// </summary>
        public static RedCardManagmentListener Instance { get; set; } = new RedCardManagmentListener();

        /// <summary>
        /// 订阅不合格审核流程完成信息
        /// </summary>
        public void Start()
        {
            //流程实例撤销事件
            RT.EventBus.Subscribe<CancelFlowInstanceEvent>(this, e =>
            {
                if (e.SourceType == typeof(RedCardApplyBill).FullName)
                    RT.Service.Resolve<RedCardApplyBillWorkflowController>().CancelFlowInstanceEventHandle(e);
            });
        }
    }
}
