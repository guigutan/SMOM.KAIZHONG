using SIE.Common.Schdules;
using SIE.Core.Enums;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Job.Common;
using SIE.ERPInterface.Sap.Upload.OtherOut;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;

namespace SIE.ERPInterface.Job.Upload.OtherOut
{
    /// <summary>
    /// 其他出库上传调度
    /// </summary>
    [Job("其他出库上传调度", typeof(ULCommonParameter))]
    public class UploadOtherOutStorageJob : JobBase
    {
        /// <summary>
        /// 调度执行主键
        /// </summary>
        public static string _jobKey = "其他出库-" + typeof(UploadOtherOutStorageJob).FullName + "!" + AppRuntime.InvOrg;

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
                tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.OtherOut, TransactionType.OutStorage, null, string.Empty));
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
                var result = AppRuntime.Service.Resolve<HttpSapOtherOutController>().UploadOutStorageToErp(uploadTuples);
                AddLog("结束上传ERP。{0}".L10nFormat(result.Msg));
            }

        }
    }
}
