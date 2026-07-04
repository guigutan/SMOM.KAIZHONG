using SIE.Common.Schdules;
using SIE.Core.Enums;
using SIE.DataPortal;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Ebs.UploadEbs.AllocateIn;
using SIE.ERPInterface.Ebs.UploadEbs.CrossAllocate;
using SIE.ERPInterface.Job.Common;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Job.Upload.CrossAllocate
{
    /// <summary>
    /// 跨组织调拨(出库)事务上传EBS
    /// </summary>
    [Job("跨组织调拨事务(出库)上传EBS", typeof(JobParameter))]
    public class UploadCrossAllocateOutJob : JobBase
    {
        /// <summary>
        /// 执行调度
        /// </summary>
        /// <param name="context">参数</param>
        protected override void ExecuteJob(JobContext context)
        {
            AppRuntime.InvOrg = context.InvOrg;
            AppRuntime.Principal = new DataPortalPrincipal(context.IdentityId, 0.0, "");
            AppRuntime.InvOrg = context.InvOrg;
            JobParameter jobParameter = Activator.CreateInstance(Type.GetType(context.JobClass).GetCustomAttribute<JobAttribute>()?.ParameterType ?? typeof(JobParameter)) as JobParameter;
            jobParameter?.Initialize(context.Parameter);
            var p = jobParameter as ULCommonParameter;
            //var tuples = new List<Tuple<OrderType, TransactionType>>();
            var allocateOutTuples = new List<Tuple<OrderType, TransactionType, double?, string>>();
            var allocateInTuples = new List<Tuple<OrderType, TransactionType, double?, string>>();
            if (p?.UploadTransactionRule != null)
            {
                AppRuntime.InvOrg = context.InvOrg;
                allocateOutTuples = RT.Service.Resolve<UploadBaseController>().UploadInvTransactionsToInf(p.UploadTransactionRule);           //捉取事务交易数据到事务上传表
            }
            else
            {
                AppRuntime.InvOrg = context.InvOrg;
                //出库  事务类型为出库
                allocateOutTuples.Add(new Tuple<OrderType, TransactionType, double?, string>(OrderType.OtherOut, TransactionType.OutStorage, null, ""));
                RT.Service.Resolve<UploadBaseController>().UploadInvTransactionsToInf(allocateOutTuples);//捉取事务交易数据到事务上传表
                AddLog("跨组织调拨出库(直接调拨，两步调拨)结束上传中间表。".L10N());
            }
            AppRuntime.InvOrg = context.InvOrg;
            //出库
            var resultSmom = RT.Service.Resolve<EbsCrossAllocateController>().UploadToEbs(allocateOutTuples, 1);
            AddLog("跨组织调拨出库(跨组织调拨)结束上传ERP。{0}".L10nFormat(resultSmom == null ? "。" : "，" + resultSmom.Msg));
        }

        protected override void ExecuteJob(object param)
        {

        }
    }
}
