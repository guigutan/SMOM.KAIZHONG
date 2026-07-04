using SIE.Domain.Validation;
using SIE.EMS.EarlierStage.Budgets;
using SIE.Equipments.Enums;
using SIE.Equipments.WorkFlows;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.EarlierStage.Budgets.Commands
{
    /// <summary>
    /// 审核预算
    /// </summary>
    [JsCommand("SIE.Web.EMS.EarlierStage.Budgets.Commands.ExamineBudgetCommand")]
    public class ExamineBudgetCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ValidationException("审核预算数据参数不能为空".L10N());
            }
            var info = args.Data.ToJsonObject<ExamineInfo>();
            if (info == null)
            {
                throw new ValidationException("审核预算信息异常".L10N());
            }
            if (info.ApprovalResult == null)
            {
                throw new ValidationException("审核结果不能为空".L10N());
            }
            //审核状态为通过,且审批意见没有值则默认备注为通过
            if (info.ApprovalResult == ApprovalResult.Pass && !info.Remark.IsNotEmpty())
            {
                info.Remark = "通过".L10N();
            }
            // //审核状态不为通过时,审批意见必填
            if (info.ApprovalResult != ApprovalResult.Pass && !info.Remark.IsNotEmpty())
            {
                throw new ValidationException("驳回时审核意见不能为空".L10N());
            }
            List<double> selectedIds = new List<double>(args.SelectedIds);
            RT.Service.Resolve<BudgetController>().ExamineBudget(selectedIds, info.ApprovalResult.Value, info.Remark);
            return true;
        }
    }
}
