using SIE.Domain;
using SIE.MES.TeamManagement.ClockingIns;
using SIE.ObjectModel;
using SIE.Web.Command;

namespace SIE.Web.MES.TeamManagement.ClockingIns
{
    /// <summary>
    /// 员工出勤保存命令
    /// </summary>
    [JsCommand("SIE.Web.MES.TeamManagement.ClockingIns.Commands.EmployeeAttentSaveCommand")]
    public class EmployeeAttentSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存前操作
        /// </summary>
        /// <param name="data">员工出勤统计集合</param>
        protected override void DoSave(EntityList data)
        {
            RT.Service.Resolve<ClockInController>().CheckAttentEditData(data as EntityList<EmployeeClockInAttent>);
            base.DoSave(data);
        }

        /// <summary>
        /// 保存后操作
        /// </summary>
        /// <param name="data">员工出勤统计集合</param>
        protected override void OnSaved(EntityList data)
        {
            base.OnSaved(data);
            if (data.Count == 0) return;
            var item = data[0] as EmployeeClockInAttent;
            DateRange dr = new DateRange() { BeginValue = item.ClockInDate, EndValue = item.ClockInDate };
            RT.Service.Resolve<ClockInController>().ExeEmployeeClockInState(dr);
        }
    }
}