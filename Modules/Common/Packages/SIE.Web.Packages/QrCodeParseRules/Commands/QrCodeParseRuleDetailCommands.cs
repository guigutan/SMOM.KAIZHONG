using SIE.Web.Command;

namespace SIE.Web.Packages.QrCodeParseRules.Commands
{
    /// <summary>
    /// 添加命令
    /// </summary>
    public class AddQrCodeParseRuleDetailCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }

    /// <summary>
    /// 修改命令
    /// </summary>
    public class EditQrCodeParseRuleDetailCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}