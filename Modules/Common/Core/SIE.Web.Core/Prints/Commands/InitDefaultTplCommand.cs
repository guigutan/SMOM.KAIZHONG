using SIE.Core.Prints;
using SIE.Domain;
using SIE.Web.Command;
using System;

namespace SIE.Web.Core.Prints.Commands
{
    /// <summary>
    /// PDA标签补打
    /// </summary>
    public class InitDefaultTplCommand : ViewCommand
    {
        /// <summary>
        /// 命令执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            //var logId = Convert.ToDouble(args.Data);
            RT.Service.Resolve<PrinterSettingController>().InitDefaultTpl();

            return true;
        }
    }
}