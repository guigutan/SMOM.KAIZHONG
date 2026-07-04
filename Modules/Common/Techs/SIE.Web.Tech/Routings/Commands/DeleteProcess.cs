using SIE.Domain;
using SIE.Security;
using SIE.Tech.Processs;
using SIE.Web.Command;
using System;

namespace SIE.Web.Tech.Routings.Commands
{
    /// <summary>
    /// 删除工艺路线命令
    /// </summary>
    [JsCommand("SIE.Web.Tech.Routings.Commands.DeleteProcess")]
    [AllowAnonymous]
    public class DeleteProcess : ViewCommand
    {
        /// <summary>
        /// 删除工艺路线命令执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var processId = Convert.ToDouble(args.Data);
            var process = RF.GetById<Process>(processId);
            if (process == null) return false;
            process.PersistenceStatus = PersistenceStatus.Deleted;
            RF.Save(process);
            return true;
        }
    }
}