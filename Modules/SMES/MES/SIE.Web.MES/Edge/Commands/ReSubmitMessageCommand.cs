using SIE.EventMessages.MES.WIP;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.MES.Edge.Commands
{
    /// <summary>
    /// 重新提交失败消息命令
    /// </summary>
    public class ReSubmitMessageCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var successCount = RT.Service.Resolve<IMessageService>().ReSubmitErrorMessage(args.SelectedIds);
            return successCount;
        }
    }
}
