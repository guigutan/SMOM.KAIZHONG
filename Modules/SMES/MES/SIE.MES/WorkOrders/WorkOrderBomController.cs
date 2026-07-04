using SIE.Domain;
using SIE.MES.PanelBindings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工单BOM业务控制器
    /// </summary>
    public class WorkOrderBomController : DomainController
    {
        /// <summary>
        /// 获取工单BOM排除部份物料
        /// </summary>
        /// <param name="woId">工单</param>
        /// <param name="exceptionalItemIds"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrderBom> GetWorkOrderBoms(double woId, List<double> exceptionalItemIds)
        {
            var query = Query<WorkOrderBom>().Where(x => x.WorkOrderId == woId)
                .Where(x => !exceptionalItemIds.Contains(x.ItemId))
                .Where(x => x.IsRecoilItem && !x.IsAlternative);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工单BOM的替代料
        /// </summary>
        /// <param name="workOrderBomIds">工单BOM ID列表</param>
        /// <returns>工单BOM的替代料列表</returns>
        public virtual EntityList<WorkOrderBom> GetWorkOrderBomAlternatives(List<double> workOrderBomIds)
        {
            return workOrderBomIds.SplitContains(tempIds =>
            {
                return Query<WorkOrderBom>().As("x")
                     .Join<WorkOrderBom>("y", (x, y) => x.Alter == y.Alter)
                     .Where<WorkOrderBom>((x, y) => tempIds.Contains(y.Id) && x.IsAlternative)
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取工单BOM
        /// </summary>
        /// <param name="workOrderBomIds">工单BOM ID列表</param>
        /// <returns>工单BOM列表</returns>
        public virtual EntityList<WorkOrderBom> GetWorkOrderBoms(List<double> workOrderBomIds)
        {
            return workOrderBomIds.SplitContains(tempIds =>
            {
                return Query<WorkOrderBom>()
                    .Where(x => tempIds.Contains(x.Id))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 根据工单id获取工单bom
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrderBom> GetWorkOrderBomByOrderId(PagingInfo pagingInfo,double workOrderId)
        {
            return Query<WorkOrderBom>().Where(p => p.WorkOrderId == workOrderId).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询 当前工单 和物料是否则工序BOM里面
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public virtual bool GetWorkOrderBom(double workOrderId,double itemId)
        {
          var workOrderBom=  Query<WorkOrderBom>().Where(p => p.WorkOrderId == workOrderId&&p.ItemId== itemId).ToList();
            if (workOrderBom.Count > 0)
            {
                return false;
            }
            return true;
        }
    }
}
