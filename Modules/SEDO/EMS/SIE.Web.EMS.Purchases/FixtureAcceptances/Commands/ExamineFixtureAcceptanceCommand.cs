using SIE.Domain.Validation;
using SIE.EMS.Purchases.FixtureAcceptances;
using SIE.Equipments.Enums;
using SIE.Equipments.WorkFlows;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.FixtureAcceptances.Commands
{
    /// <summary>
    /// 审核设备验收
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.FixtureAcceptances.Commands.ExamineFixtureAcceptanceCommand")]
    public class ExamineFixtureAcceptanceCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args == null)
            {
                return false;
            }
            var info = args.Data.ToJsonObject<ExamineInfo>();
            if (info == null)
            {
                throw new ValidationException("审核信息异常".L10N());
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
            RT.Service.Resolve<FixtureAcceptancesController>().ExamineFixtureAcceptancesAccept(selectedIds, info.ApprovalResult.Value, info.Remark);
            return true;
        }
    }
}
