using SIE.EMS.Purchases.EquipmentSetups;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.EquipmentSetups.Commands
{
    /// <summary>
    /// 提交
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.EquipmentSetups.Commands.SubmitEquipmentSetupCommand")]
    public class SubmitEquipmentSetupCommand : ViewCommand
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
            RT.Service.Resolve<EquipmentSetupController>().SubmitEquipmentSetup(selectedIds);
            return true;
        }
    }
}
