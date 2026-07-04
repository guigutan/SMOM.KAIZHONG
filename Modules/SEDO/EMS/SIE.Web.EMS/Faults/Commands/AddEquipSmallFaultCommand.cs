using SIE.EMS.Faults;
using SIE.Web.Command;

namespace SIE.Web.EMS.Faults.Commands
{
    /// <summary>
    /// 添加故障小类
    /// </summary>
    [JsCommand("SIE.Web.EMS.Faults.Commands.AddEquipSmallFaultCommand")]
    public class AddEquipSmallFaultCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 获取设备故障小类编码
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>设备故障小类编码</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return RT.Service.Resolve<EquipFaultController>().GetEquipSmallFaultCode();
        }
    }
}
