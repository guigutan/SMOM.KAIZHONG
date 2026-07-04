using SIE.EMS.Purchases.EquipmentInbounds;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.EquipmentInbounds.Commands
{
    /// <summary>
    /// 入库
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.EquipmentInbounds.Commands.WarehousingCommand")]
    public class WarehousingCommand : ViewCommand
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
            RT.Service.Resolve<EquipmentInboundController>().Warehousing(selectedIds);
            return true;
        }
    }
}
