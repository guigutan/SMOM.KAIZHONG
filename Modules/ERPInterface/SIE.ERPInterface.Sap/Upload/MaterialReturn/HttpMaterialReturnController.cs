using SIE.Core.Enums;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Sap.Controller;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Sap.Upload.MaterialReturn
{
    /// <summary>
    /// 生产退料事务上传
    /// </summary>
    public class HttpMaterialReturnController : SapUploadController
    {
        /// <summary>
        /// 生产退料事务上传
        /// </summary>      
        public virtual ProcessResult UploadMaterialReturnToErp(List<Tuple<OrderType, TransactionType, double?, string>> tuples)
        {
            //接口名称,必改
            string interfaceName = "生产退料事务上传接口";
            string zifcd = "SMOM0007";
            var uploadDataHandler = new IUploadMaterialReturnHandle();
            var processResult = UploadDataToErp(tuples, uploadDataHandler, interfaceName,zifcd);
            //成功的数据：processResult.SuccessSapKey
            return processResult;
        }
    }
}
