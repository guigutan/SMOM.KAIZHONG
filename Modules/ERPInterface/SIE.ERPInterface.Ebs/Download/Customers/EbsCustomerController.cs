using SIE.Domain;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SIE.ERPInterface.Ebs.Download.Customers
{
    /// <summary>
    /// 客户数据下载
    /// </summary>
    public class EbsCustomerController : DomainController
    {
        /// <summary>
        /// 客户数据下载(需要先下载供应商数据)
        /// </summary>
        /// <param name="invOrgId">组织</param>
        /// <param name="isManual">是否手工下载</param>        
        /// <returns>处理结果</returns>
        public virtual ProcessResult Download(int? invOrgId, bool isManual = false)
        {
            if (invOrgId.HasValue)
                AppRuntime.InvOrg = invOrgId;
            var ebsPara = EbsHelper.GetEbsParameter();

            //Copy必改内容
            ebsPara.InterfaceCode = "S_E2W_CUSTOMER";//接口编码，接口卡有
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
            var soapResult = EbsHelper.ExecuteEbs<CustomerDataEbs>(ebsPara);

            var codes = soapResult.XV_RESULT.Select(a => a.Party_Number).ToList();
            var allDatas = codes.SplitContains(pCodes =>
             {
                 return Query<SIE.CSM.Customers.Customer>().Where(p => pCodes.Contains(p.Code)).ToList();
             });
            //移除不存在并且是失效的数据
            soapResult.XV_RESULT.RemoveAll(x => !allDatas.Select(a => a.Code).Contains(x.Party_Number) && x.Enable_Flag != "Y" || x.Organization_Id != invOrgId);

            var sups = RF.GetAll<SIE.CSM.Suppliers.Supplier>();
            List<string> names = new List<string>();
            List<string> excodes = new List<string>();
            soapResult.XV_RESULT.ForEach(p =>
            {
                var sId = sups.FirstOrDefault(a => a.Code == p.Vendor_Code)?.Id;
                p.Vendor_Code = sId.HasValue ? sId.ToString() : "";
                p.Customer = allDatas.FirstOrDefault(a => a.Code == p.Party_Number);
                if (excodes.Contains(p.Party_Number))
                    p.IsRepeat = true;
                else
                    excodes.Add(p.Party_Number);
                if (names.Contains(p.Party_Name))
                    p.IsRepeat = true;
                else
                    names.Add(p.Party_Name);
                if (!p.IsRepeat)
                {                   
                    if (p.Primary_Phone_Number.IsNotEmpty())
                    {
                        var regex = new Regex(@"^(\d{7,8})$|^(0\d{2,3}-\d{7,8})$|^(1[3456789]\d{9})$");
                        if (!regex.IsMatch(p.Primary_Phone_Number))
                            p.Primary_Phone_Number = "";
                    }
                    else if (p.Postal_Code.IsNotEmpty())
                    {
                        var regex = new Regex(@"^\d{6}$");
                        if (!regex.IsMatch(p.Postal_Code))
                            p.Postal_Code = "";
                    }
                    else if (p.Email_Address.IsNotEmpty())
                    {
                        var regex = new Regex(@"^([a-zA-Z0-9_\.-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$");
                        if (!regex.IsMatch(p.Email_Address))
                            p.Email_Address = "";
                    }
                }
            });
            var result = RT.Service.Resolve<DownloadInfBaseController>().DownloadBusData(soapResult, p =>
             {    //Copy必改内容
                 if (!p.IsRepeat)
                 {
                     if (p.Customer == null)
                     {
                         var data = new SIE.CSM.Customers.Customer()
                         {
                             Code = p.Party_Number,
                             Name = p.Party_Name,
                             ContactsAddress = p.Address1,
                             Contacts = p.Contact_Person,
                             ContactsNumber = p.Primary_Phone_Number,
                             SourceType = SIE.Common.SourceType.External,
                             CustomerType = CSM.Customers.CustomerType.CUSTOMER,
                             DutyParagraph = p.Num_1099,
                             State = State.Enable,
                             EMail = p.Email_Address,
                             Region = p.Country,
                             ShortName = p.Known_As,
                             ZipCode = p.Postal_Code,
                             SupplierId = p.Vendor_Code.IsNotEmpty() ? double.Parse(p.Vendor_Code) : null,
                         };
                         return data;
                     }
                     else
                     {
                         if (p.Enable_Flag != "Y")
                             p.Customer.State = State.Disable;
                         else
                         {
                             p.Customer.Name = p.Party_Name;
                             p.Customer.ContactsAddress = p.Address1;
                             p.Customer.Contacts = p.Contact_Person;
                             p.Customer.ContactsNumber = p.Primary_Phone_Number;
                             p.Customer.CustomerType = CSM.Customers.CustomerType.CUSTOMER;
                             p.Customer.DutyParagraph = p.Num_1099;
                             p.Customer.State = State.Enable;
                             p.Customer.EMail = p.Email_Address;
                             p.Customer.SourceType = SIE.Common.SourceType.External;
                             p.Customer.Region = p.Country;
                             p.Customer.ShortName = p.Known_As;
                             p.Customer.ZipCode = p.Postal_Code;
                             p.Customer.SupplierId = p.Vendor_Code.IsNotEmpty() ? double.Parse(p.Vendor_Code) : null;
                         }
                         return p.Customer;
                     }
                 }
                 else
                     return null;
             }, jobType, jobTime, jobTimeDetail, beginTime, isManual);

            return result;
        }
    }
}
