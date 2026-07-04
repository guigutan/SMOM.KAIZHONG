using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using SIE.ERPInterface.EBS.InfDataEntitys.Download;
using System;
using System.Data;

namespace SIE.ERPInterface.Ebs.Download.PurchaseOrders
{
    /// <summary>
    /// 从ERP下载采购订单分配行到中间表
    /// </summary>
    public class DbPoDisController : DomainController
    {
        /// <summary>
        /// 创建采购订单分配行接口数据
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>接口数据</returns>
        private PurchaseOrderDistributionsInf CreatePoDisInfData(DataRow row)
        {
            var poDisInf = new PurchaseOrderDistributionsInf();
            poDisInf.PoErpId = row["PO_HEADER_ID"].ToString();
            poDisInf.PoNo = row["PO_NUM"].ToString();
            poDisInf.PoLineErpId = row["PO_LINE_ID"].ToString();
            poDisInf.PoLineNo = row["LINE_NUM"].ToString();
            poDisInf.LineLocationNo = row["SHIPMENT_NUM"].ToString();
            poDisInf.LineLocationErpId = row["LINE_LOCATION_ID"].ToString();
            poDisInf.PoReleaseId = int.Parse(row["PO_RELEASE_ID"].ToString());
            poDisInf.QuantityOrdered = decimal.Parse(row["QUANTITY_ORDERED"].ToString());
            poDisInf.WoNo = row["WIP_ENTITY_ID"].ToString();
            poDisInf.LastUpdateDate = DateTime.Parse(row["LAST_UPDATE_DATE"].ToString());
            poDisInf.ErpKey = row["PO_DISTRIBUTION_ID"].ToString();
            return poDisInf;
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
            jobTimeDetail.RequestStr = "CUX_ERP_MES_INTERFACE_PKG.ERP_MES_PT_PO_DIS";
            var dt = DbHelper.ExecuteProcedure("CUX_ERP_MES_INTERFACE_PKG.ERP_MES_PT_PO_DIS", jobTime?.LastDownloadDate, keyWord);
            var result = ctl.SaveInfData<PurchaseOrderDistributionsInf, DownloadBaseEntity>(dt.Select(), CreatePoDisInfData, null, JobType.PurchaseOrderDetail, jobTime, jobTimeDetail, DateTime.Now, isManual);
            return result;
        }
    }
}
