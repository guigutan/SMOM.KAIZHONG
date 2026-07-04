using SIE.Web.Command;

namespace SIE.Web.Resources.ShiftTypes.Commands
{
    /// <summary>
    /// 休息时间添加命令
    /// </summary>
    public class ShiftResetAddCommand : ViewCommand
    {
        /// <summary>
        /// 休息时间添加命令
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
