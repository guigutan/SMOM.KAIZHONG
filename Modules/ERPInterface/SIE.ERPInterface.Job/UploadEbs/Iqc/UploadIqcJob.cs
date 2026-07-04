using SIE.Common.Schdules;
using SIE.Core.Enums;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.ERPJobCloseRules;
using SIE.ERPInterface.Job.Common;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;

namespace SIE.ERPInterface.Job.Upload
{
    /// <summary>
    /// 来料检验事务上传
    /// </summary>
    [Job("来料检验事务上传", typeof(JobParameter))]
    public class UploadIqcJob : JobBase
    {
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
            var tuples = new List<Tuple<OrderType, TransactionType, double?, string>>();

            if (p?.UploadTransactionRule != null)
                tuples = RT.Service.Resolve<UploadBaseController>().UploadInvTransactionsToInf(p.UploadTransactionRule);           //捉取事务交易数据到事务上传表
            else
            {
                tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.PurchaseIn, TransactionType.IqcQualified, null, ""));
                tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.PurchaseIn, TransactionType.IqcUnQualified, null, ""));       //没有配置规则，默认事务定义
                RT.Service.Resolve<UploadBaseController>().UploadInvTransactionsToInf(tuples);                          //捉取事务交易数据到事务上传表
            }
            AddLog("结束上传中间表。".L10N());
        }
    }
}
