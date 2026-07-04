using SIE.Common.Schdules;
using SIE.MES.TeamManagement.ScoreRecords;
using System;

namespace SIE.MES.TeamManagement.Job
{
    /// <summary>
    /// 考勤结果运算
    /// </summary>
    [Job("自动生成系统评分项目", typeof(JobParameter))]
    public class ExcScoreJob : JobBase
    {
        /// <summary>
        /// 调度执行3天的(不包括今天)考勤结果
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n".L10N());
                var message = RT.Service.Resolve<ScoreRecordController>().ExcScore(3, false);
                AddLog(message.IsNullOrEmpty() ? "自动生成系统评分项目执行完成".L10N() : $"自动生成系统评分项目执行失败，错误信息：{message}".L10N());
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
                Logging.LogManager.GetLogger("job").Error("执行失败，错误信息：{0}".L10nFormat(ex.Message));
            }
        }
    }
}
