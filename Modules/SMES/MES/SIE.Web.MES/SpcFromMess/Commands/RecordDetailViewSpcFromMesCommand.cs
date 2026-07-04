using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.SpcFromMess.Commands
{
    /// <summary>
    ///记录测量数据 命令
    /// </summary>
    [JsCommand("SIE.Web.MES.SpcFromMess.Commands.RecordDetailViewSpcFromMesCommand")]
    public class RecordDetailViewSpcFromMesCommand : ViewCommand
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
