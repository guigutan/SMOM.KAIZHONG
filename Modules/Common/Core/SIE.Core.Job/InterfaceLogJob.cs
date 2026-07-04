using SIE.Common.Schdules;
using SIE.Core.Logs;
using System;
using System.Threading;

namespace SIE.Core.Job
{
    /// <summary>
    /// 清除接口日志
    /// </summary>
    [Job("清除接口日志Job", typeof(InterfaceLogParameter))]
    public class InterfaceLogJob : JobBase
    {
        /// <summary>
        /// 物料呼叫预警调度
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            var p = param as InterfaceLogParameter;
            if (p?.Day > 0)
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");

                try
                {
                    AddLog("开始清空{0}天以前的日志".FormatArgs(p.Day));
                    RT.Service.Resolve<InterfaceLogController>().ClearInterfaceLog(p.Day);
                    AddLog($"清除接口日志执行成功 !");
                }
                catch (Exception exMsg)
                {
                    AddLog($"清除接口日志执行失败，错误信息: {exMsg.Message}");
                }
                finally
                {
                    Thread.Sleep(10);
                }
            }                
        }
    }
}
