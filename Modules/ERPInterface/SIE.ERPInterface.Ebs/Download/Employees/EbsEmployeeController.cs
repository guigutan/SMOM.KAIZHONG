using SIE.Domain;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Ebs.Download.Employees
{
    /// <summary>
    /// 员工数据下载
    /// </summary>
    public class EbsEmployeeController : DomainController
    {
        /// <summary>
        /// 员工数据下载 
        /// </summary>
        /// <param name="isManual">是否手工下载</param>        
        /// <returns>处理结果</returns>
        public virtual ProcessResult Download(int? invOrgId, bool isManual = false)
        {
            if (invOrgId.HasValue)
                AppRuntime.InvOrg = invOrgId.Value;
            var ebsPara = EbsHelper.GetEbsParameter(false);
            //Copy必改内容
            ebsPara.InterfaceCode = "S_E2W_EMP";//接口编码，接口卡有
            const JobType jobType = JobType.Customer;
            //End

            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            var jobTime = ctl.GetDownloadJobTime(jobType);
            var jobTimeDetail = new DownloadJobTimeDetail();
            jobTimeDetail.GenerateId();

            if (jobTime?.LastDownloadDate.HasValue == true)
                ebsPara.DownParameter.LastUpdateDate = jobTime.LastDownloadDate.Value;

            DateTime beginTime = DateTime.Now;
            //ERP数据获取
            var soapResult = EbsHelper.ExecuteEbs<EmployeeDataEbs>(ebsPara);
            var codes = soapResult.XV_RESULT.Select(a => a.Employee_Number).ToList();
            var allDatas = RT.Service.Resolve<SIE.Resources.Employees.EmployeeController>().GetEmployeeList(codes);
            List<string> excodes = new List<string>();
            soapResult.XV_RESULT.ForEach(p =>
            {
                p.Employee = allDatas.FirstOrDefault(a => a.Code == p.Employee_Number);
                if (excodes.Contains(p.Employee_Number))
                    p.IsRepeat = true;
                else
                    excodes.Add(p.Employee_Number);
            });

            //移除不存在并且是失效的数据
            soapResult.XV_RESULT.RemoveAll(x => !allDatas.Select(a => a.Code).Contains(x.Employee_Number) && x.Enable_Flag != "Y" || x.Employee_Number.IsNullOrEmpty() || x.Full_Name.IsNullOrEmpty()
            || x.Original_Date_Of_Hire?.Year <= 1900);


            var result = RT.Service.Resolve<DownloadInfBaseController>().DownloadBusData(soapResult, p =>
             {    //Copy必改内容
                 if (p.IsRepeat)
                     return null;
                 if (p.Employee == null)
                 {
                     var data = new SIE.Resources.Employees.Employee()
                     {
                         Code = p.Employee_Number,
                         Name = p.Full_Name,
                         Email = p.Email_Address,
                         EmployeeStatus = Resources.Employees.EmployeeStatus.Job,
                         HireDate = p.Original_Date_Of_Hire,
                         Sex = p.Sex == 0 ? Resources.Employees.Sex.Man : Resources.Employees.Sex.Madam,
                     };
                     return data;
                 }
                 else
                 {
                     if (p.Enable_Flag == "Y")
                     {
                         p.Employee.Name = p.Full_Name;
                         p.Employee.Email = p.Email_Address;
                         p.Employee.HireDate = p.Original_Date_Of_Hire;
                         p.Employee.Sex = p.Sex == 0 ? Resources.Employees.Sex.Man : Resources.Employees.Sex.Madam;
                         return p.Employee;
                     }
                     return null;
                     
                 }
             }, jobType, jobTime, jobTimeDetail, beginTime, isManual);

            return result;
        }
    }
}
