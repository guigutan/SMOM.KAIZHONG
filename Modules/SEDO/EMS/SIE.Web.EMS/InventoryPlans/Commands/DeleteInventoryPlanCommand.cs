using SIE.EMS.InventoryPlans;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.InventoryPlans.Commands
{
    /// <summary>
    /// 删除盘点计划
    /// </summary>
    [JsCommand("SIE.Web.EMS.InventoryPlans.Commands.DeleteInventoryPlanCommand")]
    public class DeleteInventoryPlanCommand : DeleteCommand
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
            var ids = new List<double>();
            list.DeletedList.ForEach(p =>
            {
                var model = p as InventoryPlan;
                if (model != null)
                {
                    ids.Add(model.Id);
                }
            });
            RT.Service.Resolve<InventoryPlanController>().DeleteInventoryPlan(ids);
            return true;
        }
    }
}
