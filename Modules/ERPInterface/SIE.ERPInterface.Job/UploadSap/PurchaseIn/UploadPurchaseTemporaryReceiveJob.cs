using SIE.Common.Schdules;
using SIE.Core.Enums;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Job.Common;
using SIE.ERPInterface.Sap.Upload.PurchaseIn;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Job.Upload.PurchaseIn
{
    /// <summary>
    /// 采购送货单暂收上传调度
    /// </summary>
    [Job("采购送货单暂收上传调度", typeof(ULCommonParameter))]
    public class UploadPurchaseTemporaryReceiveJob : JobBase
    {
        /// <summary>
        /// 调度执行主键
        /// </summary>
        public static string _jobKey = "采购送货单暂收-" + typeof(UploadPurchaseTemporaryReceiveJob).FullName + "!" + RT.InvOrg;

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
                tuples = RT.Service.Resolve<UploadBaseController>().UploadInvTransactionsToInf(p.UploadTransactionRule);
            else
            {
                //没有配置规则，默认事务定义
                var transaction = RT.Service.Resolve<TransactionController>().GetTransactionByCodes(new List<string>() { "采购入库" });
                if (transaction == null||!transaction.Any())
                    throw new ValidationException("未找到[采购入库]的单据小类，请维护。".L10N());
                tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.PurchaseIn, TransactionType.Receive, transaction.FirstOrDefault().Id, string.Empty));
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
                    uploadTuples.Add(new Tuple<OrderType, TransactionType, double?, string>(tuple.Item1, tuple.Item2, tuple.Item3, string.Empty));
                }
                //上传ERP
                var result = RT.Service.Resolve<HttpSapPurchaseInController>().UploadTemporaryReceiveToErp(uploadTuples);
                AddLog("结束上传ERP。{0}".L10nFormat(result.Msg));
            }

        }
    }
}
