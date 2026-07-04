using SIE.ERPInterface.Common.Datas;

namespace SIE.ERPInterface.Ebs.Download.PurchaseOrders
{
    /// <summary>
    /// 采购订单下载控制
    /// </summary>
    public class SoapPoController : DomainController
    {
        /// <summary>
        /// 下载到中间表
        /// </summary>
        /// <param name="isManual">是否手工下载</param>
        /// <param name="keyWord">查询关键字</param>
        /// <returns>处理结果</returns>
        public virtual ProcessResult DownloadToInf(bool isManual = false, string keyWord = null)
        {
            return null;
        }

        /// <summary>
        /// 从ERP下载采购订单明细到中间表
        /// </summary>
        public virtual void DownloadPoDtlERPToInf()
        {

        }
    }
}
