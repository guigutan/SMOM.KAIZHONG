using SIE.EMS.Purchases.EquipmentSetups;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.EquipmentSetups.Commands
{
    /// <summary>
    /// 转派
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.EquipmentSetups.Commands.ReassignmentCommand")]
    public class ReassignmentCommand : ViewCommand
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
            List<double> selectedIds = new List<double>(args.SelectedIds);
            var principalId = args.Data.ToJsonObject<double>();
            RT.Service.Resolve<EquipmentSetupController>().Reassignment(selectedIds, principalId);
            return true;
        }
    }
}
