using SIE.Items;
using System;
using System.ComponentModel;

namespace SIE.MES.WorkOrders.Events
{
    /// <summary>
    /// ERP接口创建工单属性变更
    /// </summary>
    public class ErpWorkOrderPropertyChanged : WorkOrderPropertyChanged
    {
        /// <summary>
        /// 工单属性变更事件
        /// </summary>
        /// <param name="sender">工单</param>
        /// <param name="e">参数</param>
        public override void WorkOrderOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var workOrder = sender as WorkOrder;
            if (workOrder == null)
                return;
            SetMesWorkOrder(workOrder, e);
        }

        /// <summary>
        /// 生成工单bom清单
        /// ERP接口存在bom则不自动创建
        /// </summary>        
        /// <param name="workOrder">工单</param>
        public override void GenerateWorkOrderBoms(WorkOrder workOrder)
        {
            if (workOrder is null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            if (workOrder.LocalContext.GetPropertyOrDefault<bool>("IsExistBom"))
            {
                return;
            }

            base.GenerateWorkOrderBoms(workOrder);
        }

        /// <summary>
        /// 计划数量变更
        /// </summary>
        /// <param name="workOrder">工单</param>  
        public override void PlanQtyChanged(WorkOrder workOrder)
        {
            if (workOrder.LocalContext.GetPropertyOrDefault<bool>("IsExistBom"))
                return;
            base.PlanQtyChanged(workOrder);
        }
    }
}