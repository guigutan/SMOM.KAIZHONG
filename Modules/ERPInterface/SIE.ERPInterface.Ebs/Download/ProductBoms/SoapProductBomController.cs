using SIE.ERPInterface.Common.Datas;

namespace SIE.ERPInterface.Ebs.Download.ProductBoms
{
    /// <summary>
    /// 产品BOM下载控制器
    /// </summary>
    public class SoapProductBomController : DomainController
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
        /// 从ERP下载产品BOM明细到中间表
        /// </summary>
        public virtual ProcessResult DownloadProductBomDtlERPToInf()
        {
            return null;
        }
    }
}
