using SIE.Web.Command;

namespace SIE.Web.Tech.Processs.Commands
{
    [JsCommand("SIE.Web.Tech.Processs.Commands.SaveProcessCommand")]
    public class SaveProcessCommand : SaveCommand
    {
        /// <summary>
        /// 工序保存命令
        /// </summary>
        /// <param name="args">数据</param>
        /// <param name="scope">scope</param>
        /// <returns></returns>
        /// <remarks>在添加修改工序命令中调用</remarks>
        protected override object Excute(ViewArgs args, string scope)
        {
            base.Excute(args, scope);
            return true;
        }
    }
}
