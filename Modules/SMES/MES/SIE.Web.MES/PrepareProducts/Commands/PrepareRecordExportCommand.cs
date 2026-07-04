using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.PrepareProducts.Commands
{
    /// <summary>
    /// 产前准备记录子表导出命令
    /// </summary>
    public class PrepareRecordExportCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
