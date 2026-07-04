using SIE.Core.Enums;
using SIE.Inventory.Transactions;


namespace SIE.Web.Inventory.Task.DataQueryer
{
    /// <summary>
    /// 任务管理数据查询器
    /// </summary>
    public class TaskManagementDataQueryer : Data.DataQueryer
    {
        /// <summary>
        /// 获取配置的模板的值
        /// </summary>
        /// <returns>模板数据</returns>
        public TaskManagementTempleteData GetTaskManagementNumberRule(OrderType orderType)
        {
            var fun = RT.Service.Resolve<TransactionController>().GetFunctionByType(orderType);
            TaskManagementTempleteData rst = new TaskManagementTempleteData()
            {
                PrintBillRuleId = fun?.BillTemplateId,
                PrintBillRuleName = fun?.BillTemplate?.FileName,
            };
            return rst;
        }
    }

    /// <summary>
    /// 任务管理单配置模板
    /// </summary>
    public class TaskManagementTempleteData
    {
        /// <summary>
        /// 单据模板
        /// </summary>
        public double? PrintBillRuleId { get; set; }

        /// <summary>
        /// 单据模板
        /// </summary>
        public string PrintBillRuleName { get; set; }
    }
}
