using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.AbnormalInfo.AbnormalMonitors.AbnormalDefinitions;
using SIE.Common.Schdules;
using SIE.DataPortal;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SIE.AbnormalInfo.Job
{
    /// <summary>
    /// 异常定义生成异常任务调度
    /// </summary>
    [Job("异常定义生成异常任务调度", typeof(JobParameter))]
    public class AbnormalDefinitionJob : JobBase
    {

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="context"></param>
        protected override void ExecuteJob(JobContext context)
        {
            RT.InvOrg = context.InvOrg;
            RT.Principal = new DataPortalPrincipal(context.IdentityId, 0, "");
            Type paramType = Type.GetType(context.JobClass).GetCustomAttribute<JobAttribute>()?.ParameterType ?? typeof(JobParameter);
            var param = Activator.CreateInstance(paramType) as JobParameter;
            param?.Initialize(context.Parameter);

            var jobConfig = RT.Service.Resolve<AbnormalJobConfigController>().GetJobConfig(context.Key);
            if (jobConfig == null) return;
            var curDate = DateTime.Now;
            string info = String.Empty;
            try
            {
                info = RT.Service.Resolve<AbnormalDefineJobService>().GenerateTask(curDate, jobConfig.Id, param);
            }
            catch (Exception ex)
            {
                throw new SIE.Domain.Validation.ValidationException("遇到错误：{0}".L10nFormat(ex.Message));
            }
            finally
            {
                AddLog("[{0}]任务调度完成：{1}".L10nFormat(curDate.ToString(), info));
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        protected override void ExecuteJob(object param)
        {
            //具体执行在ExecuteJob(JobContext context)方法中
        }
    }
}
