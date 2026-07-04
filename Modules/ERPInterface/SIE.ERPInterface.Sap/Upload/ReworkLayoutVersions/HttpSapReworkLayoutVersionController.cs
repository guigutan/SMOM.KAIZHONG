using SIE.Core.Enums;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Sap.Controller;
using SIE.ERPInterface.Sap.Upload.Deduction;
using SIE.Inventory.Transactions;
using SIE.KZ.Base.Interfaces.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Sap.Upload.ReworkLayoutVersions
{
    public class HttpSapReworkLayoutVersionController : SapUploadController
    {
        /// <summary>
        /// 扣料上传
        /// </summary>      
        public virtual ProcessResult UploadReworkLayoutVersionToErp()
        {
            List<Tuple<OrderType, TransactionType, double?, string>> tuples = new List<Tuple<OrderType, TransactionType, double?, string>>();
            tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.ReworkInfoRecord, TransactionType.ReworkInfoRecord, null, string.Empty));

            //接口名称,必改
            string interfaceName = "ReworkPOCreate";
            string zifcd = "";//SAP接口参数，接口卡提供
            var uploadDataHandler = new UploadReworkLayoutVersionHandle();
            var processResult = UploadInterface(tuples, uploadDataHandler, interfaceName, InfType.ReworkInfoRecord, CallDirection.MesToSap, zifcd);

            //成功的数据：processResult.SuccessSapKey
            return processResult;
        }

    }
}
