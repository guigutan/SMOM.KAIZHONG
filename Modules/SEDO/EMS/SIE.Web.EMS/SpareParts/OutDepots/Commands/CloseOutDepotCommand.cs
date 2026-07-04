using SIE.EMS.SpareParts.OutDepots;
using SIE.EMS.SpareParts.OutDepots.Controllers;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.SpareParts.OutDepots.Commands
{
    /// <summary>
    /// 出库单关单命令
    /// </summary>
    public class CloseOutDepotCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 出库单关单操作
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var entity = args.Data.ToJsonObject<OutDepot>();
            RT.Service.Resolve<OutDepotController>().CloseOutDepotBill(entity);
            return true;
        }
    }
}
