using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using System;
using System.Data;

namespace SIE.ERPInterface.Ebs.Download.Suppliers
{
    /// <summary>
    /// 从ERP下载客户到中间表
    /// </summary>
    public class DbSupplierController : DomainController
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
            jobTimeDetail.RequestStr = "CUX_ERP_MES_INTERFACE_PKG.ERP_MES_SUPPLIER";
            var dt = DbHelper.ExecuteProcedure("CUX_ERP_MES_INTERFACE_PKG.ERP_MES_SUPPLIER", jobTime?.LastDownloadDate, keyWord);
            var result = ctl.SaveInfData<SupplierInf, DownloadBaseEntity>(dt.Select(), CreateInfData, null, JobType.Item, jobTime, jobTimeDetail, DateTime.Now, isManual);
            return result;
        }

        /// <summary>
        /// 创建接口数据
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>接口数据</returns>
        private SupplierInf CreateInfData(DataRow row)
        {
            var inf = new SupplierInf();
            inf.Code = row["SUPPLIER_CODE"].ToString();
            inf.Name = row["SUPPLIER_NAME"].ToString();
            inf.ContactAddress = row["ADDRESS"].ToString();
            inf.ContactNumber = row["TELEPHONE"].ToString();
            inf.Contacts = row["CONTACT"].ToString();
            inf.DutyParagraph = row["CS_TAX_CLASSIFICATION_CODE"].ToString();
            inf.Email = row["EMAIL"].ToString();
            inf.ZipCode = row["POSTCODE"].ToString();
            inf.LastUpdateDate = DateTime.Parse(row["LAST_UPDATE_DATE"].ToString());
            inf.ErpKey = inf.Code;
            return inf;
        }
    }
}
