using SIE.EMS.Faults;
using SIE.Web.Command;

namespace SIE.Web.EMS.Faults.Commands
{
    /// <summary>
    /// 添加故障大类
    /// </summary>
    [JsCommand("SIE.Web.EMS.Faults.Commands.AddEquipLargeFaultCommand")]
    public class AddEquipLargeFaultCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 获取设备大类编码
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return RT.Service.Resolve<EquipFaultController>().GetEquipLargeFaultCode();
        }
    }
}
