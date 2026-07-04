using SIE.Core.Enums;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap.Controller;
using SIE.ERPInterface.Sap.Upload.Finished;
using SIE.Inventory.Commom;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Sap.Upload.Allocate
{
    /// <summary>
    /// 库存调拨上传接口
    /// </summary>
    public class HttpAllocateController : SapUploadController
    {         
        /// <summary>
        /// 库存调拨上传
        /// </summary>      
        public virtual ProcessResult UploadAllocateToErp(List<Tuple<OrderType, TransactionType, double?, string>> tuples)
        {
            //接口名称,必改
            string interfaceName = "库存调拨上传接口";
            string zifcd = "SMOM0009";//SAP接口参数，接口卡提供
            var uploadDataHandler = new IUploadAllocateHandle();
            var processResult = UploadDataToErp(tuples, uploadDataHandler, interfaceName, zifcd);
            //处理入库凭证
            return processResult;
        }
    
    }
}
