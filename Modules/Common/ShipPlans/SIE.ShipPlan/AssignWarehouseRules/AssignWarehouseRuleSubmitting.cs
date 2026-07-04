using SIE.Domain;
using SIE.Domain.Validation;
using System;

namespace SIE.ShipPlan
{
    /// <summary>
    /// 新增时
    /// </summary>
    [System.ComponentModel.DisplayName("分配仓库规则保存前事件")]
    [System.ComponentModel.Description("工单发料类型唯一数据验证")]
    public class AssignWarehouseRuleSubmitting : OnSubmitting<AssignWarehouseRule>
    {
        /// <summary>
        /// 分配仓库规则保存前事件
        /// </summary>
        /// <param name="entity">分配仓库规则</param>
        /// <param name="e">实体提交参数</param>
        protected override void Invoke(AssignWarehouseRule entity, EntitySubmittingEventArgs e)
        {
            if (e != null)
            {
                if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
                {
                    if (entity.OrderType == Core.Enums.OrderType.AutoJoinLineWarehouse && entity.ItemCategory == null)
                    {
                        throw new ValidationException("单据类型为自动入库线边仓时,库存分类必填".L10N());
                    }
                }

                if (entity != null && (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
                    && RT.Service.Resolve<AssignWarehouseRuleController>().GetSameRule(entity.OrderType, entity.Id, entity.ResourceId, entity.ItemType, entity.ItemCategoryId,
                        entity.CustomerId, entity.SupplierId, entity.EnterpriseId))
                {
                    //获取相同的记录
                    //工单发料需要验证资源
                    if (entity.OrderType == Core.Enums.OrderType.WorkFeed)
                    {
                        throw new ValidationException("工单发料不能存在单据类型、基本分类、库存类别、客户、供应商、部门、资源一样的数据".L10N());
                    }
                    else
                    {//其他不需要验证资源
                        throw new ValidationException("不能存在单据类型、基本分类、库存类别、客户、供应商、部门一样的数据".L10N());
                    }
                }
            }
        }
    }

}
