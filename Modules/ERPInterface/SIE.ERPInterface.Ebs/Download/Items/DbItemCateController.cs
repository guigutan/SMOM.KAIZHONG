using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;

namespace SIE.ERPInterface.Ebs.Download.Items
{
    /// <summary>
    /// 从ERP下载物料分类到中间表
    /// </summary>
    public class DbItemCateController : DomainController
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
