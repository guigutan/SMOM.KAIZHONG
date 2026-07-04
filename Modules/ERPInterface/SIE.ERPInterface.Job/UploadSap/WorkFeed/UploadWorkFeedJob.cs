using SIE.Common.Schdules;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Ebs.Upload;
using SIE.ERPInterface.Job.Common;
using SIE.Inventory.Commom;
using SIE.Inventory.Transactions;
using SIE.Threading;
using System;
using System.Collections.Generic;
using SIE.Core.Enums;
using SIE.ERPInterface.Sap.Upload.SupplierReturn;
using SIE.ERPInterface.Sap.Upload.WorkFeed;
using SIE.ERPInterface.Sap.Upload.OutPurchase;
using SIE.ERPInterface.Common.ERPJobCloseRules;

namespace SIE.ERPInterface.Job.Upload
{
    /// <summary>
    /// 工单发料事务上传
    /// </summary>
    [Job("工单发料事务上传", typeof(ULCommonParameter))]
    public class SapUploadWorkFeedJob : JobBase
    {
        /// <summary>
        /// 调度执行主键
        /// </summary>
        public static string _jobKey = "工单发料事务上传-" + typeof(UploadWorkFeedJob).FullName + "!" + RT.InvOrg;

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
                var transactions = RT.Service.Resolve<TransactionController>().GetTransactions(OrderType.WorkFeed);
                foreach (var item in transactions)
                {
                    tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.WorkFeed, TransactionType.OutStorage, item.Id, ""));
                }
                //tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.WorkFeed, TransactionType.OutStorage, null, ""));
                //没有配置规则，默认事务定义
                RT.Service.Resolve<UploadBaseController>().UploadInvTransactionsToInf(tuples);                              //捉取事务交易数据到事务上传表
            }
            AddLog("结束上传中间表。".L10N());

            if (tuples.Count > 0)
            { //单据大类，事务类型，单据小类，单据号
                var uploadTuples = new List<Tuple<OrderType, TransactionType, double?, string>>();
                foreach (var tuple in tuples)
                {
                    uploadTuples.Add(new Tuple<OrderType, TransactionType, double?, string>(tuple.Item1, tuple.Item2, tuple.Item3, string.Empty));
                }

                var result = RT.Service.Resolve<HttpWorkFeedController>().UploadWorkFeedToErp(uploadTuples);                      //上传ERP
                AddLog("结束上传ERP。{0}".L10nFormat(result.Msg));
            }
        }
    }
}
