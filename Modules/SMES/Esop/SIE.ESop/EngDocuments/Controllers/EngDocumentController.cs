using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ESop.EngDocuments.Controllers
{
    /// <summary>
    /// Controller层
    /// </summary>
    public class EngDocumentController : DomainController
    {
        /// <summary>
        /// 配置项获取
        /// </summary>
        /// <param name="orderInfos"></param>
        /// <param name="pagingInfo"></param>
        public virtual EntityList<FileUseDetail> GetFileUseDetailList(IList<OrderInfo> orderInfos, PagingInfo pagingInfo)
        {
            return Query<FileUseDetail>().OrderBy(orderInfos).ToList(pagingInfo);
        }
    }
}
