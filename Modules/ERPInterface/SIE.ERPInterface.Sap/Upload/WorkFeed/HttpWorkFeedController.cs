using SIE.Core.Enums;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Sap.Controller;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Sap.Upload.WorkFeed
{
    /// <summary>
    /// 工单发料事务上传
    /// </summary>
    public class HttpWorkFeedController : SapUploadController
    {
        /// <summary>
        /// 工单发料事务上传
        /// </summary>      
        public virtual ProcessResult UploadWorkFeedToErp(List<Tuple<OrderType, TransactionType, double?, string>> tuples)
        {
            //接口名称,必改
            string interfaceName = "工单发料事务上传接口";
            string zifcd = "SMOM0006";
            var uploadDataHandler = new IUploadWorkFeedHandle();
            var processResult = UploadDataToErp(tuples, uploadDataHandler, interfaceName, zifcd);
            //成功的数据：processResult.SuccessSapKey
            return processResult;
        }
    }
}
