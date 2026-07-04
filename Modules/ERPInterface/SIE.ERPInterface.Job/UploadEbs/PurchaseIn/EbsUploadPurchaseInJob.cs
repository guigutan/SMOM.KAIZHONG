using SIE.Common.Schdules;
using SIE.Core.Enums;
using SIE.DataPortal;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.ERPJobCloseRules;
using SIE.ERPInterface.Ebs.UploadEbs.PurchaseIn;
using SIE.ERPInterface.Job.Common;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SIE.ERPInterface.Job.Upload.PurchaseIn
{
    /// <summary>
    /// 采购入库上架事务上传
    /// </summary>
    [Job("来料暂收、采购入库上架事务上传EBS", typeof(JobParameter))]
    public class EbsUploadPurchaseInJob : JobBase
    {
        /// <summary>
        /// 执行调度
        /// </summary>
        /// <param name="context">参数</param>
        protected override void ExecuteJob(JobContext context)
        {
            if (RT.Service.Resolve<ErpJobCloseRuleController>().ValidateInCloseTime())
            {
                return;
            }
            AppRuntime.InvOrg = context.InvOrg;
            AppRuntime.Principal = new DataPortalPrincipal(context.IdentityId, 0.0, "");
            AppRuntime.InvOrg = context.InvOrg;
            JobParameter jobParameter = Activator.CreateInstance(Type.GetType(context.JobClass).GetCustomAttribute<JobAttribute>()?.ParameterType ?? typeof(JobParameter)) as JobParameter;
            jobParameter?.Initialize(context.Parameter);
            var p = jobParameter as ULCommonParameter;
            var tuples = new List<Tuple<OrderType, TransactionType, double?, string>>();
            if (p?.UploadTransactionRule != null)
                tuples = RT.Service.Resolve<UploadBaseController>().UploadInvTransactionsToInf(p.UploadTransactionRule);           //捉取事务交易数据到事务上传表
            else
            {
                tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.PurchaseIn, TransactionType.Receive, null, ""));
                tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.PurchaseIn, TransactionType.InStorage, null, ""));       //没有配置规则，默认事务定义
                tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.PurchaseIn, TransactionType.RecInStorage, null, ""));       //没有配置规则，默认事务定义
                AppRuntime.InvOrg = context.InvOrg;
                RT.Service.Resolve<UploadBaseController>().UploadInvTransactionsToInf(tuples);                          //捉取事务交易数据到事务上传表
                AddLog("来料暂收、采购入库结束上传中间表。".L10N());
            }

            AppRuntime.InvOrg = context.InvOrg;
            var resultSmom = RT.Service.Resolve<EbsInComeController>().UploadToEbs(tuples);
            AddLog("来料暂收结束上传ERP。{0}".L10nFormat(resultSmom == null ? "。" : "，" + resultSmom.Msg));

            AppRuntime.InvOrg = context.InvOrg;
            resultSmom = RT.Service.Resolve<EbsPurchaseInController>().UploadToEbs(tuples);         //执行数据上传ERP
            AddLog("采购入库结束上传ERP。{0}".L10nFormat(resultSmom == null ? "。" : "，" + resultSmom.Msg));
        }

        protected override void ExecuteJob(object param)
        {

        }
    }
}
