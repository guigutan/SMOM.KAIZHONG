using SIE.Core.ProjectMaintains;
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

namespace SIE.ERPInterface.Ebs.Download.Projects
{
    /// <summary>
    /// 项目号数据下载
    /// </summary>
    public class EbsProjectNoController : DomainController
    {
        /// <summary>
        /// 项目号数据下载
        /// </summary>
        /// <param name="invOrgId"></param>
        /// <param name="isManual">是否手工下载</param>        
        /// <returns>处理结果</returns>
        public virtual ProcessResult Download(int? invOrgId, bool isManual = false)
        {
            if (invOrgId.HasValue)
                AppRuntime.InvOrg = invOrgId.Value;
            var ebsPara = EbsHelper.GetEbsParameter(false);
            //Copy必改内容
            ebsPara.InterfaceCode = "S_E2W_PROJECT";//接口编码，接口卡有
            const JobType jobType = JobType.Project;
            //End

            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            var jobTime = ctl.GetDownloadJobTime(jobType);
            var jobTimeDetail = new DownloadJobTimeDetail();
            jobTimeDetail.GenerateId();

            if (jobTime?.LastDownloadDate.HasValue == true)
                ebsPara.DownParameter.LastUpdateDate = jobTime.LastDownloadDate.Value;

            DateTime beginTime = DateTime.Now;
            //ERP数据获取
            var soapResult = EbsHelper.ExecuteEbs<ProjectNoData>(ebsPara);


            var codes = soapResult.XV_RESULT.Select(a => a.Project_Code).ToList();
            var allDatas = codes.SplitContains(pCodes =>
            {
                return Query<ProjectMaintain>().Where(p => pCodes.Contains(p.Code)).ToList();
            });
            List<string> excodes = new List<string>();
            List<string> names = new List<string>();
            //移除不存在并且是失效的数据
            soapResult.XV_RESULT.RemoveAll(x => !allDatas.Select(a => a.Code).Contains(x.Project_Code) && x.Enable_Flag != "Y");
            soapResult.XV_RESULT.ForEach(p =>
            {
                p.ProjectMaintain = allDatas.FirstOrDefault(a => a.Code == p.Project_Code);
                if (excodes.Contains(p.Project_Code))
                {
                    p.IsRepeat = true;
                }
                else
                {
                    excodes.Add(p.Project_Code);
                }
                if (names.Contains(p.Project_Name))
                {
                    p.IsRepeat = true;
                }
                else
                {
                    names.Add(p.Project_Name);
                }
            });

            var result = RT.Service.Resolve<DownloadInfBaseController>().DownloadBusData(soapResult, p =>
             {    //Copy必改内容
                 if (p.IsRepeat)
                     return null;
                if (p.ProjectMaintain == null)
                 {
                     var data = new ProjectMaintain()
                     {
                         Code = p.Project_Code,
                         Name = p.Project_Name,
                         Desc = p.Description,
                         State = State.Enable,
                     };
                     return data;
                 }
                 else
                 {
                     if (p.Enable_Flag != "Y")
                         p.ProjectMaintain.State =  State.Disable;
                     else
                     {
                         p.ProjectMaintain.State = State.Enable;
                         p.ProjectMaintain.Name = p.Project_Name;
                         p.ProjectMaintain.Desc = p.Description;
                     }
                     return p.ProjectMaintain;
                 }
             }, jobType, jobTime, jobTimeDetail, beginTime, isManual);

            return result;
        }
    }
}
