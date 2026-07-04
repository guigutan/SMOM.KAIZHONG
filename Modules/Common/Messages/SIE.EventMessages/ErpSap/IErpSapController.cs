using SIE.EventMessages.ErpCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.ErpSap
{
    [Services.Service(FallbackType = typeof(DefaultIUploadLogControllercs))]
    public interface IErpSapController
    {
        /// <summary>
        /// 发货确认上传接口
        /// </summary>
        /// <param name="zuids"></param>
        void HttpOutboundConfirm(List<string> zuids);
    }

    public class DefaultIErpSapController : IErpSapController
    {
        /// <summary>
        /// 发货确认上传接口
        /// </summary>
        /// <param name="zuids"></param>
        public void HttpOutboundConfirm(List<string> zuids)
        {
            return;
        }
    }
}
