using SIE.Domain.Validation;
using SIE.Equipments.WorkFlows;
using SIE.Fixtures;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.Fixtures.Demands.Commands
{
    /// <summary>
    /// 审核
    /// </summary>
    [JsCommand("SIE.Web.Fixtures.Demands.Commands.ExamineDemandCommand")]
    public class ExamineDemandCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var info = args.Data.ToJsonObject<ExamineInfo>();
            if (info == null)
                throw new ValidationException("审核信息异常".L10N());
            if (info.ApprovalResult == null)
                throw new ValidationException("审核结果必填".L10N());
            if(info.ApprovalResult == SIE.Equipments.Enums.ApprovalResult.Reject && info.Remark.IsNullOrWhiteSpace())
                throw new ValidationException("审核结果驳回时，审核意见必填".L10N());
            List<double> selectedIds = new List<double>(args.SelectedIds);
            RT.Service.Resolve<CoreFixtureController>().ExamineDemands(selectedIds, info.ApprovalResult.Value, info.Remark);
            return true;
        }
    }
}
