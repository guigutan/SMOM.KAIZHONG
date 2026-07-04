using SIE.Domain.Validation;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipmentCards;
using SIE.Equipments.WorkFlows;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.Equipments.EquipmentCards.Commands
{
    /// <summary>
    /// 审核
    /// </summary>
    public class AuditEquipCardCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args != null)
            {
                var info = args.Data.ToJsonObject<ExamineInfo>();
                if (null == args.SelectedIds || args.SelectedIds.Length == 0)
                {
                    throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
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
                List<double> cardIds = new List<double>();
                for (int i = 0; i < args.SelectedIds.Length; i++)
                {
                    cardIds.Add(args.SelectedIds[i]);
                }

                RT.Service.Resolve<EquipmentCardController>().AuditEquipCard(cardIds, info.ApprovalResult.Value, info.Remark);

                return true;
            }
            return "审核成功!";
        }
    }
}
