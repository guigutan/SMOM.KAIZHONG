using SIE.Common.Schdules;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Job.Common;
using SIE.ERPInterface.Sap.Upload.SaleReturn;
using SIE.ERPInterface.Sap.Upload.TaskReport;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Job.Upload.TaskReport
{

    /// <summary>
    /// 报工上传调度
    /// </summary>
    [Job("报工上传调度", typeof(JobParameter))]
    public class UplodTaskRecordJob : JobBase
    {
        /// <summary>
        /// 调度执行主键
        /// </summary>
        public static string _jobKey = "报工-" + typeof(UplodTaskRecordJob).FullName + "!" + RT.InvOrg;

        /// <summary>
        /// 执行调度
        /// </summary>
        /// <param name="param">参数</param>
        protected override void ExecuteJob(object param)
        {
            var p = param as ULCommonParameter;

            //报工记录上传至事务上传表
            RT.Service.Resolve<UploadBaseController>().UploadTaskReportRecordToInf();
            AddLog("结束上传中间表。".L10N());

            //上传ERP
            var result = RT.Service.Resolve<HttpSapTaskReportController>().UploadTaskReportToErp();
            AddLog("结束上传ERP。{0}".L10nFormat(result.Msg));


        }
    }
}
