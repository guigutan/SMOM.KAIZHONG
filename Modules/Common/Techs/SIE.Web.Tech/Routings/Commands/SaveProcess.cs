using SIE.Security;
using SIE.Web.Command;

namespace SIE.Web.Tech.Routings.Commands
{
    [JsCommand("SIE.Web.Tech.Routings.Commands.SaveProcess")]
    [AllowAnonymous]
    public class SaveProcess : FormSaveCommand
    {
        /// <summary>
        /// 工艺路线工序弹框保存命令
        /// </summary>
        /// <param name="args">数据</param>
        /// <param name="scope">scope</param>
        /// <returns></returns>
        /// <remarks>在添加修改工序命令中调用</remarks>
        protected override object Excute(ViewArgs args, string scope)
        {
            base.Excute(args, "SIE.Tech.Processs.Process,SIE.Tech");
            return true;
        }
    }
}
