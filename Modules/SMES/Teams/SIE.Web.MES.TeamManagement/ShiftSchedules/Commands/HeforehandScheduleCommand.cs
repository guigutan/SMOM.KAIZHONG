using SIE.MES.TeamManagement.ShiftSchedules;
using SIE.MES.TeamManagement.ShiftSchedules.Models;
using SIE.Web.Command;
using SIE.Web.MES.TeamManagement.ShiftSchedules.Helpers;

namespace SIE.Web.MES.TeamManagement.ShiftSchedules
{
    /// <summary>
    /// 预排班命令
    /// </summary>
    [JsCommand("SIE.Web.MES.TeamManagement.ShiftSchedules.HeforehandScheduleCommand")]
    public class HeforehandScheduleCommand : ViewCommand
    {
        /// <summary>
        /// 预排班命令执行
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">范围</param>
        /// <returns>预排班结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<ScheduleData>();
            var stores = RT.Service.Resolve<ShiftScheduleController>().HeforehandSchedule(data);
            return ScheduleHelper.GetStoreEntityJsons(stores, data.ShiftConfig);
        }
    }
}