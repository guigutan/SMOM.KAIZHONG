using SIE.Domain;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Datas.EbsData;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using SIE.Resources;
using System;
using System.Linq;

namespace SIE.ERPInterface.Ebs.Download.AccountAliases
{
    /// <summary>
    /// 单位下载
    /// </summary>
    public class EbsAccountController : DomainController
    {
        /// <summary>
        /// 下载单位数据
        /// </summary>
        /// <param name="invOrgId"></param>
        /// <param name="isManual">是否手工下载</param>        
        /// <returns>处理结果</returns>
        public virtual ProcessResult Download(int? invOrgId, bool isManual = false)
        {
            if (invOrgId.HasValue)
                AppRuntime.InvOrg = invOrgId.Value;
            var ebsPara = EbsHelper.GetEbsParameter();
            //Copy必改内容
            ebsPara.InterfaceCode = "S_E2W_DISPOSITION";//接口编码，接口卡有
            const JobType jobType = JobType.AccountAliases;
            //End

            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            var jobTime = ctl.GetDownloadJobTime(jobType);
            var jobTimeDetail = new DownloadJobTimeDetail();
            jobTimeDetail.GenerateId();

            if (jobTime?.LastDownloadDate.HasValue == true)
                ebsPara.DownParameter.LastUpdateDate = jobTime.LastDownloadDate.Value;

            DateTime beginTime = DateTime.Now;
            //ERP数据获取
            var soapResult = EbsHelper.ExecuteEbs<ErpAccountData>(ebsPara);

            var codes = soapResult.XV_RESULT.Select(a => a.DISPOSITION_CODE).ToList();
            var allDatas = codes.SplitContains(pCodes =>
            {
                return Query<ErpAccount>().Where(p => pCodes.Contains(p.Code)).ToList();
            });
            soapResult.XV_RESULT.ForEach(p =>
            {
                p.ErpAccount = allDatas.FirstOrDefault(a => a.Code == p.DISPOSITION_CODE);
            });
            var result = RT.Service.Resolve<DownloadInfBaseController>().DownloadBusData(soapResult, p =>
             {    //Copy必改内容
                 if (p.ErpAccount == null)
                 {
                      
                     var data = new ErpAccount()
                     {
                         Code = p.DISPOSITION_CODE,
                         Name = p.DESCRIPTION,
                         SourceKey = p.Pri_Key,
                         CreateDate = p.CREATION_DATE,
                     };
                     if (!p.EFFECTIVE_DATE.IsNullOrEmpty())
                     {
                         DateTime.TryParse(p.EFFECTIVE_DATE, out DateTime effectiveDate);
                         data.EffectiveDate = effectiveDate;
                     }
                     else
                         data.EffectiveDate = DateTime.Now;
                     if (!p.EFFECTIVE_DATE.IsNullOrEmpty())
                     {
                         DateTime.TryParse(p.DISABLE_DATE, out DateTime disableDate);
                         data.DisableDate = disableDate;
                     }
                     return data;
                 }
                 else
                 {
                     p.ErpAccount.Name = p.DESCRIPTION;
                     if (!p.EFFECTIVE_DATE.IsNullOrEmpty())
                     {
                         DateTime.TryParse(p.EFFECTIVE_DATE, out DateTime effectiveDate);
                         p.ErpAccount.EffectiveDate = effectiveDate;
                     }
                     else
                         p.ErpAccount.EffectiveDate = DateTime.Now;
                     if (!p.EFFECTIVE_DATE.IsNullOrEmpty())
                     {
                         DateTime.TryParse(p.DISABLE_DATE, out DateTime disableDate);
                         p.ErpAccount.DisableDate = disableDate;
                     }
                     p.ErpAccount.SourceKey = p.Pri_Key;
                     return p.ErpAccount;
                 }
             }, jobType, jobTime, jobTimeDetail, beginTime, isManual);

            return result;
        }
    }
}
