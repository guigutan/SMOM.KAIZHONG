using SIE.Web.Command;

namespace SIE.Web.EMS.Equipments.Units.Commands
{
    /// <summary>
    /// 添加子设备单元命令
    /// </summary>
    [JsCommand("SIE.Web.EMS.Equipments.Units.Commands.AddChildUnitCommand")]
    public class AddChildUnitCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行添加
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
