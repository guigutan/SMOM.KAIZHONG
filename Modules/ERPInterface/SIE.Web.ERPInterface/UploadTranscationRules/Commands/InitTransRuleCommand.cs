using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Enums;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.ERPInterface.UploadTransactionRules.Commands
{
    /// <summary>
    /// 初始化交易上传规则命令
    /// </summary>
    public class InitTransRuleCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            RT.Service.Resolve<UploadBaseController>().InitTransRule();

            return true;
        }
    }
}


