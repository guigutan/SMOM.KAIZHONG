using SIE.EMS.SpareParts.OutDepots.Controllers;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.EMS.SpareParts.OutDepots.Commands
{
    /// <summary>
    /// 删除
    /// </summary>
    [JsCommand("SIE.Web.EMS.SpareParts.OutDepots.Commands.DelSuppCommand")]
    public class DelSuppCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> ids = args.Data.ToJsonObject<List<double>>();
            int count = RT.Service.Resolve<OutDepotController>().DeleteSupp(ids);
            return count;
        }
    }
}
