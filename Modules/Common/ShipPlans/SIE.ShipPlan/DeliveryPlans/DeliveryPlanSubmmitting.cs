using SIE.Domain;
using System;

namespace SIE.ShipPlan.DeliveryPlans
{

    /// <summary>
    /// 发货计划保存
    /// </summary>
    [System.ComponentModel.DisplayName("发货计划保存前事件")]
    [System.ComponentModel.Description("发货计划保存前事件")]
    public class DeliveryPlanSubmmitting : OnSubmitting<DeliveryPlan>
    {
        /// <summary>
        /// 保存ASN明细后执行
        /// </summary>
        /// <param name="entity">ASN明细实体</param>
        /// <param name="e">实体提交参数</param>
        protected override void Invoke(DeliveryPlan entity, EntitySubmittingEventArgs e)
        {
            if (e != null && entity != null)
            {
                if (e.Action == SubmitAction.Update && entity.SourceType == DeliverySourceType.Erp)
                {
                    var dbEntity = RF.GetById<DeliveryPlan>(entity.Id);
                    if (dbEntity.DeliveryQty < entity.DeliveryQty)
                    {
                        DeliveryPlanToErpDtl deliveryPlanToErpDtl = new DeliveryPlanToErpDtl()
                        {
                            DeliveryPlanId = entity.Id,
                            ErpDetailId = entity.ErpDetailId,
                            ShippingDate = entity.DeliveryDate ?? DateTime.Now,
                            ShippingQty = entity.DeliveryQty - dbEntity.DeliveryQty,
                            ErpOrderId = entity.ErpOrderId.Value,
                        };
                        RF.Save(deliveryPlanToErpDtl);
                    }
                }
            }
        }

    }
}
