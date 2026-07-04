using SIE.Common.Schdules;
using SIE.MES.TeamManagement.Vacancies;
using System;

namespace SIE.MES.TeamManagement.Win.Job
{
    /// <summary>
    /// 同步当天的考勤数据并运算结果
    /// </summary>
    [Job("调度执行当天考勤结果及班组缺编统计", typeof(JobParameter))]
    public class ExcTodayAttentResultJob : JobBase
    {
        /// <summary>
        /// 调度执行3天的(不包括今天)考勤结果
        /// </summary>
        /// <param name="param">调度参数</param>
        /// <returns>是否执行成功</returns>
        protected override void ExecuteJob(object param)
        {
            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");
                var message = RT.Service.Resolve<JobController>().GetAndExcAttentRecord(0, true);
                if (message.IsNullOrEmpty())
                {
                    message = RT.Service.Resolve<VacancyController>().ExeWorkGroupVacancy(0, true);
                    if (!message.IsNullOrEmpty())
                    {
                        message = "考勤运算成功！但班组缺编统计失败" + message;
                    }
                }

                AddLog(message.IsNullOrEmpty() ? $"补当天考勤结果运算及班组缺编统计执行完成" : $"当天考勤结果运算及班组缺编统计执行失败，错误信息：{message}");
            }
            catch (Exception ex)
            {
                AddLog($"当天考勤结果运算及班组缺编统计执行失败，错误信息：{ex.Message}");
            }
        }
    }
}
