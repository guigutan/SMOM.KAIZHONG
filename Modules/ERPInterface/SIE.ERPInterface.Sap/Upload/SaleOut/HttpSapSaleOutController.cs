using SIE.Core.Enums;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Sap.Controller;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;

namespace SIE.ERPInterface.Sap.Upload.SaleOut
{
    /// <summary>
    /// 销售出库事务上传
    /// </summary>
    public class HttpSapSaleOutController : SapUploadController
    {
        #region 销售出库    

        /// <summary>
        /// 销售出库事务上传
        /// </summary>      
        public virtual ProcessResult UploadSaleOutToErp(List<Tuple<OrderType, TransactionType, double?, string>> tuples)
        {
            //接口名称,必改
            string interfaceName = "销售出库上传接口";
            string zifcd = "SMOM0112";//SAP接口参数，接口卡提供
            var uploadDataHandler = new IUploadSaleOutHandle();
            var processResult = UploadDataToErp(tuples, uploadDataHandler, interfaceName, zifcd);
            return processResult;
        }

        #endregion

    }
}
