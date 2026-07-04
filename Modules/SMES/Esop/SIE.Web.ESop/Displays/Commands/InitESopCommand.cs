using SIE.ESop.Displays;
using SIE.Web.Command;

namespace SIE.Web.ESop.Displays.Commands
{
    /// <summary>
    /// 初始化ESOP
    /// </summary>
    public class InitESopCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            RT.Service.Resolve<DisplayPointController>().InitEsop();
            return true;
        }
    }
}
