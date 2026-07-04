using SIE.Common.Schdules;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Sap.Upload.Deduction;
using SIE.ERPInterface.Sap.Upload.ScrapWeighingRecord;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Job.UploadSap.ScrapWeighingRecord
{
    /// <summary>
    /// 余料称重记录上传调度
    /// </summary>
    [Job("余料称重记录上传调度", typeof(JobParameter))]
    public class UploadScrapWeighingRecordJob : JobBase
    {
        protected override void ExecuteJob(object param)
        {
            var redisKey = "UploadScrapWeighingRecordJob" + RT.InvOrg;
            string lockId = null;
            var locked = RT.Redis.Lock(redisKey, out lockId, 1800);
            if (!locked)
                throw new ValidationException("任务正在运行中,不允许并发执行".L10N());

            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");

                RT.Service.Resolve<UploadBaseController>().UploadScrapWeighingRecordToInf();
                AddLog("结束上传中间表。".L10N());

                //上传ERP
                var result = RT.Service.Resolve<HttpSapScrapWeighingController>().UploadScrapWeighingToErp();
                AddLog("结束上传。{0}".L10nFormat(result.Msg));

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
