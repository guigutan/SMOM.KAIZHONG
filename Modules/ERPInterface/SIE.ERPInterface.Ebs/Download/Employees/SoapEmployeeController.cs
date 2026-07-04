using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using System;
using System.Xml;

namespace SIE.ERPInterface.Download.Employees
{
    /// <summary>
    /// 员工下载控制器
    /// </summary>
    public class SoapEmployeeController : DomainController
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

            var jobTime = ctl.GetDownloadJobTime(JobType.Item);
            var jobTimeDetail = new DownloadJobTimeDetail();
            jobTimeDetail.GenerateId();

            var soapPara = SoapHelper.GetSoapParameter();
            soapPara.P_SERVICE_NAME = "GET_EMPLOYEE_PEOPLE";
            if (isManual)
            {
                soapPara.SoapDownloadParameter = new SoapDownloadParameter() { Code_Label = "ITEM_CODE", Code = keyWord };
            }
            soapPara.P_BATCH_ID = jobTimeDetail.Id;
            soapPara.SoapDownloadParameter.LAST_UPDATE_DATE_FROM = jobTime?.LastDownloadDate?.ToString();

            //ERP数据获取
            var soapResult = SoapHelper.ExecuteSoap(soapPara, true);

            var result = ctl.SaveInfData<EmployeeInf>(soapResult, p =>
            {
                XmlNode xmlCode = p.SelectSingleNode("EMPLOYEE_NUMBER");
                XmlNode xmlName = p.SelectSingleNode("FULL_NAME");
                XmlNode xmlSex = p.SelectSingleNode("SEX");
                XmlNode xmlPhone = p.SelectSingleNode("TELEPHONE_NUMBER");
                XmlNode xmlEmail = p.SelectSingleNode("EMAIL_ADDRESS");
                XmlNode xmlHireDate = p.SelectSingleNode("EFFECTIVE_START_DATE");
                XmlNode xmlLastUpdateDate = p.SelectSingleNode("LAST_UPDATE_DATE");

                var employeeInf = new EmployeeInf();
                employeeInf.Code = xmlCode.InnerText;
                employeeInf.Name = xmlName.InnerText;
                employeeInf.Sex = xmlSex.InnerText.IsNullOrEmpty() || xmlSex.InnerText == "F" ? 0 : 1;
                employeeInf.Email = xmlEmail.InnerText;
                employeeInf.AccountCode = employeeInf.Code;
                employeeInf.Phone = xmlPhone.InnerText;
                employeeInf.HireDate = DateTime.Parse(xmlHireDate.InnerText);
                employeeInf.LastUpdateDate = DateTime.Parse(xmlLastUpdateDate.InnerText);
                employeeInf.ErpKey = employeeInf.Code;
                employeeInf.IsManual = false;
                return employeeInf;

            }, JobType.Item, jobTime, jobTimeDetail, DateTime.Now, false);
            return result;
        }
    }
}
