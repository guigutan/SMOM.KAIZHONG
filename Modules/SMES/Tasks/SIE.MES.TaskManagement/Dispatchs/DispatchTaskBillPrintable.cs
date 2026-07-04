using SIE.Common.Prints;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SIE.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 任务单打印
    /// </summary>
    [Serializable]
    [DisplayName("任务单打印")]
    public class DispatchTaskBillPrintable : BillPrintable<DispatchTask>
    {
        /// <summary>
        /// 根据实体类型获取属性
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <returns>对应type的属性</returns>
        public override IEnumerable<string> GetPropertys(Type type = null)
        {
            var propertys = base.GetPropertys(type).ToList();
            if (type == typeof(DispatchTask))
            {
                propertys.Add("Task_Priority");
                propertys.Add("WorkOrder_State");
                propertys.Add("Product_Code");
                propertys.Add("Product_Name");
                propertys.Add("Process_Name");
                propertys.Add("Specification_Code");
                propertys.Add("Specification_Name");
                propertys.Add("WorkShop_Name");
                propertys.Add("Resource_Name");
                propertys.Add("Process_Code");
                propertys.Add("Short_Description");
            }

            return propertys;
        }

        /// <summary>
        /// 转换数据
        /// </summary>
        /// <param name="data">实体对象</param>
        /// <returns>转换后的数据</returns>
        public override string ConverterData(object data)
        {
            var content = base.ConverterData(data);
            if (data is DispatchTask)
            {
                var task = data as DispatchTask;
                if (task != null)
                {
                    string WorkOrder_State = string.Empty;
                    var state = task.WorkOrder?.State;
                    if (task.IsPause == YesNo.Yes && (state == SIE.Core.WorkOrders.WorkOrderState.Release || state == SIE.Core.WorkOrders.WorkOrderState.Producing))
                        WorkOrder_State = state.ToLabel() + "暂停";
                    else
                        WorkOrder_State = state.ToLabel();

                    content += EnumViewModel.EnumToLabel(task.Priority).L10N() + Separator + WorkOrder_State + Separator
                          + task.Product?.Code + Separator + task.Product?.Name + Separator + task.Process?.Name + Separator + task.Specification?.Code + Separator + task.Specification?.Name + Separator + task.WorkShop?.Name + Separator + task.Resource?.Name + Separator + task.Process?.Code + Separator + task.Product?.ShortDescription + Separator;
                }
            }

            return content;
        }
    }
}
