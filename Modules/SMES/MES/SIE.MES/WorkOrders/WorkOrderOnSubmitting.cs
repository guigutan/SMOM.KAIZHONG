using IronPython.Runtime.Operations;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.ComponentModel;
using System.Linq;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工单提交前事件
    /// </summary>
    [DisplayName("工单提交前事件")]
    [Description("工艺路线版本切换，已上线条码生成产品工艺路线")]
    public class WorkOrderOnSubmitting : OnSubmitting<WorkOrder>
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="entity">工单</param>
        /// <param name="e">参数</param>
        protected override void Invoke(WorkOrder entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Update)
            {
                using (DataAuth.DataAuths.LoadAll())
                {
                    //工单工艺路线版本发生修改，
                    var oldWorkOrder = RF.GetById<WorkOrder>(entity.Id);
                    if (oldWorkOrder.VersionId != entity.VersionId)
                    {
                        RT.Service.Resolve<WorkOrderController>().CreateProductRouting(entity.Id, oldWorkOrder.Layout?.Layout);
                    }
                }
            }
            if (e.Action == SubmitAction.Update || e.Action == SubmitAction.Insert)
            {

                if (entity.PlanEndDate < entity.PlanBeginDate)
                {
                    throw new ValidationException("计划开始时间不能早于计划结束时间!".L10N());
                }

                if (entity.WorkOrderOutputProductList.Any())
                {

                    foreach (var workOrderOutputProduct in entity.WorkOrderOutputProductList)
                    {
                        if (workOrderOutputProduct.Item == null)
                        {
                            throw new ValidationException("物料编码不能为空，请填写".L10N());
                        }
                        if (workOrderOutputProduct.Item.EnableExtendProperty && workOrderOutputProduct.ItemExtProp.IsNullOrEmpty())
                        {
                            throw new ValidationException("启用物料扩展属性的物料，物料扩展属性必填".L10N());
                        }
                    }
                }
            }

        }
    }
}