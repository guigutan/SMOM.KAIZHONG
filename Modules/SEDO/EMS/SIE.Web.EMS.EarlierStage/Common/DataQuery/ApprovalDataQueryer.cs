using SIE.EMS.EarlierStage.Budgets;
using SIE.EMS.EarlierStage.Common.Controller;
using SIE.EMS.EarlierStage.Projects;
using SIE.Web.Data;

namespace SIE.Web.EMS.EarlierStage.Common.DataQuery
{
    /// <summary>
    /// 预算查询器
    /// </summary>
    public class ApprovalDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取是否启用审批流程
        /// </summary>
        /// <param name="typeQty">界面类型（0：预算，1：预算变更，2：项目，3：项目变更，4：项目结项）</param>
        /// <returns>是否启用审批流程</returns>
        public bool GetEnableApproval(int typeQty)
        {
            var type = typeof(Budget);
            switch (typeQty)
            {
                case 1:
                    type = typeof(BudgetChange);
                    break;
                case 2:
                    type = typeof(Project);
                    break;
                case 3:
                    type = typeof(ProjectChange);
                    break;
                case 4:
                    type = typeof(ProjectClose);
                    break;
                default:
                    break;
            }
            //是否启用审批
            return RT.Service.Resolve<EarlierStageApprovalController>().GetApprovalConfigValue(type).EnableAudit;
        }
    }
}
