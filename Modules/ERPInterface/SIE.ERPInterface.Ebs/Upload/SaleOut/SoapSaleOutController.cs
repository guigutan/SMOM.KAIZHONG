using SIE.Core.Enums;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Ebs.Upload
{
    /// <summary>
    /// 销售出库事务上传
    /// </summary>
    public class SoapSaleOutController : Common.Controller.UploadBaseController
    {
        #region 销售出库事务上传

        /// <summary>
        /// 从SMOM中间表上传销售出库事务到ERP
        /// </summary>
        public virtual ProcessResult UploadReceiveInfToErp(List<Tuple<OrderType, TransactionType, double?, string>> tuples)
        {
            var soapPara = SoapHelper.GetSoapParameter();

            //构建上传接口参数明细
            int lineId = 1;
            var soapUploadParameterDtls = new List<SoapUploadParameterDtl>();
            var uploadTransactions = this.GetUploadTransactions(tuples);        //查询未处理事务上传表数据
            foreach (var uploadTransaction in uploadTransactions)
            {
                soapUploadParameterDtls.Add(new SoapUploadParameterDtl()
                {
                    //WIP_ENTITY_NAME = uploadTransaction.WoNo,
                    LINE_ID = lineId.ToString(),
                    //MOVE_QUANTITY = uploadTransaction.Quantity.ToString(),
                    UploadTransactionId = uploadTransaction.Id
                });
                lineId++;
            }

            //上传事务记录
            var transLog = new UploadTransactionLog();
            transLog.GenerateId();
            //上传接口参数
            soapPara.P_SERVICE_NAME = "SALE_OUT";
            soapPara.P_BATCH_ID = transLog.Id;
            soapPara.SoapUploadParameterDtlList.AddRange(soapUploadParameterDtls);
            //webservice获取ERP数据
            var soapResult = SoapHelper.ExecuteSoap(soapPara, false);

            var paraDtlDic = soapUploadParameterDtls.ToDictionary(p => p.LINE_ID);
            var result = this.SaveTransactionData(soapResult, uploadTransactions, transLog, paraDtlDic);
            return result;
        }

        #endregion
    }
}
