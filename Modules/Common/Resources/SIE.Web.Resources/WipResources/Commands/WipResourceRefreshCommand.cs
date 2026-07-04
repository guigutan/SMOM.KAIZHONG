using SIE.Resources.WipResources;
using SIE.Web.Command;

namespace SIE.Web.Resources.WipResources.Commands
{
    /// <summary>
    /// 生产资源刷新命令
    /// </summary>
    [JsCommand("SIE.Web.Resources.WipResources.Commands.WipResourceRefreshCommand")]
    public class WipResourceRefreshCommand : ViewCommand
    {
        /// <summary>
        /// 命令执行逻辑
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>错误信息字符串</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return RT.Service.Resolve<WipResourceController>().RunSync();
        }
    }
}