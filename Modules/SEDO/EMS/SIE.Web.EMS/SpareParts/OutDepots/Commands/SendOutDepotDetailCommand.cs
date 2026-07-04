using SIE.EMS.SpareParts.OutDepots.Controllers;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.EMS.SpareParts.OutDepots.Commands
{
    /// <summary>
    /// 出库单发货命令
    /// </summary>
    public class SendOutDepotDetailCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 出库单发货操作
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var list = args.Data.ToJsonObject<List<PartOutDepotDetail>>();
            RT.Service.Resolve<OutDepotController>().SendPartOutDepotDetails(list);
            return true;
        }
    }
}
