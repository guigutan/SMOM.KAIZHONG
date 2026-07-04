using SIE.Common.Schdules;
using SIE.Hangfire.Client;
using SIE.MES.TeamManagement.ClockingIns;
using SIE.MES.TeamManagement.Vacancies;
using System;

namespace SIE.MES.TeamManagement.Job
{
    /// <summary>
    /// 同步当天的考勤数据并运算结果
    /// </summary>
    [Job("调度执行当天考勤结果及班组缺编统计", typeof(HangfireBaseParameter))]
    public class ExcTodayAttentResultJob : HangfireBaseJob
    {
        /// <summary>
        /// 调度执行3天的(不包括今天)考勤结果
        /// </summary>
        /// <param name="para">调度参数</param>
        /// <returns>是否执行成功</returns>
        public override void ExecuteJob(object prarm)
        {
            try
            {
                Logging.LogManager.GetLogger("job").Info("当前执行库存组织[{0}]".FormatArgs(RT.InvOrg));
                var message = RT.Service.Resolve<ClockInController>().GetAndExcAttentRecord(0, true);
                if (message.IsNullOrEmpty())
                {
                    message = RT.Service.Resolve<VacancyController>().ExeWorkGroupVacancy(0, true);
                    if (!message.IsNullOrEmpty())
                    {
                        message = "考勤运算成功！但班组缺编统计失败" + message;
                    }
                }

                if (!message.IsNullOrEmpty())
                    Logging.LogManager.GetLogger("job").Error("{0}\r\n当天考勤结果运算及班组缺编统计执行失败，错误信息：\r\n{1}".FormatArgs("当前执行库存组织[{0}]".FormatArgs(RT.InvOrg), message));
                else
                    Logging.LogManager.GetLogger("job").Info("{0}\r\n补当天考勤结果运算及班组缺编统计执行完成：{1}".FormatArgs("当前执行库存组织[{0}]".FormatArgs(RT.InvOrg), DateTime.Now));
            }
            catch (Exception ex)
            {
                Logging.LogManager.GetLogger("job").Error("执行失败，错误信息：{0}".FormatArgs(ex.Message));
            }
        }
    }
}
