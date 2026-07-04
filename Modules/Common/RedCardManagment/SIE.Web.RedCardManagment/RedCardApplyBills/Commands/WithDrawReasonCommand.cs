using SIE.RedCardManagment.RedCardApplyBills.Service;
using SIE.RedCardManagment.WorkFlow;
using SIE.Web.Command;
using SIE.Web.RedCardManagment.RedCardApplyBills.ViewModels;
using System;

namespace SIE.Web.RedCardManagment.RedCardApplyBills.Commands
{
    /// <summary>
    /// 撤回命令
    /// </summary>
    public class WithDrawReasonCommand : ViewCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">SIE.MetaModel.View.Block.EntityType</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<WithDrawReasonViewModel>();
            if (null == data)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(data)));
            }

            string reason = data.Reason;
            if (reason.IsNullOrWhiteSpace())
                throw new SIE.Domain.Validation.ValidationException("撤回失败：[撤回原因]不可为空或超出字符".L10N());
            if (reason.Length > 3000)
            {
                throw new SIE.Domain.Validation.ValidationException("撤回失败：[撤回原因]超出字符数[3000]".L10N());
            }

            RT.Service.Resolve<RedCardApplyBillWorkflowController>().WithDrawWorkflow(data.RedCardApplyBillId, reason);
            return true;
        }
    }
}
