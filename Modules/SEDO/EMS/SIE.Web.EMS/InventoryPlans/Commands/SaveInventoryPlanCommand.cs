using SIE.EMS.InventoryPlans;
using SIE.Web.Command;

namespace SIE.Web.EMS.InventoryPlans.Commands
{
    /// <summary>
    /// 保存盘点计划
    /// </summary>
    [JsCommand("SIE.Web.EMS.InventoryPlans.Commands.SaveInventoryPlanCommand")]
    public class SaveInventoryPlanCommand : FormSaveCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var list = GetDeserializeData(args, scope);
            var entity = list.Count > 0 ? list[0] : null;
            if (entity != null)
            {
                var plan = entity as InventoryPlan;
                if (plan.PersistenceStatus == Domain.PersistenceStatus.New)
                {
                    plan.IsSave = true;
                }
                RT.Service.Resolve<InventoryPlanController>().SaveInventoryPlan(plan);
            }
            return entity;
        }
    }
}
