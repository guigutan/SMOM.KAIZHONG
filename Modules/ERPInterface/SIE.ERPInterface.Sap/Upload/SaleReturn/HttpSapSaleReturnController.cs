using SIE.Core.Enums;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Sap.Controller;
using SIE.ERPInterface.Sap.Upload.SaleOut;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;

namespace SIE.ERPInterface.Sap.Upload.SaleReturn
{
    /// <summary>
    /// 销售退货事务上传
    /// </summary>
    public class HttpSapSaleReturnController : SapUploadController
    {
        #region 销售退货（非冲销）


        /// <summary>
        /// 销售退货（非冲销）事务上传
        /// </summary>      
        public virtual ProcessResult UploadSaleReturnToErp(List<Tuple<OrderType, TransactionType, double?, string>> tuples)
        {
            //接口名称,必改
            string interfaceName = "销售退货（非冲销）上传接口";
            string zifcd = "SMOM0113";//SAP接口参数，接口卡提供
            var uploadDataHandler = new UploadSaleReturnHandle();
            var processResult = UploadDataToErp(tuples, uploadDataHandler, interfaceName, zifcd);
            //成功的数据：processResult.SuccessSapKey
            return processResult;
        }

        #endregion

        #region 销售退货（冲销）


        /// <summary>
        /// 销售退货（冲销）事务上传
        /// </summary>      
        public virtual ProcessResult UploadSaleReturnWriteOffToErp(List<Tuple<OrderType, TransactionType, double?, string>> tuples)
        {
            //接口名称,必改
            string interfaceName = "销售退货（非冲销）上传接口";
            string zifcd = "SMOM0114";//SAP接口参数，接口卡提供
            var uploadDataHandler = new UploadSaleReturnWriteOffHandle();
            var processResult = UploadDataToErp(tuples, uploadDataHandler, interfaceName, zifcd);
            //成功的数据：processResult.SuccessSapKey
            return processResult;
        }

        #endregion
    }
}
