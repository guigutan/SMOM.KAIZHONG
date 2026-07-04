using SIE.Domain;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using System;
using System.Collections.Generic;
using System.Xml;

namespace SIE.ERPInterface.Download.Customers
{
    /// <summary>
    /// 客户下载控制器
    /// </summary>
    public class SoapCustomerController : DomainController
    {
        /// <summary>
        /// 从ERP下载客户到中间表
        /// </summary>
        /// <param name="isManual"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public virtual ProcessResult DownloadToInf(bool isManual = false, string keyWord = null)
        {
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();

            var jobTime = ctl.GetDownloadJobTime(JobType.Supplier);
            var jobTimeDetail = new DownloadJobTimeDetail();
            jobTimeDetail.GenerateId();

            var soapPara = SoapHelper.GetSoapParameter();
            soapPara.P_SERVICE_NAME = "GET_PARTY";
            soapPara.P_BATCH_ID = jobTimeDetail.Id;
            if (isManual)
            {
                soapPara.SoapDownloadParameter.Code_Label = "VENDOR_CODE";
                soapPara.SoapDownloadParameter.Code = keyWord;
            }
            soapPara.SoapDownloadParameter.LAST_UPDATE_DATE_FROM = jobTime?.LastDownloadDate?.ToString();


            //webservice获取ERP数据
            var soapResult = SoapHelper.ExecuteSoap(soapPara, true);

            var result = ctl.SaveInfData<CustomerInf>(soapResult, p =>
            {
                XmlNode xmlCode = p.SelectSingleNode("CUSTOMER_CODE");
                XmlNode xmlName = p.SelectSingleNode("CUSTOMER_NAME");
                XmlNode xmlContactAddress = p.SelectSingleNode("ADDRESS");
                XmlNode xmlContactNumber = p.SelectSingleNode("PHONE_NUMBER");
                XmlNode xmlContacts = p.SelectSingleNode("CONTACT");
                XmlNode xmlDutyParagraph = p.SelectSingleNode("CS_TAX_CLASSIFICATION_CODE");
                XmlNode xmlLastUpdateDate = p.SelectSingleNode("LAST_UPDATE_DATE");

                var customerInf = new CustomerInf();
                customerInf.Code = xmlCode.InnerText;
                customerInf.Name = xmlName.InnerText;
                customerInf.ContactsAddress = xmlContactAddress.InnerText;
                customerInf.ContactsNumber = xmlContactNumber.InnerText;
                customerInf.Contacts = xmlContacts.InnerText;
                customerInf.DutyParagraph = xmlDutyParagraph.InnerText;
                customerInf.LastUpdateDate = DateTime.Parse(xmlLastUpdateDate.InnerText);
                customerInf.ErpKey = customerInf.Code;

                return customerInf;
            }, JobType.Supplier, jobTime, jobTimeDetail, DateTime.Now, false);
            return result;
        }
    }
}
