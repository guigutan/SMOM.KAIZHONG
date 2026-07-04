using SIE.EMS.InventoryBalances;
using SIE.EMS.InventoryTasks;
using SIE.Equipments.Configs;
using SIE.Web.Data;

namespace SIE.Web.EMS.InventoryBalances
{
    /// <summary>
    /// 盘点平账查询器
    /// </summary>
    public class InventoryBalanceDataQueryer : DataQueryer
    {
        ///<summary>
        /// 获取审批流程配置
        /// </summary>
        /// <returns>审批流程配置</returns>
        public ApprovalConfigValue GetInventoryBalanceApproval()
        {
            return RT.Service.Resolve<InventoryBalanceController>().GetApprovalConfigValue();
        }

        /// <summary>
        /// 修改原因分析
        /// </summary>
        /// <param name="cause">原因分析</param>
        public void EditInventoryCause(InventoryCause cause)
        {
            RT.Service.Resolve<InventoryBalanceController>().EditInventoryCause(cause);
        }
    }
}
