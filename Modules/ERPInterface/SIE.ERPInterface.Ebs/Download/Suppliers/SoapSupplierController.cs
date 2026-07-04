using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using System;
using System.Xml;

namespace SIE.ERPInterface.Ebs.Download.Suppliers
{
    /// <summary>
    /// 供应商下载控制器
    /// </summary>
    public class SoapSupplierController : DomainController
    {
        /// <summary>
        /// 下载到中间表
        /// </summary>
        /// <param name="isManual">是否手工下载</param>
        /// <param name="keyWord">查询关键字</param>
        /// <returns>处理结果</returns>
        public virtual ProcessResult DownloadToInf(bool isManual = false, string keyWord = null)
        {
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();

            var jobTime = ctl.GetDownloadJobTime(JobType.Supplier);
            var jobTimeDetail = new DownloadJobTimeDetail();
            jobTimeDetail.GenerateId();

            var soapPara = SoapHelper.GetSoapParameter();
            soapPara.P_SERVICE_NAME = "GET_PO_VENDOR";
            soapPara.P_BATCH_ID = jobTimeDetail.Id;
            if (isManual)
            {
                soapPara.SoapDownloadParameter.Code_Label = "VENDOR_CODE";
                soapPara.SoapDownloadParameter.Code = keyWord;
            }

            soapPara.SoapDownloadParameter.LAST_UPDATE_DATE_FROM = jobTime?.LastDownloadDate?.ToString();
            //webservice获取ERP数据
            var soapResult = SoapHelper.ExecuteSoap(soapPara, true);

            var result = ctl.SaveInfData<SupplierInf>(soapResult, p =>
             {
                 XmlNode xmlCode = p.SelectSingleNode("SUPPLIER_CODE");
                 XmlNode xmlName = p.SelectSingleNode("SUPPLIER_NAME");
                 XmlNode xmlContactAddress = p.SelectSingleNode("ADDRESS");
                 XmlNode xmlContactNumber = p.SelectSingleNode("TELEPHONE");
                 XmlNode xmlContacts = p.SelectSingleNode("CONTACT");
                 XmlNode xmlDutyParagraph = p.SelectSingleNode("CS_TAX_CLASSIFICATION_CODE");
                 XmlNode xmlEmail = p.SelectSingleNode("EMAIL");
                 XmlNode xmlZipCode = p.SelectSingleNode("POSTCODE");
                 XmlNode xmlLastUpdateDate = p.SelectSingleNode("LAST_UPDATE_DATE");

                 var supplierInf = new SupplierInf();
                 supplierInf.Code = xmlCode.InnerText;
                 supplierInf.Name = xmlName.InnerText;
                 supplierInf.ContactAddress = xmlContactAddress.InnerText;
                 supplierInf.ContactNumber = xmlContactNumber.InnerText;
                 supplierInf.Contacts = xmlContacts.InnerText;
                 supplierInf.DutyParagraph = xmlDutyParagraph.InnerText;
                 supplierInf.Email = xmlEmail.InnerText;
                 supplierInf.ZipCode = xmlZipCode.InnerText;
                 supplierInf.LastUpdateDate = DateTime.Parse(xmlLastUpdateDate.InnerText);
                 supplierInf.ErpKey = supplierInf.Code;

                 return supplierInf;
             }, JobType.Supplier, jobTime, jobTimeDetail, DateTime.Now, false);
            return result;
        }
    }
}
