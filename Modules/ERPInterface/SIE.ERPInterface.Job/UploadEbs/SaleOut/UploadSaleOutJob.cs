using SIE.Common.Schdules;
using SIE.Core.Enums;
using SIE.DataPortal;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.ERPJobCloseRules;
using SIE.ERPInterface.Ebs.Download.Items;
using SIE.ERPInterface.Ebs.Upload;
using SIE.ERPInterface.Job.Common;
using SIE.ERPInterface.Smom.Download;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SIE.ERPInterface.Job.Upload
{
    /// <summary>
    /// 销售出库事务上传
    /// </summary>
    [Job("销售出库事务上传EBS", typeof(JobParameter))]
    public class UploadSaleOutJob : JobBase
    {
        //
        // 摘要:
        //     执行任务
        //
        // 参数:
        //   context:
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
            {
                AppRuntime.InvOrg = context.InvOrg;
                tuples = RT.Service.Resolve<UploadBaseController>().UploadInvTransactionsToInf(p.UploadTransactionRule);
            }      //捉取事务交易数据到事务上传表
            else
            {
                AppRuntime.InvOrg = context.InvOrg;
                tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.SaleOut, TransactionType.OutStorage, null, ""));       //没有配置规则，默认事务定义
                RT.Service.Resolve<UploadBaseController>().UploadInvTransactionsToInf(tuples);                          //捉取事务交易数据到事务上传表
                AddLog("销售出库结束上传中间表。".L10N());
            }
            //Copy必改内容
            AppRuntime.InvOrg = context.InvOrg;
            var resultSmom = RT.Service.Resolve<EbsSaleOutController>().UploadToEbs(tuples);         //执行数据上传ERP
            AddLog("销售出库结束上传ERP。{0}".L10nFormat(resultSmom == null ? "。" : "，" + resultSmom.Msg));
        }

        protected override void ExecuteJob(object param)
        {

        }
    }
}
