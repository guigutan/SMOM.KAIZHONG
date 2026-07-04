using SIE.Domain;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Datas.EbsData;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Ebs.Download.TaskNos
{
    /// <summary>
    /// 任务数据下载
    /// </summary>
    public class EbsTaskNoController : DomainController
    {
        /// <summary>
        /// 任务数据下载
        /// </summary>
        /// <param name="invOrgId"></param>
        /// <param name="isManual">是否手工下载</param>        
        /// <returns>处理结果</returns>
        //public virtual ProcessResult Download(int? invOrgId, bool isManual = false)
        //{
        //    if (invOrgId.HasValue)
        //        AppRuntime.InvOrg = invOrgId.Value;
        //    var ebsPara = EbsHelper.GetEbsParameter(false);
        //    //Copy必改内容
        //    ebsPara.InterfaceCode = "S_E2W_PRODTASK";//接口编码，接口卡有
        //    const JobType jobType = JobType.Task;
        //    //End

        //    var ctl = RT.Service.Resolve<DownloadInfBaseController>();
        //    var jobTime = ctl.GetDownloadJobTime(jobType);
        //    var jobTimeDetail = new DownloadJobTimeDetail();
        //    jobTimeDetail.GenerateId();

        //    if (jobTime?.LastDownloadDate.HasValue == true)
        //        ebsPara.DownParameter.LastUpdateDate = jobTime.LastDownloadDate.Value;

        //    DateTime beginTime = DateTime.Now;
        //    //ERP数据获取
        //    var soapResult = EbsHelper.ExecuteEbs<TaskNoData>(ebsPara);

        //    var codes = soapResult.XV_RESULT.Select(a => a.Task_Number).ToList();
        //    var allDatas = codes.SplitContains(pCodes =>
        //    {
        //        return Query<SIE.WMS.Common.TaskNoMaintain>().Where(p => pCodes.Contains(p.Code)).ToList();
        //    });
        //    //移除不存在并且是失效的数据
        //    soapResult.XV_RESULT.RemoveAll(x => !allDatas.Select(a => a.Code).Contains(x.Task_Number + "[" + x.TASK_ID + "]") && x.Enable_Flag != "Y");
        //    soapResult.XV_RESULT.ForEach(p =>
        //    {
        //        p.Task = allDatas.FirstOrDefault(a => a.Code == (p.Task_Number + "[" + p.TASK_ID + "]"));
        //    });

        //    var result = RT.Service.Resolve<DownloadInfBaseController>().DownloadBusData(soapResult, p =>
        //    {    //Copy必改内容
        //        if (p.Task == null)
        //        {
        //            var data = new SIE.WMS.Common.TaskNoMaintain()
        //            {
        //                Code = p.Task_Number + "[" + p.TASK_ID + "]",
        //                Name = p.Task_Name,
        //                Desc = p.Description,
        //                State = State.Enable,
        //            };
        //            return data;
        //        }
        //        else
        //        {
        //            if (p.Enable_Flag != "Y")
        //                p.Task.State = State.Disable;
        //            else
        //            {
        //                p.Task.State = State.Enable;
        //                p.Task.Name = p.Task_Name;
        //                p.Task.Desc = p.Description;
        //            }
        //            return p.Task;
        //        }
        //    }, jobType, jobTime, jobTimeDetail, beginTime, isManual);

        //    return result;
        //}
    }
}
