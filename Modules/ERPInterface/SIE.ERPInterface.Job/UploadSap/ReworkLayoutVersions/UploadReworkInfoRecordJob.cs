using SIE.Common.Schdules;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Sap.Upload.ReworkLayoutVersions;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Job.UploadSap.ReworkLayoutVersions
{
    /// <summary>
    /// 返工信息上传调度
    /// </summary>
    [Job("返工信息上传调度", typeof(JobParameter))]
    public class UploadReworkInfoRecordJob : JobBase
    {
        /// <summary>
        /// Job执行方法
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            var redisKey = "UploadReworkInfoRecordJob" + RT.InvOrg;
            string lockId = null;
            var locked = RT.Redis.Lock(redisKey, out lockId, 600);
            if (!locked)
                throw new ValidationException("任务正在运行中,不允许并发执行".L10N());

            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");

                RT.Service.Resolve<UploadBaseController>().UploadReworkInfoRecordToInf();
                AddLog("结束上传中间表。".L10N());

                var result = RT.Service.Resolve<HttpSapReworkLayoutVersionController>().UploadReworkLayoutVersionToErp();
                AddLog("结束上传ERP。{0}".L10nFormat(result.Msg));
            }
            catch (Exception exMsg)
            {
                AddLog($"执行失败，错误信息: {exMsg.Message}");
            }
            finally
            {
                RT.Redis.UnLock(redisKey, lockId);
            }
        }

    }
}
