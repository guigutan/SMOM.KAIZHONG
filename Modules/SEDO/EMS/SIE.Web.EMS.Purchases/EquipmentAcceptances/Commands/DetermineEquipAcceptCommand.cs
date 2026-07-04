using SIE.EMS.Purchases.EquipmentAcceptances;
using SIE.Web.Command;

namespace SIE.Web.EMS.Purchases.EquipmentAcceptances.Commands
{
    /// <summary>
    /// 验收确定
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.EquipmentAcceptances.Commands.DetermineEquipAcceptCommand")]
    public class DetermineEquipAcceptCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args == null)
            {
                return false;
            }
            var acceptId = args.Data.ToJsonObject<double>();
            RT.Service.Resolve<EquipmentAcceptanceController>().DetermineEquipAccept(acceptId);
            return true;
        }
    }
}
