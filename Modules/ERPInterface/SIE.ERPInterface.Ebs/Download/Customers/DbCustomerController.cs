using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using System;
using System.Data;

namespace SIE.ERPInterface.Ebs.Download.Customers
{
    /// <summary>
    /// 从ERP下载客户到中间表
    /// </summary>
    public class DbCustomerController : DomainController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isManual"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public virtual ProcessResult DownloadToInf(bool isManual = false, string keyWord = null)
        {
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();

            var jobTime = ctl.GetDownloadJobTime(JobType.Item);
            var jobTimeDetail = new DownloadJobTimeDetail();
            jobTimeDetail.GenerateId();
            jobTimeDetail.RequestStr = "CUX_ERP_MES_INTERFACE_PKG.ERP_MES_CUSTOMER";
            var dt = DbHelper.ExecuteProcedure("CUX_ERP_MES_INTERFACE_PKG.ERP_MES_CUSTOMER", jobTime?.LastDownloadDate, keyWord);
            var result = ctl.SaveInfData<CustomerInf, DownloadBaseEntity>(dt.Select(), CreateInfData, null, JobType.Item, jobTime, jobTimeDetail, DateTime.Now, isManual);
            return result;
        }

        /// <summary>
        /// 创建接口数据
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>接口数据</returns>
        private CustomerInf CreateInfData(DataRow row)
        {
            var inf = new CustomerInf();
            inf.Code = row["CUSTOMER_CODE"].ToString();
            inf.Name = row["CUSTOMER_NAME"].ToString();
            inf.ContactsAddress = row["ADDRESS"].ToString();
            inf.ContactsNumber = row["PHONE_NUMBER"].ToString();
            inf.Contacts = row["CONTACT"].ToString();
            inf.DutyParagraph = row["CS_TAX_CLASSIFICATION_CODE"].ToString();
            inf.LastUpdateDate = DateTime.Parse(row["LAST_UPDATE_DATE"].ToString());
            inf.ErpKey = inf.Code;
            return inf;
        }
    }
}
