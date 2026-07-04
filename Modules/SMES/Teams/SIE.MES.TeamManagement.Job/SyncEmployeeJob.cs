using SIE.Common.Schdules;
using SIE.MES.TeamManagement.ClockingIns;
using SIE.MES.TeamManagement.Vacancies;
using System;

namespace SIE.MES.TeamManagement.Job
{
    /// <summary>
    /// 每天早上同步人员到人员出勤表调度
    /// </summary>
    [Job("同步人员信息到人员出勤及班组缺编初始化", typeof(JobParameter))]
    public class SyncEmployeeJob : JobBase
    {
        /// <summary>
        /// 调度同步人员到人员出勤表调度
        /// </summary>
        /// <param name="param">调度参数</param>
        /// <returns>是否执行成功</returns>
        protected override void ExecuteJob(object param)
        {
            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");
                string message = RT.Service.Resolve<ClockInController>().GetEffectEmployee();
                if (message.IsNullOrEmpty())
                {
                    message = RT.Service.Resolve<VacancyController>().SyncWorkGroupVacancy();
                    if (!message.IsNullOrEmpty())
                    {
                        message = "同步人员信息成功！但班组缺编初始化失败" + message;
                    }
                }

                AddLog(message.IsNullOrEmpty() ? "同步人员信息到人员出勤及班组缺编初始化执行完成" : $"同步人员信息到人员出勤及班组缺编初始化执行失败，错误信息：{message}");
            }
            catch (Exception ex)
            {
                AddLog($"同步人员信息到人员出勤及班组缺编初始化执行失败，错误信息: {ex.Message}");
            }
        }
    }
}
