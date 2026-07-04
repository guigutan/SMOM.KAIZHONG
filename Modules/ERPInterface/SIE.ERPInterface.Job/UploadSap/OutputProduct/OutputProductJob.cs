using SIE.Common.Schdules;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Sap.Upload.Deduction;
using SIE.ERPInterface.Sap.Upload.OutputProduct;
using System;

namespace SIE.ERPInterface.Job.UploadSap.OutputProduct
{
    /// <summary>
    /// "副产品记录上传调度
    /// </summary>
    [Job("副产品记录上传调度", typeof(JobParameter))]
    public class OutputProductJob : JobBase
    {
        //是否运行中,用来防止并发问题
        public static bool IsRun = false;

        protected override void ExecuteJob(object param)
        {
            if (IsRun)
                throw new ValidationException("任务正在运行中,不允许并发执行".L10N());
            IsRun = true;

            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");

                //"副产品记录上传事务
                RT.Service.Resolve<UploadBaseController>().UploadOutputProductToInf();
                AddLog("结束上传中间表。".L10N());

                //上传ERP
                var result = RT.Service.Resolve<HttpSapOutputProductController>().UploadOutputProductToErp();
                AddLog("结束上传ERP。{0}".L10nFormat(result.Msg));

            }
            catch (Exception exMsg)
            {
                AddLog($"执行失败，错误信息: {exMsg.Message}");
            }
            finally
            {
                IsRun = false;
            }
        }

    }
}
