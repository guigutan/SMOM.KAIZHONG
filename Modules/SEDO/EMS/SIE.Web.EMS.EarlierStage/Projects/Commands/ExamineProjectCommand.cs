using SIE.Domain.Validation;
using SIE.Equipments.Enums;
using SIE.EMS.EarlierStage.Projects;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using SIE.Equipments.WorkFlows;

namespace SIE.Web.EMS.EarlierStage.Projects.Commands
{
    /// <summary>
    /// 审核项目
    /// </summary>
    [JsCommand("SIE.Web.EMS.EarlierStage.Projects.Commands.ExamineProjectCommand")]
    public class ExamineProjectCommand : ViewCommand
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
            {
                throw new ValidationException("审核项目信息异常".L10N());
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
            if (info.ApprovalResult.Value == ApprovalResult.Pass)
            {
                RT.Service.Resolve<ProjectController>().ExaminePassProject(selectedIds, info.Remark);
            }
            else
            {
                RT.Service.Resolve<ProjectController>().CancelProject(selectedIds, ApprovalStatus.Reject, ApprovalResult.Reject, info.Remark);
            }
            return true;
        }
    }
}
