using SIE.Domain;
using SIE.Items;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SIE.EMS.SpareParts;

namespace SIE.Web.EMS.SpareParts.Commands
{
    /// <summary>
    /// 同步备件物料命令
    /// </summary>
    public class ImportSparePartItemCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            RT.Service.Resolve<SparePartController>().ImportSparePartItems();
            return true;
        }
    }
}
