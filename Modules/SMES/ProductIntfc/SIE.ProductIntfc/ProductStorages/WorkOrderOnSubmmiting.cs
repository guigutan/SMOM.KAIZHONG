using SIE.Domain;
using SIE.MES.WorkOrders;

namespace SIE.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 工单提交事件
    /// </summary>
    public class WorkOrderOnSubmmiting : OnSubmitting<WorkOrder>
    {
        /// <summary>
        /// 工单提交事件
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">e</param>
        protected override void Invoke(WorkOrder entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Update && entity.PackageRuleDetailList.IsDirty)
            {
                //注释原因：顾问那边在定:修改入库层级只能通过选择新包装来完成，后面根据最终方案要改动
                //var oldWo = RF.GetById<WorkOrder>(entity.Id);
                //var oldRule = oldWo.PackageRuleDetailList.Where(p => p.IsInStockLabel).FirstOrDefault();
                //var newRule = entity.PackageRuleDetailList.Where(p => p.IsInStockLabel).FirstOrDefault();
                ////取高层级包装转低层级包装
                //if (oldRule == null || oldRule.PackageUnit.IsMasterUnit || newRule != null && oldRule.Id == newRule.Id )
                //    return;
                //RT.Service.Resolve<ProductStorageController>().AddLessBarCode(entity.Id, oldRule, entity.ProductId, newRule);
            }
        }
    }
}
