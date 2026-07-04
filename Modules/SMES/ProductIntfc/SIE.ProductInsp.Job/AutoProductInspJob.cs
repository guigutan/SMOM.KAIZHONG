using SIE.Common.Schdules;
using SIE.ProductIntfc.InspRecords;
using System;

namespace SIE.ProductInsp.Job
{
    /// <summary>
    /// 成品自动报检调度
    /// </summary>
    [Job("成品自动报检", typeof(JobParameter))]
    public class AutoProductInspJob : JobBase
    {
        /// <summary>
        /// 执行成品自动报检调度
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");
                var message = RT.Service.Resolve<InspRecordController>().AutoProductInsp();
                AddLog(message.IsNullOrEmpty() ? "成品自动报检执行完成" : $"成品自动报检执行失败，错误信息：{message}");
            }
            catch (Exception ex)
            {
                AddLog($"成品自动报检执行失败，错误信息：{ex.Message}");
            }
        }
    }
}