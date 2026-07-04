using SIE.Domain;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using SIE.Rbac.InvOrgs;
using System;
using System.Linq;

namespace SIE.ERPInterface.Download.InvOrgs
{
    /// <summary>
    /// 组织下载控制器
    /// </summary>
    public class InvOrgDownloadController : DomainController
    {
        /// <summary>
        /// 下载库存组织数据
        /// </summary>
        /// <param name="isManual">是否手工下载</param>        
        /// <returns>处理结果</returns>
        public virtual ProcessResult Download(bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();

            var jobTime = ctl.GetDownloadJobTime(JobType.InvOrg);
            var jobTimeDetail = new DownloadJobTimeDetail();
            jobTimeDetail.GenerateId();

            var ebsPara = EbsHelper.GetEbsParameter(false);
            ebsPara.InterfaceCode = "S_E2W_ORGANIZATION";
            if (jobTime?.LastDownloadDate.HasValue == true)
                ebsPara.DownParameter.LastUpdateDate = jobTime.LastDownloadDate.Value;

            DateTime beginTime = DateTime.Now;
            //ERP数据获取
            var soapResult = EbsHelper.ExecuteEbs<InvOrgData>(ebsPara);
            var allOrgs = RF.GetAll<InvOrg>();
            soapResult.XV_RESULT.RemoveAll(x => allOrgs.Select(a => a.Code).Contains(x.ORGANIZATION_ID));
            var result = RT.Service.Resolve<DownloadInfBaseController>().DownloadBusData(soapResult, p =>
            {
                var org = new InvOrg()
                {
                    Code = p.ORGANIZATION_ID,
                    Name = p.ORGANIZATION_NAME,
                    Remark = "ERP接口创建",
                    ExternalId = p.ORGANIZATION_ID.ToString(),
                };
                return org;
            }, JobType.InvOrg, jobTime, jobTimeDetail, beginTime, isManual);

            return result;
        }
    }
}
