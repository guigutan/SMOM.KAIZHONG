using SIE.Domain;
using SIE.MES.InspectionStandards;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WIP.Inspects
{
    /// <summary>
    /// 检验采集控制器
    /// </summary>
    public partial class InspectController : WipController
    {
        /// <summary>
        /// 根据机型和工序获取检验项目
        /// </summary>
        /// <returns>机型检验项目列表</returns>
        public virtual EntityList<ModelInspectionItem> GetInspectionItems()
        {
            var q = Query<ModelInspectionItem>();
            return q.ToList();
        }

        /// <summary>
        /// 获取工单
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="eagerLoad">贪婪加载选项</param>
        /// <returns>工单</returns>
        public virtual WorkOrder GetWorkOrder(double workOrderId, EagerLoadOptions eagerLoad)
        {
            var q = Query<WorkOrder>();
            return q.Where(w => w.Id == workOrderId).ToList(null, eagerLoad).FirstOrDefault();
        }
    }
}
