using SIE.Security;
using SIE.Tech.Routings;
using SIE.Tech.Routings.Technologys;
using SIE.Tech.Routings.ViewModels;
using SIE.Web.Command;

namespace SIE.Web.Tech.Routings.Commands
{
    /// <summary>
    /// 添加工艺路线版本命令
    /// </summary>
    [JsCommand("SIE.Web.Tech.Routings.Commands.AddRoutingVersion")]
    [AllowAnonymous]
    public class AddRoutingVersion : ViewCommand<int>
    {
        /// <summary>
        /// 添加工艺路线版本命令执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(int args, string scope)
        {
            return true;
        }
    }
}