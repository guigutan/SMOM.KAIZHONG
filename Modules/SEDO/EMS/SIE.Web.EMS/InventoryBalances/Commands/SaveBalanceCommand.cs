using SIE.Domain;
using SIE.EMS.InventoryBalances;
using SIE.Web.Command;

namespace SIE.Web.EMS.InventoryBalances.Commands
{
    /// <summary>
    /// 保存盘点平账
    /// </summary>
    [JsCommand("SIE.Web.EMS.InventoryBalances.Commands.SaveBalanceCommand")]
    public class SaveBalanceCommand : SaveCommand
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
            EntityList<InventoryBalance> balanceList = list as EntityList<InventoryBalance>;
            RT.Service.Resolve<InventoryBalanceController>().SaveInventoryBalanceList(balanceList);
            return list;
        }
    }
}
