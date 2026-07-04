using SIE.EMS.SpareParts;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.SpareParts.Commands
{
    /// <summary>
    /// 整单交接命令
    /// </summary>
    public class WholeBillStoreCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 整单交接操作
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var entity = args.Data.ToJsonObject<SparePartStore>();
            RT.Service.Resolve<SparePartController>().WholeBillInStorage(entity);
            return true;
        }
    }
}
