using SIE.Common.InvOrg;
using SIE.Domain;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using SIE.Rbac.InvOrgs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Ebs.Download.Suppliers
{
    /// <summary>
    /// 供应商数据下载
    /// </summary>
    public class EbsSupplierController : DomainController
    {
        /// <summary>
        /// 供应商数据下载
        /// </summary>
        /// <param name="invOrgId">库存组织</param>
        /// <param name="isManual">是否手工下载</param>        
        /// <returns>处理结果</returns>
        public virtual ProcessResult Download(int? invOrgId, bool isManual = false)
        {
            if (invOrgId.HasValue)
                AppRuntime.InvOrg = invOrgId;
            var ebsPara = EbsHelper.GetEbsParameter(false);
            //Copy必改内容
            ebsPara.InterfaceCode = "S_E2W_VENDOR";//接口编码，接口卡有
            const JobType jobType = JobType.Supplier;
            //End

            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            var jobTime = ctl.GetDownloadJobTime(jobType);
            var jobTimeDetail = new DownloadJobTimeDetail();
            jobTimeDetail.GenerateId();

            if (jobTime?.LastDownloadDate.HasValue == true)
                ebsPara.DownParameter.LastUpdateDate = jobTime.LastDownloadDate.Value;

            DateTime beginTime = DateTime.Now;
            //ERP数据获取
            var soapResult = EbsHelper.ExecuteEbs<SupplierDataEbs>(ebsPara);
            var codes = soapResult.XV_RESULT.Select(a => a.Vendor_Num).ToList();
            var allDatas = codes.SplitContains(pCodes =>
            {
                return Query<SIE.CSM.Suppliers.Supplier>().Where(p => pCodes.Contains(p.Code)).ToList();
            });
            //移除不存在并且是失效的数据
            soapResult.XV_RESULT.RemoveAll(x => !allDatas.Select(a => a.Code).Contains(x.Vendor_Num) && x.Enable_Flag != "Y");
            List<string> excodes = new List<string>();
            List<string> names = new List<string>();
            soapResult.XV_RESULT.ForEach(p =>
            {
                p.Supplier = allDatas.FirstOrDefault(a => a.Code == p.Vendor_Num);
                if (excodes.Contains(p.Vendor_Num))
                    p.IsRepeat = true;
                else
                    excodes.Add(p.Vendor_Num);
                if (names.Contains(p.Vendor_Name))
                    p.IsRepeat = true;
                else
                    names.Add(p.Vendor_Name);
            });

            var result = RT.Service.Resolve<DownloadInfBaseController>().DownloadBusData(soapResult, p =>
             {    //Copy必改内容
                 if (!p.IsRepeat)
                 {
                     if (p.Supplier == null)
                     {
                         var data = new SIE.CSM.Suppliers.Supplier()
                         {
                             Code = p.Vendor_Num,
                             Name = p.Vendor_Name,
                             SourceType = SIE.Common.SourceType.External,
                             ShortName = p.Vendor_Name_Alt,
                             IsPortal = false,
                         };
                         return data;
                     }
                     else
                     {
                         if (p.Enable_Flag != "Y")
                             p.Supplier.State = State.Disable;
                         else
                         {
                             p.Supplier.State = State.Enable;
                             p.Supplier.Name = p.Vendor_Name;
                             p.Supplier.ShortName = p.Vendor_Name_Alt;
                         }
                         return p.Supplier;
                     }
                 }
                 else return null;
             }, jobType, jobTime, jobTimeDetail, beginTime, isManual);

            return result;
        }
    }
}
