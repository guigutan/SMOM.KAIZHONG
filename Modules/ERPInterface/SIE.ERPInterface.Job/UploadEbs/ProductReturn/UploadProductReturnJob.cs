using SIE.Common.Schdules;
using SIE.Core.Enums;
using SIE.DataPortal;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.ERPJobCloseRules;
using SIE.ERPInterface.Ebs.UploadEbs.ProductReturn;
using SIE.ERPInterface.Ebs.UploadEbs.WorkFeed;
using SIE.ERPInterface.Job.Common;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Job.Upload.ProductReturn
{
    /// <summary>
    /// 生产退料事务上传
    /// </summary>
    [Job("生产退料事务上传EBS", typeof(JobParameter))]
    public class UploadProductReturnJob : JobBase
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
            {
                AppRuntime.InvOrg = context.InvOrg;
                tuples = RT.Service.Resolve<UploadBaseController>().UploadInvTransactionsToInf(p.UploadTransactionRule);           //捉取事务交易数据到事务上传表
            }
            else
            {
                AppRuntime.InvOrg = context.InvOrg;
                tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.MaterialReturn, TransactionType.InStorage, null, ""));       //没有配置规则，默认事务定义
                tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.MaterialReturn, TransactionType.RecInStorage, null, ""));
                RT.Service.Resolve<UploadBaseController>().UploadInvTransactionsToInf(tuples);                          //捉取事务交易数据到事务上传表
                AddLog("生产退料结束上传中间表。".L10N());
            }
            AppRuntime.InvOrg = context.InvOrg;
            var resultSmom = RT.Service.Resolve<EbsProductReturnController>().UploadToEbs(tuples);
            AddLog("生产退料结束上传ERP。{0}".L10nFormat(resultSmom == null ? "。" : "，" + resultSmom.Msg));
        }

        protected override void ExecuteJob(object param)
        {

        }
    }
}
