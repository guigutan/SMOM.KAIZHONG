using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using System;
using System.Data;

namespace SIE.ERPInterface.Ebs.Download.PurchaseOrders
{
    /// <summary>
    /// 从ERP下载采购订单到中间表
    /// </summary>
    public class DbPoController : DomainController
    {
        /// <summary>
        /// 创建采购订单接口数据
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>接口数据</returns>
        private PurchaseOrderInf CreatePoInfData(DataRow row)
        {
            var poInf = new PurchaseOrderInf();
            poInf.No = row["PO_NUM"].ToString();
            poInf.SupplierErpKey = row["VENDOR_ID"].ToString();
            poInf.SupplierAdrssErpKey = row["VENDOR_SITE_ID"].ToString();
            poInf.BillDate = DateTime.Parse(row["CREATE_DATE"].ToString());
            poInf.AuditDate = DateTime.Parse(row["APPROVED_DATE"].ToString());
            poInf.Remark = row["COMMENTS"].ToString();
            poInf.LastUpdateDate = DateTime.Parse(row["LAST_UPDATE_DATE"].ToString());
            poInf.ErpKey = row["PO_HEADER_ID"].ToString();
            return poInf;
        }

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
            jobTimeDetail.RequestStr = "CUX_ERP_MES_INTERFACE_PKG.ERP_MES_PT_PO";
            var dt = DbHelper.ExecuteProcedure("CUX_ERP_MES_INTERFACE_PKG.ERP_MES_PT_PO", jobTime?.LastDownloadDate, keyWord);
            var result = ctl.SaveInfData<PurchaseOrderInf, DownloadBaseEntity>(dt.Select(), CreatePoInfData, null, JobType.PurchaseOrder, jobTime, jobTimeDetail, DateTime.Now, isManual);
            return result;
        }
    }
}
