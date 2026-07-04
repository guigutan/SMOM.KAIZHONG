using SIE.Common.Schdules;
using SIE.Core.Enums;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Job.Common;
using SIE.ERPInterface.Sap.Upload.SaleReturn;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;

namespace SIE.ERPInterface.Job.Upload.SaleReturn
{

    /// <summary>
    /// 销售退货上传（冲销）调度
    /// </summary>
    [Job("销售退货上传（冲销）调度", typeof(ULCommonParameter))]
    public class UploadSaleReturnWriteOffJob : JobBase
    {
        /// <summary>
        /// 调度执行主键
        /// </summary>
        public static string _jobKey = "销售退货（冲销）-" + typeof(UploadSaleReturnWriteOffJob).FullName + "!" + AppRuntime.InvOrg;

        /// <summary>
        /// 执行调度
        /// </summary>
        /// <param name="param">参数</param>
        protected override void ExecuteJob(object param)
        {
            var p = param as ULCommonParameter;
            //单据大类，事务类型，单据小类，单据号
            var tuples = new List<Tuple<OrderType, TransactionType, double?, string>>();

            if (p?.UploadTransactionRule != null)
                //捉取事务交易数据到事务上传表
                tuples = AppRuntime.Service.Resolve<UploadBaseController>().UploadInvTransactionsToInf(p.UploadTransactionRule);
            else
            {
                //没有配置规则，默认事务定义
                //var transaction = AppRuntime.Service.Resolve<TransactionController>().GetTransaction("销售出库");
                //if (transaction == null)
                //    throw new ValidationException("未找到[销售出库]的单据小类，请维护。".L10N());

                //tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.SaleOut, TransactionType.MesMaterialDown, transaction.Id, string.Empty));
                //tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.SaleOut, TransactionType.RecInStorage, transaction.Id, string.Empty));
                var transactions = RT.Service.Resolve<TransactionController>().GetTransactions(OrderType.SaleOut);
                foreach (var item in transactions)
                {
                    tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.SaleOut, TransactionType.UnOutStorage, item.Id, string.Empty));
                }
                //捉取事务交易数据到事务上传表
                AppRuntime.Service.Resolve<UploadBaseController>().UploadInvTransactionsToInf(tuples);
            }
            AddLog("结束上传中间表。".L10N());

            if (tuples.Count > 0)
            {
                //单据大类，事务类型，单据小类，单据号
                var uploadTuples = new List<Tuple<OrderType, TransactionType, double?, string>>();
                foreach (var tuple in tuples)
                {
                    uploadTuples.Add(new Tuple<OrderType, TransactionType, double?, string>(tuple.Item1, tuple.Item2, tuple.Item3, string.Empty));
                }
                //上传ERP
                var result = AppRuntime.Service.Resolve<HttpSapSaleReturnController>().UploadSaleReturnWriteOffToErp(uploadTuples);
                AddLog("结束上传ERP。{0}".L10nFormat(result.Msg));
            }

        }
    }
}
