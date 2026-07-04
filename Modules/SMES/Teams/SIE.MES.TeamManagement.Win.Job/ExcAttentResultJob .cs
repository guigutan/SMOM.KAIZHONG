using SIE.Common.Schdules;
using SIE.MES.TeamManagement.Vacancies;
using System;

namespace SIE.MES.TeamManagement.Win.Job
{
    /// <summary>
    /// 考勤结果运算
    /// </summary>
    [Job("调度执行补漏考勤结果", typeof(JobParameter))]
    public class ExcAttentResultJob : JobBase
    {
        /// <summary>
        /// 调度执行3天的(不包括今天)考勤结果
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");
                var message = RT.Service.Resolve<JobController>().GetAndExcAttentRecord(3, false);
                if (message.IsNullOrEmpty())
                {
                    message = RT.Service.Resolve<VacancyController>().ExeWorkGroupVacancy(3, false);
                }

                AddLog(message.IsNullOrEmpty() ? "补漏考勤结果运算执行完成" : $"补漏考勤结果运算执行失败，错误信息：{message}");
            }
            catch (Exception ex)
            {
                AddLog($"补漏考勤结果运算执行失败，错误信息：{ex.Message}");
            }
        }
    }
}
