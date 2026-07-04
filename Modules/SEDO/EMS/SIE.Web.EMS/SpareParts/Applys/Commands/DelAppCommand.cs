using SIE.EMS.SpareParts.Applys.Controllers;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.EMS.SpareParts.Applys.Commands
{
    /// <summary>
    /// 删除
    /// </summary>
    [JsCommand("SIE.Web.EMS.SpareParts.Applys.Commands.DelAppCommand")]
    public class DelAppCommand : ViewCommand
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

            int count = RT.Service.Resolve<SparePartAppController>().DeleteApp(ids);
            return count;
        }
    }
}
