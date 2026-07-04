using SIE.Web.Command;
using System;

namespace SIE.Web.EMS.Equipments.RunngingStates.Commands
{
    /// <summary>
    /// 保存：手动同步设备运行状态
    /// </summary>
    [JsCommand("SIE.Web.EMS.Equipments.RunngingStates.Commands.ManualSyncEquipRunningStateRecordCommand")]
    public class ManualSyncEquipRunningStateRecordCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            throw new NotImplementedException();
        }
    }
}
