using SIE.Core.Enums;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Sap.Controller;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Sap.Upload.OutPurchase
{
    /// <summary>
    /// 委外发/退事务
    /// </summary>
    public class HttpOutPurchaseController : SapUploadController
    {
        /// <summary>
        /// 委外出库事务上传接口
        /// </summary>      
        public virtual ProcessResult UploadPurchaseOutToErp(List<Tuple<OrderType, TransactionType, double?, string>> tuples)
        {
            //接口名称,必改
            string interfaceName = "委外出库事务上传接口";
            string zifcd = "SMOM0003";//SAP接口参数，接口卡提供
            var uploadDataHandler = new IUploadOutPurchaseHandle();
            var processResult = UploadDataToErp(tuples, uploadDataHandler, interfaceName, zifcd);
            //成功的数据：processResult.SuccessSapKey
            return processResult;
        }

        /// <summary>
        /// 委外入库事务上传接口
        /// </summary>      
        public virtual ProcessResult UploadPurchaseInToErp(List<Tuple<OrderType, TransactionType, double?, string>> tuples)
        {
            //接口名称,必改
            string interfaceName = "委外入库事务上传接口";
            string zifcd = "SMOM0004";
            var uploadDataHandler = new IUploadOutPurchaseInHandle();
            var processResult = UploadDataToErp(tuples, uploadDataHandler, interfaceName, zifcd);
            //成功的数据：processResult.SuccessSapKey
            return processResult;
        }
    }
}
