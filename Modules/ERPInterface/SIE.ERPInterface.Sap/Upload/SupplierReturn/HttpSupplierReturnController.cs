using SIE.Core.Enums;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Sap.Controller;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Sap.Upload.SupplierReturn
{
    /// <summary>
    /// 供应商退货事务上传
    /// </summary>
    public class HttpSupplierReturnController : SapUploadController
    {
        /// <summary>
        /// 供应商退货上传接口
        /// </summary>      
        public virtual ProcessResult UploadSupplierReturnToErp(List<Tuple<OrderType, TransactionType, double?, string>> tuples)
        {
            //接口名称,必改
            string interfaceName = "供应商退货上传接口";
            string zifcd = "SMOM0005";
            var uploadDataHandler = new IUploadSupplierReturnHandle();
            var processResult = UploadDataToErp(tuples, uploadDataHandler, interfaceName, zifcd);
            //成功的数据：processResult.SuccessSapKey
            return processResult;
        }
    }
}
