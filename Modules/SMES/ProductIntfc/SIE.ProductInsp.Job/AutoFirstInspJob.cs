using SIE.Common.Schdules;
using SIE.ProductIntfc.InspLogs;
using System;

namespace SIE.ProductInsp.Job
{
    /// <summary>
    /// 首件自动报检调度
    /// </summary>
    [Job("首件自动报检", typeof(JobParameter))]
    public class AutoFirstInspJob : JobBase
    {
        /// <summary>
        /// 执行首件自动报检调度
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");
                var message = RT.Service.Resolve<InspLogController>().AutoFirstInsp();
                AddLog(message.IsNullOrEmpty() ? "首件自动报检执行完成" : $"首件自动报检执行失败，错误信息：{message}");
            }
            catch (Exception ex)
            {
                AddLog($"首件自动报检执行失败，错误信息: {ex.Message}");
            }
        }
    }
}