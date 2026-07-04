using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.PrepareProducts.Commands
{
    /// <summary>
    /// 产品产前准备设置导出命令
    /// </summary>
    public class PrepareProductExportCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
