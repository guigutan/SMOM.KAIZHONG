using SIE.Domain;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工单控制器
    /// </summary>
    public partial class WorkOrderController:DomainController
    {
        /// <summary>
        /// 根据MRB获取工单数据
        /// </summary>
        /// <param name="mrbs"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrder> GetWorkOrdersByMRB(List<string> mrbs)
        {
            var list = mrbs.SplitContains(temp =>
            {
                var query = Query<WorkOrder>().LeftJoin<Enterprise>((w, e) => w.WorkShopId == e.Id)
                .Where<Enterprise>((w, e) => temp.Contains(e.Name) || temp.Contains(e.Code))
                .Where(p => p.State == Core.WorkOrders.WorkOrderState.Release || p.State == Core.WorkOrders.WorkOrderState.Producing);
                return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

    }
}
