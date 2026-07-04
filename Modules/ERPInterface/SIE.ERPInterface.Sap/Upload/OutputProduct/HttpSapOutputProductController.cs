using SIE.Core.Enums;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Sap.Controller;
using SIE.Inventory.Transactions;
using SIE.KZ.Base.Interfaces.Enums;
using System;
using System.Collections.Generic;

namespace SIE.ERPInterface.Sap.Upload.OutputProduct
{
    public class HttpSapOutputProductController : SapUploadController
    {
        /// <summary>
        /// 副产品上传
        /// </summary>      
        public virtual ProcessResult UploadOutputProductToErp()
        {
            List<Tuple<OrderType, TransactionType, double?, string>> tuples = new List<Tuple<OrderType, TransactionType, double?, string>>();
            tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.OutputProduct, TransactionType.OutputProduct, null, string.Empty));

            //接口名称,必改
            string interfaceName = "Prodorderfeed";
            string zifcd = "";//SAP接口参数，接口卡提供
            var uploadDataHandler = new UploadOutputProductHandle();
            var processResult = UploadInterface(tuples, uploadDataHandler, interfaceName, InfType.OutputProduct, CallDirection.MesToSap, zifcd);

            //成功的数据：processResult.SuccessSapKey
            return processResult;
        }
    }
}
