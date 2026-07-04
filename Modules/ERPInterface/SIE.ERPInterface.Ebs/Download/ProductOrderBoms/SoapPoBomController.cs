using SIE.ERPInterface.Common.Datas;

namespace SIE.ERPInterface.Ebs.Download.ProductOrderBoms
{
    /// <summary>
    /// 生产订单BOM下载控制器
    /// </summary>
    public class SoapPoBomController : DomainController
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
    }
}