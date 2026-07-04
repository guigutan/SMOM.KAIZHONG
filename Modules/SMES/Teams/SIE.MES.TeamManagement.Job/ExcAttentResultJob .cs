using SIE.Common.Schdules;
using SIE.Hangfire.Client;
using SIE.MES.TeamManagement.ClockingIns;
using SIE.MES.TeamManagement.Vacancies;
using System;

namespace SIE.MES.TeamManagement.Job
{
    /// <summary>
    /// 考勤结果运算
    /// </summary>
    [Job("调度执行补漏考勤结果", typeof(HangfireBaseParameter))]
    public class ExcAttentResultJob : HangfireBaseJob
    {
        /// <summary>
        /// 调度执行3天的(不包括今天)考勤结果
        /// </summary>
        /// <param name="para">调度参数</param>
        public override void ExecuteJob(object prarm)
        {
            try
            {
                Logging.LogManager.GetLogger("job").Info("当前执行库存组织[{0}]".FormatArgs(RT.InvOrg));
                var message = RT.Service.Resolve<ClockInController>().GetAndExcAttentRecord(3, false);
                if (message.IsNullOrEmpty())
                {
                    message = RT.Service.Resolve<VacancyController>().ExeWorkGroupVacancy(3, false);
                }

                if (!message.IsNullOrEmpty())
                    Logging.LogManager.GetLogger("job").Error("{0}\r\n补漏考勤结果运算执行失败，错误信息：\r\n{1}".FormatArgs("当前执行库存组织[{0}]".FormatArgs(RT.InvOrg), message));
                else
                    Logging.LogManager.GetLogger("job").Info("{0}\r\n补漏考勤结果运算执行完成：{1}".FormatArgs("当前执行库存组织[{0}]".FormatArgs(RT.InvOrg), DateTime.Now));
            }
            catch (Exception ex)
            {
                Logging.LogManager.GetLogger("job").Error("执行失败，错误信息：{0}".FormatArgs(ex.Message));
            }
        }
    }
}
