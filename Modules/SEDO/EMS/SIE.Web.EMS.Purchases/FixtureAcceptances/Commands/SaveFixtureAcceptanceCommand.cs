using SIE.Domain;
using SIE.EMS.Purchases.FixtureAcceptances;
using SIE.Web.Command;

namespace SIE.Web.EMS.Purchases.FixtureAcceptances.Commands
{
    /// <summary>
    /// 保存备件验收
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.FixtureAcceptances.Commands.SaveFixtureAcceptanceCommand")]
    public class SaveFixtureAcceptanceCommand : ViewCommand
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
            var accepts = list as EntityList<FixtureAcceptance>;
            RT.Service.Resolve<FixtureAcceptancesController>().SaveFixtureAcceptance(accepts);
            return true;
        }
    }
}
