using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Checks;
using SIE.EMS.Checks.ApiModels;
using SIE.EMS.Checks.Plans;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.Checks.Confirmations.Commands
{
    /// <summary>
    /// 保存：点检确认
    /// </summary>
    [JsCommand("SIE.Web.EMS.Checks.Confirmations.Commands.SubmitCheckConfirmationCommand")]
    public class SubmitCheckConfirmationCommand : FormSaveCommand
    {
        // 点检确认不用这段逻辑
        ///// <summary>
        ///// 保存
        ///// </summary>
        ///// <param name="entity">实体</param>
        //protected override void DoSave(Entity entity)
        //{
        //    if (entity == null) { throw new ValidationException("没有数据可以提交。".L10N()); }

        //    var bill = entity as CheckPlan;

        //    if (bill == null)
        //    {
        //        throw new ValidationException("该数据不是点检计划数据格式。".L10N());
        //    }

        //    var list = new List<CheckConfirmationSubmitInfo>();

        //    foreach (var item in bill.CheckConfirmationList)
        //    {
        //        if ((int)item.Score == 0)
        //        {
        //            throw new ValidationException(item.ProjectName + " 项目未评分，请评分!".L10N());
        //        }
        //        list.Add(new CheckConfirmationSubmitInfo()
        //        {
        //            CheckPlanId = item.OwnerId,
        //            ConfirmDeptId = item.ConfirmDeptId,
        //            TpmScoreProjectId = item.TpmScoreProjectId,
        //            ConfirmResult = (int)bill.ConfirmResult,
        //            ConfirmNote = bill.ConfirmNote,
        //            Score = (int)item.Score,
        //            FileName = item.FileName,
        //            FilePath = item.FilePath,
        //            FileExtesion = item.FileExtesion,
        //            FileSize = item.FileSize,
        //            Content = item.Content == null ? null : System.Text.ASCIIEncoding.UTF8.GetString(item.Content)
        //        });
        //    }

        //    RT.Service.Resolve<CheckController>().SubmitCheckConfirmation(list.ToArray());
        //}
    }
}
