using MimeKit;
using SIE.Common.Alert;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Resources.WipResources;
using System;

namespace SIE.Kit.MES.CallMaterials.Alerts
{
    /// <summary>
    /// 工单叫料发送WMS失败预警插件类
    /// </summary>
    [RootEntity, Serializable]
    [Alert("工单叫料发送WMS失败预警插件", typeof(SendWMSFailAlertConfig), typeof(SendWMSFailAlertResult), "工单叫料发送WMS失败预警插件")]
    public class SendWMSFailAlert : CallMaterialAlert
    {
        /// <summary>
        /// 执行预警逻辑
        /// </summary>
        /// <returns>预警参数</returns>
        public override AlertResultBase Run()
        {
            var sendWMSFailAlertResults = GetAlertResults();
            return sendWMSFailAlertResults;
        }

        /// <summary>
        /// 获取工单叫料发送WMS失败预警参数集合
        /// </summary>
        /// <returns>预警参数</returns>
        public AlertResultListBase GetAlertResults()
        {
            double lineId = (this.Context.Config as SendWMSFailAlertConfig).LineId;
            var sendWMSFailAlertResults = new SendWMSFailAlertResultList();
            var callMaterialWorkOrders = RT.Service.Resolve<CallMaterialController>().GetCallMaterialWorkOrders(lineId);
            foreach (var callMtrlWOItem in callMaterialWorkOrders)
            {
                if (callMtrlWOItem.WorkOrder.IsPause == YesNo.No &&
                    (callMtrlWOItem.WorkOrder.State == WorkOrderState.Producing
                    || callMtrlWOItem.WorkOrder.State == WorkOrderState.Release))
                {
                    var curSendWMSFailAlertResult = new SendWMSFailAlertResult();
                    curSendWMSFailAlertResult.Line = RF.GetById<WipResource>(lineId)?.Name;
                    curSendWMSFailAlertResult.WorkOrderNO = callMtrlWOItem.WorkOrder?.No;
                    curSendWMSFailAlertResult.FailReason = callMtrlWOItem.FailReason;
                    curSendWMSFailAlertResult.SendingHours = callMtrlWOItem.WorkOrder?.Product.Model?.SendingHours;
                    curSendWMSFailAlertResult.Value = (decimal)(curSendWMSFailAlertResult.AlertTime - callMtrlWOItem.UpdateDate).TotalHours;
                    sendWMSFailAlertResults.ResultList.Add(curSendWMSFailAlertResult);
                }
            }

            return sendWMSFailAlertResults;
        }

        /// <summary>
        /// 预警结果处理
        /// </summary>
        /// <param name="alretResult">预警参数</param>
        /// <returns>bool</returns>
        public override bool AlertResultProcess(AlertResultBase alretResult)
        {
            return base.AlertResultProcess(alretResult);
        }

        /// <summary>
        /// 插件邮件附件
        /// </summary>
        /// <param name="result">预警参数</param>
        /// <returns>邮件附件</returns>
        public override AttachmentCollection CreateEmailAttachments(AlertResultBase result)
        {
            return base.CreateEmailAttachments(result);
        }
    }
}
