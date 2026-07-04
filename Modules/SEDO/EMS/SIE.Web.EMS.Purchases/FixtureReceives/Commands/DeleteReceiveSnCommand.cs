using SIE.EMS.Purchases.FixtureReceives;
using SIE.Equipments.Enums;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.FixtureReceives.Commands
{
    /// <summary>
    /// 删除接收序列号
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.FixtureReceives.Commands.DeleteReceiveSnCommand")]
    public class DeleteReceiveSnCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args == null)
            {
                return false;
            }
            return true;
        }
    }
}
