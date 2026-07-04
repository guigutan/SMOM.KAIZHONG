using MimeKit;
using SIE.Common;
using SIE.Common.Alert;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Kit.MES.CallMaterials.Alerts
{
    /// <summary>
    /// 叫料单配送超时预警插件类
    /// </summary>
    [RootEntity, Serializable]
    [Alert("叫料单配送超时预警插件", typeof(DeliveryDelayAlertConfig), typeof(DeliveryDelayAlertResult), "叫料单配送超时预警插件")]
    public class DeliveryDelayAlert : CallMaterialAlert
    {
        /// <summary>
        /// 执行预警逻辑
        /// </summary>
        /// <returns>预警参数</returns>
        public override AlertResultBase Run()
        {
            var deliveryDelayAlertResults = GetAlertResults();
            return deliveryDelayAlertResults;
        }

        /// <summary>
        /// 获取叫料单配送超时预警参数集合
        /// </summary>
        /// <returns>预警参数</returns>
        public AlertResultListBase GetAlertResults()
        {
            double lineId = (this.Context.Config as DeliveryDelayAlertConfig).LineId;
            var deliveryDelayAlertResults = new DeliveryDelayAlertResultList();
            ////var curSeverity= RT.Service.Resolve<AlertController>().SeverityOperation;
            var callMaterialBills = RT.Service.Resolve<CallMaterialController>().GetDeliveryCallMaterialBills(lineId);
            ProcessCallMaterialBillStatus(callMaterialBills);
            foreach (var billItem in callMaterialBills)
            {
                if ((billItem.CallWorkOrder?.WorkOrder.IsPause == YesNo.No)
                    && (billItem.CallWorkOrder?.WorkOrder.State == WorkOrderState.Producing
                    || billItem.CallWorkOrder?.WorkOrder.State == WorkOrderState.Release))
                {
                    var curDeliveryDelayAlertResult = new DeliveryDelayAlertResult();
                    var curRequiredTime = billItem.RequiredTime;
                    var curSendingTime = billItem.SendingTime;
                    if (curSendingTime != null)
                    {
                        curDeliveryDelayAlertResult.Value = (decimal)(curSendingTime.Value - curRequiredTime).TotalHours;
                    }
                    else
                    {
                        curDeliveryDelayAlertResult.Value = (decimal)(curDeliveryDelayAlertResult.AlertTime - curRequiredTime).TotalHours;
                    }

                    curDeliveryDelayAlertResult.CallMaterialBillNo = billItem.No;
                    curDeliveryDelayAlertResult.Line = RF.GetById<WipResource>(lineId)?.Name;
                    curDeliveryDelayAlertResult.WorkOrderNO = billItem.CallWorkOrder?.WorkOrder?.No;
                    curDeliveryDelayAlertResult.StationCode = billItem.Station?.Code;
                    curDeliveryDelayAlertResult.MaterialAlerts.AddRange(CreatAlertMaterials(billItem));
                    deliveryDelayAlertResults.ResultList.Add(curDeliveryDelayAlertResult);
                }
            }

            return deliveryDelayAlertResults;
        }

        /// <summary>
        /// 处理叫料单的状态
        /// 待处理、当前时间>需求时间的叫料单
        /// </summary>
        /// <param name="callMaterialBills">叫料单集合</param>
        /// <returns>true: 更新成功; false: 更新失败</returns>
        private void ProcessCallMaterialBillStatus(EntityList<CallMaterialBill> callMaterialBills)
        {
            try
            {
                var nowDate = RF.Find<CallMaterialBill>().GetDbTime();
                var curPendingCMBills = callMaterialBills.Where(x => x.Status == CallMaterialStatus.Pending && x.RequiredTime < nowDate).AsEntityList();
                foreach (var cmbill in curPendingCMBills)
                {
                    cmbill.Status = CallMaterialStatus.Timeout;
                }

                RF.Save(curPendingCMBills);
            }
            catch (Exception exMsg)
            {
                throw new ValidationException("配送超时预警处理叫料单状态失败: {0}".L10nFormat(exMsg.Message));
            }
        }

        /// <summary>
        /// 创建叫料单预警的物料信息
        /// </summary>
        /// <param name="bill">叫料单</param>
        /// <returns>预警物料集合</returns>
        private List<MaterialAlert> CreatAlertMaterials(CallMaterialBill bill)
        {
            var alertMaterials = new List<MaterialAlert>();
            foreach (var billDetail in bill.DetailList)
            {
                var curAlertMaterial = new MaterialAlert(billDetail.Item.Code, billDetail.Item.Name);
                alertMaterials.Add(curAlertMaterial);
            }

            return alertMaterials;
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
