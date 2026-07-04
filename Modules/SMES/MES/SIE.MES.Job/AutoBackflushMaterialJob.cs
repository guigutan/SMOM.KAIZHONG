using SIE.Common.Schdules;
using SIE.MES.WIP;
using System;

namespace SIE.MES.Job
{
    /// <summary>
    /// 自动执行倒扣料Job
    /// </summary>
    [Job("自动执行倒扣料Job", typeof(JobParameter))]
    public class AutoBackflushMaterialJob : JobBase
    {
        /// <summary>
        /// 自动执行倒扣料Job执行方法
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");
                var result = RT.Service.Resolve<BackflushMaterialController>().AutoBackflushMaterial();
                AddLog(($"自动执行倒扣料完成，成功{result.SuccessCount}笔，失败{result.FailCount}笔。"));
            }
            catch (Exception exMsg)
            {
                AddLog($"报工执行失败，错误信息: {exMsg.Message}");
            }
        }
    }
}