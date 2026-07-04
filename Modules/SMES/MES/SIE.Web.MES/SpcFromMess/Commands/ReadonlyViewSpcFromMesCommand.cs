using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.SpcFromMess.Commands
{
    /// <summary>
    ///  查看预览命令（仅前端逻辑）
    /// </summary>
    [JsCommand("SIE.Web.MES.SpcFromMess.Commands.ReadonlyViewSpcFromMesCommand")]
    public class ReadonlyViewSpcFromMesCommand : ViewCommand
    {
        /// <summary>
        /// Excute
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
