using SIE.Core.Enums;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Sap.Controller;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace SIE.ERPInterface.Sap.Upload.TaskReport
{
    /// <summary>
    /// 报工上传
    /// </summary>
    public class HttpSapTaskReportController : SapUploadController
    {
        #region 报工上传
        /// <summary>
        /// 报工上传
        /// </summary>      
        public virtual ProcessResult UploadTaskReportToErp()
        {
            List<Tuple<OrderType, TransactionType, double?, string>> tuples = new List<Tuple<OrderType, TransactionType, double?, string>>();
            tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.WoFinish, TransactionType.WoFinish, null, string.Empty));
            //委外收货报工的也一起发送
            tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.ProcessingInStockReport, TransactionType.ProcessingInStockReport, null, string.Empty));

            //接口名称,必改
            string interfaceName = "TimeTicket";
            string zifcd = "";//SAP接口参数，接口卡提供
            var uploadDataHandler = new UploadTaskReportHandle();
            var processResult = UploadDataToErp(tuples, uploadDataHandler, interfaceName, zifcd);
            //成功的数据：processResult.SuccessSapKey
            return processResult;
        }

        #endregion
    }
}
