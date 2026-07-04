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
    /// 从ERP下载采购订单明细到中间表
    /// </summary>
    public class DbPoDtlController : DomainController
    {
        /// <summary>
        /// 创建采购订单明细接口数据
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>接口数据</returns>
        private PurchaseOrderDetailInf CreatePoDtlInfData(DataRow row)
        {
            var poDtlInf = new PurchaseOrderDetailInf();
            poDtlInf.PoErpKey = row["PO_HEADER_ID"].ToString();
            poDtlInf.LineNo = row["LINE_NUM"].ToString();
            poDtlInf.Quantity = decimal.Parse(row["QUANTITY"].ToString());
            poDtlInf.UnitPrice = decimal.Parse(row["UNIT_PRICE"].ToString());
            poDtlInf.ItemErpKey = row["ITEM_ID"].ToString();
            ////poDtlInf.ItemCode = row["COMMENTS"].ToString();
            poDtlInf.LastUpdateDate = DateTime.Parse(row["LAST_UPDATE_DATE"].ToString());
            poDtlInf.ErpKey = row["PO_LINE_ID"].ToString();
            return poDtlInf;
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
            jobTimeDetail.RequestStr = "CUX_ERP_MES_INTERFACE_PKG.ERP_MES_PT_PO_DTL";
            var dt = DbHelper.ExecuteProcedure("CUX_ERP_MES_INTERFACE_PKG.ERP_MES_PT_PO_DTL", jobTime?.LastDownloadDate, keyWord);
            var result = ctl.SaveInfData<PurchaseOrderDetailInf, DownloadBaseEntity>(dt.Select(), CreatePoDtlInfData, null, JobType.PurchaseOrderDetail, jobTime, jobTimeDetail, DateTime.Now, isManual);
            return result;
        }
    }
}
