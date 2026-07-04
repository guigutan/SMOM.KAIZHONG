using SIE.Common.Schdules;
using SIE.Core.Enums;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Job.Common;
using SIE.ERPInterface.Sap.Upload.Finished;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Job.Upload.Finished
{
    /// <summary>
    /// 成品入库上传调度
    /// </summary>
    [Job("成品入库上传调度", typeof(ULCommonParameter))]
    public class SapUploadFinishedInStorageJob : JobBase
    {
        /// <summary>
        /// 调度执行主键
        /// </summary>
        public static string _jobKey = "成品入库-" + typeof(SapUploadFinishedInStorageJob).FullName + "!" + AppRuntime.InvOrg;

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
                List<string> listCodes = new List<string>();
                listCodes.Add("成品入库");
                listCodes.Add("半成品入库");
                var trantions = RT.Service.Resolve<TransactionController>().GetTransactionByCodes(listCodes);
                var transaction1 = trantions.FirstOrDefault(a => a.Code == "成品入库");
                if (transaction1 == null)
                    throw new ValidationException("未找到[成品入库]的单据小类，请维护。".L10N());
                var transaction2 = trantions.FirstOrDefault(a => a.Code == "半成品入库");
                if (transaction2 == null)
                    throw new ValidationException("未找到[半成品入库]的单据小类，请维护。".L10N());

                ////单据大类：成品入库 或 半成品入库，单据小类：成品入库 或 半成品入库 ，交易类型：接收入库 或 入库；
                tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.Finished, TransactionType.InStorage, transaction1.Id, string.Empty));
                tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.Finished, TransactionType.InStorage, transaction2.Id, string.Empty));
                tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.Finished, TransactionType.RecInStorage, transaction1.Id, string.Empty));
                tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.Finished, TransactionType.RecInStorage, transaction2.Id, string.Empty));
                tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.PartedIn, TransactionType.InStorage, transaction1.Id, string.Empty));
                tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.PartedIn, TransactionType.InStorage, transaction2.Id, string.Empty));
                tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.PartedIn, TransactionType.RecInStorage, transaction1.Id, string.Empty));
                tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.PartedIn, TransactionType.RecInStorage, transaction2.Id, string.Empty));


                //List<OrderType> orderTypes = new List<OrderType>() { OrderType.Finished,OrderType.PartedIn};
                //var Finishedtransations = RT.Service.Resolve<TransactionController>().GetTransactions(OrderType.Finished);
                //var PartIntransations = RT.Service.Resolve<TransactionController>().GetTransactions(OrderType.PartedIn);
                //foreach (var transaction in Finishedtransations)
                //{
                //    tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.Finished, TransactionType.InStorage, transaction.Id, string.Empty));
                //    tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.Finished, TransactionType.RecInStorage, transaction.Id, string.Empty));
                //}
                //foreach (var transaction in PartIntransations)
                //{
                //    tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.PartedIn, TransactionType.InStorage, transaction.Id, string.Empty));
                //    tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.PartedIn, TransactionType.RecInStorage, transaction.Id, string.Empty));
                //}
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
                var result = AppRuntime.Service.Resolve<HttpSapFinishedController>().UploadInStorageToErp(uploadTuples);
                AddLog("结束上传ERP。{0}".L10nFormat(result.Msg));
            }

        }
    }
}
