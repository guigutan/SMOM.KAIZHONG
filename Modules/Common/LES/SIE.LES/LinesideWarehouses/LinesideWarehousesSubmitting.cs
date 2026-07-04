using SIE.Domain;
using SIE.Domain.Validation;
using SIE.LES.StockOrders;
using System;
using System.ComponentModel;

namespace SIE.LES.LinesideWarehouses
{


    /// <summary>
    /// 周转规则明细行号不重复
    /// </summary>
    [DisplayName("同一个资源下只能关联一个仓库")]
    [Description("同一个资源下只能关联一个仓库")]
    public class LinesideWarehousesSubmitting : OnSubmitting<LinesideWarehouse>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Invoke(LinesideWarehouse entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
            {
                var isExsitedEntity = RT.Service.Resolve<LinesideWarehouseController>().GetLinesideWarehouseByResource(entity.Id, entity.WipResouceId, entity.WorkShopId);
                if (isExsitedEntity > 0)
                {
                    throw new ValidationException("同一资源+车间下只能关联一个仓库!".L10N());
                }

            }
            else if (e.Action == SubmitAction.Delete)
            {
                StockOrderMergeIssued issued = RT.Service.Resolve<StockOrderMergeIssuedController>().GetStockOrderMergeIssued(entity.Id);
                if (issued != null)
                {
                    throw new ValidationException("已被备料单合并下发规则引用，不能删除!".L10N());
                }
            }
        }
    }
}
