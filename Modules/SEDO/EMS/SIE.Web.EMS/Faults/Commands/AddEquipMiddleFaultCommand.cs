using SIE.EMS.Faults;
using SIE.Web.Command;

namespace SIE.Web.EMS.Faults.Commands
{
    /// <summary>
    /// 添加故障中类
    /// </summary>
    [JsCommand("SIE.Web.EMS.Faults.Commands.AddEquipMiddleFaultCommand")]
    public class AddEquipMiddleFaultCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 获取设备中类编码
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return RT.Service.Resolve<EquipFaultController>().GetEquipMiddleFaultCode();
        }
    }
}
