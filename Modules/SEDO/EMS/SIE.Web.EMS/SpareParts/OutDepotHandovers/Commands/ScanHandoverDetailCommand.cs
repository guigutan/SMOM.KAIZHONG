using SIE.EMS.SpareParts.OutDepotHandovers;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.SpareParts.OutDepotHandovers.Commands
{
    /// <summary>
    /// 交接单扫码接收命令
    /// </summary>
    public class ScanHandoverDetailCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 交接单扫码接收操作
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var list = args.Data.ToJsonObject<List<OutDepotHandoverDetail>>();
            RT.Service.Resolve<OutDepotHandoverController>().ScanOutHandoverDetail(list);
            return true;
        }
    }
}
