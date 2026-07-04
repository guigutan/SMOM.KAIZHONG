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
    /// 从ERP下载采购订单发运行到中间表
    /// </summary>
    public class DbPoLocController : DomainController
    {
        /// <summary>
        /// 创建采购订单发运行接口数据
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>接口数据</returns>
        private PurchaseOrderLocationsInf CreatePoLocInfData(DataRow row)
        {
            var poLocInf = new PurchaseOrderLocationsInf();
            poLocInf.LocNo = row["SHIPMENT_NUM"].ToString();
            poLocInf.PoErpId = row["PO_HEADER_ID"].ToString();
            poLocInf.PoNo = row["PO_NUM"].ToString();
            poLocInf.PoLineErpId = row["PO_LINE_ID"].ToString();
            poLocInf.PoLineNo = row["LINE_NUM"].ToString();
            poLocInf.PoReleaseId = int.Parse(row["PO_RELEASE_ID"].ToString());
            poLocInf.Quantity = decimal.Parse(row["QUANTITY"].ToString());
            poLocInf.WoNo = row["WIP_ENTITY_ID"].ToString();
            poLocInf.LastUpdateDate = DateTime.Parse(row["LAST_UPDATE_DATE"].ToString());
            poLocInf.ErpKey = row["LINE_LOCATION_ID"].ToString();
            return poLocInf;
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
            jobTimeDetail.RequestStr = "CUX_ERP_MES_INTERFACE_PKG.ERP_MES_PT_PO_LOC";
            var dt = DbHelper.ExecuteProcedure("CUX_ERP_MES_INTERFACE_PKG.ERP_MES_PT_PO_LOC", jobTime?.LastDownloadDate, keyWord);
            var result = ctl.SaveInfData<PurchaseOrderLocationsInf, DownloadBaseEntity>(dt.Select(), CreatePoLocInfData, null, JobType.PurchaseOrderDetail, jobTime, jobTimeDetail, DateTime.Now, isManual);
            return result;
        }

    }
}
