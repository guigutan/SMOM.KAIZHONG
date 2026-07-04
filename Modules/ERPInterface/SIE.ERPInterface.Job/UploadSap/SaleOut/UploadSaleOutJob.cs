using SIE.Common.Schdules;
using SIE.Core.Enums;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.ERPJobCloseRules;
using SIE.ERPInterface.Job.Common;
using SIE.ERPInterface.Sap.Upload.SaleOut;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;

namespace SIE.ERPInterface.Job.Upload
{
    /// <summary>
    /// 销售出库事务上传
    /// </summary>
    [Job("销售出库事务上传", typeof(ULCommonParameter))]
    public class UploadSapSaleOutJob : JobBase
    {
        /// <summary>
        /// 调度执行主键
        /// </summary>
        public static string _jobKey = "销售出库-" + typeof(UploadSaleOutJob).FullName + "!" + RT.InvOrg;

        /// <summary>
        /// 执行调度
        /// </summary>
        /// <param name="param">参数</param>
        protected override void ExecuteJob(object param)
        {
            if (RT.Service.Resolve<ErpJobCloseRuleController>().ValidateInCloseTime())
            {
                return;
            }
            var p = param as ULCommonParameter;
            //单据大类，事务类型，单据小类，单据号
            var tuples = new List<Tuple<OrderType, TransactionType, double?, string>>();


            if (p?.UploadTransactionRule != null)
                tuples = RT.Service.Resolve<UploadBaseController>().UploadInvTransactionsToInf(p.UploadTransactionRule);    //捉取事务交易数据到事务上传表
            else
            {
                var transactions = RT.Service.Resolve<TransactionController>().GetTransactions(OrderType.SaleOut);
                //没有配置规则，默认事务定义
                foreach (var item in transactions)
                {
                    tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.SaleOut, TransactionType.OutStorage, item.Id, ""));
                }  
                //捉取事务交易数据到事务上传表
                RT.Service.Resolve<UploadBaseController>().UploadInvTransactionsToInf(tuples);                            
            }
            AddLog("结束上传中间表。".L10N());

            if (tuples.Count > 0)
            {
                //单据大类，事务类型，单据小类，单据号
                var uploadTuples = new List<Tuple<OrderType, TransactionType, double?, string>>();
                foreach (var tuple in tuples)
                {
                    uploadTuples.Add(new Tuple<OrderType, TransactionType, double?, string>(tuple.Item1, tuple.Item2, null, string.Empty));
                }
                //上传ERP
                var result = RT.Service.Resolve<HttpSapSaleOutController>().UploadSaleOutToErp(uploadTuples);                     
                AddLog("结束上传ERP。{0}".L10nFormat(result.Msg));
            }

        }
    }
}
