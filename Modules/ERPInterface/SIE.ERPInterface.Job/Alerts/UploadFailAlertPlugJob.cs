using SIE.Alert;
using SIE.Common.Schdules;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.AlertPlugs;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SIE.ERPInterface.Job.Alerts
{
    /// <summary>
    /// ERP接口调度上传失败预警
    /// </summary>
    [Job("ERP接口调度上传失败预警", typeof(JobParameter))]
    public class UploadFailAlertPlugJob : JobBase
    {
        /// <summary>
        /// ERP接口调度上传失败预警
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            var alerterTypes = new List<Type> { typeof(UploadFailAlertPlug) };
            var alerters = RT.Service.Resolve<AlertController>().GetAlerters(alerterTypes);
            if (alerters != null && alerters.Count > 0)
            {
                foreach (var curAlerter in alerters)
                {
                    try
                    {
                        RT.Service.Resolve<AlertController>().ExecuteAlert(curAlerter.Code);
                        AddLog("ERP接口调度上传失败预警[{0}]执行成功 !".L10nFormat(curAlerter.Name));
                    }
                    catch (Exception exMsg)
                    {
                        throw new ValidationException("ERP接口调度上传失败预警[{0}]执行失败，错误信息: {1}".L10nFormat(curAlerter.Name, exMsg.Message));
                    }
                    finally
                    {
                        Thread.Sleep(10);
                    }
                }
            }
            else
            {
                throw new ValidationException("请在预警配置中配置ERP接口调度上传失败预警!".L10N());
            }
        }
    }
}
