using SIE.Alert;
using SIE.Common.Schdules;
using SIE.EMS.Checks.AlertPlugs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SIE.EMS.Job
{
    /// <summary>
    /// 点检计划提前预警
    /// </summary>
    [Job("点检计划提前预警", typeof(JobParameter))]
    public class CheckPlanAdvanceAlertPlugJob : JobBase
    {
        /// <summary>
        /// 点检计划提前提醒
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");
            var alerterTypes = new List<Type> { typeof(CheckPlanAdvanceAlertPlug) };
            var alerters = RT.Service.Resolve<AlertController>().GetAlerters(alerterTypes);
            if (alerters != null && alerters.Count > 0)
            {
                foreach (var curAlerter in alerters)
                {
                    try
                    {
                        RT.Service.Resolve<AlertController>().ExecuteAlert(curAlerter.Code);
                        AddLog($"点检计划提前预警[{curAlerter.Name}]执行成功 !");
                    }
                    catch (Exception exMsg)
                    {
                        AddLog($"点检计划提前预警[{curAlerter.Name}]执行失败，错误信息: {exMsg.Message}");
                    }
                    finally
                    {
                        Thread.Sleep(10);
                    }
                }
            }
            else
            {
                AddLog("请在预警配置中配置点检计划提前预警!");
            }
        }


    }
}
