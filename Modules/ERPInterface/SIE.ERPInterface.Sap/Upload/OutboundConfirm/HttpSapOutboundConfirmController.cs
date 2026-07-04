using SIE.Core.Enums;
using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap.Controller;
using SIE.ERPInterface.Sap.Upload.Deduction;
using SIE.Inventory.Transactions;
using SIE.KZ.Base.Interfaces.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Sap.Upload.OutboundConfirm
{
    public class HttpSapOutboundConfirmController: SapUploadController
    {
        /// <summary>
        /// 
        /// </summary>      
        public virtual ProcessResult UploadOutboundConfirm()
        {
            List<Tuple<OrderType, TransactionType, double?, string>> tuples = new List<Tuple<OrderType, TransactionType, double?, string>>();
            tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.OutboundConfirm, TransactionType.OutboundConfirm, null, string.Empty));

            //接口名称,必改
            string interfaceName = "CheckOut";
            string zifcd = "";//SAP接口参数，接口卡提供
            var uploadDataHandler = new OutboundConfirmHandle();
            var processResult = UploadInterface(tuples, uploadDataHandler, interfaceName, InfType.OutboundConfirm, CallDirection.MesToSap, zifcd);

            //成功的数据：processResult.SuccessSapKey
            return processResult;
        }
    }
}
