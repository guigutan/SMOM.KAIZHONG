using SIE.Core.Enums;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap.Connection;
using SIE.ERPInterface.Sap.Controller;
using SIE.ERPInterface.Sap.Datas;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using SapParameter = SIE.ERPInterface.Sap.Datas.SapParameter;

namespace SIE.ERPInterface.Sap.Upload
{
    /// <summary>
    /// 销售出库事务上传
    /// </summary>
    public class RfcSapSaleOutController : DomainController
    {
        /// <summary>
        /// 从SMOM中间表上传销售出库事务到ERP
        /// </summary>
        /// <param name="tuples"></param>
        /// <returns></returns>
        public virtual ProcessResult UploadSaleOutToErp(List<Tuple<OrderType, TransactionType, double?, string>> tuples)
        {
            var ctl = RT.Service.Resolve<SapUploadController>();
            var sapParameters = new List<SapParameter>();
            var uploadTransactions = ctl.GetUploadTransactions(tuples);//查询未处理事务上传表数据

            //构建上传接口参数明细
            foreach (var uploadTransaction in uploadTransactions)
            {
                var sapParameter = new SapParameter()
                {
                    ZBLDH = uploadTransaction.BillNo,
                    ZDJLX = "SO",
                    BWART = "ZZZ",
                    ZBLDHXM = uploadTransaction.BillLineNo,
                    MATNR = uploadTransaction.ItemCode,
                    MAKTX = uploadTransaction.ItemName,
                    ZBDMNG = uploadTransaction.Quantity.ToString(),
                    MEINS = uploadTransaction.UnitName,
                    ABLAD = "",
                    UploadTransactionId = uploadTransaction.Id
                };
                sapParameters.Add(sapParameter);
            };

            var sapResult = this.SapUploadSaleOut(sapParameters);

            //上传事务记录
            var transLog = new UploadTransactionLog();
            transLog.GenerateId();

            var paraDtlDic = sapParameters.ToDictionary(p => p.UploadTransactionId);
            Guid guid = Guid.NewGuid();
            ProcessResult result = new ProcessResult();
            ctl.SaveTransactionData(sapResult, uploadTransactions.ToList(), result, guid.ToString());
            return result;
        }

        /// <summary>
        /// 过账接口
        /// </summary>
        /// <param name="sapParameters"></param>
        /// <returns></returns>
        private SapResult SapUploadSaleOut(List<SapParameter> sapParameters)
        {
            if (sapParameters.Count == 0)
                return new SapResult();

            //构建查询参数
            var paras = new SapParameterBase<SapParameter>();
            paras.IT_DATA = sapParameters.ToArray();

            //执行调用SAP
            var result = RfcHelper.Sap<SapResultErr<SapMoreErrorData>, SapParameterBase<SapParameter>>("ZFM_MES_POSTING", paras);
            return result;
        }
    }
}
