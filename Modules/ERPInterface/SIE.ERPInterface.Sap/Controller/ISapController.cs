using SIE.ERPInterface.Sap.Upload.OutboundConfirm;
using SIE.EventMessages.ErpSap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Sap.Controller
{
    public class ISapController: IErpSapController
    {
        /// <summary>
        /// 发货确认上传接口
        /// </summary>
        /// <param name="zuids"></param>
        public virtual void HttpOutboundConfirm(List<string> zuids)
        {
            //RT.Service.Resolve<HttpSapOutboundConfirmController>()
        }
    }
}
