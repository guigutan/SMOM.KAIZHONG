using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;

namespace SIE.ERPInterface.Ebs.Download.Receipts
{
    /// <summary>
    /// ASN订单下载控制器
    /// </summary>
    public class SoapAsnController : DomainController
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
        /// 从ERP下载ASN明细到中间表
        /// </summary>
        public virtual ProcessResult DownloadAsnDtlERPToInf()
        {
            return null;
        }
    }
}
